using UnityEngine;
using System.Collections;

namespace UnityEngine.UI
{
    [System.Serializable]
    public class LoopScrollPrefabSource 
    {
        public string prefabName;
        public int poolSize = 5;

        private bool inited = false;
        public virtual GameObject GetObject()
        {
            if(!inited)
            {
                SG.LsrResManager.Instance.InitPool(prefabName, poolSize);
                inited = true;
            }
            return SG.LsrResManager.Instance.GetObjectFromPool(prefabName);
        }

        public virtual void ReturnObject(Transform go)
        {
            go.SendMessage("ScrollCellReturn", SendMessageOptions.DontRequireReceiver);
            SG.LsrResManager.Instance.ReturnObjectToPool(go.gameObject);
        }
    }
}

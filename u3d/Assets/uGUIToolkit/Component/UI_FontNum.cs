
using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

//  UI_FontNum.cs
//  Author: Lu Zexi
//  2015-03-31


//font num
[AddComponentMenu("UI/UI FontNum")]
public class UI_FontNum : MonoBehaviour
{
    public Sprite[] Num = new Sprite[10];
    public int interval = 2;
    public int Number = 9999;
    public Vector2 Size;

    private List<GameObject> m_lstObj = new List<GameObject>();

    void Awake()
    {
        Setup(Number);
    }

    public void Setup( int num )
    {
        List<Transform> childs = new List<Transform> ();
        foreach (Transform child in gameObject.transform)
        {
            childs.Add (child);
        }
        for (int i = 0; i < childs.Count; i++)
        {
            Destroy(childs[i].gameObject);
        }
        m_lstObj.Clear();
        List<int> lst = new List<int>();
        do
        {
            int tmp_num = num%10;
            num = num/10;
            lst.Add(tmp_num);
        }while(num>0);

        lst.Reverse();

        float x = 0;
        for(int i = 0 ; i<this.m_lstObj.Count ; i++)
        {
            this.m_lstObj[i].SetActive(false);
        }

        //
        for(int i=0 ; i<lst.Count ; i++)
        {
            GameObject obj = null;
            Image tmpImg = null;
            if(i<m_lstObj.Count)
            {
                obj = m_lstObj[i];
                obj.SetActive(true);
                tmpImg = obj.GetComponent<Image>();
            }
            else
            {
                obj = new GameObject("num"+lst[i]);
                obj.layer = 5;
                obj.transform.SetParent(this.transform);
                obj.transform.localScale = Vector3.one;
                obj.transform.localPosition = Vector3.zero;
                tmpImg = obj.AddComponent<Image>();

                this.m_lstObj.Add(obj);
            }

            tmpImg.sprite = Num[lst[i]];
            // tmpImg.SetNativeSize();
            tmpImg.gameObject.GetComponent<RectTransform>().sizeDelta = Size;

            if(i>0) x+=interval;
            x = x + Size.x;
            tmpImg.transform.localPosition += new Vector3(x,0,0);
        }
    }
}


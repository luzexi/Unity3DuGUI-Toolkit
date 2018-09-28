using System;
using System.Collections.Generic;
using UnityEngine.Serialization;

namespace UnityEngine.UI
{
    [AddComponentMenu("UI/UI_CustomMesh")]
    public class UI_CustomMesh : MaskableGraphic
    {
        private Vector3[] mVertices = null;
        private int[] mTriangles = null;
        private Color mColor;

        public bool HasMesh
        {
            get
            {
                return mVertices != null;
            }
        }

        protected UI_CustomMesh()
        {
            useLegacyMeshGeneration = false;
        }
        
        public void SetMesh(Vector3[] _vertices, int[] _triangles, Color _color)
        {
            mVertices = _vertices;
            mTriangles = _triangles;
            mColor = _color;
            SetVerticesDirty();
        }

        protected override void OnPopulateMesh(VertexHelper vh)
        {
            vh.Clear();
            if(mVertices != null && mTriangles != null)
            {
                for(int i = 0 ; i<mVertices.Length ; i++)
                {
                    vh.AddVert(mVertices[i], mColor, new Vector2(0,0));
                }

                for(int i = 0 ; i<mTriangles.Length ; i+=3)
                {
                    vh.AddTriangle(mTriangles[i], mTriangles[i+1], mTriangles[i+2]);
                }
            }
        }
    }
}

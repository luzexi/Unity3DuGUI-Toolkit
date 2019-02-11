using System;
using System.Collections.Generic;
using UnityEngine.Serialization;

namespace UnityEngine.UI
{
    [AddComponentMenu("UI/UI_CustomMesh")]
    public class UI_CustomMesh : MaskableGraphic
    {
        Mesh mesh;

        public bool HasMesh
        {
            get { return mesh != null; }
        }

        protected UI_CustomMesh()
        {
            useLegacyMeshGeneration = false;
        }

        public void SetMesh(Mesh m)
        {
            mesh = m;
            this.SetVerticesDirty();
            this.SetMaterialDirty();
        }
        public override Texture mainTexture {
            get
            {
                if (material != null && material.mainTexture != null)
                    return material.mainTexture;
                return s_WhiteTexture;
            }
        }

        protected override void OnPopulateMesh(VertexHelper vh)
        {
            vh.Clear();
            if (mesh == null) return;

            Vector3[] vertices  = mesh.vertices;
            int[]     triangles = mesh.triangles;
            Color[]   colors    = mesh.colors;
            Vector2[] uvs       = mesh.uv;

            for (int i = 0; i < vertices.Length; i++)
            {
                vh.AddVert(vertices[i], colors[i], uvs[i]);
            }
            for (int i = 0; i < triangles.Length; i += 3)
            {
                vh.AddTriangle(triangles[i], triangles[i + 1], triangles[i + 2]);
            }
        }
    }
}

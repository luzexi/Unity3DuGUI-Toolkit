using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

[AddComponentMenu("UI/Effects/Gradient Color")]
public class UI_GradientColor : BaseMeshEffect {
	[SerializeField]
	public Color32 color1 = Color.white;
	[SerializeField]
    public Color32 color2 = Color.black;
    [SerializeField]
    public bool horizontal = false;
	
	public override void ModifyMesh(VertexHelper aHelper) {
		if (!IsActive()) {
			return;
		}
		
		List<UIVertex> vertexList = new List<UIVertex>();
		aHelper.GetUIVertexStream(vertexList);
		int count = vertexList.Count;

        if (vertexList.Count != 0)
        {

		    float min = horizontal ? vertexList[0].position.x : vertexList[0].position.y;
            float max = min;

		    for (int i = 1; i < count; i++) {
			    float pos = horizontal ? vertexList[i].position.x : vertexList[i].position.y;
			    if (pos > max) {
				    max = pos;
			    }else if (pos < min) {
				    min = pos;
			    }
		    }

		    float range = max - min;
		    for (int i = 0; i < count; i++) {
			    UIVertex uiVertex = vertexList[i];
			    uiVertex.color = 
                    horizontal ? 
                        Color32.Lerp(color1, color2, (uiVertex.position.x - min) / range) : 
                        Color32.Lerp(color2, color1, (uiVertex.position.y - min) / range);
			    vertexList[i] = uiVertex;
		    }

		    aHelper.Clear();
		    aHelper.AddUIVertexTriangleStream(vertexList);
        }
	}
}
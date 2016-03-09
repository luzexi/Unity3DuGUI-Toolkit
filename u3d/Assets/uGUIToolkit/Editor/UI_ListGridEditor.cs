using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;
using System.Collections;
using System.Collections.Generic;

[CustomEditor(typeof(UI_ListGrid))]
public class ListGridEditor : Editor
{
	public override void OnInspectorGUI()
	{
		UI_ListGrid grid = target as UI_ListGrid;
		grid.LineMaxCount = EditorGUILayout.IntField("Line Count",grid.LineMaxCount);
		grid.Interval = EditorGUILayout.IntField("Interval",grid.Interval);
		grid.Dir = (UI_ListGrid.DIR)EditorGUILayout.EnumPopup("Direction",grid.Dir);

		if(GUILayout.Button("Reset"))
		{
			grid.Refresh();
		}
	}
}




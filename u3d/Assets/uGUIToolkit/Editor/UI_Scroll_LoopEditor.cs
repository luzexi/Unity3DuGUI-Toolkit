using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;
using System.Collections;
using System.Collections.Generic;

[CustomEditor(typeof(UI_Scroll_Loop))]
public class UI_Scroll_LoopEditor : Editor
{
	public override void OnInspectorGUI()
	{
		UI_Scroll_Loop loop = target as UI_Scroll_Loop;
		serializedObject.Update();
		EditorGUILayout.PropertyField(serializedObject.FindProperty("ScrollItems"),true);
		loop.moveType = (UI_Scroll_Loop.Movement)EditorGUILayout.EnumPopup("Movement Type",loop.moveType);
		loop.StartIndex = EditorGUILayout.IntField("Start Index",loop.StartIndex);
		loop.MaxSpeed = EditorGUILayout.FloatField("MaxSpeed",loop.MaxSpeed);
		loop.MinFixSpeed = EditorGUILayout.FloatField("MinFixSpeed",loop.MinFixSpeed);
		loop.Size = EditorGUILayout.IntField("Size",loop.Size);
		loop.LeftCount = EditorGUILayout.IntField("LeftCount",loop.LeftCount);
		loop.RightCount = EditorGUILayout.IntField("RightCount",loop.RightCount);
		serializedObject.ApplyModifiedProperties();
		if(GUILayout.Button("Refresh"))
		{
			loop.Refresh();
		}
	}
}




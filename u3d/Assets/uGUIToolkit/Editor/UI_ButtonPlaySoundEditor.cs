using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;
using System.Collections;
using System.Collections.Generic;

[CustomEditor(typeof(UI_ButtonPlaySound))]
public class UI_ButtonPlaySoundEditor : Editor
{
	public override void OnInspectorGUI()
	{
		serializedObject.Update();
		UI_ButtonPlaySound sound = target as UI_ButtonPlaySound;
		EditorGUILayout.PropertyField(serializedObject.FindProperty("audioClip"));
		// sound.volume = EditorGUILayout.Slider("Volume",sound.volume,0,1);
		// sound.pitch = EditorGUILayout.Slider("Pitch",sound.pitch,0,2);
		serializedObject.ApplyModifiedProperties();
		// grid.Dir = (UI_ListGrid.DIR)EditorGUILayout.EnumPopup("Direction",grid.Dir);

		// if(GUILayout.Button("Refresh"))
		// {
		// 	grid.Refresh();
		// }
	}
}




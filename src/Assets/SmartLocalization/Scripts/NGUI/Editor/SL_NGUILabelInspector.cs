﻿//
// SL_NGUILabelInspector.cs
// 
// Written by Niklas Borglund and Jakob Hillerström
//

//#define SMARTLOC_NGUI //<--- UNCOMMENT THIS FOR NGUI CLASSES

#if SMARTLOC_NGUI
namespace SmartLocalization.Editor
{
	
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(SL_NGUILabel))]
public class SL_NGUILabelInspector : Editor 
{
	private string selectedKey = null;
	
	void Awake()
	{
		SL_NGUILabel textObject = ((SL_NGUILabel)target);
		if(textObject != null)
		{
			selectedKey = textObject.localizedKey;
		}
	}
	
	public override void OnInspectorGUI()
	{
		base.OnInspectorGUI();
		
		selectedKey = LocalizedKeySelector.SelectKeyGUI(selectedKey, true, LocalizedObjectType.STRING);
		
		if(!Application.isPlaying && GUILayout.Button("Use Key", GUILayout.Width(70)))
		{
			SL_NGUILabel textObject = ((SL_NGUILabel)target);
			
			textObject.localizedKey = selectedKey;
		}
	}
}
} //namespace SmartLocalization.Editor
#endif
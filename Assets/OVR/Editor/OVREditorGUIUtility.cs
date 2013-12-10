/************************************************************************************

Filename    :   OVREditorGUIUtility.cs
Content     :   Player controller interface. 
				This script adds extended editor functionality
Created     :   January 17, 2013
Authors     :   Peter Giokaris

Copyright   :   Copyright 2013 Oculus VR, Inc. All Rights reserved.

Use of this software is subject to the terms of the Oculus LLC license
agreement provided at the time of installation or download, or which
otherwise accompanies this software in either electronic or hard copy form.

************************************************************************************/
using UnityEngine;
using UnityEditor;

//-------------------------------------------------------------------------------------
// ***** OVREditorGUIUtility
//
// OVREditorGUIUtility contains a collection of GUI utilities for editor classes.
//
public static class OVREditorGUIUtility
{
	public static void Separator()
	{
		GUI.color = new Color(1, 1, 1, 0.25f);
		GUILayout.Box("", "HorizontalSlider", GUILayout.Height(16));
		GUI.color = Color.white;
	}

}


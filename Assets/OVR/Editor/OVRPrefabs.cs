/************************************************************************************

Filename    :   OVRPrefabs.cs
Content     :   Prefab creation editor interface. 
				This script adds the ability to add OVR prefabs into the scene.
Created     :   February 19, 2013
Authors     :   Peter Giokaris

Copyright   :   Copyright 2013 Oculus VR, Inc. All Rights reserved.

Use of this software is subject to the terms of the Oculus LLC license
agreement provided at the time of installation or download, or which
otherwise accompanies this software in either electronic or hard copy form.

************************************************************************************/
using UnityEngine;
using System.Collections;
using UnityEditor;

//-------------------------------------------------------------------------------------
// ***** OVRPrefabs
//
// OculusPrefabs adds menu items under the Oculus main menu. It allows for quick creation
// of the main Oculus prefabs without having to open the Prefab folder and dragging/dropping
// into the scene.
class OVRPrefabs
{
	[MenuItem ("Oculus/Prefabs/OVRCameraController")]	
	static void CreateOVRCameraController ()
	{
		Object ovrcam = AssetDatabase.LoadAssetAtPath ("Assets/OVR/Prefabs/OVRCameraController.prefab", typeof(UnityEngine.Object));
		PrefabUtility.InstantiatePrefab(ovrcam);
    }	
	
	[MenuItem ("Oculus/Prefabs/OVRPlayerController")]	
	static void CreateOVRPlayerController ()
	{
		Object ovrcam = AssetDatabase.LoadAssetAtPath ("Assets/OVR/Prefabs/OVRPlayerController.prefab", typeof(UnityEngine.Object));
		PrefabUtility.InstantiatePrefab(ovrcam);
    }	
}
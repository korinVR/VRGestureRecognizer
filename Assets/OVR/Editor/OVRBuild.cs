/************************************************************************************

Filename    :   OVRBuild.cs
Content     :   Rift editor interface. 
				This script adds the ability to build demo from within Unity and from
				command line
Created     :   February 14, 2013
Authors     :   Peter Giokaris

Copyright   :   Copyright 2013 Oculus VR, Inc. All Rights reserved.

Use of this software is subject to the terms of the Oculus LLC license
agreement provided at the time of installation or download, or which
otherwise accompanies this software in either electronic or hard copy form.

************************************************************************************/
using UnityEngine;
using UnityEditor;

//-------------------------------------------------------------------------------------
// ***** OculusBuild
//
// OculusBuild adds menu functionality for a user to build the currently selected scene, 
// and to also build and run the standalone build. These menu items can be found under the
// Oculus/Build menu from the main Unity Editor menu bar.
//

class OculusBuild
{
	
#if UNITY_STANDALONE_WIN
	// Build the standalone Windows demo and place into main project folder
	[MenuItem ("Oculus/Build/StandaloneWindows")]	
	static void PerformBuildStandaloneWindows ()
	{
		if(Application.isEditor)
		{
			string[] scenes = { EditorApplication.currentScene };
			BuildPipeline.BuildPlayer(scenes, "Win_OculusUnityDemoScene.exe", BuildTarget.StandaloneWindows, BuildOptions.None);
		}
	}
	
	// Build the standalone Windows demo and place into main project folder, and then run
	[MenuItem ("Oculus/Build/StandaloneWindows - Run")]	
	static void PerformBuildStandaloneWindowsRun ()
	{
		if(Application.isEditor)
		{
			string[] scenes = { EditorApplication.currentScene };
			BuildPipeline.BuildPlayer(scenes, "Win_OculusUnityDemoScene.exe", BuildTarget.StandaloneWindows, BuildOptions.AutoRunPlayer);
		}
		else
		{
			string[] scenes = { "Assets/Tuscany/Scenes/VRDemo_Tuscany.unity" };
			BuildPipeline.BuildPlayer(scenes, "Win_OculusUnityDemoScene.exe", BuildTarget.StandaloneWindows, BuildOptions.AutoRunPlayer);
		}
    }
#endif
	
#if UNITY_STANDALONE_OSX
	// Build the standalone Mac demo and place into main project folder
	[MenuItem ("Oculus/Build/StandaloneMac")]	
	static void PerformBuildStandaloneMac ()
	{
		if(Application.isEditor)
		{
			string[] scenes = { EditorApplication.currentScene };
			BuildPipeline.BuildPlayer(scenes, "Mac_OculusUnityDemoScene.app", BuildTarget.StandaloneOSXIntel, BuildOptions.None);
		}
	}
	
	// Build the standalone Mac demo and place into main project folder, and then run
	[MenuItem ("Oculus/Build/StandaloneMac - Run")]	
	static void PerformBuildStandaloneMacRun ()
	{
		if(Application.isEditor)
		{
			string[] scenes = { EditorApplication.currentScene };
			BuildPipeline.BuildPlayer(scenes, "Mac_OculusUnityDemoScene.app", BuildTarget.StandaloneOSXIntel, BuildOptions.AutoRunPlayer);
		}
		else
		{
			string[] scenes = { "Assets/Tuscany/Scenes/VRDemo_Tuscany.unity" };
			BuildPipeline.BuildPlayer(scenes, "Mac_OculusUnityDemoScene.app", BuildTarget.StandaloneOSXIntel, BuildOptions.AutoRunPlayer);
		}
    }
#endif
	

// Since the editor is not on Linux, we do not need to expose anything in this class
	
	
}

//-------------------------------------------------------------------------------------
// ***** OculusBuildDemo
//
// OculusBuild allows for command line building of the Oculus main demo (Tuscany).
//
class OculusBuildDemo
{
	static void PerformBuildStandaloneWindows ()
	{
		string[] scenes = { "Assets/Tuscany/Scenes/VRDemo_Tuscany.unity" };
		BuildPipeline.BuildPlayer(scenes, "Win_OculusUnityDemoScene.exe", BuildTarget.StandaloneWindows, BuildOptions.None);
    }
	
	static void PerformBuildStandaloneMac ()
	{
		string[] scenes = { "Assets/Tuscany/Scenes/VRDemo_Tuscany.unity" };
		// This may needed due to a full-screen Mac issue; will be addressed in a later release
		// QualitySettings.antiAliasing = 0;
		BuildPipeline.BuildPlayer(scenes, "Mac_OculusUnityDemoScene.app", BuildTarget.StandaloneOSXIntel, BuildOptions.None);
    }
	
	static void PerformBuildStandaloneLinux ()
	{
		string[] scenes = { "Assets/Tuscany/Scenes/VRDemo_Tuscany.unity" };
		BuildPipeline.BuildPlayer(scenes, "Linux_OculusUnityDemoScene", BuildTarget.StandaloneLinux, BuildOptions.None);
    }
	
	static void PerformBuildStandaloneLinux64 ()
	{
		string[] scenes = { "Assets/Tuscany/Scenes/VRDemo_Tuscany.unity" };
		BuildPipeline.BuildPlayer(scenes, "Linux_OculusUnityDemoScene", BuildTarget.StandaloneLinux64, BuildOptions.None);
    }

	//---------------------------------------------
	static void PerformBuildStandaloneWindowsRun ()
	{
		string[] scenes = { "Assets/Tuscany/Scenes/VRDemo_Tuscany.unity" };
		BuildPipeline.BuildPlayer(scenes, "Win_OculusUnityDemoScene.exe", BuildTarget.StandaloneWindows, BuildOptions.AutoRunPlayer);
    }
}
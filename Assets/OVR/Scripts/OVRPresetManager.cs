/************************************************************************************

Filename    :   OVRPresetManager.cs
Content     :   Save or load a collection of variables
Created     :   March 7, 2013
Authors     :   Peter Giokaris

Copyright   :   Copyright 2013 Oculus VR, Inc. All Rights reserved.

Use of this software is subject to the terms of the Oculus LLC license
agreement provided at the time of installation or download, or which
otherwise accompanies this software in either electronic or hard copy form.

************************************************************************************/
using UnityEngine;
using System.Collections.Generic;

//-------------------------------------------------------------------------------------
// ***** OVRPresetManager
//
// OVRPresetManager is a helper class to allow for a set of variables to be saved and
// recalled using the Unity PlayerPrefs class. 
//
// OVRPresetManager is currently being used by the OVRMainMenu component.
//
public class OVRPresetManager
{	
	static string PresetName = "";
	
	// SetCurrentPreset
	public bool SetCurrentPreset(string presetName)
	{
		PresetName = presetName;
		return true;
	}
	
	// SetPropertyInt
	public bool SetPropertyInt(string name, ref int v)
	{
		string key = PresetName + name;
		PlayerPrefs.SetInt (key, v);
		return true;
	}
	
	// GetPropertyInt
	public bool GetPropertyInt(string name, ref int v)
	{
		string key = PresetName + name;		
		if(PlayerPrefs.HasKey(key) == false)
			return false;
		
		v = PlayerPrefs.GetInt (key);
		return true;
	}
	
	// SetPropertyFloat
	public bool SetPropertyFloat(string name, ref float v)
	{
		string key = PresetName + name;
		PlayerPrefs.SetFloat (key, v);
		return true;
	}
	
	// GetPropertyFloat
	public bool GetPropertyFloat(string name, ref float v)
	{
		string key = PresetName + name;		
		if(PlayerPrefs.HasKey(key) == false)
			return false;
		
		v = PlayerPrefs.GetFloat (key);
		return true;
	}
	
	// SetPropertyString
	public bool SetPropertyString(string name, ref string v)
	{
		string key = PresetName + name;
		PlayerPrefs.SetString (key, v);
		return true;
	}
	
	// GetPropertyString
	public bool GetPropertyString(string name, ref string v)
	{
		string key = PresetName + name;		
		if(PlayerPrefs.HasKey(key) == false)
			return false;
		
		v = PlayerPrefs.GetString(key);
		return true;
	}

	// DeleteProperty
	public bool DeleteProperty(string name)
	{
		string key = PresetName + name;
		PlayerPrefs.DeleteKey(key);	
		return true;
	}
	
	// SaveAll
	public bool SaveAll()
	{
		PlayerPrefs.Save();
		return true;
	}
	
	// DeleteAll
	public bool DeleteAll()
	{
		PlayerPrefs.DeleteAll();
		return true;
	}
}

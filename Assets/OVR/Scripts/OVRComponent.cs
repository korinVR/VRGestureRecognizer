/************************************************************************************

Filename    :   OVRComponent.cs
Content     :   Base component OVR class
Created     :   January 8, 2013
Authors     :   Peter Giokaris

Copyright   :   Copyright 2013 Oculus VR, Inc. All Rights reserved.

Use of this software is subject to the terms of the Oculus LLC license
agreement provided at the time of installation or download, or which
otherwise accompanies this software in either electronic or hard copy form.

************************************************************************************/
using UnityEngine;
using System.Collections.Generic;


//-------------------------------------------------------------------------------------
// ***** OVRComponent
//
// OVRComponent is the base class for many of the OVR classes. It is used to provide base
// functionality to classes derived from it.
//
// NOTE: It is important that any overloaded functions in derived classes call 
// base.<NAME OF FUNCTION (i.e. Awake)> to allow for base functionality.
// 
public class OVRComponent : MonoBehaviour
{
	protected float DeltaTime = 1.0f;

	// Awake
	public virtual void Awake()
	{
	}

	// Start
	public virtual void Start()
	{
	}

	// Update
	public virtual void Update()
	{
		// If we are running at 60fps, DeltaTime will be set to 1.0 
		DeltaTime = (Time.deltaTime * 60.0f);
	}
	
	// LateUpdate
	public virtual void LateUpdate()
	{
	}
}



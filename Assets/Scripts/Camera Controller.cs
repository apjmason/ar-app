/*
Camera Controller.cs
This script is for controlling cameras, I use this script
to control both the First-Person camera and also a remote
camera for creating fly-through animations or to allow
viewing of the data from a different location
*/

using UnityEngine;
using System.Collections;

public class cameraController : MonoBehaviour { 

	GameObject _cameraFP = null;
	GameObject _cameraWV = null;

	void Start ()
	{
		// Initiate cameras
		_cameraFP = GameObject.Find("Main Camera"); 
		if (_cameraFP == null)
			Debug.Log("Start(): First Person Camera not found");
		
		_cameraWV = GameObject.Find("GoT camera"); 
		if (_cameraWV == null)
			Debug.Log("Start(): GoT camera not found"); 
		//run the sub-routine to select the actual camera 
		//(default camera is camera 1)		

		SelectCamera(1); 
	}

	//sub-routine to select the camera
	void SelectCamera(int cameraIndex)
	{
		if (_cameraFP != null)
			_cameraFP.GetComponent<Camera>().enabled = (cameraIndex == 0); 
		if (_cameraWV != null)
			_cameraWV.GetComponent<Camera>().enabled = (cameraIndex == 1); 
		
	}
}

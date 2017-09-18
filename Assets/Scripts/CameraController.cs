using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour {
    
	private Camera cam;
    private GameObject objectToFocusCameraOn = null;
    
	void Start () {
		cam = GetComponentInChildren<Camera> ();
	}

	// Update is called once per frame
	void Update () {

        if (objectToFocusCameraOn != null)
        {
            gameObject.transform.position = objectToFocusCameraOn.transform.position;
        }
    }
    
    public void SetCameraToObject(GameObject _gameObject)
    {
        objectToFocusCameraOn = _gameObject;
    }
	
    public void RemoveCameraFromObject(GameObject _gameObject)
    {
        if (objectToFocusCameraOn == _gameObject)
        {
            objectToFocusCameraOn = null;
        }
    }
}

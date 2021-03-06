﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCameraController : MonoBehaviour
{
    // The player this Camera is attached to
    public PlayerController player;

    public Camera cam;
    [SerializeField] private Vector3 initialOffset;
    private float initialFieldOfView;
    private Vector3 cameraOffset;
    private bool forceLookAtObject;

    public Vector3 CameraOffset {
        get { return cameraOffset; }
        set {
            cameraOffset = value;
            SetCameraToObject(objectToFocusCameraOn, cam.fieldOfView, cameraOffset);
        }
    }

    //GameObject to force camera to focus on another object that isn't the player.
    private GameObject objectToFocusCameraOn = null;

    void Start()
    {
        forceLookAtObject = true;
        initialFieldOfView = cam.fieldOfView;
        cameraOffset = initialOffset;
        if (objectToFocusCameraOn == null)
        {
            SetCameraToObject(player.gameObject);
        }
    }

    void Update()
    {
        if (objectToFocusCameraOn != null && forceLookAtObject)
        {
            transform.position = objectToFocusCameraOn.transform.position + cameraOffset;
            transform.LookAt(objectToFocusCameraOn.transform);
        }
    }

    public Camera GetCamera
    {
        get { return cam; }
    }

    public void SetCameraToObject(GameObject _gameObject, bool setToLookAtObject = true)
    {
        SetCameraToObject(_gameObject, initialFieldOfView, cameraOffset, setToLookAtObject);
    }

    /// <summary>
    /// Set the Camera while changing the field of view and initial offset
    /// </summary>
    /// <param name="_gameObject"></param>
    public void SetCameraToObject(GameObject _gameObject, float _fieldOfView, Vector3 _offset, bool setToLookAtObject = true)
    {
        objectToFocusCameraOn = _gameObject;
        transform.SetParent(objectToFocusCameraOn.transform, true);
        cam.fieldOfView = _fieldOfView;
        forceLookAtObject = setToLookAtObject;
    }

    public void RemoveCameraFromObject(GameObject _gameObject)
    {
        if (objectToFocusCameraOn == _gameObject)
        {
            transform.SetParent(null);
            cam.fieldOfView = initialFieldOfView;
            objectToFocusCameraOn = null;
        }
    }
}

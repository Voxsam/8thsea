using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCameraController : MonoBehaviour
{
    // The player this Camera is attached to
    public PlayerController player;

    public Camera cam;
    public Vector3 initialOffset;

    //GameObject to force camera to focus on another object that isn't the player.
    private GameObject objectToFocusCameraOn = null;

    void Start()
    {

    }

    void Update()
    {
        if (objectToFocusCameraOn != null)
        {
            transform.LookAt(objectToFocusCameraOn.transform);
            transform.position = objectToFocusCameraOn.transform.position + initialOffset;
        }
        else
        {
            transform.LookAt(player.gameObject.transform);
            this.transform.position = player.transform.position + initialOffset;
        }
    }

    public Camera GetCamera
    {
        get { return cam; }
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

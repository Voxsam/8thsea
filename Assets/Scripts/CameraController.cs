using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour {

	public enum CameraType
	{
		LAB, SUB
	};

	public CameraType cameraType;

	public GameObject room;
	private RoomController roomCtrl;

	private float yOffset;
	public float zOffset;

	public float minFOV; // minimum FOV of camera
	public float xDifference;

	public Camera cam;


	private Vector3 initCamPos;
	private Vector3 offset;

	private Vector3 maxBounds;
	private Vector3 minBounds;
	private Vector3 cameraCenter;

    private List<GameObject> listOfPlayersInRoom = null;

    private GameObject objectToFocusCameraOn = null;


	void Start () {
		cam = GetComponent<Camera> ();
		initCamPos = transform.position;
		offset = room.transform.position - initCamPos;
        //roomCtrl = room.GetComponent<RoomController> ();
        listOfPlayersInRoom = new List<GameObject>();
	}

	// Update is called once per frame
	void Update () {

		//GetPlayerBounds ();

        if (objectToFocusCameraOn != null)
        {
            gameObject.transform.position = objectToFocusCameraOn.transform.position - offset;

            /*
            if (maxBounds.x - minBounds.x > xDifference) {
                cam.fieldOfView = (maxBounds.x - minBounds.x) / xDifference * minFOV;
            } else {
                cam.fieldOfView = minFOV;
            }
            */
        }


    }

	private void GetPlayerBounds () {
        if (listOfPlayersInRoom.Count > 0)
        {
		    Vector3 curr = listOfPlayersInRoom[0].transform.position;
		    maxBounds = minBounds = curr;

		    for (int i = 1; i < listOfPlayersInRoom.Count; i++) {
			    curr = roomCtrl.playersInRoom [i].transform.position;
			    maxBounds = new Vector3 (Mathf.Max (maxBounds.x, curr.x), Mathf.Max (maxBounds.y, curr.y), Mathf.Max (maxBounds.z, curr.z));
			    minBounds = new Vector3 (Mathf.Min (minBounds.x, curr.x), Mathf.Min (minBounds.y, curr.y), Mathf.Min (minBounds.z, curr.z));
		    }
			
		    cameraCenter = new Vector3 (minBounds.x + (maxBounds.x - minBounds.x) / 2, minBounds.y + (maxBounds.y - minBounds.y) / 2 + yOffset, minBounds.z + (maxBounds.z - minBounds.z) / 2);//+ zOffset);

        }
    }

    public void AssignCameraToObject(GameObject _gameObject)
    {
        objectToFocusCameraOn = _gameObject;
        //listOfPlayersInRoom.Add(_gameObject);
    }
	
    public void RemoveCameraFromObject(GameObject _gameObject)
    {
        if (objectToFocusCameraOn == _gameObject)
        {
            objectToFocusCameraOn = null;
        }
    }
}

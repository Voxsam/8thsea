using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TubeController : StationControllerInterface {

	public float forwardSpeed;
	public float attractionForce;
	//public bool isActivated; // In parent
	public float radius;
	public GameObject anchorPoint;
    private bool systemActivated;

    public override GameData.ControlType ControlMode
    {
        get { return GameData.ControlType.STATION; }
    }

    //public GameObject playerCharacter;
    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        // While the A button is held down, activate the system
        if (GameController.Obj.ButtonA_Hold)
        {
            systemActivated = true;
        }
        else
        {
            systemActivated = false;
        }

        if (isActivated) {

            float x = Input.GetAxis ("Horizontal") * Time.deltaTime * forwardSpeed;
			float y = Input.GetAxis ("Vertical") * Time.deltaTime * forwardSpeed;
			Vector3 newLocation = transform.position+transform.TransformDirection(x,y,0); 
			float distance = Vector3.Distance(newLocation, anchorPoint.transform.position);
			if (distance > radius) {
				Vector3 fromAnchorToObject = newLocation - anchorPoint.transform.position;
				fromAnchorToObject *= radius / distance;
				newLocation = anchorPoint.transform.position + fromAnchorToObject;
				transform.position = newLocation;

			} else {
				transform.Translate (x, y, 0,Space.World);
				/*if (x != 0) {
					Vector3 lookatPos = new Vector3 (0, 0, x);
					transform.rotation = Quaternion.Slerp (transform.rotation, Quaternion.LookRotation (lookatPos), 0.5f);
				}*/
			}
		}
	}

    public override void SetPlayerToStation(PlayerController player)
    {
        base.SetPlayerToStation(player);
        stationCamera.SetCameraToObject(gameObject);
    }

    void OnCollisionEnter(Collision other) {
		if (isActivated & systemActivated) {
            if (GameController.Obj.GetFishFromCollider(other.collider) != null)
            {
                Destroy(other.gameObject);
            }
		}
	}

	void OnTriggerStay (Collider other) {
        if (isActivated && systemActivated)
        {
            FishController fish = GameController.Obj.GetFishFromCollider(other);
            if (fish != null && other.attachedRigidbody)
            {
                Vector3 dir = transform.position - other.transform.position;
                dir = dir.normalized;
                other.attachedRigidbody.AddForce((dir) * attractionForce);
            }
                
        }
	}

    public override bool SwitchCondition()
    {
        return true;
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TubeController : StationControllerInterface {

    public const int SPAWN_LOCATION_OFFSET = 0; // Spawn at SpawnPoint with a randomised offset of this float
    public float forwardSpeed;
    public float attractionForce;
    //public bool isActivated; // In parent
    public float radius;
    public GameObject anchorPoint;
    private bool systemActivated;
    public bool SystemActivated
    {
        get
        {
            return systemActivated;
        }
    }

    public Transform SpawnPoint; // Put the fishes here after they have been sucked up

    //The head of the suction tube.
    public GameObject tubeHeadGameObject;

    public override GameData.ControlType ControlMode
    {
        get { return GameData.ControlType.STATION; }
    }

    //public GameObject playerCharacter;
    // Use this for initialization
    void Start () {
    }
    public override void WhenActivated()
    {
    }

    public override void WhenDeactivated()
    {
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

        if (IsActivated) {
            float x = Input.GetAxis ("Horizontal") * Time.deltaTime * forwardSpeed;
			float y = Input.GetAxis ("Vertical") * Time.deltaTime * forwardSpeed;
			Vector3 newLocation = tubeHeadGameObject.transform.position + tubeHeadGameObject.transform.TransformDirection(x,y,0); 
			float distance = Vector3.Distance(newLocation, anchorPoint.transform.position);
			if (distance > radius) {
				Vector3 fromAnchorToObject = newLocation - anchorPoint.transform.position;
				fromAnchorToObject *= radius / distance;
				newLocation = anchorPoint.transform.position + fromAnchorToObject;
                tubeHeadGameObject.transform.position = newLocation;

			} else {
                tubeHeadGameObject.transform.Translate (x, y, 0,Space.World);
				/*if (x != 0) {
					Vector3 lookatPos = new Vector3 (0, 0, x);
					transform.rotation = Quaternion.Slerp (transform.rotation, Quaternion.LookRotation (lookatPos), 0.5f);
				}*/
			}

            // Free the character from the station if the conditions are met
            if (this.playerInStation.ControlMode != GameData.ControlType.CHARACTER &&
                this.SwitchCondition() && GameController.Obj.ButtonB_Down)
            {
                this.IsActivated = false;
                this.WhenDeactivated();
                this.ReleasePlayerFromStation();
            }
        }
	}

    public override void SetPlayerToStation(PlayerController player)
    {
        base.SetPlayerToStation(player);
        stationCamera.SetCameraToObject(tubeHeadGameObject);
    }

    public override bool SwitchCondition()
    {
        return true;
    }

    public void ExtractFish ( FishController fish, GameObject other )
    {
        fish.fishMovementController.FishSchoolController.RemoveFishFromSchool(other.gameObject);
        fish.transform.position = SpawnPoint.position + new Vector3(
            GameController.RNG.Next(-SPAWN_LOCATION_OFFSET, SPAWN_LOCATION_OFFSET),
            GameController.RNG.Next(-SPAWN_LOCATION_OFFSET, SPAWN_LOCATION_OFFSET),
            GameController.RNG.Next(-SPAWN_LOCATION_OFFSET, SPAWN_LOCATION_OFFSET)
        );
        fish.fishMovementController.SetEnabled(false);
        fish.SetRigidbody(true);
        fish.rb.velocity = Vector3.zero;
        fish.transform.SetParent(SpawnPoint);
    }

    public void MoveFish (Vector3 dir, GameObject other)
    {
        dir = dir.normalized;
        other.transform.Translate((dir) * attractionForce);
    }

    //Functions from Interface IInteractables
    override public void Interact()
    {
    }

    override public void Interact(GameObject otherActor)
    {
        if (this.playerInStation == null)
        {
            PlayerController player = otherActor.GetComponent<PlayerController>();
            if (player != null)
            {
                // If the player is in Character control mode
                if (player.ControlMode == GameData.ControlType.CHARACTER)
                {
                    this.SetPlayerToStation(player);
                    this.IsActivated = true;
                    this.WhenActivated();
                }
            }
        }
    }

    override public void ToggleHighlight(bool toggle = true)
    {
    }
}

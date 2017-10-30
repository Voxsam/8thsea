using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TubeController : StationControllerInterface {
    //public const int SPAWN_LOCATION_OFFSET = 0; // Spawn at SpawnPoint with a randomised offset of this float
    public const float SUBMARINE_CAMERA_FIELD_OF_VIEW = 30f;
    protected float cameraOriginalFov;

    public Vector3 SUBMARINE_CAMERA_DISTANCE_FROM_TARGET = new Vector3(0, 0, -30f);
    protected Vector3 cameraOriginalDistance;

    public float forwardSpeed;
    public float attractionForce;
    //public bool isActivated; // In parent
    public float radius;
    public GameObject anchorPoint;
    public Collider submarineSurfaceCollider;
    public Transform SpawnPoint; // Put the fishes here after they have been sucked up
    //The head of the suction tube.
    public TubeHeadController tubeHeadController;

    public enum State
    {
        Idle,
        Extracting,
        Withdrawing
    }
    private State currentState;

    private bool systemActivated;
    public bool SystemActivated
    {
        get
        {
            return systemActivated;
        }
        set
        {
            systemActivated = value;
        }
    }

    //Temp storage for the fish while succ is in Extracting state.
    private FishController fishToExtractController;
    private GameObject fishToExtractObject;

    public override GameData.ControlType ControlMode
    {
        get { return GameData.ControlType.STATION; }
    }

    //public GameObject playerCharacter;
    // Use this for initialization
    void Start () {
        currentState = State.Idle;
    }
    public override void WhenActivated()
    {
        cameraOriginalFov = stationCamera.GetCamera.fieldOfView;
        stationCamera.GetCamera.fieldOfView = SUBMARINE_CAMERA_FIELD_OF_VIEW;

        cameraOriginalDistance = stationCamera.CameraOffset;
        stationCamera.CameraOffset = SUBMARINE_CAMERA_DISTANCE_FROM_TARGET;
    }

    public override void WhenDeactivated()
    {
        stationCamera.GetCamera.fieldOfView = cameraOriginalFov;
        stationCamera.CameraOffset = cameraOriginalDistance;
    }

    // Update is called once per frame
    void Update () {
        switch (currentState)
        {
            case State.Idle:
                if (IsActivated && playerInStation != null)
                {
                    // While the A button is held down, activate the system
                    if (playerInStation.controls.GetActionKey())
                    {
                        systemActivated = true;
                    }
                    else
                    {
                        systemActivated = false;
                    }

                    float x = playerInStation.controls.GetHorizontalAxis() * Time.deltaTime * forwardSpeed;
			        float y = playerInStation.controls.GetVerticalAxis() * Time.deltaTime * forwardSpeed;
			        Vector3 newLocation = tubeHeadController.gameObject.transform.position + tubeHeadController.gameObject.transform.TransformDirection(x,y,0);

                    //Position the anchor point as the closest point on the submarine mesh to the position of the succ head.
                    Ray ray = new Ray(tubeHeadController.tubeHeadGameObject.transform.position, 
                                    (submarineSurfaceCollider.transform.position - tubeHeadController.tubeHeadGameObject.transform.position).normalized);
                    RaycastHit hit;
                    if (submarineSurfaceCollider.Raycast(ray, out hit, 100.0F))
                    {
                        anchorPoint.transform.position = hit.point;
                    }   

                    float distance = Vector3.Distance(newLocation, anchorPoint.transform.position);
			        if (distance > radius) {
				        Vector3 fromAnchorToObject = newLocation - anchorPoint.transform.position;
				        fromAnchorToObject *= radius / distance;
				        newLocation = anchorPoint.transform.position + fromAnchorToObject;
                        tubeHeadController.gameObject.transform.position = newLocation;

			        } else {
                        tubeHeadController.gameObject.transform.Translate (x, y, 0,Space.World);
				        /*if (x != 0) {
					        Vector3 lookatPos = new Vector3 (0, 0, x);
					        transform.rotation = Quaternion.Slerp (transform.rotation, Quaternion.LookRotation (lookatPos), 0.5f);
				        }*/
			        }

                    // Free the character from the station if the conditions are met
                    if (this.playerInStation.ControlMode != GameData.ControlType.CHARACTER &&
                        this.SwitchCondition() && playerInStation.controls.GetCancelKeyDown())
                    {
                        currentState = State.Withdrawing;
                    }
                }
                break;
            case State.Extracting:
                Vector3 extractionDirection = (anchorPoint.transform.position - tubeHeadController.tubeHeadGameObject.transform.position).normalized;
                tubeHeadController.gameObject.transform.Translate(extractionDirection * forwardSpeed * Time.deltaTime, Space.World);

                if (Vector3.Distance (tubeHeadController.gameObject.transform.position, anchorPoint.transform.position ) < 2.5)
                {
                    currentState = State.Idle;

                    ExtractFish(fishToExtractController, fishToExtractObject);
                }
                break;
            case State.Withdrawing:
                Vector3 withdrawDirection = (anchorPoint.transform.position - tubeHeadController.tubeHeadGameObject.transform.position).normalized;
                tubeHeadController.gameObject.transform.Translate(withdrawDirection * forwardSpeed * Time.deltaTime, Space.World);

                if (Vector3.Distance(tubeHeadController.gameObject.transform.position, anchorPoint.transform.position) < 2.5)
                {
                    currentState = State.Idle;

                    if (this.IsActivated)
                    {
                        this.IsActivated = false;
                        this.WhenDeactivated();
                        this.ReleasePlayerFromStation();
                    }
                }
                break;
        }
    }

    public override void SetPlayerToStation(PlayerController player)
    {
        base.SetPlayerToStation(player);
        stationCamera.SetCameraToObject(tubeHeadController.gameObject);
    }

    public override bool SwitchCondition()
    {
        return true;
    }

    public void StartExtraction ( FishController fish, GameObject other )
    {
        if (fishToExtractController == null)
        {
            fishToExtractController = fish;
            fishToExtractObject = other;

            currentState = State.Extracting;
            systemActivated = false;

            //Store the location the fish was caught at and the fish school it belonged to.
            //This is for releasing the fish.
            fish.CaughtPosition = fish.transform.position;
            fish.FishSchoolController = fish.fishMovementController.FishSchoolController;
            fish.fishMovementController.FishSchoolController.RemoveFishFromSchool(other);

            fish.SetEnabled(true);
            fish.fishMovementController.SetEnabled(false);

            fish.gameObject.GetComponent<Rigidbody>().detectCollisions = false;
            fish.gameObject.GetComponent<Rigidbody>().isKinematic = true;

            fish.transform.SetParent(tubeHeadController.gameObject.transform);
            fish.transform.localPosition = Vector3.zero;
        }
    }

    public void ExtractFish ( FishController fish, GameObject other )
    {
        if (this.playerInStation != null)
        {
            PlayerInteractionController player = this.playerInStation.GetComponent<PlayerInteractionController>();
            if (player != null)
            {
                //Player is not currently holding anything and can pick up the sucked fish.
                if (player.GetHeldObject() == null)
                {
                    /*fish.transform.position = SpawnPoint.position + new Vector3(
                        GameController.RNG.Next(-SPAWN_LOCATION_OFFSET, SPAWN_LOCATION_OFFSET),
                        GameController.RNG.Next(-SPAWN_LOCATION_OFFSET, SPAWN_LOCATION_OFFSET),
                        GameController.RNG.Next(-SPAWN_LOCATION_OFFSET, SPAWN_LOCATION_OFFSET)
                    );*/
                    fish.gameObject.GetComponent<Rigidbody>().detectCollisions = false;
                    fish.gameObject.GetComponent<Rigidbody>().isKinematic = true;
                    fish.transform.position = SpawnPoint.position;
                    fish.SetEnabled(true);
                    fish.fishMovementController.SetEnabled(false);

                    player.PickUpObject(other);

                    /*fish.SetRigidbody(true);
                    fish.rb.velocity = Vector3.zero;
                    fish.transform.SetParent(SpawnPoint);*/

                    this.IsActivated = false;
                    this.WhenDeactivated();
                    this.ReleasePlayerFromStation();

                    fishToExtractController = null;
                    fishToExtractObject = null;
                }
            }
        }
    }

    public void ReleaseFish (FishController fishController)
    {
        //Housekeeping for changing a fish back into "swim" mode.
        fishController.PutDown();
        fishController.SetEnabled(false);
        fishController.fishMovementController.SetEnabled(true);
        fishController.fishMovementController.Initialise();

        fishController.FishSchoolController.AddFishToSchool(fishController.gameObject);
        fishController.gameObject.transform.SetParent(null);

        fishToExtractController = null;
        fishToExtractObject = null;
    }

    public void MoveFish (Vector3 dir, Transform other)
    {
        dir = dir.normalized;
        other.Translate((dir) * attractionForce * Time.deltaTime, Space.World);
    }

    public void EjectPlayer ()
    {
        //Only eject the player if they are in the idle state, otherwise they are already in the process of ejecting.
        if (currentState == State.Idle)
        {
            currentState = State.Withdrawing;
        }
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
            PlayerInteractionController playerInteraction = otherActor.GetComponent<PlayerInteractionController>();
            if (player != null && playerInteraction != null)
            {
                // If the player is in Character control mode and not currently holding anything.
                if (player.ControlMode == GameData.ControlType.CHARACTER && playerInteraction.GetHeldObject() == null)
                {
                    this.SetPlayerToStation(player);
                    this.IsActivated = true;
                    this.WhenActivated();
                }
            }
        }
    }

    override public void ToggleHighlight(PlayerController otherPlayerController, bool toggle = true)
    {
    }

    public void MoveInDirection(Vector3 direction)
    {
        tubeHeadController.gameObject.transform.Translate(forwardSpeed * Time.deltaTime * direction.x, forwardSpeed * Time.deltaTime * direction.y, 0, Space.World);
    }
}

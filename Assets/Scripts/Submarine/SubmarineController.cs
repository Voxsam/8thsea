using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SubmarineController : StationControllerInterface {
    public static SubmarineController Obj;

    public const float ACCELERATION = 0.1f;
    public const float STARTING_SPEED = 0.2f;
    public const float MAXIMUM_SPEED = 15f;

    public float currentSpeed;

    // Camera things
    public const float SUBMARINE_CAMERA_FIELD_OF_VIEW = 60f;
    protected float cameraOriginalFov;

    public Vector3 SUBMARINE_CAMERA_DISTANCE_FROM_TARGET = new Vector3(0, 0, -50f);
    protected Vector3 cameraOriginalDistance;

    public Transform UIPrefab;
    public Transform dockingPosition;
    private SphereCollider interiorCollider;

	public MeshCollider labDoorCollider;

    public OxygenCountdown oxygenCountdownController;

    //Used to turn on/off the interactable-ness of the stations (for emergency mode)
    public StationController [] submarineStationControllers;
    public TubeController tubeController;

    public Light light1;
    public Light light2;
    private Color regularLampClr = new Color(1.0f, 0.9568f, 0.82352f, 1.0f);

    private Animator anim;
    private bool facingLeft; //false = facingRight

    public enum State
    {
        Idle,
        Docking,
        Docked
    }
    private State currentState;

    private bool emergencyMode;

    private bool withinDock;
    public bool WithinDock
    {
        get
        {
            return withinDock;
        }
        set
        {
            withinDock = value;
        }
    }


    public override GameData.ControlType ControlMode
    {
        get { return GameData.ControlType.SUBMARINE; }
    }

    public override bool IsActivated
    {
        set
        {
            base.IsActivated = value;
        }
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
        if (currentState == State.Docked)
        {
            oxygenCountdownController.StartRefilling();
        }

        stationCamera.GetCamera.fieldOfView = cameraOriginalFov;
        stationCamera.CameraOffset = cameraOriginalDistance;
    }
    
    public bool IsDocked()
    {
        return currentState == State.Docked;
    }

    void Awake()
    {
        if (Obj == null)
        {
            Obj = this;
        }
        else
        {
            Destroy(this.gameObject);
        }
    }

    // Use this for initialization
    private void Start () {
        currentSpeed = STARTING_SPEED;

        //oxygenCountdownScript.isActivated = false;

        interiorCollider = GetComponent<SphereCollider>();
        // Ensure that this submarine is a child of DockingPosition
        this.transform.SetParent(dockingPosition);
        this.transform.localPosition = Vector3.zero;

        // find animator
        anim = GetComponentInChildren<Animator>();
        anim.SetBool("docked", true);

        currentState = State.Docked;

        emergencyMode = false;
        withinDock = true;
    }

    // Update is called once per frame
    private void Update () {
		// activate and deactivate lab door
		if (currentState == State.Docked) {
			labDoorCollider.enabled = false;
		} else {
			labDoorCollider.enabled = true;
		}

        //If the sub is out of oxygen, enter emergency mode.
        if (oxygenCountdownController.IsEmergency() && !emergencyMode)
        {
            emergencyMode = true;
            StartEmergencyMode();
        }
        //Oxygen is being refilled, leave emergency mode.
        else if (!oxygenCountdownController.IsEmergency() && emergencyMode)
        {
            light1.color = Color.Lerp(regularLampClr, Color.blue, Time.deltaTime);
            light2.color = Color.Lerp(regularLampClr, Color.blue, Time.deltaTime);
            emergencyMode = false;
            EndEmergencyMode();
        }

            switch (currentState)
        {
            case State.Docked:
                if (IsActivated && playerInStation != null)
                {
                    if (oxygenCountdownController.IsReady())
                    {
                        currentState = State.Idle;
                        oxygenCountdownController.StartRunning();
                    }
                    // Free the character from the station if the conditions are met
                    if (this.playerInStation.ControlMode != GameData.ControlType.CHARACTER &&
                        this.SwitchCondition() && playerInStation.controls.GetCancelKeyDown())
                    {
                        this.IsActivated = false;
                        this.WhenDeactivated();
                        this.ReleasePlayerFromStation();
                    }
                }
                break;
            case State.Idle:
                //anim.SetBool("docked", IsDocked()); //this line is causing animation trouble - IsDocked always returns false once the driving station is activated..
                if (IsActivated && playerInStation != null)
                {
                    float horizontalControl = playerInStation.controls.GetHorizontalAxis();
                    float verticalControl = playerInStation.controls.GetVerticalAxis();

                    // ANIMATION STUFF 
                    if (horizontalControl > 0) //going right 
                    {
                        anim.SetBool("moveRight", true);
                        if (facingLeft)
                        {
                            anim.SetBool("moveLeft", false);
                            anim.SetTrigger("turnRight");
                            facingLeft = false;
                        }
                    }
                    else if (horizontalControl < 0) //going left
                    {
                        anim.SetBool("moveLeft", true);
                        if (!facingLeft)
                        {
                            anim.SetBool("moveRight", false);
                            anim.SetTrigger("turnLeft");
                            facingLeft = true;
                        }
                    } // end of animation stuff

                    /*transform.position = Vector3.Lerp
                    (
                        transform.position,
                        transform.position + new Vector3(currentSpeed * horizontalControl, currentSpeed * verticalControl, 0),
                        Time.deltaTime
                    );*/
                    //transform.Translate(currentSpeed * Time.deltaTime * horizontalControl, currentSpeed * Time.deltaTime * verticalControl, 0);
                    MoveInDirection(new Vector3(
                        horizontalControl,
                        verticalControl,
                        0
                    ));

                    currentSpeed += ACCELERATION;
                    if (currentSpeed > MAXIMUM_SPEED)
                    {
                        currentSpeed = MAXIMUM_SPEED;
                    }

                    if (horizontalControl == 0 && verticalControl == 0)
                    {
                        currentSpeed = STARTING_SPEED;
                    }

                    // docking 
                    if (playerInStation.controls.GetActionKeyDown() && IsActivated && WithinDock)
                    {
                        currentState = State.Docking;
                    }

                    // Free the character from the station if the conditions are met
                    if (this.playerInStation.ControlMode != GameData.ControlType.CHARACTER &&
                        this.SwitchCondition() && playerInStation.controls.GetCancelKeyDown())
                    {
                        this.IsActivated = false;
                        this.WhenDeactivated();
                        this.ReleasePlayerFromStation();
                    }
                }
                break;

            case State.Docking:
                Vector3 dockingDirection = (dockingPosition.position - transform.position).normalized;
                MoveInDirection(dockingDirection);
                currentSpeed += ACCELERATION;
                if (currentSpeed > MAXIMUM_SPEED)
                {
                    currentSpeed = MAXIMUM_SPEED;
                }
                if (Vector3.Distance(transform.position, dockingPosition.position) < 0.1)
                {
                    currentState = State.Docked;
                    currentSpeed = STARTING_SPEED;
                    transform.position = dockingPosition.position;
                    anim.SetBool("docked", true);
                    anim.SetBool("moveLeft", false);
                    anim.SetBool("moveRight", false);

                    this.IsActivated = false;
                    this.WhenDeactivated();
                    this.ReleasePlayerFromStation();
                }

                if (playerInStation != null && playerInStation.controls.GetCancelKeyDown() && IsActivated && WithinDock)
                {
                    currentState = State.Idle;
                }
                break;
        }
    }
    public override bool SwitchCondition()
    {
        return true; // Always allow the switch
    }

    //Functions from Interface IInteractables
    override public void Interact()
    {
    }

    override public void Interact(GameObject otherActor)
    {
        
        if (this.playerInStation == null)
        {

			if (GameController.Obj.isTutorial) {
				TutorialManager.Obj.hasTriedDriving = true;
			}

            //Make sure the submarine is ready to set off if it is docked before allowing players to drive.
            //If the submarine is not docked proceed as normal.
            if (currentState != State.Docked || (currentState == State.Docked && oxygenCountdownController.IsReady()))
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
    }

    override public void ToggleHighlight(PlayerController otherPlayerController, bool toggle = true)
    {
    }

    public void OpenDoorAnim()
    {
        anim.SetTrigger("openDoor");
        anim.SetBool("doorOpen", true);
    }

    public void CloseDoorAnim()
    {
        anim.SetTrigger("closeDoor");
        anim.SetBool("doorOpen", false);
    }

    public void MoveInDirection (Vector3 direction)
    {
        /*
        transform.Translate(
            currentSpeed * Time.deltaTime * direction.x, 
            currentSpeed * Time.deltaTime * direction.y, 
            0
        );
        //*/

        Vector3 locationOneSecondLater = transform.position +
            new Vector3(
                currentSpeed * direction.x,
                currentSpeed * direction.y,
                0
            );
        transform.position = Vector3.Lerp(
            transform.position,
            locationOneSecondLater,
            Time.deltaTime
        );
    }

    private void StartEmergencyMode ()
    {
        for (int i = 0; i < submarineStationControllers.Length; i++)
        {
            submarineStationControllers[i].IsActivated = false;
        }
        tubeController.EjectPlayer();
    }

    private void EndEmergencyMode ()
    {
        for (int i = 0; i < submarineStationControllers.Length; i++)
        {
            submarineStationControllers[i].IsActivated = true;
        }
    }
}

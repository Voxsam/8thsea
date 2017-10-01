using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SubmarineController : StationControllerInterface {
    public static SubmarineController Obj;

    public float acceleration;
    public float currentSpeed;
    public float maximumSpeed;

    // Camera things
    public const float SUBMARINE_CAMERA_FIELD_OF_VIEW = 30f;
    protected float cameraOriginalFov;

    public Transform UIPrefab;
    public Transform dockingPosition;
    public Text FishCounter;

    public OxygenCountdown oxygenCountdownScript;

    public TeleportDoor teleportToSubFromToLabDoor;
    public TeleportDoor teleportToLabFromToSubDoor;

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
    }

    public override void WhenDeactivated()
    {
        stationCamera.GetCamera.fieldOfView = cameraOriginalFov;
    }

    public bool IsDocked()
    {
        return this.transform.position == dockingPosition.position;
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
        acceleration = 0.05f;
        currentSpeed = 0.2f;
        maximumSpeed = 10f;
        oxygenCountdownScript.isActivated = false;

        // Only allow teleport if it is docked
        teleportToLabFromToSubDoor.Initialise(IsDocked);
        teleportToSubFromToLabDoor.Initialise(IsDocked);
    }
	
	// Update is called once per frame
	private void Update () {
        if (IsActivated)
        {
            float horizontalControl = Input.GetAxis("Horizontal");
            float verticalControl = Input.GetAxis("Vertical");
            transform.Translate(currentSpeed * Time.deltaTime * horizontalControl, currentSpeed * Time.deltaTime * verticalControl, 0);
            currentSpeed += acceleration;
            if (currentSpeed > maximumSpeed)
            {
                currentSpeed = maximumSpeed;
            }
            if (horizontalControl == 0 && verticalControl == 0)
            {
                currentSpeed = 0.2f;
            }

            if (GameController.Obj.ButtonA_Down && IsActivated)
            {
                transform.position = dockingPosition.position;
            }
        }
    }
    public override bool SwitchCondition()
    {
        return true; // Always allow the switch
    }
}

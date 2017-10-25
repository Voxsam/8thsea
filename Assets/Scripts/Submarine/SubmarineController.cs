﻿using System;
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
    public const float SUBMARINE_CAMERA_FIELD_OF_VIEW = 60f;
    protected float cameraOriginalFov;

    public Vector3 SUBMARINE_CAMERA_DISTANCE_FROM_TARGET = new Vector3(0, 0, -50f);
    protected Vector3 cameraOriginalDistance;

    public Transform UIPrefab;
    public Transform dockingPosition;
    public Text FishCounter;
    private SphereCollider interiorCollider;

    public OxygenCountdown oxygenCountdownScript;

    private Animator anim;
    private bool facingLeft; //false = facingRight

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

        cameraOriginalDistance = stationCamera.initialOffset;
        stationCamera.initialOffset = SUBMARINE_CAMERA_DISTANCE_FROM_TARGET;
    }

    public override void WhenDeactivated()
    {
        stationCamera.GetCamera.fieldOfView = cameraOriginalFov;
        stationCamera.initialOffset = cameraOriginalDistance;
    }

    public bool IsDocked()
    {
        return transform.position == dockingPosition.position;
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
        //oxygenCountdownScript.isActivated = false;

        interiorCollider = GetComponent<SphereCollider>();
        // Ensure that this submarine is a child of DockingPosition
        this.transform.SetParent(dockingPosition);
        this.transform.localPosition = Vector3.zero;

        // find animator
        anim = GetComponentInChildren<Animator>();
        anim.SetBool("docked", true);
    }

    // Update is called once per frame
    private void Update () {
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
            
            // docking 
            if (playerInStation.controls.GetActionKeyDown() && IsActivated)
            {
                transform.position = dockingPosition.position;
                anim.SetBool("docked", true);
                anim.SetBool("moveLeft", false);
                anim.SetBool("moveRight", false);
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
        transform.Translate(currentSpeed * Time.deltaTime * direction.x, currentSpeed * Time.deltaTime * direction.y, 0);
    }
}

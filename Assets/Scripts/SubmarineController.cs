using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SubmarineController : StationControllerInterface {
    public float acceleration;
    public float currentSpeed;
    public float maximumSpeed;
    public GameObject dockingPosition;
    public OxygenCountdown oxygenCountdownScript;

    public override GameController.ControlType ControlMode
    {
        get { return GameController.ControlType.SUBMARINE; }
    }

    // Use this for initialization
    private void Start () {
        acceleration = 0.05f;
        currentSpeed = 0.2f;
        maximumSpeed = 10f;
        oxygenCountdownScript.isActivated = false;
    }
	
	// Update is called once per frame
	private void Update () {
        if (isActivated)
        {
            //Debug.Log(OxygenCountdown.isActivated);
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
        }

    }
    public override bool SwitchCondition()
    {
        // If it is not activated it means it is docked
        return !oxygenCountdownScript.isActivated;
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.tag.Equals("lab"))
        {
            Debug.Log("lab found");
            // Set to dock postition and deactivate the sub controls and UI
            oxygenCountdownScript.isActivated = false;
            transform.position = dockingPosition.transform.position;

            // Then click the player out of the sub
            ReleasePlayerFromStation();
        }
    }
    void OnTriggerExit(Collider other)
    {
        if (other.tag.Equals("lab"))
        {
            Debug.Log("exitting line");
            oxygenCountdownScript.isActivated = true;
        }
    }
}

﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInteractionController : MonoBehaviour {

    private const KeyCode interactKey = KeyCode.E;

    private GameObject player;

    private bool pickUp;

    //Boolean to mark player holding object
    private bool isHolding;
    private GameObject holdSlot;
    private GameObject heldObject;

    //Boolean to mark player colliding with interactables.
    private bool isColliding;

	// Use this for initialization
	void Start () {
        isHolding = false;
        isColliding = false;
        pickUp = false;

        player = gameObject;
        holdSlot = player.transform.Find("HoldSlot").gameObject;
        heldObject = null;
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "FishObject")
        {
            isColliding = true;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.tag == "FishObject")
        {
            isColliding = false;
        }
    }

    void OnTriggerStay(Collider other)
    {
        //Check if player is already holding on to an object
        if (!isHolding)
        {
            if (other.tag == "FishObject")
            {
                if (Input.GetKeyDown(interactKey))
                {
                    Debug.Log("Hello", other.gameObject);
                    heldObject = other.gameObject;
                    pickUp = true;
                }
            }
        }
        else
        {
            //Do something to player holding thing.
        }
    }

    // Update is called once per frame
    void Update () {
		if (isHolding)
        {
            if (Input.GetKeyDown(interactKey))
            {
                Debug.Log("Dropped");
                isHolding = false;
                if (heldObject != null)
                {
                    setAttachObject(heldObject, false);
                    heldObject = null;
                }
            }
        }

        if (pickUp)
        {
            PickUpObject();
        }
	}

    private void setAttachObject (GameObject other, bool attach)
    {
        FishController fishControllerScript = (FishController)heldObject.GetComponent(typeof(FishController));
        if (fishControllerScript != null)
        {
            fishControllerScript.ToggleRigidBody();
        }

        if (attach)
        {
            other.transform.localPosition = Vector3.zero;
            other.transform.localScale = Vector3.one;
            other.transform.SetParent(holdSlot.transform, false);
        }
        else
        {
            other.transform.SetParent(null);
        }
    }
        

    private void PickUpObject ()
    {
        if (heldObject != null && pickUp)
        {
            setAttachObject(heldObject, true);
            isHolding = true;
            pickUp = false;
        }
    }
}

﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInteractionController : MonoBehaviour
{
    enum State
    {
        Idle,
        Hold
    };
    enum SecondaryState
    {
        Idle,
        View
    };
    State currentState;
    SecondaryState currentSecondaryState;
    
    [SerializeField] private PlayerController player;
    [SerializeField] private GameObject holdSlot;

    private GameObject atObject;
    private GameObject heldObject;
    public GameObject GetHeldObject ()
    {
        return heldObject;
    }

	// Use this for initialization
	void Start ()
    {
        currentState = State.Idle;
        currentSecondaryState = SecondaryState.Idle;

        // Set in the editor
        //player = gameObject;
        //player = FindComponent<PlayerController>();
        //holdSlot = gameObject.transform.Find("HoldSlot").gameObject;
        heldObject = null;
    }

    void OnTriggerStay (Collider other)
    {
        if (currentSecondaryState != SecondaryState.View)
        {
            if (other.tag == "FishObject" || other.tag == "StationObject")
            {
                atObject = other.gameObject;
                HighlightObject(atObject, true);
                currentSecondaryState = SecondaryState.View;
            }
        }
    }

    void OnTriggerExit (Collider other)
    {
        if (other.tag == "FishObject" || other.tag == "StationObject")
        {
            if (other.gameObject == atObject)
            {
                currentSecondaryState = SecondaryState.Idle;
                HighlightObject(atObject, false);
                atObject = null;
            }
        }
    }

    // Update is called once per frame
    void Update ()
    {
        switch (currentState)
        {
            case State.Hold:
                // Discard
                if (GameController.Obj.ButtonB_Down)
                {
                    if (heldObject != null && currentSecondaryState == SecondaryState.Idle)
                    {
                        SetAttachObject(heldObject, false);
                        currentState = State.Idle;
                    }
                }
                break;

            case State.Idle:
            default:
                break;
        }

        switch (currentSecondaryState)
        {
            case SecondaryState.View:
                if (GameController.Obj.ButtonA_Down)
                {
                    //Make sure the other object is a FishObject and the player is not currently holding something before picking up new object.
                    if (atObject.tag == "FishObject" && currentState != State.Hold)
                    {
                        PickUpObject (atObject);
                        atObject = null;
                    }
                    else if (atObject.tag == "StationObject")
                    {
                        IInteractable atObjectInteractableScript = (IInteractable)atObject.GetComponent(typeof(IInteractable));
                        if (atObjectInteractableScript != null)
                        {
                            atObjectInteractableScript.Interact(player.gameObject);
                        }
                    }
                }
                break;

            case SecondaryState.Idle:
            default:
                break;
        }
    }

    private void HighlightObject (GameObject other, bool highlight)
    {
        IInteractable otherInteractableScript = (IInteractable)other.GetComponent(typeof(IInteractable));
        if (otherInteractableScript != null)
        {
            otherInteractableScript.ToggleHighlight(highlight);
        }
    }

    private void SetAttachObject (GameObject other, bool attach)
    {
        IInteractable heldObjectInteractableScript = (IInteractable)other.GetComponent(typeof(IInteractable));
        if (heldObjectInteractableScript != null)
        {
            heldObjectInteractableScript.Interact();
            heldObjectInteractableScript.ToggleHighlight(false);
        }

        if (attach)
        {
            other.transform.SetParent(holdSlot.transform, true);
            other.transform.localPosition = Vector3.zero;
            heldObject = other;
        }
        else
        {
            other.transform.SetParent(player.LocationRef, true);
            heldObject = null;
        }
    }

    public void PickUpObject (GameObject other)
    {
        SetAttachObject(other, true);
        currentSecondaryState = SecondaryState.Idle;
        currentState = State.Hold;
    }

    public void DropObject ()
    {
        SetAttachObject(heldObject, false);
        heldObject = null;
        currentSecondaryState = SecondaryState.Idle;
        currentState = State.Idle;
    }
}

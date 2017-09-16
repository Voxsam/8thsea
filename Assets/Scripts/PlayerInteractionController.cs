using System.Collections;
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

    private const KeyCode interactKey = KeyCode.E;

    private GameObject player;

    private GameObject holdSlot;

    private GameObject atObject;
    private GameObject heldObject;
    public GameObject getHeldObject ()
    {
        return heldObject;
    }

	// Use this for initialization
	void Start ()
    {
        currentState = State.Idle;
        currentSecondaryState = SecondaryState.Idle;

        player = gameObject;
        holdSlot = player.transform.Find("HoldSlot").gameObject;
        heldObject = null;
    }

    void OnTriggerStay (Collider other)
    {
        if (currentSecondaryState != SecondaryState.View)
        {
            if (other.tag == "FishObject" || other.tag == "StationObject")
            {
                atObject = other.gameObject;
                highlightObject(atObject, true);
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
                highlightObject(atObject, false);
                atObject = null;
            }
        }
    }

    // Update is called once per frame
    void Update ()
    {
        switch (currentState)
        {
            case State.Idle:
                break;

            case State.Hold:
                if (Input.GetKeyDown(interactKey))
                {
                    if (heldObject != null && currentSecondaryState == SecondaryState.Idle)
                    {
                        setAttachObject(heldObject, false);
                        currentState = State.Idle;
                    }
                }
                break;
                
            default:
                break;
        }

        switch (currentSecondaryState)
        {
            case SecondaryState.Idle:
                break;

            case SecondaryState.View:
                if (Input.GetKeyDown(interactKey))
                {
                    //Make sure the other object is a FishObject and the player is not currently holding something before picking up new object.
                    if (atObject.tag == "FishObject" && currentState != State.Hold)
                    {
                        pickUpObject (atObject);
                        atObject = null;
                    }
                    else if (atObject.tag == "StationObject")
                    {
                        IInteractable atObjectInteractableScript = (IInteractable)atObject.GetComponent(typeof(IInteractable));
                        if (atObjectInteractableScript != null)
                        {
                            atObjectInteractableScript.interact(player);
                        }
                    }
                }
                break;

            default:
                break;
        }
    }

    private void highlightObject (GameObject other, bool highlight)
    {
        IInteractable otherInteractableScript = (IInteractable)other.GetComponent(typeof(IInteractable));
        if (otherInteractableScript != null)
        {
            otherInteractableScript.toggleHighlight(highlight);
        }
    }

    private void setAttachObject (GameObject other, bool attach)
    {
        IInteractable heldObjectInteractableScript = (IInteractable)other.GetComponent(typeof(IInteractable));
        if (heldObjectInteractableScript != null)
        {
            heldObjectInteractableScript.interact();
            heldObjectInteractableScript.toggleHighlight(false);
        }

        if (attach)
        {
            other.transform.localPosition = Vector3.zero;
            other.transform.localScale = Vector3.one;
            other.transform.SetParent(holdSlot.transform, false);
            heldObject = other;
        }
        else
        {
            other.transform.SetParent(null);
            heldObject = null;
        }
    }

    public void pickUpObject (GameObject other)
    {
        setAttachObject(other, true);
        currentSecondaryState = SecondaryState.Idle;
        currentState = State.Hold;
    }

    public void dropObject ()
    {
        setAttachObject(heldObject, false);
        heldObject = null;
        currentSecondaryState = SecondaryState.Idle;
        currentState = State.Idle;
    }
}

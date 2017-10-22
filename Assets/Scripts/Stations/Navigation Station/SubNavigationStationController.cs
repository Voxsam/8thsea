using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SubNavigationStationController : IInteractable {
    public FishReturnStationController returnStationController;

    public enum State
    {
        Empty,
        Holding
    };
    private State currentState;
    public State CurrentState
    {
        get
        {
            return currentState;
        }
    }

    private GameObject holdSlot;
    private GameObject heldObject;
    public GameObject HeldObject
    {
        get
        {
            return heldObject;
        }
    }

    // Use this for initialization
    void Start()
    {
        currentState = State.Empty;
        holdSlot = gameObject.transform.Find("HoldSlot").gameObject;
        heldObject = null;
    }

    // Update is called once per frame
    void Update()
    {
    }

    override public void Interact()
    {
    }

    override public void Interact(GameObject otherActor)
    {
        if (otherActor.tag == "Player")
        {
            if (currentState == State.Empty)
            {
                PlayerInteractionController playerControllerScript = (PlayerInteractionController)otherActor.GetComponent(typeof(PlayerInteractionController));
                if (playerControllerScript != null)
                {
                    //Get the object held by the player.
                    GameObject objectToHold = playerControllerScript.GetHeldObject();
                    if (objectToHold != null)
                    {
                        FishController heldObjectControllerScript = (FishController)objectToHold.GetComponent(typeof(FishController));
                        if (heldObjectControllerScript != null)
                        {
                            playerControllerScript.DropObject();
                            heldObjectControllerScript.PutIn();
                            holdObject(objectToHold);
                            returnStationController.Activate(heldObjectControllerScript);
                        }
                    }
                }
            }
            else if (currentState == State.Holding)
            {
                PlayerInteractionController playerControllerScript = (PlayerInteractionController)otherActor.GetComponent(typeof(PlayerInteractionController));
                if (playerControllerScript != null)
                {
                    //Check that the player is not already holding on to something.
                    if (playerControllerScript.GetHeldObject() == null)
                    {
                        if (heldObject != null)
                        {
                            playerControllerScript.PickUpObject(heldObject);
                            removeHeldObject();
                            returnStationController.Deactivate();
                        }
                    }
                }
            }
        }
    }

    override public void ToggleHighlight(bool toggle = true)
    {
    }

    public GameObject removeHeldObject()
    {
        GameObject objectToReturn = heldObject;
        heldObject = null;
        currentState = State.Empty;

        return objectToReturn;
    }

    public bool holdObject(GameObject objectToHold)
    {
        if (currentState == State.Empty)
        {
            heldObject = objectToHold;
            Vector3 originalScale = heldObject.transform.localScale;
            heldObject.transform.SetParent(holdSlot.transform);
            heldObject.transform.localPosition = Vector3.zero;
            heldObject.transform.localScale = originalScale;

            currentState = State.Holding;
            return true;
        }
        return false;
    }
}

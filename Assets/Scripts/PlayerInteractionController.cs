using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInteractionController : MonoBehaviour {

    private const KeyCode interactKey = KeyCode.E;

    private GameObject player;

    private bool pickUp;
    private bool placeFish;

    //Boolean to mark player holding object
    private bool isHolding;
    public bool getIsHolding ()
    {
        return isHolding;
    }
    private GameObject holdSlot;
    private GameObject atObject;
    private GameObject heldObject;
    public GameObject getHeldObject ()
    {
        return heldObject;
    }

	// Use this for initialization
	void Start () {
        isHolding = false;
        placeFish = pickUp = false;

        player = gameObject;
        holdSlot = player.transform.Find("HoldSlot").gameObject;
        heldObject = null;
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
            if (other.tag == "StationObject")
            {
                if (Input.GetKeyDown(interactKey))
                {
                    Debug.Log("Depositing", other.gameObject);
                    atObject = other.gameObject;
                    placeFish = true;
                }
            }
        }
    }

    // Update is called once per frame
    void Update () {
        //Eventually turn this into a state machine instead of this hackish nonsense.
        if (isHolding)
        {
            if (placeFish)
            {
                placeFish = false;
                IInteractable atObjectInteractableScript = (IInteractable)atObject.GetComponent(typeof(IInteractable));
                if (atObjectInteractableScript != null)
                {
                    atObjectInteractableScript.interact(player);
                }
            }
            else
            {
                if (Input.GetKeyDown(interactKey))
                {
                    DropHeldObject();
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
        IInteractable heldObjectInteractableScript = (IInteractable)heldObject.GetComponent(typeof(IInteractable));
        if (heldObjectInteractableScript != null)
        {
            heldObjectInteractableScript.interact();
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

    public void DropHeldObject ()
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

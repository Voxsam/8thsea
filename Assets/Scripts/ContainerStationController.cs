using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ContainerStationController : MonoBehaviour, IInteractable {
    
    enum State {
        Empty,
        Holding
    };
    State currentState;

    private GameObject holdSlot;
    private GameObject heldObject;

    // Use this for initialization
    void Start () {
        currentState = State.Empty;
        holdSlot = gameObject.transform.Find("HoldSlot").gameObject;
        heldObject = null;
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    public void interact () {
    }

    public void interact (GameObject otherActor) {
        if (otherActor.tag == "Player")
        {
            if (currentState == State.Empty)
            {
                PlayerInteractionController playerControllerScript = (PlayerInteractionController)otherActor.GetComponent(typeof(PlayerInteractionController));
                if (playerControllerScript != null)
                {
                    heldObject = playerControllerScript.getHeldObject();
                    if (heldObject != null)
                    {
                        FishController heldObjectControllerScript = (FishController)heldObject.GetComponent(typeof(FishController));
                        if (heldObjectControllerScript != null)
                        {
                            playerControllerScript.dropObject();
                            heldObjectControllerScript.putIn();
                        }
                        heldObject.transform.localPosition = Vector3.zero;
                        heldObject.transform.localScale = Vector3.one;
                        heldObject.transform.SetParent(holdSlot.transform, false);

                        currentState = State.Holding;
                    }
                }
            }
            else if (currentState == State.Holding)
            {
                PlayerInteractionController playerControllerScript = (PlayerInteractionController)otherActor.GetComponent(typeof(PlayerInteractionController));
                if (playerControllerScript != null)
                {
                    if (playerControllerScript.getHeldObject() == null)
                    {
                        if (heldObject != null)
                        {
                            playerControllerScript.pickUpObject(heldObject);
                            heldObject = null;
                            currentState = State.Empty;
                        }
                    }
                }
            }
        }
    }

    public void toggleHighlight(bool toggle = true)
    {
        if (toggle)
        {
            Renderer rend = GetComponent<Renderer>();
            rend.material.color = Color.red;
        }
        else
        {
            Renderer rend = GetComponent<Renderer>();
            rend.material.color = Color.white;
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ContainerStationController : MonoBehaviour, IInteractable {

    private GameObject holdSlot;

    // Use this for initialization
    void Start () {
        holdSlot = gameObject.transform.Find("HoldSlot").gameObject;
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            Renderer rend = GetComponent<Renderer>();
            rend.material.color = Color.red;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            Renderer rend = GetComponent<Renderer>();
            rend.material.color = Color.white;
        }
    }

    public void interact () {
    }

    public void interact (GameObject otherActor) {
        if (otherActor.tag == "Player")
        {
            PlayerInteractionController playerControllerScript = (PlayerInteractionController)otherActor.GetComponent(typeof(PlayerInteractionController));
            if (playerControllerScript.getIsHolding())
            {
                GameObject heldObject = playerControllerScript.getHeldObject();
                playerControllerScript.DropHeldObject();
                FishController heldObjectControllerScript = (FishController)heldObject.GetComponent(typeof(FishController));
                if (heldObjectControllerScript != null)
                {
                    heldObjectControllerScript.putIn();
                }
                heldObject.transform.localPosition = Vector3.zero;
                heldObject.transform.localScale = Vector3.one;
                heldObject.transform.SetParent(holdSlot.transform, false);
            }
        }
    }
}

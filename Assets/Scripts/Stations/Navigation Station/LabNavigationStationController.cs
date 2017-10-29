using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LabNavigationStationController : IInteractable {
    public GameObject pingObjectTemplate;
    public SubNavigationStationController subNavController;
    public GameObject screenSpaceCanvas;

    private GameObject worldSpaceCanvas;

    public enum State
    {
        Idle,
        Pinging
    };
    private State currentState;

    // Use this for initialization
    void Start () {
        currentState = State.Idle;
        worldSpaceCanvas = gameObject.transform.Find("WorldspaceCanvas").gameObject;
    }
	
	// Update is called once per frame
	void Update () {
        switch (currentState)
        {
            case State.Idle:
                break;
            case State.Pinging:
                //Eventually do something in the worldspace canvas to indicate to the player that a ping is in place.
                //if (worldSpaceCanvas.transform.childCount == 0)
                {
                    currentState = State.Idle;
                }
                break;
            default:
                break;
        }
    }

    override public void Interact()
    {
    }

    override public void Interact(GameObject otherActor)
    {
        if (otherActor.tag == "Player")
        {

			PlayerController player = otherActor.GetComponent<PlayerController> ();

            if (currentState == State.Idle)
            {
                GameObject pingObject = (GameObject)Instantiate(pingObjectTemplate);
				pingObject.transform.SetParent(player.panel.GetRectTransform, false);
                pingObject.name = gameObject.ToString();
                if (subNavController.CurrentState == SubNavigationStationController.State.Empty)
                {
					pingObject.GetComponent<NavigationPingController>().Init(player.panel.GetComponent<RectTransform> (), transform.position, player);
                }
                else if (subNavController.CurrentState == SubNavigationStationController.State.Holding)
                {
                    if (subNavController.HeldObject != null)
                    {
                        FishController fishController = subNavController.HeldObject.GetComponent<FishController>();
                        if (fishController != null)
                        {
							pingObject.GetComponent<NavigationPingController>().Init(player.panel.GetComponent<RectTransform> (), fishController.CaughtPosition, player);
                        }
                    }
                }
                currentState = State.Pinging;
            }
        }
    }

    override public void ToggleHighlight(PlayerController otherPlayerController, bool toggle = true)
    {
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ResearchStationController : MonoBehaviour, IInteractable {

    enum State {
        Empty,
        Holding,
        Working
    };

    State currentState;

    public string researchStationType;

    private GameObject holdSlot;
    private GameObject heldObject;
    private float maxProgress = 20f;
    private float progressMade = 0f;
    private float progressPerInteraction = 1f;
    private GameObject mainCamera;
    private GameObject gameCanvas;

    //Static so only one instance is used.
    private GameObject uiObject;

    //Prefab to instantiate progress ui
    public GameObject progressUI;

    // Use this for initialization
    void Start () {
        currentState = State.Empty;
        holdSlot = gameObject.transform.Find("HoldSlot").gameObject;
        heldObject = null;
        uiObject = null;

        mainCamera = Camera.main.gameObject;
        gameCanvas = mainCamera.gameObject.transform.Find("SuperImposedUI").gameObject;
    }

    // Update is called once per frame
    void Update() {
        if (uiObject != null)
        {
            GameObject words = uiObject.transform.GetChild(0).gameObject;
            words.GetComponent<Text>().text = progressMade.ToString() + "/" + maxProgress.ToString();

            if (uiObject.name == gameObject.ToString())
            {
                Vector2 ViewportPosition = Camera.main.WorldToScreenPoint(gameObject.transform.position);
                uiObject.GetComponent<RectTransform>().anchoredPosition = ViewportPosition;

            }
        }
    }

    public void interact()
    {
    }

    public void interact(GameObject otherActor)
    {
        if (otherActor.tag == "Player")
        {
            if (currentState == State.Empty)
            {
                PlayerInteractionController playerControllerScript = (PlayerInteractionController)otherActor.GetComponent(typeof(PlayerInteractionController));
                if (playerControllerScript != null)
                {
                    //Get the object held by the player.
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
                    //Check that the player is not already holding on to something.
                    if (playerControllerScript.getHeldObject() == null)
                    {
                        if (heldObject != null)
                        {
                            currentState = State.Working;
                        }
                    }
                }
            }
            else if (currentState == State.Working)
            {
                PlayerInteractionController playerControllerScript = (PlayerInteractionController)otherActor.GetComponent(typeof(PlayerInteractionController));
                if (playerControllerScript != null)
                {
                    if (progressMade < maxProgress)
                    {
                        progressMade += progressPerInteraction;
                        print(progressMade);
                    }
                    //Check that the player is not already holding on to something.
                    else if (playerControllerScript.getHeldObject() == null)
                    {
                        if (heldObject != null)
                        {
                            FishController heldObjectControllerScript = (FishController)heldObject.GetComponent(typeof(FishController));
                            if (heldObjectControllerScript.getCurrentResearchProtocol() == researchStationType)
                            {
                                heldObjectControllerScript.researchFish();
                            }
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
            rend.material.color = Color.blue;
            if (uiObject == null)
            {
                uiObject = (GameObject)Instantiate(progressUI);
                GameObject words = uiObject.transform.GetChild(0).gameObject;
                words.GetComponent<Text>().text = progressMade.ToString() + "/" + maxProgress.ToString();
                uiObject.transform.SetParent(gameCanvas.transform, false);
                uiObject.name = gameObject.ToString();
            }
        }
        else
        {
            Renderer rend = GetComponent<Renderer>();
            rend.material.color = Color.white;
            if (uiObject != null)
            {
                if (uiObject.name == gameObject.ToString())
                {
                    Destroy(uiObject);
                    uiObject = null;
                    progressMade = 0;
                }
            }
        }
    }

}

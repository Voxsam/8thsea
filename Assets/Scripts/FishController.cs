using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FishController : MonoBehaviour, IInteractable {
    
    //Enums for fish states
    enum State
    {
        Idle,
        Held,
        Placed
    };
    State currentState;

    //Boolean flag to mark fish as within interactable range of a player.
    private Rigidbody rigidBody;

    private GameObject mainCamera;

    private GameObject gameCanvas;
    private RectTransform canvasRect;

    //Static so only one instance is used.
    private GameObject uiObject;

    //Prefab to instantiate FishDetails
    public GameObject fishDetails;

    // Use this for initialization
    void Start () {
        currentState = State.Idle;
        //Get own rigidbody component.
        rigidBody = this.GetComponent<Rigidbody>();

        uiObject = null;

        mainCamera = Camera.main.gameObject;
        gameCanvas = mainCamera.gameObject.transform.Find("SuperImposedUI").gameObject;
        canvasRect = gameCanvas.GetComponent<RectTransform>();
    }
	
	// Update is called once per frame
	void Update () {
	    if (uiObject != null)
        {
            if (uiObject.name == gameObject.ToString())
            {
                Vector2 ViewportPosition = Camera.main.WorldToScreenPoint(gameObject.transform.position);
                uiObject.GetComponent<RectTransform>().anchoredPosition = ViewportPosition;
            }
        }
	}

    //Activates/deactivates RigidBody of prefab.
    public void ToggleRigidBody ()
    {
        if (rigidBody)
        {
            rigidBody.isKinematic = !rigidBody.isKinematic;
        }
    }

    public void ToggleDetectCollisions()
    {
        if (rigidBody)
        {
            rigidBody.detectCollisions = !rigidBody.detectCollisions;
        }
    }

    public void pickUp ()
    {
        if (rigidBody)
        {
            rigidBody.isKinematic = true;
            rigidBody.detectCollisions = false;
        }
        currentState = State.Held;
    }

    public void putDown ()
    {
        if (rigidBody)
        {
            rigidBody.isKinematic = false;
            rigidBody.detectCollisions = true;
        }
        currentState = State.Idle;
    }

    public void putIn ()
    {
        if (rigidBody)
        {
            rigidBody.isKinematic = true;
            rigidBody.detectCollisions = false;
        }

        currentState = State.Placed;
    }

    public void interact ()
    {
        switch (currentState)
        {
            case State.Idle:
                pickUp();
                break;

            case State.Held:
                putDown();
                break;

            case State.Placed:
                pickUp();
                break;

            default:
                break;
        }
    }

    public void interact (GameObject otherActor)
    {
    }

    public void toggleHighlight(bool toggle = true)
    {
        if (toggle)
        {
            Renderer rend = GetComponent<Renderer>();
            rend.material.color = Color.red;

            if (uiObject == null)
            {
                uiObject = (GameObject)Instantiate(fishDetails);
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
                }
            }
        }
    }
}

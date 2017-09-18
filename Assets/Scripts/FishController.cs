using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FishController : MonoBehaviour, IInteractable {

    //Enums for fish states
    public enum State
    {
        Idle,
        Held,
        Placed
    };
    State currentState;

    //Enums for fish secondary states
    public enum SecondaryState
    {
        Idle,
        Dead,
        Researched
    };
    SecondaryState currentSecondaryState;

    //Boolean flag to mark fish as within interactable range of a player.
    private Rigidbody rigidBody;

    private GameObject mainCamera;

    private GameObject gameCanvas;
    private RectTransform canvasRect;

    private GameObject worldspaceCanvas;
    private GameObject panicBar;
    private RectTransform panicBarRect;
    private float panicTimer;
    private float panicBarWidth;
    private GameObject fishDetailsUIObject;

    //Prefab to instantiate FishDetails
    public GameObject fishDetails;

    // Use this for initialization
    void Start () {
        currentState = State.Idle;
        currentSecondaryState = SecondaryState.Idle;
        //Get own rigidbody component.
        rigidBody = this.GetComponent<Rigidbody>();

        fishDetailsUIObject = null;

        mainCamera = Camera.main.gameObject;
        gameCanvas = mainCamera.gameObject.transform.Find("SuperImposedUI").gameObject;
        canvasRect = gameCanvas.GetComponent<RectTransform>();

        worldspaceCanvas = gameObject.transform.Find("WorldspaceCanvas").gameObject;
        worldspaceCanvas.transform.Find("DeadText").gameObject.SetActive(false);
        panicBar = worldspaceCanvas.transform.Find("PanicBar").gameObject;
        panicBarRect = panicBar.GetComponent<RectTransform>();
        panicBarWidth = panicBarRect.rect.width;
        panicTimer = 10f;
    }
	
	// Update is called once per frame
	void Update () {
        switch (currentSecondaryState)
        {
            case SecondaryState.Idle:
                panicTimer -= Time.deltaTime;

                if (panicTimer <= 0)
                {
                    currentSecondaryState = SecondaryState.Dead;
                    panicTimer = 0f;
                    worldspaceCanvas.transform.Find("DeadText").gameObject.SetActive(true);
                }

                panicBarRect.sizeDelta = new Vector2(panicBarWidth * panicTimer / 10f, panicBarRect.rect.height);

                break;

            case SecondaryState.Dead:
                break;

            case SecondaryState.Researched:
                break;

            default:
                break;
        }
	}

    void LateUpdate()
    {
        worldspaceCanvas.transform.rotation = Quaternion.identity;
        worldspaceCanvas.transform.position = transform.position + new Vector3 (0, 1, 0);
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

            if (fishDetailsUIObject == null)
            {
                fishDetailsUIObject = (GameObject)Instantiate(fishDetails);
                fishDetailsUIObject.transform.SetParent(gameCanvas.transform, false);
                fishDetailsUIObject.name = gameObject.ToString();
                fishDetailsUIObject.GetComponent<FishDetailsController>().Init(0, gameObject);
            }
        }
        else
        {
            Renderer rend = GetComponent<Renderer>();
            rend.material.color = Color.white;
            if (fishDetailsUIObject != null)
            {
                if (fishDetailsUIObject.name == gameObject.ToString())
                {
                    Destroy(fishDetailsUIObject);
                    fishDetailsUIObject = null;
                }
            }
        }
    }
}

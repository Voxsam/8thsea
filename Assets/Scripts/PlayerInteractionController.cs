using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInteractionController : MonoBehaviour
{
    public enum State
    {
        Idle,
        PickingUp,
        Hold
    };
    enum SecondaryState
    {
        Idle,
        Picking_up,
        View
    };

    public const string PICK_UP = "pickUp";
    public const string DROP = "drop";

    //public const float PICK_UP_ANIMATION_TOTAL_TIME_TAKEN = 0.729f;
    public const float PICK_UP_ANIMATION_START_DELAY = 0.3f;
    public const float PICK_UP_ANIMATION_TIME = 0.5f;

    public const float DROP_ANIMATION_START_DELAY = 0.1f;
    public const float DROP_ANIMATION_TIME = 0.5f;


    State currentState;
    SecondaryState currentSecondaryState;

    [SerializeField] private PlayerController player;
    [SerializeField] private GameObject holdSlot;

    private Animator anim;
    private GameObject atObject;
    private GameObject heldObject;
    public GameObject GetHeldObject()
    {
        return heldObject;
    }
    public State GetCurrentState()
    {
        return currentState;
    }
    // Use this for initialization
    void Start()
    {
        currentState = State.Idle;
        currentSecondaryState = SecondaryState.Idle;
        anim = GetComponentInChildren<Animator>();
        // Set in the editor
        //player = gameObject;
        //player = FindComponent<PlayerController>();
        //holdSlot = gameObject.transform.Find("HoldSlot").gameObject;
        heldObject = null;
    }

    void OnTriggerStay(Collider other)
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

    void OnTriggerExit(Collider other)
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
    public void GameUpdate()
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
            case State.PickingUp:
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
                        if (tutorialController.currentTutorialStep == 7)
                        {
                            tutorialController.isNext = false;
                            tutorialController.next();
                        }
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

    private void HighlightObject(GameObject other, bool highlight)
    {
        IInteractable otherInteractableScript = (IInteractable)other.GetComponent(typeof(IInteractable));
        if (otherInteractableScript != null)
        {
            otherInteractableScript.ToggleHighlight(highlight);
        }
    }

    private void SetAttachObject(GameObject other, bool attach)
    {
        IInteractable heldObjectInteractableScript = (IInteractable)other.GetComponent(typeof(IInteractable));
        if (heldObjectInteractableScript != null)
        {
            heldObjectInteractableScript.Interact();
            heldObjectInteractableScript.ToggleHighlight(false);
        }

        if (attach)
        {
            // The prereq to activate Pick up Trigger
            player.IsPlayerAllowedToMove = false;
            player.IsPlayerMoving = false;

            StartCoroutine(GameController.ActivateCallbackAfterDelayCoroutine(Time.deltaTime, () =>
            {
                // Wait one frame before allowing the pick up
                anim.SetTrigger(PICK_UP);
                currentState = State.PickingUp;
                other.transform.SetParent(holdSlot.transform, true);

                StartCoroutine(GameController.ActivateCallbackAfterDelayCoroutine(PICK_UP_ANIMATION_START_DELAY, () =>
                {
                    StartCoroutine(GameController.MoveGameObjectAndActivateCallbackCoroutine(other.transform, PICK_UP_ANIMATION_TIME, Vector3.zero, true, () =>
                    {
                        // Activate the pick up after the animation is complete
                        currentState = State.Hold;
                        heldObject = other;
                        player.IsPlayerAllowedToMove = true;
                    }));
                }));
            }));
        }
        else
        {
            // The prereq to drop (since after drop it moves straight to idle animation)
            player.IsPlayerAllowedToMove = false;
            player.IsPlayerMoving = false;
            StartCoroutine(GameController.ActivateCallbackAfterDelayCoroutine(Time.deltaTime, () =>
            {
                anim.SetTrigger(DROP);
                other.transform.SetParent(player.LocationRef, true);
                heldObject = null;
                
                // Delay again for the animation to finish before letting the player move again
                StartCoroutine(GameController.ActivateCallbackAfterDelayCoroutine(DROP_ANIMATION_TIME, () =>
                {
                    currentState = State.Idle;
                    player.IsPlayerAllowedToMove = true;
                }));
            }));
        }
    }

    public void PickUpObject(GameObject other)
    {
        if (GetCurrentState() != State.PickingUp )
        {
            SetAttachObject(other, true);
            currentSecondaryState = SecondaryState.Idle;
        }
    }

    public void DropObject()
    {
        if (GetCurrentState() != State.PickingUp)
        {
            SetAttachObject(heldObject, false);
            heldObject = null;
            currentSecondaryState = SecondaryState.Idle;
        }
    }
}
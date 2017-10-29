using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisposalStationController : IInteractable
{
    public enum State
    {
        Idle,
        Disposing
    }
    private State currentState;

    [SerializeField] private AnimationCurve speedCurve;
    [SerializeField] private Transform disposalPoint;

    private Vector3 teleportDirection;
    private float teleportDuration;
    private float teleportDistance;
    private float currentTime;
    private FishController fishController;

    public void Start()
    {
        currentState = State.Idle;
    }

    public void Update()
    {
        switch (currentState)
        {
            case State.Idle:
                break;
            case State.Disposing:
                if (currentTime < teleportDuration)
                {
                    fishController.gameObject.transform.position = Vector3.Lerp
                    (
                        fishController.gameObject.transform.position,
                        transform.position + (speedCurve.Evaluate(currentTime) * teleportDistance) * teleportDirection,
                        currentTime
                    );
                    fishController.transform.Rotate(Vector3.up * Time.deltaTime * 540);
                    fishController.transform.localScale = new Vector3(GameData.QuadEaseOut(currentTime, 1, -1f, teleportDuration),
                                                                GameData.QuadEaseOut(currentTime, 1, -1f, teleportDuration),
                                                                GameData.QuadEaseOut(currentTime, 1, -1f, teleportDuration));
                    currentTime += Time.deltaTime;
                }
                else
                {
                    currentState = State.Idle;
                    currentTime = 0f;
                    Destroy (fishController.gameObject);
                    fishController = null;
                }
                break;
        }
    }

    public override void ToggleHighlight(PlayerController otherPlayerController, bool toggle)
    {

    }

    public override void Interact()
    {

    }

    public override void Interact(GameObject otherActor)
    {
        if (otherActor.tag == "Player")
        {
            if (currentState == State.Idle)
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
                        }

                        fishController = heldObjectControllerScript;
                        currentState = State.Disposing;
                        currentTime = 0f;
                        teleportDuration = speedCurve.keys[speedCurve.length - 1].time;
                        teleportDirection = (disposalPoint.position - fishController.gameObject.transform.position).normalized;
                        teleportDistance = Vector3.Distance(disposalPoint.position, fishController.gameObject.transform.position);
                    }
                }
            }
        }
    }
}

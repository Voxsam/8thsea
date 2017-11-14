using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeleportDoor : IInteractable
{
    System.Func<bool> teleportCallback;

    public enum State
    {
        Idle,
        TeleportStart,
        TeleportTransit,
        TeleportEnd
    }
    private State currentState;

    public Transform teleportPoint;
    [SerializeField] private bool allowTeleport = false;

    [SerializeField] private AnimationCurve speedCurve;
    [SerializeField] private float preTeleportDuration;
    [SerializeField] private float teleportMaxSpeed;

    private Transform playerParentTransform;
    private Vector3 teleportDirection;
    private float teleportDistance;
    private float teleportDuration;
    private float currentTime;
    private PlayerController player;

    /// <summary>
    /// A variable that is used to set as the Player's parent, in case the location is moving around. This way, the player moves with the location. Otherwise, the Teleport Point is used
    /// </summary>
    [SerializeField] private Transform PlayerLocationRef;

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
            case State.TeleportStart:
                if (currentTime < preTeleportDuration)
                {
                    player.playerHolder.Rotate(Vector3.up * Time.deltaTime * 540);
                    player.playerHolder.localScale = new Vector3(GameData.QuadEaseOut(currentTime, 1, -1f, preTeleportDuration),
                                                                GameData.QuadEaseOut(currentTime, 1, -1f, preTeleportDuration),
                                                                GameData.QuadEaseOut(currentTime, 1, -1f, preTeleportDuration));
                    currentTime += Time.deltaTime;
                }
                else
                {
                    currentState = State.TeleportTransit;
                    player.ReattachCameraToPlayer();
                    currentTime = 0f;
                }
                break;
            case State.TeleportTransit:
                if (currentTime < teleportDuration)
                {
                    player.gameObject.transform.position = Vector3.Lerp
                    (
                        player.gameObject.transform.position,
                        transform.position + (speedCurve.Evaluate(currentTime) * teleportDistance) * teleportDirection,
                        currentTime
                    );
                    currentTime += Time.deltaTime;
                }
                else
                {
                    player.gameObject.transform.position = teleportPoint.transform.position;
                    player.pCameraController.SetCameraToObject(teleportPoint.gameObject, false);
                    if (PlayerLocationRef != null)
                    {
                        player.transform.SetParent(PlayerLocationRef);
                    }
                    else
                    {
                        player.transform.SetParent(playerParentTransform);
                    }
                    currentState = State.TeleportEnd;
                    currentTime = 0f;
                }
                break;
            case State.TeleportEnd:
                if (currentTime < preTeleportDuration)
                {
                    player.playerHolder.Rotate(Vector3.up * Time.deltaTime * 540);
                    player.playerHolder.localScale = new Vector3(GameData.QuadEaseOut(currentTime, 0, 1f, preTeleportDuration),
                                                                GameData.QuadEaseOut(currentTime, 0, 1f, preTeleportDuration),
                                                                GameData.QuadEaseOut(currentTime, 0, 1f, preTeleportDuration));
                    currentTime += Time.deltaTime;
                }
                else
                {
                    currentState = State.Idle;
                    currentTime = 0f;
                    player.rb.isKinematic = false;
                    player.rb.detectCollisions = true;
                    player.ReturnControlToCharacter();
                    player.ReattachCameraToPlayer();
                }
                break;
        }
    }

    public bool AllowTeleport {
        get {
            // Use the teleportCallback to check for conditions to teleport
            // for when a function is required to verify if the player is allowed to teleport
            if (teleportCallback != null)
            {
                return teleportCallback();
            }
            // Otherwise, let the developer set this setting in the Editor menu
            else
            {
                return allowTeleport;
            }
        }
    }

    // Use this for initialization
    public void Initialise(System.Func<bool> callback)
    {
        teleportCallback = callback;
    }

    /*
    void OnTriggerStay(Collider other)
    {
        PlayerController player = GameController.Obj.GetPlayerFromCollider(other);
        if (player != null && AllowTeleport)
        {
            // If the player is in Character control mode
            if (player.ControlMode == GameData.ControlType.CHARACTER &&
				player.GetPlayerControls.GetActionKeyDown())
            {
                player.transform.position = teleportPoint.transform.position;
                if (PlayerLocationRef != null)
                {
                    player.transform.SetParent(PlayerLocationRef);
                }
                else
                {
                    player.transform.SetParent(teleportPoint);
                }
            }
        }
    }
    */

    public override void ToggleHighlight (PlayerController otherPlayerController, bool toggle)
    {

    }

    public override void Interact()
    {

    }

    public override void Interact(GameObject other)
    {
        //Make sure the teleporter is not currently teleporting a player.
        if (other.tag == "Player" && currentState == State.Idle)
        { 
            player = other.GetComponent<PlayerController>();
            if (player != null && AllowTeleport)
            {

                // If the player is in Character control mode
                if (player.ControlMode == GameData.ControlType.CHARACTER)
                {
                    player.RequestControlChange(GameData.ControlType.STATION);
                    player.rb.isKinematic = true;
                    player.rb.detectCollisions = false;
                    currentState = State.TeleportStart;
                    player.pCameraController.SetCameraToObject(this.gameObject, false);
                    currentTime = 0f;
                    teleportDuration = speedCurve.keys[speedCurve.length - 1].time;
                    teleportDirection = (teleportPoint.position - transform.position).normalized;
                    teleportDistance = Vector3.Distance(teleportPoint.position, transform.position);

                    playerParentTransform = player.transform.parent;

					if (GameController.Obj.isTutorial) {
						TutorialManager.Obj.hasUsedTeleporter = true;
					}


                }
            }
        }
    }
}

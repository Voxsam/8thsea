using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeleportDoor : MonoBehaviour
{
    System.Func<bool> teleportCallback;

    public Transform teleportPoint;
    [SerializeField] private bool allowTeleport = false;

    /// <summary>
    /// A variable that is used to set as the Player's parent, in case the location is moving around. This way, the player moves with the location. Otherwise, the Teleport Point is used
    /// </summary>
    [SerializeField] private Transform PlayerLocationRef; 

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

    void OnTriggerStay(Collider other)
    {
        PlayerController player = GameController.Obj.GetPlayerFromCollider(other);
        if (player != null && AllowTeleport)
        {
            // If the player is in Character control mode
            if (player.ControlMode == GameData.ControlType.CHARACTER &&
                GameController.Obj.ButtonA_Up)
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
}

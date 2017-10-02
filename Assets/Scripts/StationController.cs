using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StationController : MonoBehaviour
{
    public StationControllerInterface controller;

    // Use this for initialization
    void Start()
    {
        gameObject.tag = "lab";
    }

    void OnTriggerStay(Collider other)
    {
        //if (other.gameObject.tag.Equals ("Player")) { // Old code

        // Should try to change this, since OnTriggerStay will be called every loop and might become expensive.
        if (controller.playerInStation == null)
        {
            PlayerController player = GameController.Obj.GetPlayerFromCollider(other);
            if (player != null)
            {
                // If the player is in Character control mode
                if (player.ControlMode == GameData.ControlType.CHARACTER &&
                    GameController.Obj.ButtonA_Down)
                {
                    controller.SetPlayerToStation(player);
                    controller.IsActivated = true;
                    controller.WhenActivated();
                }
            }
        }
        else
        {
            // Free the character from the station if the conditions are met
            if (controller.playerInStation.ControlMode != GameData.ControlType.CHARACTER &&
                controller.SwitchCondition() && GameController.Obj.ButtonB_Down)
            {
                controller.IsActivated = false;
                controller.WhenDeactivated();
                controller.ReleasePlayerFromStation();
            }
        }
    }
}

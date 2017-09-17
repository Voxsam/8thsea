using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SucStationController : MonoBehaviour {
	public TubeController controller;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {

	}

    void OnTriggerStay(Collider other){
        //if (other.gameObject.tag.Equals ("Player")) { // Old code

        // Should try to change this, since OnTriggerStay will be called every loop and might become expensive.
        PlayerController player = GameController.Obj.GetPlayerFromCollider(other); 
        if (player != null)
        {
            // If the player is in Character control mode
            if (player.ControlMode == GameController.ControlType.CHARACTER && 
                GameController.Obj.ButtonA_Up) {
				controller.isActivated = true;
                player.RequestControlChange(GameController.ControlType.STATION);
			}
            // If the character is currently in the station, free the character from the station
            else if (player.ControlMode == GameController.ControlType.STATION &&
                GameController.Obj.ButtonB_Up)
            {
                controller.isActivated = false;
                player.ReturnControlToCharacter();
            }
        }
	}
}

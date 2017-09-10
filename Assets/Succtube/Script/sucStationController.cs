using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class sucStationController : MonoBehaviour {
	public tubeController controller;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {

	}

    void OnTriggerStay(Collider other){
		if (other.gameObject.tag.Equals ("Player")) {
			if (Input.GetKeyUp (KeyCode.Space)) {
				controller.isActivated = true;
			}
            else if (Input.GetKeyUp(KeyCode.E))
            {
                controller.isActivated = false;
            }
        }
	}
}

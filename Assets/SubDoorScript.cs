using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SubDoorScript : MonoBehaviour {

    public GameObject sub;
    public GameObject door;
    private SubmarineController subCtrl;
    private BoxCollider doorCollider;
	// Use this for initialization
	void Start () {
        doorCollider = door.GetComponent<BoxCollider>();
        subCtrl = sub.GetComponent<SubmarineController>();

    }
	
	// Update is called once per frame
	void Update () {
		
	}

    private void OnTriggerEnter(Collider other)
    {
        
    }

    private void OnTriggerStay(Collider other)
    {
        PlayerController player = GameController.Obj.GetPlayerFromCollider(other);
        if (player != null)
        {
            subCtrl.OpenDoorAnim();
            doorCollider.enabled = false;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        subCtrl.CloseDoorAnim();
        doorCollider.enabled = true;
    }
}

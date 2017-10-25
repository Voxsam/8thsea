using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PutPlayerInSub : MonoBehaviour {
    public Transform sub;
    public Transform playerHolder;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
    private void OnTriggerEnter(Collider other)
    {
        PlayerController player = GameController.Obj.GetPlayerFromCollider(other);
        if (player != null)
        {
            player.transform.SetParent(sub);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        PlayerController player = GameController.Obj.GetPlayerFromCollider(other);
        if (player != null)
        {
            player.transform.SetParent(playerHolder);
        }
    }
}

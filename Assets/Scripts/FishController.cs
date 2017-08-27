using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FishController : MonoBehaviour, IInteractable {

    //Boolean flag to mark fish as held/unheld.
    private bool held;
    //Boolean flag to mark fish as within interactable range of a player.
    private Rigidbody rigidBody;

	// Use this for initialization
	void Start () {
        held = false;
        //Get own rigidbody component.
        rigidBody = this.GetComponent<Rigidbody>();
    }
	
	// Update is called once per frame
	void Update () {
	
	}

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            Renderer rend = GetComponent<Renderer>();
            rend.material.color = Color.red;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            Renderer rend = GetComponent<Renderer>();
            rend.material.color = Color.white;
        }
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
    }

    public void putDown ()
    {
        if (rigidBody)
        {
            rigidBody.isKinematic = false;
            rigidBody.detectCollisions = true;
        }
    }

    public void putIn ()
    {
        if (rigidBody)
        {
            rigidBody.isKinematic = true;
            rigidBody.detectCollisions = true;
        }
    }

    public void interact ()
    {
        if (!held)
        {
            held = true;
            pickUp();
        }
        else
        {
            held = false;
            putDown();
        }
    }

    public void interact (GameObject otherActor)
    {
    }
}

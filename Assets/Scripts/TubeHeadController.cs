using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TubeHeadController : MonoBehaviour
{
    public TubeController tubeController;
    
    // Use this for initialization
    void Start()
    {
    }

    /*
    void OnCollisionEnter(Collision other)
    {
        if (tubeController.SystemActivated)
        {
            FishController fish = GameController.Obj.GetFishFromCollider(other.collider);
            if (fish != null)
            {
                tubeController.ExtractFish(fish, other.gameObject);
            }
        }
    }*/

    void OnTriggerStay(Collider other)
    {
        if (tubeController.SystemActivated)
        {
            FishController fish = GameController.Obj.GetFishFromCollider(other);
            if (fish != null && other.attachedRigidbody)
            {
                Vector3 dir = transform.position - other.transform.position;

                if (Vector3.Distance(transform.position, other.transform.position) < 3)
                {
                    tubeController.ExtractFish(fish, other.gameObject);
                }
                else
                {
                    tubeController.MoveFish(dir, other.gameObject);
                }
            }

        }
    }
}

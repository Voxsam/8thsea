using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TubeHeadController : MonoBehaviour
{
    public TubeController tubeController;
    public GameObject tubeHeadGameObject;
    public float minDistance;
    
    // Use this for initialization
    void Start()
    {
        tubeHeadGameObject.tag = "SuccHead";
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
                Vector3 dir = tubeHeadGameObject.transform.position - other.transform.position;

                if (Vector3.Distance(tubeHeadGameObject.transform.position, other.transform.position) < minDistance)
                {
                    tubeController.StartExtraction(fish, other.gameObject);
                }
                else
                {
                    tubeController.MoveFish(dir, other.gameObject);
                }
            }

        }
    }
}

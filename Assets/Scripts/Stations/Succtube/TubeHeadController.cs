using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TubeHeadController : MonoBehaviour
{
    public TubeController tubeController;
    public GameObject tubeHeadGameObject;
    public float minDistance;
    public FishController currentTarget;
    
    // Use this for initialization
    void Start()
    {
        tubeHeadGameObject.tag = "SuccHead";
        currentTarget = null;
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

    void Update()
    {
        if (currentTarget != null)
        {
            if (tubeController.SystemActivated)
            {
                if (Vector3.Distance(tubeHeadGameObject.transform.position, currentTarget.transform.position) < minDistance)
                {
                    tubeController.StartExtraction(currentTarget, currentTarget.gameObject);
                    currentTarget = null;
                }
                else
                {
                    Vector3 dir = tubeHeadGameObject.transform.position - currentTarget.transform.position;
                    tubeController.MoveFish(dir, currentTarget.transform);
                }
            }
            else
            {
                currentTarget = null;
            }
        }
    }

    void OnTriggerStay(Collider other)
    {
        if (tubeController.SystemActivated && currentTarget == null)
        {
            FishController fish = GameController.Obj.GetFishFromCollider(other);
            if (fish != null && other.attachedRigidbody)
            {
                currentTarget = fish;
            }
        }
    }
}

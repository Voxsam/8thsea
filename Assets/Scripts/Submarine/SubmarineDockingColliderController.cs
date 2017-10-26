using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SubmarineDockingColliderController : MonoBehaviour {
    public SubmarineController submarineController;

    // Use this for initialization
    void Start()
    {
    }

    void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Submarine") && submarineController.IsActivated)
        {
            submarineController.WithinDock = true;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Submarine") && submarineController.IsActivated)
        {
            submarineController.WithinDock = false;
        }
    }
}

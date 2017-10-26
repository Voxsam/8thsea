using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TubeHeadColliderController : MonoBehaviour {
    public TubeController tubeController;
    public float reboundForce;
    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("lab") || other.CompareTag("Submarine"))
        {
            Vector3 direction = transform.position - other.gameObject.transform.position;
            direction = direction.normalized;
            tubeController.MoveInDirection(direction * reboundForce);
        }
    }
}

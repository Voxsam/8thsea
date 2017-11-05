using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SubmarineColliderController : MonoBehaviour {
    public SubmarineController subController;
    public float reboundForce;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("lab"))
        {
            Vector3 direction = transform.position - other.gameObject.GetComponent<Collider>().ClosestPointOnBounds(transform.position);
            direction = direction.normalized;
            subController.MoveInDirection(direction * reboundForce);
            Debug.DrawLine(other.transform.position, other.gameObject.GetComponent<Collider>().ClosestPointOnBounds(transform.position), Color.red, 3);
        }
        if (other.CompareTag("seabed"))
        {
            print("lol fuck la");
        }
    }
}

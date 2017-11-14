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
            //Position the anchor point as the closest point on the submarine mesh to the position of the succ head.
            Vector3 pointOnBounding = other.ClosestPointOnBounds(transform.position);
            Ray ray = new Ray(pointOnBounding, (other.transform.position - pointOnBounding).normalized);
            RaycastHit hit;
            Vector3 direction = Vector3.zero;
            if (other.Raycast(ray, out hit, 100.0F))
            {
                direction = transform.position - hit.point;
                Debug.DrawLine(transform.position, hit.point, Color.red, 3f);
            }
            
            direction = direction.normalized;
            subController.MoveInDirection(direction * reboundForce);
        }
        if (other.CompareTag("seabed"))
        {
            print("lol fuck la");
        }
    }
}

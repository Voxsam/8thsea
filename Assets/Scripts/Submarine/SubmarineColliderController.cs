using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SubmarineColliderController : MonoBehaviour {
    public SubmarineController subController;
    public float reboundForce;

    public float minimumWaitTimeBetweenSFX = 0f; // seconds
    public float maximumWaitTimeBetweenSFX = 0.2f; // seconds
    private float timeToWaitUntil = 0f;
    private bool allowToPlaySFX = true;

    private float GetWaitTimeBetweenSFX()
    {
        float multiplier = (float)GameController.RNG.NextDouble();
        return minimumWaitTimeBetweenSFX + multiplier * (maximumWaitTimeBetweenSFX - minimumWaitTimeBetweenSFX);
    }

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if (!allowToPlaySFX)
        {
            if (Time.time > timeToWaitUntil)
            {
                allowToPlaySFX = true;
            }
        }
	}

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("lab"))
        {
            // Play sound
            if (allowToPlaySFX)
            {
                allowToPlaySFX = false;
                GameController.Audio.PlaySFXOnce(AudioController.SoundEffect.Scrap);
                timeToWaitUntil = Time.time + GetWaitTimeBetweenSFX();
            }

            //Position the anchor point as the closest point on the submarine mesh to the position of the succ head.
            Vector3 pointOnBounding = other.ClosestPointOnBounds(transform.position);
            Ray ray = new Ray(pointOnBounding, (other.transform.position - pointOnBounding).normalized);
            RaycastHit hit;
            Vector3 direction = Vector3.zero;
            if (other.Raycast(ray, out hit, 100.0F))
            {
                direction = transform.position - hit.point;
            }
            direction = direction.normalized;
            subController.MoveInDirection(direction * reboundForce);
        }
        else if (other.CompareTag("seabed"))
        {
            Vector3 pointOnBounding = other.ClosestPointOnBounds(transform.position);
            Vector3 direction = (transform.position - pointOnBounding).normalized;
            subController.MoveInDirection(direction * reboundForce);
        }
    }
}

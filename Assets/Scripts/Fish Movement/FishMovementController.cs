﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FishMovementController : MonoBehaviour {

    private FishSchoolController fishSchoolController;

    private float minSpeed;
    private float maxSpeed;

    private float minRotationSpeed;
    private float maxRotationSpeed;

    //Control how often the schooling logic is applied to govern movement.
    //E.g. every frame there is a 1/5 chance the fish moves according to the schooling logic.
    private int schoolingRateNumerator = 1;
    private int schoolingRateDenominator = 5;

    private float speed;
    private float rotationSpeed;

    private Vector3 averageHeading;
    private Vector3 averageLocation;
    private float minNeighbourDistance;

    //Variables to prevent fish from swimming into colliders.
    private bool outOfBounds = false;
    private bool turning = false;
    private Vector3 avoidanceGoalLocation;

    private Quaternion randomRotation;

	// Use this for initialization
	void Start () {
        speed = Random.Range(minSpeed, maxSpeed);
        rotationSpeed = Random.Range(minRotationSpeed, maxRotationSpeed);
        randomRotation = Random.rotation;
	}
	
	// Update is called once per frame
	void Update () {
        if (fishSchoolController.zoneName.Length > 0)
        {
            if (Vector3.Distance(this.transform.position, fishSchoolController.gameObject.transform.position) > fishSchoolController.AverageBounds)
            {
                outOfBounds = true;
            }
        }
        
        if (turning)
        {
            Vector3 finalDirection = avoidanceGoalLocation.normalized;
            if (finalDirection != Vector3.zero)
            {
                this.transform.rotation = Quaternion.Slerp
                (
                    this.transform.rotation,
                    Quaternion.LookRotation(finalDirection),
                    rotationSpeed * 5 * Time.deltaTime
                );

                if (Mathf.Abs(Quaternion.Angle(this.transform.rotation, Quaternion.LookRotation(finalDirection))) < 0.2f)
                {
                    turning = false;
                    speed = Random.Range(minSpeed, maxSpeed);
                }
            }
        }
        else
        {
            if (outOfBounds)
            {
                this.transform.rotation = Quaternion.Slerp
                (
                    this.transform.rotation,
                    Quaternion.LookRotation((fishSchoolController.GoalLocation - this.transform.position)),
                    rotationSpeed * Time.deltaTime
                );

                if (Vector3.Distance(fishSchoolController.GoalLocation, this.transform.position) <= minNeighbourDistance)
                {
                    outOfBounds = false;
                }
            }
            else
            {
                if (Random.Range(0, schoolingRateDenominator) < schoolingRateNumerator)
                {
                    if (fishSchoolController.FishInSchool.Count == 1)
                    {
                        this.transform.rotation = Quaternion.Slerp
                        (
                            this.transform.rotation,
                            Quaternion.LookRotation((fishSchoolController.GoalLocation - this.transform.position)),
                            rotationSpeed * Time.deltaTime
                        );

                        if (Vector3.Distance(this.transform.position, fishSchoolController.gameObject.transform.position) > fishSchoolController.AverageBounds)
                        {
                            speed = Random.Range(minSpeed, maxSpeed);
                        }
                    }
                    else
                    {
                        School();
                    }
                }
            }
        }
        transform.position = Vector3.Lerp(transform.position, transform.position + (transform.forward * speed), Time.deltaTime);
        //transform.Translate(0, 0, speed * Time.deltaTime);
	}

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag != "FishObject" && other.tag != "Zone")
        {
            if (!turning)
            {
                //avoidanceGoalLocation = this.transform.position - other.gameObject.transform.position;
                avoidanceGoalLocation = -this.transform.forward;
            }
            turning = true;

            if (other.tag == "Submarine")
            {
                speed = maxSpeed * 5;
            }
            else
                speed = Random.Range(minSpeed, maxSpeed);
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.tag == "Submarine")
        {
            avoidanceGoalLocation = transform.position - other.transform.position;
            speed = maxSpeed * 5;
            turning = true;
        }
        if (other.tag == "lab")
        {
            //Make sure that the fish belong to a school that is in a zone and not the schools in the aquarium.
            if (fishSchoolController.zoneName.Length > 0)
            {
                avoidanceGoalLocation = transform.position - other.transform.position;
                speed = maxSpeed * 5;
                turning = true;
            }
        }
    }

    /*
    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Zone" && other.gameObject.name == fishSchoolController.zoneName)
        {
            outOfBounds = true;
        }
        //if (other.tag != "FishObject")
            //turning = false;
    }
    */

    private void School ()
    {
        if (fishSchoolController != null)
        {
            List<GameObject> fishInSchool = fishSchoolController.FishInSchool;
            Vector3 centralVector = Vector3.zero;
            Vector3 avoidanceVector = Vector3.zero;
            Vector3 goalLocation = fishSchoolController.GoalLocation;
            float groupSpeed = 0.1f;
            float distanceToOther;

            int groupSize = 0;
            foreach ( GameObject otherFish in fishInSchool )
            {
                if (otherFish != this.gameObject)
                {
                    distanceToOther = Vector3.Distance(otherFish.transform.position, this.transform.position);
                    if (distanceToOther <= minNeighbourDistance)
                    {
                        centralVector += otherFish.transform.position;
                        groupSize++;

                        if (distanceToOther < 1.0f)
                        {
                            avoidanceVector = avoidanceVector + (this.transform.position - otherFish.transform.position);
                        }
                        FishMovementController otherFishMovementController = otherFish.GetComponent<FishMovementController>();
                        if (otherFishMovementController != null)
                        {
                            groupSpeed = groupSpeed + otherFishMovementController.Speed;
                        }
                    }
                }
            }
            if ( groupSize > 0 )
            {
                centralVector = centralVector / groupSize + (goalLocation - this.transform.position);
                speed = groupSpeed / groupSize;

                Vector3 finalDirection = (centralVector + avoidanceVector) - this.transform.position;

                if (finalDirection != Vector3.zero)
                {
                    this.transform.rotation = Quaternion.Slerp
                    (
                        this.transform.rotation,
                        Quaternion.LookRotation(finalDirection),
                        rotationSpeed * Time.deltaTime
                    );
                }
            }
            else
            {
                this.transform.rotation = Quaternion.Slerp
                (
                    this.transform.rotation,
                    randomRotation,
                    rotationSpeed * Time.deltaTime
                );

                if (Mathf.Abs (Quaternion.Angle(this.transform.rotation, randomRotation)) < 0.2f)
                {
                    randomRotation = Random.rotation;
                }
            }
        }
    }

    public void SetEnabled (bool enable)
    {
        if (!enable)
        {
            Rigidbody rigidBody = gameObject.GetComponent<Rigidbody>();
            if (rigidBody)
            {
                rigidBody.isKinematic = false;
                rigidBody.useGravity = true;
            }
            this.enabled = false;
        }
        else
        {
            Rigidbody rigidBody = gameObject.GetComponent<Rigidbody>();
            if (rigidBody)
            {
                rigidBody.isKinematic = true;
                rigidBody.useGravity = false;
            }
            this.enabled = true;
        }
    }

    #region Init and Getters/Setters

    public void Initialise(float _minSpeed = 1f, float _maxSpeed = 3f, float _minRotationSpeed = 1.0f, float _maxRotationSpeed = 4.0f, float _minNeighbourDistance = 15.0f )
    {
        minSpeed = _minSpeed;
        maxSpeed = _maxSpeed;

        minRotationSpeed = _minRotationSpeed;
        maxRotationSpeed = _maxRotationSpeed;

        minNeighbourDistance = _minNeighbourDistance;

        speed = Random.Range(minSpeed, maxSpeed);
        rotationSpeed = Random.Range(minRotationSpeed, maxRotationSpeed);
    }

    public float Speed
    {
        get
        {
            return speed;
        }
    }

    public FishSchoolController FishSchoolController
    {
        get
        {
            return fishSchoolController;
        }
        set
        {
            fishSchoolController = value;
        }
    }
    #endregion
}

﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FishSchoolController : MonoBehaviour {

    protected List<GameObject> fishInSchool;

    private Vector3 fishSchoolGoalLocation;

    //Note that these diameters should be half the size of the actual zone diameters.
    public int zoneWidth;
    public int zoneHeight;
    public int zoneLength;

    //Control how often the goal location of the school changes.
    //E.g. every frame there is a 50/10000 chance the goal location changes.
    private int goalLocationRefreshRateNumerator = 50;
    private int goalLocationRefreshRateDenominator = 10000;

    // Use this for initialization
    void Awake () {
        fishSchoolGoalLocation = Vector3.zero;
        fishInSchool = new List<GameObject>();
    }

    void Start ()
    {
        testInit();
        fishSchoolGoalLocation = new Vector3
        (
            transform.position.x + Random.Range(-zoneWidth, zoneWidth),
            transform.position.y + Random.Range(-zoneHeight, zoneHeight),
            transform.position.z + Random.Range(-zoneLength, zoneLength)
        );
    }
	
	// Update is called once per frame
	void Update () {
		if (Random.Range(0, goalLocationRefreshRateDenominator) < goalLocationRefreshRateNumerator)
        {
            fishSchoolGoalLocation = new Vector3
            (
                transform.position.x + Random.Range ( -zoneWidth, zoneWidth ),
                transform.position.y + Random.Range ( -zoneHeight, zoneHeight ),
                transform.position.z + Random.Range ( -zoneLength, zoneLength )
            );
        }
	}

    public void AddFishToSchool (GameObject fish)
    {
        FishMovementController fishMovementScript = (FishMovementController)fish.GetComponent(typeof(FishMovementController));
        if ( fishMovementScript != null )
        {
            fishMovementScript.FishSchoolController = this;
            fishInSchool.Add(fish);
        }
    }

    public GameObject RemoveFishFromSchool (int index)
    {
        GameObject fish = null;
        if (index >= 0 && index < fishInSchool.Count)
        {
            fish = fishInSchool[index];
            if (fish != null)
            {
                FishMovementController fishMovementScript = (FishMovementController)fish.GetComponent(typeof(FishMovementController));
                if (fishMovementScript != null)
                {
                    fishInSchool.RemoveAt(index);
                }
            }
        }
        return fish;
    }

    #region TEST
    //public GameObject testFish;
    public GameObject pointer;
    private GameObject thisPointer;

    public void testInit()
    {
        thisPointer = (GameObject)Instantiate(pointer);
        /*for (int i = 0; i < 5; i++)
        {
            GameObject newFish = (GameObject)Instantiate(testFish);
            FishMovementController newFishMoveControl = newFish.GetComponent<FishMovementController>();
            newFish.transform.position = this.transform.position + new Vector3 ( Random.Range (-2, 2), Random.Range (-2, 2), Random.Range (-2, 2) );
            newFishMoveControl.Initialise();
            AddFishToSchool(newFish);
        }
        */
    }

    private void LateUpdate()
    {
        thisPointer.transform.position = fishSchoolGoalLocation;
    }
    #endregion

    #region Inits, Getters and Setters
    public List<GameObject> FishInSchool
    {
        get
        {
            return fishInSchool;
        }
    }

    public Vector3 GoalLocation
    {
        get
        {
            return fishSchoolGoalLocation;
        }
    }
    #endregion
}

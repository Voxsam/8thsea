using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PersistentData : MonoBehaviour {

    public static PersistentData Obj;

    public class AquariumSchoolData
    {
        public GameData.FishType fishType;
        public int numberOfFishes;

        public AquariumSchoolData ()
        {
            numberOfFishes = 0;
        }
    }
    public List<AquariumSchoolData> savedSchools;

    void Awake()
    {
        if (Obj == null)
        {
            savedSchools = new List<AquariumSchoolData>();
            Obj = this;
        }
        else
        {
            Destroy(this.gameObject);
        }

        // GameController object should not be destructable
        DontDestroyOnLoad(this.gameObject);
    }

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void AddSchool (FishSchoolController schoolController)
    {
        AquariumSchoolData newData = new AquariumSchoolData();

        foreach ( GameObject fish in schoolController.FishInSchool )
        {
            FishController fishController = fish.GetComponent<FishController>();
            if ( fishController != null )
            {
                newData.numberOfFishes++;
                newData.fishType = fishController.fishType;
            }
        }

        savedSchools.Add(newData);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour {

	public GameObject[] fishes;
    public int maxNumberFish;
	public Vector3 spawnValues;
    public Transform FishHolder;

	public float spawnWait;
	public float spawnMostWait; // Time Increments
	public float spawnLeastWait;
	public int startWait;

	public bool stop;
    public int count;

	int randEnemy; // Picks a Random Enemy

	// Use this for initialization
	void Start ()
	{
        count = 0;
		StartCoroutine (WaitSpawner ());
	}
	
	// Update is called once per frame
	void Update ()
	{
		spawnWait = Random.Range (spawnLeastWait, spawnMostWait);
	}

	IEnumerator WaitSpawner ()
	{
		yield return new WaitForSeconds (startWait);

		while (!stop && count < maxNumberFish)
		{
			randEnemy = Random.Range (0, fishes.Length); // Chooses between 0 and 1

			Vector3 spawnPosition = new Vector3 (Random.Range (-spawnValues.x, spawnValues.x), Random.Range(-spawnValues.y, spawnValues.y), 0);
			FishController fish = Instantiate (fishes [randEnemy], spawnPosition + transform.TransformPoint(0, 0, 0), transform.rotation).GetComponent<FishController>();
            fish.GetComponent<Rigidbody>().useGravity = false;
            count++;
			yield return new WaitForSeconds (spawnWait);
		}
	}
}
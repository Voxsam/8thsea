using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour {

	public GameObject[] fishes;

	public Vector3 spawnValues;

	public float spawnWait;
	public float spawnMostWait; // Time Increments
	public float spawnLeastWait;
	public int startWait;

	public bool stop;

	int randEnemy; // Picks a Random Enemy

	// Use this for initialization
	void Start ()
	{
		StartCoroutine (waitSpawner ());
	}
	
	// Update is called once per frame
	void Update ()
	{
		spawnWait = Random.Range (spawnLeastWait, spawnMostWait);
	}

	IEnumerator waitSpawner ()
	{
		yield return new WaitForSeconds (startWait);

		while (!stop)
		{
			randEnemy = Random.Range (0, 2); // Chooses between 0 and 1

			Vector3 spawnPosition = new Vector3 (Random.Range (-spawnValues.x, spawnValues.x), 1, Random.Range(-spawnValues.z, spawnValues.z));

			Instantiate (fishes [randEnemy], spawnPosition + transform.TransformPoint(0, 0, 0), transform.rotation);

			yield return new WaitForSeconds (spawnWait);
		}
	}
}
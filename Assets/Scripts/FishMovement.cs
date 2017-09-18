using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FishMovement : MonoBehaviour
{
	public bool stop;
	public int time = 2;
 	
	public Vector3 currentPosition;

    public float newPositionX;
    public float newPositionZ;
	public float newPositionY;
	public Vector3 newPosition;
	public Vector3 collidedPosition;

	public bool obstacleCrash = false;
	public bool timeLock = false;

	// Use this for initialization
	void Start ()
	{
		currentPosition = transform.position;
        newPositionX = currentPosition.x + Random.Range(-3.0f, 3.0f);
        newPositionZ = currentPosition.z + Random.Range (-3.0f, 3.0f);
		newPositionY = currentPosition.y + Random.Range (-3.0f, 3.0f);
		newPosition = new Vector3 (1.0f, newPositionY, newPositionZ);

		StartCoroutine(moveToPosition(newPosition, time));
	}
	
	// Update is called once per frame
	void Update ()
	{
	}

	/*void OnGUI ()
	{
		string clashedString = clashed.ToString ();
		GUI.Button(new Rect(10, 10, 50, 50), clashedString);
	}*/

	void OnCollisionEnter (Collision col)
	{
		if(col.gameObject.tag == "Obstacle")
		{
			//Destroy(col.gameObject);
			obstacleCrash = true;
			//Debug.Log ("Crashed " + gameObject.name);
		}

		if (col.gameObject.tag == "Zone")
		{
			Destroy (gameObject);
			Debug.Log (gameObject.name + " has entered " + col.gameObject.name); 
		}
	}

	public IEnumerator moveToPosition(Vector3 newPosition, float time)
	{
		while (!stop)
		{
			float elapsedTime = 0;

			currentPosition = transform.position;

			/*newPositionY = Mathf.Min(Mathf.Max(currentPosition.y + Random.Range (-3.0f, 3.0f),-6.0f), 3.0f);
            newPositionZ = Mathf.Min(Mathf.Max(currentPosition.z + Random.Range(-3.0f, 3.0f),-6.0f),3.0f);
            newPositionX = Mathf.Min(Mathf.Max(currentPosition.x + Random.Range(-3.0f, 3.0f),-6.0f),3.0f);*/

            newPositionY =currentPosition.y + Random.Range(-3.0f, 3.0f);
            newPositionZ = currentPosition.z + Random.Range(-3.0f, 3.0f);
            newPositionX = currentPosition.x + Random.Range(-3.0f, 3.0f);
            newPosition = new Vector3 (newPositionX, newPositionY, newPositionZ);
            Debug.Log(newPosition);
			while (elapsedTime < time)
			{
				if (obstacleCrash == false) 
				{
					transform.position = Vector3.Lerp (currentPosition, newPosition, (elapsedTime / time));
				}

				else
				{
					if (timeLock == false) 
					{
						elapsedTime = 0;
						collidedPosition = transform.position;
						timeLock = true;
					}
						
					transform.position = Vector3.Lerp (collidedPosition, currentPosition, (elapsedTime / time));
				}

				elapsedTime += Time.deltaTime;

				yield return null;
			}

			timeLock = false;
			obstacleCrash = false;
		}
	}
}
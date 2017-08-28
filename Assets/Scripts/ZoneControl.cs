using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZoneControl : MonoBehaviour {

	// Use this for initialization
	void Start () 
	{
		GameObject.Find ("Zone I").transform.localScale = new Vector3(0, 0, 0);
		GameObject.Find ("Zone II").transform.localScale = new Vector3(0, 0, 0);
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	/*void OnCollisionEnter (Collision col)
	{
		if (col.gameObject.tag == "Fish")
		{
			Debug.Log (col.gameObject.name + " has entered " + gameObject.name); 
		}
	}*/
}

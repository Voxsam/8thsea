using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RopeTest2 : MonoBehaviour {

	//public GameObject anchor;
	// Use this for initialization
	void Start () {
		//if (anchor == null)
			//return;
		//gameObject.AddComponent<CharacterJoint>();
		GetComponent<ConfigurableJoint>().connectedBody=transform.parent.GetComponent<Rigidbody>();

	}
	
	// Update is called once per frame
	void Update () {
		
	}
}

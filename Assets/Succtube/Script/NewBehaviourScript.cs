using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewBehaviourScript : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        float x = Input.GetAxis("Horizontal") * Time.deltaTime * 5.0f;
        float y = Input.GetAxis("Vertical") * Time.deltaTime * 5.0f;
        transform.Translate(x, y, 0);
        
    }
}

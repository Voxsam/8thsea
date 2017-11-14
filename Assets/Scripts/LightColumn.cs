using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightColumn : MonoBehaviour {

    public float limit = 300f;
    public float relPos = 0f;
    public float speed = 1f;
    bool goBack = true;
	// Use this for initialization
	void Start () {
		
	}

    // Update is called once per frame
    void Update() {
        if (goBack)
        {
            transform.position = transform.position + new Vector3(speed, 0, 0);
            relPos += speed;
        }
        else
        {
            transform.position = transform.position - new Vector3(speed, 0, 0);
            relPos -= speed;
        }
        if (goBack)
        {
            if (relPos >= limit)
            {
                goBack = false;
            }
        } else
        {
            if (relPos <= 0)
            {
                goBack = true;
            }
        }
    }
}

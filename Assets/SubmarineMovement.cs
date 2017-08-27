﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SubmarineMovement : MonoBehaviour {
    public float acceleration;
    public float currentSpeed;
    public float maximumSpeed;

	// Use this for initialization
	private void Start () {
        acceleration = 0.05f;
        currentSpeed = 0.2f;
        maximumSpeed = 10f;
	}
	
	// Update is called once per frame
	private void Update () {
        float horizontalControl = Input.GetAxis("Horizontal");
        float verticalControl = Input.GetAxis("Vertical");
        transform.Translate(currentSpeed*Time.deltaTime*horizontalControl,currentSpeed*Time.deltaTime*verticalControl,0);
        currentSpeed += acceleration;
        if (currentSpeed > maximumSpeed) {
            currentSpeed = maximumSpeed;
        }
        if (horizontalControl == 0 && verticalControl == 0) {
            currentSpeed = 0.2f;
        }
	}
}

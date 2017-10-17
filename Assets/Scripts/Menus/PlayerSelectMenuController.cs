using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerSelectMenuController : MonoBehaviour {

	public Text p1;
	public Text p2;
	public Text p3;
	public Text p4;

	public MultiplayerManager mm;

	void Update ()
	{

		if (Input.GetKey ("joystick 1 button 8")) {
			print (8);	
		}
		if (Input.GetKey ("joystick 1 button 9")) {
			print (9);	
		}
		if (Input.GetKey ("joystick 1 button 10")) {
			print (10);	
		}
		if (Input.GetKey ("joystick 1 button 11")) {
			print (11);	
		}


	}

}

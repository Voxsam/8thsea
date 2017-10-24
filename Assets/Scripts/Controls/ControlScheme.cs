using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControlScheme {

	public bool isKeyboard;
	public int playerNumber;
	public int joystickNumber;

	public ControlScheme(int joystickNumber, bool isKeyboard, KeyCode upKey) {

		this.isKeyboard = isKeyboard;

		if (!isKeyboard) {
			
			this.joystickNumber = joystickNumber;
		
		} else {
			
			switch (upKey) {
				
			case KeyCode.W:
				this.joystickNumber = 1;
				break;

			case KeyCode.UpArrow:
				this.joystickNumber = 2;
				break;

			default: 
				this.joystickNumber = 1;
				break;

			}
		}

	}

	public float GetHorizontalAxis() {
		if (!isKeyboard) {
			return Input.GetAxis ("Horizontal_J" + joystickNumber);
		} else {
			return Input.GetAxis ("Horizontal_K" + joystickNumber);
		}
	}


	public float GetVerticalAxis() {
		if (!isKeyboard) {
			return Input.GetAxis ("Vertical_J" + joystickNumber);
		} else {
			return Input.GetAxis ("Vertical_K" + joystickNumber);
		}
	}

	// Action on joystick = A button
	// For WASD = E button
	// For arrow keys = Enter key
	public bool GetActionKeyDown() {
		if (!isKeyboard) {
			return Input.GetKeyDown ("joystick " + joystickNumber + " button 0");
		} else {
			switch (joystickNumber) {
			case 1:
				return Input.GetKeyDown (KeyCode.E);
			case 2:
				return Input.GetKeyDown (KeyCode.Return);
			default:
				return Input.GetKeyDown (KeyCode.E);
			}
		}
	}

	// Cancel = B button
	// For WASD = R
	// For arrow keys = right shift
	public bool GetCancelKeyDown() {
		if (!isKeyboard) {
			return Input.GetKeyDown ("joystick " + joystickNumber + " button 1");
		} else {
			switch (joystickNumber) {
			case 1:
				return Input.GetKeyDown (KeyCode.R);
			case 2:
				return Input.GetKeyDown (KeyCode.RightShift);
			default:
				return Input.GetKeyDown (KeyCode.R);
			}
		}
	}
}

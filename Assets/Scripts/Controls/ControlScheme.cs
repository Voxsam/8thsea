using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControlScheme {

	public bool isKeyboard;
	public int playerNumber;
	public int joystickNumber;

    public static int MAXIMUM_JOYSTICKS = 4;
    public static int MAXIMUM_KEYBOARD = 2;

    /// <summary>
    /// The valid controls that the player can use
    /// </summary>
    public enum Controller
    {
        Up,
        Down,
        Left,
        Right,
        Action,
        Cancel,
		Menu
    }

    private static KeyCode[][] Controls = {
        // Follows ControlOrder
        new KeyCode[]{ KeyCode.W, KeyCode.S, KeyCode.A, KeyCode.D, KeyCode.E, KeyCode.R, KeyCode.Q },
		new KeyCode[]{ KeyCode.UpArrow, KeyCode.DownArrow, KeyCode.LeftArrow, KeyCode.RightArrow, KeyCode.Return, KeyCode.RightShift, KeyCode.Backspace },
    };

    /// <summary>
    /// JoystickNum corresponds to the Player number, aka Player/Joysticks 1-4
    /// Action is the action that the player has taken
    /// </summary>
    /// <returns></returns>
    public static KeyCode GetKeyCode(int joystickNum, Controller action)
    {
        if (joystickNum <= 0 || joystickNum > MAXIMUM_KEYBOARD)
        {
            return KeyCode.None;
        }
        return Controls[joystickNum - 1][(int)action];
    }

	public ControlScheme(int playerNumber, bool isKeyboard) {

		this.isKeyboard = isKeyboard;
        
        this.joystickNumber = playerNumber;
        /*
        if (!isKeyboard) {

            this.joystickNumber = playerNumber;
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
        //*/
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
	public bool GetActionKeyDown () {
		if (!isKeyboard) {
			return Input.GetKeyDown ("joystick " + joystickNumber + " button 0");
		} else {
            return Input.GetKeyDown(GetKeyCode(joystickNumber, Controller.Action));
		}
	}

	public bool GetActionKey () {
		if (!isKeyboard) {
			return Input.GetKey ("joystick " + joystickNumber + " button 0");
		} else {
            return Input.GetKey(GetKeyCode(joystickNumber, Controller.Action));
        }
	}

	public bool GetActionKeyUp () {
		if (!isKeyboard) {
			return Input.GetKeyUp ("joystick " + joystickNumber + " button 0");
		} else {
            return Input.GetKeyUp(GetKeyCode(joystickNumber, Controller.Action));
        }
	}

	// Cancel = B button
	// For WASD = R
	// For arrow keys = right shift
	public bool GetCancelKeyDown () {
		if (!isKeyboard) {
			return Input.GetKeyDown ("joystick " + joystickNumber + " button 1");
		} else {
            return Input.GetKeyDown(GetKeyCode(joystickNumber, Controller.Cancel));
        }
	}

	public bool GetCancelKey () {
		if (!isKeyboard) {
			return Input.GetKey ("joystick " + joystickNumber + " button 1");
		} else {
            return Input.GetKey(GetKeyCode(joystickNumber, Controller.Cancel));
        }
	}

	public bool GetCancelKeyUp () {
		if (!isKeyboard) {
			return Input.GetKeyUp ("joystick " + joystickNumber + " button 1");
		} else {
            return Input.GetKeyUp(GetKeyCode(joystickNumber, Controller.Cancel));
        }
	}

	// Cancel = X button
	// For WASD = Q
	// For arrow keys = Backspace
	public bool GetMenuKeyDown () {
		
		if (!isKeyboard) {
			return Input.GetKeyDown ("joystick " + joystickNumber + " button 2");
		} else {
			return Input.GetKeyDown(GetKeyCode(joystickNumber, Controller.Menu));
		}
	}

	public bool GetMenuKey () {
		if (!isKeyboard) {
			return Input.GetKey ("joystick " + joystickNumber + " button 2");
		} else {
			return Input.GetKey(GetKeyCode(joystickNumber, Controller.Menu));
		}
	}

	public bool GetMenuKeyUp () {
		if (!isKeyboard) {
			return Input.GetKeyUp ("joystick " + joystickNumber + " button 2");
		} else {
			return Input.GetKeyUp(GetKeyCode(joystickNumber, Controller.Menu));
		}
	}

}

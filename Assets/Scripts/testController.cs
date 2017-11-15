using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;// Required when using Event data.
using UnityEngine.UI;

public class testController : MonoBehaviour {
	public ControlScheme cs = new ControlScheme(1,true);
	public Button buttons;
	public Button NewButton;

	// Use this for initialization
	void Start () {
		cs.isKeyboard = true;
		NewButton = buttons.GetComponent<NewButton>();
	}
	
	// Update is called once per frame
	void Update () {
        if (cs.GetUpKeyUp())
        {
			EventSystem.current.firstSelectedGameObject = NewButton;
        }
        else if (cs.GetUpKeyDown())
        {
            print("down");
        }
	}
}

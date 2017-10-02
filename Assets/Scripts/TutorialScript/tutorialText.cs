﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class tutorialText : MonoBehaviour {

    public GameObject TutorialCanvas;
    private Text TextObject;
    public static bool isScriptActivated = false;
    private static string[] conversations = new string[] {
        "Step 1\nHi, welcome to tutorial. Type 't'\nto toggle tutorial screen to read what you need to do.",
        "Step 2\nGood job. Use A, W, D, S OR\nleft, up, right, down keys to move around.\nType space to get in the submarine.",
        "Step 3\nWell done."
    };

	// Use this for initialization
	void Start () {
        //TutorialCanvas.SetActive(isScriptActivated);
        TextObject = TutorialCanvas.GetComponentInChildren<Text>();
        TutorialCanvas.SetActive(isScriptActivated);
    }
	
	// Update is called once per frame
	void Update () {
        if (tutorialController.isNext) {
            TextObject.text = conversations[tutorialController.currentTutorialStep];
            // print(conversations[tutorialController.currentTutorialStep]);
        }
        TutorialCanvas.SetActive(isScriptActivated);
    }

    public static void turnOnScript() {
        isScriptActivated = true;
    }

    public static void turnOffScript()
    {
        isScriptActivated = false;
    }
}
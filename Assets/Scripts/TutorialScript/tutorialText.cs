using System.Collections;
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
        "Step 3\nWell done. Come over here and hit <SPACE> to drive the\nsubmarine or return to base.",
        "Step4\nUse A, W, D, S OR\nleft, up right, down keys to move around.\nType 'E' to return to player mode.",
        "Step5\nGood job. Now let's catch some fishes. Come over here\nand type <SPACE>.",
        "Step6\nMove the suction around and hold <SPACE> to suck fishes.\nIf you are stuck:\nType 'E' to return to player mode.",
        "Step7\nCool you caught fishes! Lets now get back to the station.\nGet to the driving station and type <SPACE> to dock\nback at station.",
        "Step8\nNow you have gathered some fishes, type <SPACE> to pick\nup. Type 'E' to drop the fish.\n*Warning, once picked, the fish have time-limit.",
        "Step9\nPick up a fish and follow me to the first station.\nSpam <SPACE> until you massage finish and pick it up.",
        "Step10\nWell done. Now get the fish and now try to find your way\ndownstairs with <SPACE>. Now continue your research here.",
        "Step11\nWoah, you have completed tutorial and is ready to explore\nthe game (:"
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

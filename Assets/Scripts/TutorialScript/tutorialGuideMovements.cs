using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class tutorialGuideMovements : MonoBehaviour {

    public GameObject submarine;
    public GameObject tutorialGuide;

    private static Vector3[] positions = new Vector3[] {
        new Vector3(-17.28f,4.6f,1.35f),
        new Vector3(-2f,4.6f,1.35f), // stay put
        new Vector3(10.44f,4.6f,-12.82f),
        new Vector3(0,0,0), //dummy
        new Vector3(0,0,0), //dummy
        new Vector3(0,0,0),
        new Vector3(0,0,0),
        new Vector3(0,0,0),
        new Vector3(-6f,4.6f,-5f),
        new Vector3(-5f,-4f,-4.5f),
        new Vector3(0,0,0)
    };
    public static bool isTutorialGuideRunning;

	// Use this for initialization
	void Start () {
        isTutorialGuideRunning = true;
        tutorialController.startTutorial();
        submarine = GameObject.Find("Submarine");
        tutorialGuide = GameObject.Find("TutorialGuide");
    }
	
	// Update is called once per frame
	void Update () {
        if (isTutorialGuideRunning) {
            if (isEqualVector3(transform.position, positions[tutorialController.currentTutorialStep]))
            {
                if (tutorialController.currentTutorialStep == 0)
                {
                    isTutorialGuideRunning = false;
                    tutorialText.turnOnScript();
                    tutorialController.isNext = true;
                }
                else if (tutorialController.currentTutorialStep == 1)
                {
                    tutorialController.isPlay = true;
                }
            } else {
                if (tutorialController.currentTutorialStep == 0)
                {
                    moveToLocation(transform.position, positions[tutorialController.currentTutorialStep], 1f);
                }
                else if (tutorialController.currentTutorialStep == 1)
                {
                    moveToLocation(transform.position, positions[tutorialController.currentTutorialStep], 3f);
                }
                else if (tutorialController.currentTutorialStep == 2)
                {
                    moveToLocation(transform.position, positions[tutorialController.currentTutorialStep], 1f);
                }
                else if (tutorialController.currentTutorialStep == 3 || (tutorialController.currentTutorialStep > 4 && tutorialController.currentTutorialStep < 8))
                {
                    moveToLocation(tutorialGuide.transform.position, submarine.transform.position + new Vector3(-1.86f, 5.5f, -12.72f), 3f);
                }
                else if (tutorialController.currentTutorialStep == 4) // 5 can be ignored.
                {
                    moveToLocation(tutorialGuide.transform.position, submarine.transform.position + new Vector3(6.7f, 5.5f, 1.36f), 3f);
                }
                else if (tutorialController.currentTutorialStep == 8)
                {
                    moveToLocation(transform.position, positions[tutorialController.currentTutorialStep], 1f);
                }
                else if (tutorialController.currentTutorialStep == 9)
                {
                    moveToLocation(transform.position, positions[tutorialController.currentTutorialStep], 1f);
                }
            }
        }
	}

    void activateTutorialGuideRunning() {
        isTutorialGuideRunning = true;
    }

    void moveToLocation(Vector3 start, Vector3 end, float speed) {
        transform.position = Vector3.Lerp(start, end, speed * Time.deltaTime);
    }

    bool isEqualVector3(Vector3 a, Vector3 b)
    {
        return Vector3.SqrMagnitude(a - b) < 0.01;
    }
}

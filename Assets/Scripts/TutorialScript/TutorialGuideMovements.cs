using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialGuideMovements : MonoBehaviour {

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
        TutorialController.startTutorial();
        submarine = GameObject.Find("Submarine");
        tutorialGuide = GameObject.Find("TutorialGuide");
    }
	
	// Update is called once per frame
	void Update () {
        if (isTutorialGuideRunning) {
            if (isEqualVector3(transform.position, positions[TutorialController.currentTutorialStep]))
            {
                if (TutorialController.currentTutorialStep == 0)
                {
                    isTutorialGuideRunning = false;
                    TutorialText.turnOnScript();
                    TutorialController.isNext = true;
                }
                else if (TutorialController.currentTutorialStep == 1)
                {
                    TutorialController.isPlay = true;
                }
            } else {
                if (TutorialController.currentTutorialStep == 0)
                {
                    moveToLocation(transform.position, positions[TutorialController.currentTutorialStep], 1f);
                }
                else if (TutorialController.currentTutorialStep == 1)
                {
                    moveToLocation(transform.position, positions[TutorialController.currentTutorialStep], 3f);
                }
                else if (TutorialController.currentTutorialStep == 2)
                {
                    moveToLocation(transform.position, positions[TutorialController.currentTutorialStep], 1f);
                }
                else if (TutorialController.currentTutorialStep == 3 || (TutorialController.currentTutorialStep > 4 && TutorialController.currentTutorialStep < 8))
                {
                    moveToLocation(tutorialGuide.transform.position, submarine.transform.position + new Vector3(-1.86f, 5.5f, -12.72f), 3f);
                }
                else if (TutorialController.currentTutorialStep == 4) // 5 can be ignored.
                {
                    moveToLocation(tutorialGuide.transform.position, submarine.transform.position + new Vector3(6.7f, 5.5f, 1.36f), 3f);
                }
                else if (TutorialController.currentTutorialStep == 8)
                {
                    moveToLocation(transform.position, positions[TutorialController.currentTutorialStep], 1f);
                }
                else if (TutorialController.currentTutorialStep == 9)
                {
                    moveToLocation(transform.position, positions[TutorialController.currentTutorialStep], 1f);
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

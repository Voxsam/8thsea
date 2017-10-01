using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class tutorialGuideMovements : MonoBehaviour {

    private static Vector3[] positions = new Vector3[] {
        new Vector3(-17.28f,4.6f,1.35f),
        new Vector3(0.261f,4.6f,1.35f)
    };
    public static bool isTutorialGuideRunning;

	// Use this for initialization
	void Start () {
        isTutorialGuideRunning = true;
        tutorialController.startTutorial();
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
                    tutorialController.isPlay = true;
                }
                else if (tutorialController.currentTutorialStep == 1)
                {

                }
            } else {
                moveToLocation(transform.position, positions[tutorialController.currentTutorialStep], 1f);
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

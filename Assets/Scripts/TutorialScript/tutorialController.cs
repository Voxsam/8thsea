using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class tutorialController : MonoBehaviour {
    public static bool isPlay = true; // this is for debugging currently
    public static int currentTutorialStep = 0;
    public static bool isNext = false;
    public GameObject mainPlayer;

    void Start()
    {
        mainPlayer = GameObject.Find("Player");
    }

    void Update()
    {
        if(currentTutorialStep == 1)
        {
            if (mainPlayer.transform.position.x > 0f)
            {
                isNext = false;
                tutorialGuideMovements.isTutorialGuideRunning = true;
                next();
            }
        }
    }

    public static void startTutorial() {
        isPlay = false;
    }

    public static void next()
    {
        if (!isNext) {
            ++currentTutorialStep;
        }
        isNext = true;
    }

    public static void turnOff() {
        isNext = false;
    }
}

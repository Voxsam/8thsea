using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonPromptController : MonoBehaviour {

    public enum State
    {
        Idle,
        Depress
    }
    private State currentState;

    [SerializeField] private RectTransform rectTransform;
    [SerializeField] private AnimationCurve idlePulseCurve;
    [SerializeField] private AnimationCurve depressPulseCurve;
    [SerializeField] private Sprite aUpSprite;
    [SerializeField] private Sprite aDownSprite;
    [SerializeField] private Image image;

    private float depressPulseDuration;
    private float animIdlePulseDuration;
    private static float buttonUpDuration = 0.1f;
    private float currentTime;

	// Use this for initialization
	void Start () {
        currentState = State.Idle;
        currentTime = 0f;
        animIdlePulseDuration = idlePulseCurve.keys[idlePulseCurve.length - 1].time;
        depressPulseDuration = depressPulseCurve.keys[depressPulseCurve.length - 1].time;
    }

    // Update is called once per frame
    void Update() {
        switch (currentState)
        {
            case State.Idle:
                if (currentTime < animIdlePulseDuration)
                {
                    rectTransform.localScale = new Vector3(idlePulseCurve.Evaluate(currentTime),
                                                            idlePulseCurve.Evaluate(currentTime),
                                                            idlePulseCurve.Evaluate(currentTime));
                    currentTime += Time.deltaTime;
                }
                else
                {
                    currentTime = 0f;
                }
                break;
            case State.Depress:
                if (currentTime < animIdlePulseDuration)
                {
                    if (currentTime > buttonUpDuration)
                    {
                        image.sprite = aUpSprite;
                    }
                    rectTransform.localScale = new Vector3(depressPulseCurve.Evaluate(currentTime),
                                                            depressPulseCurve.Evaluate(currentTime),
                                                            depressPulseCurve.Evaluate(currentTime));
                    currentTime += Time.deltaTime;
                }
                else
                {
                    currentState = State.Idle;
                    image.sprite = aUpSprite;
                }
                break;
        }
	}

    public void Depress ()
    {
        currentState = State.Depress;
        currentTime = 0f;

        image.sprite = aDownSprite;
    }
}

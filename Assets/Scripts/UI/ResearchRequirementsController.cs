using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ResearchRequirementsController : MonoBehaviour {
    public enum State
    {
        Idle,
        Selected
    };
    private State currentState;
    private GameData.FishType fishTypeIndex;
    private Quaternion originalRotation;

    [SerializeField]  private Text numResearched;
    [SerializeField]  private Text numToResearch;
    [SerializeField] private Transform meshTransform;

    private void Awake()
    {
    }

    // Use this for initialization
    private void Start () {
        currentState = State.Idle;
        originalRotation = meshTransform.rotation;
    }

    private void Update()
    {
        switch (currentState)
        {
            case State.Idle:
                meshTransform.rotation = Quaternion.RotateTowards(meshTransform.rotation, originalRotation, Time.deltaTime * 50);
                break;
            case State.Selected:
                meshTransform.Rotate(Vector3.up, Time.deltaTime * 50);
                break;
        }
    }

    // Update is called once per frame
    private void LateUpdate () {
        numResearched.text = GameData.GetFishParameters(fishTypeIndex).totalResearched.ToString();
    }

    public void Init(GameData.FishType type)
    {
        fishTypeIndex = type;

        numResearched.text = GameData.GetFishParameters(fishTypeIndex).totalResearched.ToString();
        numToResearch.text = GameData.GetFishParameters(fishTypeIndex).totalToResearch.ToString();
    }

    public void Select()
    {
        currentState = State.Selected;
    }

    public void Deselect()
    {
        currentState = State.Idle;
    }
}

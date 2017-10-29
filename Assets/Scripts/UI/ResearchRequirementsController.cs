using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ResearchRequirementsController : MonoBehaviour {

    private GameData.FishType fishTypeIndex;

    private Text numResearched;
    private Text numToResearch;
    private RectTransform rectTransform;
    private GameObject fishDetails;

    //Prefab to instantiate FishDetails
    public GameObject fishDetailsTemplate;

    private void Awake()
    {
    }

    // Use this for initialization
    void Start () {
        
    }
	
	// Update is called once per frame
	void LateUpdate () {
        numResearched.text = GameData.GetFishParameters(fishTypeIndex).totalResearched.ToString();
    }

    public void ToggleFishDetails ( bool toggle )
    {
        fishDetails.SetActive(toggle);
    }

    public void Init(GameData.FishType type)
    {
        fishTypeIndex = type;
        numResearched = transform.Find("ResearchedNumber").GetComponent<Text>();
        numToResearch = transform.Find("ToResearchNumber").GetComponent<Text>();
        rectTransform = gameObject.GetComponent<RectTransform>();

        numResearched.text = GameData.GetFishParameters(fishTypeIndex).totalResearched.ToString();
        numToResearch.text = GameData.GetFishParameters(fishTypeIndex).totalToResearch.ToString();

        fishDetails = (GameObject)Instantiate(fishDetailsTemplate);
        //TODO: CHANGE THIS.
        //fishDetails.transform.SetParent(GameController.Obj.gameCamera.GetCanvas.transform, false);
        fishDetails.name = gameObject.ToString();
        fishDetails.GetComponent<FishDetailsController>().Init(fishTypeIndex, gameObject);
        fishDetails.SetActive(false);
    }
}

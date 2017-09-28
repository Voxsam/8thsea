using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ResearchRequirementsController : MonoBehaviour {

    private int fishTypeIndex;

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
        numResearched.text = GameLogicController.AllFishParameters[fishTypeIndex].totalResearched.ToString();
    }

    public void ToggleFishDetails ( bool toggle )
    {
        fishDetails.SetActive(toggle);
    }

    public void Init(int type)
    {
        fishTypeIndex = type;
        numResearched = transform.Find("ResearchedNumber").GetComponent<Text>();
        numToResearch = transform.Find("ToResearchNumber").GetComponent<Text>();
        rectTransform = gameObject.GetComponent<RectTransform>();

        numResearched.text = GameLogicController.AllFishParameters[fishTypeIndex].totalResearched.ToString();
        numToResearch.text = GameLogicController.AllFishParameters[fishTypeIndex].totalToResearch.ToString();

        fishDetails = (GameObject)Instantiate(fishDetailsTemplate);
        fishDetails.transform.SetParent(GameController.Obj.gameCamera.GetCanvas.transform, false);
        fishDetails.name = gameObject.ToString();
        fishDetails.GetComponent<FishDetailsController>().Init(fishTypeIndex, gameObject);
        fishDetails.SetActive(false);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FishDetailsController : MonoBehaviour {

    private int fishTypeIndex;
    private Text fishName;
    private RectTransform rectTransform;

    private GameObject anchorGameObject;

    public GameObject researchProtocolTemplateObject;

    // Use this for initialization
    void Start () {
        fishTypeIndex = 0;
    }

    public void Init ( int type, GameObject anchor )
    {
        fishName = transform.Find("FishName").GetComponent<Text>();
        rectTransform = gameObject.GetComponent<RectTransform>();

        fishTypeIndex = type;
        anchorGameObject = anchor;
        fishName.text = GameLogicController.AllFishParameters[fishTypeIndex].name;
        for (int i = 0; i < GameLogicController.AllFishParameters[fishTypeIndex].researchProtocols.Length; i++)
        {
            GameObject researchProtocolUIObject = (GameObject)Instantiate(researchProtocolTemplateObject);
            researchProtocolUIObject.transform.SetParent(gameObject.transform, false);
            researchProtocolUIObject.GetComponent<RectTransform>().anchoredPosition = new Vector2((i * 100), 80);
            researchProtocolUIObject.transform.Find("ProtocolName").gameObject.GetComponent<Text>().text = GameLogicController.AllFishParameters[fishTypeIndex].researchProtocols[i].researchStation;
        }
    }
	
	// Update is called once per frame
	void Update () {
        if (anchorGameObject != null)
        {
            Vector2 ViewportPosition = Camera.main.WorldToScreenPoint(anchorGameObject.transform.position);
            ViewportPosition.x += 30;
            rectTransform.anchoredPosition = ViewportPosition;
        }
    }
}

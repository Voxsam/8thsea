using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FishDetailsController : MonoBehaviour {

    private GameController.FishType fishTypeIndex;
    [SerializeField] private Text fishName;
    [SerializeField] private RectTransform rectTransform;

    private GameObject anchorGameObject;

    public GameObject researchProtocolTemplateObject;

    // Use this for initialization
    void Start () {
        fishTypeIndex = 0;
    }

    public void Init (GameController.FishType type, GameObject anchor )
    {
        //fishName = transform.Find("FishName").GetComponent<Text>();
        //rectTransform = gameObject.GetComponent<RectTransform>();

        fishTypeIndex = type;
        anchorGameObject = anchor;
        fishName.text = GameController.GetFishParameter(fishTypeIndex).name;
        for (int i = 0; i < GameController.GetFishParameter(fishTypeIndex).researchProtocols.Length; i++)
        {
            GameObject researchProtocolUIObject = (GameObject)Instantiate(researchProtocolTemplateObject);
            researchProtocolUIObject.transform.SetParent(gameObject.transform, false);
            researchProtocolUIObject.GetComponent<RectTransform>().anchoredPosition = new Vector2((i * 100), 80);
            researchProtocolUIObject.GetComponentInChildren<Text>().text = GameController.GetFishParameter(fishTypeIndex).researchProtocols[i].researchStation;
        }
    }
	
	// Update is called once per frame
	void Update () {
        if (anchorGameObject != null)
        {
            Vector2 ViewportPosition = GameController.Obj.gameCamera.GetCamera.WorldToScreenPoint(anchorGameObject.transform.position);
            ViewportPosition.x += 30;
            rectTransform.anchoredPosition = ViewportPosition;
        }
    }
}

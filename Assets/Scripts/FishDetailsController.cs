using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FishDetailsController : MonoBehaviour {

    private int fishTypeIndex;
    private Text fishName;
    private RectTransform rectTransform;

    private GameObject anchorGameObject;

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
        fishName.text = GameLogicController.AllFishParameters[0].name;
    }
	
	// Update is called once per frame
	void Update () {
        Vector2 ViewportPosition = Camera.main.WorldToScreenPoint(anchorGameObject.transform.position);
        ViewportPosition.x += 30;
        rectTransform.anchoredPosition = ViewportPosition;
    }
}

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
        fishTypeIndex = type;
        anchorGameObject = anchor;
        fishName = transform.Find("FishName").GetComponent<Text>();
        rectTransform = gameObject.GetComponent<RectTransform>();

        fishName.text = GameLogicController.AllFishParameters[fishTypeIndex].name;
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

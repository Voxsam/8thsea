using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerPanelController : MonoBehaviour {

	public Player player;
	private Canvas mainCanvas;
	private float mainCanvasWidth;
	private float mainCanvasHeight;

	public GameObject bottomLeftPanel;
	public Text bottomLeftText;
	public ResearchIconHolderScript researchIcons;

	void Awake () {
		mainCanvas = GameObject.FindGameObjectWithTag ("Main Canvas").GetComponent<Canvas> ();
		mainCanvasWidth = mainCanvas.GetComponent<RectTransform> ().rect.width;
		mainCanvasHeight = mainCanvas.GetComponent<RectTransform> ().rect.height;
	}

	public void Setup (Player p)
	{

		RectTransform rectTrans = GetComponent<RectTransform> ();

		rectTrans.SetParent (mainCanvas.GetComponent<RectTransform> ());

		// Set pivot to bottom left corner
		rectTrans.pivot = new Vector2 (0, 0);

		switch (PlayerList.Obj.numPlayers) {

		case 1:
			rectTrans.rect.Set (0, 0, mainCanvasWidth, mainCanvasHeight);
			break;

		case 2:
			if (p.playerNumber == 1) {
				rectTrans.anchorMin = new Vector2 (0, 0);
				rectTrans.anchorMax = new Vector2 (0.5f, 1f);

			} else {
				rectTrans.anchorMin = new Vector2 (0.5f, 0);
				rectTrans.anchorMax = new Vector2 (1f, 1f);
				// rectTrans.localPosition = new Vector2 (mainCanvasWidth / 2, 0);
			}
			break;

		// LET'S JUST NOT ALLOW 3 PLAYERS FOR NOW
		/*
		case 3:
			rectTrans.rect.width = mainCanvas.GetComponent<RectTransform> ().rect.width;
			rectTrans.rect.height = mainCanvas.GetComponent<RectTransform> ().rect.height;
			break;
		*/

		case 4:
			if (p.playerNumber == 1) {
				rectTrans.anchorMin = new Vector2 (0, 0.5f);
				rectTrans.anchorMax = new Vector2 (0.5f, 1f);
			} else if (p.playerNumber == 2) {
				rectTrans.anchorMin = new Vector2 (0.5f, 0.5f);
				rectTrans.anchorMax = new Vector2 (1f, 1f);
			} else if (p.playerNumber == 3) {
				rectTrans.anchorMin = new Vector2 (0, 0);
				rectTrans.anchorMax = new Vector2 (0.5f, 0.5f);
			} else {
				rectTrans.anchorMin = new Vector2 (0.5f, 0);
				rectTrans.anchorMax = new Vector2 (1f, 0.5f);
			}
			break;
		
		default:
			rectTrans.rect.Set (0, 0, mainCanvasWidth / 2, mainCanvasHeight);
			break;

		}

	}

	public void ShowFishDetailsPanel (string fishName, FishController.ResearchProtocol[] protocols)
	{
		bottomLeftPanel.SetActive (true);

		bottomLeftText.text = fishName;

		researchIcons.Show (protocols.Length);

		for (int i = 0; i < protocols.Length; i++) {
			print (protocols [i].researchStation + " " + protocols [i].complete);

			if (!protocols [i].complete) {

				switch (protocols [i].researchStation) {

				case GameData.StationType.Research:
					researchIcons.SwitchSpriteForIcon (researchIcons.researchIcon, i);
					break;

				case GameData.StationType.Sample:
					researchIcons.SwitchSpriteForIcon (researchIcons.dissectIcon, i);
					break;

				case GameData.StationType.Photograph:
					researchIcons.SwitchSpriteForIcon (researchIcons.cameraIcon, i);
					break;

				default:
					break;

				}
					
			} else {

				switch (protocols [i].researchStation) {

				case GameData.StationType.Research:
					researchIcons.SwitchSpriteForIcon (researchIcons.researchDoneIcon, i);
					break;

				case GameData.StationType.Sample:
					researchIcons.SwitchSpriteForIcon (researchIcons.dissectDoneIcon, i);
					break;

				case GameData.StationType.Photograph:
					researchIcons.SwitchSpriteForIcon (researchIcons.cameraDoneIcon, i);
					break;

				default:
					break;

				}

			}

		}
	}

	public void ShowInBottomLeftPanel (string content)
	{
		bottomLeftPanel.SetActive (true);
		bottomLeftText.text = content;
	}

	public void HideBottomLeftPanel ()
	{
		bottomLeftPanel.SetActive (false);
	}


	public RectTransform GetRectTransform {
		get { return GetComponent<RectTransform> (); }
	}

		
}

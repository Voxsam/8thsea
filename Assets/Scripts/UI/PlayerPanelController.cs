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
	public GameObject centerPanel;

	void Awake () {
		mainCanvas = GameObject.FindGameObjectWithTag ("Main Canvas").GetComponent<Canvas> ();
		mainCanvasWidth = mainCanvas.GetComponent<RectTransform> ().rect.width;
		mainCanvasHeight = mainCanvas.GetComponent<RectTransform> ().rect.height;
	}

	public void Setup (Player p)
	{

		RectTransform rectTrans = GetComponent<RectTransform> ();

		// Set pivot to bottom left corner
		rectTrans.pivot = new Vector2 (0, 0);

		switch (PlayerList.Obj.numPlayers) {

		case 1:
			rectTrans.rect.Set (0, 0, mainCanvasWidth, mainCanvasHeight);
			break;

		case 2:

			if (p.playerNumber == 1) {
				rectTrans.anchorMax = new Vector2 (0.5f, 1f);
			} else {
				rectTrans.anchorMin = new Vector2 (0.5f, 0);
				rectTrans.anchorMax = new Vector2 (1f, 1f);
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

	public void ShowInPanel (GameObject obj)
	{
		obj.GetComponent<RectTransform> ().SetParent (GetComponent<RectTransform> (), false);
	}
}

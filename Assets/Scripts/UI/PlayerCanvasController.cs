using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerCanvasController : MonoBehaviour {

	public Camera playerCamera;

	[Tooltip("Time taken for UI elements to slide in.")]
	public float slideTime;

	public GameObject bottomLeftPanel;
	public Text bottomLeftText;
	public ResearchIconHolderScript researchIcons;


	public void Setup (Camera cam) {

		playerCamera = cam;
		GetComponent<Canvas> ().worldCamera = cam;

		bottomLeftPanel.SetActive (false);

	}

	public void ShowFishDetailsPanel (string fishName, FishController.ResearchProtocol[] protocols)
	{
		bottomLeftPanel.SetActive (true);

		SlideInFromLeft (bottomLeftPanel.GetComponent<RectTransform> ());

		bottomLeftText.text = fishName;

		researchIcons.Show (protocols.Length);

		for (int i = 0; i < protocols.Length; i++) {

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

	public void HideBottomLeftPanel ()
	{
		SlideOutToLeft (bottomLeftPanel.GetComponent<RectTransform> ());
		bottomLeftPanel.SetActive (false);
	}


	private void SlideInFromLeft (RectTransform panelRect) {

		Vector2 endPosition = panelRect.localPosition;
		panelRect.localPosition = new Vector2 (panelRect.localPosition.x - panelRect.rect.width, panelRect.localPosition.y);

		StartCoroutine (LerpPosition (panelRect, endPosition, 0.2f));

	}

	private void SlideOutToLeft (RectTransform panelRect) {
		Vector2 endPosition = new Vector2 (panelRect.localPosition.x - panelRect.rect.width, panelRect.localPosition.y);
		StartCoroutine (LerpPosition (panelRect, endPosition, 0.2f));
	}

	IEnumerator LerpPosition (RectTransform item, Vector2 endPosition, float time) {

		Vector2 startPosition = item.localPosition;
		float elapsedTime = 0f;

		while (elapsedTime < time) {
			item.localPosition = Vector2.Lerp (startPosition, endPosition, Mathf.SmoothStep(0, 1, elapsedTime / time));
			elapsedTime += Time.deltaTime;
			yield return new WaitForEndOfFrame ();
		}

	}

	private void AddLayerRecursively (Transform trans, int layerNo) {
		trans.gameObject.layer = layerNo;
		foreach (Transform t in trans) {
			AddLayerRecursively (t, layerNo);
		}
	}


}

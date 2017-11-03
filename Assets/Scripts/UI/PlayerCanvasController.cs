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
		GetComponent<Canvas> ().planeDistance = 10.0f;
		bottomLeftPanel.SetActive (false);

	}

	public void ShowFishDetailsPanel (string fishName, FishController.ResearchProtocol[] protocols)
	{

		SlideInFromLeft (bottomLeftPanel);

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
		SlideOutToLeft (bottomLeftPanel);
	}
	

	private void SlideInFromLeft (GameObject panel) {
		Vector3 endPosition = panel.transform.localPosition;
		Vector3 startPosition = new Vector3 (panel.transform.localPosition.x - panel.GetComponent<RectTransform> ().rect.width, panel.transform.localPosition.y);
		panel.SetActive (true);
		panel.transform.localPosition = startPosition;
		StartCoroutine (LerpPosition (panel, startPosition, endPosition, slideTime));
	}

	private void SlideOutToLeft (GameObject panel) {
		Vector3 startPosition = panel.transform.localPosition;
		Vector3 endPosition = new Vector3 (panel.transform.localPosition.x - panel.GetComponent<RectTransform> ().rect.width, panel.transform.localPosition.y);
		StartCoroutine (DeactivateAfterCoroutine(panel, LerpPosition (panel, startPosition, endPosition, slideTime, startPosition)));
	}

	IEnumerator LerpPosition (GameObject _object, Vector3 startPosition, Vector3 endPosition, float time) {
		float elapsedTime = 0f;
		while (elapsedTime < time) {
			_object.transform.localPosition = Vector3.Lerp (startPosition, endPosition, Mathf.SmoothStep(0, 1, elapsedTime / time));
			elapsedTime += Time.deltaTime;
			yield return new WaitForEndOfFrame ();
		}
		_object.transform.localPosition = endPosition;
	}

	IEnumerator LerpPosition (GameObject _object, Vector3 startPosition, Vector3 endPosition, float time, Vector3 realEndPosition) {
		yield return LerpPosition (_object, startPosition, endPosition, time);
		_object.transform.localPosition = realEndPosition;
	}

	IEnumerator DeactivateAfterCoroutine (GameObject _object, IEnumerator _coroutine) {
		yield return _coroutine;
		_object.SetActive (false);
	}


	private void AddLayerRecursively (Transform trans, int layerNo) {
		trans.gameObject.layer = layerNo;
		foreach (Transform t in trans) {
			AddLayerRecursively (t, layerNo);
		}
	}

}

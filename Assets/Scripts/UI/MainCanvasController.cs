using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainCanvasController : MonoBehaviour
{

	public static MainCanvasController Obj;

	public GameObject ss; //get ridda this later
	public GameObject boxOutline;

	public PanelController centerPanel;
	public PanelController bottomPanel;

	private Color invisible = new Color (0, 0, 0, 0);

	public void Setup () {
		Obj = this;
		ss.SetActive (true);
	}

	void Update ()
	{
		foreach (PlayerController p in MultiplayerManager.Obj.playerControllerList) {
			if (p.controls.GetMenuKeyDown ()) {
				centerPanel.gameObject.SetActive (!centerPanel.gameObject.activeSelf);
				if (GameController.Obj.isTutorial) {
					if (TutorialManager.Obj.notifiedPlayerOfTutorialAccess == false) {
						bottomPanel.gameObject.SetActive (true);
						bottomPanel.FadeInAndOut (1f, 5f, 3f, true);
						TutorialManager.Obj.notifiedPlayerOfTutorialAccess = true;
					}
				}
				break;
			}
		}

	}

	public void SetCenterPanelText(string title, string body) {
		centerPanel.textboxes [0].text = title;
		centerPanel.textboxes [1].text = body;
	}

	public void ShowCenterPanel() {
		centerPanel.gameObject.SetActive (true);
		LayoutRebuilder.ForceRebuildLayoutImmediate (centerPanel.gameObject.GetComponent<RectTransform> ());
	}

	public void HideCenterPanel() {
		centerPanel.gameObject.SetActive (true);
	}

	public void SetBottomPanelText(string _text) {
		bottomPanel.textboxes [0].text = _text;
	}

	public void ShowBottomPanel() {
		bottomPanel.gameObject.SetActive (true);
		LayoutRebuilder.ForceRebuildLayoutImmediate (bottomPanel.gameObject.GetComponent<RectTransform> ());
	}

	public void HideBottomPanel() {
		bottomPanel.gameObject.SetActive (true);
	}


	IEnumerator BlinkText(Text textToBlink, float interval) {
		Color init = textToBlink.color;
		float elapsedTime = 0f;
		if (elapsedTime >= interval) {
			textToBlink.color = (textToBlink.color == init) ? invisible : init;
			elapsedTime = 0;
			yield return null;
		} else {
			elapsedTime += Time.deltaTime;
			yield return null;
		}
	}

	IEnumerator DeactivateInTime (GameObject obj, float seconds) {
		yield return new WaitForSecondsRealtime (seconds);
		obj.SetActive (false);
	}

	IEnumerator Wait (float seconds) {
		yield return new WaitForSecondsRealtime (seconds);
	}

}

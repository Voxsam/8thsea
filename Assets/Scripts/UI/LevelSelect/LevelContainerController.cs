using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelContainerController : MonoBehaviour {

	public Text levelName;

	public int currentlySelectedItem = 0;

	public LevelContainerItem[] levels;

	public float distanceBetweenImages = 400f;
	public Vector3 newScale = new Vector3 (1.2f, 1.2f, 1f);
	public float slideTime = 0.3f;

	private bool canChangeLevel = true;
	private Vector3 initPos;

	void Start () {
		initPos = transform.position;
		RefreshSelectedItem ();
	}

	void Update () {

		if (canChangeLevel) {

			if (IsRightButtonPressed ()) {

				if (currentlySelectedItem < levels.Length - 1) {
					ResetSelectedItem ();
					currentlySelectedItem++;
					RefreshSelectedItem ();
					StartCoroutine (SmoothlyMoveContainer (distanceBetweenImages * -1f));
				}

			} else if (IsLeftButtonPressed ()) {

				if (currentlySelectedItem > 0) {
					ResetSelectedItem ();
					currentlySelectedItem--;
					RefreshSelectedItem ();
					StartCoroutine (SmoothlyMoveContainer (distanceBetweenImages));
				}

			}
		}

	}
		


	private bool IsRightButtonPressed () {
		return Input.GetKeyDown (KeyCode.RightArrow) || Input.GetKeyDown (KeyCode.D);
	}

	private bool IsLeftButtonPressed () {
		return Input.GetKeyDown (KeyCode.LeftArrow) || Input.GetKeyDown (KeyCode.A);
	}

	private bool IsActionKeyPressed () {
		return Input.GetKeyDown (KeyCode.E) || Input.GetKeyDown (KeyCode.RightControl);
	}

	IEnumerator SmoothlyMoveContainer (float distance) {
		canChangeLevel = false;
		float elapsedTime = 0;
		Vector3 originalPos = transform.position;
		while (transform.position.x != originalPos.x + distance) {
			transform.position = Vector3.Lerp (originalPos, new Vector3 (originalPos.x + distance, originalPos.y, originalPos.z), elapsedTime / slideTime);
			elapsedTime += Time.deltaTime;
			yield return null;
		}
		canChangeLevel = true;
	}

	private void ResetSelectedItem () {
		levels [currentlySelectedItem].rectTransform.localScale = Vector3.one;
		levels [currentlySelectedItem].floatController.enabled = false;
	}

	private void RefreshSelectedItem () {
		levels [currentlySelectedItem].rectTransform.localScale = newScale;
		levels [currentlySelectedItem].floatController.enabled = true;
		levelName.text = levels [currentlySelectedItem].levelName;
	}

}

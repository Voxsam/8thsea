using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ResearchIconHolderScript : MonoBehaviour {

	public Sprite cameraIcon;
	public Sprite cameraDoneIcon;
	public Sprite dissectIcon;
	public Sprite dissectDoneIcon;
	public Sprite researchIcon;
	public Sprite researchDoneIcon;

	public Image[] icons;

	public void Show (int num) {

		for (int i = 0; i < icons.Length; i++) {

			if (num <= 0) {
				icons [i].gameObject.SetActive (false);
			} else {
				icons [i].gameObject.SetActive (true);
				num--;
			}
				

		}

	}

	public void SwitchSpriteForIcon (Sprite img, int iconNo) {

		icons [iconNo].sprite = img;

	}



}

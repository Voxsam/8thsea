using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MenuPersistence : MonoBehaviour
{

	public static MenuPersistence Obj;

	public bool hasSeenTitle = false;

	void Awake () {

		if (Obj != null) {
			Destroy (this.gameObject);
		} else {
			Obj = this;
		}

		DontDestroyOnLoad (this);
	}

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LoadingBarManager : MonoBehaviour
{
	public float timeToLoadFor = 1.5f;
	private float timeLoaded = 0.0f;

	public Image loadingBar;
	public Text LoadingPercent;

	// Use this for initialization
	void Start () 
	{
		loadingBar.fillAmount = 0;
	}
	
	// Update is called once per frame
	void Update () 
	{
		if (loadingBar.fillAmount <= 1) 
		{
			loadingBar.fillAmount += Time.deltaTime / timeToLoadFor;
		}

		if (timeLoaded <= timeToLoadFor) 
		{
			timeLoaded += Time.deltaTime;
		}
		else 
		{
            SceneManager.LoadScene("main_merged");
		}
			
		LoadingPercent.text = ((int)(loadingBar.fillAmount * 100)).ToString () + "%";
	}
}

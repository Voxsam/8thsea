using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LoadingBarManager : MonoBehaviour
{
	public float loadingTime;
	public float launchTime = 0.0f;

	public Image loadingBar;
	public Text loadingPercent;

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
			loadingBar.fillAmount += 1.0f / loadingTime * Time.deltaTime;
		}

		if (launchTime <= 1.5f) 
		{
			launchTime += 1.0f / loadingTime * Time.deltaTime;
		} 

		else 
		{
			SceneManager.LoadScene (2);
		}
			
		loadingPercent.text = (loadingBar.fillAmount * 100).ToString ("f0");
	}
}

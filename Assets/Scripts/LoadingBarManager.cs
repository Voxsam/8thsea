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
	public Text LoadingTextPercentage;
    public bool isTimeBased = false;

    public ZoneController[] Zones;

	// Use this for initialization
	void Start () 
	{
		loadingBar.fillAmount = 0;
        if (Zones.Length == 0)
        {
            isTimeBased = true;
        }
	}
	
	// Update is called once per frame
	void Update () 
	{
        if (isTimeBased)
        {
            if (timeLoaded <= timeToLoadFor)
            {
                timeLoaded += Time.deltaTime;
            }

            if (loadingBar.fillAmount < 1f)
            {
                loadingBar.fillAmount += timeLoaded / timeToLoadFor;
            }
            else
            {
                //SceneManager.LoadScene("main_merged");
                GameController.Obj.StartGame();
                Destroy(this.gameObject);
            }
        }
        else
        {
            float totalPercent = 0f;
            foreach(ZoneController zone in Zones)
            {
                totalPercent += zone.percentageSpawned;
            }
            float avgPercent = totalPercent / Zones.Length;
            loadingBar.fillAmount = avgPercent;

            if (avgPercent >= 1f)
            {
                // Fake waiting for one frame by forcing it to check again on the next loop
                isTimeBased = true;
            }
        }
			
		LoadingTextPercentage.text = ((int)(loadingBar.fillAmount * 100)).ToString () + "%";
	}
}

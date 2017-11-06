using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextEmitterController : MonoBehaviour {
    [SerializeField] public GameObject particleTemplate;
    [SerializeField] private AnimationCurve explodeCurve;

    private GameObject currentTextParticle;

    // Use this for initialization
    void Start () {
        currentTextParticle = null;
	}
	
	// Update is called once per frame
	void Update () {
	}

    public void SpawnText (string _text)
    {
        if (currentTextParticle != null)
        {
            currentTextParticle.GetComponent<TextParticlesController>().Explode();
        }
        currentTextParticle = Instantiate(particleTemplate);
        currentTextParticle.transform.SetParent(this.transform);
        currentTextParticle.transform.position = this.transform.position;
        TextParticlesController particleObjectController = currentTextParticle.GetComponent<TextParticlesController>();
        particleObjectController.Init(explodeCurve, _text);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExpandingEmitterController : EmitterBase {

    [SerializeField] public MinMaxPair distance;
    [SerializeField] public MinMaxPair scale;

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
	}

    public void Fire ()
    {
        int numberOfParticles = (int)Random.Range(numParticles.min, numParticles.max);
        for (int i = 0; i < numberOfParticles; i++)
        {
            GameObject particleObject = Instantiate(particleTemplate);
            particleObject.transform.SetParent(this.transform);
            particleObject.transform.position = this.transform.position;
            ExpandingParticlesController particleObjectController = particleObject.GetComponent<ExpandingParticlesController>();
            particleObjectController.Init(lifespan.min, lifespan.min, distance.min, distance.max, scale.min, scale.max);
        }
    }
}

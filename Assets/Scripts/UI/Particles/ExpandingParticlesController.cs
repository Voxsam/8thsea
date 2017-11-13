using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ExpandingParticlesController : ParticlesBase {
    [SerializeField] public MinMaxPair distance;
    [SerializeField] public MinMaxPair scale;
    [SerializeField] private RectTransform rectTransform;
    [SerializeField] private Image image;

    private float targetLifespan;
    private float targetDistance;
    private float targetScale;
    private Vector3 direction;
    private Vector3 startPosition;
    private Vector3 endPosition;

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        if (currentLife < targetLifespan)
        {
            transform.position = startPosition + direction * GameData.QuadEaseOut(currentLife, 0, targetDistance, targetLifespan);
            rectTransform.localScale = new Vector3(GameData.QuadEaseOut (currentLife, 0, targetScale, targetLifespan), GameData.QuadEaseOut(currentLife, 0, targetScale, targetLifespan), 0);
            Color c = image.color;
            c.a = 1 - (currentLife / targetLifespan);
            image.color = c;

            currentLife += Time.deltaTime;
        }
        else
        {
            Destroy(gameObject);
        }
	}

    public void Init (float _minLifespan = 1, float _maxLifespan = 1, float _minDist = 10, float _maxDist = 30, float _minScale = 1.1f, float _maxScale = 1.5f)
    {
        lifespan.min = _minLifespan;
        lifespan.max = _maxLifespan;
        distance.min = _minDist;
        distance.max = _maxDist;
        scale.min = _minScale;
        scale.max = _maxScale;
        currentLife = 0f;

        targetLifespan = Random.Range(lifespan.min, lifespan.max);
        targetDistance = Random.Range(distance.min, distance.max);
        targetScale = Random.Range(scale.min, scale.max);
        direction = new Vector3(Random.Range(-1.0f, 1.0f), Random.Range(-1.0f, 1.0f), 0).normalized;
        startPosition = transform.position;
    }
}

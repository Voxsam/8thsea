using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticlesBase : MonoBehaviour {
    [System.Serializable]
    public struct MinMaxPair
    {
        public float min;
        public float max;
    }

    [SerializeField] protected MinMaxPair lifespan;
    protected float currentLife;
}

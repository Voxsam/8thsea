using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EmitterBase : MonoBehaviour {
    [SerializeField] protected GameObject canvas;
    [SerializeField] public GameObject particleTemplate;

    [System.Serializable]
    public struct MinMaxPair
    {
        public float min;
        public float max;
    }
    [SerializeField] protected MinMaxPair numParticles;
    [SerializeField] protected MinMaxPair lifespan;
}

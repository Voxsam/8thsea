using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Waves : MonoBehaviour {

	public float scale = 10.0f;
	public float speed = 1.0f;
	public float resolution = 1;

	Mesh mesh;
	MeshCollider mc;

	Vector3[] vertices;
	Vector3[] newvertices;

	float randomizer = 1.0f;

	void Start () {


		mc = GetComponent<MeshCollider> ();
		mesh = GetComponent<MeshFilter> ().mesh; 

	}


	void Update () {

		randomizer = -Time.time/2;
		vertices = mesh.vertices;
		newvertices = new Vector3[vertices.Length];

		if (resolution <= 0)
			resolution = 0;


		for (int i = 0; i < vertices.Length; i++) {

			Vector3 ver = vertices [i];
			ver.y = Mathf.PerlinNoise ( (speed * randomizer) +(vertices [i].x + transform.position.x) / resolution, -(speed * randomizer) + (vertices [i].z+ transform.position.z) / resolution) * scale;
			newvertices [i] = ver;

		}
		mesh.vertices = newvertices;
		mc.sharedMesh = mesh;
		mesh.RecalculateBounds ();
		mesh.RecalculateNormals ();

	}
}
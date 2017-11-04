using UnityEngine;
using System.Collections;

public class TreeTest : MonoBehaviour {

	// Use this for initialization
	void Start () {
		Terrain terrain = Terrain.activeTerrain;
		float treeHeight = terrain.SampleHeight(new Vector3(transform.position.x,0.0f,transform.position.z));
		Vector3 pos = transform.position;
		transform.position = new Vector3(pos.x,treeHeight,pos.z);
	}
}

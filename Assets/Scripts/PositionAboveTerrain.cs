using UnityEngine;
using System.Collections;

public class PositionAboveTerrain : MonoBehaviour {

	public float yOffset;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {

		// Place tree on top of terrain
		Terrain terrain = Terrain.activeTerrain;
		float height = terrain.SampleHeight(new Vector3(transform.position.x,0.0f,transform.position.z));
		Vector3 pos = transform.position;
		transform.position = new Vector3(pos.x,height + yOffset,pos.z);
	}
}

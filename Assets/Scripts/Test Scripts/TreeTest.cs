using UnityEngine;
using System.Collections;

public class TreeTest : MonoBehaviour {

	int seed = 0;

	public void RefreshTree() {
		Debug.Log("Generating new tree...");
		PGTreeBase tree = gameObject.GetComponent<PGTreeBase>();
		tree.Reset();
		Invoke("InitSeed", 0f);
	}

	private void InitSeed() {
		PGTreeBase tree = gameObject.GetComponent<PGTreeBase>();
		tree.Init(seed);
	}

	public void SetSeed(string new_seed) {
		Debug.Log("Setting seed to " + new_seed);
		seed = int.Parse(new_seed);
	}

	void Update () {

		// Place tree on top of terrain
		Terrain terrain = Terrain.activeTerrain;
		float treeHeight = terrain.SampleHeight(new Vector3(transform.position.x,0.0f,transform.position.z));
		Vector3 pos = transform.position;
		transform.position = new Vector3(pos.x,treeHeight,pos.z);
	}
}

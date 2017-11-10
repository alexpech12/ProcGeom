using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class TreeTest : MonoBehaviour {

	int seed = 0;

	public void RefreshTree() {
		Debug.Log("Generating new tree...");
		//PGTreeBase tree = gameObject.GetComponent<PGTreeBase>();
		//tree.Reset();
		Invoke("InitSeed", 0f);
	}

	public void RefreshTreeRandom() {
		Debug.Log("Generating new random tree...");
		int startSeed = (int)System.DateTime.Now.Ticks;
		Debug.Log(startSeed);
		Random.InitState(startSeed);
		seed = Mathf.RoundToInt(Random.Range(1,System.Int32.MaxValue));
		InputField inputField = GameObject.Find("SeedInputField").GetComponent<InputField>();
		inputField.text = seed.ToString();
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

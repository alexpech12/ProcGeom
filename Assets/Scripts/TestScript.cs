using UnityEngine;
using System.Collections;

public class TestScript : MonoBehaviour {

	// Use this for initialization
	void Start () {
		Texture2D grassTexture = new Texture2D(128,128);
		TerrainGenerator tg = Terrain.activeTerrain.GetComponent("TerrainGenerator") as TerrainGenerator;
		float[,] noise = tg.FractalNoise(128,128,new float[]{0.0f,0.0f,0.0f,0.0f},1.0f);
		for(int i = 0; i < 128; i++) {
			for(int j = 0; j < 128; j++) {
				grassTexture.SetPixel(i,j,new Color(noise[i,j],noise[i,j],noise[i,j],1.0f));
			}
		}
		
		grassTexture.Apply();
		gameObject.GetComponent<Renderer>().material.SetTexture("_MainTex",grassTexture);
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}

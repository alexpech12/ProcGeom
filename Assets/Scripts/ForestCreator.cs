using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ForestCreator : MonoBehaviour {

	private int speciesNum = 1;
	private Terrain terrain;
	
	public List<TreeSpecies> Species { get { return m_Species; } }
	private List<TreeSpecies> m_Species = new List<TreeSpecies>();
	
	// Forest bounds
	public float xMin = 200.0f;
	public float xMax = 300.0f;
	public float zMin = 200.0f;
	public float zMax = 300.0f;
	

	public float treeDensity = 0.1f;

	int treeCount = 0;
	int treesLoadedPerUpdate = 10;
	int treeMax = 0; // Calculated at runtime to determine number of trees to spawn.
	int treeMaxCap = 5000; // Used to prevent performance problems and crashes when treeMax is too large.
	
	void Start() {

		terrain = Terrain.activeTerrain;

		// Generate random tree species
		for(int i = 0; i < speciesNum; i++) {
			TreeSpecies species = new TreeSpecies();
			species.CreateNewSpecies();
			Species.Add(species);
		}
		// Determine maximum number of trees to create
		treeMax = (int)Mathf.RoundToInt((float)(xMax-xMin)*(float)(zMax-zMin)*treeDensity*0.1f);
	}
	
	void Update () {
		// Place trees on terrain
		if (treeCount < treeMax && treeCount < treeMaxCap) {
			int randInt = Random.Range(0,speciesNum);
			GenerateTrees(Species[Random.Range(0,speciesNum)],treesLoadedPerUpdate,xMin,xMax,zMin,zMax);
			treeCount += treesLoadedPerUpdate;
			Debug.Log("treeCount = " + treeCount);
		}
	}
	
	private void GenerateTrees(TreeSpecies species, int treeNum, float xMin, float xMax, float zMin, float zMax) {
		
		for(int i = 0; i < treeNum; i++) {
			GameObject newTree = AddTree(species);
			float randX = Random.Range(xMin,xMax);
			float randZ = Random.Range(zMin,zMax);
			float treeHeight = terrain.SampleHeight(new Vector3(randX,0.0f,randZ));
			newTree.transform.position = new Vector3(randX,treeHeight,randZ);
		}
	}
	
	private GameObject AddTree(TreeSpecies species) {
		GameObject tree = new GameObject("TreeInstance");
		PGTreeBase treeBase = tree.AddComponent<PGTreeBase>();
		treeBase.SetSpecies(species);
		return tree;
	}
	
	public void SetSpeciesNum(int newNum){
		speciesNum = newNum;
	}
	public void SetTerrain(Terrain newTerrain) {
		terrain = newTerrain;
	}
	/*
	private int treeCount = 0;
	//private int treeMax = 10;
	private int treesLoadedPerUpdate = 10;
	private int grassCount = 0;
	//private int grassMax = 10000;
	private int grassLoadedPerUpdate = 100;
	
	private float treeDensity = 0.1f;
	private float grassDensity = 2.0f;
	private int grassMax = 0;
	private int treeMax = 0;
	private int treeMaxCap = 5000;
	private int grassMaxCap = 20000;
	
	private PGTreeSpecies treeSpecies1 = new PGTreeSpecies();
	private PGTreeSpecies treeSpecies2 = new PGTreeSpecies();
	private PGTreeSpecies treeSpecies3 = new PGTreeSpecies();
	
	bool initialUpdate = true;
	
	
	void Update() {
		float xMin = 200.0f;
		float xMax = 300.0f;
		float zMin = 200.0f;
		float zMax = 300.0f;
		if(initialUpdate) {
			grassMax = (int)Mathf.RoundToInt((float)(xMax-xMin)*(float)(zMax-zMin)*grassDensity*0.1f);
			treeMax = (int)Mathf.RoundToInt((float)(xMax-xMin)*(float)(zMax-zMin)*treeDensity*0.1f);
			//treeSpecies.CreateNewSpecies();
			initialUpdate = false;
		}
		
		
		
		if (treeCount < treeMax) {
			int randInt = Random.Range(0,3);
			switch(randInt) {
			case 0:
				GenerateTrees(treeSpecies1, treesLoadedPerUpdate, xMin, xMax, zMin, zMax);
				break;
			case 1:
				GenerateTrees(treeSpecies2, treesLoadedPerUpdate, xMin, xMax, zMin, zMax);
				break;
			case 2:
				GenerateTrees(treeSpecies3, treesLoadedPerUpdate, xMin, xMax, zMin, zMax);
				break;
			}
			//GenerateTrees(treeSpecies, treesLoadedPerUpdate, xMin, xMax, zMin, zMax);
			treeCount += treesLoadedPerUpdate;
			Debug.Log("treeCount = " + treeCount);
		}
		if (grassCount < grassMax) {
			GenerateGrass(grassLoadedPerUpdate,xMin,xMax,zMin,zMax);
			grassCount += grassLoadedPerUpdate;
			Debug.Log("grassCount = " + grassCount);
		}
		
	}
	
	*/
	
	
}

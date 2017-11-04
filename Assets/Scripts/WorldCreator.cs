using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class WorldCreator : MonoBehaviour {
	
	public bool generate_terrain = true;
	public bool generate_trees = true;
	public bool generate_creatures = true;
	public int tree_species_num = 3;
	public int creature_species_num = 1;
	
	private Terrain terrain = null;
	private List<CreatureSpecies> creature_species_list = new List<CreatureSpecies>();
	
	
	// Use this for initialization
	void Start () {
		if(generate_terrain && Terrain.activeTerrain != null) {
			terrain = Terrain.activeTerrain;
			if(terrain.GetComponent<TerrainGenerator>() != null) {
				terrain.GetComponent<TerrainGenerator>().GenerateTerrain();
				if(generate_trees) {
					ForestCreator forest = gameObject.GetComponent<ForestCreator>();//gameObject.AddComponent<ForestCreator>();
					forest.SetTerrain(terrain);
					forest.SetSpeciesNum(tree_species_num);
				}
				
			} else {
				Debug.LogError("ERROR: No TerrainGenerator component on active terrain.");
			}
		}
		
		if(generate_creatures) {
			// Generate species for new creatures
			for(int i = 0; i < creature_species_num; i++) {
				CreatureSpecies newSpecies = new CreatureSpecies();
				newSpecies.CreateNewSpecies();
				creature_species_list.Add(newSpecies);
			}
			// Generate creatures from new species
			
			// For now, just generate a single test creature
			GameObject testCreature = new GameObject("TestCreature");
			testCreature.transform.position = Vector3.zero;
			CreatureBase creatureScript = testCreature.AddComponent<CreatureBase>();
			creatureScript.species = creature_species_list[0];
		}
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}

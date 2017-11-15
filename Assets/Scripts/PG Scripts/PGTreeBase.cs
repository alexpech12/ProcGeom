using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PGTreeBase : MonoBehaviour {
	
	public int m_seed = 0;

	public float m_height = 3.0f;
	public float m_start_radius = 1.5f;
	public float m_end_radius = 1.0f;
	public float m_twist_angle = 1.0f;
	public float m_max_bend = 10f;
	public PGTreeTrunkSimple.TrunkCurveType m_trunk_curve_type_s = PGTreeTrunkSimple.TrunkCurveType.ExpoLinear;
	public PGTreeTrunk.TrunkCurveType m_trunk_curve_type = PGTreeTrunk.TrunkCurveType.ExpoLinear;
	public float m_circ_trunk_bulge = 1.0f;
	public float m_circ_trunk_bulge_offset = 0.0f;
	public float m_circ_trunk_bulge_freq = 1.0f;
	public float m_exp_mid_radius = 1.05f;
	public float m_expolinear_blend = 0.2f;
	public float m_start_irregularity = 0.3f;
	public float m_irregularity_falloff = 5.0f;
	public int m_radial_segments = 8;
	public int m_height_segments = 10;
	
	
	
	// Branch variables
	
	public float m_branch_length = 2.0f;
	public float m_branch_length_randomness = 1.0f;
	public float m_branch_length_falloff = 0.8f;
	public int m_branch_segments = 3;
	
	public float m_branch_max_bend = 45f;
	public float m_branch_bend_falloff = 0.8f;
	public float m_branch_min_uprightness = 0.5f;
	
	public float m_branch_min_fork_angle = 0f;
	public float m_branch_max_fork_angle = 75f;
	public float m_branch_twist_angle = 90f;
	public float m_branch_twist_randomness = 15f;
	public float m_branch_twist_falloff = 0.8f;
	
	public float m_branch_min_radius = 0.1f;
	public float m_branch_radius_falloff = 1.0f;
	
	// Stem variables
	public bool m_hasStems = false;
	public float m_stem_length = 0.1f;
	public float m_stem_radius = 0.01f;
	public int m_stem_segments = 1;
	public float m_stem_start_angle = 90.0f;
	public float m_stem_bend = 30.0f;
	
	// Leaf variables
	public bool m_hasLeaves = true;
	public PGTreeLeaf.LeafType m_leaf_type = PGTreeLeaf.LeafType.Flat;
	
	public int m_leaves_per_branch = 3;
	
	public float m_leaf_length = 1.0f;
	public float m_leaf_length_rand = 0.2f;
	public float m_leaf_width = 1.0f;
	public float m_leaf_width_rand = 0.2f;
	public int m_leaf_length_segments = 2;
	public int m_leaf_width_segments = 2;
	public float m_leaf_bend = 0.0f;
	public float m_leaf_bend_rand = 20.0f;
	public float m_leaf_curl = 0.0f;
	public float m_leaf_curl_rand = 20.0f;
	public float m_leaf_twist = 0.0f;
	public float m_leaf_twist_rand = 20.0f;
	
	// Fruit variables
	public bool m_has_fruit = true;
	
	// Texture variables
	public Color[] m_tree_colors = new Color[5];
	public float[] m_color_thresholds = new float[4];
	public float[] m_noise_scales = new float[4];
	public Vector2[] m_noise_offsets = new Vector2[4];
	public float[] m_noise_opacity = new float[4];
	
	public bool m_leaf_variation = true;
	public bool m_leaf_randomize_shape = true;
	public bool m_leaf_randomize_colors = true;
	public Color[] m_leaf_colors = new Color[]{Color.green,Color.green,Color.green,Color.green};
	
	// LOD variables
	public int m_LOD1_radial_segments = 4;
	public int m_LOD1_height_segments = 3;
	public int m_LOD1_branch_segments = 2;
	public int m_LOD2_radial_segments = 3;
	public int m_LOD2_height_segments = 1;
	public int m_LOD2_branch_segments = 1;
	
	public TreeSpecies treeSpecies = null;
	
	private GameObject trunkObject;
	private GameObject leafObject;
	private CapsuleCollider collider;

	public void Reset() {
		// Destroy all generated content
		foreach ( Transform child in transform) {
			Destroy(child.gameObject);
		}
		Destroy(collider);
	}

	public void Init() { Init(m_seed); }

	public void Init(int seed)
	{

		Random.InitState(seed);

		treeSpecies = gameObject.GetComponent<TreeSpecies>();
		// Load variables from species creator
		if(treeSpecies == null) {
			Debug.LogError("ERROR: No tree species attached!");
		}
		GetVariablesFromSpecies();
		treeSpecies.GenerateMaterials();
		
		//LODManager lodManager = new LODManager();
		
		//trunkObject = new GameObject("Trunk");
		//trunkObject.transform.parent = transform;
		trunkObject = gameObject.EnsureChildGameObject("Trunk");
		trunkObject.transform.localPosition = Vector3.zero; // Remove offset from parenting
		PGTreeTrunkSimple trunkScript = trunkObject.EnsureComponent<PGTreeTrunkSimple>(); //trunkObject.AddComponent<PGTreeTrunkSimple>();
		trunkObject.EnsureComponent<MeshRenderer>(); //trunkObject.AddComponent<MeshRenderer>();
		trunkObject.GetComponent<Renderer>().material = treeSpecies.m_trunk_mat;
		
		trunkScript.CreateObject(false);
		
		// Add collider
		collider = trunkObject.EnsureComponent<CapsuleCollider>();
		collider.radius = (m_start_radius+m_end_radius)/2.0f;
		collider.height = m_height;
		collider.center = new Vector3(0.0f,m_height/2.0f-collider.radius,0.0f);
		
		if(treeSpecies.m_hasLeaves) {
			leafObject = gameObject.EnsureChildGameObject("Leaves");
			//leafObject.transform.parent = transform;
			leafObject.transform.localPosition = Vector3.zero; // Remove offset from parenting
			PGTreeLeaf leafScript = leafObject.EnsureComponent<PGTreeLeaf>();
			leafObject.EnsureComponent<MeshRenderer>();
			leafObject.GetComponent<Renderer>().material = treeSpecies.m_leaf_mat;

			leafScript.CreateObject(false);
		} else {
			Transform leafTransform = transform.Find("Leaves");
			if(leafTransform != null) {
				Destroy(leafTransform.gameObject);
			}
		}
		
		// Add fruit
		if(m_has_fruit) {
			int fruitNum = Random.Range(treeSpecies.m_fruit_num_min,treeSpecies.m_fruit_num_max+1);
			int leafPositionNum = trunkScript.Leaves.Count;
			int[] usedIndices = new int[fruitNum];
			
			for(int i = 0; i < fruitNum && i < leafPositionNum; i++) {
				GameObject newFruit = GameObject.Instantiate(treeSpecies.m_fruit) as GameObject;
				int fruitPositionIndex = Random.Range(0,leafPositionNum-1);
				bool spareIndex = false;
				int loopSafetyCount = 0;
				while (spareIndex && loopSafetyCount < leafPositionNum) {
					fruitPositionIndex = Random.Range(0,leafPositionNum-1);
					// Check if position index already has a fruit
					bool matchFound = false;
					for(int j = 0; j < fruitNum; j++) {
						if(fruitPositionIndex == usedIndices[j]) {
							matchFound = true;
						}
					}
					loopSafetyCount++;
					// If no match is found, no fruit is at that index
					spareIndex = !matchFound;
				}
				usedIndices[i] = fruitPositionIndex;
				newFruit.transform.parent = transform;
				newFruit.transform.localPosition = trunkScript.Leaves[fruitPositionIndex].position;
				newFruit.transform.rotation = trunkScript.Leaves[fruitPositionIndex].rotation*Quaternion.Euler(180.0f,0.0f,0.0f);
			}
		}
		
	}
	
	public void SetSpecies(TreeSpecies species) {
		treeSpecies = species;
	}
	
	private void GetVariablesFromSpecies() {
		m_height = treeSpecies.m_height;
		m_start_radius = treeSpecies.m_start_radius;
		m_end_radius = treeSpecies.m_end_radius;
		m_exp_mid_radius = treeSpecies.m_exp_mid_radius;
		m_twist_angle = treeSpecies.m_twist_angle;
		m_max_bend = treeSpecies.m_max_bend;
		
		m_branch_length = treeSpecies.m_branch_length;
		m_branch_min_fork_angle = treeSpecies.m_branch_min_fork_angle;
		m_branch_max_fork_angle = treeSpecies.m_branch_max_fork_angle;
		
		m_leaf_width = treeSpecies.m_leaf_width;
		m_leaf_length = treeSpecies.m_leaf_length;
		m_leaves_per_branch = treeSpecies.m_leaves_per_branch;
		
		m_branch_length_randomness = treeSpecies.m_branch_length_randomness;
		m_branch_length_falloff = treeSpecies.m_branch_length_falloff;
		
		m_branch_max_bend = treeSpecies.m_branch_max_bend;
		m_branch_bend_falloff = treeSpecies.m_branch_bend_falloff;
		m_branch_min_uprightness = treeSpecies.m_branch_min_uprightness;
		m_branch_segments = treeSpecies.m_branch_segments;
	
	
		m_branch_twist_angle = treeSpecies.m_twist_angle;
		m_branch_twist_randomness = treeSpecies.m_branch_twist_randomness;
		m_branch_twist_falloff = treeSpecies.m_branch_twist_falloff;
		
		m_branch_min_radius = treeSpecies.m_branch_min_radius;
		m_branch_radius_falloff = treeSpecies.m_branch_radius_falloff;
	}
}


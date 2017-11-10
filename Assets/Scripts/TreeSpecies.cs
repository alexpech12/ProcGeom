using UnityEngine;
using System.Collections;

// A class to hold all genetic information involved in creating a type of tree.
public class TreeSpecies : MonoBehaviour {
	
	// Dictionary of tree variables
	//public Dictionary<string, Object> m_properties = new Dictionary<string, Object>();

	// Tree properties
	public string species_name = "DefaultTreeSpecies";
	public float rarity = 1.0f;
	
	
	// Mesh creation variables
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
	
	public GameObject m_fruit; // The GameObject to use to instantiate fruit
	public Material m_fruit_mat;
	public int m_fruit_texture_size = 128;
	
	public float m_fruit_size = 1.0f;
	public float m_fruit_size_randomness = 0.2f;
	public int m_fruit_num_min = 1;
	public int m_fruit_num_max = 8;
	
	// Texture variables
	public Texture2D m_trunk_texture;
	public Material m_trunk_mat;
	public int m_trunk_texture_size = 128;
	
	public Color[] m_tree_colors = new Color[5];
	public float[] m_color_thresholds = new float[4];
	public float[] m_noise_scales = new float[4];
	public Vector2[] m_noise_offsets = new Vector2[4];
	public float[] m_noise_opacity = new float[4];
	
	public Texture2D m_leaf_texture;
	public Material m_leaf_mat;
	public int m_leaf_texture_size = 128 ;
	
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
	
	// Color variables
	Color treeColor = new Color(0.36f,0.27f,0.18f,1.0f);
	Color treeColorVar = new Color(0.1f,0.1f,0.1f,0.0f);
	Color treeColorSpecial = new Color(0.5f,0.5f,0.5f,1.0f);
	Color treeColorSpecialVar = new Color(0.4f,0.4f,0.4f,1.0f);
	float specialChance = 0.1f;
	
	public void SetProperty<T>(string property, T value)
	{
		//this.GetType().GetField(property)
	}

	public void CreateNewSpecies() {
		// Generate random variables
		m_height = Random.Range(1.0f,10.0f);
		m_start_radius = Random.Range(3.0f,0.5f);
		m_end_radius = Random.Range(m_start_radius-0.1f,0.1f);
		m_exp_mid_radius = m_end_radius+0.05f;
		m_twist_angle = Random.Range(-2.0f*Mathf.PI,2.0f*Mathf.PI);
		m_max_bend = Random.Range(0.0f,45.0f);
		
		m_branch_length = Random.Range(0.5f,5.0f);
		m_branch_min_fork_angle = Random.Range(0.0f, 60.0f);
		m_branch_max_fork_angle = Random.Range(m_branch_min_fork_angle,80f);
		
		m_leaves_per_branch = Random.Range(1,7);
		m_leaf_width = Random.Range(0.25f,2.0f);
		float leaf_proportion = Random.Range(0.5f,4.0f);
		m_leaf_length = leaf_proportion*m_leaf_width;
		
		m_branch_length_randomness = Random.Range(0.0f,m_branch_length/4.0f);
		m_branch_length_falloff = Random.Range(0.9f,1.0f);
		
		m_branch_max_bend = Random.Range(0.0f,180.0f);
		m_branch_bend_falloff = Random.Range(0.5f,1.5f);
		m_branch_min_uprightness = Random.Range(0.0f,0.7f);
		// Make branch segments proportional to branch bending
		m_branch_segments = (int)Mathf.RoundToInt(m_branch_max_bend/15.0f);
	
	
		m_branch_twist_angle = Random.Range(0.0f,360.0f);
		m_branch_twist_randomness = Random.Range(0.0f,90.0f);
		m_branch_twist_falloff = Random.Range(0.5f,1.5f);
		
		// In future, change this to control polycount
		m_branch_min_radius = Random.Range(0.5f,0.1f);//0.1f;
		m_branch_radius_falloff = 1.0f;
		
		// Generate fruit
		if(m_has_fruit) {
			m_fruit = GenerateFruit();
		}
		
		// Generate tree texture
		GenerateMaterials();
	}
	
	public void GenerateMaterials() {
		m_trunk_mat = new Material(Shader.Find("Diffuse"));
		m_trunk_mat.SetTexture("_MainTex", GenerateTrunkTexture(m_trunk_texture_size));
		
		m_leaf_mat = new Material(Shader.Find("Transparent/Cutout/Diffuse"));
		m_leaf_mat.SetTexture("_MainTex",GenerateLeafTexture(m_leaf_texture_size));
	} 

	private Texture2D GenerateTrunkTexture(int size) {
		
		TextureBuilder texturer = new TextureBuilder();
		
		// Randomize colors
		Color treeColor = new Color(0.36f,0.27f,0.18f,1.0f);
		Color treeColorVar = new Color(0.1f,0.1f,0.1f,0.0f);
		Color treeColorSpecial = new Color(0.5f,0.5f,0.5f,1.0f);
		Color treeColorSpecialVar = new Color(0.4f,0.4f,0.4f,1.0f);
		float specialChance = 0.1f;
		
		Color[] randomTreeColors = new Color[4];
		if(Random.Range(0.00f,1.00f) > specialChance) {
			// Use normal values
			randomTreeColors[0] = new Color(Random.Range(treeColor.r-treeColorVar.r,treeColor.r+treeColorVar.r),
											Random.Range(treeColor.g-treeColorVar.g,treeColor.g+treeColorVar.g),
											Random.Range(treeColor.b-treeColorVar.b,treeColor.b+treeColorVar.b),1.0f);
			for(int i = 1; i <= 3; i++) {
				randomTreeColors[i] = randomTreeColors[0]*new Color(Random.Range(treeColor.r-treeColorVar.r,treeColor.r+treeColorVar.r),
																	Random.Range(treeColor.g-treeColorVar.g,treeColor.g+treeColorVar.g),
																	Random.Range(treeColor.b-treeColorVar.b,treeColor.b+treeColorVar.b),1.0f);
			}
		} else {
			// Use special values
			randomTreeColors[0] = new Color(Random.Range(treeColorSpecial.r-treeColorSpecialVar.r,treeColorSpecial.r+treeColorSpecialVar.r),
											Random.Range(treeColorSpecial.g-treeColorSpecialVar.g,treeColorSpecial.g+treeColorSpecialVar.g),
											Random.Range(treeColorSpecial.b-treeColorSpecialVar.b,treeColorSpecial.b+treeColorSpecialVar.b),1.0f);
			for(int i = 1; i <= 3; i++) {
				randomTreeColors[i] = randomTreeColors[0]*new Color(Random.Range(treeColorSpecial.r-treeColorSpecialVar.r,treeColorSpecial.r+treeColorSpecialVar.r),
																	Random.Range(treeColorSpecial.g-treeColorSpecialVar.g,treeColorSpecial.g+treeColorSpecialVar.g),
																	Random.Range(treeColorSpecial.b-treeColorSpecialVar.b,treeColorSpecial.b+treeColorSpecialVar.b),1.0f);
			}
		}
		
		texturer.ColorTexture(size,randomTreeColors[0]);
		//texturer.AddPerlinNoise(50.0f,0.5f,-0.5f);
		//for (int i = 0; i < 4; i++) {
		//	texturer.OverlaySolidNoise(m_tree_colors[i+1],m_color_thresholds[i],m_noise_scales[i],m_noise_offsets[i],m_noise_opacity[i]);
		//}
		texturer.OverlayStripes(randomTreeColors[1],8.0f,0.2f,75.0f,0.7f);
		texturer.OverlayStripes(randomTreeColors[2],10.0f,0.4f,80.0f,0.7f);
		
		texturer.GetTexture().Apply();
		return texturer.GetTexture();
	}
	
	private Texture2D GenerateLeafTexture(int size) {
		
		TextureBuilder texturer = new TextureBuilder();
		
		Color leafColor = new Color(0.1f,0.5f,0.1f,1.0f);
		if(m_leaf_randomize_colors) {
			/*
			Color[] randomColors = new Color[4];
			randomColors[0] = new Color(Random.Range(0.2f,0.8f),Random.Range(0.2f,0.8f),Random.Range(0.2f,0.8f),1.0f);
			for(int i = 1; i <= 3; i++) {
				randomColors[i] = randomColors[0]*new Color(Random.Range(0.2f,0.8f),Random.Range(0.2f,0.8f),Random.Range(0.2f,0.8f),1.0f);
			}
			texturer.Color4GridTexture(size,randomColors);
			*/
			Color[] randomColors = new Color[4];
			if(Random.Range(0.00f,1.00f) > specialChance) {
				// Use normal values
				
				randomColors[0] = new Color(Random.Range(leafColor.r-treeColorVar.r,leafColor.r+treeColorVar.r),
												Random.Range(leafColor.g-treeColorVar.g,leafColor.g+treeColorVar.g),
												Random.Range(leafColor.b-treeColorVar.b,leafColor.b+treeColorVar.b),1.0f);
				for(int i = 1; i <= 3; i++) {
					randomColors[i] = randomColors[0]*new Color(Random.Range(leafColor.r-treeColorVar.r,leafColor.r+treeColorVar.r),
																		Random.Range(leafColor.g-treeColorVar.g,leafColor.g+treeColorVar.g),
																		Random.Range(leafColor.b-treeColorVar.b,leafColor.b+treeColorVar.b),1.0f);
				}
			} else {
				// Use special values
				randomColors[0] = new Color(Random.Range(treeColorSpecial.r-treeColorSpecialVar.r,treeColorSpecial.r+treeColorSpecialVar.r),
												Random.Range(treeColorSpecial.g-treeColorSpecialVar.g,treeColorSpecial.g+treeColorSpecialVar.g),
												Random.Range(treeColorSpecial.b-treeColorSpecialVar.b,treeColorSpecial.b+treeColorSpecialVar.b),1.0f);
				for(int i = 1; i <= 3; i++) {
					randomColors[i] = randomColors[0]*new Color(Random.Range(treeColorSpecial.r-treeColorSpecialVar.r,treeColorSpecial.r+treeColorSpecialVar.r),
																		Random.Range(treeColorSpecial.g-treeColorSpecialVar.g,treeColorSpecial.g+treeColorSpecialVar.g),
																		Random.Range(treeColorSpecial.b-treeColorSpecialVar.b,treeColorSpecial.b+treeColorSpecialVar.b),1.0f);
				}
			}
			texturer.Color4GridTexture(size,randomColors);
		} else {
			texturer.Color4GridTexture(size,m_leaf_colors);
		}
		//texturer.OverlaySolidNoise(m_leaf_color-new Color(0.3f,0.3f,0.3f,0.0f),0.5f,20.0f,new Vector2(0.0f,0.0f),1.0f);
		//texturer.OverlaySolidNoise(m_leaf_color,0.6f,20.0f,new Vector2(0.0f,0.0f),1.0f);
		
		if(m_leaf_randomize_shape) {
			texturer.CutOutLeafShape(1,8,true);
		} else {
			// Default leaf shape
			TextureBuilder.LeafCurvePoint[] leafPoints = new TextureBuilder.LeafCurvePoint[2];
			//Vector2 up = new Vector2(0.0f,0.1f);
			//Vector2 down = new Vector2(0.0f,-0.1f);
			leafPoints[0].start = new Vector2(0.5f,0.0f);
			leafPoints[0].mid1 = leafPoints[0].start+new Vector2(0.0f,0.5f);
			leafPoints[0].end = new Vector2(0.2f,0.7f);
			leafPoints[0].mid2 = leafPoints[0].end + new Vector2(0.0f,-0.5f);
			leafPoints[1].start = new Vector2(0.2f,0.7f);
			leafPoints[1].mid1 = leafPoints[1].start+new Vector2(0.2f,-0.3f);
			leafPoints[1].end = new Vector2(0.5f,1.0f);
			leafPoints[1].mid2 = leafPoints[1].end + new Vector2(-0.1f,-0.3f);
			texturer.CutOutLeafShape(leafPoints,2,true);
		}
		
		texturer.GetTexture().Apply();
		
		return texturer.GetTexture();
	}
	
	private GameObject GenerateFruit() {
		GameObject fruitObject = new GameObject("Fruit");
		FruitBase fruitScript = fruitObject.AddComponent<FruitBase>();
		fruitScript.InitialiseMesh();
		m_fruit_mat = new Material(Shader.Find("Diffuse"));
		m_fruit_mat.SetTexture("_MainTex",GenerateFruitTexture());
		fruitObject.AddComponent<MeshRenderer>();
		fruitObject.GetComponent<Renderer>().material = m_fruit_mat;
		return fruitObject;
	}
	
	private Texture2D GenerateFruitTexture() {
		TextureBuilder texturer = new TextureBuilder();
		
		int size = m_fruit_texture_size;
		
		// Randomize colors
		Color fruitColor = new Color(Random.Range(0.1f,0.9f),Random.Range(0.1f,0.9f),Random.Range(0.1f,0.9f),1.0f);
		
		texturer.ColorTexture(size,fruitColor);
		int randPattern = Random.Range(0,3);
		switch (randPattern) {
		case 0:
			texturer.AddPerlinNoise(Random.Range(2.0f,100.0f),0.2f,-0.5f);
			break;
		case 1:
			Color fruitColor1 =  new Color(Random.Range(0.1f,0.9f),Random.Range(0.1f,0.9f),Random.Range(0.1f,0.9f),1.0f);
			texturer.OverlaySolidNoise(fruitColor1,Random.Range(0.1f,0.9f),Random.Range(1.0f,50.0f),Vector2.zero,1.0f);
			break;
		case 2:
			Color fruitColor2 = new Color(Random.Range(0.1f,0.9f),Random.Range(0.1f,0.9f),Random.Range(0.1f,0.9f),1.0f);
			texturer.OverlayStripes(fruitColor2,Random.Range (1.0f,10.0f),Random.Range(0.2f,0.8f),Random.Range(0.0f,180.0f),1.0f);
			break;
		}
		texturer.GetTexture().Apply();
		return texturer.GetTexture();
	}
	
	/*
	// Apply textures to objects
		
		TextureBuilder texturer = new TextureBuilder();
		int size = 128;
		//texturer.RandomTexture(size);
		
		// Randomize colors
		Color treeColor = new Color(0.36f,0.27f,0.18f,1.0f);
		Color treeColorVar = new Color(0.1f,0.1f,0.1f,0.0f);
		Color treeColorSpecial = new Color(0.5f,0.5f,0.5f,1.0f);
		Color treeColorSpecialVar = new Color(0.4f,0.4f,0.4f,1.0f);
		float specialChance = 0.1f;
		
		Color[] randomTreeColors = new Color[4];
		if(Random.Range(0.00f,1.00f) > specialChance) {
			// Use normal values
			randomTreeColors[0] = new Color(Random.Range(treeColor.r-treeColorVar.r,treeColor.r+treeColorVar.r),
											Random.Range(treeColor.g-treeColorVar.g,treeColor.g+treeColorVar.g),
											Random.Range(treeColor.b-treeColorVar.b,treeColor.b+treeColorVar.b),1.0f);
			for(int i = 1; i <= 3; i++) {
				randomTreeColors[i] = randomTreeColors[0]*new Color(Random.Range(treeColor.r-treeColorVar.r,treeColor.r+treeColorVar.r),
																	Random.Range(treeColor.g-treeColorVar.g,treeColor.g+treeColorVar.g),
																	Random.Range(treeColor.b-treeColorVar.b,treeColor.b+treeColorVar.b),1.0f);
			}
		} else {
			// Use special values
			randomTreeColors[0] = new Color(Random.Range(treeColorSpecial.r-treeColorSpecialVar.r,treeColorSpecial.r+treeColorSpecialVar.r),
											Random.Range(treeColorSpecial.g-treeColorSpecialVar.g,treeColorSpecial.g+treeColorSpecialVar.g),
											Random.Range(treeColorSpecial.b-treeColorSpecialVar.b,treeColorSpecial.b+treeColorSpecialVar.b),1.0f);
			for(int i = 1; i <= 3; i++) {
				randomTreeColors[i] = randomTreeColors[0]*new Color(Random.Range(treeColorSpecial.r-treeColorSpecialVar.r,treeColorSpecial.r+treeColorSpecialVar.r),
																	Random.Range(treeColorSpecial.g-treeColorSpecialVar.g,treeColorSpecial.g+treeColorSpecialVar.g),
																	Random.Range(treeColorSpecial.b-treeColorSpecialVar.b,treeColorSpecial.b+treeColorSpecialVar.b),1.0f);
			}
		}
		
		
		texturer.ColorTexture(size,randomTreeColors[0]);
		//texturer.AddPerlinNoise(50.0f,0.5f,-0.5f);
		//for (int i = 0; i < 4; i++) {
		//	texturer.OverlaySolidNoise(m_tree_colors[i+1],m_color_thresholds[i],m_noise_scales[i],m_noise_offsets[i],m_noise_opacity[i]);
		//}
		texturer.OverlayStripes(randomTreeColors[1],8.0f,0.2f,75.0f,0.7f);
		texturer.OverlayStripes(randomTreeColors[2],10.0f,0.4f,80.0f,0.7f);
		//texturer.OverlaySpots(m_tree_colors[4],7.0f,0.4f,1.0f);
		//texturer.SetFilterMode(FilterMode.Point);
		texturer.AssignTexture(trunkObject,Shader.Find("Diffuse"));
		
		//texturer.ColorTexture(size,new Color(0.2f,0.9f,0.3f,1.0f));
		//texturer.ColorTexture(size,m_leaf_color);
		Color leafColor = new Color(0.1f,0.5f,0.1f,1.0f);
		
		
		if(m_leaf_randomize_colors) {
			Color[] randomColors = new Color[4];
			if(Random.Range(0.00f,1.00f) > specialChance) {
				// Use normal values
				
				randomColors[0] = new Color(Random.Range(leafColor.r-treeColorVar.r,leafColor.r+treeColorVar.r),
												Random.Range(leafColor.g-treeColorVar.g,leafColor.g+treeColorVar.g),
												Random.Range(leafColor.b-treeColorVar.b,leafColor.b+treeColorVar.b),1.0f);
				for(int i = 1; i <= 3; i++) {
					randomColors[i] = randomColors[0]*new Color(Random.Range(leafColor.r-treeColorVar.r,leafColor.r+treeColorVar.r),
																		Random.Range(leafColor.g-treeColorVar.g,leafColor.g+treeColorVar.g),
																		Random.Range(leafColor.b-treeColorVar.b,leafColor.b+treeColorVar.b),1.0f);
				}
			} else {
				// Use special values
				randomColors[0] = new Color(Random.Range(treeColorSpecial.r-treeColorSpecialVar.r,treeColorSpecial.r+treeColorSpecialVar.r),
												Random.Range(treeColorSpecial.g-treeColorSpecialVar.g,treeColorSpecial.g+treeColorSpecialVar.g),
												Random.Range(treeColorSpecial.b-treeColorSpecialVar.b,treeColorSpecial.b+treeColorSpecialVar.b),1.0f);
				for(int i = 1; i <= 3; i++) {
					randomColors[i] = randomColors[0]*new Color(Random.Range(treeColorSpecial.r-treeColorSpecialVar.r,treeColorSpecial.r+treeColorSpecialVar.r),
																		Random.Range(treeColorSpecial.g-treeColorSpecialVar.g,treeColorSpecial.g+treeColorSpecialVar.g),
																		Random.Range(treeColorSpecial.b-treeColorSpecialVar.b,treeColorSpecial.b+treeColorSpecialVar.b),1.0f);
				}
			}
			texturer.Color4GridTexture(size,randomColors);
		} else {
			texturer.Color4GridTexture(size,m_leaf_colors);
		}
		//texturer.OverlaySolidNoise(m_leaf_color-new Color(0.3f,0.3f,0.3f,0.0f),0.5f,20.0f,new Vector2(0.0f,0.0f),1.0f);
		//texturer.OverlaySolidNoise(m_leaf_color,0.6f,20.0f,new Vector2(0.0f,0.0f),1.0f);
		
		if(m_leaf_randomize_shape) {
			texturer.CutOutLeafShape(1,8,true);
		} else {
			// Default leaf shape
			TextureBuilder.LeafCurvePoint[] leafPoints = new TextureBuilder.LeafCurvePoint[2];
			//Vector2 up = new Vector2(0.0f,0.1f);
			//Vector2 down = new Vector2(0.0f,-0.1f);
			leafPoints[0].start = new Vector2(0.5f,0.0f);
			leafPoints[0].mid1 = leafPoints[0].start+new Vector2(0.0f,0.5f);
			leafPoints[0].end = new Vector2(0.2f,0.7f);
			leafPoints[0].mid2 = leafPoints[0].end + new Vector2(0.0f,-0.5f);
			leafPoints[1].start = new Vector2(0.2f,0.7f);
			leafPoints[1].mid1 = leafPoints[1].start+new Vector2(0.2f,-0.3f);
			leafPoints[1].end = new Vector2(0.5f,1.0f);
			leafPoints[1].mid2 = leafPoints[1].end + new Vector2(-0.1f,-0.3f);
			texturer.CutOutLeafShape(leafPoints,2,true);
		}
		*/
}

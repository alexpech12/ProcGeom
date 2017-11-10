using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class TreeSpeciesSliderUpdate : MonoBehaviour {

	public GameObject m_listContainer;

	public GameObject m_floatSlider; // Prefab to instantiate

	public float sliderSpacing = 50.0f;

	TreeSpecies species;

	int sliderCount = 0;


	// Use this for initialization
	void Start () {
		species = GetComponent<TreeSpecies>();
		CreateSlider("Trunk Height", "m_height", 1.0f, 5.0f);
		CreateSlider("Trunk Start Radius", "m_start_radius", 0.5f, 5.0f);
		CreateSlider("Trunk End Radius", "m_end_radius", 0.5f, 5.0f);
		CreateSlider("Trunk Twist Angle", "m_twist_angle", 0.0f, 180.0f);
		CreateSlider("Trunk Max Bend", "m_max_bend", 0.0f, 90.0f);
		//CreateSlider("Blending", "m_expolinear_blend", 0.0f, 1.0f);
		CreateSlider("Irregularity", "m_start_irregularity", 0.0f, 1.0f);
		CreateSlider("Irregularity Falloff", "m_irregularity_falloff", 0.0f, 10.0f);


		CreateSlider("Branch Length", "m_branch_length", 0.5f, 10.0f);
		CreateSlider("Branch Length Randomness", "m_branch_length_randomness", 0.0f, 5.0f);
		CreateSlider("Branch Length Falloff", "m_branch_length_falloff", 0.0f, 2.0f);
		CreateSlider("Branch Max Bend", "m_branch_max_bend", 0.0f, 90.0f);
		CreateSlider("Branch Bend Falloff", "m_branch_bend_falloff", 0.0f, 5.0f);
		CreateSlider("Branch Min Upright", "m_branch_min_uprightness", 0.0f, 10.0f);
		CreateSlider("Branch Fork Angle Min", "m_branch_min_fork_angle", 0.0f, 90.0f);
		CreateSlider("Branch Fork Angle Max", "m_branch_max_fork_angle", 0.0f, 90.0f);
		CreateSlider("Branch Twist Angle", "m_branch_twist_angle", 0.0f, 90.0f);
		CreateSlider("Branch Twist Randomness", "m_branch_twist_randomness", 0.0f, 30.0f);
		CreateSlider("Branch Twist Falloff", "m_branch_twist_falloff", 0.0f, 2.0f);
		CreateSlider("Branch Min Radius", "m_branch_min_radius", 0.0f, 10.0f);
		CreateSlider("Branch Radius Falloff", "m_branch_radius_falloff", 0.0f, 10.0f);



	//public float m_branch_length = 2.0f;
	//public float m_branch_length_randomness = 1.0f;
	//public float m_branch_length_falloff = 0.8f;
	//public int m_branch_segments = 3;
	
	//public float m_branch_max_bend = 45f;
	//public float m_branch_bend_falloff = 0.8f;
	//public float m_branch_min_uprightness = 0.5f;
	
	//public float m_branch_min_fork_angle = 0f;
	//public float m_branch_max_fork_angle = 75f;
	//public float m_branch_twist_angle = 90f;
	//public float m_branch_twist_randomness = 15f;
	//public float m_branch_twist_falloff = 0.8f;
	
	//public float m_branch_min_radius = 0.1f;
	//public float m_branch_radius_falloff = 1.0f;

	//public int m_radial_segments = 8;
	//public int m_height_segments = 10;

	}
	
	void CreateSlider(string name, string property, float minValue, float maxValue) {
		GameObject newSliderObj = Instantiate(m_floatSlider, m_listContainer.transform) as GameObject;
		// Set name text
		newSliderObj.transform.Find("Name").GetComponent<Text>().text = name;
		RectTransform rt = newSliderObj.GetComponent<RectTransform>();
		rt.anchoredPosition = new Vector2(0,-sliderCount*sliderSpacing);

		Slider newSlider = newSliderObj.GetComponentInChildren<Slider>();
		newSlider.minValue = minValue;
		newSlider.maxValue = maxValue;
		newSlider.value = (float)species.GetType().GetField(property).GetValue(species);
		newSlider.onValueChanged.AddListener( (value) => { species.GetType().GetField(property).SetValue(species, value); } );
		sliderCount++;
	}

	// Update is called once per frame
	void Update () {
	
	}
}

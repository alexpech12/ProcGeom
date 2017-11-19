using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using System.Reflection;

public class TreeSpeciesSliderUpdate : MonoBehaviour {

	public GameObject m_listContainer;

	// Slider prefabs to instantiate
	public GameObject m_sliderPrefab;
	public GameObject m_togglePrefab;
	public GameObject m_dropDownPrefab;
	public GameObject m_colorSelectorPrefab;

	bool m_liveUpdating = true; public bool LiveUpdating { get { return m_liveUpdating; } set { m_liveUpdating = value; } }

	TreeSpecies species;


	// Use this for initialization
	void Start () {
		species = GetComponent<TreeSpecies>();
		CreateSlider("Trunk Height", "m_height", 1.0f, 10.0f);
		CreateSlider("Trunk Start Radius", "m_start_radius", 0.2f, 5.0f);
		CreateSlider("Trunk End Radius", "m_end_radius", 0.1f, 5.0f);
		CreateSlider("Trunk Twist Angle", "m_twist_angle", -180.0f, 180.0f);
		CreateSlider("Trunk Max Bend", "m_max_bend", -90.0f, 90.0f);
		CreateDropdown("Trunk Curve Type", "m_trunk_curve_type_s");
		//CreateSlider("Blending", "m_expolinear_blend", 0.0f, 1.0f);
		CreateSlider("Radial Segments", "m_radial_segments",3,24);
		CreateSlider("Height Segments", "m_height_segments",1,12);
		CreateSlider("Trunk Frequency 1", "m_trunk_freq_1", 1, 8);
		CreateSlider("Trunk Freq Offset 1", "m_trunk_freq_off_1", 0.0f, 2*Mathf.PI);
		CreateSlider("Trunk Frequency 2", "m_trunk_freq_2", 1, 8);
		CreateSlider("Trunk Freq Offset 2", "m_trunk_freq_off_2", 0.0f, 2*Mathf.PI);
		CreateSlider("Irregularity", "m_trunk_irregularity", 0.0f, 1.0f);
		CreateSlider("Irregularity Coeff", "m_trunk_irregularity_coeff", 0.0f, 5.0f);
		
		//CreateSlider("Irregularity Falloff", "m_irregularity_falloff", 0.0f, 10.0f);

		CreateSlider("Branch Segments", "m_branch_segments",1,10);

		CreateSlider("Branch Length", "m_branch_length", 0.5f, 10.0f);
		CreateSlider("Branch Length Randomness", "m_branch_length_randomness", 0.0f, 5.0f);
		CreateSlider("Branch Length Falloff", "m_branch_length_falloff", 0.0f, 2.0f);
		CreateSlider("Branch Max Bend", "m_branch_max_bend", 0.0f, 90.0f);
		CreateSlider("Branch Bend Falloff", "m_branch_bend_falloff", 0.0f, 5.0f);
		CreateSlider("Branch Min Upright", "m_branch_min_uprightness", 0.0f, 0.95f);
		CreateSlider("Branch Fork Angle", "m_branch_fork_angle", 0f,90.0f);
		CreateSlider("Branch Fork Angle Min", "m_branch_min_fork_angle", -90.0f, 90.0f);
		CreateSlider("Branch Fork Angle Max", "m_branch_max_fork_angle", -90.0f, 90.0f);
		CreateSlider("Branch Twist Angle", "m_branch_twist_angle", -180.0f, 180.0f);
		CreateSlider("Branch Twist Randomness", "m_branch_twist_randomness", -30.0f, 30.0f);
		CreateSlider("Branch Twist Falloff", "m_branch_twist_falloff", 0.0f, 2.0f);
		CreateSlider("Branch Min Radius", "m_branch_min_radius", 0.1f, 2.0f);
		CreateSlider("Branch Radius Falloff", "m_branch_radius_falloff", 0.1f, 2.0f);

		CreateToggle("Has Leaves", "m_hasLeaves");

		CreateSlider("Leaves Per Branch", "m_leaves_per_branch", 1,8);
		CreateSlider("Leaf Length", "m_leaf_length",0.1f,5.0f);
		CreateSlider("Leaf Width", "m_leaf_width",0.1f,5.0f);

		CreateColorSelector("Tree Color", "treeColor");
		CreateColorSelector("Tree Color Variance", "treeColorVar");
		CreateColorSelector("Tree Color Special", "treeColorSpecial");
		CreateColorSelector("Tree Color Special Variance", "treeColorSpecialVar");
		CreateSlider("Special Chance", "specialChance", 0.0f,1.0f);



	}
	
	// Default float slider
	Slider CreateSlider(string name, string property, float minValue, float maxValue) {
		return CreateSlider(name, property, minValue, maxValue, false);
	} 

	Slider CreateSlider(string name, string property, int minValue, int maxValue) {
		return CreateSlider(name, property, (float)minValue, (float)maxValue, true);
	}

	Slider CreateSlider(string name, string property, float minValue, float maxValue, bool wholeNumbers)
	{
		GameObject newSliderObj = Instantiate(m_sliderPrefab, m_listContainer.transform) as GameObject;
		// Set name text
		newSliderObj.transform.Find("Name").GetComponent<Text>().text = name;

		Slider newSlider = newSliderObj.GetComponentInChildren<Slider>();
		newSlider.minValue = minValue;
		newSlider.maxValue = maxValue;
		newSlider.wholeNumbers = wholeNumbers;
		if(wholeNumbers) {
			newSlider.value = (int)species.GetType().GetField(property).GetValue(species);
			newSlider.onValueChanged.AddListener( (value) => { 
				species.GetType().GetField(property).SetValue(species, (int)value);
				if (m_liveUpdating) {
					species.transform.GetComponent<TreeTest>().RefreshTree();
				}
			});
		} else {
			newSlider.value = (float)species.GetType().GetField(property).GetValue(species);
			newSlider.onValueChanged.AddListener( (value) => { 
				species.GetType().GetField(property).SetValue(species, (float)value);
				if (m_liveUpdating) { species.transform.GetComponent<TreeTest>().RefreshTree(); }
			});
		}
		
		return newSlider;
	}

	Toggle CreateToggle(string name, string property) {
		GameObject newToggleObj = Instantiate(m_togglePrefab, m_listContainer.transform) as GameObject;
		
		Toggle newToggle = newToggleObj.GetComponentInChildren<Toggle>();
		// Set name text
		newToggle.GetComponentInChildren<Text>().text = name;

		newToggle.isOn = (bool)species.GetType().GetField(property).GetValue(species);
		newToggle.onValueChanged.AddListener( (value) => { 
			species.GetType().GetField(property).SetValue(species, (bool)value);
			if (m_liveUpdating) { species.transform.GetComponent<TreeTest>().RefreshTree(); }
		});

		return newToggle;
	}

	Dropdown CreateDropdown(string name, string property) {
		GameObject newDropdownObj = Instantiate(m_dropDownPrefab, m_listContainer.transform) as GameObject;

		Dropdown newDropdown = newDropdownObj.GetComponentInChildren<Dropdown>();
		
		// Get type of property
		string[] dropdownNames = System.Enum.GetNames (species.GetType().GetField(property).FieldType);
		newDropdown.ClearOptions();
		newDropdown.AddOptions(new List<string>(dropdownNames));
	     
		// Set name text
		newDropdownObj.transform.Find("Name").GetComponent<Text>().text = name;

		newDropdown.value = (int)species.GetType().GetField(property).GetValue(species);
		newDropdown.onValueChanged.AddListener( (value) => { 
			Debug.Log("Setting value to " + value);
			species.GetType().GetField(property).SetValue(species, (int)value);
			if (m_liveUpdating) { species.transform.GetComponent<TreeTest>().RefreshTree(); }
		});

		return newDropdown;
	}

	void CreateColorSelector(string name, string property) {
		GameObject newColorObj = Instantiate(m_colorSelectorPrefab, m_listContainer.transform) as GameObject;
		// Set name text
		newColorObj.transform.Find("Name").GetComponent<Text>().text = name;

		// Set initial values for sliders
		Color speciesColor = (Color)species.GetType().GetField(property).GetValue(species);

		ColorSlider[] sliders = newColorObj.transform.GetComponentsInChildren<ColorSlider>();

		foreach (ColorSlider slider in sliders) {
			int colorRef = (int)slider.color;
			slider.transform.GetComponent<Slider>().value = speciesColor[colorRef];
			slider.transform.GetComponent<Slider>().onValueChanged.AddListener( (value) => {
				Color newColor = (Color)species.GetType().GetField(property).GetValue(species);
				newColor[colorRef] = value;
				species.GetType().GetField(property).SetValue(species, newColor);
				if (m_liveUpdating) { species.transform.GetComponent<TreeTest>().RefreshTree(); }
			});
		}
	}

	// Update is called once per frame
	void Update () {
	
	}
}

using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class SliderTextOutput : MonoBehaviour {

	Text text;
	Slider slider;

	// Use this for initialization
	void Awake () {
		text = GetComponent<Text>();
		slider = transform.parent.GetComponentInChildren<Slider>();
		slider.onValueChanged.AddListener( delegate { UpdateText(); } );
		UpdateText();
	}

	void UpdateText() {
		text.text = slider.value.ToString("F2");
	}
}

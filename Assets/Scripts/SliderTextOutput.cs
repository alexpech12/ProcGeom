using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class SliderTextOutput : MonoBehaviour {

	Text text;
	Slider slider;

	string rounding = "F2"; // 2 decimal points by default
	bool wholeNumbers = false;

	// Use this for initialization
	void Start () {
		text = GetComponent<Text>();
		slider = transform.parent.GetComponentInChildren<Slider>();
		wholeNumbers = slider.wholeNumbers;
		slider.onValueChanged.AddListener( delegate { UpdateText(); } );
		UpdateText();
	}

	void UpdateText() {
		if(wholeNumbers) {
			text.text = slider.value.ToString();
		} else {
			text.text = slider.value.ToString(rounding);
		}
	}
}

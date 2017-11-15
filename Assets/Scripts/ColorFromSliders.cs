using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ColorFromSliders : MonoBehaviour {

	public Slider redSlider;
	public Slider greenSlider;
	public Slider blueSlider;
	public Slider alphaSlider;

	RawImage colorImage;

	// Use this for initialization
	void Start () {
		colorImage = GetComponent<RawImage>();
		SetColorFromSliders();
		redSlider.onValueChanged.AddListener(delegate { SetColorFromSliders(); });
		greenSlider.onValueChanged.AddListener(delegate { SetColorFromSliders(); });
		blueSlider.onValueChanged.AddListener(delegate { SetColorFromSliders(); });
		alphaSlider.onValueChanged.AddListener(delegate { SetColorFromSliders(); });

	}

	void SetColorFromSliders() {
		colorImage.color = new Color(
			redSlider.value,
			greenSlider.value,
			blueSlider.value,
			alphaSlider.value);
	}
}

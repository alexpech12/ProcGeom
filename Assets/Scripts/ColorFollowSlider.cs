using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ColorFollowSlider : MonoBehaviour {

	public Slider m_slider;
	public Color m_startColor = Color.black;
	public Color m_endColor = Color.white;

	Image colorImage;

	// Use this for initialization
	void Start () {
		colorImage = transform.GetComponent<Image>();
		UpdateColor(m_slider.value);
		m_slider.onValueChanged.AddListener( (value) => { 
			UpdateColor(value);
		});
	}

	void UpdateColor(float t) {
		colorImage.color = Color.Lerp(m_startColor, m_endColor, t);
	}
}

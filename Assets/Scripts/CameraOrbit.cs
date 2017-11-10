using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;

public class CameraOrbit : MonoBehaviour {

	public Transform target;

	public float rotationSpeed = 2.0f;
	public float zoomSpeed = 1.0f;
	public float heightSpeed = 10.0f;
	public float zoomMin = 1.0f;
	public float zoomMax = 30.0f;
	public float heightMin = 1.0f;
	public float heightMax = 10.0f;

	private float focusHeight = 10f;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		if(!EventSystem.current.IsPointerOverGameObject()) {
			if(Input.GetMouseButton(0)) {

				transform.RotateAround(target.position, Vector3.up, Input.GetAxis("Mouse X") * rotationSpeed);

				float zoomAmount = Input.GetAxis("Mouse Y") * zoomSpeed;
				Vector3 translation = transform.forward * zoomAmount;
				Vector3 distVec = target.position - (transform.position + translation);
				distVec.y = 0;
				float dist = distVec.magnitude;

				if(dist >= zoomMin) { // Can zoom in
					if(zoomAmount >= 0) {
						// We are zooming in. Do it.
						transform.Translate(translation, Space.World);
					}
				}
				if(dist <= zoomMax) { // Can zoom out
					if(zoomAmount <= 0) {
						// We are zooming out. Do it.
						transform.Translate(translation, Space.World);
					}

				}

			}


			float heightChange = Input.GetAxis("Mouse ScrollWheel") * heightSpeed;
			focusHeight += heightChange;

			focusHeight = Mathf.Clamp(focusHeight, heightMin, heightMax);

			PositionAboveTerrain heightScript = GetComponent<PositionAboveTerrain>();
			heightScript.yOffset = focusHeight;

			transform.LookAt(target.position + Vector3.up*focusHeight);
		}
	}
}

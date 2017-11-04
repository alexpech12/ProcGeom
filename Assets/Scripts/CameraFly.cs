using UnityEngine;
using System.Collections;

public class CameraFly : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	public float move_speed = 5;
	public float rotate_speed = 2;
	
	
	// Update is called once per frame
	void Update () {
		float move_speed_loc = move_speed;
		if(Input.GetKey(KeyCode.LeftShift)) {
			move_speed_loc = move_speed*1.5f;
		}
		if(Input.GetKey("w")) {
			// Move forward
			transform.Translate(Vector3.forward * move_speed_loc * Time.deltaTime);
		}
		if(Input.GetKey("s")) {
			transform.Translate(Vector3.back * move_speed_loc * Time.deltaTime);	
		}
		if(Input.GetKey("a")) {
			transform.Translate(Vector3.left * move_speed_loc * Time.deltaTime);	
		}
		if(Input.GetKey("d")) {
			transform.Translate(Vector3.right * move_speed_loc * Time.deltaTime);	
		}
		if(Input.GetKey("space")) {
			transform.Translate(Vector3.up * move_speed_loc * Time.deltaTime);	
		}
		if(Input.GetKey(KeyCode.LeftControl)) {
			transform.Translate(Vector3.down * move_speed_loc * Time.deltaTime);	
		}
		transform.Rotate(0, Input.GetAxis("Mouse X") * rotate_speed, 0);
		transform.Rotate(-Input.GetAxis("Mouse Y") * rotate_speed, 0, 0);
	}
}

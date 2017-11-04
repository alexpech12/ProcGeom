using UnityEngine;
using System.Collections;

public class ObjectThrower : MonoBehaviour {
	
	public GameObject objectTemplate;
	public float force = 1000.0f;
	
	private CharacterController controller;
	
	void Start() {
		controller = gameObject.GetComponent<CharacterController>();
	}
	
	// Update is called once per frame
	void Update () {
		if(Input.GetMouseButtonDown(0)) {
			GameObject newObject = Instantiate(objectTemplate,transform.position+transform.right*controller.radius,Quaternion.identity) as GameObject;
			newObject.GetComponent<Rigidbody>().AddForce(Camera.main.transform.forward*force);
		}
	}
}

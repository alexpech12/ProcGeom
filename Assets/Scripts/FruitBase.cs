using UnityEngine;
using System.Collections;

public class FruitBase : MonoBehaviour {
	
	public float radius = 0.5f;
	public float height = 1.0f;
	public int height_segments = 8;
	public int radial_segments = 8;
	
	private TreeSpecies species;
	
	// Use this for initialization
	public void InitialiseMesh() {
		radius = Random.Range(0.05f,0.5f);
		height = Random.Range(radius*2.0f,radius*6.0f);
		PGFruit fruitMesh = gameObject.AddComponent<PGFruit>();
		//gameObject.AddComponent<MeshRenderer>();
		//gameObject.renderer.material = new Material(Shader.Find("Diffuse"));
		fruitMesh.CreateObject();
		//PGTreeTrunkSimple trunkScript = trunkObject.GetComponent("PGTreeTrunkSimple") as PGTreeTrunkSimple;
		//trunkScript.CreateObject(true);
		
		
		
		// Add collider
		CapsuleCollider collider = gameObject.AddComponent<CapsuleCollider>();
		collider.radius = fruitMesh.maxRadius;
		collider.height = height;
		collider.center = new Vector3(0.0f,-height/2.0f,0.0f);
		
	}
	
	// Check for collisions
	void OnCollisionEnter(Collision collision) {
		Debug.Log("Collision!");
		Rigidbody rigidBody = gameObject.GetComponent<Rigidbody>();
		if(rigidBody == null) {
			Debug.Log ("Adding rigidbody...");
			gameObject.AddComponent<Rigidbody>();
			gameObject.GetComponent<Rigidbody>().angularDrag = 10.0f;
			//gameObject.rigidbody.AddForce(collision.rigidbody.velocity*collision.rigidbody.mass*100.0f);
		}
	}
	
	void SetSpecies(TreeSpecies newSpecies) {
		species = newSpecies;
	}
	
}

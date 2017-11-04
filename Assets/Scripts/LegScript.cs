using UnityEngine;
using System.Collections;

public class LegScript : MonoBehaviour {
	
	// Inspector variables
	public float thighLength = 4.0f;
	public float calfLength = 3.0f;
	public float footLength = 2.0f;
	public float toeLength = 1.0f;
	BodySegmentMeshData thData;
	BodySegmentMeshData cData;
	BodySegmentMeshData fData;
	BodySegmentMeshData toData;
	public float standKneeAngle = 60.0f;
	public float elevationAngle = 60.0f;
	public float standStraightness = 0.8f;
	
	public float walkSpeed = 2.0f;
	public float stepLength = 1.0f;
	
	
	// Private variables
	GameObject thighJoint;
	GameObject calfJoint;
	GameObject footJoint;
	GameObject toeJoint;
	
	GameObject legPivot;
	
	public float standHeight = 1.0f;
	
	float t = 0;
	
	
	// This function must be called first to set up variables
	public void InitialiseLeg(Vector3 offset, LegData data) {
		thData = data.thighData;
		cData = data.calfData;
		fData = data.footData;
		toData = data.toeData;
		
		// Get variables from creature species
		//
		//
		//
		//
		
		MeshFilter filter;
		MeshRenderer renderer;
		BodySegmentMesh meshScript;
		Material mat = new Material(Shader.Find("Diffuse"));
		
		thighLength = thData.length;
		calfLength = cData.length;
		footLength = fData.length;
		toeLength = toData.length;
		
		legPivot = new GameObject("legPivot");
		//legPivot.transform.position = new Vector3(0.0f,data.standHeight,0.0f);
		legPivot.transform.parent = transform;
		legPivot.transform.localPosition = Vector3.zero;
		
		// Create leg segments
			
		thighJoint = new GameObject("thighJoint");
		filter = thighJoint.AddComponent<MeshFilter>();
		renderer = thighJoint.AddComponent<MeshRenderer>();
		meshScript = thighJoint.AddComponent<BodySegmentMesh>();
		filter.sharedMesh = meshScript.BuildMesh(thData);
		renderer.material = mat;
		thighJoint.transform.parent = legPivot.transform;
		
		calfJoint = new GameObject("calfJoint");
		calfJoint.transform.parent = thighJoint.transform;
		filter = calfJoint.AddComponent<MeshFilter>();
		renderer = calfJoint.AddComponent<MeshRenderer>();
		meshScript = calfJoint.AddComponent<BodySegmentMesh>();
		filter.sharedMesh = meshScript.BuildMesh(cData);
		renderer.material = mat;
		
		footJoint = new GameObject("footJoint");
		footJoint.transform.parent = calfJoint.transform;
		filter = footJoint.AddComponent<MeshFilter>();
		renderer = footJoint.AddComponent<MeshRenderer>();
		meshScript = footJoint.AddComponent<BodySegmentMesh>();
		filter.sharedMesh = meshScript.BuildMesh(fData);
		renderer.material = mat;
		
		toeJoint = new GameObject("toeJoint");
		toeJoint.transform.parent = footJoint.transform;
		filter = toeJoint.AddComponent<MeshFilter>();
		renderer = toeJoint.AddComponent<MeshRenderer>();
		meshScript = toeJoint.AddComponent<BodySegmentMesh>();
		filter.sharedMesh = meshScript.BuildMesh(toData);
		renderer.material = mat;
		
		// Set up initial positions
		thighJoint.transform.localPosition = new Vector3(0.0f,0.0f,0.0f);
		thighJoint.transform.Rotate(0.0f,0.0f,180.0f);
		calfJoint.transform.Translate(0.0f,thighLength,0.0f);
		footJoint.transform.Translate(0.0f,calfLength,0.0f);
		toeJoint.transform.Translate(0.0f,footLength,0.0f);
		toeJoint.transform.Rotate(0.0f,0.0f,90.0f);
	}
}

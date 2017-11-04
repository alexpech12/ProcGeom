using UnityEngine;
using System.Collections;

public class LegSolverTest : MonoBehaviour {
	
	public Transform hipPoint;
	public Transform targetPoint;
	
	[Range(0.1f,3.0f)]
	public float hipWidth = 1.5f;
	[Range(0.4f,3.0f)]
	public float thighLength = 1.5f;
	[Range(0.4f,3.0f)]
	public float calfLength = 1.5f;
	[Range(0.4f,3.0f)]
	public float footLength = 1.0f;
	public bool reverseKnee = false;
	
	GameObject root, hipPivot, hip, thigh, calf, foot;
	GameObject thighPrimitive, calfPrimitive, footPrimitive;
	
	JointSolver solver;
	
	// Use this for initialization
	void Start () {
		
		// This code just creates all the GameObjects and primitives required for this test
		// Note on hip and hipPivot:
		// - hip is the topmost node of the leg and is the direct child of the overarcing GameObject
		// - hipPivot is a child of the root spine node
		// - hip is attached to hipPivot but does not take the rotation value
		root = new GameObject("root");
		hipPivot = new GameObject("hipPivot");
		hip = new GameObject("hip");
		thigh = new GameObject("thigh");
		thighPrimitive = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
		calf = new GameObject("calf");
		calfPrimitive = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
		foot = new GameObject("foot");
		footPrimitive = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
		
		// This code below shows the proper parent/child heirarchy for the animation controls to work
		// The primitives should be replaced with meshes in implementation
		root.transform.parent = transform;
		hipPivot.transform.parent = root.transform;
		hip.transform.parent = transform;
		thigh.transform.parent = hip.transform;
		thighPrimitive.transform.parent = thigh.transform;
		calf.transform.parent = thigh.transform;
		calfPrimitive.transform.parent = calf.transform;
		foot.transform.parent = calf.transform;
		footPrimitive.transform.parent = foot.transform;
		
		solver = gameObject.AddComponent<JointSolver>();
		
	}
	
	// Update is called once per frame
	void Update () {
		
		// This code just adjusts the primitives and has no effect on the joint solver
		thighPrimitive.transform.localPosition = new Vector3(0.0f,thighLength/2,0.0f);
		calfPrimitive.transform.localPosition = new Vector3(0.0f,calfLength/2,0.0f);
		footPrimitive.transform.localPosition = new Vector3(0.0f,footLength/2,0.0f);
		Vector3 thighScale = new Vector3(0.5f,thighLength/2,0.5f);
		Vector3 calfScale = new Vector3(0.5f,calfLength/2,0.5f);
		Vector3 footScale = new Vector3(0.5f,footLength/2,0.5f);
		thighPrimitive.transform.localScale = thighScale;
		calfPrimitive.transform.localScale = calfScale;
		footPrimitive.transform.localScale = footScale;
		
		// This code demonstrates how to set up joint positions and rotations based only on the 2 control transforms
		// All positions and rotations are local to properly work when parented under a single GameObject
		// During implementation, these would be set to animation clips and not updated in real time as below
		root.transform.localPosition = hipPoint.localPosition;
		root.transform.localRotation = hipPoint.localRotation;
		hipPivot.transform.localPosition = new Vector3(0.0f,0.0f,hipWidth);
		hipPivot.transform.localRotation = Quaternion.identity; // Not necessary as rotation of hipPivot is not used
		hip.transform.localPosition = hipPoint.localPosition+hipPoint.localRotation*new Vector3(0.0f,0.0f,hipWidth);
		calf.transform.localPosition = new Vector3(0.0f,thighLength,0.0f);
		foot.transform.localPosition = new Vector3(0.0f,calfLength,0.0f);
		
		Vector3 sideHipPoint = hipPoint.localPosition + hipPoint.localRotation*(new Vector3(0.0f,0.0f,hipWidth));
		JointSolver.LegSolverOutput output = solver.LegRotationSolver(sideHipPoint,targetPoint.localPosition,thighLength,calfLength,footLength,targetPoint.eulerAngles.z,targetPoint.eulerAngles.y,reverseKnee);
		hip.transform.localRotation = output.legPivotRotation;
		thigh.transform.localRotation = output.thighRotation;
		calf.transform.localRotation = output.calfRotation;
		foot.transform.localRotation = output.footRotation;
	}
}

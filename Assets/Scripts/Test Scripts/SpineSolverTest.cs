using UnityEngine;
using System.Collections;

public class SpineSolverTest : MonoBehaviour {
	
	public Transform rootControl;
	[Range(0.2f,4.0f)]
	public float pelvisLength = 2.0f;
	[Range(0.2f,4.0f)]
	public float waistLength = 2.0f;
	[Range(0.2f,4.0f)]
	public float torsoLength = 2.0f;
	[Range(-1.0f,1.0f)]
	public float C_CurveX = 0.0f;
	[Range(-1.0f,1.0f)]
	public float C_CurveY = 0.0f;
	[Range(-1.0f,1.0f)]
	public float S_CurveX = 0.0f;
	[Range(-1.0f,1.0f)]
	public float S_CurveY = 0.0f;
	[Range(0.0f,180.0f)]
	public float X_Angle_Max = 90.0f;
	[Range(0.0f,180.0f)]
	public float Y_Angle_Max = 90.0f;
	
	
	
	GameObject root, pelvis, waist, torso;
	GameObject pelvisPrimitive, waistPrimitive, torsoPrimitive;
	
	GameObject neckMarker;
	
	JointSolver solver;
	
	
	// Use this for initialization
	void Start () {
	
		// Create all GameObjects and primitives required for test:
		root = new GameObject("root");
		pelvis = new GameObject("pelvis");
		waist = new GameObject("waist");
		torso = new GameObject("torso");
		pelvisPrimitive = GameObject.CreatePrimitive(PrimitiveType.Cube);
		waistPrimitive = GameObject.CreatePrimitive(PrimitiveType.Cube);
		torsoPrimitive = GameObject.CreatePrimitive(PrimitiveType.Cube);
		
		neckMarker = GameObject.CreatePrimitive(PrimitiveType.Sphere);
		neckMarker.transform.parent = root.transform;
		// Set up object heirarchy
		root.transform.parent = transform;
		pelvis.transform.parent = root.transform;
		waist.transform.parent = pelvis.transform;
		torso.transform.parent = waist.transform;
		pelvisPrimitive.transform.parent = pelvis.transform;
		waistPrimitive.transform.parent = waist.transform;
		torsoPrimitive.transform.parent = torso.transform;
		
		solver = gameObject.AddComponent<JointSolver>();
	}
	
	// Update is called once per frame
	void Update () {
	
		// This code just adjusts the primitives and has no effect on the joint solver
		pelvisPrimitive.transform.localPosition = new Vector3(0.0f,pelvisLength/2,0.0f);
		waistPrimitive.transform.localPosition = new Vector3(0.0f,waistLength/2,0.0f);
		torsoPrimitive.transform.localPosition = new Vector3(0.0f,torsoLength/2,0.0f);
		Vector3 pelvisScale = new Vector3(0.5f,pelvisLength,0.5f);
		Vector3 waistScale = new Vector3(0.5f,waistLength,0.5f);
		Vector3 torsoScale = new Vector3(0.5f,torsoLength,0.5f);
		pelvisPrimitive.transform.localScale = pelvisScale;
		waistPrimitive.transform.localScale = waistScale;
		torsoPrimitive.transform.localScale = torsoScale;
		
		// The code below demonstrates how to determine all joint rotations and positions from
		// the given control points and values
		root.transform.localPosition = rootControl.localPosition;
		root.transform.localRotation = rootControl.localRotation;
		pelvis.transform.localPosition = Vector3.zero;
		waist.transform.localPosition = new Vector3(0.0f,pelvisLength,0.0f);
		torso.transform.localPosition = new Vector3(0.0f,waistLength,0.0f);
		
		JointSolver.SpineSolverOutput output = solver.SpineRotationSolver(C_CurveX,C_CurveY,S_CurveX,S_CurveY,X_Angle_Max,Y_Angle_Max,pelvisLength,waistLength,torsoLength);
		pelvis.transform.localRotation = output.pelvisRotation;
		waist.transform.localRotation = output.waistRotation;
		torso.transform.localRotation = output.torsoRotation;
		
		neckMarker.transform.localPosition = new Vector3(0.0f,output.neckDistance,0.0f);
		
	}
}

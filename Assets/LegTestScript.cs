using UnityEngine;
using System.Collections;

public class LegTestScript : MonoBehaviour {
	
	// Inspector variables
	public float thighLength = 4.0f;
	public float calfLength = 3.0f;
	public float footLength = 2.0f;
	public float toeLength = 1.0f;
	public float kneeAngle = 60.0f;
	public float elevationAngle = 60.0f;
	
	
	// Private variables
	GameObject thighJoint;
	GameObject calfJoint;
	GameObject footJoint;
	GameObject toeJoint;
	
	GameObject legPivot;
	
	float legHeight = 1.0f;
	
	float t = 0;
	
	GameObject testSphere;
	GameObject testSphere2;
	/*
	// Use this for initialization
	void Start () {
		
		testSphere = GameObject.CreatePrimitive(PrimitiveType.Sphere);
		testSphere2 = GameObject.CreatePrimitive(PrimitiveType.Sphere);
		
		float L1 = thighLength;
		float L2 = calfLength;
		float L3 = footLength;
		float L4 = toeLength;
		float theta1;
		float theta2 = kneeAngle;
		float theta3 = elevationAngle;
		
		
		Vector3 toeLoc = Vector3.zero;
		Vector3 footLoc = toeLoc + L3*new Vector3(Mathf.Cos(Mathf.Deg2Rad*theta3),Mathf.Sin(Mathf.Deg2Rad*theta3),0.0f);
		Vector3 calfLoc = footLoc + L2*new Vector3(-Mathf.Cos(Mathf.Deg2Rad*theta2),Mathf.Sin(Mathf.Deg2Rad*theta2),0.0f);
		
		float kneeHeight = L3*Mathf.Sin(Mathf.Deg2Rad*theta3)+L2*Mathf.Sin(Mathf.Deg2Rad*theta2);
		//Debug.Log("TEST: " + Mathf.Sqrt(L1*L1-Mathf.Pow(L2*Mathf.Cos(Mathf.Deg2Rad*theta2)-calfLoc.x,2)));
		legHeight = kneeHeight + Mathf.Sqrt(L1*L1-Mathf.Pow(calfLoc.x,2));
		//Debug.Log("legHeight = " + legHeight);
		
		legPivot = new GameObject("LegPivot");
		legPivot.transform.position = new Vector3(0.0f,legHeight,0.0f);
		legPivot.transform.parent = transform;
		
		Vector3 thighLoc = new Vector3(toeLoc.x,legHeight,0.0f);
		//GameObject.CreatePrimitive(PrimitiveType.Sphere).transform.position = thighLoc;
		//GameObject.CreatePrimitive(PrimitiveType.Sphere).transform.position = calfLoc;
		//GameObject.CreatePrimitive(PrimitiveType.Sphere).transform.position = footLoc;
		//GameObject.CreatePrimitive(PrimitiveType.Sphere).transform.position = toeLoc;
		
		
		theta1 = Mathf.Rad2Deg*Mathf.Acos(Mathf.Clamp((legHeight-calfLoc.y)/L1,-1.0f,1.0f));
		
		// Create leg segments
		GameObject thigh = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
		thighJoint = new GameObject("thighJoint");
		thighJoint.transform.parent = legPivot.transform;
		thigh.transform.parent = thighJoint.transform;
		thigh.transform.localPosition = new Vector3(0.0f,L1/2.0f,0.0f);
		thigh.transform.localScale = new Vector3(1.0f,L1/2.0f,1.0f);
		thighJoint.transform.position = thighLoc;
		thighJoint.transform.Rotate(0.0f,0.0f,180.0f-theta1);
		
		GameObject calf = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
		calfJoint = new GameObject("calfJoint");
		calfJoint.transform.parent = transform;
		calf.transform.parent = calfJoint.transform;
		calf.transform.localPosition = new Vector3(0.0f,L2/2.0f,0.0f);
		calf.transform.localScale = new Vector3(1.0f,L2/2.0f,1.0f);
		calfJoint.transform.position = calfLoc;
		calfJoint.transform.Rotate(0.0f,0.0f,-90.0f-theta2);
		calfJoint.transform.parent = thighJoint.transform;
		
		GameObject foot = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
		footJoint = new GameObject("footJoint");
		footJoint.transform.parent = transform;
		foot.transform.parent = footJoint.transform;
		foot.transform.localPosition = new Vector3(0.0f,L3/2.0f,0.0f);
		foot.transform.localScale = new Vector3(1.0f,L3/2.0f,1.0f);
		footJoint.transform.position = footLoc;
		footJoint.transform.Rotate(0.0f,0.0f,90.0f+theta3);
		footJoint.transform.parent = calfJoint.transform;
		
		GameObject toe = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
		toeJoint = new GameObject("toeJoint");
		toeJoint.transform.parent = transform;
		toe.transform.parent = toeJoint.transform;
		toe.transform.localPosition = new Vector3(0.0f,L4/2.0f,0.0f);
		toe.transform.localScale = new Vector3(1.0f,L4/2.0f,1.0f);
		toeJoint.transform.position = toeLoc;
		toeJoint.transform.Rotate(0.0f,0.0f,90.0f);
		toeJoint.transform.parent = footJoint.transform;
	}
	
	// Update is called once per frame
	void Update () {
		// Rotate joint to point (2,2,0)
		t += Time.deltaTime;
		float amp = legHeight/2.0f - 0.001f;
		float freq = 0.2f;
		float xOffset = amp*Mathf.Cos(2*Mathf.PI*freq*t);
		float yOffset = amp*Mathf.Sin(2*Mathf.PI*freq*t)+amp-1.0f;
		
		// Also update ankle rotation here...
		float footRotation = elevationAngle;
		float theta2 = kneeAngle;
		
		// Target point is the point the ball of the foot should move to.
		Vector3 targetPoint = new Vector3(0.0f,yOffset,0.0f);
		// Foot point is the point to move the ankle joint to, based on the ankle rotation
		Vector3 footPoint = targetPoint + footJoint.transform.localPosition;
		
		testSphere.transform.position = targetPoint;
		testSphere2.transform.position = footPoint;
		
		Vector3 targetDirection = targetPoint - legPivot.transform.position;
		Debug.DrawRay(legPivot.transform.position,targetDirection);
		float targetDist = targetDirection.magnitude;
		float targetAngle = Mathf.Rad2Deg*Mathf.Atan2(targetDirection.y,targetDirection.x);
		Debug.Log("targetDist = " + targetDist + ", targetAngle = " + targetAngle + ", direction vector = " + targetDirection);
		legPivot.transform.Rotate(new Vector3(0.0f,0.0f,(targetAngle)-legPivot.transform.rotation.eulerAngles.z+90.0f));
		
		
		Vector3 toeLoc = new Vector3(0.0f,legHeight-targetDist,0.0f);//Vector3.zero;
		Vector3 footLoc = toeLoc + footLength*new Vector3(Mathf.Cos(Mathf.Deg2Rad*footRotation),Mathf.Sin(Mathf.Deg2Rad*footRotation),0.0f);
		Vector3 calfLoc = footLoc + calfLength*new Vector3(-Mathf.Cos(Mathf.Deg2Rad*theta2),Mathf.Sin(Mathf.Deg2Rad*theta2),0.0f);
		
		theta1 = Mathf.Rad2Deg*Mathf.Acos(Mathf.Clamp((legHeight-calfLoc.y)/thighLength,-1.0f,1.0f));
		
		//thighJoint.transform.position = thighLoc;
		thighJoint.transform.Rotate(0.0f,0.0f,180.0f-theta1 - thighJoint.transform.localRotation.eulerAngles.z);
		
		calfJoint.transform.position = calfLoc;
		calfJoint.transform.Rotate(0.0f,0.0f,-90.0f-theta2 - calfJoint.transform.localRotation.eulerAngles.z);
		
		footJoint.transform.position = footLoc;
		footJoint.transform.Rotate(0.0f,0.0f,90.0f+theta3 - footJoint.transform.localRotation.eulerAngles.z);
		
		toeJoint.transform.position = toeLoc;
		toeJoint.transform.Rotate(0.0f,0.0f,90.0f - toeJoint.transform.localRotation.eulerAngles.z);
		
		
		// Squash leg
		
		// Clamp maximum length
		float legMaxLength = thighLength+calfLength;
		targetDist = (targetDist > legMaxLength) ? legMaxLength : targetDist;
		
		float squashFactor = targetDist/legHeight;
		Debug.Log("squashFactor = " + squashFactor);
		float thighAngle = squashFactor*90.0f+90.0f;
		thighJoint.transform.Rotate(new Vector3(0.0f,0.0f,thighAngle-thighJoint.transform.localRotation.eulerAngles.z));
		
		float calfAngle = 180.0f-squashFactor*180.0f;
		calfJoint.transform.Rotate(new Vector3(0.0f,0.0f,calfAngle-calfJoint.transform.localRotation.eulerAngles.z));
		
		float footAngle = 0.0f;
		//squashFactor = 1.0f;
		if(targetDist > legHeight) {
			footAngle = -(elevationAngle/(legHeight-legMaxLength))*(targetDist-legMaxLength);
		} else {
			footAngle = -(((elevationAngle-150.0f)/legHeight)*targetDist+150.0f);
		}
		//float footAngle = -(elevationAngle/(legHeight-legMaxLength))*(legMaxLength*squashFactor-legMaxLength);
		// Clamp foot angle to maximum
		footAngle = footAngle < -150.0f ? -150.0f : footAngle;
		footJoint.transform.Rotate(new Vector3(0.0f,0.0f,footAngle-footJoint.transform.localRotation.eulerAngles.z));
		
	}
	*/
}

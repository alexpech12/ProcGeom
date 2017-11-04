using UnityEngine;
using System.Collections;

public class LegTestScript2 : MonoBehaviour {
	
	// Inspector variables
	public float thighLength = 4.0f;
	public float calfLength = 3.0f;
	public float footLength = 2.0f;
	public float toeLength = 1.0f;
	public float standKneeAngle = 60.0f;
	public float elevationAngle = 60.0f;
	public float standStraightness = 0.8f;
	
	public float walkSpeed = 2.0f;
	public float stepLength = 3.0f;
	public float animationTOffset = 0.5f;
	
	
	// Private variables
	GameObject thighJoint;
	GameObject calfJoint;
	GameObject footJoint;
	GameObject toeJoint;
	
	GameObject legPivot;
	
	float standHeight = 1.0f;
	
	float t = 0;
	
	GameObject testSphere;
	GameObject testSphere2;
	
	Animation legPivotAnim;
	Animation thighJointAnim;
	Animation calfJointAnim;
	Animation footJointAnim;
	
	
	// Use this for initialization
	void Start () {
		
		testSphere = GameObject.CreatePrimitive(PrimitiveType.Sphere);
		testSphere2 = GameObject.CreatePrimitive(PrimitiveType.Sphere);
		
		standHeight = standStraightness * (thighLength+calfLength+footLength); // Set default stand height to 80% of maximum stand height
		
		legPivot = new GameObject("LegPivot");
		legPivot.transform.position = new Vector3(0.0f,standHeight,0.0f);
		legPivot.transform.parent = transform;
		
		// Create leg segments
		GameObject thigh = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
		thighJoint = new GameObject("thighJoint");
		thigh.transform.localPosition = new Vector3(0.0f,thighLength/2.0f,0.0f);
		thigh.transform.localScale = new Vector3(1.0f,thighLength/2.0f,1.0f);
		thighJoint.transform.parent = legPivot.transform;
		thigh.transform.parent = thighJoint.transform;
		
		GameObject calf = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
		calfJoint = new GameObject("calfJoint");
		calf.transform.localPosition = new Vector3(0.0f,calfLength/2.0f,0.0f);
		calf.transform.localScale = new Vector3(1.0f,calfLength/2.0f,1.0f);
		calfJoint.transform.parent = thighJoint.transform;
		calf.transform.parent = calfJoint.transform;
		
		GameObject foot = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
		footJoint = new GameObject("footJoint");
		foot.transform.localPosition = new Vector3(0.0f,footLength/2.0f,0.0f);
		foot.transform.localScale = new Vector3(1.0f,footLength/2.0f,1.0f);
		footJoint.transform.parent = calfJoint.transform;
		foot.transform.parent = footJoint.transform;
		
		GameObject toe = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
		toeJoint = new GameObject("toeJoint");
		toe.transform.localPosition = new Vector3(0.0f,toeLength/2.0f,0.0f);
		toe.transform.localScale = new Vector3(1.0f,toeLength/2.0f,1.0f);
		toeJoint.transform.parent = footJoint.transform;
		toe.transform.parent = toeJoint.transform;
		
		// Set up initial positions
		thighJoint.transform.localPosition = new Vector3(0.0f,0.0f,0.0f);
		thighJoint.transform.Rotate(0.0f,0.0f,180.0f);
		calfJoint.transform.Translate(0.0f,thighLength,0.0f);
		footJoint.transform.Translate(0.0f,calfLength,0.0f);
		toeJoint.transform.Translate(0.0f,footLength,0.0f);
		toeJoint.transform.Rotate(0.0f,0.0f,-90.0f);
		
		// Set up test animation clip
		
		legPivotAnim = legPivot.AddComponent<Animation>();
		thighJointAnim = thighJoint.AddComponent<Animation>();
		calfJointAnim = calfJoint.AddComponent<Animation>();
		footJointAnim = footJoint.AddComponent<Animation>();
		
		//legPivotAnim.AddClip(RotationAnim(new Vector3(0.0f,0.0f,30.0f),2.0f),"test_pivot");
		AnimationClip[] clips = WalkAnim();
		legPivotAnim.AddClip(clips[0],"test_clip");
		thighJointAnim.AddClip(clips[1],"test_clip");
		calfJointAnim.AddClip(clips[2],"test_clip");
		footJointAnim.AddClip(clips[3],"test_clip");
		//legPivotAnim.AddClip(WalkAnim());
		legPivotAnim.Play("test_clip");
		thighJointAnim.Play("test_clip");
		calfJointAnim.Play("test_clip");
		footJointAnim.Play("test_clip");
		
		legPivotAnim["test_clip"].time = animationTOffset*legPivotAnim["test_clip"].length;
		thighJointAnim["test_clip"].time = animationTOffset*thighJointAnim["test_clip"].length;
		calfJointAnim["test_clip"].time = animationTOffset*calfJointAnim["test_clip"].length;
		footJointAnim["test_clip"].time = animationTOffset*footJointAnim["test_clip"].length;
		
		
		/*
		Animation thighJointAnim = thighJoint.AddComponent<Animation>();
		Animation calfJointAnim = calfJoint.AddComponent<Animation>();
		Animation footJointAnim = footJoint.AddComponent<Animation>();
		Animation toeJointAnim = toeJoint.AddComponent<Animation>();
		
		Quaternion legPivotZero = Quaternion.identity;
		Quaternion legPivotRotation = Quaternion.Euler(0.0f,0.0f,30.0f);
		
		AnimationCurve legPivotCurveW = AnimationCurve.Linear(0.0f,legPivotZero.w,5.0f,legPivotRotation.w);
		AnimationCurve legPivotCurveX = AnimationCurve.Linear(0.0f,legPivotZero.x,5.0f,legPivotRotation.x);
		AnimationCurve legPivotCurveY = AnimationCurve.Linear(0.0f,legPivotZero.y,5.0f,legPivotRotation.y);
		AnimationCurve legPivotCurveZ = AnimationCurve.Linear(0.0f,legPivotZero.z,5.0f,legPivotRotation.z);
		
		AnimationCurve thighJointCurve = AnimationCurve.Linear(0.0f,0.0f,5.0f,30.0f);
		AnimationCurve calfJointCurve = AnimationCurve.Linear(0.0f,0.0f,5.0f,30.0f);
		AnimationCurve footJointCurve = AnimationCurve.Linear(0.0f,0.0f,5.0f,30.0f);
		AnimationCurve toeJointCurve = AnimationCurve.Linear(0.0f,0.0f,5.0f,30.0f);
		
		AnimationClip legPivotClip = new AnimationClip();
		legPivotClip.SetCurve("",typeof(Transform),"localRotation.w",legPivotCurveW);
		legPivotClip.SetCurve("",typeof(Transform),"localRotation.x",legPivotCurveX);
		legPivotClip.SetCurve("",typeof(Transform),"localRotation.y",legPivotCurveY);
		legPivotClip.SetCurve("",typeof(Transform),"localRotation.z",legPivotCurveZ);
		legPivotAnim.AddClip(legPivotClip,"test_pivot");
		calfJointAnim.AddClip(legPivotClip,"test_pivot");
		legPivotAnim.Play("test_pivot");
		calfJointAnim.Play("test_pivot");
		*/
	}
	
	void Update() {
		legPivotAnim.Play("test_clip");
		thighJointAnim.Play("test_clip");
		calfJointAnim.Play("test_clip");
		footJointAnim.Play("test_clip");
	}
	
	AnimationClip RotationAnim(Vector3 eulerRotation, float time) {
		Quaternion rotStart = Quaternion.identity;
		Quaternion rotation = Quaternion.Euler(eulerRotation);
		
		AnimationCurve newCurveW = AnimationCurve.Linear(0.0f,rotStart.w,time,rotation.w);
		AnimationCurve newCurveX = AnimationCurve.Linear(0.0f,rotStart.x,time,rotation.x);
		AnimationCurve newCurveY = AnimationCurve.Linear(0.0f,rotStart.y,time,rotation.y);
		AnimationCurve newCurveZ = AnimationCurve.Linear(0.0f,rotStart.z,time,rotation.z);
		
		AnimationClip newClip = new AnimationClip();
		newClip.SetCurve("",typeof(Transform),"localRotation.w",newCurveW);
		newClip.SetCurve("",typeof(Transform),"localRotation.x",newCurveX);
		newClip.SetCurve("",typeof(Transform),"localRotation.y",newCurveY);
		newClip.SetCurve("",typeof(Transform),"localRotation.z",newCurveZ);
		return newClip;
	}
	
	AnimationClip[] WalkAnim() {
		AnimationClip[] clips = new AnimationClip[4];
		clips[0] = new AnimationClip();
		clips[1] = new AnimationClip();
		clips[2] = new AnimationClip();
		clips[3] = new AnimationClip();
		AnimationCurve pivotCurveW = new AnimationCurve();
		AnimationCurve pivotCurveX = new AnimationCurve();
		AnimationCurve pivotCurveY = new AnimationCurve();
		AnimationCurve pivotCurveZ = new AnimationCurve();
		AnimationCurve pivotCurveOffX = new AnimationCurve();
		AnimationCurve pivotCurveOffY = new AnimationCurve();
		AnimationCurve thighCurveW = new AnimationCurve();
		AnimationCurve thighCurveX = new AnimationCurve();
		AnimationCurve thighCurveY = new AnimationCurve();
		AnimationCurve thighCurveZ = new AnimationCurve();
		AnimationCurve calfCurveW = new AnimationCurve();
		AnimationCurve calfCurveX = new AnimationCurve();
		AnimationCurve calfCurveY = new AnimationCurve();
		AnimationCurve calfCurveZ = new AnimationCurve();
		AnimationCurve footCurveW = new AnimationCurve();
		AnimationCurve footCurveX = new AnimationCurve();
		AnimationCurve footCurveY = new AnimationCurve();
		AnimationCurve footCurveZ = new AnimationCurve();
		float animTime = 2.0f;//stepLength/walkSpeed;
		//float stepDistance = 3.0f;
		float stepHeight = 1.0f;
		Vector4 angles;
		for (float t = 0.0f; t < animTime; t += Time.deltaTime) {
			// Calculate position offset based on t
			float xOffset = 0.0f;
			float yOffset = 0.0f;
			
			if(t<animTime/2.0f) {
				yOffset = 0.0f;
				xOffset = stepLength*((4.0f*t)/animTime-1);
			} else {
				yOffset = stepHeight*Mathf.Sin((2.0f*Mathf.PI)/(animTime)*t-Mathf.PI);
				xOffset = stepLength*Mathf.Cos((2.0f*Mathf.PI)/(animTime)*t-Mathf.PI);
			}
			
			float hipOffsetX = -0.8f*Mathf.Sin((2*Mathf.PI)/animTime*(t));
			float hipOffsetY = 0.8f*Mathf.Cos((2*Mathf.PI)/animTime*(t+0.5f));
			
			angles = JointRotationSolver(legPivot.transform.position+new Vector3(hipOffsetX,hipOffsetY,0.0f),new Vector2(xOffset,yOffset),thighLength,calfLength,footLength,elevationAngle);
			pivotCurveW.AddKey(t,Quaternion.Euler(0.0f,0.0f,angles.w).w);
			pivotCurveX.AddKey(t,Quaternion.Euler(0.0f,0.0f,angles.w).x);
			pivotCurveY.AddKey(t,Quaternion.Euler(0.0f,0.0f,angles.w).y);
			pivotCurveZ.AddKey(t,Quaternion.Euler(0.0f,0.0f,angles.w).z);
			pivotCurveOffX.AddKey(t,hipOffsetX);
			pivotCurveOffY.AddKey(t,hipOffsetY+standHeight);
			thighCurveW.AddKey(t,Quaternion.Euler(0.0f,0.0f,angles.x).w);
			thighCurveX.AddKey(t,Quaternion.Euler(0.0f,0.0f,angles.x).x);
			thighCurveY.AddKey(t,Quaternion.Euler(0.0f,0.0f,angles.x).y);
			thighCurveZ.AddKey(t,Quaternion.Euler(0.0f,0.0f,angles.x).z);
			calfCurveW.AddKey(t,Quaternion.Euler(0.0f,0.0f,angles.y).w);
			calfCurveX.AddKey(t,Quaternion.Euler(0.0f,0.0f,angles.y).x);
			calfCurveY.AddKey(t,Quaternion.Euler(0.0f,0.0f,angles.y).y);
			calfCurveZ.AddKey(t,Quaternion.Euler(0.0f,0.0f,angles.y).z);
			footCurveW.AddKey(t,Quaternion.Euler(0.0f,0.0f,angles.z).w);
			footCurveX.AddKey(t,Quaternion.Euler(0.0f,0.0f,angles.z).x);
			footCurveY.AddKey(t,Quaternion.Euler(0.0f,0.0f,angles.z).y);
			footCurveZ.AddKey(t,Quaternion.Euler(0.0f,0.0f,angles.z).z);
		}
		clips[0].SetCurve("",typeof(Transform),"localRotation.w",pivotCurveW);
		clips[0].SetCurve("",typeof(Transform),"localRotation.x",pivotCurveX);
		clips[0].SetCurve("",typeof(Transform),"localRotation.y",pivotCurveY);
		clips[0].SetCurve("",typeof(Transform),"localRotation.z",pivotCurveZ);
		clips[0].SetCurve("",typeof(Transform),"localPosition.x",pivotCurveOffX);
		clips[0].SetCurve("",typeof(Transform),"localPosition.y",pivotCurveOffY);
		clips[1].SetCurve("",typeof(Transform),"localRotation.w",thighCurveW);
		clips[1].SetCurve("",typeof(Transform),"localRotation.x",thighCurveX);
		clips[1].SetCurve("",typeof(Transform),"localRotation.y",thighCurveY);
		clips[1].SetCurve("",typeof(Transform),"localRotation.z",thighCurveZ);
		clips[2].SetCurve("",typeof(Transform),"localRotation.w",calfCurveW);
		clips[2].SetCurve("",typeof(Transform),"localRotation.x",calfCurveX);
		clips[2].SetCurve("",typeof(Transform),"localRotation.y",calfCurveY);
		clips[2].SetCurve("",typeof(Transform),"localRotation.z",calfCurveZ);
		clips[3].SetCurve("",typeof(Transform),"localRotation.w",footCurveW);
		clips[3].SetCurve("",typeof(Transform),"localRotation.x",footCurveX);
		clips[3].SetCurve("",typeof(Transform),"localRotation.y",footCurveY);
		clips[3].SetCurve("",typeof(Transform),"localRotation.z",footCurveZ);
		
		return clips;
	}
	
	// Update is called once per frame
	void NoUpdate () {
		
		// Calculate new foot position and rotation
		
		float amp = legPivot.transform.position.y*0.8f;
		float stepLength = 2.0f;
		float stepHeight = 0.5f;
		float freq = 0.5f;
		float stepTime = 1.0f/freq;
		t += Time.deltaTime;
		
		//thetaE = elevationAngle + t*freq*360.0f;
		float xOffset = -stepLength*Mathf.Cos(2*Mathf.PI*freq*t);
		float yOffset = 0.0f;
		if(t>stepTime/2.0f) {
			yOffset = -stepHeight*Mathf.Sin(2*Mathf.PI*freq*t);
		}
		if(t>stepTime) {t = 0.0f;}
		float elevationOffset = 60.0f;
		
		yOffset = 5.0f*Mathf.Sin(2*Mathf.PI*freq*t);
		xOffset = 5.0f*Mathf.Cos(2*Mathf.PI*freq*t);
		
		Vector4 newAngles = JointRotationSolver(legPivot.transform.position,new Vector2(xOffset,yOffset),thighLength,calfLength,footLength,elevationOffset);
		legPivot.transform.localRotation = Quaternion.Euler(0.0f,0.0f,newAngles.w);
		thighJoint.transform.localRotation = Quaternion.Euler(0.0f,0.0f,newAngles.x);
		calfJoint.transform.localRotation = Quaternion.Euler(0.0f,0.0f,newAngles.y);
		footJoint.transform.rotation = Quaternion.Euler(0.0f,0.0f,newAngles.z);
		
		/*
		// Calculate new joint angles
		
		float L1 = thighLength;
		float L2 = calfLength;
		float L3 = footLength;
		float thetaE = elevationOffset;
		
		// Everything relative to hip joint
		Vector3 relativePoint = legPivot.transform.position;
		
		Vector3 Px = new Vector3(xOffset,yOffset,0.0f) - relativePoint; // Target point
		Vector3 Pf = Px + L3*new Vector3(Mathf.Cos(Mathf.Deg2Rad*thetaE),Mathf.Sin(Mathf.Deg2Rad*thetaE),0.0f);
		
		testSphere.transform.position = Px+relativePoint;
		testSphere2.transform.position = Pf+relativePoint;
		
		// Clamp Px to maximum length
		if(Pf.magnitude >= L1+L2) {
			Pf = Pf.normalized*(L1+L2-0.001f); // Subtract 0.001 to give small amount of rounding tolerance
			Px = Pf - L3*new Vector3(Mathf.Cos(Mathf.Deg2Rad*thetaE),Mathf.Sin(Mathf.Deg2Rad*thetaE),0.0f);
		}
		
		float Df = Pf.magnitude;
		//Df = Mathf.Clamp(Df,0.1f,L1+L2-0.01f);
		
		float psi2 = CosineAngle(Df,L1,L2);
		float theta2 = 180.0f - psi2;
		float Dt = (Pf-Px).magnitude;
		float phi1 = CosineAngle(Dt,Df,Px.magnitude);
		float psi1 = CosineAngle(L2,L1,Df);
		Debug.Log(phi1 + ", " + psi1);
		float theta1 = 180.0f - (psi1-phi1);
		
		Debug.Log("Px = " + Px + ", Pf = " + Pf + "Df = " + Df + ", Dt = " + Dt + ", theta1 = " + theta1 + ", theta2 = " + theta2 + ", thetaE = " + thetaE);
		
		// Finally, rotate entire leg to face point
		float legAngle = Mathf.Rad2Deg*Mathf.Atan2(Px.y,Px.x);
		
		thighJoint.transform.localRotation = Quaternion.Euler(0.0f,0.0f,theta1);
		calfJoint.transform.localRotation = Quaternion.Euler(0.0f,0.0f,theta2);
		//thighJoint.transform.Rotate(0.0f,0.0f,theta1-thighJoint.transform.localRotation.eulerAngles.z);
		//calfJoint.transform.Rotate(0.0f,0.0f,theta2-calfJoint.transform.localRotation.eulerAngles.z);
		// Use absolute rotation for foot
		Quaternion test = Quaternion.Euler(0.0f,0.0f,90.0f+thetaE);
		footJoint.transform.rotation = Quaternion.Euler(0.0f,0.0f,thetaE+90.0f);
		//footJoint.transform.Rotate(0.0f,0.0f,thetaE-footJoint.transform.rotation.eulerAngles.z);
		
		//legPivot.transform.Rotate(new Vector3(0.0f,0.0f,(legAngle)-legPivot.transform.rotation.eulerAngles.z+90.0f));
		legPivot.transform.localRotation = Quaternion.Euler(0.0f,0.0f,legAngle+90.0f);
		*/
		
	}
	
	// Takes a Vector2 x,y position offset, joint lengths and heel elevation and returns a Vector4 of angles as (legPivot, thigh, calf, foot)
	Vector4 JointRotationSolver(Vector3 hipPoint, Vector2 positionOffset, float thighLength, float calfLength, float footLength, float heelElevationAngle) {
		
		// Calculate new joint angles
		
		float L1 = thighLength;
		float L2 = calfLength;
		float L3 = footLength;
		float thetaE = heelElevationAngle;
		
		// Everything relative to hip joint
		Vector3 relativePoint = hipPoint;
		
		Vector3 Px = new Vector3(positionOffset.x,positionOffset.y,0.0f) - relativePoint; // Target point
		Vector3 Pf = Px + L3*new Vector3(Mathf.Cos(Mathf.Deg2Rad*thetaE),Mathf.Sin(Mathf.Deg2Rad*thetaE),0.0f);
		
		// Clamp Px to maximum length
		if(Pf.magnitude >= L1+L2) {
			Pf = Pf.normalized*(L1+L2-0.001f); // Subtract 0.001 to give small amount of rounding tolerance
			Px = Pf - L3*new Vector3(Mathf.Cos(Mathf.Deg2Rad*thetaE),Mathf.Sin(Mathf.Deg2Rad*thetaE),0.0f);
		}
		
		float Df = Pf.magnitude;
		
		float psi2 = CosineAngle(Df,L1,L2);
		float theta2 = 180.0f - psi2;
		float Dt = (Pf-Px).magnitude;
		float phi1 = CosineAngle(Dt,Df,Px.magnitude);
		float psi1 = CosineAngle(L2,L1,Df);
		float theta1 = 180.0f - (psi1-phi1);
		
		// Finally, rotate entire leg to face point
		float legAngle = Mathf.Rad2Deg*Mathf.Atan2(Px.y,Px.x);
		
		Vector4 angles = Vector4.zero;
		angles.w = legAngle+90.0f;
		angles.x = theta1;
		angles.y = theta2;
		angles.z = thetaE-theta1-theta2-legAngle;
		return angles;
	}
	
	// Returns an angle in degrees opposite side a
	float CosineAngle(float a, float b, float c) {
		if(a > b+c || b > a+c || c > a+b) {
			if(a > b+c) {a = b+c-Mathf.Epsilon;}
			if(b > a+c) {b = a+c-Mathf.Epsilon;}
			if(c > a+b) {c = a+b-Mathf.Epsilon;}
			//Debug.LogError("ERROR: CosineAngle will return invalid value!");
		}
		return Mathf.Rad2Deg*Mathf.Acos((b*b+c*c-a*a)/(2.0f*b*c));
	}
}

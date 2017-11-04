using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PGAnimationController : MonoBehaviour {
	
	public Transform legFR;
	public Transform legFL;
	public Transform legBR;
	public Transform legBL;
	
	float animationTOffset = 0.0f;
	
	float t = 0.0f;
	
	CreatureSpecies species;
	
	float thighLength = 1.0f;
	float calfLength = 1.0f;
	float footLength = 1.0f;
	float elevationAngle = 30.0f;
	float standHeight = 2.0f;
	
	//public List<Vector3> Vertices { get { return m_Vertices; } }
	//private List<Vector3> m_Vertices = new List<Vector3>();
	
	List<Animation> animObjectList = new List<Animation>();
	
	struct Leg
	{
		public Transform[] legPart;
	}
	
	// Use this for initialization
	public void Init (ref CreatureSpecies species, Transform[] legBase) {
		
		// Get reference to creature species
		this.species = species;
		
		// Attach animation components to all objects
		//int animNum = 28;
		//Animation[] animComps = new Animation[animNum];
		
		
		GameObject bodyBase = gameObject.transform.FindChild("body").gameObject;
		GameObject root = bodyBase.transform.FindChild("root").gameObject;
		AddToAnimList(ref root);
		GameObject rootPivot = root.transform.FindChild("rootPivot").gameObject;
		AddToAnimList(ref rootPivot);
		GameObject pelvisPivot = rootPivot.transform.FindChild("pelvis").gameObject;
		AddToAnimList(ref pelvisPivot);
		GameObject waistPivot = pelvisPivot.transform.FindChild("waist").gameObject;
		AddToAnimList(ref waistPivot);
		GameObject torsoPivot = waistPivot.transform.FindChild("torso").gameObject;
		AddToAnimList(ref torsoPivot);;
		//GameObject neckPivot = torsoPivot.transform.FindChild("neckPivot").gameObject;
		//neckPivot.AddComponent<Animation>();
		
		GameObject BL_hipPivot = root.transform.FindChild("BL_hipPivot").gameObject;
		//AddToAnimList(ref BL_hipPivot);
		GameObject BR_hipPivot = root.transform.FindChild("BR_hipPivot").gameObject;
		//AddToAnimList(ref BR_hipPivot);
		GameObject FL_hipPivot = torsoPivot.transform.FindChild("FL_hipPivot").gameObject;
		//AddToAnimList(ref FL_hipPivot);
		GameObject FR_hipPivot = torsoPivot.transform.FindChild("FR_hipPivot").gameObject;
		//AddToAnimList(ref FR_hipPivot);
		GameObject BL_hip = bodyBase.transform.FindChild("BL_hip").gameObject;
		GameObject BR_hip = bodyBase.transform.FindChild("BR_hip").gameObject;
		GameObject FL_hip = bodyBase.transform.FindChild("FL_hip").gameObject;
		GameObject FR_hip = bodyBase.transform.FindChild("FR_hip").gameObject;
		GameObject[] hips = {BL_hip,BR_hip,FL_hip,FR_hip};
		for(int i = 0; i < 4 /* Number of legs */ ; i++) {
			string prefix = i==0?"BL_":i==1?"BR_":i==2?"FL_":i==3?"FR_":"ERROR_";
			GameObject child = hips[i];
			AddToAnimList(ref child,prefix+"Hip");
			child = child.transform.FindChild("thigh").gameObject;
			AddToAnimList(ref child,prefix+"Thigh");
			child = child.transform.FindChild("calf").gameObject;
			AddToAnimList(ref child,prefix+"Knee");
			child = child.transform.FindChild("foot").gameObject;
			AddToAnimList(ref child,prefix+"Ankle");
			child = child.transform.FindChild("toe").gameObject;
			AddToAnimList(ref child,prefix+"Toe");
		}
		/*
		ClipContainer clips = species.clips;
		for(int i = 0; i < animObjectList.Count; i++) {
			for(int j = 0; j = clips.ActiveGroupNum(); j++) {
				animObjectList[i].AddClip(clips.idleAnim.rootClip,"idle");
			}
		}
		*/
	}
	
	void AddToAnimList(ref GameObject obj) {
		AddToAnimList(ref obj, obj.name);
	}
	
	void AddToAnimList(ref GameObject obj, string partName) {
		Animation anim = obj.AddComponent<Animation>();
		animObjectList.Add(anim);
		Debug.Log("Adding object " + partName + " to list of length " + animObjectList.Count);
		if(species.clips.partDictionary.ContainsKey(partName)) {
			foreach(KeyValuePair<string,AnimationClip> entry in species.clips.GetAnimPartDictionary(partName)) {
				Debug.Log("Adding clip " + entry.Key + " with AnimationClip component " + entry.Value);
				anim.AddClip(entry.Value,entry.Key);
			}
		}
	}
	
	// Update is called once per frame
	void Update () {
		// play animations based on current creature state
		PlayAnimation("idle",WrapMode.Loop);
	}
	
	void PlayAnimation(string name, WrapMode wrapMode) {
		foreach(Animation anim in animObjectList) {
			Debug.Log("playing anim for object " + anim.gameObject.name);
			if(anim.GetClip(name) != null) {
				anim.Play(name);
				anim.wrapMode = wrapMode;
			}
		}
	}
	
	/*
	AnimationClip[] WalkAnim(float animTime, float stepHeight, float stepLength) {
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
		//float animTime = 2.0f;//stepLength/walkSpeed;
		//float stepDistance = 3.0f;
		//float stepHeight = 1.0f;
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
			
			//float hipOffsetX = -0.8f*Mathf.Sin((2*Mathf.PI)/animTime*(t));
			//float hipOffsetY = 0.8f*Mathf.Cos((2*Mathf.PI)/animTime*(t+0.5f));
			float hipOffsetX = 0.0f;
			float hipOffsetY = 0.0f;
			
			angles = JointRotationSolver(legFR.transform.position+new Vector3(hipOffsetX,hipOffsetY,0.0f),new Vector2(xOffset,yOffset),thighLength,calfLength,footLength,elevationAngle);
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
	*/
	/*
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
	*/
}

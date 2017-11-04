using UnityEngine;
using System.Collections;

public class WalkAnimation : AnimationBase {
	
	CreatureSpecies species;
	
	float thighLength;
	float calfLength;
	float footLength;
	float toeLength;
	float elevationAngle;
	float standHeight;
	
	float stepHeight = 1.0f;
	float stepLength = 1.0f;
	
	Transform[] frontLegPivots,backLegPivots,spinePivots;
	LegData frontLegData,backLegData;
	
	const float animTime = 2.0f;
	
	public WalkAnimation(CreatureSpecies species_w, string animName) : base(species_w,animName,animTime) {
		species = species_w;
	}
	
	// This function takes a reference to a creature object with animation components already attached and creates all animation clips for a walk cycle.
	public void CreateWalkAnimation(Transform crBase, Transform[] legBase) {
		// Set up references
		
		// Create body animations
		
		// Create leg animations
		Transform[] frontLegPivots_local = {legBase[0].GetChild(0),legBase[1].GetChild(0)};
		Transform[] backLegPivots_local = {legBase[2].GetChild(0),legBase[3].GetChild(0)};
		frontLegPivots = frontLegPivots_local;
		backLegPivots = backLegPivots_local;
		
		Transform rootPivot = crBase.FindChild("body").FindChild("rootPivot");
		Transform waistPivot = rootPivot.FindChild("waistPivot");
		Transform torsoPivot = waistPivot.FindChild("torsoPivot");
		Transform neckPivot = torsoPivot.FindChild("neckPivot");
		Transform[] spinePivots_local = {rootPivot,waistPivot,torsoPivot,neckPivot};
		spinePivots = spinePivots_local;
		
		//WalkAnim(rootPivot,frontLegs,species.frontLegData,1.0f,1.0f);
		//WalkAnim(rootPivot,backLegs,species.backLegData,1.0f,1.0f);
		
	}
	
	void WalkAnim(Transform rootPivot, Transform[] legPivots, LegData legData,float stepHeight, float stepLength) {
		float thighLength = legData.thighData.length;
		float calfLength = legData.calfData.length;
		float footLength = legData.footData.length;
		float toeLength = legData.toeData.length;
		float elevationAngle = legData.elevationAngle;
		float standHeight = legData.standHeight;
		bool reverseKnee = legData.reverseKnee;
		float animTime = 1.0f;
		
		// Generate rotation values
		int arrayLength = (int)(animTime/Time.deltaTime)+1;
		Quaternion[] rootPivotQuats = new Quaternion[arrayLength];
		rootPivotQuats[0] = rootPivot.localRotation;
		Quaternion[] pivotsQuats = new Quaternion[arrayLength];
		Quaternion[] thighQuats = new Quaternion[arrayLength];
		Quaternion[] calfQuats = new Quaternion[arrayLength];
		Quaternion[] footQuats = new Quaternion[arrayLength];
		float[] times = new float[arrayLength];
		Vector4 angles;
		int counter = 0;
		for (float t = 0.0f; t < animTime; t += Time.deltaTime) {
			// Define transformOffset, target position
			
			Vector3 footPosition = WalkAnimPositionFunction(t,animTime,stepLength,stepHeight);
			if(counter>0) {
				rootPivotQuats[counter] = rootPivotQuats[counter-1]*Quaternion.Euler(0.0f,0.0f,Mathf.Sin(2*Mathf.PI*(t/animTime)));
			}
			
			//float hipOffsetX = -0.8f*Mathf.Sin((2*Mathf.PI)/animTime*(t));
			//float hipOffsetY = 0.8f*Mathf.Cos((2*Mathf.PI)/animTime*(t+0.5f));
			float hipOffsetX = 0.0f;
			float hipOffsetY = 0.0f;
			
			Vector3 transformOffset = new Vector3(0.0f,legPivots[0].position.y,0.0f);
			
			angles = JointRotationSolver(transformOffset+new Vector3(hipOffsetX,hipOffsetY,0.0f),new Vector2(footPosition.x,footPosition.y),thighLength,calfLength,footLength,elevationAngle,reverseKnee);
			pivotsQuats[counter] = Quaternion.Euler(0.0f,0.0f,angles.w);
			thighQuats[counter] = Quaternion.Euler(0.0f,0.0f,angles.x);
			calfQuats[counter] = Quaternion.Euler(0.0f,0.0f,angles.y);
			footQuats[counter] = Quaternion.Euler(0.0f,0.0f,angles.z);
			times[counter] = t;
			counter++;
		}
		/*
		AnimationClip rootPivotClip = CreateLocalRotationClip(rootPivotQuats,times);
		
		AnimationClip hipPivotClip = CreateLocalRotationClip(pivotsQuats,times);
		AnimationClip thighClip = CreateLocalRotationClip(thighQuats,times);
		AnimationClip calfClip = CreateLocalRotationClip(calfQuats,times);
		AnimationClip footClip = CreateLocalRotationClip(footQuats,times);
		
		AnimationClip[] clips = {hipPivotClip,thighClip,calfClip,footClip};
		*/
		// Attach clips
		//rootPivot.animation.AddClip(rootPivotClip,"walk");
		
		// Attach clips to leg
		for(int i = 0; i < 2 /*legPivots[i] != null*/; i++) {
			//Transform child = legPivots[i];
			for(int j = 0; j < 4 /* Number of clips */; j++) {
				//child.GetComponent<Animation>().AddClip(clips[j],"walk");
				//child = child.GetChild(0);
			}
		}
		
	}
	
	// Takes an array of times and returns an array of position vectors
	Vector3 WalkAnimPositionFunction(float t, float t_total, float stepLength, float stepHeight) {
		Vector3 position = Vector3.zero;
		if(t<t_total/2.0f) {
			position.y = 0.0f;
			position.x = stepLength*((4.0f*t)/t_total-1.0f);
		} else {
			position.y = stepHeight*Mathf.Sin((2.0f*Mathf.PI)/(t_total)*t-Mathf.PI);
			position.x = stepLength*Mathf.Cos((2.0f*Mathf.PI)/(t_total)*t-Mathf.PI);
		}
		return position;
	}
	
	void WalkAnimOld(Transform[] legPivots, LegData legData,/* float animTime, */float stepHeight, float stepLength) {
		float thighLength = legData.thighData.length;
		float calfLength = legData.calfData.length;
		float footLength = legData.footData.length;
		float toeLength = legData.toeData.length;
		float elevationAngle = legData.elevationAngle;
		float standHeight = legData.standHeight;
		float animTime = 1.0f;
		
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
		Vector4[] pivotAngles = new Vector4[(int)(animTime/Time.deltaTime)];
		for (float t = 0.0f; t < animTime; t += Time.deltaTime) {
			// Calculate position offset based on t
			float xOffset = 0.0f;
			float yOffset = 0.0f;
			
			if(t</*animTime*/1.0f/2.0f) {
				yOffset = 0.0f;
				xOffset = stepLength*((4.0f*t)/animTime-1.0f);
			} else {
				yOffset = stepHeight*Mathf.Sin((2.0f*Mathf.PI)/(animTime)*t-Mathf.PI);
				xOffset = stepLength*Mathf.Cos((2.0f*Mathf.PI)/(animTime)*t-Mathf.PI);
			}
			
			//float hipOffsetX = -0.8f*Mathf.Sin((2*Mathf.PI)/animTime*(t));
			//float hipOffsetY = 0.8f*Mathf.Cos((2*Mathf.PI)/animTime*(t+0.5f));
			float hipOffsetX = 0.0f;
			float hipOffsetY = 0.0f;
			
			Vector3 transformOffset = new Vector3(0.0f,legPivots[0].position.y,0.0f);
			
			angles = JointRotationSolver(transformOffset+new Vector3(hipOffsetX,hipOffsetY,0.0f),new Vector2(xOffset,yOffset),thighLength,calfLength,footLength,elevationAngle,false);
			
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
		
		// Attach clips to leg
		for(int i = 0; i < 2 /*legPivots[i] != null*/; i++) {
			Transform child = legPivots[i];
			for(int j = 0; j < 4 /* Number of clips */; j++) {
				child.GetComponent<Animation>().AddClip(clips[j],"walk");
				child = child.GetChild(0);
			}
		}
		/*
		legPivotAnim.AddClip(clips[0],"walk");
		thighJointAnim.AddClip(clips[1],"walk");
		calfJointAnim.AddClip(clips[2],"walk");
		footJointAnim.AddClip(clips[3],"walk");
		*/
	}
	
	// Takes a Vector2 x,y position offset, joint lengths and heel elevation and returns a Vector4 of angles as (legPivot, thigh, calf, foot)
	Vector4 JointRotationSolver(Vector3 hipPoint, Vector2 positionOffset, float thighLength, float calfLength, float footLength, float heelElevationAngle, bool reverseKnee) {
		
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
		
		float psi2 = CosineAngle(Df,L1,L2)*(reverseKnee?-1:1);
		float theta2 = 180.0f - psi2;
		float Dt = (Pf-Px).magnitude;
		float phi1 = CosineAngle(Dt,Df,Px.magnitude);
		float psi1 = CosineAngle(L2,L1,Df)*(reverseKnee?-1:1);
		float theta1 = 180.0f - (psi1-phi1);
		
		// Finally, rotate entire leg to face point
		float legAngle = Mathf.Rad2Deg*Mathf.Atan2(Px.y,Px.x);
		
		Vector4 angles = Vector4.zero;
		angles.w = legAngle+90.0f;
		angles.x = theta1;
		angles.y = theta2;
		angles.z = thetaE-theta1-theta2-legAngle;
		return -angles; // This is negative because legs were coming out 180 degrees backwards
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

using UnityEngine;
using System.Collections;

public class AnimationBase {
	
	protected CreatureSpecies species;
	protected ClipContainer.AnimationGroup animGroup;
	
	
	// ANIMATION CONTROLS
	// These are the variables controled by the animation functions to create animation clips
	//  - Spine Controls
	protected Vector3 rootOffset = Vector3.zero;
	protected Quaternion rootRotation = Quaternion.identity;
	protected float C_CurveX = 0.0f;
	protected float C_CurveY = 0.0f;
	protected float S_CurveX = 0.0f;
	protected float S_CurveY = 0.0f;
	// - Neck Controls
	protected Quaternion neck_startRotation = Quaternion.identity;
	protected Quaternion neck_endRotation = Quaternion.identity;
	// - Tail Controls
	protected Quaternion tail_startRotation = Quaternion.identity;
	protected Quaternion tail_endRotation = Quaternion.identity;
	// - Leg Controls
	protected Vector3 FL_footTarget = Vector3.zero;
	protected float FL_ankleElevation = 0.0f;
	protected float FL_toeRotation = 0.0f;
	protected Vector3 FR_footTarget = Vector3.zero;
	protected float FR_ankleElevation = 0.0f;
	protected float FR_toeRotation = 0.0f;
	protected Vector3 BL_footTarget = Vector3.zero;
	protected float BL_ankleElevation = 0.0f;
	protected float BL_toeRotation = 0.0f;
	protected Vector3 BR_footTarget = Vector3.zero;
	protected float BR_ankleElevation = 0.0f;
	protected float BR_toeRotation = 0.0f;
	// - Head Controls
	protected float jawOffset = 0.0f;
	protected float TL_eyelidControl = 0.0f;
	protected float TR_eyelidControl = 0.0f;
	protected float BL_eyelidControl = 0.0f;
	protected float BR_eyelidControl = 0.0f;
	
	
	protected Vector3 root_defOffset;
	protected Quaternion root_defRotation;
	
	// Leg control default offsets
	protected Vector3 FL_defOffset;
	protected Vector3 FR_defOffset;
	protected Vector3 BL_defOffset;
	protected Vector3 BR_defOffset;
	
	
	public AnimationBase(CreatureSpecies species, string animName, float animTime) {
		this.species = species;
		float legSeparation = species.legSeparation;
		float shoulderWidth = species.spineData.torsoData.radiusB;
		float hipWidth = species.spineData.pelvisData.radiusA;
		root_defOffset = new Vector3(-legSeparation/2,species.backLegData.legLength,0.0f);
		root_defRotation = Quaternion.Euler(0.0f,0.0f,species.spineElevation-90.0f);
		FL_defOffset = new Vector3(legSeparation/2,0.0f,shoulderWidth);
		FR_defOffset = new Vector3(legSeparation/2,0.0f,-shoulderWidth);
		BL_defOffset = new Vector3(-legSeparation/2,0.0f,hipWidth);
		BR_defOffset = new Vector3(-legSeparation/2,0.0f,-hipWidth);
		GameObject sphere0 = GameObject.CreatePrimitive(PrimitiveType.Sphere);
		sphere0.transform.position = FL_defOffset;
		animGroup = CreateClips(animTime, animName);
		UpdateDictionaries(animName);
	}
	
	
	
	protected AnimationClip CreatePositionClip(Vector3[] positions, float[] times, string name) {
		AnimationClip newClip = new AnimationClip();
		newClip.name = name;
		return CreatePositionClip(newClip,positions,times);
	}
	protected AnimationClip CreatePositionClip(AnimationClip clip, Vector3[] positions, float[] times) {
		AnimationCurve curveX = new AnimationCurve();
		AnimationCurve curveY = new AnimationCurve();
		AnimationCurve curveZ = new AnimationCurve();
		
		for(int i = 0; i < times.GetLength(0); i++) {
			curveX.AddKey(times[i],positions[i].x);
			curveY.AddKey(times[i],positions[i].y);
			curveZ.AddKey(times[i],positions[i].z);
		}
		
		clip.SetCurve("",typeof(Transform),"localPosition.x",curveX);
		clip.SetCurve("",typeof(Transform),"localPosition.y",curveY);
		clip.SetCurve("",typeof(Transform),"localPosition.z",curveZ);
		return clip;
	}
	
	protected AnimationClip CreateRotationClip(Quaternion[] rotations, float[] times, string name) {
		AnimationClip newClip = new AnimationClip();
		newClip.name = name;
		return CreateRotationClip(newClip,rotations,times);
	}
	protected AnimationClip CreateRotationClip(AnimationClip clip, Quaternion[] rotations, float[] times) {
		
		AnimationCurve curveW = new AnimationCurve();
		AnimationCurve curveX = new AnimationCurve();
		AnimationCurve curveY = new AnimationCurve();
		AnimationCurve curveZ = new AnimationCurve();
		
		for(int i = 0; i < times.GetLength(0); i++) {
			curveW.AddKey(times[i],rotations[i].w);
			curveX.AddKey(times[i],rotations[i].x);
			curveY.AddKey(times[i],rotations[i].y);
			curveZ.AddKey(times[i],rotations[i].z);
		}
		
		clip.SetCurve("",typeof(Transform),"localRotation.w",curveW);
		clip.SetCurve("",typeof(Transform),"localRotation.x",curveX);
		clip.SetCurve("",typeof(Transform),"localRotation.y",curveY);
		clip.SetCurve("",typeof(Transform),"localRotation.z",curveZ);
		return clip;
	}
	
	// Below is an abstract list for all animation functions to override
	protected ClipContainer.AnimationGroup CreateClips(float animTime, string animName) {
		
		int i = 0;
		int arrayLength = (int)(animTime/Time.deltaTime)+1;
		float[] times = new float[arrayLength];
		
		Vector3[] rootPos = new Vector3[arrayLength];
		Quaternion[] rootPivotRot= new Quaternion[arrayLength];
		Quaternion[] pelvisRot= new Quaternion[arrayLength];
		Quaternion[] waistRot= new Quaternion[arrayLength];
		Quaternion[] torsoRot= new Quaternion[arrayLength];
		Vector3[] FL_hipPos = new Vector3[arrayLength];
		Quaternion[] FL_hipRot = new Quaternion[arrayLength];
		Quaternion[] FL_thighRot = new Quaternion[arrayLength];
		Quaternion[] FL_calfRot = new Quaternion[arrayLength];
		Quaternion[] FL_footRot = new Quaternion[arrayLength];
		Quaternion[] FL_toeRot = new Quaternion[arrayLength];
		Vector3[] FR_hipPos = new Vector3[arrayLength];
		Quaternion[] FR_hipRot = new Quaternion[arrayLength];
		Quaternion[] FR_thighRot = new Quaternion[arrayLength];
		Quaternion[] FR_calfRot = new Quaternion[arrayLength];
		Quaternion[] FR_footRot = new Quaternion[arrayLength];
		Quaternion[] FR_toeRot = new Quaternion[arrayLength];
		Vector3[] BL_hipPos = new Vector3[arrayLength];
		Quaternion[] BL_hipRot = new Quaternion[arrayLength];
		Quaternion[] BL_thighRot = new Quaternion[arrayLength];
		Quaternion[] BL_calfRot = new Quaternion[arrayLength];
		Quaternion[] BL_footRot = new Quaternion[arrayLength];
		Quaternion[] BL_toeRot = new Quaternion[arrayLength];
		Vector3[] BR_hipPos = new Vector3[arrayLength];
		Quaternion[] BR_hipRot = new Quaternion[arrayLength];
		Quaternion[] BR_thighRot = new Quaternion[arrayLength];
		Quaternion[] BR_calfRot = new Quaternion[arrayLength];
		Quaternion[] BR_footRot = new Quaternion[arrayLength];
		Quaternion[] BR_toeRot = new Quaternion[arrayLength];
		JointSolver.SpineSolverOutput spine_out = new JointSolver.SpineSolverOutput();
		JointSolver.LegSolverOutput FL_out = new JointSolver.LegSolverOutput();
		JointSolver.LegSolverOutput FR_out = new JointSolver.LegSolverOutput();
		JointSolver.LegSolverOutput BL_out = new JointSolver.LegSolverOutput();
		JointSolver.LegSolverOutput BR_out = new JointSolver.LegSolverOutput();
		
		
		GameObject solverObject = new GameObject("solverObject");
		JointSolver solver = solverObject.AddComponent<JointSolver>();
		
		for(float t = 0.0f; t < animTime; t += Time.deltaTime) {
			// Call all animation functions and store values in appropriate variables
			rootPos[i] = AF_rootPosition(t);
			rootPivotRot[i] = AF_rootPivotRotation(t);
			Vector3 FL_target = AF_FL_LegTarget(t);
			Vector3 FR_target = AF_FR_LegTarget(t);
			Vector3 BL_target = AF_BL_LegTarget(t);
			Vector3 BR_target = AF_BR_LegTarget(t);
			
			float hipWidth = species.spineData.pelvisData.radiusA;
			float shoulderWidth = species.spineData.torsoData.radiusB;
			
			float spine_CX = AF_spine_CX(t);
			float spine_CY = AF_spine_CY(t);
			float spine_SX = AF_spine_SX(t);
			float spine_SY = AF_spine_SY(t);
			
			// Convert all function values to raw rotations and positions for each joint
			spine_out = solver.SpineRotationSolver(spine_CX,spine_CY,spine_SX,spine_SY,90.0f,90.0f,
															species.spineData.pelvisData.length,
															species.spineData.waistData.length,
															species.spineData.torsoData.length);
			
			pelvisRot[i] = spine_out.pelvisRotation;
			waistRot[i] = spine_out.waistRotation;
			torsoRot[i] = spine_out.torsoRotation;
			
			//Vector3 sideHipPoint = rootPos[i] + rootPivotRot[i]*(new Vector3(0.0f,0.0f,hipWidth));
			BL_hipPos[i] = rootPos[i] + rootPivotRot[i]*(new Vector3(0.0f,0.0f,hipWidth));
			BL_out = solver.LegRotationSolver(BL_hipPos[i],BL_target,
														species.backLegData.thighData.length,
														species.backLegData.calfData.length,
														species.backLegData.footData.length,
														30.0f,0.0f,false);
			BL_hipRot[i] = BL_out.legPivotRotation;
			BL_thighRot[i] = BL_out.thighRotation;
			BL_calfRot[i] = BL_out.calfRotation;
			BL_footRot[i] = BL_out.footRotation;
			BL_toeRot[i] = Quaternion.identity;
			
			BR_hipPos[i] = rootPos[i] + rootPivotRot[i]*(new Vector3(0.0f,0.0f,-hipWidth));
			BR_out = solver.LegRotationSolver(BR_hipPos[i],BR_target,
														species.backLegData.thighData.length,
														species.backLegData.calfData.length,
														species.backLegData.footData.length,
														30.0f,0.0f,false);
			BR_hipRot[i] = BR_out.legPivotRotation;
			BR_thighRot[i] = BR_out.thighRotation;
			BR_calfRot[i] = BR_out.calfRotation;
			BR_footRot[i] = BR_out.footRotation;
			BR_toeRot[i] = Quaternion.identity;
			
			
			Vector3 neckPivot = rootPos[i]+rootPivotRot[i]*(new Vector3(0.0f,spine_out.neckDistance,0.0f));
			
			FL_hipPos[i] = neckPivot + rootPivotRot[i]*pelvisRot[i]*waistRot[i]*torsoRot[i]*(new Vector3(0.0f,0.0f,shoulderWidth));
			Debug.Log ("FL_hipPos = " + FL_hipPos[i] + ", FL_target = " + FL_target);
			FL_out = solver.LegRotationSolver(FL_hipPos[i],FL_target,
														species.frontLegData.thighData.length,
														species.frontLegData.calfData.length,
														species.frontLegData.footData.length,
														30.0f,0.0f,true);
			FL_hipRot[i] = FL_out.legPivotRotation;
			FL_thighRot[i] = FL_out.thighRotation;
			FL_calfRot[i] = FL_out.calfRotation;
			FL_footRot[i] = FL_out.footRotation;
			FL_toeRot[i] = Quaternion.identity;
			
			FR_hipPos[i] = neckPivot + rootPivotRot[i]*pelvisRot[i]*waistRot[i]*torsoRot[i]*(new Vector3(0.0f,0.0f,-shoulderWidth));
			FR_out = solver.LegRotationSolver(FR_hipPos[i],FR_target,
														species.frontLegData.thighData.length,
														species.frontLegData.calfData.length,
														species.frontLegData.footData.length,
														30.0f,0.0f,true);
			FR_hipRot[i] = FR_out.legPivotRotation;
			FR_thighRot[i] = FR_out.thighRotation;
			FR_calfRot[i] = FR_out.calfRotation;
			FR_footRot[i] = FR_out.footRotation;
			FR_toeRot[i] = Quaternion.identity;
			
			times[i] = t;
			i++;
		}
		
		// Assign values to animation clips in new AnimationGroup and store in ClipContainer
		ClipContainer.AnimationGroup animGroup = new ClipContainer.AnimationGroup();
		animGroup.rootClip = CreatePositionClip(rootPos,times,animName);
		animGroup.rootPivotClip = CreateRotationClip(rootPivotRot,times,animName);
		animGroup.pelvisClip = CreateRotationClip(pelvisRot,times,animName);
		animGroup.waistClip = CreateRotationClip(waistRot,times,animName);
		animGroup.torsoClip = CreateRotationClip(torsoRot,times,animName);
		
		animGroup.FL_HipClip = CreatePositionClip(FL_hipPos,times,animName);
		animGroup.FL_HipClip = CreateRotationClip(animGroup.FL_HipClip,FL_hipRot,times);
		animGroup.FL_ThighClip = CreateRotationClip(FL_thighRot,times,animName);
		animGroup.FL_KneeClip = CreateRotationClip(FL_calfRot,times,animName);
		animGroup.FL_AnkleClip = CreateRotationClip(FL_footRot,times,animName);
		animGroup.FL_ToeClip = CreateRotationClip(FL_toeRot,times,animName);
		
		animGroup.FR_HipClip = CreatePositionClip(FR_hipPos,times,animName);
		animGroup.FR_HipClip = CreateRotationClip(animGroup.FR_HipClip,FR_hipRot,times);
		animGroup.FR_ThighClip = CreateRotationClip(FR_thighRot,times,animName);
		animGroup.FR_KneeClip = CreateRotationClip(FR_calfRot,times,animName);
		animGroup.FR_AnkleClip = CreateRotationClip(FR_footRot,times,animName);
		animGroup.FR_ToeClip = CreateRotationClip(FR_toeRot,times,animName);
		
		animGroup.BL_HipClip = CreatePositionClip(BL_hipPos,times,animName);
		animGroup.BL_HipClip = CreateRotationClip(animGroup.BL_HipClip,BL_hipRot,times);
		animGroup.BL_ThighClip = CreateRotationClip(BL_thighRot,times,animName);
		animGroup.BL_KneeClip = CreateRotationClip(BL_calfRot,times,animName);
		animGroup.BL_AnkleClip = CreateRotationClip(BL_footRot,times,animName);
		animGroup.BL_ToeClip = CreateRotationClip(BL_toeRot,times,animName);
		
		animGroup.BR_HipClip = CreatePositionClip(BR_hipPos,times,animName);
		animGroup.BR_HipClip = CreateRotationClip(animGroup.BR_HipClip,BR_hipRot,times);
		animGroup.BR_ThighClip = CreateRotationClip(BR_thighRot,times,animName);
		animGroup.BR_KneeClip = CreateRotationClip(BR_calfRot,times,animName);
		animGroup.BR_AnkleClip = CreateRotationClip(BR_footRot,times,animName);
		animGroup.BR_ToeClip = CreateRotationClip(BR_toeRot,times,animName);
		
		// etc...
		
		//SetAnimGroupName(ref animGroup, animName);
		GameObject.Destroy(solverObject);
		return animGroup;
		
		
	}
	/*
	void SetAnimGroupName(ref ClipContainer.AnimationGroup animGroup, string name) {
		animGroup.rootClip.name = name;
		animGroup.rootClip.name = name;
		animGroup.waistClip.name = name;
	    animGroup.torsoClip.name = name;
	    animGroup.neckPivotClip.name = name;
		foreach(AnimationClip clip in animGroup.neckJointClips) {clip.name = name;}
	    animGroup.headClip.name = name;
	    animGroup.tailClip.name = name;
		foreach(AnimationClip clip in animGroup.tailClips) {clip.name = name;}
		// Leg joint clips
		animGroup.frontHipClip.name = name;
		animGroup.frontThighClip.name = name;
		animGroup.frontKneeClip.name = name;
		animGroup.frontAnkleClip.name = name;
		animGroup.frontToeClip.name = name;
		animGroup.backHipClip.name = name;
		animGroup.backThighClip.name = name;
		animGroup.backKneeClip.name = name;
		animGroup.backAnkleClip.name = name;
		animGroup.backToeClip.name = name;
		// Arm joint clips
		animGroup.rightShoulderClip.name = name;
	    animGroup.rightUpperArmClip.name = name;
	    animGroup.rightElbowClip.name = name;
	    animGroup.rightWristClip.name = name;
		foreach(AnimationClip clip in animGroup.rightDigitClips) {clip.name = name;}
		animGroup.leftShoulderClip.name = name;
	    animGroup.leftUpperArmClip.name = name;
	    animGroup.leftElbowClip.name = name;
	    animGroup.leftWristClip.name = name;
		foreach(AnimationClip clip in animGroup.leftDigitClips) {clip.name = name;}
		// Head clips
		animGroup.jawClip.name = name;
		animGroup.rightEyeClip.name = name;
		animGroup.rightUpperEyelidClip.name = name;
		animGroup.rightLowerEyelidClip.name = name;
		animGroup.leftEyeClip.name = name;
		animGroup.leftUpperEyelidClip.name = name;
		animGroup.leftLowerEyelidClip.name = name;
	}
	*/
	private void UpdateDictionaries(string animName) {
		Debug.Log("animGroup in UpdateDictionaries... -> " + animGroup.rootClip);
		ClipContainer clips = species.clips;
		Debug.Log("clips = " + clips);
		clips.rootDict.Add(animName,animGroup.rootClip);
		clips.rootPivotDict.Add(animName,animGroup.rootPivotClip);
		clips.pelvisDict.Add(animName,animGroup.pelvisClip);
		clips.waistDict.Add(animName,animGroup.waistClip);
		clips.torsoDict.Add(animName,animGroup.torsoClip);
		
		clips.FL_HipDict.Add(animName,animGroup.FL_HipClip);
		clips.FL_ThighDict.Add(animName,animGroup.FL_ThighClip);
		clips.FL_KneeDict.Add(animName,animGroup.FL_KneeClip);
		clips.FL_AnkleDict.Add(animName,animGroup.FL_AnkleClip);
		clips.FL_ToeDict.Add(animName,animGroup.FL_ToeClip);
		clips.FR_HipDict.Add(animName,animGroup.FR_HipClip);
		clips.FR_ThighDict.Add(animName,animGroup.FR_ThighClip);
		clips.FR_KneeDict.Add(animName,animGroup.FR_KneeClip);
		clips.FR_AnkleDict.Add(animName,animGroup.FR_AnkleClip);
		clips.FR_ToeDict.Add(animName,animGroup.FR_ToeClip);
		clips.BL_HipDict.Add(animName,animGroup.BL_HipClip);
		clips.BL_ThighDict.Add(animName,animGroup.BL_ThighClip);
		clips.BL_KneeDict.Add(animName,animGroup.BL_KneeClip);
		clips.BL_AnkleDict.Add(animName,animGroup.BL_AnkleClip);
		clips.BL_ToeDict.Add(animName,animGroup.BL_ToeClip);
		clips.BR_HipDict.Add(animName,animGroup.BR_HipClip);
		clips.BR_ThighDict.Add(animName,animGroup.BR_ThighClip);
		clips.BR_KneeDict.Add(animName,animGroup.BR_KneeClip);
		clips.BR_AnkleDict.Add(animName,animGroup.BR_AnkleClip);
		clips.BR_ToeDict.Add(animName,animGroup.BR_ToeClip);
		// etc...
	}
	
	protected virtual Vector3 AF_rootPosition(float t) {return root_defOffset;}
	protected virtual Quaternion AF_rootPivotRotation(float t) {return root_defRotation;}
	protected virtual Vector3 AF_FL_LegTarget(float t){return FL_defOffset;}
	protected virtual Vector3 AF_FR_LegTarget(float t){return FR_defOffset;}
	protected virtual Vector3 AF_BL_LegTarget(float t){return BL_defOffset;}
	protected virtual Vector3 AF_BR_LegTarget(float t){return BR_defOffset;}
	protected virtual float AF_spine_CX(float t){return 0.0f;}
	protected virtual float AF_spine_CY(float t){return 0.0f;}
	protected virtual float AF_spine_SX(float t){return 0.0f;}
	protected virtual float AF_spine_SY(float t){return 0.0f;}
	
}

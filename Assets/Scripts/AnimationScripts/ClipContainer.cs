using UnityEngine;
using System.Collections;
using System.Collections.Generic;

// This class acts as a container to hold all the animation clips for a creature.

public class ClipContainer {
	
	CreatureSpecies species;
	
	// GameObjects requiring animation
	/*
	 Spine Joints
	  - rootPivot (pelvis)
	  - waistPivot
	  - torsoPivot
	  - neckPivot
	  - neckJoints
	  - headPivot
	  - tailPivot
	  - tailJoints
	 Leg Joints
	  - hipPivot
	  - thighPivot
	  - kneePivot
	  - anklePivot
	  - toePivot
	 Arm Joints
	  - shoulderPivot
	  - upperArmPivot
	  - elbowPivot
	  - wristPivot
	  - digitPivots
	 Head Joints
	  - jawPivot
	  - eyePivots
	  - eyelidPivots
	*/
	
	// Animations required
	/*
	 - idle
	 - lie down (sleep/rest)
	 - walk
	 - run (trot, canter, gallop)
	 - turn
	 - running turn
	 - jaw attack
	 - claw attack
	 - charge attack
	 - taunts
	*/
	
	public struct AnimationGroup {
		public bool groupActive;
		// Spine joint clips
		public AnimationClip rootClip;
		public AnimationClip rootPivotClip;
		public AnimationClip pelvisClip;
		public AnimationClip waistClip;
	    public AnimationClip torsoClip;
	    public AnimationClip neckPivotClip;
	    public AnimationClip[] neckJointClips;
	    public AnimationClip headClip;
	    public AnimationClip tailClip;
	    public AnimationClip[] tailClips;
		// Leg joint clips
		public AnimationClip FL_HipClip;
		public AnimationClip FL_ThighClip;
		public AnimationClip FL_KneeClip;
		public AnimationClip FL_AnkleClip;
		public AnimationClip FL_ToeClip;
		public AnimationClip FR_HipClip;
		public AnimationClip FR_ThighClip;
		public AnimationClip FR_KneeClip;
		public AnimationClip FR_AnkleClip;
		public AnimationClip FR_ToeClip;
		public AnimationClip BL_HipClip;
		public AnimationClip BL_ThighClip;
		public AnimationClip BL_KneeClip;
		public AnimationClip BL_AnkleClip;
		public AnimationClip BL_ToeClip;
		public AnimationClip BR_HipClip;
		public AnimationClip BR_ThighClip;
		public AnimationClip BR_KneeClip;
		public AnimationClip BR_AnkleClip;
		public AnimationClip BR_ToeClip;
		// Arm joint clips
		public AnimationClip rightShoulderClip;
	    public AnimationClip rightUpperArmClip;
	    public AnimationClip rightElbowClip;
	    public AnimationClip rightWristClip;
	    public AnimationClip[] rightDigitClips;
		public AnimationClip leftShoulderClip;
	    public AnimationClip leftUpperArmClip;
	    public AnimationClip leftElbowClip;
	    public AnimationClip leftWristClip;
	    public AnimationClip[] leftDigitClips;
		// Head clips
		public AnimationClip jawClip;
		public AnimationClip rightEyeClip;
		public AnimationClip rightUpperEyelidClip;
		public AnimationClip rightLowerEyelidClip;
		public AnimationClip leftEyeClip;
		public AnimationClip leftUpperEyelidClip;
		public AnimationClip leftLowerEyelidClip;
	}
	
	public Dictionary<string, Dictionary<string,AnimationClip>> partDictionary;
	public Dictionary<string,AnimationClip> rootDict = new Dictionary<string,AnimationClip>();
	public Dictionary<string,AnimationClip> rootPivotDict = new Dictionary<string,AnimationClip>();
	public Dictionary<string,AnimationClip> pelvisDict = new Dictionary<string,AnimationClip>();
	public Dictionary<string,AnimationClip> waistDict = new Dictionary<string,AnimationClip>();
	public Dictionary<string,AnimationClip> torsoDict = new Dictionary<string,AnimationClip>();
	public Dictionary<string,AnimationClip> neckPivotDict = new Dictionary<string,AnimationClip>();
	//public Hashtable[] neckJointClips;
    public Dictionary<string,AnimationClip> headDict = new Dictionary<string,AnimationClip>();
    public Dictionary<string,AnimationClip> tailDict = new Dictionary<string,AnimationClip>();
    //public Hashtable[] tailClips;
	// Leg joint clips
	public Dictionary<string,AnimationClip> FL_HipDict = new Dictionary<string,AnimationClip>();
	public Dictionary<string,AnimationClip> FL_ThighDict = new Dictionary<string,AnimationClip>();
	public Dictionary<string,AnimationClip> FL_KneeDict = new Dictionary<string,AnimationClip>();
	public Dictionary<string,AnimationClip> FL_AnkleDict = new Dictionary<string,AnimationClip>();
	public Dictionary<string,AnimationClip> FL_ToeDict = new Dictionary<string,AnimationClip>();
	public Dictionary<string,AnimationClip> FR_HipDict = new Dictionary<string,AnimationClip>();
	public Dictionary<string,AnimationClip> FR_ThighDict = new Dictionary<string,AnimationClip>();
	public Dictionary<string,AnimationClip> FR_KneeDict = new Dictionary<string,AnimationClip>();
	public Dictionary<string,AnimationClip> FR_AnkleDict = new Dictionary<string,AnimationClip>();
	public Dictionary<string,AnimationClip> FR_ToeDict = new Dictionary<string,AnimationClip>();
	public Dictionary<string,AnimationClip> BL_HipDict = new Dictionary<string,AnimationClip>();
	public Dictionary<string,AnimationClip> BL_ThighDict = new Dictionary<string,AnimationClip>();
	public Dictionary<string,AnimationClip> BL_KneeDict = new Dictionary<string,AnimationClip>();
	public Dictionary<string,AnimationClip> BL_AnkleDict = new Dictionary<string,AnimationClip>();
	public Dictionary<string,AnimationClip> BL_ToeDict = new Dictionary<string,AnimationClip>();
	public Dictionary<string,AnimationClip> BR_HipDict = new Dictionary<string,AnimationClip>();
	public Dictionary<string,AnimationClip> BR_ThighDict = new Dictionary<string,AnimationClip>();
	public Dictionary<string,AnimationClip> BR_KneeDict = new Dictionary<string,AnimationClip>();
	public Dictionary<string,AnimationClip> BR_AnkleDict = new Dictionary<string,AnimationClip>();
	public Dictionary<string,AnimationClip> BR_ToeDict = new Dictionary<string,AnimationClip>();
	// Arm joint clips
	public Dictionary<string,AnimationClip> rightShoulderDict = new Dictionary<string,AnimationClip>();
    public Dictionary<string,AnimationClip> rightUpperArmDict = new Dictionary<string,AnimationClip>();
    public Dictionary<string,AnimationClip> rightElbowDict = new Dictionary<string,AnimationClip>();
    public Dictionary<string,AnimationClip> rightWristDict = new Dictionary<string,AnimationClip>();
   // public Hashtable[] rightDigitClips;
	public Dictionary<string,AnimationClip> leftShoulderDict = new Dictionary<string,AnimationClip>();
    public Dictionary<string,AnimationClip> leftUpperArmDict = new Dictionary<string,AnimationClip>();
    public Dictionary<string,AnimationClip> leftElbowDict = new Dictionary<string,AnimationClip>();
    public Dictionary<string,AnimationClip> leftWristDict = new Dictionary<string,AnimationClip>();
    //public Hashtable[] leftDigitClips;
	// Head clips
	public Dictionary<string,AnimationClip> jawDict = new Dictionary<string,AnimationClip>();
	public Dictionary<string,AnimationClip> rightEyeDict = new Dictionary<string,AnimationClip>();
	public Dictionary<string,AnimationClip> rightUpperEyelidDict = new Dictionary<string,AnimationClip>();
	public Dictionary<string,AnimationClip> rightLowerEyelidDict = new Dictionary<string,AnimationClip>();
	public Dictionary<string,AnimationClip> leftEyeDict = new Dictionary<string,AnimationClip>();
	public Dictionary<string,AnimationClip> leftUpperEyelidDict = new Dictionary<string,AnimationClip>();
	public Dictionary<string,AnimationClip> leftLowerEyelidDict = new Dictionary<string,AnimationClip>();
	// etc...
	
	public enum AnimType {
		IDLE,
		WALK
		// etc...
	};
	
	public AnimationGroup idleAnim;
	public AnimationGroup walkAnim;
	// etc...
	
	public ClipContainer(CreatureSpecies species) {
		this.species = species;
		partDictionary = new Dictionary<string, Dictionary<string, AnimationClip>>();
		partDictionary.Add("root",rootDict);
		partDictionary.Add("rootPivot",rootPivotDict);
		partDictionary.Add("pelvis",pelvisDict);
		partDictionary.Add("waist",waistDict);
		partDictionary.Add("torso",torsoDict);
		partDictionary.Add("FL_Hip",FL_HipDict);
		partDictionary.Add("FL_Thigh",FL_ThighDict);
		partDictionary.Add("FL_Knee",FL_KneeDict);
		partDictionary.Add("FL_Ankle",FL_AnkleDict);
		partDictionary.Add("FL_Toe",FL_ToeDict);
		partDictionary.Add("FR_Hip",FR_HipDict);
		partDictionary.Add("FR_Thigh",FR_ThighDict);
		partDictionary.Add("FR_Knee",FR_KneeDict);
		partDictionary.Add("FR_Ankle",FR_AnkleDict);
		partDictionary.Add("FR_Toe",FR_ToeDict);
		partDictionary.Add("BL_Hip",BL_HipDict);
		partDictionary.Add("BL_Thigh",BL_ThighDict);
		partDictionary.Add("BL_Knee",BL_KneeDict);
		partDictionary.Add("BL_Ankle",BL_AnkleDict);
		partDictionary.Add("BL_Toe",BL_ToeDict);
		partDictionary.Add("BR_Hip",BR_HipDict);
		partDictionary.Add("BR_Thigh",BR_ThighDict);
		partDictionary.Add("BR_Knee",BR_KneeDict);
		partDictionary.Add("BR_Ankle",BR_AnkleDict);
		partDictionary.Add("BR_Toe",BR_ToeDict);
		
		//etc...
	}
	
	public Dictionary<string,AnimationClip> GetAnimPartDictionary(string partName) {
		return partDictionary[partName];
	}
	
	public void EnableAnimationGroup(AnimType anim) {
		switch(anim) {
		case AnimType.IDLE:
			IdleAnimation idleAnimator = new IdleAnimation(species,"idle");
			idleAnim = idleAnimator.idleAnim;
			idleAnim.groupActive = true;
			Debug.Log("idleAnim -> " + idleAnim.rootClip + ", " + idleAnim.FL_AnkleClip);
			break;
		case AnimType.WALK:
			
			break;
		}
	}
	
	public int ActiveGroupNum() {
		int num = 0;
		num = idleAnim.groupActive?num+1:num;
		num = walkAnim.groupActive?num+1:num;
		// ... etc
		return num;
	}
}

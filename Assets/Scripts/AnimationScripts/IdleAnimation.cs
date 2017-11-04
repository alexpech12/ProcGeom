using UnityEngine;
using System.Collections;

public class IdleAnimation : AnimationBase {
	
	ClipContainer.AnimationGroup animGroup; public ClipContainer.AnimationGroup idleAnim { get { return animGroup; } }
	
	const float animTime = 2.0f;
	
	// It is the constructors function, when this object is created, to fill the animation group with all the relevant animation clips
	public IdleAnimation(CreatureSpecies species,string animName) : base(species,animName,animTime) {}
	
	override protected Vector3 AF_rootPosition(float t) {
		Vector3 offset = base.AF_rootPosition(t);
		offset = new Vector3(offset.x,species.backLegData.standHeight,offset.z);
		return offset+new Vector3(0.0f,0.1f*Mathf.Sin(((2*Mathf.PI)/animTime)*t),0.0f);
	}
	override protected Quaternion AF_rootPivotRotation(float t) {
		Quaternion defRotation = base.AF_rootPivotRotation(t);
		return defRotation*Quaternion.Euler(0.0f,5.0f*Mathf.Sin(((2*Mathf.PI)/animTime)*t),0.0f);
	}
	
	override protected float AF_spine_CX(float t) {
		return 0.1f*Mathf.Sin(((2*Mathf.PI)/animTime)*t);
	}
	
	override protected float AF_spine_CY(float t) {
		return 0.05f*Mathf.Cos(((2*Mathf.PI)/animTime)*t);
	}
	
	override protected Vector3 AF_FL_LegTarget(float t) {
		Vector3 offset = base.AF_FL_LegTarget(t);
		return offset;
	}
	override protected Vector3 AF_FR_LegTarget(float t) {
		Vector3 offset = base.AF_FR_LegTarget(t);
		return offset;
	}
	override protected Vector3 AF_BL_LegTarget(float t) {
		Vector3 offset = base.AF_BL_LegTarget(t);
		return offset;
	}
	override protected Vector3 AF_BR_LegTarget(float t) {
		Vector3 offset = base.AF_BR_LegTarget(t);
		return offset;
	}
	
}

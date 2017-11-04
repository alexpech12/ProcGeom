using UnityEngine;
using System.Collections;

public class JointSolver : MonoBehaviour {
	
	public struct LegSolverOutput {
		public Quaternion legPivotRotation;
		public Quaternion thighRotation;
		public Quaternion calfRotation;
		public Quaternion footRotation;
	}
	
	public struct SpineSolverOutput {
		public Quaternion pelvisRotation;
		public Quaternion waistRotation;
		public Quaternion torsoRotation;
		public float neckDistance;
	}
	
	// Takes a Vector3 x,y,z position offset, joint lengths and heel elevation and returns an array of quaternions as (legPivot, thigh, calf, foot)
	public LegSolverOutput LegRotationSolver(Vector3 hipPoint, Vector3 targetPoint, float thighLength, float calfLength, float footLength, float heelElevationAngle, float kneeAngle, bool reverseKnee) {
		
		// Calculate new joint angles
		
		float L1 = thighLength;
		float L2 = calfLength;
		float L3 = footLength;
		float thetaE = heelElevationAngle;
		
		// Everything relative to hip joint
		Vector3 relativePoint = hipPoint;
		Vector3 directionVector = targetPoint-relativePoint;
		
		//Vector3 Px = new Vector3(targetPoint.x,targetPoint.y,0.0f) - relativePoint; // Target point
		Vector3 Px = new Vector3(directionVector.x,-Mathf.Sqrt(directionVector.y*directionVector.y+directionVector.z*directionVector.z),0.0f); // Target point
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
		
		LegSolverOutput output = new LegSolverOutput();
		output.legPivotRotation = Quaternion.FromToRotation(Vector3.down,directionVector)*Quaternion.Euler(0.0f,kneeAngle+180.0f,0.0f);
		output.thighRotation = Quaternion.Euler(0.0f,0.0f,theta1);
		output.calfRotation = Quaternion.Euler(0.0f,0.0f,theta2);
		output.footRotation = Quaternion.Euler(0.0f,0.0f,thetaE-theta1-theta2-legAngle);
		
		return output;
	}
	
	// Takes values for modifying C and S curves along a spine of given length and returns the proper rotations. Max and min values are given in bounds arguments
	public SpineSolverOutput SpineRotationSolver(float C_CurveX, float C_CurveY, float S_CurveX, float S_CurveY, float x_bound, float y_bound, float pelvisLength, float waistLength, float torsoLength) {
		
		float spineLength = pelvisLength+waistLength+torsoLength;
		float pelvisRatio = pelvisLength/spineLength;
		float waistRatio = waistLength/spineLength;
		float torsoRatio = torsoLength/spineLength;
		
		float angle_CX = x_bound*C_CurveX;
		float angle_CY = y_bound*C_CurveY;
		float angle_SX = x_bound*S_CurveX;
		float angle_SY = y_bound*S_CurveY;
		
		
		Quaternion pelvisRotation = Quaternion.Euler(pelvisRatio*(angle_CX+angle_SX),0.0f,pelvisRatio*(angle_CY+angle_SY));
		Quaternion waistRotation = Quaternion.Euler(waistRatio*(angle_CX-angle_SX),0.0f,waistRatio*(angle_CY-angle_SY));
		Quaternion torsoRotation = Quaternion.Euler(torsoRatio*(angle_CX+angle_SX),0.0f,torsoRatio*(angle_CY+angle_SY));
		
		// Calculate endpoint
		Vector3 endPoint = pelvisRotation*(waistRotation*((torsoRotation*new Vector3(0.0f,torsoLength,0.0f))+new Vector3(0.0f,waistLength,0.0f))+new Vector3(0.0f,pelvisLength,0.0f));
		
		Quaternion compensationRotation = Quaternion.FromToRotation(endPoint,Vector3.up);
		pelvisRotation = compensationRotation*pelvisRotation;
		
		SpineSolverOutput output = new SpineSolverOutput();
		output.pelvisRotation = pelvisRotation;
		output.waistRotation = waistRotation;
		output.torsoRotation = torsoRotation;
		output.neckDistance = endPoint.magnitude;
		
		return output;
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

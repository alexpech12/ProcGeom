using UnityEngine;
using System.Collections;

      //---------------------------------------------------------------------------//
    // This class used as a container for all variables related to leg creation. //
  //         This allows data to easily be passed between functions.           //
//---------------------------------------------------------------------------//

public class LegData : BaseData {
	
	BodySegmentMeshData m_thighData;	public BodySegmentMeshData thighData { get { return m_thighData; } }
	BodySegmentMeshData m_calfData;		public BodySegmentMeshData calfData { get { return m_calfData; } }
	BodySegmentMeshData m_footData;		public BodySegmentMeshData footData { get { return m_footData; } }
	BodySegmentMeshData m_toeData;		public BodySegmentMeshData toeData { get { return m_toeData; } }
	
	//Bezier3D m_legWidthCurve;			public Bezier3D legWidthCurve { get { return m_legWidthCurve; } }
	
	// Stand height takes into account a natural bend whereas leg length is just the total length if leg is perfectly straight
	float m_standHeight;				public float standHeight { get { return m_standHeight; } }
	float m_legLength;					public float legLength { get { return m_legLength; } }
	
	// Used to shift leg up or down from spine
	float m_heightOffset = 0.0f;		public float heightOffset { get { return m_heightOffset; } }
	
	// widthOffset determined later by the width curve of the body
	float m_widthOffset;				public float widthOffset { get { return m_widthOffset; } set { m_widthOffset = value;} }
	
	float m_standKneeAngle = 60.0f;		public float standKneeAngle { get { return m_standKneeAngle; } }
	bool m_reverseKnee = false;			public bool reverseKnee { get { return m_reverseKnee; } set { m_reverseKnee = value;} }
	float m_elevationAngle = 60.0f;		public float elevationAngle { get { return m_elevationAngle; } }
	float m_standStraightness = 0.8f;	public float standStraightness { get { return m_standStraightness; } }
	float m_walkSpeed = 2.0f;			public float walkSpeed { get { return m_walkSpeed; } }
	float m_stepLength = 3.0f;			public float stepLength { get { return m_stepLength; } }
	float m_animationTOffset = 0.5f;	public float animationTOffset { get { return m_animationTOffset; } }
	
	
	public LegData(BodySegmentMeshData thigh, BodySegmentMeshData calf, BodySegmentMeshData foot, BodySegmentMeshData toe) {
		
		m_thighData = thigh;
		m_calfData = calf;
		m_footData = foot;
		m_toeData = toe;
		
		m_legLength = m_thighData.length+m_calfData.length+m_footData.length;
		m_standHeight =  m_legLength*standStraightness;
		
		float legUpperWidth = 1.0f;
		float legMid1Width = 0.8f;
		float legMid2Width = 0.7f;
		float legLowerWidth = 0.3f;
		
		/*
		m_legWidthCurve = new Bezier3D(new Vector3(0.0f,legUpperWidth,0.0f),
									   new Vector3(0.33f,legMid1Width,0.0f),
									   new Vector3(0.67f,legMid2Width,0.0f),
									   new Vector3(1.0f,legLowerWidth,0.0f));
									   */
	}
}

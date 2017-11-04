using UnityEngine;
using System.Collections;

public class SpineData : BaseData {

	BodySegmentMeshData m_torsoData;	public BodySegmentMeshData torsoData { get { return m_torsoData; } }
	BodySegmentMeshData m_waistData;	public BodySegmentMeshData waistData { get { return m_waistData; } }
	BodySegmentMeshData m_pelvisData;	public BodySegmentMeshData pelvisData { get { return m_pelvisData; } }
	
	float m_spineLength;				public float spineLength { get { return m_spineLength; } }
	float m_pelvisAngleOffset;			public float pelvisAngleOffset { get { return m_pelvisAngleOffset; } }
	float m_waistAngleOffset;			public float waistAngleOffset { get { return m_waistAngleOffset; } }
	
	
	public SpineData(BodySegmentMeshData torso, BodySegmentMeshData waist, BodySegmentMeshData pelvis, float pelvisAngleOffset, float waistAngleOffset) {
		
		m_torsoData = torso;
		m_waistData = waist;
		m_pelvisData = pelvis;
		m_pelvisAngleOffset = pelvisAngleOffset;
		m_waistAngleOffset = waistAngleOffset;
		
		m_spineLength = m_torsoData.length+m_waistData.length+m_pelvisData.length;
	}
}

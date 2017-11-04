using UnityEngine;
using System.Collections;

public class BodySegmentMeshData : BaseData {
	
	private float m_length = 1.0f;		public float length { get { return m_length; } }
	private float m_radiusA = 0.5f;		public float radiusA { get { return m_radiusA; } }
	private float m_radiusB = 0.3f;		public float radiusB { get { return m_radiusB; } }
	private float m_roundnessA = 1.0f;	public float roundnessA { get { return m_roundnessA; } }
	private float m_roundnessB = 1.0f; 	public float roundnessB { get { return m_roundnessB; } }
	private float m_angleA = 0.0f;		public float angleA { get { return m_angleA; } }
	private float m_angleB = 0.0f; 		public float angleB { get { return m_angleB; } }
	private int m_heightSegments = 8;	public int heightSegments { get { return m_heightSegments; } }
	private int m_radialSegments = 8;	public int radialSegments { get { return m_radialSegments; } }
	
	public BodySegmentMeshData() {}
	
	public BodySegmentMeshData(bool randomize) {
		if(randomize) {RandomizeData();}
	}
	
	public BodySegmentMeshData(BodySegmentMeshData referenceData) {
		RandomizeDataFromReference(referenceData);
	}
	// This function is a simplified version of the one below.
	// Only one segment integer is taken, and quads are kept relatively square
	// The roundness variable controls both angles and roundness arguments
	// This limits shape to concave or convex
	public BodySegmentMeshData(float length, float radiusA, float radiusB, float roundness, int segments) {
		
		float radiusAverage = (radiusA+radiusB)/2.0f;
		float polyRatio = length/(2*Mathf.PI*radiusAverage);
		int heightSegments = (int)Mathf.RoundToInt(segments*polyRatio);
		
		// Angles always negative of each other and inversely proportional to radius/length ratios
		float radiusARatio = radiusA/length;
		float radiusBRatio = radiusB/length;
		Mathf.Clamp(roundness,0.0f,1.0f);
		float angleAMax = -180.0f*radiusARatio;
		float angleAMin = 180.0f*radiusARatio;
		float angleA = angleAMax*roundness+angleAMin*(1.0f-roundness);
		float angleBMax = 180.0f*radiusBRatio;
		float angleBMin = -180.0f*radiusBRatio;
		float angleB = angleBMax*roundness+angleBMin*(1.0f-roundness);
		float roundnessA = 0.5f-0.5f*(angleB/90.0f);
		float roundnessB = 0.5f-0.5f*(angleB/90.0f);
		
		m_length = length;
		m_radiusA = radiusA; m_radiusB = radiusB;
		m_roundnessA = roundnessA; m_roundnessB = roundnessB;
		m_angleA = angleA; m_angleB = angleB;
		m_heightSegments = heightSegments; m_radialSegments = segments;
	}
	
	public BodySegmentMeshData(float length,float radiusA,float radiusB,float roundnessA,float roundnessB,float angleA,float angleB,int heightSegments,int radialSegments) {
		m_length = length;
		m_radiusA = radiusA; m_radiusB = radiusB;
		m_roundnessA = roundnessA; m_roundnessB = roundnessB;
		m_angleA = angleA; m_angleB = angleB;
		m_heightSegments = heightSegments; m_radialSegments = radialSegments;
	}
	
	public void RandomizeData() {
		
	}
	public void RandomizeDataFromReference(BodySegmentMeshData dataRef) {
		
	}
	
	public void SetRadii(float newRadA, float newRadB) {
		m_radiusA = newRadA;
		m_radiusB = newRadB;
	}
}

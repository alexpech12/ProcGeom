using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CompositeBezier3D {
	
	public List<Vector3> Points { get { return m_Points; } }
	private List<Vector3> m_Points = new List<Vector3>();
	
	private List<float> m_DistSquared = new List<float>();
	private float total_dist_squared = 0f;
	
	private int n = 0; // Number of 3-segment beziers in composite
	
	// Composite bezier acts as regular bezier when only 4 points are passed
	// Number of points, n, must be multiple of 3n+4
	public CompositeBezier3D(Vector3 start, Vector3 mid1, Vector3 mid2, Vector3 end) {
		m_Points.Add(start);
		AddPoints(mid1,mid2,end);
		
	}
	
	public void AddPoints(Vector3 mid1, Vector3 mid2, Vector3 end)
	{
		m_Points.Add(mid1);
		m_Points.Add(mid2);
		m_Points.Add(end);
		int index = m_Points.Count-1; // Points to last added value
		m_DistSquared.Add((m_Points[index-4]-mid1).sqrMagnitude);
		m_DistSquared.Add((mid2-mid1).sqrMagnitude);
		m_DistSquared.Add((end-mid2).sqrMagnitude);
		int dist_index = m_DistSquared.Count-1;
		total_dist_squared += m_DistSquared[index]+m_DistSquared[index-1]+m_DistSquared[index-2];
		n++;
	}
	
	public Vector3 Point(float t)
	{
		// Step through distance list until ratio is equivalent to t
		float dist = 0;
		for(int i = 0; i < n; i++) {
			dist += m_DistSquared[i*3];
			if(dist/total_dist_squared > t) break; // Need to remember i value
			dist += m_DistSquared[i*3+1];
			if(dist/total_dist_squared > t) break;
			dist += m_DistSquared[i*3+2];
			if(dist/total_dist_squared > t) break;
		}
		return Vector3.zero;
	}
	
	private Vector3 PointSeg(float t, Vector3 start, Vector3 mid1, Vector3 mid2, Vector3 end)
	{
		float t2 = t * t;
		float t3 = t2 * t;

		float mt = 1 - t;
		float mt2 = mt * mt;
		float mt3 = mt2 * mt;

		return start * mt3 + mid1 * mt2 * t * 3.0f + mid2 * mt * t2 * 3.0f + end * t3;
	}

	private Vector3 TangentSeg(float t, Vector3 start, Vector3 mid1, Vector3 mid2, Vector3 end)
	{
		float t2 = t * t;

		float mt = 1 - t;
		float mt2 = mt * mt;

		float mid = 2.0f * t * mt;

		Vector3 tangent = start * -mt2 + mid1 * (mt2 - mid) + mid2 * (-t2 + mid) + end * t2;

		return tangent.normalized;
	}
}

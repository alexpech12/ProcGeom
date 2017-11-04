using UnityEngine;
using System.Collections;

public class Bezier3D {
	
	public Vector3 start;
	public Vector3 mid1;
	public Vector3 mid2;
	public Vector3 end;
	
	public Bezier3D(Vector3 startP, Vector3 mid1P, Vector3 mid2P, Vector3 endP) {
		start = startP;
		mid1 = mid1P;
		mid2 = mid2P;
		end = endP;
	}
	
	public Vector3 Point(float t)
	{
		float t2 = t * t;
		float t3 = t2 * t;

		float mt = 1 - t;
		float mt2 = mt * mt;
		float mt3 = mt2 * mt;

		return start * mt3 + mid1 * mt2 * t * 3.0f + mid2 * mt * t2 * 3.0f + end * t3;
	}

	public Vector3 Tangent(float t)
	{
		float t2 = t * t;

		float mt = 1 - t;
		float mt2 = mt * mt;

		float mid = 2.0f * t * mt;

		Vector3 tangent = start * -mt2 + mid1 * (mt2 - mid) + mid2 * (-t2 + mid) + end * t2;

		return tangent.normalized;
	}
}

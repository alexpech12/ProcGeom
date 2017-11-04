using UnityEngine;
using System.Collections;

public class PGFruit : PGBase {
	
	public float maxRadius = 0.0f;
	
	public override Mesh BuildLODMesh (int LODindex)
	{
		throw new System.NotImplementedException ();
	}
	
	public override Mesh BuildMesh (bool setupLODs)
	{
		FruitBase b = gameObject.GetComponent<FruitBase>();
		MeshBuilder meshBuilder = new MeshBuilder();
		Vector3 offset = new Vector3(0.0f,-b.height,0.0f);
		//BuildSphere(meshBuilder,offset,b.radius,8,8);
		BuildRevolvedCurve(meshBuilder,offset,b.height,b.radius,b.height_segments,b.radial_segments);
		return meshBuilder.CreateMesh();
	}
	
	private void BuildRevolvedCurve(MeshBuilder meshBuilder, Vector3 offset, float height, float radiusGuide, int heightSegments, int radialSegments) {
		// Define bezier curve
		Vector3 start = Vector3.zero;
		//Vector3 end = Vector3.up*height;
		Vector3 end = Vector3.up*(height/2.0f)+Vector3.right*Random.Range(radiusGuide*0.25f,radiusGuide);
		Vector3 mid1 = start+Vector3.right*Random.Range(radiusGuide*0.25f,radiusGuide)+Vector3.up*Random.Range(-0.25f,0.25f)*height;
		//Vector3 mid2 = end + Vector3.right*Random.Range(0.1f,1.0f)*radiusGuide;
		Vector3 mid2 = end + Vector3.down*Random.Range(0.05f,0.5f)*height;
		for(int t = 0; t <= heightSegments/2; t++) {
			Vector3 radiusVec = Bezier(start,mid1,mid2,end,(float)(t*2)/(float)heightSegments);
			if(radiusVec.x > maxRadius) {maxRadius = radiusVec.x;}
			//Debug.Log("radiusVec = " + radiusVec);
			float v = (float)t/(float)heightSegments;
			BuildRing(meshBuilder,offset+new Vector3(0.0f,radiusVec.y,0.0f),radiusVec.x,radialSegments,v,t!=0);
		}
		start = end;
		end = Vector3.up*height;
		mid1 = start+Vector3.up*Random.Range(0.05f,0.5f)*height;
		mid2 = end + Vector3.right*Random.Range(radiusGuide*0.25f,radiusGuide)+Vector3.up*Random.Range(-0.25f,0.25f)*height;
		
		for(int t = 1; t <= heightSegments/2; t++) {
			Vector3 radiusVec = Bezier(start,mid1,mid2,end,(float)(t*2)/(float)heightSegments);
			if(radiusVec.x > maxRadius) {maxRadius = radiusVec.x;}
			//Debug.Log("radiusVec = " + radiusVec);
			float v = (float)t/(float)heightSegments+0.5f;
			BuildRing(meshBuilder,offset+new Vector3(0.0f,radiusVec.y,0.0f),radiusVec.x,radialSegments,v,true);
		}
		
	}
	
	private void BuildSphere(MeshBuilder meshBuilder, Vector3 offset, float radius, int heightSegments, int radialSegments) {
		for(int i = 0; i <= heightSegments; i++) {
			float distFromBase = -Mathf.Cos(Mathf.PI*((float)i/(float)heightSegments))*radius+radius;
			float ringRadius = radius*Mathf.Sin(Mathf.PI*((float)i/(float)heightSegments));
			BuildRing(meshBuilder,offset+new Vector3(0.0f,distFromBase,0.0f),ringRadius, radialSegments,(float)i/(float)heightSegments,i!=0);
		}
	}
	
	private void BuildRing(MeshBuilder meshBuilder, Vector3 offset, float radius, int radialSegments, float v, bool buildTriangles) {
		for(int i = 0; i <= radialSegments; i++) {
			// Calculate vertex position
			float theta = (Mathf.PI*2.0f*(float)i)/(float)radialSegments;
			Vector3 position = radius*new Vector3(Mathf.Cos(theta),0.0f,Mathf.Sin(theta));
			
			// Set normals to face along radial line
			meshBuilder.Normals.Add(new Vector3(Mathf.Cos(theta), 0, Mathf.Sin(theta)));
			meshBuilder.Vertices.Add(position + offset);
			meshBuilder.UVs.Add(new Vector2(i/(float)(radialSegments),v));
			
			if(buildTriangles) {
				int baseIndex = meshBuilder.Vertices.Count-1;
				
				if(i != 0) {
					meshBuilder.AddTriangle(baseIndex,baseIndex-radialSegments-1,baseIndex-1);
				}
				if(i != radialSegments) {
					meshBuilder.AddTriangle(baseIndex,baseIndex-radialSegments,baseIndex-radialSegments-1);
				}
			}
		}
	}
	
	public Vector3 Bezier(Vector3 start, Vector3 controlMid1, Vector3 controlMid2, Vector3 end, float t)
	{
		float t2 = t * t;
		float t3 = t2 * t;

		float mt = 1 - t;
		float mt2 = mt * mt;
		float mt3 = mt2 * mt;

		return start * mt3 + controlMid1 * mt2 * t * 3.0f + controlMid2 * mt * t2 * 3.0f + end * t3;
	}

	public Vector3 BezierTangent(Vector3 start, Vector3 controlMid1, Vector3 controlMid2, Vector3 end, float t)
	{
		float t2 = t * t;

		float mt = 1 - t;
		float mt2 = mt * mt;

		float mid = 2.0f * t * mt;

		Vector3 tangent = start * -mt2 + controlMid1 * (mt2 - mid) + controlMid2 * (-t2 + mid) + end * t2;

		return tangent.normalized;
	}
	
	public override void GenerateLODs (LODManager lodManager, MeshFilter filter)
	{
		throw new System.NotImplementedException ();
	}
	
	
}

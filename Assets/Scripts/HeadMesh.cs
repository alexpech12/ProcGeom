using UnityEngine;
using System.Collections;

public class HeadMesh : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	public Mesh BuildMesh(BodySegmentMeshData data) {
		return BuildMesh(data.length,data.radiusA,data.radiusB,data.roundnessA,data.roundnessB,data.angleA,data.angleB,data.heightSegments,data.radialSegments);
	}
	
	public Mesh BuildMesh(float length, float radiusA, float radiusB, float roundnessA, float roundnessB, float angleA, float angleB, int heightSegments, int radialSegments) {
		MeshBuilder meshBuilder = new MeshBuilder();
		
		// Test proportions here and log any debug or error messages
		if(length < radiusA || length < radiusB) {Debug.LogWarning("Warning: Bad body segment proportions. Length should be greater than minimum radius.");}
		
		// Determine cap segments based on height segments and relative lengths
		int capSegments = (int)Mathf.RoundToInt(((radiusA+radiusB)/(2*length))*heightSegments);
		
		// Define all curve points as 3D Vectors with 0 in z direction
		Vector3 A = new Vector3(0.0f,-roundnessA*radiusA,0.0f);
		Vector3 B = new Vector3(radiusA,0.0f,0.0f);
		Vector3 C = new Vector3(radiusB,length,0.0f);
		Vector3 D = new Vector3(0.0f,length+roundnessB*radiusB,0.0f);
		// Distances between vectors
		float d1 = Vector3.Distance(A,B);
		float d2 = Vector3.Distance(B,C);
		float d3 = Vector3.Distance(C,D);
		
		Vector3 midAB1 = A + new Vector3(d1/3.0f,0.0f,0.0f);
		Vector3 midAB2 = B + new Vector3((d1/3.0f)*Mathf.Cos(Mathf.Deg2Rad*(angleA-90)),(d1/3.0f)*Mathf.Sin(Mathf.Deg2Rad*(angleA-90)),0.0f);
		Vector3 midBC1 = B + new Vector3((d2/3.0f)*Mathf.Cos(Mathf.Deg2Rad*(angleA+90)),(d2/3.0f)*Mathf.Sin(Mathf.Deg2Rad*(angleA+90)),0.0f);
		Vector3 midBC2 = C + new Vector3((d2/3.0f)*Mathf.Cos(Mathf.Deg2Rad*(angleB-90)),(d2/3.0f)*Mathf.Sin(Mathf.Deg2Rad*(angleB-90)),0.0f);
		Vector3 midCD1 = C + new Vector3((d3/3.0f)*Mathf.Cos(Mathf.Deg2Rad*(angleB+90)),(d3/3.0f)*Mathf.Sin(Mathf.Deg2Rad*(angleB+90)),0.0f);
		Vector3 midCD2 = D + new Vector3(d3/3.0f,0.0f,0.0f);
		
		// Define the 3 bezier curves to use
		Bezier3D capCurveA = new Bezier3D(A,midAB1,midAB2,B);
		Bezier3D mainCurve = new Bezier3D(B,midBC1,midBC2,C);
		Bezier3D capCurveB = new Bezier3D(C,midCD1,midCD2,D);
		
		// Loop along curve to define geometry
		for(int t = 0; t <= capSegments; t++) {
			Vector3 radiusVec = capCurveA.Point((float)t/(float)capSegments);
			float v = (float)t/(float)capSegments;
			BuildRing(meshBuilder,new Vector3(0.0f,radiusVec.y,0.0f),radiusVec.x,radialSegments,v,t!=0);
		}
		for(int t = 1; t <= heightSegments; t++) {
			Vector3 radiusVec = mainCurve.Point((float)t/(float)heightSegments);
			float v = (float)t/(float)heightSegments;
			BuildRing(meshBuilder,new Vector3(0.0f,radiusVec.y,0.0f),radiusVec.x,radialSegments,v,true);
		}
		for(int t = 1; t <= capSegments; t++) {
			Vector3 radiusVec = capCurveB.Point((float)t/(float)capSegments);
			float v = (float)t/(float)heightSegments;
			BuildRing(meshBuilder,new Vector3(0.0f,radiusVec.y,0.0f),radiusVec.x,radialSegments,v,true);
		}
		return meshBuilder.CreateMesh();
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
}

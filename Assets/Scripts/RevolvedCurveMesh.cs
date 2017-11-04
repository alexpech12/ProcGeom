using UnityEngine;
using System.Collections;

public class RevolvedCurveMesh : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	public Mesh BuildMesh(float length, float upper_width, float lower_width, int heightSegments, int radialSegments) {
		MeshBuilder meshBuilder = new MeshBuilder();
		
		// Define bezier curve
		Vector3 start = Vector3.zero;
		Vector3 end = Vector3.up*length;
		Vector3 mid1 = start+Vector3.right*lower_width;
		Vector3 mid2 = end + Vector3.right*upper_width;
		for(int t = 0; t <= heightSegments; t++) {
			Bezier3D curve = new Bezier3D(start, mid1, mid2, end);
			Vector3 radiusVec = curve.Point((float)(t)/(float)heightSegments);
			//Vector3 radiusVec = Bezier(start,mid1,mid2,end,(float)(t)/(float)heightSegments);
			//if(radiusVec.x > maxRadius) {maxRadius = radiusVec.x;}
			//Debug.Log("radiusVec = " + radiusVec);
			float v = (float)t/(float)heightSegments;
			BuildRing(meshBuilder,/*offset+*/new Vector3(0.0f,radiusVec.y,0.0f),radiusVec.x,radialSegments,v,t!=0);
		}
		/*
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
		*/
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

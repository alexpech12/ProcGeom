using UnityEngine;
using System.Collections;

public class PGCylinder : MonoBehaviour {
	
	public float m_radius = 2;
	public float m_height = 4;
	public int m_width_segments = 8;
	public int m_height_segments = 4;
	public bool m_caps = false;
	
	// Use this for initialization
	void Start () {
		MeshBuilder meshBuilder = new MeshBuilder();
		
		//BuildCylinder(meshBuilder,new Vector3(0.0f,0.0f,0.0f),m_radius,m_height,m_width_segments,m_height_segments,m_caps);
		BuildSphere(meshBuilder,new Vector3(0.0f,0.0f,0.0f),m_radius,m_width_segments,m_height_segments);
		
		MeshFilter filter = GetComponent<MeshFilter>();
		if (filter != null) {
			filter.sharedMesh = meshBuilder.CreateMesh();
		}
	}
	
	void BuildRing(MeshBuilder meshBuilder, Vector3 offset, float radius, int segments, float v, bool buildTriangles) {
		
		for(int i = 0; i <= segments; i++) {
			float theta = (Mathf.PI*2.0f*(float)i)/(float)segments;
			float x = offset.x + Mathf.Cos(theta)*radius;
			float z = offset.z + Mathf.Sin(theta)*radius;
			
			// Set normals to face along radial line
			meshBuilder.Normals.Add(new Vector3(Mathf.Cos(theta), 0, Mathf.Sin(theta)));
			meshBuilder.Vertices.Add(new Vector3(x,offset.y,z));
			meshBuilder.UVs.Add(new Vector2(i/(float)(segments),v));
			
			if(buildTriangles) {
				int baseIndex = meshBuilder.Vertices.Count-1;
				
				if(i != 0) {
					meshBuilder.AddTriangle(baseIndex,baseIndex-segments-1,baseIndex-1);
				}
				if(i != segments) {
					meshBuilder.AddTriangle(baseIndex,baseIndex-segments,baseIndex-segments-1);
				}
			}
		}
	}
	
	void BuildCap(MeshBuilder meshBuilder, Vector3 offset, float radius, int segments) {
		BuildCap(meshBuilder, offset, radius, segments, true);
	}
	
	void BuildCap(MeshBuilder meshBuilder, Vector3 offset, float radius, int segments, bool facingUp) {
		meshBuilder.Vertices.Add(offset);
		meshBuilder.UVs.Add(new Vector2(0.5f,0.5f));
		meshBuilder.Normals.Add(facingUp ? Vector3.up : Vector3.down);
		for(int i = 0; i < segments; i++) {
			float theta = (Mathf.PI*2.0f*(float)i)/(float)segments;
			float x = offset.x + Mathf.Cos(theta)*radius;
			float z = offset.z + Mathf.Sin(theta)*radius;
			
			meshBuilder.UVs.Add(new Vector2(Mathf.Cos(theta),Mathf.Sin(theta)));
			meshBuilder.Normals.Add(facingUp ? Vector3.up : Vector3.down);
			meshBuilder.Vertices.Add(new Vector3(x,offset.y,z));
		}
		int baseIndex = meshBuilder.Vertices.Count-segments-1;
		if(facingUp) {
			meshBuilder.AddTriangle(baseIndex,baseIndex+1,baseIndex+segments);
			for(int i = 1; i < segments; i++) {
				meshBuilder.AddTriangle(baseIndex,baseIndex+i+1,baseIndex+i);
			}
		} else {
			meshBuilder.AddTriangle(baseIndex,baseIndex+segments,baseIndex+1);
			for(int i = 1; i < segments; i++) {
				meshBuilder.AddTriangle(baseIndex,baseIndex+i,baseIndex+i+1);
			}
		}
		
	}
	
	void BuildCylinder(MeshBuilder meshBuilder, Vector3 offset, float radius, float height, int width_segments, int height_segments, bool caps) {
		BuildRing(meshBuilder,offset,radius,width_segments,0.0f,false);
		for(int i = 1; i <= height_segments; i++) {
			Vector3 height_v = new Vector3(offset.x,offset.y+(height*((float)i/(float)height_segments)),offset.z); // Height vector
			BuildRing(meshBuilder,offset+height_v,radius,width_segments,(float)i/(float)height,true);
		}
		if(caps) {
			BuildCap(meshBuilder,offset,radius,width_segments,false);
			BuildCap(meshBuilder,new Vector3(offset.x,offset.y+height,offset.z),radius,width_segments);
		}
	}
	
	void BuildSphere(MeshBuilder meshBuilder, Vector3 offset, float radius, int width_segments, int height_segments) {
		BuildRing(meshBuilder,new Vector3(offset.x,offset.y-radius,offset.z),0.0f,width_segments,0.0f,false);
		for(int i = 1; i <= height_segments; i++) {
			float angle = (i*Mathf.PI)/height_segments;
			float ringRadius = Mathf.Sin(angle)*radius;
			Vector3 height_v = new Vector3(offset.x,offset.y-Mathf.Cos(angle)*radius,offset.z); // Height vector
			
			BuildRing(meshBuilder,offset+height_v,ringRadius,width_segments,(float)i/(float)height_segments,true);
		}
	}
}

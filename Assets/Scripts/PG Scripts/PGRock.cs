using UnityEngine;
using System.Collections;

public class PGRock : PGBase {
	
	public float m_radius = 1.0f;
	public int m_height_segments = 8;
	public int m_radial_segments = 8;
	
	public int m_deform_iterations = 1;
	
	public override Mesh BuildMesh (/*bool setupLODs*/)
	{
		MeshBuilder meshBuilder = new MeshBuilder();
		BuildSphere(meshBuilder, new Vector3(0.0f,0.0f,0.0f),m_radius,m_height_segments,m_radial_segments);
		// Deform sphere
		PerlinDeform(meshBuilder,m_radial_segments,m_height_segments,122.0f,0.5f,new Vector2(Random.Range(0.0f,100f),Random.Range(0.0f,100f)));
		/*
		for(int i = 0; i < m_deform_iterations; i++) {
			// Select random point a fixed radius from sphere
			Quaternion randRotation = Quaternion.Euler(Random.Range(0.0f,360.0f),Random.Range(0.0f,360.0f),Random.Range(0.0f,360.0f));
			Vector3 deformCentre = randRotation*(new Vector3(0.0f,m_radius,0.0f)*1.2f);
			SphericalDeform(meshBuilder,deformCentre,Random.Range(0.1f,0.4f),3.0f*m_radius);
		}
		*/
		Mesh mesh = meshBuilder.CreateMesh();
		mesh.RecalculateNormals();
		return mesh;
	}
	
	private void SphericalDeform(MeshBuilder meshBuilder, Vector3 deformCentre, float strength, float influenceRange) {
		// For each vertice, get distance and direction from deform centre and move vertice
		int vNum = meshBuilder.Vertices.Count;
		for(int i = 0; i < vNum; i++) {
			float distance = Vector3.Distance(meshBuilder.Vertices[i],deformCentre);
			float deformDist = 0; // Distance for vertex to move
			if(distance < influenceRange) {
				deformDist = (influenceRange-distance)*strength;
			}
			Vector3 direction = (meshBuilder.Vertices[i]-deformCentre).normalized;
			meshBuilder.Vertices[i] += direction*deformDist;
		}
	}
	
	private void PerlinDeform(MeshBuilder meshBuilder, int radialSegments, int heightSegments, float noiseRange, float noiseStrength, Vector2 noiseOffset) {
		for(int u = 0; u < heightSegments; u++) {
			for(int v = 0; v < radialSegments; v++) {
				float noise = Mathf.PerlinNoise(((float)u/(float)heightSegments)*noiseRange+noiseOffset.x,((float)v/(float)radialSegments)*noiseRange+noiseOffset.y)*noiseStrength;
				Vector3 direction = (meshBuilder.Vertices[u*radialSegments+v]-Vector3.up*m_radius).normalized;
				meshBuilder.Vertices[u*radialSegments+v] += direction*noise;
			}
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
	
	/*
	public override Mesh BuildLODMesh (int LODindex)
	{
		throw new System.NotImplementedException ();
	}
	
	public override void GenerateLODs (LODManager lodManager, MeshFilter filter)
	{
		throw new System.NotImplementedException ();
	}
	*/
}

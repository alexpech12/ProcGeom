using UnityEngine;
using System.Collections;

public class PGQuad : MonoBehaviour {
	
	public float m_length = 1;
	public float m_width = 1;
	
	/*
	Vector3[] vertices = new Vector3[4];
	Vector3[] normals = new Vector3[4];
	Vector2[] uv = new Vector2[4];
	
	// Use this for initialization
	void Start () {
		vertices[0] = new Vector3(0.0f, 0.0f, 0.0f);
		normals[0] = Vector3.up;
		uv[0] = new Vector2(0.0f,0.0f);
		
		vertices[1] = new Vector3(0.0f, 0.0f, m_length);
		normals[1] = Vector3.up;
		uv[1] = new Vector2(0.0f,1.0f);
		
		vertices[2] = new Vector3(m_width, 0.0f, m_length);
		normals[2] = Vector3.up;
		uv[2] = new Vector2(1.0f,1.0f);
		
		vertices[3] = new Vector3(m_width, 0.0f, 0.0f);
		normals[3] = Vector3.up;
		uv[3] = new Vector2(1.0f,0.0f);
		
		int[] indices = new int[6];
		
		indices[0] = 0;
		indices[1] = 1;
		indices[2] = 2;
		
		indices[3] = 0;
		indices[4] = 2;
		indices[5] = 3;
		
		Mesh mesh = new Mesh();
		
		mesh.vertices = vertices;
		mesh.normals = normals;
		mesh.uv = uv;
		mesh.triangles = indices;
		
		mesh.RecalculateBounds();
		
		MeshFilter filter = GetComponent<MeshFilter>();
		
		if (filter != null) {
			filter.sharedMesh = mesh;
		}
	}
	*/
	
	void Start () {
		MeshBuilder meshBuilder = new MeshBuilder();
		
		BuildQuad(meshBuilder, new Vector3(0.0f, 0.0f, 0.0f));
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	void BuildQuad(MeshBuilder meshBuilder, Vector3 offset) {
		
		meshBuilder.Vertices.Add(new Vector3(0.0f,0.0f,0.0f) + offset);
		meshBuilder.Normals.Add(Vector3.up);
		meshBuilder.UVs.Add(new Vector2(0.0f,0.0f));
		
		meshBuilder.Vertices.Add(new Vector3(0.0f,0.0f,m_length) + offset);
		meshBuilder.Normals.Add(Vector3.up);
		meshBuilder.UVs.Add(new Vector2(0.0f,1.0f));
		
		meshBuilder.Vertices.Add(new Vector3(m_width,0.0f,m_length) + offset);
		meshBuilder.Normals.Add(Vector3.up);
		meshBuilder.UVs.Add(new Vector2(1.0f,1.0f));
		
		meshBuilder.Vertices.Add(new Vector3(m_width,0.0f,0.0f) + offset);
		meshBuilder.Normals.Add(Vector3.up);
		meshBuilder.UVs.Add(new Vector2(1.0f,0.0f));
		
		meshBuilder.AddTriangle(0,1,2);
		meshBuilder.AddTriangle(0,2,3);
		
		MeshFilter filter = GetComponent<MeshFilter>();
		if (filter != null) {
			filter.sharedMesh = meshBuilder.CreateMesh();
		}
		
		int baseIndex = meshBuilder.Vertices.Count - 4;
		
		meshBuilder.AddTriangle(baseIndex, baseIndex+1, baseIndex+2);
		meshBuilder.AddTriangle(baseIndex, baseIndex+2, baseIndex+3);
	}
}

using UnityEngine;
using System.Collections;

public class PGPlane : MonoBehaviour {
	
	public float m_length = 10;
	public float m_width = 5;
	public int m_length_segments = 5;
	public int m_width_segments = 3;
	
	// Use this for initialization
	void Start () {
		MeshBuilder meshBuilder = new MeshBuilder();
		
		BuildPlane(meshBuilder, new Vector3(0.0f,0.0f,0.0f));
	}
	
	void BuildPlane(MeshBuilder meshBuilder, Vector3 offset) {
		bool buildTriangles = false;
		
		float seg_length = m_length / m_length_segments;
		float seg_width = m_width / m_width_segments;
		
		for(int row = 0; row <= m_width_segments; row++) {
			float v = (float)row/(float)m_width_segments;
			for (int col = 0; col <= m_length_segments; col++) {
				float u = (float)col/(float)m_length_segments;
				buildTriangles = (row!=0 && col!=0) ? true : false;
				
				BuildQuadForGrid(meshBuilder, new Vector3(row*seg_width,Random.Range(0,0.0f),col*seg_length)+offset,new Vector2(u,v),buildTriangles);
			}
		}
		MeshFilter filter = GetComponent<MeshFilter>();
		if (filter != null) {
			Mesh mesh = meshBuilder.CreateMesh();
			mesh.RecalculateNormals();
			filter.sharedMesh = mesh;
		}
	}
	
	void BuildQuadForGrid(MeshBuilder meshBuilder, Vector3 position, Vector2 uv, bool buildTriangles) {
		
		meshBuilder.Vertices.Add(position);
		meshBuilder.UVs.Add(uv);
		
		if(buildTriangles) {
			int baseIndex = meshBuilder.Vertices.Count-1;
			
			int index0 = baseIndex;
			int index1 = baseIndex - 1;
			int index2 = baseIndex-m_length_segments-1;
			int index3 = baseIndex-m_length_segments-2;
				
			meshBuilder.AddTriangle(index0,index1,index2);
			meshBuilder.AddTriangle(index1,index3,index2);
		}
	}
}

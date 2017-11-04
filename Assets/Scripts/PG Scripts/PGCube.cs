using UnityEngine;
using System.Collections;

public class PGCube : MonoBehaviour {
	
	public float height = 1;
	public float width = 1;
	public float depth = 1;
	
	// Use this for initialization
	void Start () {
		MeshBuilder meshBuilder = new MeshBuilder();
		
		BuildCube(meshBuilder, new Vector3(0.0f,0.0f,0.0f));
	}
	
	void BuildCube(MeshBuilder meshBuilder, Vector3 offset) {
		
		Vector3 upDir = Vector3.up * height;
		Vector3 rightDir = Vector3.right * width;
		Vector3 forwardDir = Vector3.forward * depth;
		
		Vector3 nearCorner = offset;
		Vector3 farCorner = offset+upDir+rightDir+forwardDir;
		
		BuildQuad(meshBuilder,nearCorner,rightDir,upDir);
		BuildQuad(meshBuilder,nearCorner,upDir,forwardDir);
		BuildQuad(meshBuilder,nearCorner,forwardDir,rightDir);
		
		BuildQuad(meshBuilder,farCorner,-forwardDir,-upDir);
		BuildQuad(meshBuilder,farCorner,-rightDir,-forwardDir);
		BuildQuad(meshBuilder,farCorner,-upDir,-rightDir);
		
		MeshFilter filter = GetComponent<MeshFilter>();
		if (filter != null) {
			filter.sharedMesh = meshBuilder.CreateMesh();
		}
	}
	
	void BuildQuad(MeshBuilder meshBuilder, Vector3 offset, Vector3 widthDir, Vector3 lengthDir) {
		Vector3 normal = Vector3.Cross(lengthDir,widthDir).normalized;
		
		meshBuilder.Vertices.Add(offset);
		meshBuilder.UVs.Add(new Vector2(0.0f,0.0f));
		meshBuilder.Normals.Add(normal);
		
		meshBuilder.Vertices.Add(offset+lengthDir);
		meshBuilder.UVs.Add(new Vector2(1.0f,0.0f));
		meshBuilder.Normals.Add(normal);
		
		meshBuilder.Vertices.Add(offset+lengthDir+widthDir);
		meshBuilder.UVs.Add(new Vector2(1.0f,1.0f));
		meshBuilder.Normals.Add(normal);
		
		meshBuilder.Vertices.Add(offset+widthDir);
		meshBuilder.UVs.Add(new Vector2(0.0f,1.0f));
		meshBuilder.Normals.Add(normal);
		
		int baseIndex = meshBuilder.Vertices.Count-4;
		
		meshBuilder.AddTriangle(baseIndex,baseIndex+1,baseIndex+2);
		meshBuilder.AddTriangle(baseIndex,baseIndex+2,baseIndex+3);
	}
}

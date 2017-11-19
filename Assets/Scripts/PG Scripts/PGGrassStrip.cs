using UnityEngine;
using System.Collections;

public class PGGrassStrip : PGBase {
	
	void Start() {
		CreateObject(/*true*/);
		CreateTexture();
	}
	
	/*
	public override Mesh BuildLODMesh (int LODindex)
	{
		return null;
	}
	*/
	/*
	public override void GenerateLODs (LODManager lodManager, MeshFilter filter)
	{
		lodManager.SetLODMesh(filter.sharedMesh,0);
		lodManager.SetLODDistance(50.0f,0);
	}
	*/
	
	public override Mesh BuildMesh (/*bool setupLODs*/)
	{
		MeshBuilder meshBuilder = new MeshBuilder();
		//BuildQuad(meshBuilder,Vector3.zero,new Vector2(0.0f,0.0f),false,2,new Vector3(1.0f,0.0f,0.0f),true);
		
		// Calculate random angle for grass to orient to
		float angle = Random.Range(0f,180f);
		float length = 50.0f;
		float offsetFromGround = -0.05f;
		float heightMag = 1.5f;
		Vector3 height = Vector3.up*heightMag; // Using Vector3 to assist with later addition
		int lengthSegments = 5;
		
		// Use terrain height for height offset
		Terrain t = Terrain.activeTerrain;
		Vector3 positionVector = new Vector3(Mathf.Cos(angle),0.0f,Mathf.Sin(angle));
		Vector3 normal = Vector3.Cross(positionVector,Vector3.up).normalized;
		
		for(int i = 0; i <= lengthSegments; i++) {
			bool buildTriangles = i!=0;
			float distFromOrigin = ((float)i/(float)lengthSegments)*length;
			Vector3 offset = positionVector*distFromOrigin;
			float u = (i*length)/((float)lengthSegments*height.y);
			//float u = (i*((float)lengthSegments/length))/height.y;
			
			// Adjust height based on height of terrain
			float heightOffset = t.SampleHeight(transform.position+offset)-transform.position.y+offsetFromGround;
			offset.y += heightOffset;
			
			BuildQuad(meshBuilder,offset,new Vector2(u,0.0f),false,4,normal,false);
			BuildQuad(meshBuilder,offset+height,new Vector2(u,1.0f),buildTriangles,4,normal,false);
			// Build backside
			BuildQuad(meshBuilder,offset,new Vector2(u,0.0f),false,4,normal,true);
			BuildQuad(meshBuilder,offset+height,new Vector2(u,1.0f),buildTriangles,4,normal,true);
		}
		
		Mesh mesh = meshBuilder.CreateMesh();
		return mesh;
	}
	
	private void BuildQuad(MeshBuilder meshBuilder, Vector3 position, Vector2 uv, bool buildTriangles, int vertsPerRow, Vector3 normal, bool backSide) {
		meshBuilder.Vertices.Add (position);
		meshBuilder.UVs.Add(uv);
		if(backSide) {
			meshBuilder.Normals.Add(-normal);
		} else {
			meshBuilder.Normals.Add(normal);
		}
		
		if(buildTriangles) {
			int baseIndex = meshBuilder.Vertices.Count-1;
			if(backSide) {
				meshBuilder.AddTriangle(baseIndex,baseIndex-1,baseIndex-vertsPerRow);
				meshBuilder.AddTriangle(baseIndex-vertsPerRow,baseIndex-1,baseIndex-vertsPerRow-1);
			} else {
				meshBuilder.AddTriangle(baseIndex,baseIndex-vertsPerRow,baseIndex-1);
				meshBuilder.AddTriangle(baseIndex-vertsPerRow,baseIndex-vertsPerRow-1,baseIndex-1);
			}
		}
	}
	
	private void CreateTexture() {
		//TextureBuilder texturer = new TextureBuilder();
		//texturer.ColorTexture(128,Color.green);
		//texturer.CutOutGrassShape();
		//texturer.AssignTexture(gameObject,Shader.Find("Transparent/Cutout/Diffuse"));
	}
}

using UnityEngine;
using System.Collections;

public class PGTreeLeaf : PGBase {
	
	public enum LeafType{
		Flat,
		Spherical
	};
	
	public override void GenerateLODs (LODManager lodManager, MeshFilter filter)
	{
		lodManager.SetLODMesh(filter.sharedMesh,0);
		lodManager.SetLODDistance(100.0f,0);
		//lodManager.SetLODMesh(BuildLODMesh(1),1);
		//lodManager.SetLODMesh(BuildLODMesh(2),2);
	}
	public override Mesh BuildMesh(bool setupLODs)
	{
		return BuildLeaves();
	}
	
	public override Mesh BuildLODMesh (int LODindex)
	{
		switch(LODindex) {
		case 1:
			return BuildLOD1Leaves();
			break;
		case 2:
			return BuildLOD2Leaves();
			break;
		default:
			Debug.LogError("Error in PGTreeLeaf.BuildLODMesh: Invalid LODindex!");
			break;
		}
		return null;
	}
	
	public Mesh BuildLeaves() {
		
		PGTreeBase p = transform.parent.GetComponent("PGTreeBase") as PGTreeBase;
		PGTreeTrunkSimple trunk = transform.parent.GetComponentInChildren<PGTreeTrunkSimple>();
		
		MeshBuilder meshBuilder = new MeshBuilder();
		
		for(int i = 0; i < trunk.Leaves.Count-1;i++) {
			for(int j = 0; j < p.m_leaves_per_branch; j++) {
				Vector3 offset = trunk.Leaves[i].position;
				Vector3 localOffset = new Vector3(0.0f,Mathf.Cos(p.m_stem_start_angle*Mathf.Deg2Rad),Mathf.Sin(p.m_stem_start_angle*Mathf.Deg2Rad))*p.m_stem_length;
				Quaternion rotation = trunk.Leaves[i].rotation*Quaternion.Euler(0.0f,(360*(float)j)/(float)p.m_leaves_per_branch,0.0f);
				BuildLeaf(meshBuilder,offset,localOffset,rotation,p.m_leaf_length,p.m_leaf_width,p.m_leaf_length_rand,p.m_leaf_width_rand,p.m_leaf_length_segments,p.m_leaf_width_segments,
					p.m_leaf_bend,p.m_leaf_bend_rand,p.m_leaf_curl,p.m_leaf_curl_rand,p.m_leaf_twist,p.m_leaf_twist_rand,true,p.m_leaf_variation);
				//BuildLeaf(meshBuilder,offset,localOffset,rotation,p.m_leaf_length,p.m_leaf_width,p.m_leaf_length_rand,p.m_leaf_width_rand,p.m_leaf_length_segments,p.m_leaf_width_segments,
				//	p.m_leaf_bend,p.m_leaf_bend_rand,p.m_leaf_curl,p.m_leaf_curl_rand,p.m_leaf_twist,p.m_leaf_twist_rand,true,p.m_leaf_variation);
			}
		}
		
		Mesh mesh = meshBuilder.CreateMesh();
		mesh.RecalculateNormals();
		return mesh;
	}
	
	public Mesh BuildLOD1Leaves() {
		PGTreeBase p = transform.parent.GetComponent("PGTreeBase") as PGTreeBase;
		PGTreeTrunkSimple trunk = transform.parent.GetComponentInChildren<PGTreeTrunkSimple>();
		
		MeshBuilder meshBuilder = new MeshBuilder();
		
		for(int i = 0; i < trunk.Leaves.Count-1;i++) {
			for(int j = 0; j < p.m_leaves_per_branch; j++) {
				Vector3 offset = trunk.Leaves[i].position;
				Vector3 localOffset = new Vector3(0.0f,Mathf.Cos(p.m_stem_start_angle*Mathf.Deg2Rad),Mathf.Sin(p.m_stem_start_angle*Mathf.Deg2Rad))*p.m_stem_length;
				Quaternion rotation = trunk.Leaves[i].rotation*Quaternion.Euler(0.0f,(360*(float)j)/(float)p.m_leaves_per_branch,0.0f);
				BuildLeafSimple(meshBuilder,offset,localOffset,rotation,p.m_leaf_length,p.m_leaf_width,true,p.m_leaf_variation);
			}
		}
		
		Mesh mesh = meshBuilder.CreateMesh();
		mesh.RecalculateNormals();
		return mesh;
	}
	
	public Mesh BuildLOD2Leaves() {
		PGTreeBase p = transform.parent.GetComponent("PGTreeBase") as PGTreeBase;
		MeshBuilder meshBuilder = new MeshBuilder();
		
		Vector3 position = Vector3.zero;
		Vector3 offsetXZ = new Vector3(5.0f,0.0f,0.0f);
		Vector3 offsetY = new Vector3(0.0f,5.0f,0.0f);
		BuildQuad(meshBuilder,position-offsetXZ,new Vector2(0.0f,0.0f),false,2,Vector3.zero,true);
		BuildQuad(meshBuilder,position+offsetXZ,new Vector2(5.0f,0.0f),false,2,Vector3.zero,true);
		BuildQuad(meshBuilder,position-offsetXZ+offsetY,new Vector2(0.0f,5.0f),false,2,Vector3.zero,true);
		BuildQuad(meshBuilder,position+offsetXZ+offsetY,new Vector2(5.0f,5.0f),true,2,Vector3.zero,true);
		
		Mesh mesh = meshBuilder.CreateMesh();
		mesh.RecalculateNormals();
		return mesh;
		
	}
	
	private void BuildLeaf(MeshBuilder meshBuilder,Vector3 position,Vector3 offset,Quaternion rotation,float length,float width,float lengthRand,float widthRand,
						   int lengthSegments,int widthSegments,float bend,float bendRand,float curl,float curlRand,float twist,float twistRand,bool backSide,bool leafVariation) {
		
		int quadX = 0;
		int quadY = 0;
		if(leafVariation) {
			quadX = (int)Mathf.Round(Random.Range(0.0f,1.0f));
			quadY = (int)Mathf.Round(Random.Range(0.0f,1.0f));
		}
		
		float r_length = length+Random.Range(-lengthRand,lengthRand);
		float r_width = width+Random.Range(-widthRand,widthRand);
		float r_bend = bend+Random.Range(-bendRand,bendRand);
		float r_curl = curl+Random.Range(-curlRand,curlRand);
		float r_twist = twist+Random.Range(-twistRand,twistRand);
		
		int repeat = backSide ? 1 : 0;
		for(int b = 0; b <= repeat; b++) {
			for(int i = 0; i <= lengthSegments; i++) {
				for(int j = 0; j <= widthSegments; j++) {
					bool buildTriangles = true;
					if(j==0 || i==0) {
						buildTriangles = false;
					}
					
					float widthRatio = (float)j/(float)widthSegments;
					float lengthRatio = (float)i/(float)lengthSegments;
					Vector2 uv = Vector2.zero;
					if(leafVariation) {
						uv = new Vector2(0.5f*(widthRatio+(float)quadX),0.5f*(lengthRatio + (float)quadY));
					} else {
						uv = new Vector2(widthRatio,lengthRatio);
					}
					Vector3 normal = rotation*(backSide&&b==1 ? Vector3.down : Vector3.up);
					float x = (widthRatio-0.5f)*r_width;
					float y = 0.0f;
					float z = lengthRatio*r_length;
					
					Quaternion curlRotation = Quaternion.Euler(0.0f,0.0f,-r_curl*(widthRatio-0.5f));
					Quaternion bendRotation = Quaternion.Euler(r_bend*(lengthRatio),0.0f,0.0f);
					Quaternion twistRotation = Quaternion.Euler(0.0f,0.0f,r_twist*lengthRatio);
						
					Vector3 finalPosition = rotation*bendRotation*twistRotation*curlRotation*(new Vector3(x,y,z)+offset)+position;
					
					BuildQuad(meshBuilder,finalPosition,uv,buildTriangles,widthSegments+1,normal,backSide&&b==1);
				}
			}
		}
	}
	
	private void BuildLeafSimple(MeshBuilder meshBuilder,Vector3 position,Vector3 offset,Quaternion rotation,float length,float width,bool backSide,bool leafVariation) {
		int quadX = 0;
		int quadY = 0;
		if(leafVariation) {
			quadX = (int)Mathf.Round(Random.Range(0.0f,1.0f));
			quadY = (int)Mathf.Round(Random.Range(0.0f,1.0f));
		}
		
		int repeat = backSide ? 1 : 0;
		for(int b = 0; b <= repeat; b++) {
			for(int i = 0; i <= 1; i++) {
				for(int j = 0; j <= 1; j++) {
					bool buildTriangles = true;
					if(j==0 || i==0) {
						buildTriangles = false;
					}
					Vector2 uv = Vector2.zero;
					if(leafVariation) {
						uv = new Vector2(0.5f*((float)j+(float)quadX),0.5f*((float)i + (float)quadY));
					} else {
						uv = new Vector2(j,i);
					}
					Vector3 normal = rotation*(backSide&&b==1 ? Vector3.down : Vector3.up);
					float x = ((float)j-0.5f)*width;
					float y = 0.0f;
					float z = (float)i*length;
						
					Vector3 finalPosition = rotation*(new Vector3(x,y,z)+offset)+position;
					
					BuildQuad(meshBuilder,finalPosition,uv,buildTriangles,2,normal,backSide&&b==1);
				}
			}
		}
	}
	
	private void BuildQuad(MeshBuilder meshBuilder, Vector3 position, Vector2 uv, bool buildTriangles, int vertsPerRow, Vector3 normal, bool backSide) {
		meshBuilder.Vertices.Add (position);
		meshBuilder.UVs.Add(uv);
		meshBuilder.Normals.Add(normal);
		
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
}

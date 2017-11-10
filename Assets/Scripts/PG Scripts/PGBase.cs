using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public abstract class PGBase : MonoBehaviour {

	public int LODRandIndex = 0;
	public List<float> LODRandNums = new List<float>();
	
	public abstract Mesh BuildMesh(bool setupLODs);
	
	public void CreateObject() {
		CreateObject(false);
	}
	
	public void CreateObject(bool generateLODs)
	{	
		//Build the mesh:
		Mesh mesh = BuildMesh(generateLODs);

		//Look for a MeshFilter component attached to this GameObject:
		MeshFilter filter = gameObject.EnsureComponent<MeshFilter>();
		
		//Destroy(filter.sharedMesh);

		filter.sharedMesh = mesh;
		
		MeshRenderer renderer = GetComponent<MeshRenderer>();
		
		if(renderer == null) {
			renderer = gameObject.AddComponent<MeshRenderer>() as MeshRenderer;
			renderer.material = Resources.Load("DefaultMat") as Material;
		}
		if(generateLODs) SetupLODs();
	}
	
	private void SetupLODs() {
		LODManager lodManager = gameObject.EnsureComponent<LODManager>();
		MeshFilter filter = GetComponent<MeshFilter>();
		GenerateLODs(lodManager, filter);
	}
	
	public abstract void GenerateLODs(LODManager lodManager, MeshFilter filter);
	
	public abstract Mesh BuildLODMesh(int LODindex);
}

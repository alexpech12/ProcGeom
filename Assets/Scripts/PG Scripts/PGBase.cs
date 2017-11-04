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
		MeshFilter filter = GetComponent<MeshFilter>();
		
		//If the MeshFilter exists, attach the new mesh to it.
		//Assuming the GameObject also has a renderer attached, our new mesh will now be visible in the scene.
		if (filter == null)
		{
			filter = gameObject.AddComponent<MeshFilter>() as MeshFilter;
		}
		filter.sharedMesh = mesh;
		
		MeshRenderer renderer = GetComponent<MeshRenderer>();
		
		if(renderer == null) {
			renderer = gameObject.AddComponent<MeshRenderer>() as MeshRenderer;
			renderer.material = Resources.Load("DefaultMat") as Material;
		}
		if(generateLODs) SetupLODs();
	}
	
	private void SetupLODs() {
		LODManager lodManager = gameObject.AddComponent<LODManager>();
		MeshFilter filter = GetComponent<MeshFilter>();
		GenerateLODs(lodManager, filter);
	}
	
	public abstract void GenerateLODs(LODManager lodManager, MeshFilter filter);
	
	public abstract Mesh BuildLODMesh(int LODindex);
}

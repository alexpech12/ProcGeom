using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public abstract class PGBase : MonoBehaviour {
	
	public abstract Mesh BuildMesh();
	
	//public void CreateObject() {
	//	CreateObject(false);
	//}
	
	public void CreateObject()
	{	
		//Build the mesh:
		Mesh mesh = BuildMesh();

		//Look for a MeshFilter component attached to this GameObject:
		MeshFilter filter = gameObject.EnsureComponent<MeshFilter>();

		//Update mesh
		filter.sharedMesh = mesh;
		
		MeshRenderer renderer = gameObject.EnsureComponent<MeshRenderer>();
		renderer.material = Resources.Load("DefaultMat") as Material;
	}
}

using UnityEngine;
using System.Collections;

public class BodySegmentTest : MonoBehaviour {
	
	BodySegmentMeshData data;
	BodySegmentMesh script;
	MeshFilter filter;
	MeshRenderer renderer;
	
	public bool useSimplified = false;
	[Range(0.1f,10.0f)]
	public float length = 1.0f;
	[Range(0.1f,2.0f)]
	public float radiusA = 0.5f;
	[Range(0.1f,2.0f)]
	public float radiusB = 0.3f;
	[Range(0.0f,1.0f)]
	public float roundnessA = 1.0f;
	[Range(0.0f,1.0f)]
	public float roundnessB = 1.0f;
	[Range(-50.0f,50.0f)]
	public float angleA = 0.0f;
	[Range(-50.0f,50.0f)]
	public float angleB = 0.0f;
	[Range(3,20)]
	public int heightSegments = 8;
	[Range(3,20)]
	public int radialSegments = 8;
	[Range(0.0f,1.0f)]
	public float simple_roundness = 1.0f;
	[Range(3,40)]
	public int simple_segments = 8;
	
	// Use this for initialization
	void Start () {
		data = new BodySegmentMeshData(length,radiusA,radiusB,roundnessA,roundnessB,angleA,angleB,heightSegments,radialSegments);
		script = gameObject.AddComponent<BodySegmentMesh>();
		filter = gameObject.AddComponent<MeshFilter>();
		renderer = gameObject.AddComponent<MeshRenderer>();
		renderer.material = new Material(Shader.Find("Diffuse"));
		filter.sharedMesh = script.BuildMesh(data);
	}
	
	// Update is called once per frame
	void Update () {
		filter.sharedMesh.Clear();
		if(useSimplified) {
			data = new BodySegmentMeshData(length,radiusA,radiusB,simple_roundness,simple_segments);
		} else {
			data = new BodySegmentMeshData(length,radiusA,radiusB,roundnessA,roundnessB,angleA,angleB,heightSegments,radialSegments);
		}
		filter.sharedMesh = script.BuildMesh(data);
	}
}

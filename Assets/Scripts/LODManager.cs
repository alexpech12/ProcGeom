using UnityEngine;
using System.Collections;

public class LODManager : MonoBehaviour {
	
	private Transform Player;
	
	private Mesh LOD0 = null;
	private Mesh LOD1 = null;
	private Mesh LOD2 = null;
	private Mesh LOD3 = null;
	
	public float LOD0Distance = 50.0f;
	public float LOD1Distance = 100.0f;
	public float LOD2Distance = 200.0f;
	
	private int currentLOD = 2;
	private MeshFilter meshFilter;
	
	
	public float updateInterval = 2.0f;
	private float intervalCountdown = 0.0f;
	
	void Start() {
		intervalCountdown = updateInterval + Random.Range(0.0f,updateInterval);
		meshFilter = GetComponent<MeshFilter>();
		// Default to lowest detail
		meshFilter.sharedMesh = LOD3;
		Player = GameObject.FindGameObjectWithTag("MainCamera").transform;
	}
	
	void Update() {
		if(intervalCountdown <= 0.0f) {
			// Check distance
			float distance = Vector3.Distance(Player.transform.position,transform.position);
			if(distance < LOD0Distance) {
				meshFilter.sharedMesh = LOD0;
				//Debug.Log("Switching to LOD0...");
				currentLOD = 0;
			} else if(distance < LOD1Distance) {
				meshFilter.sharedMesh = LOD1;
				//Debug.Log("Switching to LOD1...");
				currentLOD = 1;
			} else if(distance < LOD2Distance) {
				meshFilter.sharedMesh = LOD2;
				//Debug.Log("Switching to LOD2...");
				currentLOD = 2;
			} else {
				meshFilter.sharedMesh = LOD3;
				//Debug.Log("Switching to LOD3...");
				currentLOD = 2;
			}
			intervalCountdown = updateInterval;
		} else {
			intervalCountdown -= Time.deltaTime;
		}
	}
	
	public void SetLODMesh(Mesh mesh, int LODindex) {
		switch(LODindex) {
		case 0:
			LOD0 = mesh;
			break;
		case 1:
			LOD1 = mesh;
			break;
		case 2:
			LOD2 = mesh;
			break;
		default:
			Debug.LogError("Error in LODManager: LOD index does not exist!");
			break;
		}
	}
	
	public void SetLODDistance(float distance, int LODindex) {
		switch(LODindex) {
		case 0:
			LOD0Distance = distance;
			break;
		case 1:
			LOD1Distance = distance;
			break;
		case 2:
			LOD2Distance = distance;
			break;
		default:
			Debug.LogError("Error in LODManager: LOD index does not exist!");
			break;
		}
	}
	
	public void SetPlayer(Transform player) {
		Player = player;
	}
}

using UnityEngine;
using System.Collections;

public class BodyBase : MonoBehaviour {
	
	private Transform[] m_legBase; public Transform[] legBase { get { return m_legBase; } }
	
	private Material defaultMat; 
	private CreatureSpecies species;
	
	void Awake() {
		m_legBase = new Transform[4];
		defaultMat = new Material(Shader.Find("Diffuse"));
		species = transform.parent.GetComponent<CreatureBase>().species;
	}
	
	GameObject root, rootPivot, pelvis, waist, torso;
	GameObject pelvisMesh, waistMesh, torsoMesh;
	
	
	GameObject /*root, */ hipPivot, hip, thigh, calf, foot, toe;
	GameObject thighMesh, calfMesh, footMesh, toeMesh;
	
	// Use this for initialization
	public void Init () {
		
		// Create all GameObjects and meshes:
		root = new GameObject("root");
		rootPivot = new GameObject("rootPivot");
		pelvis = new GameObject("pelvis");
		waist = new GameObject("waist");
		torso = new GameObject("torso");
		pelvisMesh = NewBodySegment(species.spineData.pelvisData, "pelvisMesh");
		waistMesh = NewBodySegment(species.spineData.waistData, "waistMesh");
		torsoMesh = NewBodySegment(species.spineData.torsoData, "torsoMesh");
		
		//neckMarker = GameObject.CreatePrimitive(PrimitiveType.Sphere);
		//neckMarker.transform.parent = root.transform;
		
		// Set up object heirarchy
		root.transform.parent = transform;
		rootPivot.transform.parent = root.transform;
		pelvis.transform.parent = rootPivot.transform;
		waist.transform.parent = pelvis.transform;
		torso.transform.parent = waist.transform;
		pelvisMesh.transform.parent = pelvis.transform;
		waistMesh.transform.parent = waist.transform;
		torsoMesh.transform.parent = torso.transform;
		
		// Set up mesh position offsets
		//pelvisPrimitive.transform.localPosition = new Vector3(0.0f,pelvisLength/2,0.0f);
		//waistPrimitive.transform.localPosition = new Vector3(0.0f,waistLength/2,0.0f);
		//torsoPrimitive.transform.localPosition = new Vector3(0.0f,torsoLength/2,0.0f);
		
		// Set up joint position offsets
		// This is only necessary for objects that won't have their position animated
		pelvis.transform.localPosition = Vector3.zero;
		waist.transform.localPosition = new Vector3(0.0f,species.spineData.pelvisData.length,0.0f);
		torso.transform.localPosition = new Vector3(0.0f,species.spineData.waistData.length,0.0f);
		
		// The code below can be used to scale the mesh objects
		/* 
		Vector3 pelvisScale = new Vector3(0.5f,pelvisLength,0.5f);
		Vector3 waistScale = new Vector3(0.5f,waistLength,0.5f);
		Vector3 torsoScale = new Vector3(0.5f,torsoLength,0.5f);
		pelvisPrimitive.transform.localScale = pelvisScale;
		waistPrimitive.transform.localScale = waistScale;
		torsoPrimitive.transform.localScale = torsoScale;
		*/
		
		BuildLegPair(transform,root.transform,species.backLegData,species.spineData.pelvisData.radiusA,"B");
		BuildLegPair(transform,torso.transform,species.frontLegData,species.spineData.torsoData.radiusB,"F");
		
		// Build tail
		GameObject tailSegment;
		Transform tailParent = rootPivot.transform;
		float tailRadius = species.spineData.pelvisData.radiusA;
		for(int i = 0; i < species.tailSegments; i++) {
			tailSegment = new GameObject("tail_" + i);
			if(i==0) {
				SnapToParent(tailSegment.transform,tailParent,Vector3.zero,Quaternion.Euler(0.0f,0.0f,180.0f));
			} else {
				SnapToParent(tailSegment.transform,tailParent,new Vector3(0.0f,species.tailLength/species.tailSegments,0.0f));
			}
			GameObject tailMesh = NewBodySegment(new BodySegmentMeshData(species.tailLength/species.tailSegments,tailRadius,tailRadius*species.tailTaper,0.5f,12),"tail_" + i + "_Mesh");
			SnapToParent(tailMesh.transform,tailSegment.transform);
			tailRadius = tailRadius*species.tailTaper;
			tailParent = tailSegment.transform;
			/*
			GameObject tailBase = new GameObject("tailBase");
			SnapToParent(tailBase.transform,rootPivot.transform,Vector3.zero,Quaternion.Euler(0.0f,0.0f,180.0f));
			GameObject tailBaseMesh = new GameObject("tailBaseMesh");
			AttachBodyPart(tailBaseMesh.transform,new BodySegmentMeshData(1.0f,0.3f,0.2f,1.0f,12));
			*/
			//GameObject tailBaseMesh = NewBodySegment(new BodySegmentMeshData(1.0f,0.3f,0.2f,1.0f,12),"tailMesh");
			//SnapToParent(tailBaseMesh.transform,tailBase.transform);
		}
	}
	
	void BuildLegPair(Transform bodyParent, Transform hipParent, LegData legData, float hipWidth, string prefix) {
		BuildLeg(bodyParent,hipParent,legData,hipWidth,prefix+"L_");
		BuildLeg(bodyParent,hipParent,legData,-hipWidth,prefix+"R_");
	}
	
	void BuildLeg(Transform bodyParent, Transform hipParent, LegData legData, float hipWidth, string prefix) {
		GameObject hipPivot = new GameObject(prefix+"hipPivot");
		GameObject hip = new GameObject(prefix+"hip");
		GameObject thigh = new GameObject("thigh");
		GameObject calf = new GameObject("calf");
		GameObject foot = new GameObject("foot");
		GameObject toe = new GameObject("toe");
		GameObject thighMesh = NewBodySegment(legData.thighData, "thighMesh");
		GameObject calfMesh = NewBodySegment(legData.calfData, "calfMesh");
		GameObject footMesh = NewBodySegment(legData.footData, "footMesh");
		GameObject toeMesh = NewBodySegment(legData.toeData, "toeMesh");
		
		// Set up object heirarchy here
		hipPivot.transform.parent = hipParent.transform;
		hip.transform.parent = bodyParent;
		thigh.transform.parent = hip.transform;
		calf.transform.parent = thigh.transform;
		foot.transform.parent = calf.transform;
		toe.transform.parent = foot.transform;
		thighMesh.transform.parent = thigh.transform;
		calfMesh.transform.parent = calf.transform;
		footMesh.transform.parent = foot.transform;
		toeMesh.transform.parent = toe.transform;
		
		// Set up joint position offsets
		hipPivot.transform.localPosition = new Vector3(0.0f,0.0f,hipWidth);
		// This one is not actually necessary as it is animated to always snap to hipPivot
		hip.transform.localPosition = hipPivot.transform.position;
		calf.transform.localPosition = new Vector3(0.0f,legData.thighData.length,0.0f);
		foot.transform.localPosition = new Vector3(0.0f,legData.calfData.length,0.0f);
		toe.transform.localPosition = new Vector3(0.0f,legData.footData.length,0.0f);
	}
	
	// Function: BuildBody
	// Inputs: 4 points used to define curve of spine, taken relative to base of creature (ground level)
	void BuildBody() {
		
		transform.Rotate(0.0f,0.0f,-90.0f+species.spineElevation);
		transform.localPosition = new Vector3(-species.legSeparation/2.0f,species.backLegData.legLength,0.0f);
		
		GameObject rootPivot = new GameObject("rootPivot");
		SnapToParent(rootPivot.transform,transform);
		AttachBodyPart(rootPivot.transform,species.spineData.pelvisData);
		GameObject waist = new GameObject("waist");
		SnapToParent(waist.transform,rootPivot.transform,new Vector3(0.0f,species.spineData.pelvisData.length,0.0f));
		AttachBodyPart(waist.transform,species.spineData.waistData);
		GameObject torso = new GameObject("torso");
		SnapToParent(torso.transform,waist.transform,new Vector3(0.0f,species.spineData.waistData.length,0.0f));
		AttachBodyPart(torso.transform,species.spineData.torsoData);
		GameObject neck = new GameObject("neck");
		SnapToParent(neck.transform,torso.transform,new Vector3(0.0f,species.spineData.torsoData.length,0.0f));
		// Attach neck here
		
		
	}
	/*
	GameObject BuildLeg(string objectName, Transform parentHipNode, LegData data) {return BuildLeg(objectName, parentHipNode, Vector3.zero,data);}
		
	GameObject BuildLeg(string objectName, Transform parentHipNode, Vector3 offset, LegData data) {
		
		GameObject newLeg = new GameObject(objectName);
		newLeg.transform.parent = gameObject.transform;
		newLeg.transform.localPosition = Vector3.zero;
		
		LegScript newLegScript = newLeg.AddComponent<LegScript>();
		newLegScript.InitialiseLeg(offset,data);
		
		return newLeg;
	}
	*/
	GameObject NewBodySegment(BodySegmentMeshData data, string name) {
		GameObject newObject = new GameObject(name);
		BodySegmentMesh meshScript = newObject.AddComponent<BodySegmentMesh>();
		MeshFilter filter = newObject.AddComponent<MeshFilter>();
		MeshRenderer renderer = newObject.AddComponent<MeshRenderer>();
		renderer.material = defaultMat;
		filter.sharedMesh = meshScript.BuildMesh(data);
		return newObject;
	}
	
	void AttachBodyPart(Transform parent, BodySegmentMeshData data) {
		MeshFilter filter = parent.GetComponent<MeshFilter>();
		MeshRenderer renderer = parent.GetComponent<MeshRenderer>();
		BodySegmentMesh meshScript = parent.GetComponent<BodySegmentMesh>();
		if(filter == null) {filter = parent.gameObject.AddComponent<MeshFilter>();}
		if(renderer == null) {renderer = parent.gameObject.AddComponent<MeshRenderer>();}
		if(meshScript == null) {meshScript = parent.gameObject.AddComponent<BodySegmentMesh>();}
		filter.sharedMesh = meshScript.BuildMesh(data);
		renderer.material = defaultMat;
	}
	
	//GameObject AttachNewBodyPart(string name, Vector3 offset, Quaternion rotation, float length, float upperWidth, float lowerWidth,
	//							 float roundnessA, float roundnessB, float angleA, float angleB, int heightSegments, int radialSegments) {
	public GameObject AttachNewBodyPart(string name, Vector3 offset, Quaternion rotation, BodySegmentMeshData data) {
		GameObject newPart = NewChild(name);
		newPart.transform.localPosition = offset;
		newPart.transform.rotation = rotation;
		MeshFilter filter = newPart.AddComponent<MeshFilter>();
		MeshRenderer renderer = newPart.AddComponent<MeshRenderer>();
		BodySegmentMesh meshScript = newPart.AddComponent<BodySegmentMesh>();
		filter.sharedMesh = meshScript.BuildMesh(data);
		renderer.material = new Material(Shader.Find("Diffuse"));
		return newPart;
	}
	
	GameObject NewChild(string name) {
		GameObject childObject = new GameObject(name);
		childObject.transform.parent = transform;
		childObject.transform.localPosition = Vector3.zero;
		return childObject;
	}
	
	void SnapToParent(Transform child, Transform parent) {
		SnapToParent(child, parent, Vector3.zero, Quaternion.identity);
	}
	void SnapToParent(Transform child, Transform parent, Vector3 positionOffset) {
		SnapToParent(child, parent, positionOffset, Quaternion.identity);
	}
	void SnapToParent(Transform child, Transform parent, Vector3 positionOffset, Quaternion rotationOffset) {
		child.parent = parent;
		child.localPosition = positionOffset;
		child.localRotation = rotationOffset;
	}
}

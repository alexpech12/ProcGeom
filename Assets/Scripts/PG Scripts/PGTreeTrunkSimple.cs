﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PGTreeTrunkSimple : PGBase {
	
	public enum TrunkCurveType{
		Circular,
		Exponential,
		ExpoLinear
	};
	
	public struct LeafConstruct{
		public Vector3 position;
		public Quaternion rotation;
		public float stemLength;
		public float stemBend;
		public LeafConstruct(Vector3 p, Quaternion r,float sl,float sb)
		{
			position = p;
			rotation = r;
			stemLength = sl;
			stemBend = sb;
		}
	}
	
	public List<LeafConstruct> Leaves { get { return m_Leaves; } }
	private List<LeafConstruct> m_Leaves = new List<LeafConstruct>();
	
	
	public override void GenerateLODs (LODManager lodManager, MeshFilter filter)
	{
		lodManager.SetLODMesh(filter.sharedMesh,0);
		lodManager.SetLODMesh(BuildLODMesh(1),1);
		lodManager.SetLODMesh(BuildLODMesh(2),2);
		
		// Clear LOD variable list.
		LODRandNums.Clear();
	}
	
	public override Mesh BuildMesh(bool setupLOD)
	{
		return BuildTrunk(setupLOD);
	}
	
	public override Mesh BuildLODMesh (int LODindex)
	{
		return BuildLODTrunk(LODindex);
	}
	
	public Mesh BuildTrunk() {
		return BuildTrunk(false);
	}
	
	private Mesh BuildTrunk(bool setupLOD) {
		
		// Set variables from parent
		PGTreeBase p = transform.parent.GetComponent("PGTreeBase") as PGTreeBase;

		// Ensure leaf list is empty
		m_Leaves.Clear();
		
		MeshBuilder meshBuilder = new MeshBuilder();
		
		Vector3 offset = new Vector3(0.0f,0.0f,0.0f);
		
		int f1 = 0;
		int f2 = 0;
		float angle_offset1 = 0.0f;
		float angle_offset2 = 0.0f;
		
		// Generate 2 random harmonic frequencies to blend.
		f1 = Random.Range(1,5)*2;
		f2 = Random.Range(1,3)*2+1;
		
		angle_offset1 = Random.Range(0.0f,Mathf.PI*2);
		angle_offset2 = Random.Range(0.0f,Mathf.PI*2);
		
		Vector3 ring_centre = offset;
		Quaternion rotation = Quaternion.identity;
		Vector3 angle_rand;
		angle_rand = new Vector3(Random.Range(-p.m_max_bend,p.m_max_bend),Random.Range(-p.m_max_bend,p.m_max_bend),Random.Range(-p.m_max_bend,p.m_max_bend));
		LODRandNums.Add(angle_rand.x);
		LODRandNums.Add(angle_rand.y);
		LODRandNums.Add(angle_rand.z);
		
		BuildCurve(meshBuilder,offset,p.m_start_radius,p.m_radial_segments,0.0f,f1,f2,angle_offset1,angle_offset2,Quaternion.identity,0.0f,p.m_start_irregularity,false);
		for(int i = 1; i <= p.m_height_segments; i++) {
			float ring_height = ((float)i/(float)p.m_height_segments)*p.m_height;
			float radius = 1.0f;
			
			if(p.m_trunk_curve_type_s == TrunkCurveType.Circular) {
				radius = p.m_circ_trunk_bulge*Mathf.Sin((Mathf.PI*p.m_circ_trunk_bulge_freq*ring_height)/p.m_height+p.m_circ_trunk_bulge_offset)+(p.m_end_radius-p.m_start_radius)*ring_height/p.m_height + p.m_start_radius;
			} else if(p.m_trunk_curve_type_s == TrunkCurveType.Exponential || p.m_trunk_curve_type_s == TrunkCurveType.ExpoLinear) {
				bool isExpoLin = p.m_trunk_curve_type_s == TrunkCurveType.ExpoLinear;
				if(isExpoLin) {
					radius = ExpoLinInterp(p.m_start_radius,p.m_exp_mid_radius,p.m_end_radius,p.m_height,ring_height,p.m_expolinear_blend);
				} else {
					radius = ExpoLinInterp(p.m_start_radius,p.m_exp_mid_radius,p.m_end_radius,p.m_height,ring_height);
				}
			}
			
			float irregularity = p.m_start_irregularity*Mathf.Exp(-ring_height*p.m_irregularity_falloff/p.m_height);
			float twist_angle = (ring_height*p.m_twist_angle)/p.m_height;
			
			float hr = (ring_height/p.m_height);
			rotation = Quaternion.Euler(angle_rand.x*hr,angle_rand.y*hr,angle_rand.z*hr);
			Vector3 new_ring_offset = new Vector3(0.0f,p.m_height/(float)p.m_height_segments,0.0f);
			ring_centre += rotation * new_ring_offset;
			
			float v = ring_height/(2*Mathf.PI*radius);
			
			BuildCurve(meshBuilder,ring_centre,radius,p.m_radial_segments,v,f1,f2,angle_offset1,angle_offset2,rotation,twist_angle,irregularity,true);
			
			if(i==p.m_height_segments) {
				// Add twist angle to rotation for complete angle reference
				Quaternion rotationRef = rotation*Quaternion.Euler(0.0f,-p.m_twist_angle,0.0f);
				LODRandNums.Add(radius);
				BuildFork(meshBuilder,ring_centre,radius,rotationRef,p.m_branch_segments,p.m_radial_segments,new Vector2(p.m_branch_min_fork_angle,p.m_branch_max_fork_angle),
						  p.m_branch_max_bend,p.m_branch_length,p.m_branch_length_randomness,p.m_branch_twist_angle,p.m_branch_twist_randomness);
				break;
			}
		}
		//Debug.Log("Number of Leaves: " + m_Leaves.Count);
		Mesh mesh = meshBuilder.CreateMesh();
		//Debug.Log("Length of LODRandNums = " + LODRandNums.Count);
		mesh.RecalculateNormals();
		return mesh;
	}
	
	private Mesh BuildLODTrunk(int LODindex) {
		// Set variables from parent
		PGTreeBase p = transform.parent.GetComponent("PGTreeBase") as PGTreeBase;
		
		MeshBuilder meshBuilder = new MeshBuilder();
		
		// Set up LOD variables
		int height_segments = p.m_height_segments;
		int radial_segments = p.m_radial_segments;
		int branch_segments = p.m_branch_segments;
		switch (LODindex) {
		case 1:
			height_segments = p.m_LOD1_height_segments;
			radial_segments = p.m_LOD1_radial_segments;
			branch_segments = p.m_LOD1_branch_segments;
			break;
		case 2:
			height_segments = p.m_LOD2_height_segments;
			radial_segments = p.m_LOD2_radial_segments;
			branch_segments = p.m_LOD2_branch_segments;
			break;
		default:
			Debug.LogError("Error in BuildLODTrunk(): Invalid LODindex!");
			break;
		}
		
		float radius = p.m_start_radius;
		float height = p.m_height;
		
		Vector3 offset = new Vector3(0.0f,0.0f,0.0f);
		
		Vector3 ring_centre = offset;
		Quaternion rotation = Quaternion.identity;
		Vector3 angle_rand = Vector3.zero;
		angle_rand.x = NextLODRand();
		angle_rand.y = NextLODRand();
		angle_rand.z = NextLODRand();
		BuildCurveSimple(meshBuilder,offset,radius,radial_segments,0.0f,Quaternion.identity,0.0f,false);
		
		float end_radius = NextLODRand();
		
		for(int i = 1; i <= height_segments; i++) {
			float ring_height = ((float)i/(float)height_segments)*height;
			
			float twist_angle = (ring_height*p.m_twist_angle)/height;
			
			float hr = (ring_height/height);
			rotation = Quaternion.Euler(angle_rand.x*hr,angle_rand.y*hr,angle_rand.z*hr);
			Vector3 new_ring_offset = new Vector3(0.0f,height/(float)height_segments,0.0f);
			ring_centre += rotation * new_ring_offset;
			
			radius = p.m_start_radius - ((p.m_start_radius-end_radius)/height)*ring_height;
			
			float v = ring_height/(2*Mathf.PI*radius);
			BuildCurveSimple(meshBuilder,ring_centre,radius,radial_segments,v,rotation,twist_angle,true);
			//BuildCurve(meshBuilder,ring_centre,radius,radial_segments,v,rotation,twist_angle,irregularity,true);
			
			if(i==height_segments) {
				// Add twist angle to rotation for complete angle reference
				Quaternion rotationRef = rotation*Quaternion.Euler(0.0f,-p.m_twist_angle,0.0f);
				radius = end_radius;
				BuildLODFork(LODindex,meshBuilder,ring_centre,radius,rotationRef,branch_segments,radial_segments);
				break;
			}
		}
		//Debug.Log("Number of Leaves: " + m_Leaves.Count);
		Mesh mesh = meshBuilder.CreateMesh();
		LODRandIndex = 0;
		mesh.RecalculateNormals();
		return mesh;
	}
	
	// Creates a fork and builds 2 branches off it
	// -> radiusRef: radius of trunk
	// -> angleRef: angle of the final trunk ring
	void BuildFork(MeshBuilder meshBuilder,Vector3 offset,float radiusRef,Quaternion angleRef,int lengthSegments,int radialSegments,Vector2 forkAngleBounds,float maxBendAngle,float branchLength,float lengthRandomness,float branchTwist,float twistRandomness) {
		// Calculate a random angle for branches to leave from
		float forkAngle = Random.Range(forkAngleBounds.x,forkAngleBounds.y) * (Random.Range(0,1)==1 ? 1:-1); // angle of the fork ring (vertices curving between branches)
		LODRandNums.Add(forkAngle);
		
		// Add offset to LODList for greater accuracy
		LODRandNums.Add(offset.x);
		LODRandNums.Add(offset.y);
		LODRandNums.Add(offset.z);
		
		float angle_A = (Mathf.PI/4.0f) + (forkAngle*Mathf.Deg2Rad)/2.0f;
		float angle_B = (Mathf.PI/4.0f) - (forkAngle*Mathf.Deg2Rad)/2.0f;
		float radius_A = Mathf.Cos(angle_A)*radiusRef;
		float radius_B = Mathf.Cos(angle_B)*radiusRef;
		
		Quaternion ringRotation_A = angleRef*Quaternion.Euler(angle_A*Mathf.Rad2Deg,0.0f,0.0f);
		Vector3 ringCentre_A = (ringRotation_A*(new Vector3(0.0f,radiusRef,0.0f))) + offset;
		Quaternion ringRotation_B = angleRef*Quaternion.Euler(-angle_B*Mathf.Rad2Deg,0.0f,0.0f);
		Vector3 ringCentre_B = (ringRotation_B*(new Vector3(0.0f,radiusRef,0.0f))) + offset;
		
		int segments_A = radialSegments;
		int segments_B = radialSegments;
		if(radius_A>radius_B) {
			ringCentre_B = offset;
			segments_B = Mathf.RoundToInt(radialSegments*(radius_B/radiusRef));
			if(segments_B < 3) segments_B = 3;
		}
		if(radius_B>radius_A) {
			ringCentre_A = offset;
			segments_A = Mathf.RoundToInt(radialSegments*(radius_A/radiusRef));
			if(segments_A < 3) segments_A = 3;
		}
		
		BuildBranch(meshBuilder,ringCentre_A,radius_A,ringRotation_A,lengthSegments,segments_A,forkAngleBounds,maxBendAngle,branchLength,lengthRandomness,branchTwist,twistRandomness,true);
		
		BuildBranch(meshBuilder,ringCentre_B,radius_B,ringRotation_B,lengthSegments,segments_B,forkAngleBounds,maxBendAngle,branchLength,lengthRandomness,branchTwist,twistRandomness,false);
		
	}
	
	void BuildLODFork(int LODindex,MeshBuilder meshBuilder,Vector3 offset,float radiusRef,Quaternion angleRef,int lengthSegments,int radialSegments) {
		
		// Use forkAngle from full detail model
		float forkAngle = NextLODRand();
		Vector3 LODoffset = Vector3.zero;
		LODoffset.x = NextLODRand();
		LODoffset.y = NextLODRand();
		LODoffset.z = NextLODRand();
		
		//Debug.Log("LOD1 ForkAngle: " + forkAngle);
		float angle_A = (Mathf.PI/4.0f) + (forkAngle*Mathf.Deg2Rad)/2.0f;
		float angle_B = (Mathf.PI/4.0f) - (forkAngle*Mathf.Deg2Rad)/2.0f;
		float radius_A = Mathf.Cos(angle_A)*radiusRef;
		float radius_B = Mathf.Cos(angle_B)*radiusRef;
		
		Quaternion ringRotation_A = angleRef*Quaternion.Euler(angle_A*Mathf.Rad2Deg,0.0f,0.0f);
		Vector3 ringCentre_A = (ringRotation_A*(new Vector3(0.0f,radiusRef,0.0f))) + LODoffset;
		Quaternion ringRotation_B = angleRef*Quaternion.Euler(-angle_B*Mathf.Rad2Deg,0.0f,0.0f);
		Vector3 ringCentre_B = (ringRotation_B*(new Vector3(0.0f,radiusRef,0.0f))) + LODoffset;
		
		int segments_A = radialSegments;
		int segments_B = radialSegments;
		if(radius_A>radius_B) {
			ringCentre_B = LODoffset;
			segments_B = Mathf.RoundToInt(radialSegments*(radius_B/radiusRef));
			if(segments_B < 3) segments_B = 3;
		}
		if(radius_B>radius_A) {
			ringCentre_A = LODoffset;
			segments_A = Mathf.RoundToInt(radialSegments*(radius_A/radiusRef));
			if(segments_A < 3) segments_A = 3;
		}
		
		BuildLODBranch(LODindex, meshBuilder,ringCentre_A,radius_A,ringRotation_A,lengthSegments,segments_A,true);
		
		BuildLODBranch(LODindex, meshBuilder,ringCentre_B,radius_B,ringRotation_B,lengthSegments,segments_B,false);
		
	}
	
	void BuildBranch(MeshBuilder meshBuilder,Vector3 offset,float startRadius,Quaternion startAngle,int lengthSegments,int radialSegments,Vector2 forkAngleBounds,float maxBendAngle,float branchLength,float lengthRandomness,float branchTwist,float twistRandomness,bool startTriangles) {
		BuildBranch(meshBuilder,offset,startRadius,startAngle,lengthSegments,radialSegments,forkAngleBounds,maxBendAngle,branchLength,lengthRandomness,branchTwist,twistRandomness,startTriangles,true);
	}
	
	void BuildBranch(MeshBuilder meshBuilder,Vector3 offset,float startRadius,Quaternion startAngle,int lengthSegments,int radialSegments,Vector2 forkAngleBounds,float maxBendAngle,float branchLength,float lengthRandomness,float branchTwist,float twistRandomness,bool startTriangles,bool useMinRadius) {
		BuildBranch(meshBuilder,offset,startRadius,startAngle,lengthSegments,radialSegments,forkAngleBounds,maxBendAngle,branchLength,lengthRandomness,branchTwist,twistRandomness,startTriangles,true,false);
	}
	
	void BuildBranch(MeshBuilder meshBuilder,Vector3 offset,float startRadius,Quaternion startAngle,int lengthSegments,int radialSegments,Vector2 forkAngleBounds,float maxBendAngle,float branchLength,float lengthRandomness,float branchTwist,float twistRandomness,bool startTriangles,bool useMinRadius,bool generateLOD) {
		
		PGTreeBase p = transform.parent.GetComponent("PGTreeBase") as PGTreeBase;
		
		// Calculate a random bend towards the sky
		Vector3 angleRand = Vector3.zero;
		Vector3 testVector = Vector3.down;
		float maxAngle = maxBendAngle;
		for(int i = 0; i < 100 && testVector.y <= p.m_branch_min_uprightness; i++) {
			angleRand = new Vector3(Random.Range(-maxAngle,maxAngle),Random.Range(-maxAngle,maxAngle),Random.Range(-maxAngle,maxAngle));
			maxAngle += 30;
			Quaternion newRot = startAngle*Quaternion.Euler(angleRand.x,angleRand.y,angleRand.z);
			testVector = newRot*Vector3.up;
			if(i == 99) {
				Debug.LogError("Maximum iterations reached. Bend angle may not be upright!");
			}
		}
		LODRandNums.Add(angleRand.x);
		LODRandNums.Add(angleRand.y);
		LODRandNums.Add(angleRand.z);
		
		Vector3 ringCentre = offset;
		Quaternion rotation = Quaternion.identity;
		
		float twistRand = branchTwist+Random.Range(-twistRandomness,twistRandomness);
		float lengthRand = branchLength + Random.Range(-lengthRandomness,lengthRandomness);
		LODRandNums.Add(twistRand);
		LODRandNums.Add(lengthRand);
		
		float radius = startRadius;
		
		for (int i = 0; i<=lengthSegments;i++) {
			float heightRatio = (float)i/(float)(lengthSegments);
			//float ringHeight = heightRatio*lengthRand;
			float twistAngle = heightRatio*twistRand;
			bool buildTriangles = !startTriangles && i==0 ? false : true;
			radius = useMinRadius ? radius : radius*p.m_branch_radius_falloff;
			
			float v = (heightRatio*lengthRand)/(2*Mathf.PI*radius) + 1.0f/(2*Mathf.PI);
			
			
			if(i == lengthSegments && useMinRadius) {
				// Add new fork
				BuildFork(meshBuilder,ringCentre,radius,startAngle*rotation*Quaternion.Euler(0.0f,-twistRand,0.0f),lengthSegments,radialSegments,
						  forkAngleBounds,maxBendAngle*p.m_branch_bend_falloff,branchLength*p.m_branch_length_falloff,lengthRandomness,branchTwist*p.m_branch_twist_falloff,twistRandomness);
				break;
			} else if(radius < p.m_branch_min_radius && useMinRadius) {
				// Complete branch
				//rotation = Quaternion.Euler(angleRand*heightRatio);
				//ringCentre += startAngle*rotation*new Vector3(0.0f,lengthRand/lengthSegments,0.0f);
				v = (((float)(i+1)/(float)lengthSegments)*lengthRand)/(2*Mathf.PI*radius) + 1.0f/(2*Mathf.PI);
				radius = 0;
				BuildCurve(meshBuilder,ringCentre,radius,radialSegments,v,0,0,0.0f,0.0f,startAngle*rotation,twistAngle,0,buildTriangles);
				if(p.m_hasLeaves) {
					if(p.m_hasStems) {
						BuildStems(meshBuilder,ringCentre,startAngle*rotation*Quaternion.Euler(0.0f,twistRand,0.0f),p.m_stem_length,p.m_stem_radius,p.m_stem_segments,p.m_stem_bend);
					}
					m_Leaves.Add(new LeafConstruct(ringCentre,startAngle*rotation*Quaternion.Euler(0.0f,twistRand,0.0f),p.m_stem_length,p.m_stem_bend));
				}
				break;
			}
			BuildCurve(meshBuilder,ringCentre,radius,radialSegments,v,0,0,0.0f,0.0f,startAngle*rotation,twistAngle,0,buildTriangles);
			rotation = Quaternion.Euler(angleRand*((float)i/(float)(lengthSegments+1)));
			ringCentre += startAngle*rotation*new Vector3(0.0f,lengthRand/(float)lengthSegments,0.0f);
		}
		
	}
	
	void BuildLODBranch(int LODindex, MeshBuilder meshBuilder,Vector3 offset,float startRadius,Quaternion startAngle,int lengthSegments,int radialSegments,bool startTriangles) {
		
		PGTreeBase p = transform.parent.GetComponent("PGTreeBase") as PGTreeBase;
		
		// Calculate a random bend towards the sky
		Vector3 angleRand = Vector3.zero;
		angleRand.x = NextLODRand();
		angleRand.y = NextLODRand();
		angleRand.z = NextLODRand();
		
		Vector3 ringCentre = offset;
		Quaternion rotation = Quaternion.identity;
		
		float twistRand = NextLODRand();
		float lengthRand = NextLODRand();
		
		float radius = startRadius;
		
		for (int i = 0; i<=lengthSegments;i++) {
			float heightRatio = (float)i/(float)lengthSegments;
			//float ringHeight = heightRatio*lengthRand;
			float twistAngle = heightRatio*twistRand;
			bool buildTriangles = !startTriangles && i==0 ? false : true;
			//radius = useMinRadius ? radius : radius*p.m_branch_radius_falloff;
			
			float v = (heightRatio*lengthRand)/(2*Mathf.PI*radius) + 1.0f/(2*Mathf.PI);
			
			
			//BuildCurve(meshBuilder,ringCentre,radius,radialSegments,v,0,0,0.0f,0.0f,startAngle*rotation,twistAngle,0,buildTriangles);
			if(i == lengthSegments) {
				// Add new fork
				BuildLODFork(LODindex,meshBuilder,ringCentre,radius,startAngle*rotation*Quaternion.Euler(0.0f,-twistRand,0.0f),lengthSegments,radialSegments);
				break;
			} else if(radius < p.m_branch_min_radius) {
				// Complete branch
				//rotation = Quaternion.Euler(angleRand*heightRatio);
				//ringCentre += startAngle*rotation*new Vector3(0.0f,lengthRand/lengthSegments,0.0f);
				v = (((float)(i+1)/(float)lengthSegments)*lengthRand)/(2*Mathf.PI*radius) + 1.0f/(2*Mathf.PI);
				radius = 0;
				BuildCurveSimple(meshBuilder,ringCentre,radius,radialSegments,v,startAngle*rotation,twistAngle,buildTriangles);
				break;
			}
			BuildCurveSimple(meshBuilder,ringCentre,radius,radialSegments,v,startAngle*rotation,twistAngle,buildTriangles);
			rotation = Quaternion.Euler(angleRand*((float)i/(float)(lengthSegments+1)));
			ringCentre += startAngle*rotation*new Vector3(0.0f,lengthRand/(float)lengthSegments,0.0f);
			
			
		}
		
	}
	
	void BuildStems(MeshBuilder meshBuilder, Vector3 offset, Quaternion rotation,float length,float radius,int segments,float bendAngle) {
		PGTreeBase p = transform.parent.GetComponent("PGTreeBase") as PGTreeBase;
		for(int i = 0; i < p.m_leaves_per_branch; i++) {
			Quaternion stemRotation = rotation*Quaternion.Euler(0.0f,(360*(float)i)/(float)p.m_leaves_per_branch,0.0f)*Quaternion.Euler(0.0f,0.0f,p.m_stem_start_angle);
			BuildBranch(meshBuilder,offset,radius,stemRotation,segments,3,new Vector2(0.0f,0.0f),0.0f,length,0.0f,0.0f,0.0f,false,false);
		}
	}
	
	void BuildCurve(MeshBuilder meshBuilder, Vector3 offset, float radius, int segments,
					float v, Quaternion rotation, bool buildTriangles) {
		BuildCurve(meshBuilder,offset,radius,segments,v,0,0,0.0f,0.0f,rotation,0.0f,0.0f,buildTriangles,0);
			
	}
	
	void BuildCurve(MeshBuilder meshBuilder, Vector3 offset, float radius, int segments,
					float v, Quaternion rotation, bool buildTriangles, float circDivide) {
		BuildCurve(meshBuilder,offset,radius,segments,v,0,0,0.0f,0.0f,rotation,0.0f,0.0f,buildTriangles,0,circDivide);
			
	}
		
	void BuildCurve(MeshBuilder meshBuilder, Vector3 offset, float radius, int segments,
					float v, int f1, int f2, float angle_offset1, float angle_offset2, 
					Quaternion rotation, float twist_angle, float irregularity, bool buildTriangles) 
	{
		BuildCurve(meshBuilder,offset,  radius,  segments,
					 v,  f1,  f2,  angle_offset1,  angle_offset2, 
					rotation, twist_angle, irregularity, buildTriangles, 0);
	}
	
	void BuildCurve(MeshBuilder meshBuilder, Vector3 offset, float radius, int segments,
					float v, int f1, int f2, float angle_offset1, float angle_offset2, 
					Quaternion rotation, float twist_angle, float irregularity, bool buildTriangles, int branchVertices) 
	{
		BuildCurve(meshBuilder,offset,  radius,  segments,
					 v,  f1,  f2,  angle_offset1,  angle_offset2, 
					rotation, twist_angle, irregularity, buildTriangles, branchVertices,1.0f);
	}
	void BuildCurve(MeshBuilder meshBuilder, Vector3 offset, float radius, int segments,
					float v, int f1, int f2, float angle_offset1, float angle_offset2, 
					Quaternion rotation, float twist_angle, float irregularity, bool buildTriangles, int branchVertices, float circDivide) 
	{
		for(int i = 0; i <= segments/circDivide; i++) {
			// Calculate vertex position
			float theta = (Mathf.PI*2.0f*(float)i)/(float)segments + twist_angle*Mathf.Deg2Rad;
			// Offset radius randomly
			float ro1 = Mathf.Sin(f1*((float)i/(float)segments)*2*Mathf.PI + angle_offset1)*irregularity;
			float ro2 = Mathf.Sin(f2*((float)i/(float)segments)*2*Mathf.PI + angle_offset2)*irregularity;
			float r = radius+(ro1+ro2);
			
			float x = Mathf.Cos(theta)*r;
			float y = 0.0f;
			float z = Mathf.Sin(theta)*r;
			//
			
			// Set normals to face along radial line
			meshBuilder.Normals.Add(new Vector3(Mathf.Cos(theta), 0, Mathf.Sin(theta)));
			meshBuilder.Vertices.Add((rotation * new Vector3(x,y,z)) + offset);
			meshBuilder.UVs.Add(new Vector2(i/(float)(segments),v));
			
			if(buildTriangles) {
				int baseIndex = meshBuilder.Vertices.Count-1;
				
				if(i != 0) {
					meshBuilder.AddTriangle(baseIndex,baseIndex-segments-branchVertices-1,baseIndex-1);
				}
				if(i != segments) {
					meshBuilder.AddTriangle(baseIndex,baseIndex-segments-branchVertices,baseIndex-segments-branchVertices-1);
				}
			}
		}
	}
	
	private void BuildCurveSimple(MeshBuilder meshBuilder, Vector3 offset, float radius, int segments,float v,
								  Quaternion rotation, float twist_angle, bool buildTriangles) 
	{
		BuildCurveSimple(meshBuilder,offset,radius,segments,v,rotation,twist_angle,buildTriangles,0);
	}
	
	private void BuildCurveSimple(MeshBuilder meshBuilder, Vector3 offset, float radius, int segments,float v,
								  Quaternion rotation, float twist_angle, bool buildTriangles, int branchVertices)
	{
		for(int i = 0; i <= segments; i++) {
			// Calculate vertex position
			float theta = (Mathf.PI*2.0f*(float)i)/(float)segments + twist_angle*Mathf.Deg2Rad;
			
			float x = Mathf.Cos(theta)*radius;
			float y = 0.0f;
			float z = Mathf.Sin(theta)*radius;
			//
			
			// Set normals to face along radial line
			meshBuilder.Normals.Add(new Vector3(Mathf.Cos(theta), 0, Mathf.Sin(theta)));
			meshBuilder.Vertices.Add((rotation * new Vector3(x,y,z)) + offset);
			meshBuilder.UVs.Add(new Vector2(i/(float)(segments),v));
			
			if(buildTriangles) {
				int baseIndex = meshBuilder.Vertices.Count-1;
				
				if(i != 0) {
					meshBuilder.AddTriangle(baseIndex,baseIndex-segments-branchVertices-1,baseIndex-1);
				}
				if(i != segments) {
					meshBuilder.AddTriangle(baseIndex,baseIndex-segments-branchVertices,baseIndex-segments-branchVertices-1);
				}
			}
		}
	}
			
	public Vector3 Bezier(Vector3 start, Vector3 controlMid1, Vector3 controlMid2, Vector3 end, float t)
	{
		float t2 = t * t;
		float t3 = t2 * t;

		float mt = 1 - t;
		float mt2 = mt * mt;
		float mt3 = mt2 * mt;

		return start * mt3 + controlMid1 * mt2 * t * 3.0f + controlMid2 * mt * t2 * 3.0f + end * t3;
	}

	public Vector3 BezierTangent(Vector3 start, Vector3 controlMid1, Vector3 controlMid2, Vector3 end, float t)
	{
		float t2 = t * t;

		float mt = 1 - t;
		float mt2 = mt * mt;

		float mid = 2.0f * t * mt;

		Vector3 tangent = start * -mt2 + controlMid1 * (mt2 - mid) + controlMid2 * (-t2 + mid) + end * t2;

		return tangent.normalized;
	}
	
	public float ExpoLinInterp(float startRadius, float midRadius, float endRadius, float endHeight,float interpHeight) {
		return ExpoLinInterp(startRadius,midRadius,endRadius,endHeight,interpHeight,0.0f);			
	}
	public float ExpoLinInterp(float startRadius, float midRadius, float endRadius, float endHeight, float interpHeight, float linearBlend) {
		float rm = midRadius;
		float r0 = startRadius;
		float rf = endRadius;
		float hf = endHeight;
				
		if(rf+r0 == 2*rm) {rm+=0.001f;} // Avoid DIV-by-0
		float M = (Mathf.Pow(rm,2)-2*rm*r0+Mathf.Pow(r0,2))/(rf-2*rm+r0);
		float c = r0-M;
		float alpha = (-1/hf)*Mathf.Log(((rf-r0)/M)+1);
		return M*Mathf.Exp(-alpha*interpHeight)+c - (interpHeight*endRadius*linearBlend)/endHeight;
	}
	
	// LOD Helper functions
	private float NextLODRand() {
		return LODRandNums[LODRandIndex++];
	}
}

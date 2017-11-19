﻿using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class PGTree : PGBase {
	
	public class ForkConstruct {

		public struct BranchPoint 
		{
			public Vector3 position;
			public Quaternion rotation;
			public Vector3 direction;
			public float radius;
			public BranchPoint(Vector3 position, Quaternion rotation, Vector3 direction, float radius) {
				this.position = position;
				this.rotation = rotation;
				this.direction = direction;
				this.radius = radius;
			}
		}

		// Point and rotation of final ring of previous branch
		Vector3 position;
		Quaternion rotation;

		// Angle of the main branch
		float branchAngle;

		// Indices of vertices to connect to
		int startIndex, endIndex;

		// Branch point outputs
		public BranchPoint mainBranchPoint;
		public BranchPoint secondBranchPoint;

		public ForkConstruct(Vector3 position, Quaternion rotation, float branchAngle, int startIndex, int endIndex) {
			this.position = position;
			this.rotation = rotation;
			this.branchAngle = branchAngle;
			this.startIndex = startIndex;
			this.endIndex = endIndex;

			// Create 2 branch points

			// Main branch
			//Vector3 mainPosition = 
			mainBranchPoint = new BranchPoint();

		}

		public Vector3 mainBranchPosition() {
			return Vector3.zero;
		}

		public Quaternion mainBranchRotation() {
			return Quaternion.identity;
		}

		public Vector3 branchDirection;
	}

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
	
	// Frequency variables for trunk vertex offsets
	int[] m_frequencies;
	float[] m_frequency_offsets;
	float irregularity; // Irregularity is the amount the frequencies are applied
	
	public override Mesh BuildMesh()
	{
		return BuildTrunk();
	}

	private Mesh BuildTrunk() {
		
		// Set variables from parent
		TreeSpecies p = transform.parent.GetComponent<TreeSpecies>();

		// Ensure leaf list is empty
		m_Leaves.Clear();
		
		MeshBuilder meshBuilder = new MeshBuilder();
		
		Vector3 offset = new Vector3(0.0f,0.0f,0.0f);
		
		// Create an irregular trunk shape by blending some sin waves
		
		// Generate 2 random harmonic frequencies to blend.
		//m_frequencies = new int[] { UnityEngine.Random.Range(1,5)*2, UnityEngine.Random.Range(1,3)*2+2 };
		//m_frequency_offsets = new float[] { UnityEngine.Random.Range(0.0f,Mathf.PI*2), UnityEngine.Random.Range(0.0f,Mathf.PI*2) };
		m_frequencies = new int[] { p.m_trunk_freq_1, p.m_trunk_freq_2 };
		m_frequency_offsets = new float[] { p.m_trunk_freq_off_1, p.m_trunk_freq_off_2 };


		// Define radius function
		// Function has 2 arguments - angle and height
		// Both are normalised and vary between 0 and 1
		float height = 0;
		Func<float, float> radiusFunction = (angle) => {

			float radius = 0;
			for (int i = 0; i < 2; i++) 
			{
				radius += 0.5f*Mathf.Sin( 2*Mathf.PI * m_frequencies[i] * angle + m_frequency_offsets[i]) + 0.5f;
			}
			// Exponential coefficients
			float A = 1.0f;
			float b = p.m_trunk_irregularity_coeff;
			float c = -Mathf.Log(A);
			// Exponential falloff * linear falloff (to ensure final value is zero)
			float height_falloff = (A * Mathf.Exp(c-height*b)) * Mathf.Lerp(1.0f,0.0f,height);
			return 1 + radius * height_falloff * p.m_trunk_irregularity;
		};

		// Offset radius randomly
		

		Vector3 ring_centre = offset;
		Quaternion rotation = Quaternion.identity;
		
		BuildRing(meshBuilder,ring_centre,rotation,p.m_radial_segments,0.0f,p.m_start_radius,radiusFunction,false);
		for(int i = 1; i <= p.m_height_segments; i++) {
			float t = (float)i/(float)p.m_height_segments;
			height = t;
			float ring_height = t*p.m_height;
			float radius = p.m_start_radius; 
			
			
			//float twist_angle = (ring_height*p.m_twist_angle)/p.m_height;
			
			Vector3 upDirection = Vector3.up; // Change this to bezier tangent

			rotation *= Quaternion.AngleAxis(p.m_twist_angle, upDirection);

			//float hr = (ring_height/p.m_height);
			//rotation = Quaternion.Euler(angle_rand.x*hr,angle_rand.y*hr,angle_rand.z*hr);
			//Vector3 new_ring_offset = new Vector3(0.0f,p.m_height/(float)p.m_height_segments,0.0f);
			float ringSpacing = p.m_height/(float)p.m_height_segments;
			ring_centre += upDirection * ringSpacing;
			
			float v = ring_height/(2*Mathf.PI*radius);
			
			//BuildCurve(meshBuilder,ring_centre,radius,p.m_radial_segments,v,f1,f2,angle_offset1,angle_offset2,rotation,twist_angle,irregularity,true);
			
			BuildRing(meshBuilder,ring_centre,rotation,p.m_radial_segments,v,radius,radiusFunction,true);

			if(i==p.m_height_segments) {
				// This is the final segment

				// Build a fork

				// Major branch
				float branchAngle = p.m_branch_fork_angle-180;
				float branchRadius = 0.707f * radius * Mathf.Sqrt(1 - Mathf.Sin(branchAngle * Mathf.Deg2Rad));
				float branchDirectionAngle = (90 + branchAngle)/2;
				Vector3 branchDirection = Quaternion.Euler(0,0,branchDirectionAngle) * Vector3.up;
				Quaternion branchRotation = rotation * Quaternion.Euler(0,0,branchDirectionAngle);
				Vector3 branchStartOffset = branchDirection*radius;
				BuildRing(meshBuilder,ring_centre + branchStartOffset,branchRotation,p.m_radial_segments,0.0f,branchRadius,true);

				BuildRing(meshBuilder,ring_centre + branchStartOffset + branchDirection*4,branchRotation,p.m_radial_segments,1.0f,branchRadius,true);

				// Minor branch
				branchAngle = p.m_branch_fork_angle;
				branchRadius = 0.707f * radius * Mathf.Sqrt(1 - Mathf.Sin(branchAngle * Mathf.Deg2Rad));
				branchDirectionAngle = (90 + branchAngle)/2;
				branchDirection = Quaternion.Euler(0,0,branchDirectionAngle) * Vector3.up;
				branchRotation = rotation * Quaternion.Euler(0,0,branchDirectionAngle);
				branchStartOffset = branchDirection*radius/2;
				BuildRing(meshBuilder,ring_centre + branchStartOffset,branchRotation,p.m_radial_segments,0.0f,branchRadius,false);

				BuildRing(meshBuilder,ring_centre + branchStartOffset + branchDirection*4,branchRotation,p.m_radial_segments,1.0f,branchRadius,true);

				// Build two branches from fork




				/*
				// Add twist angle to rotation for complete angle reference
				Quaternion rotationRef = rotation*Quaternion.Euler(0.0f,-p.m_twist_angle,0.0f);
				LODRandNums.Add(radius);
				BuildFork(meshBuilder,ring_centre,radius,rotationRef,p.m_branch_segments,p.m_radial_segments,new Vector2(p.m_branch_min_fork_angle,p.m_branch_max_fork_angle),
						  p.m_branch_max_bend,p.m_branch_length,p.m_branch_length_randomness,p.m_branch_twist_angle,p.m_branch_twist_randomness);
				break;
				*/
			}
		}
		Mesh mesh = meshBuilder.CreateMesh();
		mesh.RecalculateNormals();
		return mesh;
	}
	

/*
	// Creates a fork and builds 2 branches off it
	// -> radiusRef: radius of trunk
	// -> angleRef: angle of the final trunk ring
	void BuildFork(MeshBuilder meshBuilder,Vector3 offset,float radiusRef,Quaternion angleRef,int lengthSegments,int radialSegments,Vector2 forkAngleBounds,float maxBendAngle,float branchLength,float lengthRandomness,float branchTwist,float twistRandomness) {
		// Calculate a random angle for branches to leave from
		float forkAngle = Random.Range(forkAngleBounds.x,forkAngleBounds.y) * (Random.Range(0,1)==1 ? 1:-1); // angle of the fork ring (vertices curving between branches)
		
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
	
	void BuildBranch(MeshBuilder meshBuilder,Vector3 offset,float startRadius,Quaternion startAngle,int lengthSegments,int radialSegments,Vector2 forkAngleBounds,float maxBendAngle,float branchLength,float lengthRandomness,float branchTwist,float twistRandomness,bool startTriangles) {
		BuildBranch(meshBuilder,offset,startRadius,startAngle,lengthSegments,radialSegments,forkAngleBounds,maxBendAngle,branchLength,lengthRandomness,branchTwist,twistRandomness,startTriangles,true);
	}
	
	void BuildBranch(MeshBuilder meshBuilder,Vector3 offset,float startRadius,Quaternion startAngle,int lengthSegments,int radialSegments,Vector2 forkAngleBounds,float maxBendAngle,float branchLength,float lengthRandomness,float branchTwist,float twistRandomness,bool startTriangles,bool useMinRadius) {
		BuildBranch(meshBuilder,offset,startRadius,startAngle,lengthSegments,radialSegments,forkAngleBounds,maxBendAngle,branchLength,lengthRandomness,branchTwist,twistRandomness,startTriangles,true,false);
	}
	
	void BuildBranch(MeshBuilder meshBuilder,Vector3 offset,float startRadius,Quaternion startAngle,int lengthSegments,int radialSegments,Vector2 forkAngleBounds,float maxBendAngle,float branchLength,float lengthRandomness,float branchTwist,float twistRandomness,bool startTriangles,bool useMinRadius,bool generateLOD) {
		
		PGTreeBase p = transform.parent.GetComponent("PGTreeBase") as PGTreeBase;

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
*/
/*
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
*/

	void BuildRing(MeshBuilder meshBuilder, Vector3 position, Quaternion rotation, int segments, float v, float baseRadius, bool buildTriangles)
	{
		BuildRing(meshBuilder,position,rotation,segments,v,baseRadius,(value) => { return 1.0f; }, buildTriangles);
	}

	// Creates a ring of (segments+1) vertices at the given position and rotation.
	// If a radiusFunction is given, it can be used to offset the radius based on the normalized angle (360 deg = 1.0)
	// If buildTriangles is set to true, function assumes a previous ring of the same number of segments was created
	// and creates triangles to form a cylinder.
	void BuildRing(MeshBuilder meshBuilder, Vector3 position, Quaternion rotation, int segments, float v, float baseRadius, Func<float,float> radiusFunction, bool buildTriangles) {

		for(int i = 0; i <= segments; i++) {

			float t = (float)i/(float)segments;

			// Calculate vertex position
			float angle = Mathf.PI*2.0f*t;
			// Apply radius function
			float r = baseRadius * radiusFunction(t);
			
			float x = Mathf.Cos(angle)*r;
			float y = 0.0f;
			float z = Mathf.Sin(angle)*r;
			//
			
			// Set normals to face along radial line
			meshBuilder.Normals.Add(new Vector3(Mathf.Cos(angle), 0, Mathf.Sin(angle)));
			meshBuilder.Vertices.Add((rotation * new Vector3(x,y,z)) + position);
			meshBuilder.UVs.Add(new Vector2(t,v));
			
			if(buildTriangles) {
				int baseIndex = meshBuilder.Vertices.Count-1;
				
				if(i != 0) {
					meshBuilder.AddTriangle(baseIndex,baseIndex-segments-1,baseIndex-1);
				}
				if(i != segments) {
					meshBuilder.AddTriangle(baseIndex,baseIndex-segments,baseIndex-segments-1);
				}
			}
		}

	}
}
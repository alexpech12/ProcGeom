using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BranchPoint {
		public Vector3 point = Vector3.zero;
		public Quaternion rotation = Quaternion.identity;
		public BranchPoint() {}
		public BranchPoint(Vector3 point, Quaternion rotation) {
				this.point = point;
				this.rotation = rotation;
		}
}

[ExecuteInEditMode]
public class PGTree2 : MonoBehaviour {

		public float m_polygonDensity = 1.0f;

		public float m_startWidth = 1.0f;
		public float m_widthFalloff = 0.9f;
		public float m_trunkLength = 5.0f;
		public float m_branchLength = 3.0f;
		public float m_branchLengthFalloff = 0.9f;
		[Range(-45.0f, 45.0f)]
		public float m_branchRotation = 0.0f;
		public int m_branchNum = 3;

		MeshBuilder m_builder;

	// Use this for initialization
	void Start () {
	
				m_builder = new MeshBuilder();


	}
	
	// Update is called once per frame
	void Update () {
				m_builder = new MeshBuilder();

				List<BranchPoint> branchList = new List<BranchPoint>();
				branchList.Add(new BranchPoint());
				float branchLength = m_branchLength;
				// For as many branches are wanted
				for(int b = 0; b < m_branchNum; b++) {
						// For each branch...
						List<BranchPoint> newBranchList = new List<BranchPoint>();
						foreach(BranchPoint branch in branchList) {

							// Build branches
								Vector3 finalPosition = Vector3.zero;
								Quaternion finalRotation = Quaternion.identity;
								for(int n = 0; n < 2; n++) {
									for(int i = 0; i < 8; i++) {
												Vector3 branchStart = branch.point;
								Quaternion branchRotation = Quaternion.FromToRotation(Vector3.up,branch.rotation*Quaternion.AngleAxis((2*n-1)*45.0f + m_branchRotation, Vector3.forward)*Vector3.up);
								Vector3 branchEnd = branchStart + branchRotation*Vector3.up*branchLength;
												Vector3 branchMid1 = (branchEnd-branchStart)*0.33f;
												Vector3 branchMid2 = (branchEnd-branchStart)*0.67f;
												Bezier3D branchCurve = new Bezier3D(branchStart, branchMid1, branchMid2, branchEnd);

												float t = i/(8.0f-1.0f);

												Vector3 ringPosition = branchCurve.Point(t);
												Quaternion rotation = Quaternion.FromToRotation(Vector3.up, branchCurve.Tangent(t));
												BuildRing(m_builder, 8, ringPosition, m_startWidth, t, i!=0, rotation, Vector3.zero);
												finalPosition = ringPosition;
												finalRotation = rotation;

										}
										newBranchList.Add(new BranchPoint(finalPosition, finalRotation));
								}

						}
						branchList.Clear();
						branchList = newBranchList;
						

						branchLength *= m_branchLengthFalloff;


				}
				Mesh newMesh = m_builder.CreateMesh();
				newMesh.RecalculateNormals();
				MeshFilter filter = GetComponent<MeshFilter>();
				filter.sharedMesh = newMesh;
	
	}

		protected void BuildRing(MeshBuilder meshBuilder, int segmentCount, Vector3 centre, float radius, float v, bool buildTriangles, Quaternion rotation, Vector3 normalOffset)
		{
				float angleInc = (Mathf.PI * 2.0f) / segmentCount;

				for (int i = 0; i <= segmentCount; i++)
				{
						float angle = angleInc * i;

						Vector3 unitPosition = Vector3.zero;
						unitPosition.x = Mathf.Cos(angle);
						unitPosition.z = Mathf.Sin(angle);

						Quaternion normalRotation = Quaternion.LookRotation(unitPosition);
						Vector3 normal = normalRotation * normalOffset;

						unitPosition = rotation * unitPosition;

						meshBuilder.Vertices.Add(centre + unitPosition * radius);
						meshBuilder.Normals.Add(rotation * normal);
						meshBuilder.UVs.Add(new Vector2((float)i / segmentCount, v));

						if (i > 0 && buildTriangles)
						{
								int baseIndex = meshBuilder.Vertices.Count - 1;

								int vertsPerRow = segmentCount + 1;

								int index0 = baseIndex;
								int index1 = baseIndex - 1;
								int index2 = baseIndex - vertsPerRow;
								int index3 = baseIndex - vertsPerRow - 1;

								meshBuilder.AddTriangle(index0, index2, index1);
								meshBuilder.AddTriangle(index2, index3, index1);
						}
				}
		}
}

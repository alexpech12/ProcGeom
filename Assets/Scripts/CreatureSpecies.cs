using UnityEngine;
using System.Collections;

// Class to hold all the genetic information of a creature
public class CreatureSpecies {
	
	// These variables are defined using genetic algorithm when new species is created
	LegData m_frontLegData; 	public LegData frontLegData { get { return m_frontLegData; } }
	LegData m_backLegData;		public LegData backLegData { get { return m_backLegData; } }
	
	SpineData m_spineData;		public SpineData spineData { get { return m_spineData; } }
	
	float m_legSeparation;		public float legSeparation { get { return m_legSeparation; } }
	float m_spineElevation;		public float spineElevation{ get { return m_spineElevation; } }
	
	float m_tailLength;			public float tailLength { get { return m_tailLength; } }
	float m_tailTaper;			public float tailTaper { get { return m_tailTaper; } }
	int m_tailSegments;			public int tailSegments { get { return m_tailSegments; } }
	
	Bezier3D m_bodyWidthCurve;	public Bezier3D bodyWidthCurve { get { return m_bodyWidthCurve; } }
	Bezier3D m_bodyBackCurve;	public Bezier3D bodyBackCurve { get { return m_bodyBackCurve; } }
	
	// These variables are derived from the random variables declared above
	ClipContainer m_clips;		public ClipContainer clips { get { return m_clips; } }
	
	public void CreateNewSpecies() {
		
		// First we must generate all random variables
		
		// First, define leg dimensions
		// Note, radii should always be less than length for stability, and ideally much less
		float frontHipRadius = 0.5f;
		float frontThighLength = 0.7f;	float frontThighRoundness = 0.6f;
		float frontKneeRadius = 0.4f;
		float frontCalfLength = 0.7f;	float frontCalfRoundness = 0.3f;
		float frontAnkleRadius = 0.3f;
		float frontFootLength = 0.6f;	float frontFootRoundness = 0.6f;
		float frontFootRadius = 0.4f;
		float frontToeLength = 0.5f;	float frontToeRoundness = 0.5f;
		float frontToeRadius = 0.3f;
		
		float backHipRadius = 0.7f;
		float backThighLength = 1.3f;	float backThighRoundness = 0.3f;
		float backKneeRadius = 0.4f;
		float backCalfLength = 1.2f;	float backCalfRoundness = 0.6f;
		float backAnkleRadius = 0.3f;
		float backFootLength = 0.6f;	float backFootRoundness = 0.7f;
		float backFootRadius = 0.2f;
		float backToeLength = 0.3f;		float backToeRoundness = 0.9f;
		float backToeRadius = 0.1f;
		
		// Data containers must be created for each segment
		BodySegmentMeshData fr_thighData = new BodySegmentMeshData(frontThighLength,frontHipRadius,frontKneeRadius,frontThighRoundness,12);
		BodySegmentMeshData fr_calfData = new BodySegmentMeshData(frontCalfLength,frontKneeRadius,frontAnkleRadius,frontCalfRoundness,12);
		BodySegmentMeshData fr_footData = new BodySegmentMeshData(frontFootLength,frontAnkleRadius,frontFootRadius,frontFootRoundness,12);
		BodySegmentMeshData fr_toeData = new BodySegmentMeshData(frontToeLength,frontFootRadius,frontToeRadius,frontToeRoundness,12);
		
		BodySegmentMeshData bk_thighData = new BodySegmentMeshData(backThighLength,backHipRadius,backKneeRadius,backThighRoundness,12);
		BodySegmentMeshData bk_calfData = new BodySegmentMeshData(backCalfLength,backKneeRadius,backAnkleRadius,backCalfRoundness,12);
		BodySegmentMeshData bk_footData = new BodySegmentMeshData(backFootLength,backAnkleRadius,backFootRadius,backFootRoundness,12);
		BodySegmentMeshData bk_toeData = new BodySegmentMeshData(backToeLength,backFootRadius,backToeRadius,backToeRoundness,12);
		
		// LegData objects can now be created by passing in segment data
		// Front and back legs have separate leg data
		m_frontLegData = new LegData(fr_thighData,fr_calfData,fr_footData,fr_toeData);
		m_frontLegData.reverseKnee = true; // Front legs bend the opposite way
		m_backLegData = new LegData(bk_thighData,bk_calfData,bk_footData,bk_toeData);
		
		// Creating the leg data has also defined the height at which each leg stands
		// Use this to set the default heights for the spine
		float shoulderHeight = m_frontLegData.legLength;
		float pelvisHeight = m_backLegData.legLength;
		
		// Define other random variables for the spine
		float shoulderRadius = 0.8f;
		float torsoLength = 1.2f;	float torsoRoundness = 0.8f;
		float chestRadius = 1.0f;
		float waistLength = 1.5f;	float waistRoundness = 0.2f;
		float waistRadius = 0.8f;
		float pelvisLength = 1.2f;	float pelvisRoundness = 0.4f;
		float hipRadius = 0.6f;
		
		BodySegmentMeshData torsoData = new BodySegmentMeshData(torsoLength,chestRadius,shoulderRadius,torsoRoundness,20);
		BodySegmentMeshData waistData = new BodySegmentMeshData(waistLength,waistRadius,chestRadius,waistRoundness,20);
		BodySegmentMeshData pelvisData = new BodySegmentMeshData(pelvisLength,hipRadius,waistRadius,pelvisRoundness,20);
		
		m_spineData = new SpineData(torsoData,waistData,pelvisData,0.0f,0.0f);
		Debug.Log("Checking, spine length is " + m_spineData.spineLength);
		m_legSeparation = Mathf.Sqrt(m_spineData.spineLength*m_spineData.spineLength-(shoulderHeight-pelvisHeight)*(shoulderHeight-pelvisHeight));
		m_spineElevation = Mathf.Rad2Deg*Mathf.Asin((shoulderHeight-pelvisHeight)/m_spineData.spineLength);
		Debug.Log("Leg separation: " + m_legSeparation + ", Spine elevation: " + m_spineElevation);
		
		
		// Define tail dimensions
		m_tailLength = 5.0f;
		m_tailTaper = 0.8f;
		m_tailSegments = 6;
		
		
		
		// Create animations
		// Animations are based only on species variables defined above
		//AnimationGenerator animGenerator = new AnimationGenerator(this);
		//m_clips = animGenerator.GenerateAnimation();
		m_clips = new ClipContainer(this);
		m_clips.EnableAnimationGroup(ClipContainer.AnimType.IDLE);
		//ClipContainer clips = new ClipContainer(species);
		//clips.EnableAnimationGroup(ClipContainer.AnimType.IDLE);
		Debug.Log("m_clips = " + m_clips);
	}
}

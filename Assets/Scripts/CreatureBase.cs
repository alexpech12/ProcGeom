using UnityEngine;
using System.Collections;

public class CreatureBase : MonoBehaviour {
	
	public CreatureSpecies species = null;
	
	// Use this for initialization
	void Start () {
		if(species == null) {
			Debug.LogWarning("CreatureBase added with no species set. Creating new species...");
			species = new CreatureSpecies();
			species.CreateNewSpecies();
		}
		
		// Attach body parts as children
		GameObject body = NewChild("body");
		BodyBase bodyBase = body.AddComponent<BodyBase>();
		bodyBase.Init();
		
		PGAnimationController animator = gameObject.AddComponent<PGAnimationController>();
		animator.Init(ref species, bodyBase.legBase);
	}
	
	GameObject NewChild(string name) {
		GameObject childObject = new GameObject(name);
		childObject.transform.parent = transform;
		childObject.transform.localPosition = Vector3.zero;
		return childObject;
	}
}

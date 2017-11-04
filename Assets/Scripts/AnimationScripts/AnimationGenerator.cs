using UnityEngine;
using System.Collections;

public class AnimationGenerator {
	
	CreatureSpecies species;
	
	public AnimationGenerator(CreatureSpecies species) {this.species = species;}
	
	public ClipContainer GenerateAnimation() {
		ClipContainer clips = new ClipContainer(species);
		clips.EnableAnimationGroup(ClipContainer.AnimType.IDLE);
		return clips;
	}
}

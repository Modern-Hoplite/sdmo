using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationEventPlayAnim : AnimationEvent
{
	public AnimationData anim;
	public bool forceAnim = true;

	public AnimationEventPlayAnim(AnimationData anim, bool force = true)
	{
		this.anim = anim;
		this.forceAnim = force;
	}

	public override void Activate (Mecha m, AnimationSet s, int curFrame)
	{
		if (s == null)
			return;

		if (forceAnim)
			s.ForceAnim (anim, m);
		else
			s.PlayAnim (anim, m);
	}
}

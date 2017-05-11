using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationListenerBoostStop : AnimationListener
{
	public List<AnimationData> okToTransitionToData = new List<AnimationData>();

	public AnimationListenerBoostStop()
	{}

	public void AddVettedAnim(AnimationData d)
	{
		okToTransitionToData.Add (d);
	}

	public override bool OnTryToPlayAnimation (Mecha m, AnimationSet s, AnimationData d, out bool playAnim, bool wasGoingToPlay)
	{
		playAnim = true;
		if (MechaInput.boosting)
			return false;

		if (okToTransitionToData.Contains (d)) {
			return true;
		} else {
			return false;
		}
	}
}

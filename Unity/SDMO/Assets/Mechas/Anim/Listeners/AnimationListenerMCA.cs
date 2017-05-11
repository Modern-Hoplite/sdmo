using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationListenerMCA : AnimationListener {
	private bool unlock = false;
	// TODO Timing
	public override void OnAnimReset (Mecha m, AnimationSet s)
	{
		unlock = false;
	}

	public override bool OnTryToPlayAnimation (Mecha m, AnimationSet s, AnimationData d, out bool playAnim, bool wasGoingToPlay)
	{
		List<AnimationData> listSwitchAnims = s.GetWeaponSwitchAnimations ();
		bool isSwitchAnim = listSwitchAnims.Contains(d);
		playAnim = wasGoingToPlay;

		if (isSwitchAnim) {
			unlock = true;
			playAnim = false;
			s.FastForwardAnim (d, m);
			return true;
		} else {
			if (unlock) {
				bool isFollowUpAnim = GetFollowupAnims (m, s).Contains (d);
				playAnim = isFollowUpAnim;
				return isFollowUpAnim;
			} else {
				return false;
			}
		}

		// IF NOT SWITCH ANIM - FALSE
		//return false;

		// IF SWITCH ANIM CHANGE OVERRIDE PRIORITY
	}

	public virtual List<AnimationData> GetFollowupAnims(Mecha m, AnimationSet s)
	{
		List<AnimationData> l = new List<AnimationData> ();

		l.Add (s.boostB);
		l.Add (s.boostF);
		l.Add (s.boostL);
		l.Add (s.boostR);
		l.AddRange (s.GetWeaponUseAnimations ());
		// TODO Check if that is all the animations needed

		return l;
	}
}

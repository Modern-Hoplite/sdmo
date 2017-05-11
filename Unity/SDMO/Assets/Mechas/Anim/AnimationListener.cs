using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* Animations Listeners activate events when a specific trigger is detected
 * 
 * They can also be used to regulate if an incoming animation is going to be played
 */
public class AnimationListener
{
	public AnimationEvent animEvent;

	public AnimationListener(AnimationEvent e = null)
	{
		animEvent = e;
	}

	// Is called when the animation is reset
	public virtual void OnAnimReset(Mecha m, AnimationSet s) {}

	// Is called each time AnimProgress is called
	public virtual void OnAnimProgress(Mecha m, AnimationSet s, float time) {}

	// Is called each frame pass
	public virtual void OnFramePass(Mecha m, AnimationSet s, int frame){}

	// Is called when an animation is going to be played
	// returns if the listener has an inpact on if we are going to play the animation or not
	// the actual result being in playAnim
	public virtual bool OnTryToPlayAnimation(Mecha m, AnimationSet s, AnimationData d, out bool playAnim, bool wasGoingToPlay)
	{
		playAnim = wasGoingToPlay;
		return false;
	}
}

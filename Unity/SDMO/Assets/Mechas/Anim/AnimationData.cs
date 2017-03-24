using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationData
{
	public string animNameSystem, animNameUser;

	public float fps;
	public int nbFrames;

	private int currentFrame = 0;
	private float frameProgress = 0f;

	public int priority = 0, minimumPriorityToCancel = 0;

	public List<AnimationEvent> animEvents = new List<AnimationEvent>();

	public AnimationData(string animNameSystem = "error", string animNameUser = "", int priority = 0, int nbFrames = 24, float fps = 24f)
	{
		this.animNameSystem = animNameSystem;
		this.animNameUser = (animNameUser == "" ? animNameSystem : animNameUser);
		this.fps = fps;
		this.nbFrames = nbFrames;
		this.priority = priority;
	}

	// Makes the animation ready for use
	public virtual void ResetAnim(Mecha m, AnimationSet s)
	{
		currentFrame = 0;
		frameProgress = 0f;

		FramePass (m, s, 0);
	}

	// Called regularly to progress the animation data
	public virtual void AnimProgress(Mecha m, AnimationSet s, float time)
	{
		frameProgress += time * fps;

		while (frameProgress >= 1f) {
			frameProgress -= 1f;
			currentFrame++;
			FramePass (m, s, currentFrame);
		}
	}

	// Called when going to frame [frame], to execute events
	public virtual void FramePass(Mecha m, AnimationSet s, int frame)
	{
		foreach (AnimationEvent e in animEvents) {
			int eventFrame = e.frame;

			if (frame == eventFrame)
				e.Activate (m, s);
			else if (frame < eventFrame)
				e.BeforeActivation (m,s);
			else
				e.AfterActivation (m,s);
		}
	}


	// Sends back an erroneous AnimationData
	public static AnimationData GetAnimationDataError(string name = "No name")
	{
		AnimationData a = new AnimationData ("error");
		a.animNameUser = "Error (" + name + ")";

		return a;
	}


	public bool CanBeCancelled(AnimationData challenger)
	{
		return challenger.priority >= minimumPriorityToCancel;
	}


	public void AddEvent(AnimationEvent e, int frame = 0)
	{
		e.frame = frame;
		animEvents.Add (e);
	}

	public void AddEventAtEnd(AnimationEvent e)
	{
		AddEvent (e, nbFrames);
	}
}

public enum AnimationType
{
	Normal = 0, WeaponSwitch = 50, Boost = 20, Attack = 100
}
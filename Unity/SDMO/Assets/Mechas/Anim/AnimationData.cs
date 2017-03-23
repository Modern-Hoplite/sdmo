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

	public List<AnimationEvent> animEvents = new List<AnimationEvent>();

	public AnimationData(string animNameSystem = "error", string animNameUser = "", int nbFrames = 24, float fps = 24f)
	{
		this.animNameSystem = animNameSystem;
		this.animNameUser = (animNameUser == "" ? animNameSystem : animNameUser);
		this.fps = fps;
		this.nbFrames = nbFrames;
	}

	// Makes the animation ready for use
	public virtual void ResetAnim()
	{
		currentFrame = 0;
		frameProgress = 0f;

		FramePass (0);
	}

	// Called regularly to progress the animation data
	public virtual void AnimProgress(float time)
	{
		frameProgress += time;

		while (frameProgress >= 1f) {
			frameProgress -= 1f;
			currentFrame++;
			FramePass (currentFrame);
		}
	}

	// Called when going to frame [frame], to execute events
	public virtual void FramePass(int frame)
	{
		foreach (AnimationEvent e in animEvents) {
			int eventFrame = e.frame;

			if (frame == eventFrame)
				e.Activate ();
			else if (frame < eventFrame)
				e.BeforeActivation ();
			else
				e.AfterActivation ();
		}
	}


	// Sends back an erroneous AnimationData
	public static AnimationData GetAnimationDataError(string name = "No name")
	{
		AnimationData a = new AnimationData ("error");
		a.animNameUser = "Error (" + name + ")";

		return a;
	}
}

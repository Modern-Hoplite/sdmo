using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationEventChangePriorityOverride : AnimationEvent
{
	public int priorityBefore = 10, priorityAfter = 0;

	public AnimationEventChangePriorityOverride(int priorityBefore, int priorityAfter)
	{
		this.priorityBefore = priorityBefore;
		this.priorityAfter = priorityAfter;
	}

	public override void BeforeActivation (Mecha m, AnimationSet s, int curFrame)
	{
		s.currentAnim.minimumPriorityToCancel = priorityBefore;
	}

	public override void AfterActivation (Mecha m, AnimationSet s, int curFrame)
	{
		s.currentAnim.minimumPriorityToCancel = priorityAfter;
	}

	public override void Activate (Mecha m, AnimationSet s, int curFrame)
	{
		AfterActivation (m, s, curFrame);
	}
}

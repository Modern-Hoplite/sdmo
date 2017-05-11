using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationEvent
{
	public int frame = 0;

	public virtual void Activate(Mecha m, AnimationSet s, int curFrame) {}
	public virtual void BeforeActivation(Mecha m, AnimationSet s, int curFrame) {}
	public virtual void AfterActivation(Mecha m, AnimationSet s, int curFrame) {}
}

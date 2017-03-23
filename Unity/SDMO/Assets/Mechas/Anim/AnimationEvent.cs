using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationEvent
{
	public int frame = 0;

	public virtual void Activate() {}
	public virtual void BeforeActivation() {}
	public virtual void AfterActivation() {}
}

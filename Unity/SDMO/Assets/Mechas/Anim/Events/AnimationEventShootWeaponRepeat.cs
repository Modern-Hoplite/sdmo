using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationEventShootWeaponRepeat : AnimationEventShootWeapon
{
	public int endFrame;

	public AnimationEventShootWeaponRepeat(UnitWeapon w, int endFrame) : base(w)
	{
		this.endFrame = endFrame;
	}

	public override void AfterActivation (Mecha m, AnimationSet s, int curFrame)
	{
		if (curFrame <= endFrame)
			Action (m);
	}
}

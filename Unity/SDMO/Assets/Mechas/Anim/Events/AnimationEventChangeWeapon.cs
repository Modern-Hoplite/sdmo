using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationEventChangeWeapon : AnimationEvent {
	public UnitWeapon weapon;

	public AnimationEventChangeWeapon(UnitWeapon w)
	{
		weapon = w;
	}

	public override void Activate (Mecha m, AnimationSet s, int curFrame)
	{
		m.currentWeapon = weapon;
	}
}

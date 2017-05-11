using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Shoots either a specified weapon or the currently equiped one
public class AnimationEventShootWeapon : AnimationEvent
{
	public UnitWeapon weapon;

	public AnimationEventShootWeapon(UnitWeapon w = null)
	{
		weapon = w;
	}

	public override void Activate (Mecha m, AnimationSet s, int curFrame)
	{
		Action (m);
	}

	public void Action(Mecha m)
	{
		UnitWeapon w = weapon;
		if (weapon == null)
			w = m.currentWeapon;

		w.Shoot (m);
	}
}

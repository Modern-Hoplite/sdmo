using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* Sets a melee weapon's combo to 
 * 
 */
public class AnimationEventComboSet : AnimationEvent
{
	public UnitWeaponMelee weapon;
	public int combo;

	public AnimationEventComboSet(UnitWeaponMelee w, int c) : base()
	{
		weapon = w;
		combo = c;
	}

	public override void Activate (Mecha m, AnimationSet s, int curFrame)
	{
		weapon.combo = combo;
	}
}

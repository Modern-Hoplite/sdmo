using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitWeaponGun : UnitWeapon
{
	public override void UseWeapon (Mecha m)
	{
		AttackData ad = GetAttackData ().Clone ();

		foreach (UnitSkill s in m.GetActiveSkills()) {
			s.OnAttack (ad);
		}

		// TODO Make an object or a raycast

		ManagerShared.i.CreateSFX (0, m.sub.firePoint.position, m.sub.firePoint.rotation);
	}
}

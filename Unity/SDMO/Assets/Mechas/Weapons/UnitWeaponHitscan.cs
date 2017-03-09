using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* This class should be used for most ranged weapons
 */
public class UnitWeaponHitscan : UnitWeapon
{
	public override void UseWeapon (Mecha m)
	{
		AttackData ad = GetAttackData ().Clone ();

		foreach (UnitSkill s in m.GetActiveSkills()) {
			s.OnAttack (ad);
		}

		// TODO Make an object or a raycast
		// It will take care of checking for a target

		// The SFX is used to reduce the strain on the network
		m.sub.firePoint.transform.LookAt(m.aimPoint);

		// TMP
		//m.sub.firePoint.transform.position = m.aimPoint;

		ManagerShared.i.CreateSFX (0, m.sub.firePoint.position, m.sub.firePoint.rotation);
	}
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* This class should be used for most ranged weapons
 */
public class UnitWeaponHitscan : UnitWeapon
{
	private float range = 50f;

	public UnitWeaponHitscan(string name, AttackData attackData)
	{
		Constructor (name, attackData);
	}

	public override void Shoot (Mecha m, Transform firePoint)
	{
		ShotSFX (m, firePoint.position, firePoint.rotation);

		AttackData ad = GetAttackData ().Clone ();

		foreach (UnitSkill s in m.GetActiveSkills()) {
			s.OnAttack (ad);
		}

		RaycastHit rayHit;
		if (Physics.Raycast (firePoint.position, firePoint.TransformDirection (Vector3.forward), out rayHit, range)) {
			Mecha target = rayHit.collider.GetComponent<Mecha> ();
			if (target) {
				target.GetHit (ad);
			}
		}
	}


	/*public override void UseWeapon (Mecha m)
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
	}*/


}

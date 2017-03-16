using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitWeaponSpecial : UnitWeaponComposed
{
	private float spRequired = 100f;

	public UnitWeaponSpecial(UnitWeapon internalWeapon)
	{
		Constructor ("Special-Weapon", new AttackData ());
		SetInternalWeapon (internalWeapon);
	}

	public override bool UseWeapon (Mecha m)
	{
		bool r = false;
		if (m.sp >= spRequired) {
			r = GetInternalWeapon ().UseWeapon (m);
			if(r)
				m.sp -= spRequired;
		}
		return r;
	}
}

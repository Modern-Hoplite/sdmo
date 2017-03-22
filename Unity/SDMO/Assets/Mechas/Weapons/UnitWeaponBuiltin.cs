using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitWeaponBuiltin : UnitWeaponComposed
{
	private bool fastUse = false; // TODO Fast use

	public UnitWeaponBuiltin(UnitWeapon internalWeapon)
	{
		Constructor ("Builtin-Weapon", new AttackData ());
		SetInternalWeapon (internalWeapon);
	}

	public override void SwitchToWeapon (Mecha m)
	{
		if (fastUse)
			GetInternalWeapon ().UseWeapon (m);
		else
			base.SwitchToWeapon (m);
	}

	protected override Transform[] FirePoints (Mecha m)
	{
		if (fastUse)
			return m.sub.GetFirePoints (m.GetWeaponID (this));
		else
			return base.FirePoints (m);
	}
}

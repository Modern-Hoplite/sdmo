using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitWeaponComposed : UnitWeapon
{
	private UnitWeapon internalWeapon;

	public UnitWeaponComposed()
	{
		internalWeapon = new UnitWeapon ();
	}

	public UnitWeaponComposed(UnitWeapon internalWeapon)
	{
		this.internalWeapon = internalWeapon;
	}

	public UnitWeapon GetInternalWeapon()
	{
		return internalWeapon;
	}

	public void SetInternalWeapon(UnitWeapon w)
	{
		internalWeapon = w;
	}

	// REDIRECT FUNCTIONS

	public override string GetName ()
	{
		return internalWeapon.GetName ();
	}
	public override void SetName (string name)
	{
		internalWeapon.SetName (name);
	}

	public override AttackData GetAttackData ()
	{
		return internalWeapon.GetAttackData ();
	}
	public override void SetAttackData (AttackData attackData)
	{
		internalWeapon.SetAttackData (attackData);
	}

	public override void Shoot (Mecha m, Transform firePoint)
	{
		internalWeapon.Shoot (m, firePoint);
	}

	public override bool UseWeapon (Mecha m)
	{
		return internalWeapon.UseWeapon (m);
	}
}

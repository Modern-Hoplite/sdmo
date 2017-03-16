using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitWeaponMelee : UnitWeapon
{
	public UnitWeaponMelee(string name, AttackData attackData)
	{
		Constructor (name, attackData);
	}

	public override bool UseWeapon (Mecha m)
	{
		Debug.Log ("Melee has no code yet");
		return false;
	}

}

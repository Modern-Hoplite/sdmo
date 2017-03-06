using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitSkillAttackUp : UnitSkill
{
	private float multiplier = 1.0f;

	public override void OnAttack (AttackData attackData)
	{
		attackData.SetDamage ((int)(attackData.GetDamage() * multiplier));
	}
}

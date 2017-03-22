using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* Represents a skill
 * Contains a lot of empty virtual methods that can be overwritten to accomodate a specific skill
 * 
 * Incomplete as of now, we need to add the triggers as needed.
 */
public class UnitSkill
{
	public virtual void OnAttack(AttackData attackData) {}
	public virtual void OnDefense(AttackData attackData) {}

	public virtual string GetName()
	{
		return "No Skill";
	}

	// TODO [...]
}

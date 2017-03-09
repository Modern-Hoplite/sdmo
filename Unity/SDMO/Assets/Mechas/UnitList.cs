using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* Unit List
 * Contains some static methods to centralize access to units.
 * 
 */
public class UnitList
{
	public static Unit GetUnit(int id)
	{
		Unit u = new Unit(id, "Gundam", UnitRank.B);
		u.SetSkill1 (new UnitSkillAttackUp ());
		u.SetWeapon1 (new UnitWeaponHitscan());

		/* Transformable unit template
		 if (id == 2) {
			Unit untransformed = new Unit (id, "Gundam (Magnetic Coating)", UnitRank.A);
			Unit transformed = new Unit (id, "Gundam (Last Stand)", UnitRank.A);
			u = new UnitTransform (untransformed, transformed);
		}*/

		return u;
	}
}

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
		int unitID = id % 100;
		int seriesID = (unitID - id) / 100;

		switch (seriesID) {
		case 0:
			return List000 (unitID);
		default:
			return UndefinedUnit (id);
		}

		/* Transformable unit template
		 if (id == 2) {
			Unit untransformed = new Unit (id, "Gundam (Magnetic Coating)", UnitRank.A);
			Unit transformed = new Unit (id, "Gundam (Last Stand)", UnitRank.A);
			u = new UnitTransform (untransformed, transformed);
		}*/
	}
		
	private static Unit UndefinedUnit(int id)
	{
		Unit u = new Unit (id, "Box Gundam (" + id+")", UnitRank.SS);
		u.SetSkill1 (new UnitSkillAttackUp ());
		u.SetSkill2 (new UnitSkillAttackUp ());
		u.SetWeapon1 (new UnitWeaponHitscan ("Box Punch", new AttackData()));
		u.SetWeapon2 (new UnitWeaponHitscan ("Box Rifle", new AttackData()));
		u.SetWeapon3 (new UnitWeaponHitscan ("Box Bazooka", new AttackData()));

		return u;
	}

	// Gundam 0079
	private static Unit List000(int unitID)
	{
		int seriesID = 0;
		int id = seriesID * 100 + unitID;

		Unit unit;
		UnitSkill skill1, skill2;
		UnitWeapon weapon1, weapon2, weapon3;

		switch (unitID) {
		case 1:
			unit = new Unit (id, "Gundam", UnitRank.B);
			skill1 = new UnitSkillAttackUp ();
			skill2 = new UnitSkillAttackUp ();
			weapon1 = new UnitWeaponMelee ("Beam Saber", new AttackData());
			weapon2 = new UnitWeaponHitscan ("Beam Rifle", new AttackData());
			weapon3 = new UnitWeaponBuiltin(new UnitWeaponHitscan ("Vulcan", new AttackData()));
			break;
		default:
			return UndefinedUnit (id);
		}

		return BuildUnit (unit, weapon1, weapon2, weapon3, skill1, skill2);
	}

	// For convinence
	private static Unit BuildUnit(Unit unit, UnitWeapon weapon1, UnitWeapon weapon2, UnitWeapon weapon3, UnitSkill skill1, UnitSkill skill2)
	{
		unit.SetSkill1 (skill1);
		unit.SetSkill2 (skill2);
		unit.SetWeapon1 (weapon1);
		unit.SetWeapon2 (weapon2);
		unit.SetWeapon3 (weapon3);

		return unit;
	}
}

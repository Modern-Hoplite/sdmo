using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * Parses a text file to load a unit
 * 
 * Currently incomplete
 */
public class UnitParser
{/* commented to clear up the console of warnings
	private int lineNumber = 0;
	private UnitRank unitRank = UnitRank.B;
	private int unitID = 0;
	private string lineInMemory = "";

	public UnitParser()
	{
		AcceptLine ();
	}

	// Returns the next line with no spaces before
	public string NextLine()
	{
		string curL = PeekLine ();
		AcceptLine ();
		return curL;
	}

	// Allows to see the next line without advancing the counter
	public string PeekLine()
	{
		return lineInMemory;
	}

	// Goes to the next line
	public void AcceptLine()
	{
		lineNumber++;
	}

	// Writes an error to the logs for easier debugging
	private void ReportError(string error, int line = -1)
	{
		if (line < 0)
			line = lineNumber;

		Debug.LogError ("[UNIT-PARSER / LINE "+line+"] " + error);
	}

	// Parsing starts here
	public Unit Parse()
	{
		return ParseUnit ();
	}

	// Reads a unit or a transform
	private Unit ParseUnit()
	{
		string line = NextLine ();

		if (line == "UNIT")
			return StandardUnit ();
		if (line == "TRANSFORM")
			return Transform ();
		if (line == "LOAD")
			return LoadUnit ();
		ReportError ("ParseUnit : Unit type unknown ("+line+")");
		return new Unit();
	}

	// Reads a standard unit
	private Unit StandardUnit()
	{
		string unitName = NextLine ();
		Unit u = new Unit(unitID, unitName, unitRank);

		UnitWeapon w1, w2, w3;
		UnitSkill s1, s2;

		w1 = Weapon ();
		w2 = Weapon ();
		w3 = Weapon ();

		s1 = Skill ();
		s2 = Skill ();

		List<UnitSkill> unitSkills = SkillList ();

		u.SetWeapon1 (w1);
		u.SetWeapon2 (w2);
		u.SetWeapon3 (w3);
		u.SetSkill1 (s1);
		u.SetSkill2 (s2);
		//u.SetUnitSkills(unitSkills);

		return u;
	}

	// Reads a Transformable unit
	private Unit Transform()
	{
		string transformType = NextLine ();
		int l = lineNumber;
		Unit a = ParseUnit ();
		Unit b = ParseUnit ();

		if (transformType == "SWITCH") {
			Unit t = new UnitTransform (a, b);
			return t;
		} else if (transformType == "PURGE") {
			Unit t = new UnitTransformPurge (a, b);
			return t;
		} else if (transformType == "DEATH") {
			Unit t = new UnitTransformDeath (a, b);
			return t;
		}
		ReportError ("Transform : Transform type unknown ("+transformType+")", l);
		return new Unit ();
	}

	// Loads another unit, useful for expanding upon
	private Unit LoadUnit()
	{
		// Not done yet
		return new Unit ();
	}

	// Reads a weapon
	private UnitWeapon Weapon()
	{
		string weaponType = NextLine ();

		if (weaponType == "NULL")
			return new UnitWeapon ();

		string weaponName = NextLine ();
		AttackData ad = ReadAttackData ();
		List<UnitSkill> weaponSkills = SkillList();

		UnitWeapon w;

		if (weaponType == "HITSCAN") {
			w = new UnitWeaponHitscan (weaponName, ad);
		} else {
			ReportError ("Weapon : Type Unknown (" + weaponType + ")");
			return new UnitWeapon ();
		}

		w.SetName (weaponName);
		w.SetAttackData (ad);
		// w.SetSkills(weaponSkills);

		return w;
	}

	// Reads a skill
	private UnitSkill Skill()
	{
		string skillType = NextLine();

		if (skillType == "NULL") {
			return new UnitSkill ();
		}
		ReportError ("Skill : Type Unknown ("+skillType+")");
		return new UnitSkill ();
	}

	// Reads a list of skills, can be empty
	private List<UnitSkill> SkillList()
	{
		List<UnitSkill> list = new List<UnitSkill> ();
		string peekLine = PeekLine ();

		while (peekLine == "SKILL") {
			AcceptLine ();
			list.Add (Skill ());
			peekLine = PeekLine ();
		}

		return list;
	}

	// Reads some attack data. Must start with ATTACKDATA
	private AttackData ReadAttackData()
	{
		string adType = NextLine ();

		if (adType == "ATTACKDATA") {
			
			int damage = ReadInt (NextLine (), "ATTACKDATA.Damage");

			AttackType type = ReadAttackType();

			AttackData ad = new AttackData();
			return ad;
		}
		ReportError ("AttackData : Unknown Type ("+adType+")");
		return new AttackData ();
	}

	// Reads an attack type
	private AttackType ReadAttackType()
	{
		string s = NextLine ();

		if (s == "TRUE")
			return AttackType.True;
		if (s == "BEAM")
			return AttackType.Beam;
		if (s == "PHYSICAL")
			return AttackType.Physical;
		ReportError ("AttackType : " + s + " is not a type");
		return AttackType.True;
	}

	// Tries to parse an int.
	private int ReadInt(string line, string errorMessage = "Unspecified")
	{
		int i = 1;
		if (!int.TryParse (line, out i)) {
			ReportError ("Read Int : Parsing error for value ["+errorMessage+"] : " + line);
			i = 1;
		}
		return i;
	}*/
}

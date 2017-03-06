using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitTransform : Unit
{
	private bool transformed = false;
	private Unit unitBase, unitTransform;

	public UnitTransform(Unit untransformed, Unit transformed)
	{
		unitBase = untransformed;
		unitTransform = transformed;
	}

	public override void Transform ()
	{
		transformed = !transformed;
	}

	public override string GetName ()
	{
		return (transformed ? unitTransform.GetName () : unitBase.GetName ());
	}

	// TODO [...]
}

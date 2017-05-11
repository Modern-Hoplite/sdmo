using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MechaMovementBoost : MechaMovement
{
	public float boostSpeed;

	public MechaMovementBoost(MechaMovement mm, float boostSpeed) : base(mm)
	{
		this.boostSpeed = boostSpeed;
	}

	public override Vector3 Movement (Mecha m, Vector3 mov)
	{
		mov = base.Movement (m, mov);

		Vector3 boostDir = new Vector3 (MechaInput.boostingDirection.x, 0f, MechaInput.boostingDirection.y);

		mov += m.transform.TransformDirection (boostDir) * boostSpeed;

		return mov;
	}

	public override string ToString ()
	{
		return "Boost[" + boostSpeed + "] " + base.ToString ();
	}
}

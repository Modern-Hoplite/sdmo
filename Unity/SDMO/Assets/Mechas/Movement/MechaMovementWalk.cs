using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MechaMovementWalk : MechaMovement
{
	public float walkSpeed = 1f;

	public MechaMovementWalk(MechaMovement mm, float walkSpeed) : base(mm)
	{
		this.walkSpeed = walkSpeed;
	}

	public override Vector3 Movement (Mecha m, Vector3 mov)
	{
		mov = base.Movement (m, mov);

		Vector3 inputMov = Vector3.right * MechaInput.movement.x + Vector3.forward * MechaInput.movement.y;

		mov += m.transform.TransformDirection(inputMov) * walkSpeed;

		return mov;
	}

	public override string ToString ()
	{
		return "Walk[" + walkSpeed + "] " + base.ToString ();
	}
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MechaMovementGravity : MechaMovement
{
	public float gravity;

	public MechaMovementGravity(MechaMovement m, float gravityForce) : base(m)
	{
		gravity = gravityForce;
	}

	public override Vector3 Movement (Mecha m, Vector3 mov)
	{
		mov = base.Movement (m, mov);

		mov += Vector3.down * gravity * Time.fixedDeltaTime;

		return mov;
	}
}

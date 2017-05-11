using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MechaMovementJump : MechaMovement
{
	public float jumpSpeed;

	public MechaMovementJump (MechaMovement mm, float jumpSpeed) : base (mm)
	{
		this.jumpSpeed = jumpSpeed;
	}

	public override Vector3 Movement (Mecha m, Vector3 mov)
	{
		mov = base.Movement (m, mov);

		return mov + Vector3.up * jumpSpeed;
	}

	public override string ToString ()
	{
		return "Jump[" + jumpSpeed + "] " + base.ToString ();
	}
}

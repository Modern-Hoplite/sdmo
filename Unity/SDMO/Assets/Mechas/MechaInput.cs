using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* Static structure containing all the input needed
 * 
 * Polled by Mecha
 * Pass the delta time to the Poll method
 * 
 * Pretty much hardcoded right now
 */
public class MechaInput
{
	public static Vector2 movement, aim;
	public static bool shoot, jump, boosting;

	private static float timeSinceLastForward = 10f, boostDoubletap = 0.2f;

	public static void Poll(float deltaTime)
	{
		float mouseSensX = -6f, mouseSensY = 4f;

		bool oldMovement = movement.sqrMagnitude > 0f && movement.y > 0f;

		movement = Vector2.zero;
		if (Input.GetKey(KeyCode.D))
			movement += Vector2.right;
		if (Input.GetKey(KeyCode.A) || Input.GetKey (KeyCode.Q))
			movement -= Vector2.right;
		if (Input.GetKey (KeyCode.W) || Input.GetKey(KeyCode.Z))
			movement += Vector2.up;
		if (Input.GetKey (KeyCode.S))
			movement -= Vector2.up;

		bool curMovement = movement.sqrMagnitude > 0f && movement.y > 0f;

		if (oldMovement && !curMovement) {
			boosting = false;
			timeSinceLastForward = 0f;
		}
		if (!oldMovement && curMovement && timeSinceLastForward <= boostDoubletap)
			boosting = true;

		aim = Vector2.zero;
		aim += Vector2.right * Input.GetAxis ("Mouse X") * mouseSensX;
		aim += Vector2.up * Input.GetAxis ("Mouse Y") * mouseSensY;

		shoot = Input.GetKeyDown (KeyCode.Mouse0);
		jump = Input.GetKey (KeyCode.Space);
		timeSinceLastForward += deltaTime;
	}
}

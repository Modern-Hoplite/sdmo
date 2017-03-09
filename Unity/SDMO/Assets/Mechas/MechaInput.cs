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
	public static Vector2 movement, aim, boostingDirection;
	public static bool shoot, jump, boosting;

	private static float timeSinceMovement = 10f, boostDoubletap = 0.2f, boostDoubletapMaxAngle = 30f;
	private static Vector2 lastBoostMovement = Vector2.up, oldMovement = Vector2.zero;

	public static void Poll(float deltaTime)
	{
		float mouseSensX = -6f, mouseSensY = 4f;

		bool oldMovementNonZero = movement.sqrMagnitude > 0f;

		oldMovement = movement;
		movement = Vector2.zero;
		if (Input.GetKey(KeyCode.D))
			movement += Vector2.right;
		if (Input.GetKey(KeyCode.A) || Input.GetKey (KeyCode.Q))
			movement -= Vector2.right;
		if (Input.GetKey (KeyCode.W) || Input.GetKey(KeyCode.Z))
			movement += Vector2.up;
		if (Input.GetKey (KeyCode.S))
			movement -= Vector2.up;

		bool curMovementNonZero = movement.sqrMagnitude > 0f;

		if (oldMovementNonZero && !curMovementNonZero) {
			boosting = false;
			timeSinceMovement = 0f;
			lastBoostMovement = oldMovement;
		}
		if (!oldMovementNonZero && curMovementNonZero && timeSinceMovement <= boostDoubletap) {
			if (Vector2.Dot (movement.normalized, lastBoostMovement.normalized) >= Mathf.Cos (boostDoubletapMaxAngle * Mathf.Deg2Rad)) {
				boosting = true;
				if (Mathf.Abs (movement.normalized.y) >= 0.5f)
					boostingDirection = (movement.y > 0f ? Vector2.up : Vector2.down);
				else
					boostingDirection = (movement.x > 0f ? Vector2.right : Vector2.left);
			}
		}

		aim = Vector2.zero;
		aim += Vector2.right * Input.GetAxis ("Mouse X") * mouseSensX;
		aim += Vector2.up * Input.GetAxis ("Mouse Y") * mouseSensY;

		shoot = Input.GetKeyDown (KeyCode.Mouse0);
		jump = Input.GetKey (KeyCode.Space);
		timeSinceMovement += deltaTime;

		Debug.Log (boostingDirection.ToString());
	}
}

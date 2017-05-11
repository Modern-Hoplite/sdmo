using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MechaMovementDemo : MechaMovement
{
	// OLD MOVEMENT CODE
	// Movement variables, should be pulled from the units at a later point
	/*float verticalMinSpeed = 20f, verticalMaxSpeed = 15f, boostVerticalMinSpeed = 10f,
				boostSideVerticalMinSpeed = 12f, boostBackVerticalMinSpeed = 6f;
			float speedWalk = 8f, speedBoost = 20f, speedBoostSide = 18f;
			float jumpSpeed = 30f, gravity = 45f;
			float energyJump = 20f, energyBoost = 30f, energyBoostSide = 25f;
			float speedBoostBack = 80f, energyBoostBack = 20f, boostBackTime = 0.4f;



			// Normal movement
			Vector3 movement = Vector3.zero;
			movement = transform.TransformDirection (inputMov) * speedWalk;

			verticalMinSpeed = (charC.isGrounded ? 0.0f : -verticalMinSpeed);

			if (MechaInput.boosting) {
				if (MechaInput.boostingDirection.y > 0f && UseEnergy (energyBoost * deltaTime)) { // FORWARD BOOST
					Vector3 boostDir = aimDirectionHor + Vector3.up * Mathf.Max (0.0f, aimDirection.y);
					boostDir.Normalize ();
					if (MechaInput.jump) {
						boostDir += Vector3.up;
						boostDir.Normalize ();
					}
					movement = boostDir * speedBoost;
					verticalMinSpeed = Mathf.Max (verticalMinSpeed, -boostVerticalMinSpeed);
				} else if (MechaInput.boostingDirection.x != 0f && UseEnergy (energyBoostSide * deltaTime)) { // SIDEWAYS BOOST
					Vector3 boostDir = transform.TransformDirection(MechaInput.boostingDirection);
					if (MechaInput.jump) {
						boostDir += Vector3.up;
						boostDir.Normalize ();
					}
					movement = boostDir * speedBoostSide;
					verticalMinSpeed = Mathf.Max (verticalMinSpeed, -boostSideVerticalMinSpeed);
				} else if (MechaInput.boostingDirection.y < 0f && backBoostRemainingTime <= 0f && UseEnergy (energyBoostBack)) { // BACKWARDS BOOST
					backBoostRemainingTime = boostBackTime;
				} else
					MechaInput.boosting = false;
			} else
				MechaInput.boosting = false;

			// Back boost overrides movement for a time
			if (backBoostRemainingTime > 0f) {
				backBoostRemainingTime -= deltaTime;
				verticalMinSpeed = Mathf.Max (verticalMinSpeed, -boostBackVerticalMinSpeed);
				movement = -aimDirectionHor * speedBoostBack;
				usedEnergyThisFrame = true;

				if (backBoostRemainingTime > 0f) {
					MechaInput.boosting = true;
					MechaInput.boostingDirection = Vector2.down;
				} else {
					MechaInput.boosting = false;
				}
			}

			// Jumping
			if (!MechaInput.boosting && MechaInput.jump && UseEnergy (energyJump * deltaTime)) {
				vSpeed += jumpSpeed * deltaTime;
			} else {
				vSpeed -= gravity * deltaTime;
				MechaInput.jump = (MechaInput.boosting ? MechaInput.jump : false);
			}



			// Vertical movement adjust and actual movement
			vSpeed += movement.y;
			movement -= Vector3.up * movement.y;
			vSpeed = Mathf.Clamp (vSpeed, verticalMinSpeed, verticalMaxSpeed);
			movement += Vector3.up * vSpeed;
			charC.Move (movement * deltaTime);*/
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SFXHitscan : SFXBase
{
	// TEnd is going to be moved to the point that was shot
	// TBeam is going to be extended on the z axis to the lenght of the shot
	public Transform tStart, tBeam, tEnd;

	private float range = 50f;

	public void Start()
	{
		RaycastHit rayHit;
		Vector3 shotPoint = transform.position + transform.TransformDirection (Vector3.forward) * range;
		float distance = range;

		if (Physics.Raycast (transform.position, transform.TransformDirection (Vector3.forward), out rayHit, range)) {
			shotPoint = rayHit.point;
			distance = rayHit.distance;
		}

		tBeam.localScale = new Vector3 (1f, 1f, distance);
		tEnd.position = shotPoint;
	}
}

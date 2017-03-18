using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* Manager Level contains data specific to the current level
 */
public class ManagerLevel : MonoBehaviour
{
	public static ManagerLevel i;

	public Transform[] spawnPoints;

	public Transform spectatorCamPoint;

	public void Awake()
	{
		i = this;
	}
}

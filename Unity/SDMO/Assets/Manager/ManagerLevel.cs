using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManagerLevel : MonoBehaviour
{
	public static ManagerLevel i;

	public Transform[] spawnPoints;

	public void Awake()
	{
		i = this;
	}
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon;

public class ManagerShared : PunBehaviour
{
	public static ManagerShared i;

	public void Awake()
	{
		i = this;
	}
}

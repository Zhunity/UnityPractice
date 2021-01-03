using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameObjectPathMonoBehaviour : MonoBehaviour
{
	public GameObjectPathSctiptObject sobj;

	private void Awake()
	{
		Debug.Log(sobj, sobj.hi);
	}
}

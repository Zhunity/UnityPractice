using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IfElseOrAnd : MonoBehaviour {

	// Use this for initialization
	void Start () {
		if (a() && b())
		{
			Debug.Log("------------and-------------");
		}
		else
		{
			Debug.Log("------------and else-------------");
		}

		if (b() && a())
		{
			Debug.Log("------------and-------------");
		}
		else
		{
			Debug.Log("------------and else-------------");
		}

		if (a() && c())
		{
			Debug.Log("------------and-------------");
		}
		else
		{
			Debug.Log("------------and else-------------");
		}

		if (b() && d())
		{
			Debug.Log("------------and-------------");
		}
		else
		{
			Debug.Log("------------and else-------------");
		}

		if (a() || b())
		{
			Debug.Log("------------or-------------");
		}
		else
		{
			Debug.Log("------------or else-------------");
		}

		if (b() || a())
		{
			Debug.Log("------------or-------------");
		}
		else
		{
			Debug.Log("------------or else-------------");
		}

		if (a() || c())
		{
			Debug.Log("------------or-------------");
		}
		else
		{
			Debug.Log("------------or else-------------");
		}

		if (b() || d())
		{
			Debug.Log("------------or-------------");
		}
		else
		{
			Debug.Log("------------or else-------------");
		}
	}

	private bool a()
	{
		Debug.Log("a true");
		return true;
	}

	private bool b()
	{
		Debug.Log("b false");
		return false;
	}

	private bool c()
	{
		Debug.Log("c true");
		return true;
	}

	private bool d()
	{
		Debug.Log("d false");
		return false;
	}
}

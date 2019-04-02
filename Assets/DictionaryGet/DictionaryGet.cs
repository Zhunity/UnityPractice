using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class DictionaryGet : MonoBehaviour {
	void Start () {
		Dictionary<int, TestType> dict = new Dictionary<int, TestType>();

		dict[1] = new TestType(1);
		dict.Add(2, null);
		try
		{
			Debug.Log(dict[2].ToString());
		}
		catch(Exception e)
		{
			Debug.LogError(e.ToString());
		}
	}
}

public class TestType
{
	int data = 1;

	public TestType(int i)
	{
		data = i;
	}

	public override string ToString()
	{
		return "data:" + data;
	}
}

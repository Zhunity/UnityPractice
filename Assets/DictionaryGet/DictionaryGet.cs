using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class DictionaryGet : MonoBehaviour {
	void Start () {
		Dictionary<int, TestClass> dict = new Dictionary<int, TestClass>();

		dict[1] = new TestClass(1);
		dict.Add(2, null); // null 可以添加到Dictionary里面，并且可以取出来
		dict.Add(3, new TestClass());
		Debug.Log("Count:" + dict.Count);

		try
		{
			var info = dict[2];
			PrintInfo(info, "dict[2]:  ");

			Debug.Log("dict.ContainsKey(2):    " + dict.ContainsKey(2).ToString()); // 可以找得到空的
		}
		catch(Exception e)
		{
			Debug.LogError(e.ToString());
		}
	}

	private void PrintInfo(TestClass info, string desc)
	{
		if (info == null)
		{
			Debug.Log(desc + "null");
		}
		else
		{
			Debug.Log(desc + info.ToString());
		}
	}
}

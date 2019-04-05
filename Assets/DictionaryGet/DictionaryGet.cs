using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 结论：
/// 在获得List和Dictionary中的元素后，最好也判一下空
/// </summary>

public class DictionaryGet : MonoBehaviour {
	void Start () {
		//FindDictionaryNull();
		FindListNull();
	}

	private void FindListNull()
	{
		List<TestClass> list = new List<TestClass>();
		list.Add(new TestClass(1));
		//list[1] = null; // list 不能这么直接超长度赋值 ArgumentOutOfRangeException: Argument is out of range.
		list.Add(null);
		list.Add(new TestClass());

		Debug.Log("Count:" + list.Count);

		var info = list[2];
		PrintInfo(info, "dict[2]:  ");

		// PrintInfo(list.Find(null), "list.Find(null) "); // ArgumentNullException: Argument cannot be null.
		PrintInfo(list.Find((test) => { return test == null; }), "list.Find(null) "); // 返回null了。。。。不好说找没找到

		// Debug.Log(list.FindIndex(null) + "list.FindIndex(null) "); // ArgumentNullException: Argument cannot be null.
		Debug.Log(list.FindIndex((test)=> { return test == null; }) + "list.FindIndex(null) "); // 找到了

		int i = 0;
		foreach (var item in list)
		{
			// 可以找到null
			PrintInfo(item, i++ + "   foreach : ");
		}

		for(i = 0; i < list.Count; i ++)
		{
			// 可以找到
			PrintInfo(list[i], "for ");
		}
	}

	/// <summary>
	/// Dictionary里面看看有没有null值
	/// </summary>
	private void FindDictionaryNull()
	{
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

			int i = 0;
			foreach (var item in dict)
			{
				// 可以找到null
				PrintInfo(item.Value, i++ + "   foreach : ");
			}

			TestClass setNull = new TestClass();
			PrintInfo(setNull, "before ");
			if (dict.TryGetValue(2, out setNull))
			{
				// 这个也可以找到
				PrintInfo(setNull, "dict.TryGetValue(2, out setNull) after ");
			}
			else
			{
				Debug.Log("dict.TryGetValue(2, out setNull) can not find");
			}
			// TODO ContainValues
		}
		catch (Exception e)
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

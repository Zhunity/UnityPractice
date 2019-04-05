using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DelegateParams : MonoBehaviour
{
	/// <summary>
	/// 只能用参数类型为params object[] objs的函数，不过先这样吧，以后再研究不用params object[] objs类型的
	/// 不过感觉可能会有装箱拆箱消耗
	/// </summary>
	/// <param name="objs"></param>
	delegate void delegateParams(params object[] objs);

	void Start()
	{
		delegateParams testNoParams = new delegateParams(ObjectParams);
		if (null == testNoParams)
		{
			Debug.Log("testNoParams null");
		}
		else
		{
			Debug.Log("testNoParams not null");
		}
		testNoParams = ObjectParams;
		testNoParams += ObjectParams;
		testNoParams(1, "2");
	}

	private void NoParam1()
	{
		Debug.Log("No Param1");
	}

	private void NoParam2()
	{
		Debug.Log("No Param2");
	}

	private void OneParams(int num)
	{
		Debug.Log("One Params " + num);
	}

	private void DifferentTypeParams(int num, string str)
	{
		Debug.Log("Different Type Params " + num + "   " + str);
	}

	private void ObjectParams(params object[] obj)
	{
		Debug.Log("Object Params --------------------------------------------------");

		int i = 0;
		foreach (var item in obj)
		{
			Debug.Log(i++ + "  " + item);
		}
	}
}

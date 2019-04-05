using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DelegateParams : MonoBehaviour
{
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

	int test = 1;
	private void ObjectParams(params object[] obj)
	{
		Debug.Log("Object Params " + test++);
	}
}

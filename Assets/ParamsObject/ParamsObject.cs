using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ParamsObject : MonoBehaviour
{
	private InputField input;

	private void Start()
	{
		input = GetComponent<InputField>();
	}

	private void OnGUI()
	{
		// 数组传参
		if (GUI.Button(new Rect(0, 0, 100, 50), "ArrayParam"))
		{
			// 输入 bool print 1,2,3
			if (string.IsNullOrEmpty(input.text)) return;
			CallSdkFunction(input.text);
		}

		// 直接传参
		if (GUI.Button(new Rect(200, 0, 100, 50), "DirectParam"))
		{
			CallFunc("11111111111111111111111", "222222222222", "33333333333333333333", "4444444", "555", "666");
		}
	}

	const int RETURN_TYPE_INDEX = 0;
	const int METHOD_NAME_INDEX = 1;
	const int PARAM_INDEX = 2;
	public void CallSdkFunction(string funStr)
	{
		Debug.Log("Function Str: " + funStr);
		string[] funParts	= funStr.Split(' ');
		string returnType	= funParts[RETURN_TYPE_INDEX];
		string method		= funParts[METHOD_NAME_INDEX];
		string[] paramList	= funParts[PARAM_INDEX].Split(',');
		CallFunc(returnType, method, paramList);
	}

	private void CallFunc(string returnType, string method, params object[] paramList)
	{
		Debug.Log("Return Type: " + returnType);
		Debug.Log("Method: " + method);
		int i = 0;
		foreach (var param in paramList)
		{
			Debug.Log("param" + i++ + ": " + param);
		}
	}
}

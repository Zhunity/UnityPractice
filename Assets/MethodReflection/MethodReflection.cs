using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class Method
{
	public void MethodWithNoParaNoReturn()
	{
		Debug.Log("不带参数且不返回值的方法");
	}

	public string MethodWithNoPara()
	{
		Debug.Log("不带参数且有返回值的方法");
		return "MethodWithNoPara";
	}

	public string Method1(string str)
	{
		Debug.Log("带参数且有返回值的方法");
		return str;
	}

	public string Method2(string str, int index)
	{
		Debug.Log("带参数且有返回值的方法");
		return str + index.ToString();
	}

	public string Method3(string str, out string outStr)
	{
		outStr = "bbbb";
		Debug.Log("带参数且有返回值的方法");
		return str + outStr;
	}

	public static string StaticMethod()
	{
		Debug.Log("静态方法");
		return "cccc";
	}

	public static void AddListener(UnityAction action)
	{

	}
}


public class MethodReflection : MonoBehaviour {
	private InputField input;

	// Use this for initialization
	void Start () {
		input = GetComponent<InputField>();
	}

	private void OnGUI()
	{
		// 过渡
		if (GUI.Button(new Rect(0, 0, 100, 50), "Method"))
		{
			Type type = typeof(Method);
			object reflectTest = Activator.CreateInstance(type);

			//不带参数且不返回值的方法的调用
			MethodInfo methodInfo = type.GetMethod("MethodWithNoParaNoReturn");
			methodInfo.Invoke(reflectTest, null);

			Debug.Log("------------------------------");

			//不带参数且有返回值的方法的调用
			methodInfo = type.GetMethod("MethodWithNoPara");
			Debug.Log(methodInfo.Invoke(reflectTest, null).ToString());

			Debug.Log("------------------------------");

			//带参数且有返回值的方法的调用
			methodInfo = type.GetMethod("Method1", new Type[] { typeof(string) });
			Debug.Log(methodInfo.Invoke(reflectTest, new object[] { "测试" }).ToString());

			Debug.Log("------------------------------");

			//带多个参数且有返回值的方法的调用
			methodInfo = type.GetMethod("Method2", new Type[] { typeof(string), typeof(int) });
			Debug.Log(methodInfo.Invoke(reflectTest, new object[] { "测试", 100 }).ToString());

			Debug.Log("------------------------------");

			methodInfo = type.GetMethod("Method3");//, new Type[] { typeof(string), typeof(string) });
			object[] objs = new object[] { "测试", ""};
			Debug.Log(methodInfo.Invoke(reflectTest, objs).ToString());
			Debug.Log(objs[0]);
			Debug.Log(objs[1]);

			Debug.Log("------------------------------");

			//静态方法的调用
			methodInfo = type.GetMethod("StaticMethod");
			Debug.Log(methodInfo.Invoke(null, null).ToString());
		}
	}

		// Update is called once per frame
		void Update () {
		
	}
}

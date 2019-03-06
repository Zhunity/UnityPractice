using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using UnityEngine.UI;

public class Method
{
	public int a = 10;

	void Increment()
	{
		Debug.Log(++a);
	}

	void Decrement(int a)
	{
		Debug.Log(--a);
	}
}


public class MethodReflection : MonoBehaviour {
	private InputField input;

	// Use this for initialization
	void Start () {
		
	}

	private void OnGUI()
	{
		// 过渡
		if (GUI.Button(new Rect(0, 0, 100, 50), "CrossFade"))
		{
			//MethodInfo method = typeof(Method).GetMethod(_methodName);
			//_rankModel = method.Invoke(ModelManager.Instance, null) as RankModel;
		}
	}

		// Update is called once per frame
		void Update () {
		
	}
}

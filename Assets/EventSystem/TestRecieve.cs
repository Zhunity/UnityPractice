using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestRecieve : MonoBehaviour
{
	private void Start()
	{
		EventManager.Start();
		EventManager.Bind(EventID.HelloWorld, Test);
	}

	private void Test(params object[] values)
	{
		if (values == null || values.Length == 0)
		{
			Debug.Log("no values");
		}
		for (int i = 0; i < values.Length; i++)
		{
			Debug.Log(i + "  " + values[i]);
		}
	}
}

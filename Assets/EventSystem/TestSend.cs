using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestSend : MonoBehaviour
{
	private void Start()
	{
		StartCoroutine(TestSendEvent());
	}

	private IEnumerator TestSendEvent()
	{
		while (true)
		{
			yield return new WaitForSeconds(1f);
			Debug.Log("TestSendEvent");
			EventManager.Send(EventID.HelloWorld);
		}
	}
}

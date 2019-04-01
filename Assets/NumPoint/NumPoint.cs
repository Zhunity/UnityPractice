using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//https://blog.csdn.net/fatshaw/article/details/3856269

public class NumPoint : MonoBehaviour {

	// Use this for initialization
	void Start () {
		int a = 1;
		int b = 3;

		Debug.Log("a / b " +  a / b);
		Debug.Log("(float)a / b " + (float)a / b);
		Debug.Log("a / (float)b " + a / (float)b);
		Debug.Log("(float)a / (float)b " + (float)a / (float)b);

		float result = (float)a / (float)b;
		Debug.Log("decimal.Round(decimal.Parse(result.ToString()), 2) " + decimal.Round(decimal.Parse(result.ToString()), 2));
		Debug.Log("Math.Round(result, 2) " + Math.Round(result, 2));
		Debug.Log("result.ToString(f2) " + result.ToString("f2"));
		Debug.Log("String.Format({ 0:N2}, result) " + String.Format("{0:N2}", result));
		Debug.Log("result.ToString(#0.00) " + result.ToString("#0.00"));
		Debug.Log("result.ToString(#0.00%) " + result.ToString("#0.00%"));
		Debug.Log("result.ToString(P) " + result.ToString("P"));

		Debug.Log("---------------------------------------------------------------");
		result = 1;
		Debug.Log("decimal.Round(decimal.Parse(result.ToString()), 2) " + decimal.Round(decimal.Parse(result.ToString()), 2));
		Debug.Log("Math.Round(result, 2) " + Math.Round(result, 2));
		Debug.Log("result.ToString(f2) " + result.ToString("f2"));
		Debug.Log("String.Format({ 0:N2}, result) " + String.Format("{0:N2}", result));
		Debug.Log("result.ToString(#0.00) " + result.ToString("#0.00"));
		Debug.Log("result.ToString(#0.00%) " + result.ToString("#0.00%"));
		Debug.Log("result.ToString(P) " + result.ToString("P"));

		Debug.Log("---------------------------------------------------------------");
		Debug.Log("b / 7 " + (b / 7));
		Debug.Log("(float)b / 7 " + ((float)b / 7));
		Debug.Log("b / 7f " + (b / 7f));
		Debug.Log("(float)b / 7f " + ((float)b / 7f));
	}
	
}

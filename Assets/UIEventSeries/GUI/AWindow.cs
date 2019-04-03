using SMFrame;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AWindow : UIView
{
	[UIEventAttribute("onClick", "Button", typeof(Button))]
	public void OnClick()
	{
		Debug.Log("click Button " + gameObject.name);
	}

	[UIEventAttribute("onClick", "Button (1)", typeof(Button))]
	public void OnClickButton1()
	{
		Debug.Log("click Button (1) " + gameObject.name);
	}

	[UIEventAttribute("onValueChanged", "Toggle", typeof(Toggle))]
	public void ToggleValueChange(bool value)
	{
		Debug.Log("ToggleValueChange " + gameObject.name + "  " + value);
	}
}

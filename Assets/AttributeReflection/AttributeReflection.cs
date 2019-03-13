using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

/// <summary>
/// 事件属性
/// </summary>
public class EventAttribute : Attribute
{
	/// <summary>
	/// 事件名
	/// </summary>
	public string EventName
	{
		get;
		set;
	}

	/// <summary>
	/// GameObjectName
	/// </summary>
	public string GameObjectName
	{
		get;
		set;
	}

	public EventAttribute(string eventName, string gameObjectName)
	{
		this.EventName = eventName;
		this.GameObjectName = gameObjectName;
	}
}

public class Reflection : MonoBehaviour
{
	public bool yyyyyyyyyyyyyyyyyyyyyyyy
	{
		get;
		set;
	}
	public PropertyInfo[] _bastAttr;
	private PropertyInfo[] _thisAttr;

	private void Awake()
	{
		_bastAttr = typeof(Reflection).GetProperties(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static); // 只能找到get/set方法
		_thisAttr = this.GetType().GetProperties(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static);
		PrintAttr();
	}

	private void PrintAttr()
	{
		Debug.LogError("-----------------------Reflection Attr Begin-----------------------");
		int count = 0;
		foreach (var item in _bastAttr)
		{
			Debug.Log(count++ + "\nName: " + item.Name + "\n PropertyType: " + item.PropertyType + "\n ReflectedType:" + item.ReflectedType
				+ "\n DeclaringType:" + item.DeclaringType + "\n GetType:" + item.GetType() + "\n MemberType:" + item.MemberType + "\n ToString:" + item.ToString());
		}
		Debug.LogError("-----------------------Reflection Attr End-----------------------");

		Debug.LogError("-----------------------GameObject Attr Begin-----------------------");
		Debug.Log(this.GetType());
		count = 0;
		foreach (var item in _bastAttr)
		{
			Debug.Log(count++ + "\nName: " + item.Name + "\n PropertyType: " + item.PropertyType + "\n ReflectedType:" + item.ReflectedType
				+ "\n DeclaringType:" + item.DeclaringType + "\n GetType:" + item.GetType() + "\n MemberType:" + item.MemberType + "\n ToString:" + item.ToString());
		}
		Debug.LogError("-----------------------GameObject Attr End-----------------------");
	}
}

public class AttributeReflection : Reflection
{
	public float xxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxx;

	[EventAttribute("onClick", "Button")]
	public void OnClick_Button()
	{
		Debug.Log("Attribute Reflection");
	}
}

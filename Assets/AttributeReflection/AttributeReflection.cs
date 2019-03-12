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
	private void Awake()
	{
		PropertyInfo[] _attr = typeof(Reflection).GetProperties(BindingFlags.Public);
	}
}

public class AttributeReflection : Reflection
{

	[EventAttribute("onClick", "Button")]
	private void OnClick_Button()
	{
		Debug.Log("Attribute Reflection");
	}
}

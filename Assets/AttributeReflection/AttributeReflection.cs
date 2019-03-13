using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using UnityEngine.UI;

// https://www.zhihu.com/question/37329635/answer/71500828
//Type类的方法：
	//GetConstructor(), GetConstructors()：返回ConstructorInfo类型，用于取得该类的构造函数的信息；
	//GetEvent(), GetEvents()：返回EventInfo类型，用于取得该类的事件的信息；
	//GetField(), GetFields()：返回FieldInfo类型，用于取得该类的字段（成员变量）的信息；
	//GetInterface(), GetInterfaces()：返回InterfaceInfo类型，用于取得该类实现的接口的信息；
	//GetMember(), GetMembers()：返回MemberInfo类型，用于取得该类的所有成员的信息；
	//GetMethod(), GetMethods()：返回MethodInfo类型，用于取得该类的方法的信息；
	//GetProperty(), GetProperties()：返回PropertyInfo类型，用于取得该类的属性的信息。

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
	/// <summary>
	/// PropertyInfo
	/// </summary>
	public bool yyyyyyyyyyyyyyyyyyyyyyyy
	{
		get;
		set;
	}

	public Button.ButtonClickedEvent ButtonClick;
	public Event EventTest;
	public Action ActionTest;
	public Func<int> FuncTest;

	public PropertyInfo[] _bastAttr;
	private PropertyInfo[] _thisAttr;

	public Reflection()
	{
	}

	~Reflection()
	{

	}

	private void Awake()
	{
		_bastAttr = typeof(Reflection).GetProperties(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static); // 只能找到get/set方法
		_thisAttr = this.GetType().GetProperties(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static);
		//PrintAttr();
		PrintEvent();
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

	private void PrintEvent()
	{
		EventInfo[] baseEventInfo = typeof(Reflection).GetEvents(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static); // 只能找到get/set方法
		EventInfo[] thisEventInfo = this.GetType().GetEvents(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static);
		Debug.LogError("-----------------------Reflection Event Begin-----------------------");
		int count = 0;
		foreach (var item in baseEventInfo)
		{
			Debug.Log(count++ + "\nName: " + item.Name + "\n ReflectedType:" + item.ReflectedType
				+ "\n DeclaringType:" + item.DeclaringType + "\n GetType:" + item.GetType() + "\n MemberType:" + item.MemberType + "\n ToString:" + item.ToString());
		}
		Debug.LogError("-----------------------Reflection Event End-----------------------");

		Debug.LogError("-----------------------GameObject Event Begin-----------------------");
		Debug.Log(this.GetType());
		count = 0;
		foreach (var item in thisEventInfo)
		{
			Debug.Log(count++ + "\nName: " + item.Name + "\n ReflectedType:" + item.ReflectedType
				+ "\n DeclaringType:" + item.DeclaringType + "\n GetType:" + item.GetType() + "\n MemberType:" + item.MemberType + "\n ToString:" + item.ToString());
		}
		Debug.LogError("-----------------------GameObject Event End-----------------------");
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

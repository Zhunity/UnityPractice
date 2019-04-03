using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

/*
// https://www.zhihu.com/question/37329635/answer/71500828
//Type类的方法：
//GetConstructor(), GetConstructors()：返回ConstructorInfo类型，用于取得该类的构造函数的信息；
//GetEvent(), GetEvents()：返回EventInfo类型，用于取得该类的事件的信息；
//GetField(), GetFields()：返回FieldInfo类型，用于取得该类的字段（成员变量）的信息；
//GetInterface(), GetInterfaces()：返回InterfaceInfo类型，用于取得该类实现的接口的信息；
//GetMember(), GetMembers()：返回MemberInfo类型，用于取得该类的所有成员的信息；
//GetMethod(), GetMethods()：返回MethodInfo类型，用于取得该类的方法的信息；
//GetProperty(), GetProperties()：返回PropertyInfo类型，用于取得该类的属性的信息。
*/

/*
 * https://www.cnblogs.com/wangchuang/p/4923621.html
 * 1）使用Assembly定义和加载程序集，加载在程序集清单中列出模块，以及从此程序集中查找类型并创建该类型的实例。

（2）使用Module了解包含模块的程序集以及模块中的类等，还可以获取在模块上定义的所有全局方法或其他特定的非全局方法。

（3）使用ConstructorInfo了解构造函数的名称、参数、访问修饰符（如pulic 或private）和实现详细信息（如abstract或virtual）等。使用Type的GetConstructors或GetConstructor方法来调用特定的构造函数。

（4）使用MethodInfo了解方法的名称、返回类型、参数、访问修饰符（如pulic 或private）和实现详细信息（如abstract或virtual）等。使用Type的GetMethods或GetMethod方法来调用特定的方法。

（5）使用FiedInfo了解字段的名称、访问修饰符（如public或private）和实现详细信息（如static）等，并获取或设置字段值。

（6）使用EventInfo了解事件的名称、事件处理程序数据类型、自定义属性、声明类型和反射类型等，添加或移除事件处理程序。

（7）使用PropertyInfo了解属性的名称、数据类型、声明类型、反射类型和只读或可写状态等，获取或设置属性值。

（8）使用ParameterInfo了解参数的名称、数据类型、是输入参数还是输出参数，以及参数在方法签名中的位置等。
 * /

/*
* MSDN MethodInfo解释
* //https://docs.microsoft.com/zh-cn/dotnet/api/system.reflection.methodinfo?redirectedfrom=MSDN&view=netframework-4.7.2
*/

/*
// MSDN MemberInfo 解释
//https://docs.microsoft.com/en-us/dotnet/api/system.reflection.memberinfo?redirectedfrom=MSDN&view=netframework-4.7.2
*/

/*
 * // Attribute使用
 * // https://www.cnblogs.com/mq0036/p/7844369.html
 */

/// <summary>
/// 事件属性
/// </summary>
public class EventMethod : Attribute
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

	public Type ComponentType;

	public EventMethod(string eventName, string gameObjectName, Type componentType)
	{
		this.EventName = eventName;
		this.GameObjectName = gameObjectName;
		this.ComponentType = componentType;
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
		//PrintMember();
		//PrintAttr();
		//PrintEvent();
		PrintMethod();
		//RegistClickEvent();
	}

	private void RegistClickEvent()
	{
		Debug.Log("------------------BUtton-----------------------" + this.gameObject.name);
		var thisMethodInfo = this.GetType().GetMethods(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static);
		var thisMemberInfo = this.GetType().GetMembers(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static);
		foreach (var item in thisMethodInfo)
		{
			if(item.ReflectedType != item.DeclaringType)
			{
				continue;
			}
			var objList = item.GetCustomAttributes(typeof(EventMethod), false);
			foreach (var attr in objList)
			{
				
				if (attr is EventMethod)
                 {
					EventMethod exe = attr as EventMethod;
					Debug.Log("Click Event " + item.Name + "  EventName:" + exe.EventName + " GameObjectName:" + exe.GameObjectName);
					var button = GetChildComponent(exe.GameObjectName, exe.ComponentType);
				//	MemberInfo[] memberInfo= button.GetType().GetMember(exe.EventName);
				//	int count = 0;
				//	foreach (var member in memberInfo)
				//	{
				//		Debug.Log(count++ + "\nName: " + member.Name + "\n ReflectedType:" + member.ReflectedType
				//+ "\n DeclaringType:" + member.DeclaringType + "\n GetType:" + member.GetType() + "\n MemberType:" + member.MemberType + "\n ToString:" + member.ToString());
				////		MethodInfo addListener = button.GetType().GetMethod("AddListener");
				////		Debug.Log(count++ + "\nName: " + addListener.Name + "\n ReflectedType:" + addListener.ReflectedType
				////+ "\n DeclaringType:" + addListener.DeclaringType + "\n GetType:" + addListener.GetType() + "\n MemberType:" + addListener.MemberType
				////+ "\n ToString:" + addListener.ToString());
				//	}

					var property = button.GetType().GetProperty(exe.EventName);
					Debug.Log(property.PropertyType);

					
					var method = property.PropertyType.GetMethod("AddListener");
					UnityAction target = () => { item.Invoke(this, null);  };
					method.Invoke(property.GetValue(button, null), new object[] { target });
				}
			}
		}
	}

	public Component GetChildComponent(string name, Type type)
	{
		GameObject obj = GetChild(name);
		if(obj != null)
		{
			return obj.GetComponent(type);
		}
		else
		{
			return null;
		}
	}

	public GameObject GetChild(string name)
	{
		return GetChild(this.gameObject, name);
	}

	public GameObject GetChild(GameObject root, string name)
	{
		if(root == null)
		{
			return null;
		}

		if(string.IsNullOrEmpty(name))
		{
			return null;
		}

		if(root.name == name)
		{
			return root;
		}

		foreach(Transform child in root.transform)
		{
			var target = GetChild(child.gameObject, name);
			if(target != null)
			{
				return target;
			}
		}
		return null;
	}

	// ReflectedType 现在反射的类型
	// DeclaringType 定义的类型
	private void PrintMember()
	{
		var bastMembers = typeof(Reflection).GetMembers(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static); // 只能找到get/set方法
		var _thisMembers = this.GetType().GetMembers(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static);
		Debug.LogError("-----------------------Reflection Member Begin-----------------------");
		int count = 0;
		foreach (var item in bastMembers)
		{
			Debug.Log(count++ + "\nName: " + item.Name + "\n ReflectedType:" + item.ReflectedType
				+ "\n DeclaringType:" + item.DeclaringType + "\n GetType:" + item.GetType() + "\n MemberType:" + item.MemberType + "\n ToString:" + item.ToString());
		}
		Debug.LogError("-----------------------Reflection Member End-----------------------");

		Debug.LogError("-----------------------GameObject Member Begin-----------------------");
		Debug.Log(this.GetType());
		count = 0;
		foreach (var item in _thisMembers)
		{
			Debug.Log(count++ + "\nName: " + item.Name + "\n ReflectedType:" + item.ReflectedType
				+ "\n DeclaringType:" + item.DeclaringType + "\n GetType:" + item.GetType() + "\n MemberType:" + item.MemberType + "\n ToString:" + item.ToString());
		}
		Debug.LogError("-----------------------GameObject Member End-----------------------");
	}


	private void PrintAttr()
	{
		_bastAttr = typeof(Button).GetProperties(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static); // 只能找到get/set方法
		_thisAttr = this.GetType().GetProperties(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static);
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
		EventInfo[] baseEventInfo = typeof(Reflection).GetEvents(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static); 
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

	private void PrintMethod()
	{
		// 6
		// Name: GetChild
		// ReflectedType:Reflection
		// DeclaringType:Reflection
		// GetType:System.Reflection.MonoMethod
		// MemberType:Method
		// ToString:UnityEngine.GameObject GetChild(System.String)


		MethodInfo[] baseMethodInfo = typeof(Reflection).GetMethods(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static);
		Debug.LogError("-----------------------Reflection Method Begin-----------------------");
		int count = 0;
		foreach (var item in baseMethodInfo)
		{
			Debug.Log(count + " ---------------------------------------------------------- begin");
			Debug.Log("\nName: " + item.Name + "\n ReflectedType:" + item.ReflectedType
				+ "\n DeclaringType:" + item.DeclaringType + "\n GetType:" + item.GetType() + "\n MemberType:" + item.MemberType + "\n ToString:" + item.ToString());


			ParameterInfo[] parameter = item.GetParameters();
			// 打印参数类型
			int parameterInfoCount = 0;
			foreach (ParameterInfo pi in parameter)
			{
				Debug.Log(parameterInfoCount++ + string.Format("   Parameter: Type={0}, Name={1}", pi.ParameterType, pi.Name));
			}
			// 打印返回类型
			Debug.Log("returnType " + item.ReturnType);
			Debug.Log(count + " ---------------------------------------------------------- end");
		}
		Debug.LogError("-----------------------Reflection Method End-----------------------");

		//MethodInfo[] thisMethodInfo = this.GetType().GetMethods(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static);
		//Debug.LogError("-----------------------GameObject Method Begin-----------------------");
		//Debug.Log(this.GetType());
		//count = 0;
		//foreach (var item in thisMethodInfo)
		//{
		//	Debug.Log(count++ + "\nName: " + item.Name + "\n ReflectedType:" + item.ReflectedType
		//		+ "\n DeclaringType:" + item.DeclaringType + "\n GetType:" + item.GetType() + "\n MemberType:" + item.MemberType + "\n ToString:" + item.ToString());
		//}
		//Debug.LogError("-----------------------GameObject Method End-----------------------");
	}
}

public class AttributeReflection : Reflection
{
	public float xxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxx;

	public delegate void SupportedProtocolClientEventHandler();

	// EventInfo https://blog.csdn.net/wf751620780/article/details/78592494
	public event SupportedProtocolClientEventHandler SupportedProtocolEvent;

	[EventMethod("onClick", "Button", typeof(Button))]
	public void OnClick_Button()
	{
		Debug.Log("Attribute Reflection " + gameObject.name);
	}
}

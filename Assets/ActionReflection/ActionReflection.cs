using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

// https://www.cnblogs.com/laoyur/archive/2011/04/14/2016025.html Invoke/BeginInvoke/DynamicInvoke

public class ActionReflection : MonoBehaviour {

	void Start () {
		Type actionType = typeof(Action<bool>);

		object param = actionType.Assembly.CreateInstance(actionType.Name); // 创建AddListen参数实例
		if (param == null)
		{
			Debug.Log("为什么不行!!!!!! " + actionType + "  " + actionType.Name + "  " + actionType.Assembly.FullName);
		}
		else
		{
			Debug.Log(param.ToString() + "   " + param.GetType());
		}

		

		MethodInfo[] Minfo = actionType.GetMethods(BindingFlags.Instance | BindingFlags.Public);
		foreach(var method in Minfo)
		{
			DebugMethod(method);
		}

		// https://docs.microsoft.com/zh-cn/dotnet/framework/reflection-and-codedom/how-to-hook-up-a-delegate-using-reflection
		// https://docs.microsoft.com/zh-cn/dotnet/api/system.delegate.createdelegate?view=netframework-4.7.2#System_Delegate_CreateDelegate_System_Type_System_Reflection_MethodInfo_
		var a = Delegate.CreateDelegate(actionType, Minfo[0]);

		// MissingMethodException: Method not found: 'Default constructor not found...ctor() of System.Action`1[[System.Boolean, mscorlib, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089]]'.
		// object reflectTest = Activator.CreateInstance(actionType);
	}

	private void DebugMethod(MethodInfo method)
	{
		#region 打印method
		Debug.Log(method.Name + "_______________________________________________________________________ begin");
		ParameterInfo[] parameter = method.GetParameters(); // 他妈的这里写成item了，难怪拿不到UnityEngine.Events.UnityAction`1[System.Boolean]
															// 打印参数类型
		int parameterInfoCount = 0;
		foreach (ParameterInfo pi in parameter)
		{
			// 0   Parameter: Type=UnityEngine.Events.UnityAction`1[System.Boolean], Name=call
			Debug.Log(parameterInfoCount++ + string.Format("   Parameter: Type={0}, Name={1}", pi.ParameterType, pi.Name));
		}

		// Name: AddListener
		// ReflectedType:UnityEngine.Events.UnityEvent`1[System.Boolean]
		// DeclaringType: UnityEngine.Events.UnityEvent`1[System.Boolean]
		// GetType: System.Reflection.MonoGenericMethod
		// MemberType:Method
		// ToString:Void AddListener(UnityEngine.Events.UnityAction`1[System.Boolean])
		// UnityEngine.Debug:Log(Object)
		Debug.Log("\nName: " + method.Name + "\n ReflectedType:" + method.ReflectedType
+ "\n DeclaringType:" + method.DeclaringType + "\n GetType:" + method.GetType() + "\n MemberType:" + method.MemberType + "\n ToString:" + method.ToString());
		Debug.Log(method.Name + "------------------------------------------------------------------------ end");
		#endregion
	}
}

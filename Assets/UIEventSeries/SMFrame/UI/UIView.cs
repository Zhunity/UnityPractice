using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using UnityEngine.Events;

namespace SMFrame
{
	public class UIView : MonoBehaviour
	{
		private void Awake()
		{
			RegistUIEvent();
		}

		/// <summary>
		/// 获取控件
		/// </summary>
		/// <param name="name"></param>
		/// <param name="type"></param>
		/// <returns></returns>
		public Component GetChild(string name, Type type)
		{
			GameObject obj = GetChild(name);
			if (obj != null)
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
			if (root == null)
			{
				return null;
			}

			if (string.IsNullOrEmpty(name))
			{
				return null;
			}

			if (root.name == name)
			{
				return root;
			}

			foreach (Transform child in root.transform)
			{
				var target = GetChild(child.gameObject, name);
				if (target != null)
				{
					return target;
				}
			}
			return null;
		}

		private const string CAN_NOT_FIND_MEMBER = "CAN NOT FIND MEMBER {0} ON GAMEOBJECT {1} IN COMPONENT {2}";

		/// <summary>
		/// 注册UI控件事件
		/// </summary>
		private void RegistUIEvent()
		{
			var thisMethodInfo = this.GetType().GetMethods(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static);
			foreach (var eventMethod in thisMethodInfo)
			{
				if (eventMethod.ReflectedType != eventMethod.DeclaringType)
				{
					continue;
				}
				var objList = eventMethod.GetCustomAttributes(typeof(UIEventAttribute), false);
				foreach (var attr in objList)
				{
					if (attr is UIEventAttribute)
					{
						UIEventAttribute exe = attr as UIEventAttribute;
						// 找到GameObjectName的控件，并在上面找到ComponentType脚本
						var control = GetChild(exe.GameObjectName, exe.ComponentType);
						// 在ComponentType脚本中找到事件名EventName
						var memberInfo = control.GetType().GetMember(exe.EventName);
						if(memberInfo == null)
						{
							Debug.LogError(string.Format(CAN_NOT_FIND_MEMBER, exe.EventName, exe.GameObjectName, exe.ComponentType.ToString()));
							return;
						}

						// 参考Practice 3月18日提交
						int i = 0;
						foreach (var member in memberInfo)
						{
							Debug.Log(i++ + "  "  + member.MemberType + "  " + member.Name);
							// 判断EventName类型，比如Button的onClick就是MemberTypes.Property类型
							if (member.MemberType == MemberTypes.Property)
							{
								PropertyInfo property = (PropertyInfo)member;
								// 在EventName事件对象中找到AddListener方法
								MethodInfo method = property.PropertyType.GetMethod("AddListener");
								// 将当前函数绑定到该控件的对应时间中
								UnityAction target = () => { eventMethod.Invoke(this, null); };
								method.Invoke(property.GetValue(control, null), new object[] { target });
							}
							// Toggle的onValuedChange是Field类型
							else if (member.MemberType == MemberTypes.Field)
							{
								FieldInfo field = (FieldInfo)member;
								MethodInfo listenerMethod = field.FieldType.GetMethod("AddListener");
								UnityAction<bool> target = (value) => { eventMethod.Invoke(this, new object[] { value }); };

								ParameterInfo[] listenerParameter = listenerMethod.GetParameters(); // AddListener只有一个参数
								Type paramType = listenerParameter[0].ParameterType;
								object param = paramType.Assembly.CreateInstance(listenerParameter[0].Name); // 创建AddListen参数实例
								if(param == null)
								{
									Debug.Log("为什么不行!!!!!!");
								}
								else
								{
									Debug.Log(param.ToString() + "   " + param.GetType());
								}

								// 1、怎么根据listenerParameter新建一个事件类型，如UnityAction<bool>
								// 2、怎么根据eventParameter，新建lambda表示式或者delegate之类的，并赋值给listenerMethod

								ParameterInfo[] eventParameter = eventMethod.GetParameters();
								object[] eventParams = new object[eventParameter.Length];

								DebugMethod(listenerMethod);


								DebugMethod(eventMethod);



								listenerMethod.Invoke(field.GetValue(control), new object[] { target });
							}
						}
						
					}
				}
			}
		}

		private void TestDelegate(Action fun)
		{
			if(fun != null)
			{
				fun();
			}
		}

		private void DebugMethod(MethodInfo method)
		{
#region 打印method
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
#endregion
		}
	}
}
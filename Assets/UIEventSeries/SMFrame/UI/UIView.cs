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
			foreach (var item in thisMethodInfo)
			{
				if (item.ReflectedType != item.DeclaringType)
				{
					continue;
				}
				var objList = item.GetCustomAttributes(typeof(UIEventAttribute), false);
				foreach (var attr in objList)
				{
					if (attr is UIEventAttribute)
					{
						UIEventAttribute exe = attr as UIEventAttribute;
						var control = GetChild(exe.GameObjectName, exe.ComponentType);
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
							if (member.MemberType == MemberTypes.Property)
							{
								PropertyInfo property = (PropertyInfo)member;
								MethodInfo method = property.PropertyType.GetMethod("AddListener");
								UnityAction target = () => { item.Invoke(this, null); };
								method.Invoke(property.GetValue(control, null), new object[] { target });
							}
							else if(member.MemberType == MemberTypes.Field)
							{
								FieldInfo field = (FieldInfo)member;
								MethodInfo method = field.FieldType.GetMethod("AddListener");
								ParameterInfo[] parameter = method.GetParameters();
								UnityAction target = () => { item.Invoke(this, null); };
								method.Invoke(field.GetValue(control), new object[] { target, false});
							}
						}
						
					}
				}
			}
		}
	}
}
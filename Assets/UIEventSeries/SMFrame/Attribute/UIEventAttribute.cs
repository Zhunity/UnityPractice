using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SMFrame
{
	/// <summary>
	/// 事件属性
	/// </summary>
	public class UIEventAttribute : Attribute
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

		public UIEventAttribute(string eventName, string gameObjectName, Type componentType)
		{
			this.EventName = eventName;
			this.GameObjectName = gameObjectName;
			this.ComponentType = componentType;
		}
	}
}

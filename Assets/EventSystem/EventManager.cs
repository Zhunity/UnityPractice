using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;



public static class EventManager
{
	/// <summary>
	/// 接收事件函数类型
	/// </summary>
	/// <param name="objes"></param>
	public delegate void Reciever(params object[] values);

	/// <summary>
	/// 事件派发器
	/// </summary>
	private static Dictionary<EventID, Reciever> m_dispatcher;

	public static void Start()
	{
		m_dispatcher = new Dictionary<EventID, Reciever>();
	}

	public static void Bind(EventID eventID, Reciever reciever)
	{
		if (reciever == null)
		{
			return;
		}

		Reciever currentReciever;
		if (m_dispatcher.TryGetValue(eventID, out currentReciever))
		{
			if(currentReciever == null)
			{
				currentReciever = reciever;
			}
			else
			{
				currentReciever += reciever;
			}
		}
		else
		{
			m_dispatcher.Add(eventID, reciever);
		}
	}

	public static void Unbind(EventID eventID, Reciever reciever)
	{
		if (reciever == null)
		{
			return;
		}

		Reciever currentReciever;
		if (m_dispatcher.TryGetValue(eventID, out currentReciever))
		{
			if(currentReciever == null)
			{
				return;
			}
			currentReciever -= reciever;
		}
	}

	public static void Send(EventID eventID, params object[] values)
	{
		Reciever reciever;
		if (!m_dispatcher.TryGetValue(eventID, out reciever))
		{
			return;
		}

		if (reciever == null)
		{
			return;
		}

		reciever(values);
	}

	public static void Clear()
	{
		m_dispatcher.Clear();
	}
}

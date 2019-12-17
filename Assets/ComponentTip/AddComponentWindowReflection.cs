using SMFrame;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEditor;
using UnityEngine;
using Object = System.Object;


public class Hello
{
	public event Action<Hello> windowClosed;
	public event Action<Hello> selectionChanged;
}


/// <summary>
/// InspectorWindow.AddComponentButton打开选择Component下拉框
/// UnityResourceReference AddComponentGUI是添加代码用的
/// AddComponentWindow.Show 打开
/// AddComponentWindow.OnItemSelected 目测时这个选中
/// AddComponentWindow.
/// </summary>
[ExecuteInEditMode]
public class AddComponentWindowReflection : MonoBehaviour
{
#if UNITY_EDITOR
	static Type window = NativeCodeReader.Instance.GetType("AddComponentWindow");
	static EventInfo selectionChanged = window.GetEvent("selectionChanged");
	static EventInfo windowClosed = window.GetEvent("windowClosed");

	private void Awake()
	{
		//object item = Activator.CreateInstance(window);
		//Type t = this.GetType();
		//var method = t.GetMethod("WindowClosed");
		//windowClosed.AddEventHandler(item, method.CreateDelegate(windowClosed.EventHandlerType, this));
		DestroyOnPlay();
		Debug.Log("EditorAwake");
		// 这个会在GameObject Apply时自动调用
		// 好像只会更新自己的GameObject
		PrefabUtility.prefabInstanceUpdated += Apply;

		// 感觉用Mono的Update频率比较慢
		EditorApplication.update += EditorUpdate;
	}

	/// <summary>
	/// PrefabUtility.prefabInstanceUpdated
	/// 这个会在GameObject Apply时自动调用
	/// 好像只会更新自己的GameObject
	/// </summary>
	/// <param name="go"></param>
	private void Apply(GameObject go)
	{
		Debug.Log("updated ", go);
		//PrefabUtility.GetAddedComponents(gameObject);
	}

	private bool first = false;
	private void EditorUpdate()
	{
		DestroyOnPlay();
		var windows = Resources.FindObjectsOfTypeAll(window);
		if (windows != null && windows.Length > 0)
		{
			if (first)
			{
				return;
			}
			first = true;
			Debug.Log(windowClosed + "\n" + windowClosed.EventHandlerType + "\n" + windowClosed.GetAddMethod());
			Debug.Log(selectionChanged + "\n" + selectionChanged.EventHandlerType + selectionChanged.GetAddMethod());
			foreach (var item in windows)
			{
				Type t = this.GetType();
				var method = t.GetMethod("WindowClosed");
				windowClosed.AddEventHandler(item, method.CreateDelegate(windowClosed.EventHandlerType, this));
				//selectionChanged.AddEventHandler(item, a);
				//parameters = new object[] { parameters };
				//return mi.Invoke(objClass, parameters);
				Debug.Log(item);
				Debug.Log(selectionChanged + "\n" + selectionChanged.GetAddMethod());
				Debug.Log(windowClosed + "\n" + windowClosed.GetAddMethod());
			}
		}
	}

	public void WindowClosed(Object para)
	{
		Debug.LogError("WindowClosed");
		first = false;
	}

	private void OnDestroy()
	{
		Debug.Log("destroy");
		PrefabUtility.prefabInstanceUpdated -= Apply;

		EditorApplication.update -= EditorUpdate;
	}

	private void DestroyOnPlay()
	{
		if (Application.isPlaying)
		{
			DestroyImmediate(gameObject);
			return;
		}
	}
#endif
}

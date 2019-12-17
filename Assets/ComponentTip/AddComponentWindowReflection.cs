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
	public event Action<int> windowClosed;
	public event Action<int> selectionChanged;
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
	static Type window = NativeCodeReader.Instance.GetType("Hello");
	static EventInfo selectionChanged = window.GetEvent("selectionChanged");
	static EventInfo windowClosed = window.GetEvent("windowClosed");

	private void Awake()
	{
		object item = Activator.CreateInstance(window);
		windowClosed.AddEventHandler(item, Delegate.CreateDelegate(typeof(Action<int>), this, "WindowClosed"));
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
		//var windows = Resources.FindObjectsOfTypeAll(window);
		//if (windows != null && windows.Length > 0)
		//{
		//	if (first)
		//	{
		//		return;
		//	}
		//	first = true;
		//	Debug.Log(windowClosed + "\n" + windowClosed.EventHandlerType);
		//	foreach (var item in windows)
		//	{
		//		windowClosed.AddEventHandler(item, Delegate.CreateDelegate(windowClosed.EventHandlerType, this, "WindowClosed"));
		//		//selectionChanged.AddEventHandler(item, a);
		//		//parameters = new object[] { parameters };
		//		//return mi.Invoke(objClass, parameters);
		//		Debug.Log(item);
		//	}
		//}
	}

	void WindowClosed(params object[] obj)
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

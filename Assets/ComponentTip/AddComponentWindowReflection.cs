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
	Type window = NativeCodeReader.Instance.GetType("AddComponentWindow");
	EventInfo selectionChanged;
	EventInfo windowClosed;
	Delegate closedDelegate;
	Delegate selectDelegate;

	private void Awake()
	{
		DestroyOnPlay();
		Debug.Log("EditorAwake");

		selectionChanged = window.GetEvent("selectionChanged");
		windowClosed = window.GetEvent("windowClosed");
		closedDelegate = CreateDelegate(windowClosed.EventHandlerType, "WindowClosed");
		selectDelegate = CreateDelegate(selectionChanged.EventHandlerType, "ItemSelect");
		

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
				
				windowClosed.AddEventHandler(item, closedDelegate);
				selectionChanged.AddEventHandler(item, selectDelegate);
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

	public void ItemSelect(Object para)
	{
		Debug.LogError("ItemSelect");
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

	private Delegate CreateDelegate(Type type, string name)
	{
		Type t = this.GetType();
		var method = t.GetMethod(name);
		return method.CreateDelegate(type, this);
	}
#endif
}

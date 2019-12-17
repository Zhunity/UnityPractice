using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor.SceneManagement;
using UnityEditor;
using SMFrame;
using System;

// The PrintAwake script is placed on a GameObject.  The Awake function is
// called when the GameObject is started at runtime.  The script is also
// called by the Editor.  An example is when the Scene is changed to a
// different Scene in the Project window.
// The Update() function is called, for example, when the GameObject transform
// position is changed in the Editor.
// https://docs.unity3d.com/ScriptReference/ExecuteInEditMode.html?_ga=2.214190871.1842191051.1572255433-604024978.1568602930


/// <summary>
/// InspectorWindow.AddComponentButton打开选择Component下拉框
/// UnityResourceReference AddComponentGUI是添加代码用的
/// AddComponentWindow.Show 打开
/// AddComponentWindow.OnItemSelected 目测时这个选中
/// AddComponentWindow.
/// </summary>
[ExecuteInEditMode]
public class OnAddComponent :MonoBehaviour
{

#if UNITY_EDITOR
	private void Awake()
	{
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
		Type window = NativeCodeReader.Instance.GetType("AddComponentWindow");
		var windows = Resources.FindObjectsOfTypeAll(window);
		if(windows != null && windows.Length > 0)
		{
			if(!first)
			{
				return;
			}
			first = true;
			foreach (var item in windows)
			{
				Debug.Log(item);
			}
		}
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

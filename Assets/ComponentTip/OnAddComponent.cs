using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor.SceneManagement;
using UnityEditor;

// The PrintAwake script is placed on a GameObject.  The Awake function is
// called when the GameObject is started at runtime.  The script is also
// called by the Editor.  An example is when the Scene is changed to a
// different Scene in the Project window.
// The Update() function is called, for example, when the GameObject transform
// position is changed in the Editor.
// https://docs.unity3d.com/ScriptReference/ExecuteInEditMode.html?_ga=2.214190871.1842191051.1572255433-604024978.1568602930
[ExecuteInEditMode]
public class OnAddComponent :MonoBehaviour
{

#if UNITY_EDITOR
	private void Awake()
	{
		Debug.Log("1111111111");
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

	private void EditorUpdate()
	{
		if(Application.isPlaying)
		{
			DestroyImmediate(gameObject);
			return;
		}
		Debug.Log("Update");
		//var list = PrefabUtility.GetAddedComponents(gameObject);
		//foreach (var item in list)
		//	Debug.Log(item);
	}

	private void OnDestroy()
	{
		Debug.Log("destroy");
		PrefabUtility.prefabInstanceUpdated -= Apply;

		EditorApplication.update -= EditorUpdate;
	}
#endif


}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor.SceneManagement;
using UnityEditor;

public class OnAddComponent :MonoBehaviour
{

#if UNITY_EDITOR
	private void Reset()
	{
		Debug.Log("1111111111");
		// 这个会在GameObject Apply时自动调用
		// 好像只会更新自己的GameObject
		PrefabUtility.prefabInstanceUpdated += Apply;

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
		PrefabUtility.GetAddedComponents(gameObject);
	}

	private void EditorUpdate()
	{
		var list = PrefabUtility.GetAddedComponents(gameObject);
		foreach (var item in list)
			Debug.Log(item);
	}

	private void OnDestroy()
	{
		PrefabUtility.prefabInstanceUpdated -= Apply;

		EditorApplication.update -= EditorUpdate;
	}
#endif


}

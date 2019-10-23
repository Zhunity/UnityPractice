using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;
using System.IO;
using System.Linq;
using Object = UnityEngine.Object;

public class Selelct
{
	// https://blog.csdn.net/w87580575/article/details/81061415
	[MenuItem("Assets/Select", false, 10)]
	public static void SelectAssets()
	{
		SimpleSelect();
	}

	/// <summary>
	/// unity自带遍历接口
	/// </summary>
	public static void SimpleSelect()
	{
		Object[] selects = Selection.GetFiltered(typeof(Object), SelectionMode.DeepAssets);
		foreach (Object selected in selects)
		{
			string path = AssetDatabase.GetAssetPath(selected);
			Debug.Log(path, selected);
		}
	}

	/// <summary>
	/// 遍历选中的对象
	/// </summary>
	public static void TraverseSelection()
	{
		// Selection.activeObject Selection.objects Selection.gameObjects 区别
		var objects = Selection.objects;
		for (int i = 0; i < objects.Length; i++)
		{
			TraverseDirectory(objects[i], "*.*", (item) =>
			{
				Object prefab = AssetDatabase.LoadAssetAtPath(item, typeof(Object));
				if (prefab != null)
				{
					Debug.Log(item, prefab);
				}
			});
		}
	}

	/// <summary>
	/// 遍历所选目录或文件，递归
	/// </summary>
	/// <param name="path"></param>
	/// <param name="exts"></param>
	/// <param name="callback"></param>
	public static void TraverseDirectory(Object obj, string exts, Action<string> callback)
	{
		if(obj == null)
		{
			return;
		}
		string path = AssetDatabase.GetAssetPath(obj);
		if (exts == null) exts = "";
		bool isAll = string.IsNullOrEmpty(exts) || exts == "*" || exts == "*.*";
		string[] extList = exts.Replace("*", "").Split('|');

		if (Directory.Exists(path))
		{
			// 如果选择的是文件夹
			string[] files = Directory.GetFiles(path, "*.*", SearchOption.AllDirectories).Where(file =>
			{
				if (isAll) return true;
				foreach (var ext in extList)
				{
					if (file.EndsWith(ext, StringComparison.OrdinalIgnoreCase))
					{
						return true;
					}
				}
				return false;
			}).ToArray();

			foreach (var item in files)
			{
				callback(item);
			}
		}
		else
		{
			if (isAll)
			{
				callback(path);
				return;
			}

			// 如果选择的是文件
			foreach (var ext in extList)
			{
				if (path.EndsWith(ext, StringComparison.OrdinalIgnoreCase))
				{
					callback(path);
					break;
				}
			}
		}
	}

	/// <summary>
	/// 遍历GameObject下的子节点
	/// </summary>
	public static void TraverseChildren()
	{

	}
}

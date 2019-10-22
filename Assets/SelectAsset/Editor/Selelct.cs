using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;
using System.IO;
using System.Linq;

public class Selelct
{
	[MenuItem("Assets/Select", false, 10)]
	public static void SelectAssets()
	{
		var prefabs = Selection.gameObjects;
		for (int i = 0; i < prefabs.Length; i++)
		{
			Debug.Log("select", prefabs[i]);
		}
	}

	// 遍历所选目录或文件，递归
	public static void Walk(string path, string exts, Action<string> callback)
	{
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
}

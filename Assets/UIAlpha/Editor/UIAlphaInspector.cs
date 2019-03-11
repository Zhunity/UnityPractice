using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

// 参考 https://www.xuanyusong.com/archives/3406

[CustomEditor(typeof(UIAlpha))]
public class UIAlphaInspector : Editor
{
	UIAlpha model;
	public override void OnInspectorGUI()
	{
		model = target as UIAlpha;
		float alpha = EditorGUILayout.FloatField("Alpha", model.Alpha);
		if (model.Alpha != alpha)
		{
			model.Alpha = alpha;
		}
		base.DrawDefaultInspector();
	}
}
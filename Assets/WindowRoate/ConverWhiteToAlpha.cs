using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;

public class ConverWhiteToAlpha
{
	[MenuItem("Assets/png/ConverWhiteToAlpha")]
    public static void GeneratePNG()
	{
		Object obj = Selection.activeObject;
		string path = AssetDatabase.GetAssetPath(obj);
		var tex = AssetDatabase.LoadAssetAtPath<Texture2D>(path);
		Texture2D t = new Texture2D(tex.width, tex.height);
		for (int i = 0; i < tex.width; i ++)
		{
			for(int j = 0; j <tex.height; j ++)
			{
				var color = tex.GetPixel(i, j);
				if(approximately(color.r, 1) && approximately(color.g, 1) && approximately(color.b, 1))
				{
					color.a = 0;
					
				}
				t.SetPixel(i, j, color);
			}
		}
		
		t.Apply();
		byte[] bytes = t.EncodeToPNG();
		File.WriteAllBytes(Application.dataPath + "/tttt.png", bytes);
	}

	private static bool approximately(float a, float b, float t = 0.2f)
	{
		return Mathf.Abs(a - b) <= t;
	}
}

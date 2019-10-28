using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnAddComponent : UnityEditor.SceneManagement.AddedComponent
{

	public int a = 1;
#if UNITY_EDITOR
	public override void Apply(string prefabAssetPath)
	{
		Debug.Log("111111111111111 " + prefabAssetPath + "  " + instanceComponent);
	}
#endif
}

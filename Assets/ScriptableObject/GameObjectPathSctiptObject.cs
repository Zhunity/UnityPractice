using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class GameObjectPathSctiptObject : ScriptableObject
{
	public GameObject hi;


	[MenuItem("Tools/CreateConfig")]
	 private static void Create()
	{
		var sobj = ScriptableObject.CreateInstance<GameObjectPathSctiptObject>();
		GameObject go = Selection.activeGameObject;
		sobj.hi = go;
		AssetDatabase.CreateAsset(sobj, "Assets/ScriptableObject/GameObjectPath.asset");
	} 

}

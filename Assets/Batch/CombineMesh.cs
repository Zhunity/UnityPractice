using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class CombineMesh : Editor
{
	//https://www.jianshu.com/p/9d0f14cd9204
	[MenuItem("ModelTools/将多个物体合并为一个物体")]
	static void CombineMeshs2()
	{
		//在编辑器下选中的所有物体
		object[] objs=Selection.gameObjects;
		if (objs.Length<=0)
			return;
		//网格信息数组
		MeshFilter[] meshFilters =new MeshFilter[objs.Length];
		//渲染器数组
		MeshRenderer[] meshRenderers =new MeshRenderer[objs.Length];
		//合并实例数组
		CombineInstance[] combines =new CombineInstance[objs.Length];
		//材质数组
		Material[] mats =new Material[objs.Length];
		for (int i =0; i < objs.Length; i++)
		{
			//获取网格信息
			meshFilters[i]=((GameObject)objs[i]).GetComponent<MeshFilter>();
			//获取渲染器
			meshRenderers[i]=((GameObject)objs[i]).GetComponent<MeshRenderer>();
			//获取材质
			mats[i] = meshRenderers[i].sharedMaterial;
			//合并实例
			combines[i].mesh = meshFilters[i].sharedMesh;
			combines[i].transform = meshFilters[i].transform.localToWorldMatrix;
		}
		//创建新物体
		GameObject go =new GameObject();
		go.name ="CombinedMesh_"+ ((GameObject)objs[0]).name;
		//设置网格信息
		MeshFilter filter =go.transform.GetComponent<MeshFilter>();
		if (filter == null)
			filter =go.AddComponent<MeshFilter>();
		filter.sharedMesh =new Mesh();

		/*
		 * 这段脚本的核心是CombineMeshes()方法
		 * 该方法有三个参数，第一个参数是合并实例的数组，第二个参数是是否对子物体的网格进行合并，第三个参数是是否共享材质
		 * 如果希望物体共享材质则第三个参数为true，否则为false。
		 * 在我测试的过程中发现，如果选择了对子物体的网格进行合并，那么每个子物体都不能再使用单独的材质，默认会以第一个材质作为合并后物体的材质
		 */
		filter.sharedMesh.CombineMeshes(combines,false);
		//设置渲染器
		MeshRenderer render =go.transform.GetComponent<MeshRenderer>();
		if (render == null)
			render =go.AddComponent<MeshRenderer>();
		//设置材质
		render.sharedMaterials = mats;
	}

	[MenuItem("GameObject/SetActive %#d", false, 10)]
	public static void SetActive()
	{
		GameObject[] goList = Selection.gameObjects;
		foreach(GameObject go in goList)
		{
			go.SetActive(!go.activeSelf);
		}
	}
}

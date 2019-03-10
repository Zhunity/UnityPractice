using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ListSort : MonoBehaviour
{
	/// <summary>
	/// List.Sort 判断为正时交换两个位置
	/// </summary>
	public List<int> list;

	private void OnGUI()
	{
		// 升序排列
		if (GUI.Button(new Rect(0, 0, 100, 50), "AscendSort"))
		{
			list.Sort(AscendSort);
			DumpList();
		}

		// 降序排列
		if (GUI.Button(new Rect(200, 0, 100, 50), "DescendSort"))
		{
			list.Sort(DescendSort);
			DumpList();
		}
	}

	private void DumpList()
	{
		Debug.Log("-------------------------------------------------------------");
		foreach (var item in list)
		{
			Debug.Log(item);
		}
		Debug.Log("-------------------------------------------------------------");
	}

	/// <summary>
	/// 升序排列
	/// </summary>
	/// <param name="first"></param>
	/// <param name="next"></param>
	/// <returns></returns>
	private int AscendSort(int first, int next)
	{
		return first.CompareTo(next);
	}

	/// <summary>
	/// 降序排列
	/// </summary>
	/// <param name="first"></param>
	/// <param name="next"></param>
	/// <returns></returns>
	private int DescendSort(int first, int next)
	{
		return next.CompareTo(first);
	}
}

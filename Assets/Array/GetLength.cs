using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GetLength : MonoBehaviour
{
	public int row; // 行
	public int col;	// 列
	

	private void Start()
	{
		//int[][] array = new int[row][1](); 这样会报错
		int[,] array = new int[row, col];

		Debug.LogFormat("GetLength(0):{0}\tLength:{1}", array.GetLength(0), array.Length);
		// row:5 col:10 
		// GetLength(0):5	Length:50
		// GetLength(0) = row	Length = row * col
	}
}

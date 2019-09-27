using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GetLength : MonoBehaviour
{
	public int row; // 行
	public int col; // 列
	public int dimensio;    // 维(构成空间的因素); 尺寸; 规模; 程度; 范围; 方面; 侧面;



	private void Start()
	{
		//int[][] array = new int[row][1](); 这样会报错
		int[,] array = new int[row, col];

		Debug.LogFormat("GetLength({0}):{1}\tLength:{2}", dimensio, array.GetLength(dimensio), array.Length);
		// row:5 col:10  dimensio:0
		// GetLength(0):5	Length:50
		// GetLength(0) = row	Length = row * col

		// row:5 col:10  dimensio:1
		// GetLength(1):10	Length:50
		// GetLength(1) = col	Length = row * col
	}
}

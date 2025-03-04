﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// https://blog.csdn.net/weixin_38884324/article/details/79284373
/// </summary>
public class Compute : MonoBehaviour
{
	public ComputeShader shader;

	struct Data
	{
		public float A;
		public float B;
		public float C;
	}

    // Start is called before the first frame update
    void Start()
    {
		Data[] inputData = new Data[3];
		Data[] outputData = new Data[3];
		for(int i = 0; i < inputData.Length; i ++)
		{
			inputData[i].A = i;
			inputData[i].B = i * 3;
			inputData[i].C = i + 2;
		}

		// Data 有3個float，一個 float 有 4 Byte，所以 3 * 4 = 12
		ComputeBuffer inputBuffer = new ComputeBuffer(inputData.Length, 12);
		ComputeBuffer outputBuffer = new ComputeBuffer(outputData.Length, 12);
		int k = shader.FindKernel("CSMain");

		// 写入GPU
		inputBuffer.SetData(inputData);
		shader.SetBuffer(k, "inputData", inputBuffer);

		// 计算，并输出至CPU
		shader.SetBuffer(k, "outputData", outputBuffer);
		shader.Dispatch(k, outputData.Length, 1, 1);
		outputBuffer.GetData(outputData);

		// 打印结果
		for(int i = 0; i < outputData.Length; i ++)
		{
			Debug.Log(outputData[i].A + ", " + outputData[i].B + ", " + outputData[i].C);
		}

		// 释放
		inputBuffer.Dispose();
		outputBuffer.Dispose();
    }
}

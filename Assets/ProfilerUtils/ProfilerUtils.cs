using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Profiling;

public class ProfilerUtils : MonoBehaviour {

	void OnGUI()
	{
		// https://blog.csdn.net/fansongy/article/details/51025325
		GUILayout.TextField("Total DrawCall: " + UnityStats.drawCalls);
		GUILayout.TextField("Batch: " + UnityStats.batches);
		GUILayout.TextField("Static Batch DC: " + UnityStats.staticBatchedDrawCalls);
		GUILayout.TextField("Static Batch: " + UnityStats.staticBatches);
		GUILayout.TextField("DynamicBatch DC: " + UnityStats.dynamicBatchedDrawCalls);
		GUILayout.TextField("DynamicBatch: " + UnityStats.dynamicBatches);
		GUILayout.TextField("Tri: " + UnityStats.triangles);
		GUILayout.TextField("Ver: " + UnityStats.vertices);

		// https://www.xuanyusong.com/archives/4263
		// ShowFPS
		GUILayout.TextField("GetTotalAllocatedMemoryLong: " + Profiler.GetTotalAllocatedMemoryLong() / 1024 / 1024);
		GUILayout.TextField("GetMonoUsedSizeLong: " + Profiler.GetMonoUsedSizeLong() / 1024 / 1024);
		GUILayout.TextField("GetRuntimeMemorySizeLong: " + Profiler.GetRuntimeMemorySizeLong(this));
		GUILayout.TextField("GetMonoHeapSizeLong: " + Profiler.GetMonoHeapSizeLong() / 1024 / 1024);
	}
}

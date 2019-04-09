using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

// https://blog.csdn.net/fansongy/article/details/51025325

public class ProfilerUtils : MonoBehaviour {

	void OnGUI()
	{
		GUILayout.TextField("Total DrawCall: " + UnityStats.drawCalls);
		GUILayout.TextField("Batch: " + UnityStats.batches);
		GUILayout.TextField("Static Batch DC: " + UnityStats.staticBatchedDrawCalls);
		GUILayout.TextField("Static Batch: " + UnityStats.staticBatches);
		GUILayout.TextField("DynamicBatch DC: " + UnityStats.dynamicBatchedDrawCalls);
		GUILayout.TextField("DynamicBatch: " + UnityStats.dynamicBatches);
		GUILayout.TextField("Tri: " + UnityStats.triangles);
		GUILayout.TextField("Ver: " + UnityStats.vertices);
	}
}

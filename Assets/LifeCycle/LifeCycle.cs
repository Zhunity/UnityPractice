using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 1、把脚本挂载上去的时候，会调用构造函数和Reset
/// 2、运行的时候会调用两次构造函数
/// 3、游戏结束的时候，还会调用一次构造函数？？？？
/// 4、编辑器上在游戏时，构造两次-》Awake->OnEnable->Pause false->Focus true->Start
/// </summary>
public class LifeCycle : MonoBehaviour
{

	public LifeCycle()
	{
		Debug.Log("Ctor");
	}

	#region Editor
	/// <summary>
	/// Reset is called in the Editor when the script is attached or reset
	/// </summary>
	private void Reset()
	{
		Debug.Log("Reset");
	}
	#endregion

	#region Initialization
	public void Awake()
	{
		Debug.Log("Awake");
	}

	public void OnEnable()
	{
		Debug.Log("OnEnable");
	}

	// Start is called before the first frame update
	void Start()
    {
		Debug.Log("Start");
	}
	#endregion

	#region Physics
	#endregion

	#region Input Events
	#endregion

	#region Game Logic
	// Update is called once per frame
	void Update()
    {
		//Debug.Log("Update");
	}
	#endregion

	#region Scene Rendering
	#endregion

	private void OnApplicationPause(bool pause)
	{
		Debug.Log("OnApplicationPause " + pause);
	}

	private void OnApplicationFocus(bool focus)
	{
		Debug.Log("OnApplicationFocus " + focus);
	}

	private void OnDisable()
	{
		Debug.Log("OnDisable");
	}

	private void OnApplicationQuit()
	{
		Debug.Log("OnApplicationQuit");
	}
}

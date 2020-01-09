using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisposableClass : IDisposable
{
	public int id;

	public DisposableClass(int i)
	{
		id = i;
		Debug.Log(id + " Disposable Create");
	}

	/// <summary>
	/// 只有class才有析构函数
	/// </summary>
	~DisposableClass()
	{
		id = -1;
		Debug.Log(id + "  Disposable Destroy");
	}

	public void Dispose()
	{
		id = -2;
		Debug.Log(id + "  Disposable Dispose");
	}
}

public class DiposeClassTest : MonoBehaviour
{
	int id = 1;
	DisposableClass d;

	void Start()
	{
		d = new DisposableClass(0);
		Debug.Log(d.id + " before");
		assignment();
		Debug.Log(d.id + " after");
	}

	// Update is called once per frame
	void Update()
	{

	}

	/// <summary>
	/// 
	/// </summary>
	private void assignment()
	{
		DisposableClass temp = d;
		temp.id = id++;
		temp.Dispose(); // **
	}

	/// <summary>
	/// 还是要手动调用Dispose
	/// </summary>
	/// <param name="info"></param>
	private void RefReturn(ref DisposableClass info)
	{
		//DisposableClass temp = info;
		//temp.id = id++;
		//info.Dispose();
		info = new DisposableClass(id++);
		//temp.Dispose();
	}
}

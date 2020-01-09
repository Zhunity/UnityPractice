using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using UnityEngine;

public struct DisposableStruct : IDisposable
{
	public int id;

	public DisposableStruct(int i)
	{
		id = i;
		Debug.Log(id + " Disposable Create");
	}

	/// <summary>
	/// 只有class才有析构函数
	/// </summary>
	//~DisposableStruct()
	//{
	//	Debug.Log("Disposable Destroy");
	//}

	public void Dispose()
	{
		Debug.Log(id + "  Disposable Dispose");
	}
}

public class DiposeStructTest : MonoBehaviour
{
	int id = 1;
	DisposableStruct d;

	void Start()
    {
		d = new DisposableStruct(0);
		Debug.Log(d.id + " before");
		RefReturn(ref d);
		Debug.Log(d.id + " after");
	}

    // Update is called once per frame
    void Update()
    {
        
    }

	/// <summary>
	/// struct类型的IDisposable，如果给另外一个赋值了，在另一个对象无用之后，需要手动Dispose
	/// </summary>
	private void assignment()
	{
		DisposableStruct temp = d;
		temp.id = id ++;
		temp.Dispose(); // **
	}

	private void RefReturn(ref DisposableStruct info)
	{
		DisposableStruct temp = info;
		temp.id = id++;
		info.Dispose();
		info = new DisposableStruct(id++);
		temp.Dispose();
	}
}

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class DictionaryGet : MonoBehaviour {
	void Start () {
		Dictionary<int, TestClass> dict = new Dictionary<int, TestClass>();

		dict[1] = new TestClass(1);
		dict.Add(2, null); // null 可以添加到Dictionary里面，并且可以取出来
		dict.Add(3, new TestClass());
		Debug.Log("Count:" + dict.Count);

		try
		{
			var info = dict[2];
			PrintInfo(info, "dict[2]:  ");

			Debug.Log("dict.ContainsKey(2):    " + dict.ContainsKey(2).ToString()); // 可以找得到空的

			// ??????
			// class 类型不是可以传下去吗？怎么构造函数、传参进去都没用？？
			TestClass testConstructor = new TestClass(dict[2]);
			testConstructor.PrintTestClass("dict[2] change before testConstructor: ");
			TestClass testMethod = new TestClass();
			testMethod.SetTestClass(dict[2]);
			testConstructor.PrintTestClass("dict[2] change before testMethod: ");
			dict[2] = new TestClass(100);
			PrintInfo(info, "change info  ");
			PrintInfo(dict[2], "change dict[2]  ");
			testConstructor.PrintTestClass("dict[2] change after testConstructor: ");
			testConstructor.PrintTestClass("dict[2] change after testMethod: ");
		}
		catch(Exception e)
		{
			Debug.LogError(e.ToString());
		}
	}

	private void PrintInfo(TestClass info, string desc)
	{
		if (info == null)
		{
			Debug.Log(desc + "null");
		}
		else
		{
			Debug.Log(desc + info.ToString());
		}
	}
}

// C# 中的结构类型（struct）
// https://blog.csdn.net/seattle1215/article/details/6672903
public struct TestStruct
{
	int data;// = 9999; Assets/DictionaryGet/DictionaryGet.cs(48,2): error CS0573: 'TestStruct': Structs cannot have instance property or field initializers
	// TestStruct testStruct1; Assets/DictionaryGet/DictionaryGet.cs(49,13): error CS0523: Struct member `TestStruct.testStruct1' of type `TestStruct' causes a cycle in the struct layout
	TestClass testClass;

	// Assets/DictionaryGet/DictionaryGet.cs(52,9): error CS0568: Structs cannot contain explicit parameterless constructors
	//public TestStruct()
	//{
	//}
}

public class TestClass
{
	int data = -9999;
	TestStruct testStruct;
	TestClass testClass;

	public TestClass()
	{

	}

	public TestClass(int i)
	{
		data = i;
	}

	/// <summary>
	/// Assets/DictionaryGet/DictionaryGet.cs(76,3): error CS1604: Cannot assign to `this' because it is read-only
	/// </summary>
	/// <param name="testClass"></param>
	//public TestClass(TestClass testClass)
	//{
	//	this = testClass;
	//}

	public TestClass(TestClass testClass)
	{
		this.testClass = testClass;
	}

	public TestClass(TestStruct testStruct)
	{
		this.testStruct = testStruct;
	}

	public TestClass(TestClass testClass, TestStruct testStruct)
	{
		this.testClass = testClass;
		this.testStruct = testStruct;
	}

	public void SetTestClass(TestClass testClass)
	{
		this.testClass = testClass;
	}

	public void PrintTestClass(string desc = "")
	{
		if (this.testClass == null)
		{
			Debug.Log(desc + "null");
		}
		else
		{
			Debug.Log(desc + this.testClass.ToString());
		}
	}

	public void PrintTestStruct(string desc)
	{
		// Assets/DictionaryGet/DictionaryGet.cs(103,12): error CS0019: Operator `==' cannot be applied to operands of type `TestStruct' and `null'
		//if (this.testStruct == null)
		//{
		//	Debug.Log(desc + "null");
		//}
		//else
		//{
		Debug.Log(desc + this.testStruct.ToString());
		//}
	}

	public override string ToString()
	{
		// 判断自身为空并没有什么卵用
		if(this == null)
		{
			return "null";
		}
		else
		{
			return "data:" + data;
		}
		
	}
}

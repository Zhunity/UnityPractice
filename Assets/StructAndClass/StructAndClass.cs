using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// https://www.cnblogs.com/Christal-R/p/7400332.html
/// 值类型 引用类型区别
/// </summary>
public class StructAndClass : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
		TestObjectClass();
	}

	/// <summary>
	/// class: 引用类型（reference type）
	/// </summary>
	private void TestObjectClass()
	{
		TestClass origin = new TestClass(100);
		List<TestClass> list = new List<TestClass>();
		list.Add(origin);

		// --------------------------------赋值------------------------------------------
		Debug.Log("--------------------------------赋值 begin------------------------------------------");
		PrintInfo(origin, "change origin before:  ");

		// 测试直接赋值
		TestClass testEqual = origin;
		PrintInfo(testEqual, "origin change before testEqual: ");

		// 测试构造函数
		TestClass testConstructor = new TestClass(origin);
		testConstructor.PrintTestClass("origin change before testConstructor: ");

		// 测试函数赋值
		TestClass testMethod = new TestClass();
		testMethod.SetTestClass(origin);
		testMethod.PrintTestClass("origin change before testMethod: ");

		// 测试容器类型
		PrintInfo(list[0], "origin change before list[0]: ");

		// 测试struct中的类
		TestStruct tStruct = new TestStruct(origin);
		tStruct.PrintTestClass("origin change before tStruct.testClass: ");
		Debug.Log("--------------------------------赋值 end------------------------------------------");
		// --------------------------------赋值------------------------------------------
		origin.SetData(10);

		// --------------------------------改data之后------------------------------------------	
		Debug.Log("--------------------------------改data之后 begin------------------------------------------");
		PrintInfo(origin, "change origin after:  ");
		PrintInfo(testEqual, "change origin testEqual:  ");// 测试直接赋值
		testConstructor.PrintTestClass("origin change after testConstructor: ");// 测试构造函数
		testConstructor.PrintTestClass("origin change after testMethod: ");// 测试函数赋值
		PrintInfo(list[0], "origin change after list[0]: ");// 测试容器类型
		tStruct.PrintTestClass("origin change after tStruct.testClass: ");// 测试struct中的类
		Debug.Log("--------------------------------改data之后 end------------------------------------------");
		// --------------------------------改data之后------------------------------------------

		testEqual.SetData(99);
		// --------------------------------改testEqual之后------------------------------------------
		Debug.Log("--------------------------------改testEqual之后 begin------------------------------------------");
		PrintInfo(origin, "change testEqual after:  ");
		PrintInfo(testEqual, "change testEqual testEqual:  ");// 测试直接赋值
		testConstructor.PrintTestClass("testEqual change after testConstructor: ");// 测试构造函数
		testConstructor.PrintTestClass("testEqual change after testMethod: ");// 测试函数赋值
		PrintInfo(list[0], "testEqual change after list[0]: ");// 测试容器类型
		tStruct.PrintTestClass("testEqual change after tStruct.testClass: ");// 测试struct中的类
		Debug.Log("--------------------------------改testEqual之后 end------------------------------------------");
		// --------------------------------改testEqual之后------------------------------------------
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
	TestClass testClass; // struct 默认也是private?

	// Assets/DictionaryGet/DictionaryGet.cs(52,9): error CS0568: Structs cannot contain explicit parameterless constructors
	//public TestStruct()
	//{
	//}

	// 带个没用的参数就可以了。。。。
	public TestStruct(string str)
	{
		data = 9999;
		testClass = new TestClass();
	}

	public TestStruct(int num)
	{
		data = num;
		testClass = new TestClass(num);
	}

	public TestStruct(TestClass info)
	{
		data = info.GetData();
		testClass = info;
	}

	public void PrintTestClass(string desc = "")
	{
		if (this.testClass == null)
		{
			Debug.Log(desc + "null  data: " + data);
		}
		else
		{
			Debug.Log(desc + this.testClass.ToString()  +" data: " + data);
		}
	}
}

public class TestClass
{
	// 默认private
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

	public void SetData(int num)
	{
		data = num;
	}

	public int GetData()
	{
		return data;
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
		if (this == null)
		{
			return "null";
		}
		else
		{
			return "data:" + data;
		}

	}
}


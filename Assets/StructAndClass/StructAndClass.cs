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
		//TestObjectClass();
		TestValueStruct();
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
		PrintTestClass(origin, "change origin before:  ");

		// 测试直接赋值
		TestClass testEqual = origin;
		PrintTestClass(testEqual, "origin change before testEqual: ");

		// 测试构造函数
		TestClass testConstructor = new TestClass(origin);
		testConstructor.PrintTestClass("origin change before testConstructor: ");

		// 测试函数赋值
		TestClass testMethod = new TestClass();
		testMethod.SetTestClass(origin);
		testMethod.PrintTestClass("origin change before testMethod: ");

		// 测试容器类型
		PrintTestClass(list[0], "origin change before list[0]: ");

		// 测试struct中的类
		TestStruct tStruct = new TestStruct(origin);
		tStruct.PrintTestClass("origin change before tStruct.testClass: ");
		Debug.Log("--------------------------------赋值 end------------------------------------------");
		// --------------------------------赋值------------------------------------------
		origin.SetData(10);

		// --------------------------------改data之后------------------------------------------	
		Debug.Log("--------------------------------改data之后 begin------------------------------------------");
		PrintTestClass(origin, "change origin after:  ");
		PrintTestClass(testEqual, "change origin testEqual:  ");// 测试直接赋值
		testConstructor.PrintTestClass("origin change after testConstructor: ");// 测试构造函数
		testConstructor.PrintTestClass("origin change after testMethod: ");// 测试函数赋值
		PrintTestClass(list[0], "origin change after list[0]: ");// 测试容器类型
		tStruct.PrintTestClass("origin change after tStruct.testClass: ");// 测试struct中的类
		Debug.Log("--------------------------------改data之后 end------------------------------------------");
		// --------------------------------改data之后------------------------------------------

		testEqual.SetData(99);
		// --------------------------------改testEqual之后------------------------------------------
		Debug.Log("--------------------------------改testEqual之后 begin------------------------------------------");
		PrintTestClass(origin, "change testEqual after:  ");
		PrintTestClass(testEqual, "change testEqual testEqual:  ");// 测试直接赋值
		testConstructor.PrintTestClass("testEqual change after testConstructor: ");// 测试构造函数
		testConstructor.PrintTestClass("testEqual change after testMethod: ");// 测试函数赋值
		PrintTestClass(list[0], "testEqual change after list[0]: ");// 测试容器类型
		tStruct.PrintTestClass("testEqual change after tStruct.testClass: ");// 测试struct中的类
		Debug.Log("--------------------------------改testEqual之后 end------------------------------------------");
		// --------------------------------改testEqual之后------------------------------------------
	}

	private void TestValueStruct()
	{
		TestStructConstructer();
		TestStructNull();

		

		TestStruct origin = new TestStruct(100);
		List<TestStruct> list = new List<TestStruct>();
		list.Add(origin);

		// --------------------------------赋值------------------------------------------
		Debug.Log("--------------------------------赋值 begin------------------------------------------");
		PrintTestStruct(origin, "change origin before:  ");

		// 测试直接赋值
		TestStruct testEqual = origin;
		PrintTestStruct(testEqual, "origin change before testEqual: ");

		// 测试容器类型
		PrintTestStruct(list[0], "origin change before list[0]: ");
		Debug.Log("--------------------------------赋值 end------------------------------------------");
		// --------------------------------赋值------------------------------------------
		origin.SetData(10);

		// --------------------------------改data之后------------------------------------------	
		Debug.Log("--------------------------------改data之后 begin------------------------------------------");
		PrintTestStruct(origin, "change origin after:  ");
		PrintTestStruct(testEqual, "change origin testEqual:  ");// 测试直接赋值
		PrintTestStruct(list[0], "origin change after list[0]: ");// 测试容器类型
		Debug.Log("--------------------------------改data之后 end------------------------------------------");
		// --------------------------------改data之后------------------------------------------

		testEqual.SetData(99);
		// --------------------------------改testEqual之后------------------------------------------
		Debug.Log("--------------------------------改testEqual之后 begin------------------------------------------");
		PrintTestStruct(origin, "change testEqual after:  ");
		PrintTestStruct(testEqual, "change testEqual testEqual:  ");// 测试直接赋值
		PrintTestStruct(list[0], "testEqual change after list[0]: ");// 测试容器类型
		Debug.Log("--------------------------------改testEqual之后 end------------------------------------------");
		// --------------------------------改testEqual之后------------------------------------------
	}

	/// <summary>
	/// 测试struct的构造函数
	/// </summary>
	private void TestStructConstructer()
	{
		TestStruct testConstructor; // 结构将不会被初始化，但是也不能访问。
									//PrintTestStruct(testConstructor, "no new testConstructor"); // Assets/StructAndClass/StructAndClass.cs(80,19): error CS0165: Use of unassigned local variable `testConstructor'

		testConstructor = new TestStruct();
		//PrintTestStruct(testConstructor, "new testConstructor"); 
		// 尚未初始化testClass
		// NullReferenceException: Object reference not set to an instance of an object
		// estStruct.ToString()(at Assets / StructAndClass / StructAndClass.cs:193)
		// StructAndClass.PrintTestStruct(TestStruct info, System.String desc)(at Assets / StructAndClass / StructAndClass.cs:136)
		// StructAndClass.TestValueStruct()(at Assets / StructAndClass / StructAndClass.cs:83)
		// StructAndClass.Start()(at Assets / StructAndClass / StructAndClass.cs:15)

		testConstructor = new TestStruct(new TestClass(100));
		PrintTestStruct(testConstructor, "new testConstructor TestClass Param");

		SimpleTestStruct simple = new SimpleTestStruct();
		Debug.Log(simple.ToString()); // 默认赋值
	}

	/// <summary>
	/// 测试struct与null的关系
	/// </summary>
	private void TestStructNull()
	{
		//SimpleTestStruct strcutNull = null; // Assets/StructAndClass/StructAndClass.cs(98,33): error CS0037: Cannot convert null to `SimpleTestStruct' because it is a value type

		// SimpleTestStruct simple = new SimpleTestStruct();
		// simple = null; 不能赋空值, 报错同上

		//SimpleTestStruct simple;
		//simple = null; 不能赋空值, 报错同上

		SimpleTestStruct? strcutNull = null; // V struct 赋空值的正确做法
		Debug.Log(strcutNull.ToString());  // 可以打印，但里面什么也没有
		//if(strcutNull == null)
		//{
		//  可以赋空值，但是不能判空？
		//	Assets / StructAndClass / StructAndClass.cs(99, 12): error CS0037: Cannot convert null to `SimpleTestStruct' because it is a value type
		//}
		strcutNull = new SimpleTestStruct();
		Debug.Log(strcutNull.ToString()); // 默认赋值

		strcutNull = null;
		Debug.Log(strcutNull.ToString());  // 可以再次赋空值

		// TODO 测试strcutNull大小
	}

	private void PrintTestClass(TestClass info, string desc)
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

	private void PrintTestStruct(TestStruct info, string desc)
	{
		Debug.Log(desc + info.ToString());
	}
}


public struct SimpleTestStruct
{
	public int data; // 默认0
	public string str; // 默认null

	public override string ToString()
	{
		return "data: " + data + " str: " + str + " str is null or empty:" + string.IsNullOrEmpty(str) + " str is empty: " + (str == string.Empty).ToString() 
			+ " str is null: " + (str == null).ToString();// + " str is null or white space: " + String.IsNullOrWhiteSpace(str).ToString();
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

	public void SetData(int num)
	{
		data = num;
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

	public override string ToString()
	{
		return "data: " + data + "  testClass" + testClass.ToString();
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


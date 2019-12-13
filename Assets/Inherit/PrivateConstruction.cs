using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrivateParent
{
	//2、private改成protected之后，可以在子类访问了，但是子类的构造函数公开性无所谓
	protected PrivateParent()
	{
		Debug.Log("PrivateParent");
	}

	// 11、私有成员不可以用virtual
	private void ParentPrivate()
	{
		Debug.Log("function ParentPrivate");
	}

	protected virtual void ParentProtected()
	{
		Debug.Log("ParentProtected");
	}

	public void ParentPublic()
	{
		Debug.Log("ParentPublic");
	}
}

// 4、private PrivateParent()时，看起来不可以被继承，因为调用不了父类的构造函数
public class PublicChild : PrivateParent
{
	// 1、private PrivateParent()时，提示构造函数有问题，PrivateParent.PrivateParent不可访问
	// 6、理所当然的会调用父类的构造函数
	public PublicChild()
	{
		Debug.Log("PublicChild");
	}

	public PublicChild(int num) : base()
	{
		Debug.Log("PublicChild call PrivateParent");
	}

	protected PublicChild(string str)
	{
		Debug.Log("PublicChild get str");
	}

	// 7、这样是可以的
	public void ParentPrivate()
	{
		Debug.Log("PublicChild call ParentPrivate");
	}

	// 8、public override void ParentProtected() 要在父类标记vitual，才可以用override，不标的话，目测可以同时存在
	// 10、理所当然地在加了virtual后提示无法修改访问提示符
	protected override void ParentProtected()
	{
		Debug.Log("PublicChild call ParentProtected");
	}
}

public class Child2 : PrivateParent
{
	public void ParentPrivate()
	{
		Debug.Log("Child2 call ParentPrivate");
	}
}

public class PrivateConstruction : MonoBehaviour
{
    void Start()
    {
		PublicChild child1 = new PublicChild();
		Child2 child2 = new Child2();
		// 9、理所当然地可以两个并存
		child1.ParentPrivate();
		child2.ParentPrivate();

		//PublicChild child2 = new PublicChild(1);

		// 3、protected构造函数不可访问
		//PrivateParent parent = new PrivateParent();

		// 5、不可以在类外面创建protected构造函数的类
		//PublicChild child2 = new PublicChild("");
	}
}

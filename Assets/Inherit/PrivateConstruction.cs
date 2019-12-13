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
		Debug.Log("PublicChild call str");
	}
}

public class PrivateConstruction : MonoBehaviour
{
    void Start()
    {
		PublicChild child1 = new PublicChild();

		//PublicChild child2 = new PublicChild(1);

		// 3、protected构造函数不可访问
		//PrivateParent parent = new PrivateParent();

		// 5、不可以在类外面创建protected构造函数的类
		//PublicChild child2 = new PublicChild("");
	}
}

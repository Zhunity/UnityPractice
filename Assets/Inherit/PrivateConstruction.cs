using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrivateParent
{
	// 3、把private 改成public也看不到
	//5、子类构造函数没写，改成public也不会有打印
	public PrivateParent()
	{
		Debug.Log("PrivateParent");
	}

}


public class PublicChild
{
	// 4、把自己的注释了，private父类构造函数也不会打印
	//public PublicChild()
	//{
	//	Debug.Log("PublicChild");
	//}

	//public PublicChild(int num) : base()
	//{
	//	Debug.Log("PublicChild call PrivateParent");
	//}
}

public class PrivateConstruction : MonoBehaviour
{
    void Start()
    {
		// 1、这样只会调用PublicChild的构造函数，不会调用PrivateParent的构造函数
		PublicChild child1 = new PublicChild();

		// 2、这样也看不到
		//PublicChild child2 = new PublicChild(1);

	}
}

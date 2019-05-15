using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonoParent : MonoBehaviour
{
    // Start is called before the first frame update
    void Awake()
    {
		Debug.Log("MonoParent Awake");
    }

   
}


public class ClassParent
{
	public ClassParent()
	{
		Debug.Log("ClassParent Ctor");
	}
}

public interface InterfaceParenct
{
	//修饰符“public”对该项无效
	/*public*/ void HelloWorld();
}
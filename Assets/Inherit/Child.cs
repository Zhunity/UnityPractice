using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// C# 不支持多重继承。但是，您可以使用接口来实现多重继承
/// </summary>
public class Child : ClassParent, InterfaceParenct
{
	public void HelloWorld()
	{
		throw new System.NotImplementedException();
	}

	// Start is called before the first frame update
	void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

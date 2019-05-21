using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// https://blog.csdn.net/wodownload2/article/details/81941384
/// unity里面是可以设置是否动态合批处，设置的地方在：File->Build Settings->Player Settings 
/// </summary>
public class CreateCube : MonoBehaviour
{
	public GameObject go;
    void Start()
    {
		for (int i = 0; i < 50; i++)
		{
			GameObject cube = GameObject.Instantiate(go);
		}
	}
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

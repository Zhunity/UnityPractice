using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreateCube : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
		GameObject go = GameObject.CreatePrimitive(PrimitiveType.Cube);
		for (int i = 0; i < 50; i++)

		{
			GameObject cube = GameObject.Instantiate(go);
			cube.isStatic = true;
			if(i % 10 == 0)
			{
				cube.transform.localScale = new Vector3(2, 2, 2);
			}

		}
	}
}

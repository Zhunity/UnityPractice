using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetParentSize : MonoBehaviour
{
	public RectTransform parent;
	public Vector2 size = new Vector2(200, 200);
	public Vector2 scale = Vector3.one;

	private void OnValidate()
	{
		parent.sizeDelta = size;
		parent.localScale = scale;
	}
}

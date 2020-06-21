using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class RectOffsetTest : MonoBehaviour
{
	public Vector2 offsetMax;
	public Vector2 offsetMin;

	RectTransform _rectTransform;
	RectTransform rectTransform
	{
		get
		{
			if(_rectTransform == null)
			{
				_rectTransform = transform as RectTransform;
			}
			return _rectTransform;
		}
		set
		{
			_rectTransform = value;
		}
	}

	private void Awake()
	{
		rectTransform = transform as RectTransform;
	}

	protected void OnValidate()
	{
		rectTransform.offsetMax = offsetMax;
		rectTransform.offsetMin = offsetMin;
	}
}

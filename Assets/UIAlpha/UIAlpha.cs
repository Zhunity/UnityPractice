using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIAlpha : MonoBehaviour {
	public CanvasGroup Group;

	private float _alpha;
	public float Alpha
	{
		set
		{
			_alpha = value;
			Group.alpha = value;
			Debug.Log(Group.alpha);
		}
		get
		{
			return _alpha;
		}
	}
}

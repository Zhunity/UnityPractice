using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FieldSerialize : MonoBehaviour {
	/// <summary>
	/// 只能显示不是默认实现的get/set，如果自己实现的get/set，可以参考UIAlpha
	/// </summary>
	[field:SerializeField]
	public int Num
	{
		get;
		set;
	}
}

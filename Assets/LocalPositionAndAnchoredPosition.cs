using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// https://www.jianshu.com/p/ecebe43c4564
/// </summary>
public class LocalPositionAndAnchoredPosition : MonoBehaviour {

	private float localX;
	private float localZ;
	private float LastLocalY = 0;
	public float LocalY = 0;

	private float AnchoredX;
	private float LastAnchoredY = 0;
	public float AnchoredY = 0;

	RectTransform rect;

	// Use this for initialization
	void Start () {
		localX = transform.localPosition.x;
		localZ = transform.localPosition.z;
		rect = gameObject.GetComponent<RectTransform>();
		AnchoredX = rect.anchoredPosition.x;
	}
	
	// Update is called once per frame
	void Update () {
		if(LocalY != LastLocalY)
		{
			LastLocalY = LocalY;
			LastAnchoredY = AnchoredY = rect.anchoredPosition.y;
			transform.localPosition = new Vector3(localX, LocalY, localZ);
			Debug.Log("local change transform.localPosition: " + transform.localPosition + "\nrect.anchoredPosition: " + rect.anchoredPosition);
		}

		if(LastAnchoredY != AnchoredY)
		{
			LastAnchoredY = AnchoredY;
			LastLocalY = LocalY = transform.localPosition.y;
			rect.anchoredPosition = new Vector2(AnchoredX, AnchoredY);
			Debug.Log("anchored change transform.localPosition: " + transform.localPosition + "\nrect.anchoredPosition: " + rect.anchoredPosition);
		}
	}
}

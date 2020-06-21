using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ScaleLayoutGroup : LayoutGroup
{
	public override void CalculateLayoutInputVertical()
	{
	}

	public override void SetLayoutHorizontal()
	{
		SetChildrenAlongAxis(0);
	}

	public override void SetLayoutVertical()
	{
		SetChildrenAlongAxis(1);
	}

	Dictionary<int, Vector2[]> childCorners = new Dictionary<int, Vector2[]>();

	protected override void Awake()
    {
		base.Awake();
		Vector2[] offsets = new Vector2[4];
		Vector2[] anchors = new Vector2[4];
		Vector3[] allCorners = new Vector3[4];
		Vector2 size = rectTransform.rect.size;
		Debug.Log(rectTransform.rect.min + "  " + rectTransform.rect.max + "  " + size + "  " + rectTransform.localPosition);

		Vector3[] thisCorners = new Vector3[4];
		rectTransform.GetWorldCorners(thisCorners);
		Vector3 thisMinPos = thisCorners[0];
		for (int i = 0; i < rectTransform.childCount; i++)
		{
			RectTransform child = rectTransform.GetChild(i).transform as RectTransform;
			child.GetWorldCorners(allCorners);

			var minCorner = allCorners[0];
			var maxCorner = allCorners[3];

			Vector2[] scales = new Vector2[2];



			for (int j = 0; j < 4; j++)
			{
				var anchorPos = Vector2.Scale(anchors[j], size);
				var cornerPos = allCorners[j] - thisMinPos;
				float x = (cornerPos.x - anchorPos.x) / size.x;
				float y = (cornerPos.y - anchorPos.y) / size.y;
				scales[i] = new Vector2(x, y);
				Debug.Log("anchors[j] " + anchors[j] + "  childCorner:" + cornerPos + "  anchorPos: " + (anchorPos) + "  scales:" + scales[i]);
			}
		}
	}

	public void GetCornerPercent(Vector2 parentSize, Vector3 parentMinCorner, Vector3 corner, Vector2 anchor)
	{

	}

	public int offsetMaxX;
	public int offsetMaxY;

	protected override void OnValidate()
	{
		base.OnValidate();
		rectTransform.offsetMax = new Vector2(offsetMaxX, offsetMaxY);
		
	}

	protected void SetChildrenAlongAxis(int axis)
	{
		float size = rectTransform.rect.size[axis];
		Vector2[] offsets = new Vector2[4];
		Vector2[] anchors = new Vector2[4];
		for (int i = 0; i < rectChildren.Count; i++)
		{
			RectTransform child = rectChildren[i];
			GetOffset(child, offsets);
			GetAnchors(child, anchors);
			for(int j = 0; j < 4; j ++)
			{
				float anchorPos = anchors[j][axis] * size;

			}
		}
	}

	// 左下角 逆时针
	protected void GetOffset(RectTransform child, Vector2[] offsets)
	{
		offsets[0] = child.offsetMin;
		offsets[1] = new Vector2(child.offsetMin.x, child.offsetMax.y);
		offsets[2] = child.offsetMax;
		offsets[3] = new Vector2(child.offsetMax.x, child.offsetMin.y);
	}

	protected void GetAnchors(RectTransform child, Vector2[] anchors)
	{
		anchors[0] = child.anchorMin;
		anchors[1] = new Vector2(child.anchorMin.x, child.anchorMax.y);
		anchors[2] = child.anchorMax;
		anchors[3] = new Vector2(child.anchorMax.x, child.anchorMin.y);
	}
}

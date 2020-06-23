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

	Dictionary<int, Vector2[]> childScales = new Dictionary<int, Vector2[]>();

	protected override void Awake()
    {
		base.Awake();
		Vector2 size = rectTransform.rect.size;

		// 获取左下角坐标
		Vector3[] thisCorners = new Vector3[4];
		rectTransform.GetWorldCorners(thisCorners);
		Vector3 thisMinPos = thisCorners[0];

		Vector3[] childCorners = new Vector3[4];
		for (int i = 0; i < rectTransform.childCount; i++)
		{
			RectTransform child = rectTransform.GetChild(i).transform as RectTransform;
			child.GetWorldCorners(childCorners);

			var minCorner = childCorners[0];
			var maxCorner = childCorners[2];

			Vector2[] scales = new Vector2[2];

			scales[0] = GetCornerScale(size, thisMinPos, minCorner, child.anchorMin);
			scales[1] = GetCornerScale(size, thisMinPos, maxCorner, child.anchorMax);
			childScales.Add(child.GetHashCode(), scales);
		}

		foreach (var item in childScales)
		{
			Debug.Log(item.Key + "  " + item.Value[0] + "  " + item.Value[1]);
		}
	}

	/// <summary>
	/// 计算左下角和右上角到边缘的距离占比
	/// </summary>
	/// <param name="parentSize"></param>
	/// <param name="parentMinCorner"></param>
	/// <param name="corner"></param>
	/// <param name="anchor"></param>
	/// <returns></returns>
	public Vector2 GetCornerScale(Vector2 parentSize, Vector3 parentMinCorner, Vector3 corner, Vector2 anchor)
	{
		var anchorPos = Vector2.Scale(anchor, parentSize);
		var cornerPos = corner - parentMinCorner;
		float x = (cornerPos.x - anchorPos.x) / parentSize.x;
		float y = (cornerPos.y - anchorPos.y) / parentSize.y;
		Vector2 scale = new Vector2(x, y);
		return scale;
	}

	public void SetChildrenRect(Vector2 parentSize, RectTransform child, Vector2 scale, int axis, bool isMax)
	{
		float scaleAxisValue = scale[axis];
		Vector2 anchor = isMax ? child.anchorMax : child.anchorMin;
		float anchorPos = anchor[axis] * parentSize[axis];

		float cornerPos = scaleAxisValue * parentSize[axis] + anchorPos;

		Vector2 offset;
		if (isMax)
		{
			offset = child.offsetMax;
			offset[axis] = cornerPos - parentSize[axis];
			Debug.Log("axis  " + axis + " isMax " + isMax + "  parentSize " + parentSize + "  cornerPos :" + cornerPos + " offset " + offset);
			child.offsetMax = offset;
		}
		else
		{
			offset = child.offsetMin;
			offset[axis] = cornerPos;
			Debug.Log("axis  " + axis + " isMax " + isMax + "  parentSize " + parentSize + "  cornerPos :" + cornerPos + " offset " + offset);
			child.offsetMin = offset;
		}
	}

	public void SetChildOffset(RectTransform child, int axis)
	{

	}

	protected void SetChildrenAlongAxis(int axis)
	{
		Vector2 size = rectTransform.rect.size;

		Vector2[] scales = new Vector2[2];
		for (int i = 0; i < rectChildren.Count; i++)
		{
			RectTransform child = rectChildren[i];
			if(!childScales.TryGetValue(child.GetHashCode(), out scales))
			{
				continue;
			}
			SetChildrenRect(size, child, scales[0], axis, false);
			SetChildrenRect(size, child, scales[1], axis, true);
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

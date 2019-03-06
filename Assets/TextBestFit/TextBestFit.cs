using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Text))]
[RequireComponent(typeof(ContentSizeFitter))]
public class TextBestFit : MonoBehaviour {

	public float MaxWidth = 450f;

	private Text text;
	private ContentSizeFitter contentSizeFitter;

	void Start () {
		text = GetComponent<Text>();
		contentSizeFitter = GetComponent<ContentSizeFitter>();
	}

	private void Update()
	{
		if (text.preferredWidth <= MaxWidth)
		{
			text.resizeTextForBestFit = false;
			contentSizeFitter.horizontalFit = ContentSizeFitter.FitMode.PreferredSize;
		}
		else
		{
			RectTransform tran = text.GetComponent<RectTransform>();
			tran.sizeDelta = new Vector2(MaxWidth, tran.sizeDelta.y);
			text.resizeTextForBestFit = true;
			contentSizeFitter.horizontalFit = ContentSizeFitter.FitMode.Unconstrained;
		}
		Debug.Log(text.preferredWidth);
	}
}

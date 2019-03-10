using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text.RegularExpressions;

public class RegexNum : MonoBehaviour
{
	public string input;
	public char separator;
	private Dictionary<int, string> mapNumDesc;

	void Start()
    {
		mapNumDesc = new Dictionary<int, string>();
	}

	private void OnGUI()
	{
		if (GUI.Button(new Rect(0, 0, 100, 50), "Find"))
		{
			Find();
			DumpDict();
		}
	}


	private void Find()
	{
		var partList = input.Split(separator);
		for (int i = 0; i < partList.Length; i++)
		{
			int result = -1;
			string str = partList[i];
			Debug.Log(i + "    " + result + "   " + str + "    Bian Li");
			if (string.IsNullOrEmpty(str))
			{
				continue;
			}
			Match match = Regex.Match(str, @"\d");
			if (match != null)
			{
				result = int.Parse(match.Value);
				if (result > 0)
				{
					Debug.Log(i + "    " + match.Value + "   " + str + "    result");
					mapNumDesc[result] = str;

				}
			}
		}
	}

	private void DumpDict()
	{
		Debug.Log("-------------------------------------------------------------");
		foreach (var item in mapNumDesc)
		{
			Debug.Log(string.Format("Key:{0} Value:{1}", item.Key, item.Value));
		}
		Debug.Log("-------------------------------------------------------------");
	}
}

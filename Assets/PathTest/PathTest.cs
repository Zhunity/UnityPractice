using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;

public class PathTest : MonoBehaviour
{
	private Text m_text;

	// Use this for initialization
	void Start()
	{
		m_text = GetComponent<Text>();
		string persistentDataPath = Application.persistentDataPath;
		string dataPath = Application.dataPath;
		string streamingAssetsPath = Application.streamingAssetsPath;
		string temporaryCachePath = Application.temporaryCachePath;
		string path = string.Format("persistentDataPath:\t{0}\ndataPath:\t{1}\nstreamingAssetsPath:\t{2}\ntemporaryCachePath:\t{3}", persistentDataPath, dataPath, streamingAssetsPath, temporaryCachePath);
		Write("/text.txt", path);
		string ganEmptyPath = new FileInfo("/test.txt").FullName;
		Write("text.txt", path);
		string emptyPath = new FileInfo("test.txt").FullName;
		Write(dataPath + "/text.txt", path);
		string dataFilePath = new FileInfo("test.txt").FullName;
		path = string.Format("{0}\nganEmptyPath:\t{1}\nemptyPath:\t{2}\ndataFilePath:\t{3}", path, ganEmptyPath, emptyPath, dataFilePath);
		Debug.Log(path);
		m_text.text = path;
	}

	public void Write(string path, string text)
	{
		FileStream fs = new FileStream(path, FileMode.Append);
		StreamWriter sw = new StreamWriter(fs);
		//开始写入
		text = string.Format("TextPath:{0}\n{1}", path, text);
		sw.Write(text);
		//清空缓冲区
		sw.Flush();
		//关闭流
		sw.Close();
		fs.Close();
	}

	/// <summary>  
	/// 获取路径下所有文件以及子文件夹中文件  
	/// </summary>  
	/// <param name="path">全路径根目录</param>  
	/// <param name="FileList">存放所有文件的全路径</param>  
	/// <returns></returns>  
	public static List<string> GetFile(string path, List<string> FileList)
	{
		DirectoryInfo dir = new DirectoryInfo(path);
		FileInfo[] fil = dir.GetFiles();
		DirectoryInfo[] dii = dir.GetDirectories();
		foreach (FileInfo f in fil)
		{
			FileList.Add(f.FullName);//添加文件路径到列表中  
		}
		//获取子文件夹内的文件列表，递归遍历  
		foreach (DirectoryInfo d in dii)
		{
			GetFile(d.FullName, FileList);
		}
		return FileList;
	}
}
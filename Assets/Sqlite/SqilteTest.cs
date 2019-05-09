using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SqilteTest : MonoBehaviour
{
	// https://www.jianshu.com/p/381ed9d59654

	public SqliteDatabase Sqlite;

	void Start()
    {
		//打开测试数据库
		Sqlite = new SqliteDatabase("test.db");

		//添加数据
		Sqlite.ExecuteNonQuery("insert into example (id, name) values(1, 'test name')");
		PrintData();

	}

   private void PrintData()
	{
		DataTable dt = Sqlite.ExecuteQuery("SELECT * FROM example");
		string name;
		int id;
		foreach(DataRow item in dt.Rows)
		{
			id = (int)item["id"];
			name = (string)item["name"];
			Debug.Log(id + "  " + name);
		}
	}
}

using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using UnityEngine;

/// <summary>
/// https://blog.csdn.net/sinat_34791632/article/details/79722440 
/// </summary>
public class TCPClient : MonoBehaviour
{

	public string ipAdress;
	public int port;

	private byte[] data = new byte[1024];
	private Socket clientSocket;
	private Thread receiveT;

	void Start()
	{
		ConnectToServer();
	}

	void ConnectToServer()
	{
		try
		{
			clientSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
			clientSocket.Connect(IPAddress.Parse(ipAdress), port);
			Debug.Log("连接服务器成功");
			receiveT = new Thread(ReceiveMsg);
			receiveT.Start();

		}
		catch (System.Exception ex)
		{
			Debug.Log("连接服务器失败！");
			Debug.Log(ex.Message);
		}
	}

	private void ReceiveMsg()
	{
		while (true)
		{
			if (clientSocket.Connected == false)
			{
				Debug.Log("与服务器断开了连接");
				break;
			}

			int lenght = 0;
			lenght = clientSocket.Receive(data);

			string str = Encoding.UTF8.GetString(data, 0, data.Length);
			Debug.Log(str);

		}
	}

	void SendMesg(string ms)
	{
		byte[] data = new byte[1024];
		data = Encoding.UTF8.GetBytes(ms);
		clientSocket.Send(data);
	}

	void SendMesg(object obj)
	{
		//byte[] data = new byte[1024];
		//data = obj.SerializeToByteArray();
		//clientSocket.Send(data, data.Length, 0);
	}

	void OnDestroy()
	{
		try
		{
			if (clientSocket != null)
			{
				clientSocket.Shutdown(SocketShutdown.Both);
				clientSocket.Close();//关闭连接
			}

			if (receiveT != null)
			{
				receiveT.Interrupt();
				receiveT.Abort();
			}

		}
		catch (Exception ex)
		{
			Debug.Log(ex.Message);
		}
	}
}
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using UnityEngine;

/// <summary>
/// https://blog.csdn.net/sinat_34791632/article/details/79722440
/// </summary>
public class TCPServer : MonoBehaviour
{
	public string ipAdress;
	public int port;

	private byte[] data = new byte[1024];
	private Socket serverSocket;//服务器Socket
	private Socket client;//客户端Socket
	private Thread myThread;//启动监听线程
	private Thread receiveThread;//接收数据线程

	private List<Socket> clientSocketList = new List<Socket>();

	void Awake()
	{
		InitSocket();

	}

	void InitSocket()
	{
		try
		{
			serverSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
			IPEndPoint iPPoint = new IPEndPoint(IPAddress.Parse(ipAdress), port);
			serverSocket.Bind(iPPoint);
			serverSocket.Listen(10);
			Debug.Log("Server Running...");
			myThread = new Thread(ListenClientConnect);
			myThread.Start();

		}
		catch (System.Exception ex)
		{

			Debug.Log(ex.Message);
		}
	}

	void ListenClientConnect()
	{
		while (true)
		{
			client = serverSocket.Accept();
			clientSocketList.Add(client);

			Debug.Log("客户端：" + client.RemoteEndPoint + "连接到服务器！");
			AllSendMsg("From Server:" + client.RemoteEndPoint + "客户端已连接到服务器！");

			receiveThread = new Thread(ReceiveMsg);
			receiveThread.Start(client);

		}
	}

	void ReceiveMsg(object clientSocket)
	{
		client = clientSocket as Socket;
		while (true)
		{
			try
			{
				int lenght = 0;
				lenght = client.Receive(data);

				if (lenght == 0 || client.Poll(100, SelectMode.SelectRead))
				{
					string s = "客户端：" + client.RemoteEndPoint + "断开了连接！";
					Debug.Log(s);
					AllSendMsg(s);
					clientSocketList.Remove(client);
					break;
				}

				string str = Encoding.UTF8.GetString(data, 0, data.Length);

				AllSendMsg(str);
			}
			catch (System.Exception ex)
			{
				Debug.Log("从服务器获取数据错误" + ex.Message);
			}
		}
	}


	void AllSendMsg(string ms)
	{
		for (int i = 0; i < clientSocketList.Count; i++)
		{
			clientSocketList[i].Send(Encoding.UTF8.GetBytes(ms));
		}
	}

	void AllSendMsg(object obj)
	{
		//for (int i = 0; i < clientSocketList.Count; i++)
		//{
		//	data = obj.SerializeToByteArray();
		//	clientSocketList[i].Send(data, data.Length, 0);
		//}
	}

	void OnDestroy()
	{

		try
		{
			//关闭线程
			if (myThread != null || receiveThread != null)
			{
				myThread.Interrupt();
				myThread.Abort();

				receiveThread.Interrupt();
				receiveThread.Abort();

			}
			//最后关闭socket
			if (serverSocket != null)
			{
				for (int i = 0; i < clientSocketList.Count; i++)
				{
					clientSocketList[i].Close();
				}

				serverSocket.Shutdown(SocketShutdown.Both);
				serverSocket.Close();

			}
		}
		catch (System.Exception ex)
		{
			Debug.Log(ex.Message);
		}
		print("disconnect");
	}
}
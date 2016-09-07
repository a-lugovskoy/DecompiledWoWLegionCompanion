using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Runtime.Serialization;
using System.Text;
using UnityEngine;
using WowJamMessages.MobileJSON;

public class ClientConnection<PacketType> where PacketType : PacketFormat, new()
{
	public enum ConnectionState
	{
		Disconnected = 0,
		Connecting = 1,
		ConnectionFailed = 2,
		Connected = 3
	}

	private enum ConnectionEventTypes
	{
		OnConnected = 0,
		OnDisconnected = 1,
		OnPacketCompleted = 2
	}

	private class ConnectionEvent
	{
		public ClientConnection<PacketType>.ConnectionEventTypes Type
		{
			get;
			set;
		}

		public BattleNetErrors Error
		{
			get;
			set;
		}

		public PacketType Packet
		{
			get;
			set;
		}

		public Exception Exception
		{
			get;
			set;
		}
	}

	public delegate void ConnectHandler(BattleNetErrors error);

	public delegate void DisconnectHandler(BattleNetErrors error);

	private List<ClientConnection<PacketType>.ConnectHandler> m_connectHandlers = new List<ClientConnection<PacketType>.ConnectHandler>();

	private List<ClientConnection<PacketType>.DisconnectHandler> m_disconnectHandlers = new List<ClientConnection<PacketType>.DisconnectHandler>();

	private static int RECEIVE_BUFFER_SIZE = 65536;

	private static int BACKING_BUFFER_SIZE = 262144;

	private bool m_stolenSocket;

	private ClientConnection<PacketType>.ConnectionState m_connectionState;

	private Socket m_socket;

	private byte[] m_receiveBuffer;

	private byte[] m_backingBuffer;

	private int m_backingBufferBytes;

	private Queue<PacketType> m_outQueue = new Queue<PacketType>();

	private int m_outPacketsInFlight;

	private string m_hostAddress;

	private int m_hostPort;

	private PacketType m_currentPacket;

	private List<IConnectionListener<PacketType>> m_listeners = new List<IConnectionListener<PacketType>>();

	private List<object> m_listenerStates = new List<object>();

	private List<ClientConnection<PacketType>.ConnectionEvent> m_connectionEvents = new List<ClientConnection<PacketType>.ConnectionEvent>();

	private object m_mutex = new object();

	public bool Active
	{
		get
		{
			return this.m_socket != null && (this.m_socket.get_Connected() || this.m_connectionState == ClientConnection<PacketType>.ConnectionState.Connecting);
		}
	}

	public ClientConnection<PacketType>.ConnectionState State
	{
		get
		{
			return this.m_connectionState;
		}
	}

	public string HostAddress
	{
		get
		{
			return this.m_hostAddress + ":" + this.m_hostPort;
		}
	}

	public ClientConnection()
	{
		this.m_connectionState = ClientConnection<PacketType>.ConnectionState.Disconnected;
		this.m_receiveBuffer = new byte[ClientConnection<PacketType>.RECEIVE_BUFFER_SIZE];
		this.m_backingBuffer = new byte[ClientConnection<PacketType>.BACKING_BUFFER_SIZE];
		this.m_stolenSocket = false;
	}

	public ClientConnection(Socket socket)
	{
		this.m_socket = socket;
		this.m_connectionState = ClientConnection<PacketType>.ConnectionState.Connected;
		this.m_receiveBuffer = new byte[ClientConnection<PacketType>.RECEIVE_BUFFER_SIZE];
		this.m_stolenSocket = true;
		this.m_hostAddress = "local";
		this.m_hostPort = 0;
	}

	public bool AddConnectHandler(ClientConnection<PacketType>.ConnectHandler handler)
	{
		if (this.m_connectHandlers.Contains(handler))
		{
			return false;
		}
		this.m_connectHandlers.Add(handler);
		return true;
	}

	public bool RemoveConnectHandler(ClientConnection<PacketType>.ConnectHandler handler)
	{
		return this.m_connectHandlers.Remove(handler);
	}

	public bool AddDisconnectHandler(ClientConnection<PacketType>.DisconnectHandler handler)
	{
		if (this.m_disconnectHandlers.Contains(handler))
		{
			return false;
		}
		this.m_disconnectHandlers.Add(handler);
		return true;
	}

	public bool RemoveDisconnectHandler(ClientConnection<PacketType>.DisconnectHandler handler)
	{
		return this.m_disconnectHandlers.Remove(handler);
	}

	public bool HasEvents()
	{
		return this.m_connectionEvents.get_Count() > 0;
	}

	private void AddConnectEvent(BattleNetErrors error, Exception exception = null)
	{
		ClientConnection<PacketType>.ConnectionEvent connectionEvent = new ClientConnection<PacketType>.ConnectionEvent();
		connectionEvent.Type = ClientConnection<PacketType>.ConnectionEventTypes.OnConnected;
		connectionEvent.Error = error;
		connectionEvent.Exception = exception;
		object mutex = this.m_mutex;
		lock (mutex)
		{
			this.m_connectionEvents.Add(connectionEvent);
		}
	}

	private void AddDisconnectEvent(BattleNetErrors error)
	{
		ClientConnection<PacketType>.ConnectionEvent connectionEvent = new ClientConnection<PacketType>.ConnectionEvent();
		connectionEvent.Type = ClientConnection<PacketType>.ConnectionEventTypes.OnDisconnected;
		connectionEvent.Error = error;
		object mutex = this.m_mutex;
		lock (mutex)
		{
			this.m_connectionEvents.Add(connectionEvent);
		}
	}

	~ClientConnection()
	{
		if (this.m_socket != null)
		{
			this.m_socket.Close();
		}
	}

	public void Connect(string host, int port)
	{
		this.m_hostAddress = host;
		this.m_hostPort = port;
		IPAddress iPAddress = IPAddress.Parse(host);
		IPEndPoint iPEndPoint = new IPEndPoint(iPAddress, port);
		this.DisconnectSocket();
		this.m_socket = new Socket(2, 1, 6);
		this.m_connectionState = ClientConnection<PacketType>.ConnectionState.Connecting;
		try
		{
			this.m_socket.BeginConnect(iPEndPoint, new AsyncCallback(this.ConnectCallback), null);
		}
		catch (Exception ex)
		{
			Debug.LogWarning("Could not connect to " + this.HostAddress + " -- " + ex.get_Message());
			this.m_connectionState = ClientConnection<PacketType>.ConnectionState.ConnectionFailed;
			this.AddConnectEvent(BattleNetErrors.ERROR_RPC_PEER_UNKNOWN, null);
		}
	}

	private void ConnectCallback(IAsyncResult ar)
	{
		bool flag = false;
		Exception exception = null;
		try
		{
			this.m_socket.EndConnect(ar);
			this.m_socket.BeginReceive(this.m_receiveBuffer, 0, ClientConnection<PacketType>.RECEIVE_BUFFER_SIZE, 0, new AsyncCallback(this.ReceiveCallback), null);
		}
		catch (Exception ex)
		{
			exception = ex;
			flag = true;
		}
		if (flag || !this.m_socket.get_Connected())
		{
			this.AddConnectEvent(BattleNetErrors.ERROR_RPC_PEER_UNAVAILABLE, exception);
		}
		else
		{
			this.AddConnectEvent(BattleNetErrors.ERROR_OK, null);
		}
	}

	public void Disconnect()
	{
		this.DisconnectSocket();
		this.m_connectionState = ClientConnection<PacketType>.ConnectionState.Disconnected;
	}

	private void DisconnectSocket()
	{
		if (this.m_socket == null)
		{
			return;
		}
		if (!this.m_socket.get_Connected())
		{
			return;
		}
		this.m_socket.Close();
	}

	public void StartReceiving()
	{
		if (!this.m_stolenSocket)
		{
			Debug.LogError("StartReceiving should only be called on sockets created with ClientConnection(Socket)");
			return;
		}
		try
		{
			this.m_socket.BeginReceive(this.m_receiveBuffer, 0, ClientConnection<PacketType>.RECEIVE_BUFFER_SIZE, 0, new AsyncCallback(this.ReceiveCallback), null);
		}
		catch (Exception ex)
		{
			Debug.LogError("error receiving from local connection: " + ex.get_Message());
		}
	}

	private void BytesReceived(byte[] bytes, int nBytes, int offset)
	{
		while (nBytes > 0)
		{
			if (this.m_currentPacket == null)
			{
				this.m_currentPacket = Activator.CreateInstance<PacketType>();
			}
			int num = this.m_currentPacket.Decode(bytes, offset, nBytes);
			nBytes -= num;
			offset += num;
			if (!this.m_currentPacket.IsLoaded())
			{
				Array.Copy(bytes, offset, this.m_backingBuffer, 0, nBytes);
				this.m_backingBufferBytes = nBytes;
				return;
			}
			ClientConnection<PacketType>.ConnectionEvent connectionEvent = new ClientConnection<PacketType>.ConnectionEvent();
			connectionEvent.Type = ClientConnection<PacketType>.ConnectionEventTypes.OnPacketCompleted;
			connectionEvent.Packet = this.m_currentPacket;
			object mutex = this.m_mutex;
			lock (mutex)
			{
				this.m_connectionEvents.Add(connectionEvent);
			}
			this.m_currentPacket = (PacketType)((object)null);
		}
		this.m_backingBufferBytes = 0;
	}

	private void BytesReceived(int nBytes)
	{
		if (this.m_backingBufferBytes > 0)
		{
			if (nBytes + this.m_backingBufferBytes > ClientConnection<PacketType>.BACKING_BUFFER_SIZE)
			{
				Debug.LogError("backing buffer out of room");
				return;
			}
			int nBytes2 = this.m_backingBufferBytes + nBytes;
			Array.Copy(this.m_receiveBuffer, 0, this.m_backingBuffer, this.m_backingBufferBytes, nBytes);
			this.m_backingBufferBytes = 0;
			this.BytesReceived(this.m_backingBuffer, nBytes2, 0);
		}
		else
		{
			this.BytesReceived(this.m_receiveBuffer, nBytes, 0);
		}
	}

	private void ReceiveCallback(IAsyncResult ar)
	{
		if (this.m_socket.get_Connected())
		{
			try
			{
				int num = this.m_socket.EndReceive(ar);
				if (num > 0)
				{
					this.BytesReceived(num);
					this.m_socket.BeginReceive(this.m_receiveBuffer, 0, ClientConnection<PacketType>.RECEIVE_BUFFER_SIZE, 0, new AsyncCallback(this.ReceiveCallback), null);
				}
				else
				{
					this.AddDisconnectEvent(BattleNetErrors.ERROR_RPC_PEER_DISCONNECTED);
				}
			}
			catch (Exception ex)
			{
				Debug.Log(ex.ToString());
				this.AddDisconnectEvent(BattleNetErrors.ERROR_RPC_PEER_DISCONNECTED);
			}
		}
		else
		{
			this.AddDisconnectEvent(BattleNetErrors.ERROR_RPC_PEER_DISCONNECTED);
		}
	}

	public bool SendBytes(byte[] bytes, AsyncCallback callback, object userData)
	{
		if (bytes.Length == 0)
		{
			return false;
		}
		if (!this.m_socket.get_Connected())
		{
			return false;
		}
		bool result = false;
		try
		{
			this.m_socket.BeginSend(bytes, 0, bytes.Length, 0, callback, userData);
			result = true;
		}
		catch (Exception)
		{
		}
		return result;
	}

	private void OnSendBytes(IAsyncResult ar)
	{
		try
		{
			this.m_socket.EndSend(ar);
		}
		catch (Exception)
		{
			this.AddDisconnectEvent(BattleNetErrors.ERROR_RPC_CONNECTION_TIMED_OUT);
		}
	}

	public bool SendString(string str)
	{
		Debug.Log("SendString: " + str);
		ASCIIEncoding aSCIIEncoding = new ASCIIEncoding();
		byte[] bytes = aSCIIEncoding.GetBytes(str);
		return this.SendBytes(bytes, new AsyncCallback(this.OnSendBytes), null);
	}

	public bool SendPacket(PacketType packet)
	{
		byte[] array = packet.Encode();
		if (array.Length == 0)
		{
			return false;
		}
		if (!this.m_socket.get_Connected())
		{
			return false;
		}
		object mutex = this.m_mutex;
		lock (mutex)
		{
			this.m_outPacketsInFlight++;
		}
		bool result = false;
		try
		{
			this.m_socket.BeginSend(array, 0, array.Length, 0, new AsyncCallback(this.OnSendPacket), null);
			result = true;
		}
		catch (Exception)
		{
			object mutex2 = this.m_mutex;
			lock (mutex2)
			{
				this.m_outPacketsInFlight--;
			}
		}
		return result;
	}

	private void OnSendPacket(IAsyncResult ar)
	{
		this.OnSendBytes(ar);
		object mutex = this.m_mutex;
		lock (mutex)
		{
			this.m_outPacketsInFlight--;
		}
	}

	public void QueuePacket(PacketType packet)
	{
		this.m_outQueue.Enqueue(packet);
	}

	public bool HasQueuedPackets()
	{
		return this.m_outQueue.get_Count() > 0;
	}

	public void SendQueuedPackets()
	{
		while (this.m_outQueue.get_Count() > 0)
		{
			PacketType packet = this.m_outQueue.Dequeue();
			this.SendPacket(packet);
		}
	}

	public bool HasOutPacketsInFlight()
	{
		return this.m_outPacketsInFlight > 0;
	}

	public void AddListener(IConnectionListener<PacketType> listener, object state)
	{
		this.m_listeners.Add(listener);
		this.m_listenerStates.Add(state);
	}

	public void RemoveListener(IConnectionListener<PacketType> listener)
	{
		while (this.m_listeners.Remove(listener))
		{
		}
	}

	public void Update()
	{
		object mutex = this.m_mutex;
		lock (mutex)
		{
			using (List<ClientConnection<PacketType>.ConnectionEvent>.Enumerator enumerator = this.m_connectionEvents.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					ClientConnection<PacketType>.ConnectionEvent current = enumerator.get_Current();
					this.PrintConnectionException(current);
					switch (current.Type)
					{
					case ClientConnection<PacketType>.ConnectionEventTypes.OnConnected:
					{
						if (current.Error != BattleNetErrors.ERROR_OK)
						{
							this.DisconnectSocket();
							this.m_connectionState = ClientConnection<PacketType>.ConnectionState.ConnectionFailed;
						}
						else
						{
							this.m_connectionState = ClientConnection<PacketType>.ConnectionState.Connected;
						}
						ClientConnection<PacketType>.ConnectHandler[] array = this.m_connectHandlers.ToArray();
						for (int i = 0; i < array.Length; i++)
						{
							ClientConnection<PacketType>.ConnectHandler connectHandler = array[i];
							connectHandler(current.Error);
						}
						break;
					}
					case ClientConnection<PacketType>.ConnectionEventTypes.OnDisconnected:
					{
						if (current.Error != BattleNetErrors.ERROR_OK)
						{
							this.Disconnect();
						}
						ClientConnection<PacketType>.DisconnectHandler[] array2 = this.m_disconnectHandlers.ToArray();
						for (int j = 0; j < array2.Length; j++)
						{
							ClientConnection<PacketType>.DisconnectHandler disconnectHandler = array2[j];
							disconnectHandler(current.Error);
						}
						break;
					}
					case ClientConnection<PacketType>.ConnectionEventTypes.OnPacketCompleted:
						for (int k = 0; k < this.m_listeners.get_Count(); k++)
						{
							IConnectionListener<PacketType> connectionListener = this.m_listeners.get_Item(k);
							object state = this.m_listenerStates.get_Item(k);
							connectionListener.PacketReceived(current.Packet, state);
						}
						break;
					}
				}
			}
			this.m_connectionEvents.Clear();
		}
		if (this.m_socket == null || this.m_connectionState != ClientConnection<PacketType>.ConnectionState.Connected)
		{
			return;
		}
		this.SendQueuedPackets();
	}

	private void PrintConnectionException(ClientConnection<PacketType>.ConnectionEvent connectionEvent)
	{
		Exception exception = connectionEvent.Exception;
		if (exception == null)
		{
			return;
		}
		Debug.LogError(string.Format("ClientConnection Exception - {0} - {1}:{2}\n{3}", new object[]
		{
			exception.get_Message(),
			this.m_hostAddress,
			this.m_hostPort,
			exception.get_StackTrace()
		}));
	}

	public void SendObject(MobileServerText obj)
	{
		string str;
		using (MemoryStream memoryStream = new MemoryStream())
		{
			DataContractSerializer dataContractSerializer = new DataContractSerializer(typeof(MobileServerText));
			dataContractSerializer.WriteObject(memoryStream, obj);
			memoryStream.set_Position(0L);
			using (StreamReader streamReader = new StreamReader(memoryStream))
			{
				str = streamReader.ReadToEnd();
			}
		}
		this.SendString(str);
	}
}

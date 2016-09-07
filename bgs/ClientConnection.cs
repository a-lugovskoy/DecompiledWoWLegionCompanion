using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Text;

namespace bgs
{
	public class ClientConnection<PacketType> : IClientConnection<PacketType> where PacketType : PacketFormat, new()
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

		private List<ConnectHandler> m_connectHandlers = new List<ConnectHandler>();

		private List<DisconnectHandler> m_disconnectHandlers = new List<DisconnectHandler>();

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

		private TcpConnection m_connection = new TcpConnection();

		private PacketType m_currentPacket;

		private List<IClientConnectionListener<PacketType>> m_listeners = new List<IClientConnectionListener<PacketType>>();

		private List<object> m_listenerStates = new List<object>();

		private List<ClientConnection<PacketType>.ConnectionEvent> m_connectionEvents = new List<ClientConnection<PacketType>.ConnectionEvent>();

		private object m_mutex = new object();

		public bool Active
		{
			get
			{
				return this.m_connectionState == ClientConnection<PacketType>.ConnectionState.Connecting || this.m_connectionState == ClientConnection<PacketType>.ConnectionState.Connected;
			}
		}

		public ClientConnection<PacketType>.ConnectionState State
		{
			get
			{
				return this.m_connectionState;
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
		}

		public bool AddConnectHandler(ConnectHandler handler)
		{
			if (this.m_connectHandlers.Contains(handler))
			{
				return false;
			}
			this.m_connectHandlers.Add(handler);
			return true;
		}

		public bool RemoveConnectHandler(ConnectHandler handler)
		{
			return this.m_connectHandlers.Remove(handler);
		}

		public bool AddDisconnectHandler(DisconnectHandler handler)
		{
			if (this.m_disconnectHandlers.Contains(handler))
			{
				return false;
			}
			this.m_disconnectHandlers.Add(handler);
			return true;
		}

		public bool RemoveDisconnectHandler(DisconnectHandler handler)
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
			this.DisconnectSocket();
		}

		public void Connect(string host, int port)
		{
			this.m_connection.LogDebug = delegate(string log)
			{
				LogAdapter.Log(LogLevel.Debug, log);
			};
			this.m_connection.LogWarning = delegate(string log)
			{
				LogAdapter.Log(LogLevel.Warning, log);
			};
			this.m_connection.OnFailure = delegate
			{
				this.m_connectionState = ClientConnection<PacketType>.ConnectionState.ConnectionFailed;
				this.AddConnectEvent(BattleNetErrors.ERROR_RPC_PEER_UNKNOWN, null);
			};
			this.m_connection.OnSuccess = new Action(this.ConnectCallback);
			this.m_connectionState = ClientConnection<PacketType>.ConnectionState.Connecting;
			this.m_connection.Connect(host, port);
		}

		private void ConnectCallback()
		{
			Exception ex = null;
			this.m_socket = this.m_connection.Socket;
			try
			{
				this.m_socket.BeginReceive(this.m_receiveBuffer, 0, ClientConnection<PacketType>.RECEIVE_BUFFER_SIZE, 0, new AsyncCallback(this.ReceiveCallback), null);
			}
			catch (Exception ex2)
			{
				ex = ex2;
			}
			if (ex != null || !this.m_socket.get_Connected())
			{
				LogAdapter.Log(LogLevel.Warning, string.Format("ClientConnection - BeginReceive() failed. ip:{0}, port:{1}, exception:{3}", this.m_connection.Host, this.m_connection.Port, ex.get_Message()));
				this.DisconnectSocket();
				this.m_connectionState = ClientConnection<PacketType>.ConnectionState.ConnectionFailed;
				this.AddConnectEvent(BattleNetErrors.ERROR_RPC_PEER_UNAVAILABLE, ex);
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
			try
			{
				if (this.m_socket.get_Connected())
				{
					this.m_socket.Shutdown(2);
					this.m_socket.Close();
				}
			}
			catch (Exception ex)
			{
				LogAdapter.Log(LogLevel.Warning, string.Format("DisconnectSocket() failed. error: {0},", ex.get_Message()));
				if (ex is SocketException)
				{
					SocketException ex2 = (SocketException)ex;
					LogAdapter.Log(LogLevel.Warning, string.Format("\t Socket Error Code: {0},", ex2.get_ErrorCode()));
				}
			}
			this.m_socket = null;
		}

		public void StartReceiving()
		{
			if (!this.m_stolenSocket)
			{
				LogAdapter.Log(LogLevel.Error, "StartReceiving should only be called on sockets created with ClientConnection(Socket)");
				return;
			}
			try
			{
				this.m_socket.BeginReceive(this.m_receiveBuffer, 0, ClientConnection<PacketType>.RECEIVE_BUFFER_SIZE, 0, new AsyncCallback(this.ReceiveCallback), null);
			}
			catch (Exception ex)
			{
				LogAdapter.Log(LogLevel.Error, "error receiving from local connection: " + ex.get_Message());
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
				int num = this.m_backingBufferBytes + nBytes;
				if (num > this.m_backingBuffer.Length)
				{
					int num2 = (num + ClientConnection<PacketType>.BACKING_BUFFER_SIZE - 1) / ClientConnection<PacketType>.BACKING_BUFFER_SIZE;
					byte[] array = new byte[num2 * ClientConnection<PacketType>.BACKING_BUFFER_SIZE];
					Array.Copy(this.m_backingBuffer, 0, array, 0, this.m_backingBuffer.Length);
					this.m_backingBuffer = array;
				}
				Array.Copy(this.m_receiveBuffer, 0, this.m_backingBuffer, this.m_backingBufferBytes, nBytes);
				this.m_backingBufferBytes = 0;
				this.BytesReceived(this.m_backingBuffer, num, 0);
			}
			else
			{
				this.BytesReceived(this.m_receiveBuffer, nBytes, 0);
			}
		}

		private void ReceiveCallback(IAsyncResult ar)
		{
			try
			{
				if (this.m_socket != null && this.m_socket.get_Connected())
				{
					int num = this.m_socket.EndReceive(ar);
					if (num > 0)
					{
						this.BytesReceived(num);
						this.m_socket.BeginReceive(this.m_receiveBuffer, 0, ClientConnection<PacketType>.RECEIVE_BUFFER_SIZE, 0, new AsyncCallback(this.ReceiveCallback), null);
						return;
					}
				}
				this.AddDisconnectEvent(BattleNetErrors.ERROR_RPC_PEER_DISCONNECTED);
			}
			catch (Exception ex)
			{
				LogAdapter.Log(LogLevel.Debug, ex.ToString());
				this.AddDisconnectEvent(BattleNetErrors.ERROR_RPC_PEER_DISCONNECTED);
			}
		}

		public bool SendBytes(byte[] bytes, AsyncCallback callback, object userData)
		{
			if (bytes.Length == 0)
			{
				return false;
			}
			if (this.m_socket == null || !this.m_socket.get_Connected())
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
			if (this.m_socket == null || !this.m_socket.get_Connected())
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

		public void AddListener(IClientConnectionListener<PacketType> listener, object state)
		{
			this.m_listeners.Add(listener);
			this.m_listenerStates.Add(state);
		}

		public void RemoveListener(IClientConnectionListener<PacketType> listener)
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
							ConnectHandler[] array = this.m_connectHandlers.ToArray();
							for (int i = 0; i < array.Length; i++)
							{
								ConnectHandler connectHandler = array[i];
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
							DisconnectHandler[] array2 = this.m_disconnectHandlers.ToArray();
							for (int j = 0; j < array2.Length; j++)
							{
								DisconnectHandler disconnectHandler = array2[j];
								disconnectHandler(current.Error);
							}
							break;
						}
						case ClientConnection<PacketType>.ConnectionEventTypes.OnPacketCompleted:
							for (int k = 0; k < this.m_listeners.get_Count(); k++)
							{
								IClientConnectionListener<PacketType> clientConnectionListener = this.m_listeners.get_Item(k);
								object state = this.m_listenerStates.get_Item(k);
								clientConnectionListener.PacketReceived(current.Packet, state);
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
			LogAdapter.Log(LogLevel.Error, string.Format("ClientConnection Exception - {0} - {1}:{2}\n{3}", new object[]
			{
				exception.get_Message(),
				this.m_connection.Host,
				this.m_connection.Port,
				exception.get_StackTrace()
			}));
		}
	}
}

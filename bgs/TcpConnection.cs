using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;

namespace bgs
{
	public class TcpConnection
	{
		private Socket m_socket;

		private Queue<IPAddress> m_candidateIPAddresses;

		private IPAddress m_resolvedIPAddress;

		public Action<string> LogDebug = delegate
		{
		};

		public Action<string> LogWarning = delegate
		{
		};

		public Action OnFailure = delegate
		{
		};

		public Action OnSuccess = delegate
		{
		};

		public string Host
		{
			get;
			private set;
		}

		public int Port
		{
			get;
			private set;
		}

		public IPAddress ResolvedAddress
		{
			get
			{
				return this.m_resolvedIPAddress;
			}
		}

		public Socket Socket
		{
			get
			{
				return this.m_socket;
			}
		}

		public void Connect(string host, int port)
		{
			this.LogWarning.Invoke(string.Format("TcpConnection - Connecting to host: {0}, port: {1}", host, port));
			this.Host = host;
			this.Port = port;
			this.m_candidateIPAddresses = new Queue<IPAddress>();
			IPAddress iPAddress;
			if (IPAddress.TryParse(this.Host, ref iPAddress))
			{
				this.m_candidateIPAddresses.Enqueue(iPAddress);
			}
			try
			{
				Dns.BeginGetHostByName(this.Host, new AsyncCallback(this.GetHostEntryCallback), null);
			}
			catch (Exception ex)
			{
				this.LogWarning.Invoke(string.Format("TcpConnection - Connect() failed, could not get host entry. ip: {0}, port: {1}, exception: {2}", this.Host, this.Port, ex.get_Message()));
				this.OnFailure.Invoke();
			}
		}

		private void GetHostEntryCallback(IAsyncResult ar)
		{
			IPHostEntry iPHostEntry = Dns.EndGetHostByName(ar);
			Array.Sort<IPAddress>(iPHostEntry.get_AddressList(), delegate(IPAddress x, IPAddress y)
			{
				if (x.get_AddressFamily() < y.get_AddressFamily())
				{
					return -1;
				}
				if (x.get_AddressFamily() > y.get_AddressFamily())
				{
					return 1;
				}
				return 0;
			});
			IPAddress[] addressList = iPHostEntry.get_AddressList();
			for (int i = 0; i < addressList.Length; i++)
			{
				IPAddress iPAddress = addressList[i];
				this.m_candidateIPAddresses.Enqueue(iPAddress);
			}
			this.ConnectInternal();
		}

		private void ConnectInternal()
		{
			this.LogDebug.Invoke(string.Format("TcpConnection - ConnectInternal. address-count: {0}", this.m_candidateIPAddresses.get_Count()));
			this.Disconnect();
			if (this.m_candidateIPAddresses.get_Count() == 0)
			{
				this.LogWarning.Invoke(string.Format("TcpConnection - Could not connect to ip: {0}, port: {1}", this.Host, this.Port));
				this.OnFailure.Invoke();
				return;
			}
			this.m_resolvedIPAddress = this.m_candidateIPAddresses.Dequeue();
			IPEndPoint iPEndPoint = new IPEndPoint(this.m_resolvedIPAddress, this.Port);
			this.LogDebug.Invoke(string.Format("TcpConnection - Create Socket with ip: {0}, port: {1}, af: {2}", this.m_resolvedIPAddress, this.Port, this.m_resolvedIPAddress.get_AddressFamily()));
			this.m_socket = new Socket(this.m_resolvedIPAddress.get_AddressFamily(), 1, 6);
			try
			{
				this.m_socket.BeginConnect(iPEndPoint, new AsyncCallback(this.ConnectCallback), null);
			}
			catch (Exception ex)
			{
				this.LogDebug.Invoke(string.Format("TcpConnection - BeginConnect() failed. ip: {0}, port: {1}, af: {2}, exception: {3}", new object[]
				{
					this.m_resolvedIPAddress,
					this.Port,
					this.m_resolvedIPAddress.get_AddressFamily(),
					ex.get_Message()
				}));
				this.ConnectInternal();
			}
		}

		private void ConnectCallback(IAsyncResult ar)
		{
			Exception ex = null;
			try
			{
				this.m_socket.EndConnect(ar);
			}
			catch (Exception ex2)
			{
				ex = ex2;
			}
			if (ex != null || !this.m_socket.get_Connected())
			{
				this.LogDebug.Invoke(string.Format("TcpConnection - EndConnect() failed. ip: {0}, port: {1}, af: {2}, exception: {3}", new object[]
				{
					this.m_resolvedIPAddress,
					this.Port,
					this.m_resolvedIPAddress.get_AddressFamily(),
					ex.get_Message()
				}));
				this.ConnectInternal();
			}
			else
			{
				this.LogDebug.Invoke(string.Format("TcpConnection - Connected to ip: {0}, port: {1}, af: {2}", this.m_resolvedIPAddress, this.Port, this.m_resolvedIPAddress.get_AddressFamily()));
				this.OnSuccess.Invoke();
			}
		}

		public bool MatchSslCertName(IEnumerable<string> certNames)
		{
			IPHostEntry hostEntry = Dns.GetHostEntry(this.Host);
			using (IEnumerator<string> enumerator = certNames.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					string current = enumerator.get_Current();
					if (current.StartsWith("::ffff:"))
					{
						string text = current.Substring("::ffff:".get_Length());
						IPHostEntry hostEntry2 = Dns.GetHostEntry(text);
						IPAddress[] addressList = hostEntry2.get_AddressList();
						for (int i = 0; i < addressList.Length; i++)
						{
							IPAddress iPAddress = addressList[i];
							IPAddress[] addressList2 = hostEntry.get_AddressList();
							for (int j = 0; j < addressList2.Length; j++)
							{
								IPAddress iPAddress2 = addressList2[j];
								if (iPAddress2.Equals(iPAddress))
								{
									return true;
								}
							}
						}
					}
				}
			}
			string text2 = string.Format("TcpConnection - MatchSslCertName failed.", new object[0]);
			using (IEnumerator<string> enumerator2 = certNames.GetEnumerator())
			{
				while (enumerator2.MoveNext())
				{
					string current2 = enumerator2.get_Current();
					text2 += string.Format("\n\t certName: {0}", current2);
				}
			}
			IPAddress[] addressList3 = hostEntry.get_AddressList();
			for (int k = 0; k < addressList3.Length; k++)
			{
				IPAddress iPAddress3 = addressList3[k];
				text2 += string.Format("\n\t hostAddress: {0}", iPAddress3);
			}
			this.LogWarning.Invoke(text2);
			return false;
		}

		public void Disconnect()
		{
			if (this.m_socket == null)
			{
				return;
			}
			if (this.m_socket.get_Connected())
			{
				try
				{
					this.m_socket.Shutdown(2);
					this.m_socket.Close();
				}
				catch (SocketException ex)
				{
					this.LogWarning.Invoke(string.Format("TcpConnection.Disconnect() - SocketException: {0}", ex.get_Message()));
				}
			}
			this.m_socket = null;
		}
	}
}

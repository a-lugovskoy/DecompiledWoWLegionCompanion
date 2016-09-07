using bnet.protocol.attribute;
using bnet.protocol.notification;
using System;
using System.Collections.Generic;

namespace bgs
{
	public class BroadcastAPI : BattleNetAPI
	{
		public delegate void BroadcastCallback(IList<Attribute> AttributeList);

		private List<BroadcastAPI.BroadcastCallback> m_listenerList = new List<BroadcastAPI.BroadcastCallback>();

		public BroadcastAPI(BattleNetCSharp battlenet) : base(battlenet, "Broadcast")
		{
		}

		public override void InitRPCListeners(RPCConnection rpcConnection)
		{
			base.InitRPCListeners(rpcConnection);
		}

		public override void Initialize()
		{
			base.Initialize();
		}

		public override void OnDisconnected()
		{
			base.OnDisconnected();
		}

		public void RegisterListener(BroadcastAPI.BroadcastCallback cb)
		{
			if (this.m_listenerList.Contains(cb))
			{
				return;
			}
			this.m_listenerList.Add(cb);
		}

		public void OnBroadcast(Notification notification)
		{
			using (List<BroadcastAPI.BroadcastCallback>.Enumerator enumerator = this.m_listenerList.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					BroadcastAPI.BroadcastCallback current = enumerator.get_Current();
					current(notification.AttributeList);
				}
			}
		}
	}
}

using bnet.protocol.attribute;
using bnet.protocol.notification;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace bgs
{
	public class NotificationAPI : BattleNetAPI
	{
		private List<BnetNotification> m_notifications = new List<BnetNotification>();

		public NotificationAPI(BattleNetCSharp battlenet) : base(battlenet, "Notification")
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

		public void OnNotification(string notificationType, Notification notification)
		{
			if (notification.AttributeCount <= 0)
			{
				return;
			}
			BnetNotification bnetNotification = new BnetNotification(notificationType);
			SortedDictionary<string, int> sortedDictionary = new SortedDictionary<string, int>();
			int num = 0;
			bnetNotification.MessageType = 0;
			bnetNotification.MessageSize = 0;
			for (int i = 0; i < notification.AttributeCount; i++)
			{
				Attribute attribute = notification.Attribute.get_Item(i);
				if (attribute.Name == "message_type")
				{
					bnetNotification.MessageType = (int)attribute.Value.IntValue;
				}
				else if (attribute.Name == "message_size")
				{
					bnetNotification.MessageSize = (int)attribute.Value.IntValue;
				}
				else if (attribute.Name.StartsWith("fragment_"))
				{
					num += attribute.Value.BlobValue.Length;
					sortedDictionary.Add(attribute.Name, i);
				}
			}
			if (bnetNotification.MessageType == 0)
			{
				BattleNet.Log.LogError(string.Format("Missing notification type {0} of size {1}", bnetNotification.MessageType, bnetNotification.MessageSize));
				return;
			}
			if (0 < num)
			{
				bnetNotification.BlobMessage = new byte[num];
				SortedDictionary<string, int>.Enumerator enumerator = sortedDictionary.GetEnumerator();
				int num2 = 0;
				while (enumerator.MoveNext())
				{
					List<Attribute> arg_158_0 = notification.Attribute;
					KeyValuePair<string, int> current = enumerator.get_Current();
					byte[] blobValue = arg_158_0.get_Item(current.get_Value()).Value.BlobValue;
					Array.Copy(blobValue, 0, bnetNotification.BlobMessage, num2, blobValue.Length);
					num2 += blobValue.Length;
				}
			}
			if (bnetNotification.MessageSize != num)
			{
				BattleNet.Log.LogError(string.Format("Message size mismatch for notification type {0} - {1} != {2}", bnetNotification.MessageType, bnetNotification.MessageSize, num));
				return;
			}
			this.m_notifications.Add(bnetNotification);
		}

		public int GetNotificationCount()
		{
			return this.m_notifications.get_Count();
		}

		public void GetNotifications([Out] BnetNotification[] Notifications)
		{
			this.m_notifications.CopyTo(Notifications, 0);
		}

		public void ClearNotifications()
		{
			this.m_notifications.Clear();
		}
	}
}

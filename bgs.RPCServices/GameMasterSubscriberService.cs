using bnet.protocol.game_master;
using System;

namespace bgs.RPCServices
{
	public class GameMasterSubscriberService : ServiceDescriptor
	{
		public const uint NOTIFY_FACTORY_UPDATE_ID = 1u;

		public GameMasterSubscriberService() : base("bnet.protocol.game_master.GameMasterSubscriber")
		{
			this.Methods = new MethodDescriptor[2];
			this.Methods[(int)((UIntPtr)1)] = new MethodDescriptor("bnet.protocol.game_master.GameMasterSubscriber.NotifyFactoryUpdate", 1u, new MethodDescriptor.ParseMethod(ProtobufUtil.ParseFromGeneric<FactoryUpdateNotification>));
		}
	}
}

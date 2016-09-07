using bnet.protocol;
using System;

namespace bgs.RPCServices
{
	public class ResourcesService : ServiceDescriptor
	{
		public const uint GET_CONTENT_HANDLE = 1u;

		public ResourcesService() : base("bnet.protocol.resources.Resources")
		{
			this.Methods = new MethodDescriptor[2];
			this.Methods[(int)((UIntPtr)1)] = new MethodDescriptor("bnet.protocol.resources.Resources.GetContentHandle", 1u, new MethodDescriptor.ParseMethod(ProtobufUtil.ParseFromGeneric<bnet.protocol.ContentHandle>));
		}
	}
}

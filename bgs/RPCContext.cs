using bnet.protocol;
using System;

namespace bgs
{
	public class RPCContext
	{
		private Header header;

		private byte[] payload;

		private RPCContextDelegate callback;

		private bool responseReceived;

		public Header Header
		{
			get
			{
				return this.header;
			}
			set
			{
				this.header = value;
			}
		}

		public byte[] Payload
		{
			get
			{
				return this.payload;
			}
			set
			{
				this.payload = value;
			}
		}

		public RPCContextDelegate Callback
		{
			get
			{
				return this.callback;
			}
			set
			{
				this.callback = value;
			}
		}

		public bool ResponseReceived
		{
			get
			{
				return this.responseReceived;
			}
			set
			{
				this.responseReceived = value;
			}
		}

		public IProtoBuf Request
		{
			get;
			set;
		}

		public int SystemId
		{
			get;
			set;
		}

		public int Context
		{
			get;
			set;
		}
	}
}

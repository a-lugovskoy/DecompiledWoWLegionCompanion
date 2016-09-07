using System;

namespace Newtonsoft.Json
{
	[AttributeUsage]
	public sealed class JsonObjectAttribute : JsonContainerAttribute
	{
		private MemberSerialization _memberSerialization;

		public MemberSerialization MemberSerialization
		{
			get
			{
				return this._memberSerialization;
			}
			set
			{
				this._memberSerialization = value;
			}
		}

		public JsonObjectAttribute()
		{
		}

		public JsonObjectAttribute(MemberSerialization memberSerialization)
		{
			this.MemberSerialization = memberSerialization;
		}

		public JsonObjectAttribute(string id) : base(id)
		{
		}
	}
}

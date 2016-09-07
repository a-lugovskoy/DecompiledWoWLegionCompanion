using System;

namespace System.Runtime.Serialization
{
	[AttributeUsage]
	public sealed class EnumMemberAttribute : Attribute
	{
		private string value;

		public string Value
		{
			get
			{
				return this.value;
			}
			set
			{
				this.value = value;
			}
		}
	}
}

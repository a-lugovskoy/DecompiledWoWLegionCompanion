using System;

namespace System.Runtime.Serialization
{
	[AttributeUsage]
	public sealed class DataContractAttribute : Attribute
	{
		private string name;

		private string ns;

		public string Name
		{
			get
			{
				return this.name;
			}
			set
			{
				this.name = value;
			}
		}

		public string Namespace
		{
			get
			{
				return this.ns;
			}
			set
			{
				this.ns = value;
			}
		}

		public bool IsReference
		{
			get;
			set;
		}
	}
}

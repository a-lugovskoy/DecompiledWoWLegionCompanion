using System;

namespace Newtonsoft.Json
{
	[AttributeUsage]
	public abstract class JsonContainerAttribute : Attribute
	{
		internal bool? _isReference;

		public string Id
		{
			get;
			set;
		}

		public string Title
		{
			get;
			set;
		}

		public string Description
		{
			get;
			set;
		}

		public bool IsReference
		{
			get
			{
				bool? isReference = this._isReference;
				return isReference.get_HasValue() && isReference.get_Value();
			}
			set
			{
				this._isReference = new bool?(value);
			}
		}

		protected JsonContainerAttribute()
		{
		}

		protected JsonContainerAttribute(string id)
		{
			this.Id = id;
		}
	}
}

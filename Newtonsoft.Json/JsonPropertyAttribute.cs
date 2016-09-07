using System;

namespace Newtonsoft.Json
{
	[AttributeUsage]
	public sealed class JsonPropertyAttribute : Attribute
	{
		internal NullValueHandling? _nullValueHandling;

		internal DefaultValueHandling? _defaultValueHandling;

		internal ReferenceLoopHandling? _referenceLoopHandling;

		internal ObjectCreationHandling? _objectCreationHandling;

		internal TypeNameHandling? _typeNameHandling;

		internal bool? _isReference;

		internal int? _order;

		public NullValueHandling NullValueHandling
		{
			get
			{
				NullValueHandling? nullValueHandling = this._nullValueHandling;
				return (!nullValueHandling.get_HasValue()) ? NullValueHandling.Include : nullValueHandling.get_Value();
			}
			set
			{
				this._nullValueHandling = new NullValueHandling?(value);
			}
		}

		public DefaultValueHandling DefaultValueHandling
		{
			get
			{
				DefaultValueHandling? defaultValueHandling = this._defaultValueHandling;
				return (!defaultValueHandling.get_HasValue()) ? DefaultValueHandling.Include : defaultValueHandling.get_Value();
			}
			set
			{
				this._defaultValueHandling = new DefaultValueHandling?(value);
			}
		}

		public ReferenceLoopHandling ReferenceLoopHandling
		{
			get
			{
				ReferenceLoopHandling? referenceLoopHandling = this._referenceLoopHandling;
				return (!referenceLoopHandling.get_HasValue()) ? ReferenceLoopHandling.Error : referenceLoopHandling.get_Value();
			}
			set
			{
				this._referenceLoopHandling = new ReferenceLoopHandling?(value);
			}
		}

		public ObjectCreationHandling ObjectCreationHandling
		{
			get
			{
				ObjectCreationHandling? objectCreationHandling = this._objectCreationHandling;
				return (!objectCreationHandling.get_HasValue()) ? ObjectCreationHandling.Auto : objectCreationHandling.get_Value();
			}
			set
			{
				this._objectCreationHandling = new ObjectCreationHandling?(value);
			}
		}

		public TypeNameHandling TypeNameHandling
		{
			get
			{
				TypeNameHandling? typeNameHandling = this._typeNameHandling;
				return (!typeNameHandling.get_HasValue()) ? TypeNameHandling.None : typeNameHandling.get_Value();
			}
			set
			{
				this._typeNameHandling = new TypeNameHandling?(value);
			}
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

		public int Order
		{
			get
			{
				int? order = this._order;
				return (!order.get_HasValue()) ? 0 : order.get_Value();
			}
			set
			{
				this._order = new int?(value);
			}
		}

		public string PropertyName
		{
			get;
			set;
		}

		public Required Required
		{
			get;
			set;
		}

		public JsonPropertyAttribute()
		{
		}

		public JsonPropertyAttribute(string propertyName)
		{
			this.PropertyName = propertyName;
		}
	}
}

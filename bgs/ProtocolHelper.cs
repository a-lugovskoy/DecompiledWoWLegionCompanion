using bnet.protocol;
using bnet.protocol.attribute;
using System;
using System.Collections.Generic;
using System.Linq;

namespace bgs
{
	public static class ProtocolHelper
	{
		public static Attribute CreateAttribute(string name, long val)
		{
			Attribute attribute = new Attribute();
			Variant variant = new Variant();
			variant.SetIntValue(val);
			attribute.SetName(name);
			attribute.SetValue(variant);
			return attribute;
		}

		public static Attribute CreateAttribute(string name, bool val)
		{
			Attribute attribute = new Attribute();
			Variant variant = new Variant();
			variant.SetBoolValue(val);
			attribute.SetName(name);
			attribute.SetValue(variant);
			return attribute;
		}

		public static Attribute CreateAttribute(string name, string val)
		{
			Attribute attribute = new Attribute();
			Variant variant = new Variant();
			variant.SetStringValue(val);
			attribute.SetName(name);
			attribute.SetValue(variant);
			return attribute;
		}

		public static Attribute CreateAttribute(string name, byte[] val)
		{
			Attribute attribute = new Attribute();
			Variant variant = new Variant();
			variant.SetBlobValue(val);
			attribute.SetName(name);
			attribute.SetValue(variant);
			return attribute;
		}

		public static Attribute CreateAttribute(string name, ulong val)
		{
			Attribute attribute = new Attribute();
			Variant variant = new Variant();
			variant.SetUintValue(val);
			attribute.SetName(name);
			attribute.SetValue(variant);
			return attribute;
		}

		public static Attribute FindAttribute(List<Attribute> list, string attributeName, Func<Attribute, bool> condition = null)
		{
			if (list == null)
			{
				return null;
			}
			if (condition == null)
			{
				return Enumerable.FirstOrDefault<Attribute>(list, (Attribute a) => a.Name == attributeName);
			}
			return Enumerable.FirstOrDefault<Attribute>(list, (Attribute a) => a.Name == attributeName && condition.Invoke(a));
		}

		public static ulong GetUintAttribute(List<Attribute> list, string attributeName, ulong defaultValue, Func<Attribute, bool> condition = null)
		{
			if (list == null)
			{
				return defaultValue;
			}
			Attribute attribute;
			if (condition == null)
			{
				attribute = Enumerable.FirstOrDefault<Attribute>(list, (Attribute a) => a.Name == attributeName && a.Value.HasUintValue);
			}
			else
			{
				attribute = Enumerable.FirstOrDefault<Attribute>(list, (Attribute a) => a.Name == attributeName && a.Value.HasUintValue && condition.Invoke(a));
			}
			return (attribute != null) ? attribute.Value.UintValue : defaultValue;
		}

		public static EntityId CreateEntityId(ulong high, ulong low)
		{
			EntityId entityId = new EntityId();
			entityId.SetHigh(high);
			entityId.SetLow(low);
			return entityId;
		}

		public static bool IsNone(this Variant val)
		{
			return !val.HasBoolValue && !val.HasIntValue && !val.HasFloatValue && !val.HasStringValue && !val.HasBlobValue && !val.HasMessageValue && !val.HasFourccValue && !val.HasUintValue && !val.HasEntityidValue;
		}
	}
}

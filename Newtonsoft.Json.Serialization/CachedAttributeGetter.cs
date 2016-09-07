using Newtonsoft.Json.Utilities;
using System;
using System.Reflection;

namespace Newtonsoft.Json.Serialization
{
	internal static class CachedAttributeGetter<T> where T : Attribute
	{
		private static readonly ThreadSafeStore<ICustomAttributeProvider, T> TypeAttributeCache = new ThreadSafeStore<ICustomAttributeProvider, T>(new Func<ICustomAttributeProvider, T>(JsonTypeReflector.GetAttribute<T>));

		public static T GetAttribute(ICustomAttributeProvider type)
		{
			return CachedAttributeGetter<T>.TypeAttributeCache.Get(type);
		}
	}
}

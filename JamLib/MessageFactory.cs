using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace JamLib
{
	public static class MessageFactory
	{
		private static Dictionary<string, Type> s_messageDictionary;

		private static JsonSerializerSettings s_jsonSerializerSettings;

		public static JsonSerializerSettings SerializerSettings
		{
			get
			{
				return MessageFactory.s_jsonSerializerSettings;
			}
		}

		static MessageFactory()
		{
			IEnumerable<Type> enumerable = Enumerable.Where<Type>(Assembly.GetExecutingAssembly().GetTypes(), (Type t) => t.get_Namespace() != null && t.get_Namespace().StartsWith("WowJamMessages") && t.get_IsClass());
			MessageFactory.s_messageDictionary = Enumerable.ToDictionary<Type, string>(enumerable, (Type t) => t.get_Name());
			MessageFactory.s_jsonSerializerSettings = new JsonSerializerSettings();
			List<JsonConverter> list = new List<JsonConverter>();
			list.Add(new StringEnumConverter());
			list.Add(new JamEmbeddedMessageConverter());
			list.Add(new ByteArrayConverter());
			MessageFactory.s_jsonSerializerSettings.Converters = list;
		}

		public static Type GetMessageType(string nameSpace, string nameType)
		{
			return Type.GetType(nameSpace + "." + nameType);
		}

		public static Type GetMessageType(string nameType)
		{
			Type result;
			MessageFactory.s_messageDictionary.TryGetValue(nameType, ref result);
			return result;
		}

		public static object Deserialize(string message)
		{
			int num = message.IndexOf(':');
			if (num <= 0)
			{
				return null;
			}
			string text = message.Substring(0, num);
			string text2 = message.Substring(num + 1);
			if (text.get_Length() <= 0 || text2.get_Length() <= 0)
			{
				return null;
			}
			Type messageType = MessageFactory.GetMessageType(text);
			if (messageType == null)
			{
				return null;
			}
			try
			{
				return JsonConvert.DeserializeObject(text2, messageType, MessageFactory.s_jsonSerializerSettings);
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex);
			}
			return null;
		}

		public static string Serialize(object message)
		{
			Type type = message.GetType();
			return type.get_Name() + ":" + JsonConvert.SerializeObject(message, Formatting.None, MessageFactory.s_jsonSerializerSettings);
		}

		public static MessageDispatch GetDispatcher(Type handlerType)
		{
			IEnumerable<Type> enumerable = Enumerable.Where<Type>(MessageFactory.s_messageDictionary.get_Values(), delegate(Type t)
			{
				MethodInfo method = handlerType.GetMethod(t.get_Name() + "Handler");
				return method != null && method.GetParameters().Length == 1 && method.GetParameters()[0].get_ParameterType() == t;
			});
			Dictionary<Type, MethodInfo> dispatchDictionary = Enumerable.ToDictionary<Type, Type, MethodInfo>(enumerable, (Type t) => t, (Type t) => handlerType.GetMethod(t.get_Name() + "Handler"));
			return delegate(object handler, object message)
			{
				MethodInfo methodInfo;
				if (dispatchDictionary.TryGetValue(message.GetType(), ref methodInfo))
				{
					methodInfo.Invoke(handler, new object[]
					{
						message
					});
					return true;
				}
				return false;
			};
		}
	}
}

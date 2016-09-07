using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Utilities;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization;

namespace Newtonsoft.Json.Serialization
{
	public class DefaultContractResolver : IContractResolver
	{
		private static readonly IContractResolver _instance = new DefaultContractResolver(true);

		private static readonly IList<JsonConverter> BuiltInConverters;

		private static Dictionary<ResolverContractKey, JsonContract> _sharedContractCache;

		private static readonly object _typeContractCacheLock;

		private Dictionary<ResolverContractKey, JsonContract> _instanceContractCache;

		private readonly bool _sharedCache;

		internal static IContractResolver Instance
		{
			get
			{
				return DefaultContractResolver._instance;
			}
		}

		public bool DynamicCodeGeneration
		{
			get
			{
				return JsonTypeReflector.DynamicCodeGeneration;
			}
		}

		public BindingFlags DefaultMembersSearchFlags
		{
			get;
			set;
		}

		public bool SerializeCompilerGeneratedMembers
		{
			get;
			set;
		}

		public DefaultContractResolver() : this(false)
		{
		}

		public DefaultContractResolver(bool shareCache)
		{
			this.DefaultMembersSearchFlags = 20;
			this._sharedCache = shareCache;
		}

		static DefaultContractResolver()
		{
			// Note: this type is marked as 'beforefieldinit'.
			List<JsonConverter> list = new List<JsonConverter>();
			list.Add(new KeyValuePairConverter());
			list.Add(new BsonObjectIdConverter());
			DefaultContractResolver.BuiltInConverters = list;
			DefaultContractResolver._typeContractCacheLock = new object();
		}

		private Dictionary<ResolverContractKey, JsonContract> GetCache()
		{
			if (this._sharedCache)
			{
				return DefaultContractResolver._sharedContractCache;
			}
			return this._instanceContractCache;
		}

		private void UpdateCache(Dictionary<ResolverContractKey, JsonContract> cache)
		{
			if (this._sharedCache)
			{
				DefaultContractResolver._sharedContractCache = cache;
			}
			else
			{
				this._instanceContractCache = cache;
			}
		}

		public virtual JsonContract ResolveContract(Type type)
		{
			if (type == null)
			{
				throw new ArgumentNullException("type");
			}
			ResolverContractKey resolverContractKey = new ResolverContractKey(base.GetType(), type);
			Dictionary<ResolverContractKey, JsonContract> cache = this.GetCache();
			JsonContract jsonContract;
			if (cache == null || !cache.TryGetValue(resolverContractKey, ref jsonContract))
			{
				jsonContract = this.CreateContract(type);
				object typeContractCacheLock = DefaultContractResolver._typeContractCacheLock;
				lock (typeContractCacheLock)
				{
					cache = this.GetCache();
					Dictionary<ResolverContractKey, JsonContract> dictionary = (cache == null) ? new Dictionary<ResolverContractKey, JsonContract>() : new Dictionary<ResolverContractKey, JsonContract>(cache);
					dictionary.set_Item(resolverContractKey, jsonContract);
					this.UpdateCache(dictionary);
				}
			}
			return jsonContract;
		}

		protected virtual List<MemberInfo> GetSerializableMembers(Type objectType)
		{
			DataContractAttribute dataContractAttribute = JsonTypeReflector.GetDataContractAttribute(objectType);
			List<MemberInfo> list = Enumerable.ToList<MemberInfo>(Enumerable.Where<MemberInfo>(ReflectionUtils.GetFieldsAndProperties(objectType, this.DefaultMembersSearchFlags), (MemberInfo m) => !ReflectionUtils.IsIndexedProperty(m)));
			List<MemberInfo> list2 = Enumerable.ToList<MemberInfo>(Enumerable.Where<MemberInfo>(ReflectionUtils.GetFieldsAndProperties(objectType, 60), (MemberInfo m) => !ReflectionUtils.IsIndexedProperty(m)));
			List<MemberInfo> list3 = new List<MemberInfo>();
			using (List<MemberInfo>.Enumerator enumerator = list2.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					MemberInfo current = enumerator.get_Current();
					if (this.SerializeCompilerGeneratedMembers || !current.IsDefined(typeof(CompilerGeneratedAttribute), true))
					{
						if (list.Contains(current))
						{
							list3.Add(current);
						}
						else if (JsonTypeReflector.GetAttribute<JsonPropertyAttribute>(current) != null)
						{
							list3.Add(current);
						}
						else if (dataContractAttribute != null && JsonTypeReflector.GetAttribute<DataMemberAttribute>(current) != null)
						{
							list3.Add(current);
						}
					}
				}
			}
			Type type;
			if (objectType.AssignableToTypeName("System.Data.Objects.DataClasses.EntityObject", out type))
			{
				list3 = Enumerable.ToList<MemberInfo>(Enumerable.Where<MemberInfo>(list3, new Func<MemberInfo, bool>(this.ShouldSerializeEntityMember)));
			}
			return list3;
		}

		private bool ShouldSerializeEntityMember(MemberInfo memberInfo)
		{
			PropertyInfo propertyInfo = memberInfo as PropertyInfo;
			return propertyInfo == null || !propertyInfo.get_PropertyType().get_IsGenericType() || !(propertyInfo.get_PropertyType().GetGenericTypeDefinition().get_FullName() == "System.Data.Objects.DataClasses.EntityReference`1");
		}

		protected virtual JsonObjectContract CreateObjectContract(Type objectType)
		{
			JsonObjectContract jsonObjectContract = new JsonObjectContract(objectType);
			this.InitializeContract(jsonObjectContract);
			jsonObjectContract.MemberSerialization = JsonTypeReflector.GetObjectMemberSerialization(objectType);
			jsonObjectContract.Properties.AddRange(this.CreateProperties(jsonObjectContract.UnderlyingType, jsonObjectContract.MemberSerialization));
			if (Enumerable.Any<ConstructorInfo>(objectType.GetConstructors(52), (ConstructorInfo c) => c.IsDefined(typeof(JsonConstructorAttribute), true)))
			{
				ConstructorInfo attributeConstructor = this.GetAttributeConstructor(objectType);
				if (attributeConstructor != null)
				{
					jsonObjectContract.OverrideConstructor = attributeConstructor;
					jsonObjectContract.ConstructorParameters.AddRange(this.CreateConstructorParameters(attributeConstructor, jsonObjectContract.Properties));
				}
			}
			else if (jsonObjectContract.DefaultCreator == null || jsonObjectContract.DefaultCreatorNonPublic)
			{
				ConstructorInfo parametrizedConstructor = this.GetParametrizedConstructor(objectType);
				if (parametrizedConstructor != null)
				{
					jsonObjectContract.ParametrizedConstructor = parametrizedConstructor;
					jsonObjectContract.ConstructorParameters.AddRange(this.CreateConstructorParameters(parametrizedConstructor, jsonObjectContract.Properties));
				}
			}
			return jsonObjectContract;
		}

		private ConstructorInfo GetAttributeConstructor(Type objectType)
		{
			IList<ConstructorInfo> list = Enumerable.ToList<ConstructorInfo>(Enumerable.Where<ConstructorInfo>(objectType.GetConstructors(52), (ConstructorInfo c) => c.IsDefined(typeof(JsonConstructorAttribute), true)));
			if (list.get_Count() > 1)
			{
				throw new Exception("Multiple constructors with the JsonConstructorAttribute.");
			}
			if (list.get_Count() == 1)
			{
				return list.get_Item(0);
			}
			return null;
		}

		private ConstructorInfo GetParametrizedConstructor(Type objectType)
		{
			IList<ConstructorInfo> constructors = objectType.GetConstructors(20);
			if (constructors.get_Count() == 1)
			{
				return constructors.get_Item(0);
			}
			return null;
		}

		protected virtual IList<JsonProperty> CreateConstructorParameters(ConstructorInfo constructor, JsonPropertyCollection memberProperties)
		{
			ParameterInfo[] parameters = constructor.GetParameters();
			JsonPropertyCollection jsonPropertyCollection = new JsonPropertyCollection(constructor.get_DeclaringType());
			ParameterInfo[] array = parameters;
			for (int i = 0; i < array.Length; i++)
			{
				ParameterInfo parameterInfo = array[i];
				JsonProperty jsonProperty = memberProperties.GetClosestMatchProperty(parameterInfo.get_Name());
				if (jsonProperty != null && jsonProperty.PropertyType != parameterInfo.get_ParameterType())
				{
					jsonProperty = null;
				}
				JsonProperty jsonProperty2 = this.CreatePropertyFromConstructorParameter(jsonProperty, parameterInfo);
				if (jsonProperty2 != null)
				{
					jsonPropertyCollection.AddProperty(jsonProperty2);
				}
			}
			return jsonPropertyCollection;
		}

		protected virtual JsonProperty CreatePropertyFromConstructorParameter(JsonProperty matchingMemberProperty, ParameterInfo parameterInfo)
		{
			JsonProperty jsonProperty = new JsonProperty();
			jsonProperty.PropertyType = parameterInfo.get_ParameterType();
			bool flag;
			bool flag2;
			this.SetPropertySettingsFromAttributes(jsonProperty, parameterInfo, parameterInfo.get_Name(), parameterInfo.get_Member().get_DeclaringType(), MemberSerialization.OptOut, out flag, out flag2);
			jsonProperty.Readable = false;
			jsonProperty.Writable = true;
			if (matchingMemberProperty != null)
			{
				jsonProperty.PropertyName = ((!(jsonProperty.PropertyName != parameterInfo.get_Name())) ? matchingMemberProperty.PropertyName : jsonProperty.PropertyName);
				jsonProperty.Converter = (jsonProperty.Converter ?? matchingMemberProperty.Converter);
				jsonProperty.MemberConverter = (jsonProperty.MemberConverter ?? matchingMemberProperty.MemberConverter);
				jsonProperty.DefaultValue = (jsonProperty.DefaultValue ?? matchingMemberProperty.DefaultValue);
				jsonProperty.Required = ((jsonProperty.Required == Required.Default) ? matchingMemberProperty.Required : jsonProperty.Required);
				jsonProperty.IsReference = ((!jsonProperty.IsReference.get_HasValue()) ? matchingMemberProperty.IsReference : jsonProperty.IsReference);
				jsonProperty.NullValueHandling = ((!jsonProperty.NullValueHandling.get_HasValue()) ? matchingMemberProperty.NullValueHandling : jsonProperty.NullValueHandling);
				jsonProperty.DefaultValueHandling = ((!jsonProperty.DefaultValueHandling.get_HasValue()) ? matchingMemberProperty.DefaultValueHandling : jsonProperty.DefaultValueHandling);
				jsonProperty.ReferenceLoopHandling = ((!jsonProperty.ReferenceLoopHandling.get_HasValue()) ? matchingMemberProperty.ReferenceLoopHandling : jsonProperty.ReferenceLoopHandling);
				jsonProperty.ObjectCreationHandling = ((!jsonProperty.ObjectCreationHandling.get_HasValue()) ? matchingMemberProperty.ObjectCreationHandling : jsonProperty.ObjectCreationHandling);
				jsonProperty.TypeNameHandling = ((!jsonProperty.TypeNameHandling.get_HasValue()) ? matchingMemberProperty.TypeNameHandling : jsonProperty.TypeNameHandling);
			}
			return jsonProperty;
		}

		protected virtual JsonConverter ResolveContractConverter(Type objectType)
		{
			return JsonTypeReflector.GetJsonConverter(objectType, objectType);
		}

		private Func<object> GetDefaultCreator(Type createdType)
		{
			return JsonTypeReflector.ReflectionDelegateFactory.CreateDefaultConstructor<object>(createdType);
		}

		[SuppressMessage("Microsoft.Portability", "CA1903:UseOnlyApiFromTargetedFramework", MessageId = "System.Runtime.Serialization.DataContractAttribute.#get_IsReference()")]
		private void InitializeContract(JsonContract contract)
		{
			JsonContainerAttribute jsonContainerAttribute = JsonTypeReflector.GetJsonContainerAttribute(contract.UnderlyingType);
			if (jsonContainerAttribute != null)
			{
				contract.IsReference = jsonContainerAttribute._isReference;
			}
			else
			{
				DataContractAttribute dataContractAttribute = JsonTypeReflector.GetDataContractAttribute(contract.UnderlyingType);
				if (dataContractAttribute != null && dataContractAttribute.IsReference)
				{
					contract.IsReference = new bool?(true);
				}
			}
			contract.Converter = this.ResolveContractConverter(contract.UnderlyingType);
			contract.InternalConverter = JsonSerializer.GetMatchingConverter(DefaultContractResolver.BuiltInConverters, contract.UnderlyingType);
			if (ReflectionUtils.HasDefaultConstructor(contract.CreatedType, true) || contract.CreatedType.get_IsValueType())
			{
				contract.DefaultCreator = this.GetDefaultCreator(contract.CreatedType);
				contract.DefaultCreatorNonPublic = (!contract.CreatedType.get_IsValueType() && ReflectionUtils.GetDefaultConstructor(contract.CreatedType) == null);
			}
			this.ResolveCallbackMethods(contract, contract.UnderlyingType);
		}

		private void ResolveCallbackMethods(JsonContract contract, Type t)
		{
			if (t.get_BaseType() != null)
			{
				this.ResolveCallbackMethods(contract, t.get_BaseType());
			}
			MethodInfo methodInfo;
			MethodInfo methodInfo2;
			MethodInfo methodInfo3;
			MethodInfo methodInfo4;
			MethodInfo methodInfo5;
			this.GetCallbackMethodsForType(t, out methodInfo, out methodInfo2, out methodInfo3, out methodInfo4, out methodInfo5);
			if (methodInfo != null)
			{
				contract.OnSerializing = methodInfo;
			}
			if (methodInfo2 != null)
			{
				contract.OnSerialized = methodInfo2;
			}
			if (methodInfo3 != null)
			{
				contract.OnDeserializing = methodInfo3;
			}
			if (methodInfo4 != null)
			{
				contract.OnDeserialized = methodInfo4;
			}
			if (methodInfo5 != null)
			{
				contract.OnError = methodInfo5;
			}
		}

		private void GetCallbackMethodsForType(Type type, out MethodInfo onSerializing, out MethodInfo onSerialized, out MethodInfo onDeserializing, out MethodInfo onDeserialized, out MethodInfo onError)
		{
			onSerializing = null;
			onSerialized = null;
			onDeserializing = null;
			onDeserialized = null;
			onError = null;
			MethodInfo[] methods = type.GetMethods(54);
			for (int i = 0; i < methods.Length; i++)
			{
				MethodInfo methodInfo = methods[i];
				if (!methodInfo.get_ContainsGenericParameters())
				{
					Type type2 = null;
					ParameterInfo[] parameters = methodInfo.GetParameters();
					if (DefaultContractResolver.IsValidCallback(methodInfo, parameters, typeof(OnSerializingAttribute), onSerializing, ref type2))
					{
						onSerializing = methodInfo;
					}
					if (DefaultContractResolver.IsValidCallback(methodInfo, parameters, typeof(OnSerializedAttribute), onSerialized, ref type2))
					{
						onSerialized = methodInfo;
					}
					if (DefaultContractResolver.IsValidCallback(methodInfo, parameters, typeof(OnDeserializingAttribute), onDeserializing, ref type2))
					{
						onDeserializing = methodInfo;
					}
					if (DefaultContractResolver.IsValidCallback(methodInfo, parameters, typeof(OnDeserializedAttribute), onDeserialized, ref type2))
					{
						onDeserialized = methodInfo;
					}
					if (DefaultContractResolver.IsValidCallback(methodInfo, parameters, typeof(OnErrorAttribute), onError, ref type2))
					{
						onError = methodInfo;
					}
				}
			}
		}

		protected virtual JsonDictionaryContract CreateDictionaryContract(Type objectType)
		{
			JsonDictionaryContract jsonDictionaryContract = new JsonDictionaryContract(objectType);
			this.InitializeContract(jsonDictionaryContract);
			jsonDictionaryContract.PropertyNameResolver = new Func<string, string>(this.ResolvePropertyName);
			return jsonDictionaryContract;
		}

		protected virtual JsonArrayContract CreateArrayContract(Type objectType)
		{
			JsonArrayContract jsonArrayContract = new JsonArrayContract(objectType);
			this.InitializeContract(jsonArrayContract);
			return jsonArrayContract;
		}

		protected virtual JsonPrimitiveContract CreatePrimitiveContract(Type objectType)
		{
			JsonPrimitiveContract jsonPrimitiveContract = new JsonPrimitiveContract(objectType);
			this.InitializeContract(jsonPrimitiveContract);
			return jsonPrimitiveContract;
		}

		protected virtual JsonLinqContract CreateLinqContract(Type objectType)
		{
			JsonLinqContract jsonLinqContract = new JsonLinqContract(objectType);
			this.InitializeContract(jsonLinqContract);
			return jsonLinqContract;
		}

		protected virtual JsonISerializableContract CreateISerializableContract(Type objectType)
		{
			JsonISerializableContract jsonISerializableContract = new JsonISerializableContract(objectType);
			this.InitializeContract(jsonISerializableContract);
			ConstructorInfo constructor = objectType.GetConstructor(52, null, new Type[]
			{
				typeof(SerializationInfo),
				typeof(StreamingContext)
			}, null);
			if (constructor != null)
			{
				MethodCall<object, object> methodCall = JsonTypeReflector.ReflectionDelegateFactory.CreateMethodCall<object>(constructor);
				jsonISerializableContract.ISerializableCreator = ((object[] args) => methodCall(null, args));
			}
			return jsonISerializableContract;
		}

		protected virtual JsonStringContract CreateStringContract(Type objectType)
		{
			JsonStringContract jsonStringContract = new JsonStringContract(objectType);
			this.InitializeContract(jsonStringContract);
			return jsonStringContract;
		}

		protected virtual JsonContract CreateContract(Type objectType)
		{
			Type type = ReflectionUtils.EnsureNotNullableType(objectType);
			if (JsonConvert.IsJsonPrimitiveType(type))
			{
				return this.CreatePrimitiveContract(type);
			}
			if (JsonTypeReflector.GetJsonObjectAttribute(type) != null)
			{
				return this.CreateObjectContract(type);
			}
			if (JsonTypeReflector.GetJsonArrayAttribute(type) != null)
			{
				return this.CreateArrayContract(type);
			}
			if (type == typeof(JToken) || type.IsSubclassOf(typeof(JToken)))
			{
				return this.CreateLinqContract(type);
			}
			if (CollectionUtils.IsDictionaryType(type))
			{
				return this.CreateDictionaryContract(type);
			}
			if (typeof(IEnumerable).IsAssignableFrom(type))
			{
				return this.CreateArrayContract(type);
			}
			if (DefaultContractResolver.CanConvertToString(type))
			{
				return this.CreateStringContract(type);
			}
			if (typeof(ISerializable).IsAssignableFrom(type))
			{
				return this.CreateISerializableContract(type);
			}
			return this.CreateObjectContract(type);
		}

		internal static bool CanConvertToString(Type type)
		{
			TypeConverter converter = ConvertUtils.GetConverter(type);
			return (converter != null && !(converter is ComponentConverter) && !(converter is ReferenceConverter) && converter.GetType() != typeof(TypeConverter) && converter.CanConvertTo(typeof(string))) || (type == typeof(Type) || type.IsSubclassOf(typeof(Type)));
		}

		private static bool IsValidCallback(MethodInfo method, ParameterInfo[] parameters, Type attributeType, MethodInfo currentCallback, ref Type prevAttributeType)
		{
			if (!method.IsDefined(attributeType, false))
			{
				return false;
			}
			if (currentCallback != null)
			{
				throw new Exception("Invalid attribute. Both '{0}' and '{1}' in type '{2}' have '{3}'.".FormatWith(CultureInfo.get_InvariantCulture(), new object[]
				{
					method,
					currentCallback,
					DefaultContractResolver.GetClrTypeFullName(method.get_DeclaringType()),
					attributeType
				}));
			}
			if (prevAttributeType != null)
			{
				throw new Exception("Invalid Callback. Method '{3}' in type '{2}' has both '{0}' and '{1}'.".FormatWith(CultureInfo.get_InvariantCulture(), new object[]
				{
					prevAttributeType,
					attributeType,
					DefaultContractResolver.GetClrTypeFullName(method.get_DeclaringType()),
					method
				}));
			}
			if (method.get_IsVirtual())
			{
				throw new Exception("Virtual Method '{0}' of type '{1}' cannot be marked with '{2}' attribute.".FormatWith(CultureInfo.get_InvariantCulture(), new object[]
				{
					method,
					DefaultContractResolver.GetClrTypeFullName(method.get_DeclaringType()),
					attributeType
				}));
			}
			if (method.get_ReturnType() != typeof(void))
			{
				throw new Exception("Serialization Callback '{1}' in type '{0}' must return void.".FormatWith(CultureInfo.get_InvariantCulture(), new object[]
				{
					DefaultContractResolver.GetClrTypeFullName(method.get_DeclaringType()),
					method
				}));
			}
			if (attributeType == typeof(OnErrorAttribute))
			{
				if (parameters == null || parameters.Length != 2 || parameters[0].get_ParameterType() != typeof(StreamingContext) || parameters[1].get_ParameterType() != typeof(ErrorContext))
				{
					throw new Exception("Serialization Error Callback '{1}' in type '{0}' must have two parameters of type '{2}' and '{3}'.".FormatWith(CultureInfo.get_InvariantCulture(), new object[]
					{
						DefaultContractResolver.GetClrTypeFullName(method.get_DeclaringType()),
						method,
						typeof(StreamingContext),
						typeof(ErrorContext)
					}));
				}
			}
			else if (parameters == null || parameters.Length != 1 || parameters[0].get_ParameterType() != typeof(StreamingContext))
			{
				throw new Exception("Serialization Callback '{1}' in type '{0}' must have a single parameter of type '{2}'.".FormatWith(CultureInfo.get_InvariantCulture(), new object[]
				{
					DefaultContractResolver.GetClrTypeFullName(method.get_DeclaringType()),
					method,
					typeof(StreamingContext)
				}));
			}
			prevAttributeType = attributeType;
			return true;
		}

		internal static string GetClrTypeFullName(Type type)
		{
			if (type.get_IsGenericTypeDefinition() || !type.get_ContainsGenericParameters())
			{
				return type.get_FullName();
			}
			return string.Format(CultureInfo.get_InvariantCulture(), "{0}.{1}", new object[]
			{
				type.get_Namespace(),
				type.get_Name()
			});
		}

		protected virtual IList<JsonProperty> CreateProperties(Type type, MemberSerialization memberSerialization)
		{
			List<MemberInfo> serializableMembers = this.GetSerializableMembers(type);
			if (serializableMembers == null)
			{
				throw new JsonSerializationException("Null collection of seralizable members returned.");
			}
			JsonPropertyCollection jsonPropertyCollection = new JsonPropertyCollection(type);
			using (List<MemberInfo>.Enumerator enumerator = serializableMembers.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					MemberInfo current = enumerator.get_Current();
					JsonProperty jsonProperty = this.CreateProperty(current, memberSerialization);
					if (jsonProperty != null)
					{
						jsonPropertyCollection.AddProperty(jsonProperty);
					}
				}
			}
			return Enumerable.ToList<JsonProperty>(Enumerable.OrderBy<JsonProperty, int>(jsonPropertyCollection, delegate(JsonProperty p)
			{
				int? order = p.Order;
				return (!order.get_HasValue()) ? -1 : order.get_Value();
			}));
		}

		protected virtual IValueProvider CreateMemberValueProvider(MemberInfo member)
		{
			return new ReflectionValueProvider(member);
		}

		protected virtual JsonProperty CreateProperty(MemberInfo member, MemberSerialization memberSerialization)
		{
			JsonProperty jsonProperty = new JsonProperty();
			jsonProperty.PropertyType = ReflectionUtils.GetMemberUnderlyingType(member);
			jsonProperty.ValueProvider = this.CreateMemberValueProvider(member);
			bool flag;
			bool canSetReadOnly;
			this.SetPropertySettingsFromAttributes(jsonProperty, member, member.get_Name(), member.get_DeclaringType(), memberSerialization, out flag, out canSetReadOnly);
			jsonProperty.Readable = ReflectionUtils.CanReadMemberValue(member, flag);
			jsonProperty.Writable = ReflectionUtils.CanSetMemberValue(member, flag, canSetReadOnly);
			jsonProperty.ShouldSerialize = this.CreateShouldSerializeTest(member);
			this.SetIsSpecifiedActions(jsonProperty, member, flag);
			return jsonProperty;
		}

		private void SetPropertySettingsFromAttributes(JsonProperty property, ICustomAttributeProvider attributeProvider, string name, Type declaringType, MemberSerialization memberSerialization, out bool allowNonPublicAccess, out bool hasExplicitAttribute)
		{
			hasExplicitAttribute = false;
			DataContractAttribute dataContractAttribute = JsonTypeReflector.GetDataContractAttribute(declaringType);
			DataMemberAttribute dataMemberAttribute;
			if (dataContractAttribute != null && attributeProvider is MemberInfo)
			{
				dataMemberAttribute = JsonTypeReflector.GetDataMemberAttribute((MemberInfo)attributeProvider);
			}
			else
			{
				dataMemberAttribute = null;
			}
			JsonPropertyAttribute attribute = JsonTypeReflector.GetAttribute<JsonPropertyAttribute>(attributeProvider);
			if (attribute != null)
			{
				hasExplicitAttribute = true;
			}
			bool flag = JsonTypeReflector.GetAttribute<JsonIgnoreAttribute>(attributeProvider) != null;
			string propertyName;
			if (attribute != null && attribute.PropertyName != null)
			{
				propertyName = attribute.PropertyName;
			}
			else if (dataMemberAttribute != null && dataMemberAttribute.Name != null)
			{
				propertyName = dataMemberAttribute.Name;
			}
			else
			{
				propertyName = name;
			}
			property.PropertyName = this.ResolvePropertyName(propertyName);
			property.UnderlyingName = name;
			if (attribute != null)
			{
				property.Required = attribute.Required;
				property.Order = attribute._order;
			}
			else if (dataMemberAttribute != null)
			{
				property.Required = ((!dataMemberAttribute.IsRequired) ? Required.Default : Required.AllowNull);
				property.Order = ((dataMemberAttribute.Order == -1) ? default(int?) : new int?(dataMemberAttribute.Order));
			}
			else
			{
				property.Required = Required.Default;
			}
			property.Ignored = (flag || (memberSerialization == MemberSerialization.OptIn && attribute == null && dataMemberAttribute == null));
			property.Converter = JsonTypeReflector.GetJsonConverter(attributeProvider, property.PropertyType);
			property.MemberConverter = JsonTypeReflector.GetJsonConverter(attributeProvider, property.PropertyType);
			DefaultValueAttribute attribute2 = JsonTypeReflector.GetAttribute<DefaultValueAttribute>(attributeProvider);
			property.DefaultValue = ((attribute2 == null) ? null : attribute2.get_Value());
			property.NullValueHandling = ((attribute == null) ? default(NullValueHandling?) : attribute._nullValueHandling);
			property.DefaultValueHandling = ((attribute == null) ? default(DefaultValueHandling?) : attribute._defaultValueHandling);
			property.ReferenceLoopHandling = ((attribute == null) ? default(ReferenceLoopHandling?) : attribute._referenceLoopHandling);
			property.ObjectCreationHandling = ((attribute == null) ? default(ObjectCreationHandling?) : attribute._objectCreationHandling);
			property.TypeNameHandling = ((attribute == null) ? default(TypeNameHandling?) : attribute._typeNameHandling);
			property.IsReference = ((attribute == null) ? default(bool?) : attribute._isReference);
			allowNonPublicAccess = false;
			if ((this.DefaultMembersSearchFlags & 32) == 32)
			{
				allowNonPublicAccess = true;
			}
			if (attribute != null)
			{
				allowNonPublicAccess = true;
			}
			if (dataMemberAttribute != null)
			{
				allowNonPublicAccess = true;
				hasExplicitAttribute = true;
			}
		}

		private Predicate<object> CreateShouldSerializeTest(MemberInfo member)
		{
			MethodInfo method = member.get_DeclaringType().GetMethod("ShouldSerialize" + member.get_Name(), new Type[0]);
			if (method == null || method.get_ReturnType() != typeof(bool))
			{
				return null;
			}
			MethodCall<object, object> shouldSerializeCall = JsonTypeReflector.ReflectionDelegateFactory.CreateMethodCall<object>(method);
			return (object o) => (bool)shouldSerializeCall(o, new object[0]);
		}

		private void SetIsSpecifiedActions(JsonProperty property, MemberInfo member, bool allowNonPublicAccess)
		{
			MemberInfo memberInfo = member.get_DeclaringType().GetProperty(member.get_Name() + "Specified");
			if (memberInfo == null)
			{
				memberInfo = member.get_DeclaringType().GetField(member.get_Name() + "Specified");
			}
			if (memberInfo == null || ReflectionUtils.GetMemberUnderlyingType(memberInfo) != typeof(bool))
			{
				return;
			}
			Func<object, object> specifiedPropertyGet = JsonTypeReflector.ReflectionDelegateFactory.CreateGet<object>(memberInfo);
			property.GetIsSpecified = ((object o) => (bool)specifiedPropertyGet.Invoke(o));
			if (ReflectionUtils.CanSetMemberValue(memberInfo, allowNonPublicAccess, false))
			{
				property.SetIsSpecified = JsonTypeReflector.ReflectionDelegateFactory.CreateSet<object>(memberInfo);
			}
		}

		protected internal virtual string ResolvePropertyName(string propertyName)
		{
			return propertyName;
		}
	}
}

using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters;
using System.Text;

namespace Newtonsoft.Json.Utilities
{
	internal static class ReflectionUtils
	{
		public static bool IsVirtual(this PropertyInfo propertyInfo)
		{
			ValidationUtils.ArgumentNotNull(propertyInfo, "propertyInfo");
			MethodInfo methodInfo = propertyInfo.GetGetMethod();
			if (methodInfo != null && methodInfo.get_IsVirtual())
			{
				return true;
			}
			methodInfo = propertyInfo.GetSetMethod();
			return methodInfo != null && methodInfo.get_IsVirtual();
		}

		public static Type GetObjectType(object v)
		{
			return (v == null) ? null : v.GetType();
		}

		public static string GetTypeName(Type t, FormatterAssemblyStyle assemblyFormat)
		{
			return ReflectionUtils.GetTypeName(t, assemblyFormat, null);
		}

		public static string GetTypeName(Type t, FormatterAssemblyStyle assemblyFormat, SerializationBinder binder)
		{
			string assemblyQualifiedName = t.get_AssemblyQualifiedName();
			if (assemblyFormat == null)
			{
				return ReflectionUtils.RemoveAssemblyDetails(assemblyQualifiedName);
			}
			if (assemblyFormat != 1)
			{
				throw new ArgumentOutOfRangeException();
			}
			return t.get_AssemblyQualifiedName();
		}

		private static string RemoveAssemblyDetails(string fullyQualifiedTypeName)
		{
			StringBuilder stringBuilder = new StringBuilder();
			bool flag = false;
			bool flag2 = false;
			for (int i = 0; i < fullyQualifiedTypeName.get_Length(); i++)
			{
				char c = fullyQualifiedTypeName.get_Chars(i);
				char c2 = c;
				switch (c2)
				{
				case '[':
					flag = false;
					flag2 = false;
					stringBuilder.Append(c);
					goto IL_97;
				case '\\':
					IL_34:
					if (c2 != ',')
					{
						if (!flag2)
						{
							stringBuilder.Append(c);
						}
						goto IL_97;
					}
					if (!flag)
					{
						flag = true;
						stringBuilder.Append(c);
					}
					else
					{
						flag2 = true;
					}
					goto IL_97;
				case ']':
					flag = false;
					flag2 = false;
					stringBuilder.Append(c);
					goto IL_97;
				}
				goto IL_34;
				IL_97:;
			}
			return stringBuilder.ToString();
		}

		public static bool IsInstantiatableType(Type t)
		{
			ValidationUtils.ArgumentNotNull(t, "t");
			return !t.get_IsAbstract() && !t.get_IsInterface() && !t.get_IsArray() && !t.get_IsGenericTypeDefinition() && t != typeof(void) && ReflectionUtils.HasDefaultConstructor(t);
		}

		public static bool HasDefaultConstructor(Type t)
		{
			return ReflectionUtils.HasDefaultConstructor(t, false);
		}

		public static bool HasDefaultConstructor(Type t, bool nonPublic)
		{
			ValidationUtils.ArgumentNotNull(t, "t");
			return t.get_IsValueType() || ReflectionUtils.GetDefaultConstructor(t, nonPublic) != null;
		}

		public static ConstructorInfo GetDefaultConstructor(Type t)
		{
			return ReflectionUtils.GetDefaultConstructor(t, false);
		}

		public static ConstructorInfo GetDefaultConstructor(Type t, bool nonPublic)
		{
			BindingFlags bindingFlags = 16;
			if (nonPublic)
			{
				bindingFlags |= 32;
			}
			return t.GetConstructor(bindingFlags | 4, null, new Type[0], null);
		}

		public static bool IsNullable(Type t)
		{
			ValidationUtils.ArgumentNotNull(t, "t");
			return !t.get_IsValueType() || ReflectionUtils.IsNullableType(t);
		}

		public static bool IsNullableType(Type t)
		{
			ValidationUtils.ArgumentNotNull(t, "t");
			return t.get_IsGenericType() && t.GetGenericTypeDefinition() == typeof(Nullable);
		}

		public static Type EnsureNotNullableType(Type t)
		{
			return (!ReflectionUtils.IsNullableType(t)) ? t : Nullable.GetUnderlyingType(t);
		}

		public static bool IsUnitializedValue(object value)
		{
			if (value == null)
			{
				return true;
			}
			object obj = ReflectionUtils.CreateUnitializedValue(value.GetType());
			return value.Equals(obj);
		}

		public static object CreateUnitializedValue(Type type)
		{
			ValidationUtils.ArgumentNotNull(type, "type");
			if (type.get_IsGenericTypeDefinition())
			{
				throw new ArgumentException("Type {0} is a generic type definition and cannot be instantiated.".FormatWith(CultureInfo.get_InvariantCulture(), new object[]
				{
					type
				}), "type");
			}
			if (type.get_IsClass() || type.get_IsInterface() || type == typeof(void))
			{
				return null;
			}
			if (type.get_IsValueType())
			{
				return Activator.CreateInstance(type);
			}
			throw new ArgumentException("Type {0} cannot be instantiated.".FormatWith(CultureInfo.get_InvariantCulture(), new object[]
			{
				type
			}), "type");
		}

		public static bool IsPropertyIndexed(PropertyInfo property)
		{
			ValidationUtils.ArgumentNotNull(property, "property");
			return !CollectionUtils.IsNullOrEmpty<ParameterInfo>(property.GetIndexParameters());
		}

		public static bool ImplementsGenericDefinition(Type type, Type genericInterfaceDefinition)
		{
			Type type2;
			return ReflectionUtils.ImplementsGenericDefinition(type, genericInterfaceDefinition, out type2);
		}

		public static bool ImplementsGenericDefinition(Type type, Type genericInterfaceDefinition, out Type implementingType)
		{
			ValidationUtils.ArgumentNotNull(type, "type");
			ValidationUtils.ArgumentNotNull(genericInterfaceDefinition, "genericInterfaceDefinition");
			if (!genericInterfaceDefinition.get_IsInterface() || !genericInterfaceDefinition.get_IsGenericTypeDefinition())
			{
				throw new ArgumentNullException("'{0}' is not a generic interface definition.".FormatWith(CultureInfo.get_InvariantCulture(), new object[]
				{
					genericInterfaceDefinition
				}));
			}
			if (type.get_IsInterface() && type.get_IsGenericType())
			{
				Type genericTypeDefinition = type.GetGenericTypeDefinition();
				if (genericInterfaceDefinition == genericTypeDefinition)
				{
					implementingType = type;
					return true;
				}
			}
			Type[] interfaces = type.GetInterfaces();
			for (int i = 0; i < interfaces.Length; i++)
			{
				Type type2 = interfaces[i];
				if (type2.get_IsGenericType())
				{
					Type genericTypeDefinition2 = type2.GetGenericTypeDefinition();
					if (genericInterfaceDefinition == genericTypeDefinition2)
					{
						implementingType = type2;
						return true;
					}
				}
			}
			implementingType = null;
			return false;
		}

		public static bool AssignableToTypeName(this Type type, string fullTypeName, out Type match)
		{
			for (Type type2 = type; type2 != null; type2 = type2.get_BaseType())
			{
				if (string.Equals(type2.get_FullName(), fullTypeName, 4))
				{
					match = type2;
					return true;
				}
			}
			Type[] interfaces = type.GetInterfaces();
			for (int i = 0; i < interfaces.Length; i++)
			{
				Type type3 = interfaces[i];
				if (string.Equals(type3.get_Name(), fullTypeName, 4))
				{
					match = type;
					return true;
				}
			}
			match = null;
			return false;
		}

		public static bool AssignableToTypeName(this Type type, string fullTypeName)
		{
			Type type2;
			return type.AssignableToTypeName(fullTypeName, out type2);
		}

		public static bool InheritsGenericDefinition(Type type, Type genericClassDefinition)
		{
			Type type2;
			return ReflectionUtils.InheritsGenericDefinition(type, genericClassDefinition, out type2);
		}

		public static bool InheritsGenericDefinition(Type type, Type genericClassDefinition, out Type implementingType)
		{
			ValidationUtils.ArgumentNotNull(type, "type");
			ValidationUtils.ArgumentNotNull(genericClassDefinition, "genericClassDefinition");
			if (!genericClassDefinition.get_IsClass() || !genericClassDefinition.get_IsGenericTypeDefinition())
			{
				throw new ArgumentNullException("'{0}' is not a generic class definition.".FormatWith(CultureInfo.get_InvariantCulture(), new object[]
				{
					genericClassDefinition
				}));
			}
			return ReflectionUtils.InheritsGenericDefinitionInternal(type, genericClassDefinition, out implementingType);
		}

		private static bool InheritsGenericDefinitionInternal(Type currentType, Type genericClassDefinition, out Type implementingType)
		{
			if (currentType.get_IsGenericType())
			{
				Type genericTypeDefinition = currentType.GetGenericTypeDefinition();
				if (genericClassDefinition == genericTypeDefinition)
				{
					implementingType = currentType;
					return true;
				}
			}
			if (currentType.get_BaseType() == null)
			{
				implementingType = null;
				return false;
			}
			return ReflectionUtils.InheritsGenericDefinitionInternal(currentType.get_BaseType(), genericClassDefinition, out implementingType);
		}

		public static Type GetCollectionItemType(Type type)
		{
			ValidationUtils.ArgumentNotNull(type, "type");
			if (type.get_IsArray())
			{
				return type.GetElementType();
			}
			Type type2;
			if (ReflectionUtils.ImplementsGenericDefinition(type, typeof(IEnumerable), out type2))
			{
				if (type2.get_IsGenericTypeDefinition())
				{
					throw new Exception("Type {0} is not a collection.".FormatWith(CultureInfo.get_InvariantCulture(), new object[]
					{
						type
					}));
				}
				return type2.GetGenericArguments()[0];
			}
			else
			{
				if (typeof(IEnumerable).IsAssignableFrom(type))
				{
					return null;
				}
				throw new Exception("Type {0} is not a collection.".FormatWith(CultureInfo.get_InvariantCulture(), new object[]
				{
					type
				}));
			}
		}

		public static void GetDictionaryKeyValueTypes(Type dictionaryType, out Type keyType, out Type valueType)
		{
			ValidationUtils.ArgumentNotNull(dictionaryType, "type");
			Type type;
			if (ReflectionUtils.ImplementsGenericDefinition(dictionaryType, typeof(IDictionary), out type))
			{
				if (type.get_IsGenericTypeDefinition())
				{
					throw new Exception("Type {0} is not a dictionary.".FormatWith(CultureInfo.get_InvariantCulture(), new object[]
					{
						dictionaryType
					}));
				}
				Type[] genericArguments = type.GetGenericArguments();
				keyType = genericArguments[0];
				valueType = genericArguments[1];
				return;
			}
			else
			{
				if (typeof(IDictionary).IsAssignableFrom(dictionaryType))
				{
					keyType = null;
					valueType = null;
					return;
				}
				throw new Exception("Type {0} is not a dictionary.".FormatWith(CultureInfo.get_InvariantCulture(), new object[]
				{
					dictionaryType
				}));
			}
		}

		public static Type GetDictionaryValueType(Type dictionaryType)
		{
			Type type;
			Type result;
			ReflectionUtils.GetDictionaryKeyValueTypes(dictionaryType, out type, out result);
			return result;
		}

		public static Type GetDictionaryKeyType(Type dictionaryType)
		{
			Type result;
			Type type;
			ReflectionUtils.GetDictionaryKeyValueTypes(dictionaryType, out result, out type);
			return result;
		}

		public static bool ItemsUnitializedValue<T>(IList<T> list)
		{
			ValidationUtils.ArgumentNotNull(list, "list");
			Type collectionItemType = ReflectionUtils.GetCollectionItemType(list.GetType());
			if (collectionItemType.get_IsValueType())
			{
				object obj = ReflectionUtils.CreateUnitializedValue(collectionItemType);
				for (int i = 0; i < list.get_Count(); i++)
				{
					T t = list.get_Item(i);
					if (!t.Equals(obj))
					{
						return false;
					}
				}
			}
			else
			{
				if (!collectionItemType.get_IsClass())
				{
					throw new Exception("Type {0} is neither a ValueType or a Class.".FormatWith(CultureInfo.get_InvariantCulture(), new object[]
					{
						collectionItemType
					}));
				}
				for (int j = 0; j < list.get_Count(); j++)
				{
					object obj2 = list.get_Item(j);
					if (obj2 != null)
					{
						return false;
					}
				}
			}
			return true;
		}

		public static Type GetMemberUnderlyingType(MemberInfo member)
		{
			ValidationUtils.ArgumentNotNull(member, "member");
			MemberTypes memberType = member.get_MemberType();
			switch (memberType)
			{
			case 2:
				return ((EventInfo)member).get_EventHandlerType();
			case 3:
				IL_26:
				if (memberType != 16)
				{
					throw new ArgumentException("MemberInfo must be of type FieldInfo, PropertyInfo or EventInfo", "member");
				}
				return ((PropertyInfo)member).get_PropertyType();
			case 4:
				return ((FieldInfo)member).get_FieldType();
			}
			goto IL_26;
		}

		public static bool IsIndexedProperty(MemberInfo member)
		{
			ValidationUtils.ArgumentNotNull(member, "member");
			PropertyInfo propertyInfo = member as PropertyInfo;
			return propertyInfo != null && ReflectionUtils.IsIndexedProperty(propertyInfo);
		}

		public static bool IsIndexedProperty(PropertyInfo property)
		{
			ValidationUtils.ArgumentNotNull(property, "property");
			return property.GetIndexParameters().Length > 0;
		}

		public static object GetMemberValue(MemberInfo member, object target)
		{
			ValidationUtils.ArgumentNotNull(member, "member");
			ValidationUtils.ArgumentNotNull(target, "target");
			MemberTypes memberType = member.get_MemberType();
			if (memberType != 4)
			{
				if (memberType == 16)
				{
					try
					{
						return ((PropertyInfo)member).GetValue(target, null);
					}
					catch (TargetParameterCountException ex)
					{
						throw new ArgumentException("MemberInfo '{0}' has index parameters".FormatWith(CultureInfo.get_InvariantCulture(), new object[]
						{
							member.get_Name()
						}), ex);
					}
				}
				throw new ArgumentException("MemberInfo '{0}' is not of type FieldInfo or PropertyInfo".FormatWith(CultureInfo.get_InvariantCulture(), new object[]
				{
					CultureInfo.get_InvariantCulture(),
					member.get_Name()
				}), "member");
			}
			return ((FieldInfo)member).GetValue(target);
		}

		public static void SetMemberValue(MemberInfo member, object target, object value)
		{
			ValidationUtils.ArgumentNotNull(member, "member");
			ValidationUtils.ArgumentNotNull(target, "target");
			MemberTypes memberType = member.get_MemberType();
			if (memberType != 4)
			{
				if (memberType != 16)
				{
					throw new ArgumentException("MemberInfo '{0}' must be of type FieldInfo or PropertyInfo".FormatWith(CultureInfo.get_InvariantCulture(), new object[]
					{
						member.get_Name()
					}), "member");
				}
				((PropertyInfo)member).SetValue(target, value, null);
			}
			else
			{
				((FieldInfo)member).SetValue(target, value);
			}
		}

		public static bool CanReadMemberValue(MemberInfo member, bool nonPublic)
		{
			MemberTypes memberType = member.get_MemberType();
			if (memberType == 4)
			{
				FieldInfo fieldInfo = (FieldInfo)member;
				return nonPublic || fieldInfo.get_IsPublic();
			}
			if (memberType != 16)
			{
				return false;
			}
			PropertyInfo propertyInfo = (PropertyInfo)member;
			return propertyInfo.get_CanRead() && (nonPublic || propertyInfo.GetGetMethod(nonPublic) != null);
		}

		public static bool CanSetMemberValue(MemberInfo member, bool nonPublic, bool canSetReadOnly)
		{
			MemberTypes memberType = member.get_MemberType();
			if (memberType == 4)
			{
				FieldInfo fieldInfo = (FieldInfo)member;
				return (!fieldInfo.get_IsInitOnly() || canSetReadOnly) && (nonPublic || fieldInfo.get_IsPublic());
			}
			if (memberType != 16)
			{
				return false;
			}
			PropertyInfo propertyInfo = (PropertyInfo)member;
			return propertyInfo.get_CanWrite() && (nonPublic || propertyInfo.GetSetMethod(nonPublic) != null);
		}

		public static List<MemberInfo> GetFieldsAndProperties<T>(BindingFlags bindingAttr)
		{
			return ReflectionUtils.GetFieldsAndProperties(typeof(T), bindingAttr);
		}

		public static List<MemberInfo> GetFieldsAndProperties(Type type, BindingFlags bindingAttr)
		{
			List<MemberInfo> list = new List<MemberInfo>();
			list.AddRange(ReflectionUtils.GetFields(type, bindingAttr));
			list.AddRange(ReflectionUtils.GetProperties(type, bindingAttr));
			List<MemberInfo> list2 = new List<MemberInfo>(list.get_Count());
			var enumerable = Enumerable.Select(Enumerable.GroupBy<MemberInfo, string>(list, (MemberInfo m) => m.get_Name()), (IGrouping<string, MemberInfo> g) => new
			{
				Count = Enumerable.Count<MemberInfo>(g),
				Members = Enumerable.Cast<MemberInfo>(g)
			});
			using (var enumerator = enumerable.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					var current = enumerator.get_Current();
					if (current.Count == 1)
					{
						list2.Add(Enumerable.First<MemberInfo>(current.Members));
					}
					else
					{
						IEnumerable<MemberInfo> enumerable2 = Enumerable.Where<MemberInfo>(current.Members, (MemberInfo m) => !ReflectionUtils.IsOverridenGenericMember(m, bindingAttr) || m.get_Name() == "Item");
						list2.AddRange(enumerable2);
					}
				}
			}
			return list2;
		}

		private static bool IsOverridenGenericMember(MemberInfo memberInfo, BindingFlags bindingAttr)
		{
			if (memberInfo.get_MemberType() != 4 && memberInfo.get_MemberType() != 16)
			{
				throw new ArgumentException("Member must be a field or property.");
			}
			Type declaringType = memberInfo.get_DeclaringType();
			if (!declaringType.get_IsGenericType())
			{
				return false;
			}
			Type genericTypeDefinition = declaringType.GetGenericTypeDefinition();
			if (genericTypeDefinition == null)
			{
				return false;
			}
			MemberInfo[] member = genericTypeDefinition.GetMember(memberInfo.get_Name(), bindingAttr);
			if (member.Length == 0)
			{
				return false;
			}
			Type memberUnderlyingType = ReflectionUtils.GetMemberUnderlyingType(member[0]);
			return memberUnderlyingType.get_IsGenericParameter();
		}

		public static T GetAttribute<T>(ICustomAttributeProvider attributeProvider) where T : Attribute
		{
			return ReflectionUtils.GetAttribute<T>(attributeProvider, true);
		}

		public static T GetAttribute<T>(ICustomAttributeProvider attributeProvider, bool inherit) where T : Attribute
		{
			T[] attributes = ReflectionUtils.GetAttributes<T>(attributeProvider, inherit);
			return CollectionUtils.GetSingleItem<T>(attributes, true);
		}

		public static T[] GetAttributes<T>(ICustomAttributeProvider attributeProvider, bool inherit) where T : Attribute
		{
			ValidationUtils.ArgumentNotNull(attributeProvider, "attributeProvider");
			if (attributeProvider is Type)
			{
				return (T[])((Type)attributeProvider).GetCustomAttributes(typeof(T), inherit);
			}
			if (attributeProvider is Assembly)
			{
				return (T[])Attribute.GetCustomAttributes((Assembly)attributeProvider, typeof(T), inherit);
			}
			if (attributeProvider is MemberInfo)
			{
				return (T[])Attribute.GetCustomAttributes((MemberInfo)attributeProvider, typeof(T), inherit);
			}
			if (attributeProvider is Module)
			{
				return (T[])Attribute.GetCustomAttributes((Module)attributeProvider, typeof(T), inherit);
			}
			if (attributeProvider is ParameterInfo)
			{
				return (T[])Attribute.GetCustomAttributes((ParameterInfo)attributeProvider, typeof(T), inherit);
			}
			return (T[])attributeProvider.GetCustomAttributes(typeof(T), inherit);
		}

		public static string GetNameAndAssessmblyName(Type t)
		{
			ValidationUtils.ArgumentNotNull(t, "t");
			return t.get_FullName() + ", " + t.get_Assembly().GetName().get_Name();
		}

		public static Type MakeGenericType(Type genericTypeDefinition, params Type[] innerTypes)
		{
			ValidationUtils.ArgumentNotNull(genericTypeDefinition, "genericTypeDefinition");
			ValidationUtils.ArgumentNotNullOrEmpty<Type>(innerTypes, "innerTypes");
			ValidationUtils.ArgumentConditionTrue(genericTypeDefinition.get_IsGenericTypeDefinition(), "genericTypeDefinition", "Type {0} is not a generic type definition.".FormatWith(CultureInfo.get_InvariantCulture(), new object[]
			{
				genericTypeDefinition
			}));
			return genericTypeDefinition.MakeGenericType(innerTypes);
		}

		public static object CreateGeneric(Type genericTypeDefinition, Type innerType, params object[] args)
		{
			return ReflectionUtils.CreateGeneric(genericTypeDefinition, new Type[]
			{
				innerType
			}, args);
		}

		public static object CreateGeneric(Type genericTypeDefinition, IList<Type> innerTypes, params object[] args)
		{
			return ReflectionUtils.CreateGeneric(genericTypeDefinition, innerTypes, (Type t, IList<object> a) => ReflectionUtils.CreateInstance(t, Enumerable.ToArray<object>(a)), args);
		}

		public static object CreateGeneric(Type genericTypeDefinition, IList<Type> innerTypes, Func<Type, IList<object>, object> instanceCreator, params object[] args)
		{
			ValidationUtils.ArgumentNotNull(genericTypeDefinition, "genericTypeDefinition");
			ValidationUtils.ArgumentNotNullOrEmpty<Type>(innerTypes, "innerTypes");
			ValidationUtils.ArgumentNotNull(instanceCreator, "createInstance");
			Type type = ReflectionUtils.MakeGenericType(genericTypeDefinition, Enumerable.ToArray<Type>(innerTypes));
			return instanceCreator.Invoke(type, args);
		}

		public static bool IsCompatibleValue(object value, Type type)
		{
			if (value == null)
			{
				return ReflectionUtils.IsNullable(type);
			}
			return type.IsAssignableFrom(value.GetType());
		}

		public static object CreateInstance(Type type, params object[] args)
		{
			ValidationUtils.ArgumentNotNull(type, "type");
			return Activator.CreateInstance(type, args);
		}

		public static void SplitFullyQualifiedTypeName(string fullyQualifiedTypeName, out string typeName, out string assemblyName)
		{
			int? assemblyDelimiterIndex = ReflectionUtils.GetAssemblyDelimiterIndex(fullyQualifiedTypeName);
			if (assemblyDelimiterIndex.get_HasValue())
			{
				typeName = fullyQualifiedTypeName.Substring(0, assemblyDelimiterIndex.get_Value()).Trim();
				assemblyName = fullyQualifiedTypeName.Substring(assemblyDelimiterIndex.get_Value() + 1, fullyQualifiedTypeName.get_Length() - assemblyDelimiterIndex.get_Value() - 1).Trim();
			}
			else
			{
				typeName = fullyQualifiedTypeName;
				assemblyName = null;
			}
		}

		private static int? GetAssemblyDelimiterIndex(string fullyQualifiedTypeName)
		{
			int num = 0;
			for (int i = 0; i < fullyQualifiedTypeName.get_Length(); i++)
			{
				char c = fullyQualifiedTypeName.get_Chars(i);
				char c2 = c;
				switch (c2)
				{
				case '[':
					num++;
					goto IL_59;
				case '\\':
					IL_28:
					if (c2 != ',')
					{
						goto IL_59;
					}
					if (num == 0)
					{
						return new int?(i);
					}
					goto IL_59;
				case ']':
					num--;
					goto IL_59;
				}
				goto IL_28;
				IL_59:;
			}
			return default(int?);
		}

		public static MemberInfo GetMemberInfoFromType(Type targetType, MemberInfo memberInfo)
		{
			BindingFlags bindingFlags = 60;
			MemberTypes memberType = memberInfo.get_MemberType();
			if (memberType != 16)
			{
				return Enumerable.SingleOrDefault<MemberInfo>(targetType.GetMember(memberInfo.get_Name(), memberInfo.get_MemberType(), bindingFlags));
			}
			PropertyInfo propertyInfo = (PropertyInfo)memberInfo;
			Type[] array = Enumerable.ToArray<Type>(Enumerable.Select<ParameterInfo, Type>(propertyInfo.GetIndexParameters(), (ParameterInfo p) => p.get_ParameterType()));
			return targetType.GetProperty(propertyInfo.get_Name(), bindingFlags, null, propertyInfo.get_PropertyType(), array, null);
		}

		public static IEnumerable<FieldInfo> GetFields(Type targetType, BindingFlags bindingAttr)
		{
			ValidationUtils.ArgumentNotNull(targetType, "targetType");
			List<MemberInfo> list = new List<MemberInfo>(targetType.GetFields(bindingAttr));
			ReflectionUtils.GetChildPrivateFields(list, targetType, bindingAttr);
			return Enumerable.Cast<FieldInfo>(list);
		}

		private static void GetChildPrivateFields(IList<MemberInfo> initialFields, Type targetType, BindingFlags bindingAttr)
		{
			if ((bindingAttr & 32) != null)
			{
				BindingFlags bindingFlags = bindingAttr.RemoveFlag(16);
				while ((targetType = targetType.get_BaseType()) != null)
				{
					IEnumerable<MemberInfo> collection = Enumerable.Cast<MemberInfo>(Enumerable.Where<FieldInfo>(targetType.GetFields(bindingFlags), (FieldInfo f) => f.get_IsPrivate()));
					initialFields.AddRange(collection);
				}
			}
		}

		public static IEnumerable<PropertyInfo> GetProperties(Type targetType, BindingFlags bindingAttr)
		{
			ValidationUtils.ArgumentNotNull(targetType, "targetType");
			List<PropertyInfo> list = new List<PropertyInfo>(targetType.GetProperties(bindingAttr));
			ReflectionUtils.GetChildPrivateProperties(list, targetType, bindingAttr);
			for (int i = 0; i < list.get_Count(); i++)
			{
				PropertyInfo propertyInfo = list.get_Item(i);
				if (propertyInfo.get_DeclaringType() != targetType)
				{
					PropertyInfo propertyInfo2 = (PropertyInfo)ReflectionUtils.GetMemberInfoFromType(propertyInfo.get_DeclaringType(), propertyInfo);
					list.set_Item(i, propertyInfo2);
				}
			}
			return list;
		}

		public static BindingFlags RemoveFlag(this BindingFlags bindingAttr, BindingFlags flag)
		{
			return ((bindingAttr & flag) != flag) ? bindingAttr : (bindingAttr ^ flag);
		}

		private static void GetChildPrivateProperties(IList<PropertyInfo> initialProperties, Type targetType, BindingFlags bindingAttr)
		{
			if ((bindingAttr & 32) != null)
			{
				BindingFlags bindingFlags = bindingAttr.RemoveFlag(16);
				while ((targetType = targetType.get_BaseType()) != null)
				{
					PropertyInfo[] properties = targetType.GetProperties(bindingFlags);
					for (int i = 0; i < properties.Length; i++)
					{
						PropertyInfo nonPublicProperty2 = properties[i];
						PropertyInfo nonPublicProperty = nonPublicProperty2;
						int num = initialProperties.IndexOf((PropertyInfo p) => p.get_Name() == nonPublicProperty.get_Name());
						if (num == -1)
						{
							initialProperties.Add(nonPublicProperty);
						}
						else
						{
							initialProperties.set_Item(num, nonPublicProperty);
						}
					}
				}
			}
		}
	}
}

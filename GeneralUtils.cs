using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;

public static class GeneralUtils
{
	public const float DEVELOPMENT_BUILD_TEXT_WIDTH = 115f;

	public static void Swap<T>(ref T a, ref T b)
	{
		T t = a;
		a = b;
		b = t;
	}

	public static void ListSwap<T>(IList<T> list, int indexA, int indexB)
	{
		T t = list.get_Item(indexA);
		list.set_Item(indexA, list.get_Item(indexB));
		list.set_Item(indexB, t);
	}

	public static void ListMove<T>(IList<T> list, int srcIndex, int dstIndex)
	{
		if (srcIndex == dstIndex)
		{
			return;
		}
		T t = list.get_Item(srcIndex);
		list.RemoveAt(srcIndex);
		if (dstIndex > srcIndex)
		{
			dstIndex--;
		}
		list.Insert(dstIndex, t);
	}

	public static T[] Combine<T>(T[] arr1, T[] arr2)
	{
		T[] array = new T[arr1.Length + arr2.Length];
		Array.Copy(arr1, 0, array, 0, arr1.Length);
		Array.Copy(arr1, 0, array, arr1.Length, arr2.Length);
		return array;
	}

	public static bool IsOverriddenMethod(MethodInfo childMethod, MethodInfo ancestorMethod)
	{
		if (childMethod == null)
		{
			return false;
		}
		if (ancestorMethod == null)
		{
			return false;
		}
		if (childMethod.Equals(ancestorMethod))
		{
			return false;
		}
		MethodInfo baseDefinition = childMethod.GetBaseDefinition();
		while (!baseDefinition.Equals(childMethod) && !baseDefinition.Equals(ancestorMethod))
		{
			MethodInfo methodInfo = baseDefinition;
			baseDefinition = baseDefinition.GetBaseDefinition();
			if (baseDefinition.Equals(methodInfo))
			{
				return false;
			}
		}
		return baseDefinition.Equals(ancestorMethod);
	}

	public static bool IsObjectAlive(object obj)
	{
		return obj != null;
	}

	public static bool CallbackIsValid(Delegate callback)
	{
		bool flag = true;
		if (callback == null)
		{
			flag = false;
		}
		else if (!callback.get_Method().get_IsStatic())
		{
			object target = callback.get_Target();
			flag = GeneralUtils.IsObjectAlive(target);
			if (!flag)
			{
				Console.WriteLine(string.Format("Target for callback {0} is null.", callback.get_Method().get_Name()));
			}
		}
		return flag;
	}

	public static bool IsEditorPlaying()
	{
		return false;
	}

	public static bool TryParseBool(string strVal, out bool boolVal)
	{
		string text = strVal.ToLowerInvariant().Trim();
		if (text == "off" || text == "0" || text == "false")
		{
			boolVal = false;
			return true;
		}
		if (text == "on" || text == "1" || text == "true")
		{
			boolVal = true;
			return true;
		}
		boolVal = false;
		return false;
	}

	public static bool ForceBool(string strVal)
	{
		string text = strVal.ToLowerInvariant().Trim();
		return text == "on" || text == "1" || text == "true";
	}

	public static bool TryParseInt(string str, out int val)
	{
		return int.TryParse(str, 511, null, ref val);
	}

	public static int ForceInt(string str)
	{
		int result = 0;
		GeneralUtils.TryParseInt(str, out result);
		return result;
	}

	public static bool TryParseLong(string str, out long val)
	{
		return long.TryParse(str, 511, null, ref val);
	}

	public static long ForceLong(string str)
	{
		long result = 0L;
		GeneralUtils.TryParseLong(str, out result);
		return result;
	}

	public static bool TryParseFloat(string str, out float val)
	{
		return float.TryParse(str, 511, null, ref val);
	}

	public static float ForceFloat(string str)
	{
		float result = 0f;
		GeneralUtils.TryParseFloat(str, out result);
		return result;
	}

	public static int UnsignedMod(int x, int y)
	{
		int num = x % y;
		if (num < 0)
		{
			num += y;
		}
		return num;
	}

	public static bool AreArraysEqual<T>(T[] arr1, T[] arr2)
	{
		if (arr1 == arr2)
		{
			return true;
		}
		if (arr1 == null)
		{
			return false;
		}
		if (arr2 == null)
		{
			return false;
		}
		if (arr1.Length != arr2.Length)
		{
			return false;
		}
		for (int i = 0; i < arr1.Length; i++)
		{
			if (!arr1[i].Equals(arr2[i]))
			{
				return false;
			}
		}
		return true;
	}

	public static bool AreBytesEqual(byte[] bytes1, byte[] bytes2)
	{
		return GeneralUtils.AreArraysEqual<byte>(bytes1, bytes2);
	}

	public static T DeepClone<T>(T obj)
	{
		return (T)((object)GeneralUtils.CloneValue(obj, obj.GetType()));
	}

	private static object CloneClass(object obj, Type objType)
	{
		object obj2 = GeneralUtils.CreateNewType(objType);
		FieldInfo[] fields = objType.GetFields(52);
		FieldInfo[] array = fields;
		for (int i = 0; i < array.Length; i++)
		{
			FieldInfo fieldInfo = array[i];
			fieldInfo.SetValue(obj2, GeneralUtils.CloneValue(fieldInfo.GetValue(obj), fieldInfo.get_FieldType()));
		}
		return obj2;
	}

	private static object CloneValue(object src, Type type)
	{
		if (src != null && type != typeof(string) && type.get_IsClass())
		{
			if (!type.get_IsGenericType())
			{
				return GeneralUtils.CloneClass(src, type);
			}
			if (src is IDictionary)
			{
				IDictionary dictionary = src as IDictionary;
				IDictionary dictionary2 = GeneralUtils.CreateNewType(type) as IDictionary;
				Type type2 = type.GetGenericArguments()[0];
				Type type3 = type.GetGenericArguments()[1];
				IDictionaryEnumerator enumerator = dictionary.GetEnumerator();
				try
				{
					while (enumerator.MoveNext())
					{
						DictionaryEntry dictionaryEntry = (DictionaryEntry)enumerator.get_Current();
						dictionary2.Add(GeneralUtils.CloneValue(dictionaryEntry.get_Key(), type2), GeneralUtils.CloneValue(dictionaryEntry.get_Value(), type3));
					}
				}
				finally
				{
					IDisposable disposable = enumerator as IDisposable;
					if (disposable != null)
					{
						disposable.Dispose();
					}
				}
				return dictionary2;
			}
			if (src is IList)
			{
				IList list = src as IList;
				IList list2 = GeneralUtils.CreateNewType(type) as IList;
				Type type4 = type.GetGenericArguments()[0];
				IEnumerator enumerator2 = list.GetEnumerator();
				try
				{
					while (enumerator2.MoveNext())
					{
						object current = enumerator2.get_Current();
						list2.Add(GeneralUtils.CloneValue(current, type4));
					}
				}
				finally
				{
					IDisposable disposable2 = enumerator2 as IDisposable;
					if (disposable2 != null)
					{
						disposable2.Dispose();
					}
				}
				return list2;
			}
		}
		return src;
	}

	private static object CreateNewType(Type type)
	{
		object obj = Activator.CreateInstance(type);
		if (obj == null)
		{
			throw new SystemException(string.Format("Unable to instantiate type {0} with default constructor.", type.get_Name()));
		}
		return obj;
	}

	public static void DeepReset<T>(T obj)
	{
		Type typeFromHandle = typeof(T);
		T t = Activator.CreateInstance<T>();
		if (t == null)
		{
			throw new SystemException(string.Format("Unable to instantiate type {0} with default constructor.", typeFromHandle.get_Name()));
		}
		FieldInfo[] fields = typeFromHandle.GetFields(60);
		FieldInfo[] array = fields;
		for (int i = 0; i < array.Length; i++)
		{
			FieldInfo fieldInfo = array[i];
			fieldInfo.SetValue(obj, fieldInfo.GetValue(t));
		}
	}
}

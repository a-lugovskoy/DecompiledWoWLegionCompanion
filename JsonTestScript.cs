using Assets.DustinHorne.JsonDotNetUnity.TestCases;
using Assets.DustinHorne.JsonDotNetUnity.TestCases.TestModels;
using Newtonsoft.Json;
using SampleClassLibrary;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

public class JsonTestScript
{
	private const string BAD_RESULT_MESSAGE = "Incorrect Deserialized Result";

	private TextMesh _text;

	public JsonTestScript(TextMesh text)
	{
		this._text = text;
	}

	public void SerializeVector3()
	{
		this.LogStart("Vector3 Serialization");
		try
		{
			Vector3 vector = new Vector3(2f, 4f, 6f);
			JsonConverter[] converters = new JsonConverter[]
			{
				new Vector3Converter()
			};
			string text = JsonConvert.SerializeObject(vector, Formatting.None, converters);
			this.LogSerialized(text);
			Vector3 vector2 = JsonConvert.DeserializeObject<Vector3>(text);
			this.LogResult("4", vector2.y);
			if (vector2.y != vector.y)
			{
				this.DisplayFail("Vector3 Serialization", "Incorrect Deserialized Result");
			}
			this.DisplaySuccess("Vector3 Serialization");
		}
		catch (Exception ex)
		{
			this.DisplayFail("Vector3 Serialization", ex.get_Message());
		}
		this.LogEnd(1);
	}

	public void GenericListSerialization()
	{
		this.LogStart("List<T> Serialization");
		try
		{
			List<SimpleClassObject> list = new List<SimpleClassObject>();
			for (int i = 0; i < 4; i++)
			{
				list.Add(TestCaseUtils.GetSimpleClassObject());
			}
			string text = JsonConvert.SerializeObject(list);
			this.LogSerialized(text);
			List<SimpleClassObject> list2 = JsonConvert.DeserializeObject<List<SimpleClassObject>>(text);
			this.LogResult(list.get_Count().ToString(), list2.get_Count());
			this.LogResult(list.get_Item(2).TextValue, list2.get_Item(2).TextValue);
			if (list.get_Count() != list2.get_Count() || list.get_Item(3).TextValue != list2.get_Item(3).TextValue)
			{
				this.DisplayFail("List<T> Serialization", "Incorrect Deserialized Result");
				Debug.LogError("Deserialized List<T> has incorrect count or wrong item value");
			}
			else
			{
				this.DisplaySuccess("List<T> Serialization");
			}
		}
		catch (Exception ex)
		{
			this.DisplayFail("List<T> Serialization", ex.get_Message());
			throw;
		}
		this.LogEnd(2);
	}

	public void PolymorphicSerialization()
	{
		this.LogStart("Polymorphic Serialization");
		try
		{
			List<SampleBase> list = new List<SampleBase>();
			for (int i = 0; i < 4; i++)
			{
				list.Add(TestCaseUtils.GetSampleChid());
			}
			string text = JsonConvert.SerializeObject(list, Formatting.None, new JsonSerializerSettings
			{
				TypeNameHandling = TypeNameHandling.All
			});
			this.LogSerialized(text);
			List<SampleBase> list2 = JsonConvert.DeserializeObject<List<SampleBase>>(text, new JsonSerializerSettings
			{
				TypeNameHandling = TypeNameHandling.All
			});
			if (!(list2.get_Item(2) is SampleChild))
			{
				this.DisplayFail("Polymorphic Serialization", "Incorrect Deserialized Result");
			}
			else
			{
				this.LogResult(list.get_Item(2).TextValue, list2.get_Item(2).TextValue);
				if (list.get_Item(2).TextValue != list2.get_Item(2).TextValue)
				{
					this.DisplayFail("Polymorphic Serialization", "Incorrect Deserialized Result");
				}
				else
				{
					this.DisplaySuccess("Polymorphic Serialization");
				}
			}
		}
		catch (Exception ex)
		{
			this.DisplayFail("Polymorphic Serialization", ex.get_Message());
			throw;
		}
		this.LogEnd(3);
	}

	public void DictionarySerialization()
	{
		this.LogStart("Dictionary & Other DLL");
		try
		{
			SampleExternalClass sampleExternalClass = new SampleExternalClass();
			sampleExternalClass.set_SampleString(Guid.NewGuid().ToString());
			SampleExternalClass sampleExternalClass2 = sampleExternalClass;
			sampleExternalClass2.get_SampleDictionary().Add(1, "A");
			sampleExternalClass2.get_SampleDictionary().Add(2, "B");
			sampleExternalClass2.get_SampleDictionary().Add(3, "C");
			sampleExternalClass2.get_SampleDictionary().Add(4, "D");
			string text = JsonConvert.SerializeObject(sampleExternalClass2);
			this.LogSerialized(text);
			SampleExternalClass sampleExternalClass3 = JsonConvert.DeserializeObject<SampleExternalClass>(text);
			this.LogResult(sampleExternalClass2.get_SampleString(), sampleExternalClass3.get_SampleString());
			this.LogResult(sampleExternalClass2.get_SampleDictionary().get_Count().ToString(), sampleExternalClass3.get_SampleDictionary().get_Count());
			StringBuilder stringBuilder = new StringBuilder(4);
			StringBuilder stringBuilder2 = new StringBuilder(4);
			using (Dictionary<int, string>.Enumerator enumerator = sampleExternalClass2.get_SampleDictionary().GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					KeyValuePair<int, string> current = enumerator.get_Current();
					stringBuilder.Append(current.get_Key().ToString());
					stringBuilder2.Append(current.get_Value());
				}
			}
			this.LogResult("1234", stringBuilder.ToString());
			this.LogResult("ABCD", stringBuilder2.ToString());
			if (sampleExternalClass2.get_SampleString() != sampleExternalClass3.get_SampleString() || sampleExternalClass2.get_SampleDictionary().get_Count() != sampleExternalClass3.get_SampleDictionary().get_Count() || stringBuilder.ToString() != "1234" || stringBuilder2.ToString() != "ABCD")
			{
				this.DisplayFail("Dictionary & Other DLL", "Incorrect Deserialized Result");
			}
			else
			{
				this.DisplaySuccess("Dictionary & Other DLL");
			}
		}
		catch (Exception ex)
		{
			this.DisplayFail("Dictionary & Other DLL", ex.get_Message());
			throw;
		}
	}

	public void DictionaryObjectValueSerialization()
	{
		this.LogStart("Dictionary (Object Value)");
		try
		{
			Dictionary<int, SampleBase> dictionary = new Dictionary<int, SampleBase>();
			for (int i = 0; i < 4; i++)
			{
				dictionary.Add(i, TestCaseUtils.GetSampleBase());
			}
			string text = JsonConvert.SerializeObject(dictionary);
			this.LogSerialized(text);
			Dictionary<int, SampleBase> dictionary2 = JsonConvert.DeserializeObject<Dictionary<int, SampleBase>>(text);
			this.LogResult(dictionary.get_Item(1).TextValue, dictionary2.get_Item(1).TextValue);
			if (dictionary.get_Item(1).TextValue != dictionary2.get_Item(1).TextValue)
			{
				this.DisplayFail("Dictionary (Object Value)", "Incorrect Deserialized Result");
			}
			else
			{
				this.DisplaySuccess("Dictionary (Object Value)");
			}
		}
		catch (Exception ex)
		{
			this.DisplayFail("Dictionary (Object Value)", ex.get_Message());
			throw;
		}
	}

	public void DictionaryObjectKeySerialization()
	{
		this.LogStart("Dictionary (Object As Key)");
		try
		{
			Dictionary<SampleBase, int> dictionary = new Dictionary<SampleBase, int>();
			for (int i = 0; i < 4; i++)
			{
				dictionary.Add(TestCaseUtils.GetSampleBase(), i);
			}
			string text = JsonConvert.SerializeObject(dictionary);
			this.LogSerialized(text);
			this._text.set_text(text);
			Dictionary<SampleBase, int> dictionary2 = JsonConvert.DeserializeObject<Dictionary<SampleBase, int>>(text);
			List<SampleBase> list = new List<SampleBase>();
			List<SampleBase> list2 = new List<SampleBase>();
			using (Dictionary<SampleBase, int>.KeyCollection.Enumerator enumerator = dictionary.get_Keys().GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					SampleBase current = enumerator.get_Current();
					list.Add(current);
				}
			}
			using (Dictionary<SampleBase, int>.KeyCollection.Enumerator enumerator2 = dictionary2.get_Keys().GetEnumerator())
			{
				while (enumerator2.MoveNext())
				{
					SampleBase current2 = enumerator2.get_Current();
					list2.Add(current2);
				}
			}
			this.LogResult(list.get_Item(1).TextValue, list2.get_Item(1).TextValue);
			if (list.get_Item(1).TextValue != list2.get_Item(1).TextValue)
			{
				this.DisplayFail("Dictionary (Object As Key)", "Incorrect Deserialized Result");
			}
			else
			{
				this.DisplaySuccess("Dictionary (Object As Key)");
			}
		}
		catch (Exception ex)
		{
			this.DisplayFail("Dictionary (Object As Key)", ex.get_Message());
			throw;
		}
	}

	private void DisplaySuccess(string testName)
	{
		this._text.set_text(testName + "\r\nSuccessful");
	}

	private void DisplayFail(string testName, string reason)
	{
		try
		{
			this._text.set_text((testName + "\r\nFailed :( \r\n" + reason) ?? string.Empty);
		}
		catch
		{
			Debug.Log("%%%%%%%%%%%" + testName);
		}
	}

	private void LogStart(string testName)
	{
		this.Log(string.Empty);
		this.Log(string.Format("======= SERIALIZATION TEST: {0} ==========", testName));
	}

	private void LogEnd(int testNum)
	{
	}

	private void Log(object message)
	{
		Debug.Log(message);
	}

	private void LogSerialized(string message)
	{
		Debug.Log(string.Format("#### Serialized Object: {0}", message));
	}

	private void LogResult(string shouldEqual, object actual)
	{
		this.Log("--------------------");
		this.Log(string.Format("*** Original Test value: {0}", shouldEqual));
		this.Log(string.Format("*** Deserialized Test Value: {0}", actual));
		this.Log("--------------------");
	}
}

using Newtonsoft.Json;
using Newtonsoft.Json.Bson;
using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace DustinHorne.Json.Examples
{
	public class JNBsonSample
	{
		public void Sample()
		{
			JNSimpleObjectModel jNSimpleObjectModel = new JNSimpleObjectModel();
			jNSimpleObjectModel.IntValue = 5;
			jNSimpleObjectModel.FloatValue = 4.98f;
			jNSimpleObjectModel.StringValue = "Simple Object";
			JNSimpleObjectModel arg_54_0 = jNSimpleObjectModel;
			List<int> list = new List<int>();
			list.Add(4);
			list.Add(7);
			list.Add(25);
			list.Add(34);
			arg_54_0.IntList = list;
			jNSimpleObjectModel.ObjectType = JNObjectType.BaseClass;
			JNSimpleObjectModel value = jNSimpleObjectModel;
			byte[] array = new byte[0];
			using (MemoryStream memoryStream = new MemoryStream())
			{
				using (BsonWriter bsonWriter = new BsonWriter(memoryStream))
				{
					JsonSerializer jsonSerializer = new JsonSerializer();
					jsonSerializer.Serialize(bsonWriter, value);
				}
				array = memoryStream.ToArray();
				string text = Convert.ToBase64String(array);
				Debug.Log(text);
			}
			JNSimpleObjectModel jNSimpleObjectModel2;
			using (MemoryStream memoryStream2 = new MemoryStream(array))
			{
				using (BsonReader bsonReader = new BsonReader(memoryStream2))
				{
					JsonSerializer jsonSerializer2 = new JsonSerializer();
					jNSimpleObjectModel2 = jsonSerializer2.Deserialize<JNSimpleObjectModel>(bsonReader);
				}
			}
			if (jNSimpleObjectModel2 != null)
			{
				Debug.Log(jNSimpleObjectModel2.StringValue);
			}
		}
	}
}

using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace DustinHorne.Json.Examples
{
	public class JNSimpleObjectSample
	{
		public void Sample()
		{
			JNSimpleObjectModel jNSimpleObjectModel = new JNSimpleObjectModel();
			jNSimpleObjectModel.IntValue = 5;
			jNSimpleObjectModel.FloatValue = 4.98f;
			jNSimpleObjectModel.StringValue = "Simple Object";
			JNSimpleObjectModel arg_4F_0 = jNSimpleObjectModel;
			List<int> list = new List<int>();
			list.Add(4);
			list.Add(7);
			list.Add(25);
			list.Add(34);
			arg_4F_0.IntList = list;
			jNSimpleObjectModel.ObjectType = JNObjectType.BaseClass;
			JNSimpleObjectModel value = jNSimpleObjectModel;
			string value2 = JsonConvert.SerializeObject(value);
			JNSimpleObjectModel jNSimpleObjectModel2 = JsonConvert.DeserializeObject<JNSimpleObjectModel>(value2);
			Debug.Log(jNSimpleObjectModel2.IntList.get_Count());
		}
	}
}

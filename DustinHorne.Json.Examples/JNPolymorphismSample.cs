using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace DustinHorne.Json.Examples
{
	public class JNPolymorphismSample
	{
		private Random _rnd = new Random();

		public void Sample()
		{
			List<JNSimpleObjectModel> list = new List<JNSimpleObjectModel>();
			for (int i = 0; i < 3; i++)
			{
				list.Add(this.GetBaseModel());
			}
			for (int j = 0; j < 2; j++)
			{
				list.Add(this.GetSubClassModel());
			}
			for (int k = 0; k < 3; k++)
			{
				list.Add(this.GetBaseModel());
			}
			JsonSerializerSettings settings = new JsonSerializerSettings
			{
				TypeNameHandling = TypeNameHandling.All
			};
			string value = JsonConvert.SerializeObject(list, Formatting.None, settings);
			List<JNSimpleObjectModel> list2 = JsonConvert.DeserializeObject<List<JNSimpleObjectModel>>(value, settings);
			for (int l = 0; l < list2.get_Count(); l++)
			{
				JNSimpleObjectModel jNSimpleObjectModel = list2.get_Item(l);
				if (jNSimpleObjectModel.ObjectType == JNObjectType.SubClass)
				{
					Debug.Log((jNSimpleObjectModel as JNSubClassModel).SubClassStringValue);
				}
				else
				{
					Debug.Log(jNSimpleObjectModel.StringValue);
				}
			}
		}

		private JNSimpleObjectModel GetBaseModel()
		{
			JNSimpleObjectModel jNSimpleObjectModel = new JNSimpleObjectModel();
			jNSimpleObjectModel.IntValue = this._rnd.Next();
			jNSimpleObjectModel.FloatValue = (float)this._rnd.NextDouble();
			jNSimpleObjectModel.StringValue = Guid.NewGuid().ToString();
			JNSimpleObjectModel arg_77_0 = jNSimpleObjectModel;
			List<int> list = new List<int>();
			list.Add(this._rnd.Next());
			list.Add(this._rnd.Next());
			list.Add(this._rnd.Next());
			arg_77_0.IntList = list;
			jNSimpleObjectModel.ObjectType = JNObjectType.BaseClass;
			return jNSimpleObjectModel;
		}

		private JNSubClassModel GetSubClassModel()
		{
			JNSubClassModel jNSubClassModel = new JNSubClassModel();
			jNSubClassModel.IntValue = this._rnd.Next();
			jNSubClassModel.FloatValue = (float)this._rnd.NextDouble();
			jNSubClassModel.StringValue = Guid.NewGuid().ToString();
			JNSimpleObjectModel arg_77_0 = jNSubClassModel;
			List<int> list = new List<int>();
			list.Add(this._rnd.Next());
			list.Add(this._rnd.Next());
			list.Add(this._rnd.Next());
			arg_77_0.IntList = list;
			jNSubClassModel.ObjectType = JNObjectType.SubClass;
			jNSubClassModel.SubClassStringValue = "This is the subclass value.";
			return jNSubClassModel;
		}
	}
}

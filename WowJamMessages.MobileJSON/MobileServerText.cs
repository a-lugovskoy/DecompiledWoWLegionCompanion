using System;
using System.Runtime.Serialization;

namespace WowJamMessages.MobileJSON
{
	[DataContract]
	public class MobileServerText
	{
		[DataMember(Name = "text")]
		public string Text
		{
			get;
			set;
		}
	}
}

using JamLib;
using System;
using System.Runtime.Serialization;

namespace WowJamMessages.MobileClientJSON
{
	[FlexJamMessage(Id = 4860, Name = "MobileClientFollowerEquipmentResult", Version = 28333852u), DataContract]
	public class MobileClientFollowerEquipmentResult
	{
		[FlexJamMember(ArrayDimensions = 1, Name = "equipment", Type = FlexJamType.Struct), DataMember(Name = "equipment")]
		public MobileFollowerEquipment[] Equipment
		{
			get;
			set;
		}
	}
}

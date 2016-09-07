using JamLib;
using System;
using System.Runtime.Serialization;

namespace WowJamMessages
{
	[FlexJamStruct(Name = "JamGarrisonEncounter", Version = 28333852u), DataContract]
	public class JamGarrisonEncounter
	{
		[FlexJamMember(Name = "encounterID", Type = FlexJamType.Int32), DataMember(Name = "encounterID")]
		public int EncounterID
		{
			get;
			set;
		}

		[FlexJamMember(ArrayDimensions = 1, Name = "mechanicID", Type = FlexJamType.Int32), DataMember(Name = "mechanicID")]
		public int[] MechanicID
		{
			get;
			set;
		}
	}
}

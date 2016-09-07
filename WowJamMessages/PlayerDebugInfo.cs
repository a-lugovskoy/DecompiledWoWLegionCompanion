using JamLib;
using System;
using System.Runtime.Serialization;

namespace WowJamMessages
{
	[FlexJamStruct(Name = "PlayerDebugInfo", Version = 28333852u), DataContract]
	public class PlayerDebugInfo
	{
		[FlexJamMember(ArrayDimensions = 1, Name = "combatRatings", Type = FlexJamType.Int32), DataMember(Name = "combatRatings")]
		public int[] CombatRatings
		{
			get;
			set;
		}

		public PlayerDebugInfo()
		{
			this.CombatRatings = new int[32];
		}
	}
}

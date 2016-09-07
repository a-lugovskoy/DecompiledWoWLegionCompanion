using JamLib;
using System;
using System.Runtime.Serialization;

namespace WowJamMessages
{
	[FlexJamStruct(Name = "JamSpawnRegionDebug", Version = 28333852u), DataContract]
	public class JamSpawnRegionDebug
	{
		[FlexJamMember(Name = "pending", Type = FlexJamType.Int32), DataMember(Name = "pending")]
		public int Pending
		{
			get;
			set;
		}

		[FlexJamMember(Name = "numThresholdsHit", Type = FlexJamType.Int32), DataMember(Name = "numThresholdsHit")]
		public int NumThresholdsHit
		{
			get;
			set;
		}

		[FlexJamMember(Name = "maxThreshold", Type = FlexJamType.Float), DataMember(Name = "maxThreshold")]
		public float MaxThreshold
		{
			get;
			set;
		}

		[FlexJamMember(Name = "numGroups", Type = FlexJamType.Int32), DataMember(Name = "numGroups")]
		public int NumGroups
		{
			get;
			set;
		}

		[FlexJamMember(Name = "checkingThreshold", Type = FlexJamType.Bool), DataMember(Name = "checkingThreshold")]
		public bool CheckingThreshold
		{
			get;
			set;
		}

		[FlexJamMember(Name = "isFarmed", Type = FlexJamType.Bool), DataMember(Name = "isFarmed")]
		public bool IsFarmed
		{
			get;
			set;
		}

		[FlexJamMember(Name = "actual", Type = FlexJamType.Int32), DataMember(Name = "actual")]
		public int Actual
		{
			get;
			set;
		}

		[FlexJamMember(Name = "minThreshold", Type = FlexJamType.Float), DataMember(Name = "minThreshold")]
		public float MinThreshold
		{
			get;
			set;
		}

		[FlexJamMember(Name = "regionID", Type = FlexJamType.Int32), DataMember(Name = "regionID")]
		public int RegionID
		{
			get;
			set;
		}

		[FlexJamMember(ArrayDimensions = 1, Name = "players", Type = FlexJamType.Struct), DataMember(Name = "players")]
		public JamSpawnRegionPlayerActivity[] Players
		{
			get;
			set;
		}
	}
}

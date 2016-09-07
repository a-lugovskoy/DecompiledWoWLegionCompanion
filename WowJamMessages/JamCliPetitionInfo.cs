using JamLib;
using System;
using System.Runtime.Serialization;

namespace WowJamMessages
{
	[FlexJamStruct(Name = "JamCliPetitionInfo", Version = 28333852u), DataContract]
	public class JamCliPetitionInfo
	{
		[FlexJamMember(Name = "m_allowedMinLevel", Type = FlexJamType.Int32), DataMember(Name = "m_allowedMinLevel")]
		public int M_allowedMinLevel
		{
			get;
			set;
		}

		[FlexJamMember(Name = "m_allowedClasses", Type = FlexJamType.Int32), DataMember(Name = "m_allowedClasses")]
		public int M_allowedClasses
		{
			get;
			set;
		}

		[FlexJamMember(Name = "m_allowedGender", Type = FlexJamType.Int16), DataMember(Name = "m_allowedGender")]
		public short M_allowedGender
		{
			get;
			set;
		}

		[FlexJamMember(Name = "m_deadLine", Type = FlexJamType.Int32), DataMember(Name = "m_deadLine")]
		public int M_deadLine
		{
			get;
			set;
		}

		[FlexJamMember(Name = "m_maxSignatures", Type = FlexJamType.Int32), DataMember(Name = "m_maxSignatures")]
		public int M_maxSignatures
		{
			get;
			set;
		}

		[FlexJamMember(Name = "m_title", Type = FlexJamType.String), DataMember(Name = "m_title")]
		public string M_title
		{
			get;
			set;
		}

		[FlexJamMember(Name = "m_muid", Type = FlexJamType.UInt32), DataMember(Name = "m_muid")]
		public uint M_muid
		{
			get;
			set;
		}

		[FlexJamMember(Name = "m_petitioner", Type = FlexJamType.WowGuid), DataMember(Name = "m_petitioner")]
		public string M_petitioner
		{
			get;
			set;
		}

		[FlexJamMember(Name = "m_bodyText", Type = FlexJamType.String), DataMember(Name = "m_bodyText")]
		public string M_bodyText
		{
			get;
			set;
		}

		[FlexJamMember(Name = "m_allowedMaxLevel", Type = FlexJamType.Int32), DataMember(Name = "m_allowedMaxLevel")]
		public int M_allowedMaxLevel
		{
			get;
			set;
		}

		[FlexJamMember(Name = "m_minSignatures", Type = FlexJamType.Int32), DataMember(Name = "m_minSignatures")]
		public int M_minSignatures
		{
			get;
			set;
		}

		[FlexJamMember(Name = "m_staticType", Type = FlexJamType.Int32), DataMember(Name = "m_staticType")]
		public int M_staticType
		{
			get;
			set;
		}

		[FlexJamMember(Name = "m_numChoices", Type = FlexJamType.Int32), DataMember(Name = "m_numChoices")]
		public int M_numChoices
		{
			get;
			set;
		}

		[FlexJamMember(Name = "m_issueDate", Type = FlexJamType.Int32), DataMember(Name = "m_issueDate")]
		public int M_issueDate
		{
			get;
			set;
		}

		[FlexJamMember(Name = "m_allowedRaces", Type = FlexJamType.Int32), DataMember(Name = "m_allowedRaces")]
		public int M_allowedRaces
		{
			get;
			set;
		}

		[FlexJamMember(Name = "m_petitionID", Type = FlexJamType.Int32), DataMember(Name = "m_petitionID")]
		public int M_petitionID
		{
			get;
			set;
		}

		[FlexJamMember(Name = "m_allowedGuildID", Type = FlexJamType.Int32), DataMember(Name = "m_allowedGuildID")]
		public int M_allowedGuildID
		{
			get;
			set;
		}

		[FlexJamMember(ArrayDimensions = 1, Name = "m_choicetext", Type = FlexJamType.String), DataMember(Name = "m_choicetext")]
		public string[] M_choicetext
		{
			get;
			set;
		}

		public JamCliPetitionInfo()
		{
			this.M_choicetext = new string[10];
		}
	}
}

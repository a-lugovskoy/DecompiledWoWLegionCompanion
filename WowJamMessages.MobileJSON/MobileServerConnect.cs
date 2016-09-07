using JamLib;
using System;
using System.Runtime.Serialization;

namespace WowJamMessages.MobileJSON
{
	[FlexJamMessage(Id = 4743, Name = "MobileServerConnect", Version = 28333852u), DataContract]
	public class MobileServerConnect
	{
		[FlexJamMember(ArrayDimensions = 1, Name = "proof", Type = FlexJamType.UInt8), DataMember(Name = "proof")]
		public byte[] Proof
		{
			get;
			set;
		}

		[FlexJamMember(Name = "realmAddress", Type = FlexJamType.UInt32), DataMember(Name = "realmAddress")]
		public uint RealmAddress
		{
			get;
			set;
		}

		[FlexJamMember(ArrayDimensions = 1, Name = "joinTicket", Type = FlexJamType.UInt8), DataMember(Name = "joinTicket")]
		public byte[] JoinTicket
		{
			get;
			set;
		}

		[FlexJamMember(Name = "characterID", Type = FlexJamType.WowGuid), DataMember(Name = "characterID")]
		public string CharacterID
		{
			get;
			set;
		}

		[FlexJamMember(Name = "build", Type = FlexJamType.UInt16), DataMember(Name = "build")]
		public ushort Build
		{
			get;
			set;
		}

		[FlexJamMember(Name = "buildType", Type = FlexJamType.UInt32), DataMember(Name = "buildType")]
		public uint BuildType
		{
			get;
			set;
		}

		[FlexJamMember(ArrayDimensions = 1, Name = "clientChallenge", Type = FlexJamType.UInt8), DataMember(Name = "clientChallenge")]
		public byte[] ClientChallenge
		{
			get;
			set;
		}

		public MobileServerConnect()
		{
			this.ClientChallenge = new byte[16];
			this.Proof = new byte[24];
		}
	}
}

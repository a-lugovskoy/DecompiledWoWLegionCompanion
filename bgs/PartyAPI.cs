using bgs.types;
using bnet.protocol;
using bnet.protocol.attribute;
using bnet.protocol.channel;
using bnet.protocol.channel_invitation;
using bnet.protocol.invitation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;

namespace bgs
{
	public class PartyAPI : BattleNetAPI
	{
		public struct PartyCreateOptions
		{
			public string m_name;

			public ChannelState.Types.PrivacyLevel m_privacyLevel;
		}

		public class PartyData
		{
			public bnet.protocol.EntityId m_partyId;

			public bnet.protocol.EntityId m_friendGameAccount;

			public int m_scenarioId;

			public long m_makerDeck;

			public long m_inviteeDeck;

			public bool m_maker;

			public ulong m_subscriberObjectId;

			private BattleNetCSharp m_battleNet;

			public PartyData(BattleNetCSharp battlenet)
			{
				this.m_battleNet = battlenet;
			}

			public PartyData(BattleNetCSharp battlenet, bnet.protocol.EntityId friendGameAccount)
			{
				this.m_friendGameAccount = friendGameAccount;
				this.m_maker = true;
				this.m_battleNet = battlenet;
			}

			public PartyData(BattleNetCSharp battlenet, bnet.protocol.EntityId partyId, bnet.protocol.EntityId friendGameAccount)
			{
				this.m_partyId = partyId;
				this.m_friendGameAccount = friendGameAccount;
				this.m_battleNet = battlenet;
			}

			public void SetSubscriberObjectId(ulong objectId)
			{
				this.m_subscriberObjectId = objectId;
			}

			public void FriendlyChallenge_HandleChannelAttributeUpdate(IList<Attribute> attributeList)
			{
				using (IEnumerator<Attribute> enumerator = attributeList.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						Attribute current = enumerator.get_Current();
						if (current.Value.HasIntValue)
						{
							if (current.Name == "d2")
							{
								this.m_inviteeDeck = current.Value.IntValue;
								this.StartFriendlyChallengeGameIfReady();
							}
						}
						else if (current.Value.HasStringValue)
						{
							this.m_battleNet.Party.PushPartyEvent(this.m_partyId, current.Name, current.Value.StringValue, this.m_friendGameAccount);
						}
						else
						{
							this.m_battleNet.Party.ApiLog.LogError("Party : unknown value type, key: " + current.Name);
						}
					}
				}
			}

			public void StartFriendlyChallengeGameIfReady()
			{
				ChannelAPI.ChannelReferenceObject channelReferenceObject = this.m_battleNet.Channel.GetChannelReferenceObject(this.m_partyId);
				if (channelReferenceObject == null)
				{
					return;
				}
				if (!this.m_maker)
				{
					return;
				}
				if (this.m_makerDeck == 0L)
				{
					return;
				}
				if (this.m_inviteeDeck == 0L)
				{
					return;
				}
				List<Attribute> list = new List<Attribute>();
				list.Add(ProtocolHelper.CreateAttribute("s1", "goto"));
				list.Add(ProtocolHelper.CreateAttribute("s2", "goto"));
				this.m_battleNet.Channel.UpdateChannelAttributes((ChannelAPI.ChannelData)channelReferenceObject.m_channelData, list, null);
				this.m_battleNet.Games.CreateFriendlyChallengeGame(this.m_makerDeck, this.m_inviteeDeck, this.m_friendGameAccount, this.m_scenarioId);
				this.m_makerDeck = 0L;
				this.m_inviteeDeck = 0L;
			}

			private void CreateParty_SendInvitationCallback(RPCContext context)
			{
				BattleNetErrors status = (BattleNetErrors)context.Header.Status;
				if (status != BattleNetErrors.ERROR_OK)
				{
					this.m_battleNet.Party.ApiLog.LogError("SendInvitationCallback: " + status.ToString());
					this.m_battleNet.Party.PushPartyError(this.m_partyId, status, BnetFeatureEvent.Party_SendInvite_Callback);
					return;
				}
				SendInvitationResponse sendInvitationResponse = SendInvitationResponse.ParseFrom(context.Payload);
				if (sendInvitationResponse.Invitation.HasChannelInvitation)
				{
					ChannelInvitation channelInvitation = sendInvitationResponse.Invitation.ChannelInvitation;
					ChannelAPI.InvitationServiceType serviceType = (ChannelAPI.InvitationServiceType)channelInvitation.ServiceType;
					if (serviceType == ChannelAPI.InvitationServiceType.INVITATION_SERVICE_TYPE_PARTY)
					{
						this.m_battleNet.Party.PushPartyEvent(this.m_partyId, "cb", "inv", this.m_friendGameAccount);
					}
				}
				this.m_battleNet.Party.ApiLog.LogDebug("SendInvitationCallback, status=" + status.ToString());
			}

			public void CreateChannelCallback(RPCContext context, string szPartyType)
			{
				BattleNetErrors status = (BattleNetErrors)context.Header.Status;
				this.m_battleNet.Party.PushPartyErrorEvent(PartyListenerEventType.OPERATION_CALLBACK, "CreateParty", status, BnetFeatureEvent.Party_Create_Callback, null, szPartyType);
				if (status != BattleNetErrors.ERROR_OK)
				{
					this.m_battleNet.Party.ApiLog.LogError("CreateChannelCallback: code=" + new Error(status));
					this.m_battleNet.Party.PushPartyError(new bnet.protocol.EntityId(), status, BnetFeatureEvent.Party_Create_Callback);
					return;
				}
				CreateChannelResponse createChannelResponse = CreateChannelResponse.ParseFrom(context.Payload);
				this.m_partyId = createChannelResponse.ChannelId;
				ChannelAPI.ChannelData channelData = new ChannelAPI.ChannelData(this.m_battleNet.Channel, createChannelResponse.ChannelId, createChannelResponse.ObjectId, ChannelAPI.ChannelType.PARTY_CHANNEL);
				channelData.SetSubscriberObjectId(this.m_subscriberObjectId);
				this.m_battleNet.Party.AddActiveChannel(this.m_subscriberObjectId, new ChannelAPI.ChannelReferenceObject(channelData), this);
				if (this.m_friendGameAccount != null)
				{
					this.m_battleNet.Channel.SendInvitation(createChannelResponse.ChannelId, this.m_friendGameAccount, ChannelAPI.InvitationServiceType.INVITATION_SERVICE_TYPE_PARTY, new RPCContextDelegate(this.CreateParty_SendInvitationCallback));
				}
				this.m_battleNet.Party.ApiLog.LogDebug("CreateChannelCallback code=" + new Error(status));
			}

			public void JoinChannelCallback(RPCContext context, bnet.protocol.EntityId partyId, string szPartyType)
			{
				BattleNetErrors status = (BattleNetErrors)context.Header.Status;
				this.m_battleNet.Party.PushPartyErrorEvent(PartyListenerEventType.OPERATION_CALLBACK, "JoinParty", status, BnetFeatureEvent.Party_Join_Callback, partyId, szPartyType);
				if (status != BattleNetErrors.ERROR_OK)
				{
					this.m_battleNet.Party.ApiLog.LogError("JoinChannelCallback: code=" + new Error(status));
					this.m_battleNet.Party.PushPartyError(new bnet.protocol.EntityId(), status, BnetFeatureEvent.Party_Join_Callback);
					return;
				}
				JoinChannelResponse joinChannelResponse = JoinChannelResponse.ParseFrom(context.Payload);
				ChannelAPI.ChannelData channelData = new ChannelAPI.ChannelData(this.m_battleNet.Channel, this.m_partyId, joinChannelResponse.ObjectId, ChannelAPI.ChannelType.PARTY_CHANNEL);
				channelData.SetSubscriberObjectId(this.m_subscriberObjectId);
				this.m_battleNet.Party.AddActiveChannel(this.m_subscriberObjectId, new ChannelAPI.ChannelReferenceObject(channelData), this);
				this.m_battleNet.Party.ApiLog.LogDebug("JoinChannelCallback cide=" + new Error(status));
			}

			public void DeclineInvite_DissolvePartyInviteCallback(RPCContext context)
			{
				BattleNetErrors status = (BattleNetErrors)context.Header.Status;
				if (status != BattleNetErrors.ERROR_OK)
				{
					this.m_battleNet.Party.ApiLog.LogError("DisolvePartyInviteCallback: " + status.ToString());
					this.m_battleNet.Party.PushPartyError(this.m_partyId, status, BnetFeatureEvent.Party_Dissolve_Callback);
					return;
				}
				if (this.m_friendGameAccount != null)
				{
					this.m_battleNet.Party.PushPartyEvent(this.m_partyId, "cb", "drop", this.m_friendGameAccount);
				}
				this.m_battleNet.Party.ApiLog.LogDebug("DisolvePartyInviteCallback, status=" + status.ToString());
			}

			public void PartyMemberLeft_DissolvePartyInviteCallback(RPCContext context)
			{
				BattleNetErrors status = (BattleNetErrors)context.Header.Status;
				if (status != BattleNetErrors.ERROR_OK)
				{
					this.m_battleNet.Party.ApiLog.LogError("PartyMemberLeft_DissolvePartyInviteCallback: " + status.ToString());
					this.m_battleNet.Party.PushPartyError(this.m_partyId, status, BnetFeatureEvent.Party_Dissolve_Callback);
					return;
				}
				this.m_battleNet.Party.ApiLog.LogDebug("PartyMemberLeft_DissolvePartyInviteCallback, status=" + status.ToString());
			}
		}

		public static string PARTY_TYPE_DEFAULT = "default";

		private Map<bnet.protocol.EntityId, PartyAPI.PartyData> m_activeParties = new Map<bnet.protocol.EntityId, PartyAPI.PartyData>();

		private List<PartyEvent> m_partyEvents = new List<PartyEvent>();

		private List<PartyListenerEvent> m_partyListenerEvents = new List<PartyListenerEvent>();

		public PartyAPI(BattleNetCSharp battlenet) : base(battlenet, "Party")
		{
		}

		public override void InitRPCListeners(RPCConnection rpcConnection)
		{
			base.InitRPCListeners(rpcConnection);
		}

		public override void Initialize()
		{
			base.Initialize();
		}

		public override void OnDisconnected()
		{
			base.OnDisconnected();
			this.m_activeParties.Clear();
			this.m_partyEvents.Clear();
			this.m_partyListenerEvents.Clear();
		}

		private PartyAPI.PartyData GetPartyData(bnet.protocol.EntityId partyId)
		{
			PartyAPI.PartyData result;
			if (this.m_activeParties.TryGetValue(partyId, out result))
			{
				return result;
			}
			return null;
		}

		public string GetPartyType(bnet.protocol.EntityId partyId)
		{
			string text = string.Empty;
			ChannelAPI.ChannelReferenceObject channelReferenceObject = this.m_battleNet.Channel.GetChannelReferenceObject(partyId);
			if (channelReferenceObject == null || !(channelReferenceObject.m_channelData is ChannelAPI.ChannelData))
			{
				return text;
			}
			ChannelState channelState = ((ChannelAPI.ChannelData)channelReferenceObject.m_channelData).m_channelState;
			if (channelState == null)
			{
				return text;
			}
			text = channelState.ChannelType;
			if (text == PartyAPI.PARTY_TYPE_DEFAULT)
			{
				string text2;
				this.GetPartyAttributeString(partyId, "WTCG.Party.Type", out text2);
				if (text2 != null)
				{
					text = text2;
				}
			}
			return text;
		}

		public string GetReceivedInvitationPartyType(ulong invitationId)
		{
			string result = string.Empty;
			ChannelAPI.ReceivedInvite receivedInvite = this.m_battleNet.Channel.GetReceivedInvite(ChannelAPI.InvitationServiceType.INVITATION_SERVICE_TYPE_PARTY, invitationId);
			if (receivedInvite != null && receivedInvite.State != null)
			{
				ChannelState state = receivedInvite.State;
				for (int i = 0; i < state.AttributeList.get_Count(); i++)
				{
					Attribute attribute = state.AttributeList.get_Item(i);
					if (attribute.Name == "WTCG.Party.Type" && attribute.Value.HasStringValue)
					{
						result = attribute.Value.StringValue;
						break;
					}
				}
			}
			return result;
		}

		public Attribute GetReceivedInvitationAttribute(bnet.protocol.EntityId partyId, string attributeKey)
		{
			ChannelAPI.ReceivedInvite[] allReceivedInvites = this.m_battleNet.Channel.GetAllReceivedInvites();
			ChannelAPI.ReceivedInvite receivedInvite = Enumerable.FirstOrDefault<ChannelAPI.ReceivedInvite>(allReceivedInvites, (ChannelAPI.ReceivedInvite i) => i.ChannelId != null && i.ChannelId.High == partyId.High && i.ChannelId.Low == partyId.Low);
			if (receivedInvite != null && receivedInvite.State != null)
			{
				ChannelState state = receivedInvite.State;
				for (int j = 0; j < state.AttributeList.get_Count(); j++)
				{
					Attribute attribute = state.AttributeList.get_Item(j);
					if (attribute.Name == attributeKey)
					{
						return attribute;
					}
				}
			}
			return null;
		}

		private void GenericPartyRequestCallback(RPCContext context, string message, BnetFeatureEvent featureEvent, bnet.protocol.EntityId partyId, string szPartyType)
		{
			BattleNetErrors error = (BattleNetErrors)((context != null && context.Header != null) ? context.Header.Status : 3008u);
			this.GenericPartyRequestCallback_Internal(error, message, featureEvent, partyId, szPartyType);
		}

		private void GenericPartyRequestCallback_Internal(BattleNetErrors error, string message, BnetFeatureEvent featureEvent, bnet.protocol.EntityId partyId, string szPartyType)
		{
			this.m_battleNet.Party.PushPartyErrorEvent(PartyListenerEventType.OPERATION_CALLBACK, message, error, featureEvent, partyId, szPartyType);
			if (error != BattleNetErrors.ERROR_OK)
			{
				if (partyId != null)
				{
					message = string.Format("PartyRequestError: {0} {1} {2} partyId={3} type={4}", new object[]
					{
						(int)error,
						error.ToString(),
						message,
						partyId.Low,
						szPartyType
					});
				}
				else
				{
					message = string.Format("PartyRequestError: {0} {1} {2} type={3}", new object[]
					{
						(int)error,
						error.ToString(),
						message,
						szPartyType
					});
				}
				this.m_battleNet.Party.ApiLog.LogError(message);
				return;
			}
			if (partyId != null)
			{
				message = string.Format("PartyRequest {0} status={1} partyId={2} type={3}", new object[]
				{
					message,
					error.ToString(),
					partyId.Low,
					szPartyType
				});
			}
			else
			{
				message = string.Format("PartyRequest {0} status={1} type={2}", message, error.ToString(), szPartyType);
			}
			this.m_battleNet.Party.ApiLog.LogDebug(message);
		}

		public void AddActiveChannel(ulong objectId, ChannelAPI.ChannelReferenceObject channelRefObject, PartyAPI.PartyData partyData)
		{
			bnet.protocol.EntityId channelId = channelRefObject.m_channelData.m_channelId;
			this.m_battleNet.Channel.AddActiveChannel(objectId, channelRefObject);
			this.m_activeParties.Add(channelId, partyData);
		}

		public void DeclineFriendlyChallenge(bnet.protocol.EntityId partyId, string action)
		{
			PartyAPI.PartyData partyData = this.GetPartyData(partyId);
			ChannelAPI.ChannelReferenceObject channelReferenceObject = this.m_battleNet.Channel.GetChannelReferenceObject(partyId);
			if (partyData == null || channelReferenceObject == null)
			{
				this.PushPartyEvent(partyId, "parm", "party", new bnet.protocol.EntityId());
				return;
			}
			this.PushPartyEvent(partyId, "dll", action, partyData.m_friendGameAccount);
			this.m_rpcConnection.QueueRequest(this.m_battleNet.Channel.ChannelService.Id, 6u, new DissolveRequest(), new RPCContextDelegate(partyData.DeclineInvite_DissolvePartyInviteCallback), (uint)channelReferenceObject.m_channelData.m_objectId);
		}

		public void AcceptFriendlyChallenge(bnet.protocol.EntityId partyId)
		{
			bnet.protocol.EntityId friendGameAccount = new bnet.protocol.EntityId();
			PartyAPI.PartyData partyData = this.GetPartyData(partyId);
			if (partyData != null)
			{
				friendGameAccount = partyData.m_friendGameAccount;
			}
			this.PushPartyEvent(partyId, "dll", "ok", friendGameAccount);
			this.FriendlyChallenge_PushStateChange(partyId, "deck", false);
		}

		public void SetPartyDeck(bnet.protocol.EntityId partyId, long deckId)
		{
			PartyAPI.PartyData partyData = this.GetPartyData(partyId);
			ChannelAPI.ChannelReferenceObject channelReferenceObject = this.m_battleNet.Channel.GetChannelReferenceObject(partyId);
			if (partyData == null || channelReferenceObject == null)
			{
				this.PushPartyEvent(partyId, "parm", "party", new bnet.protocol.EntityId());
				return;
			}
			this.PushPartyEvent(partyId, "dll", "deck", partyData.m_friendGameAccount);
			if (deckId != 0L)
			{
				this.FriendlyChallenge_PushStateChange(partyId, "game", false);
			}
			else
			{
				this.FriendlyChallenge_PushStateChange(partyId, "deck", false);
			}
			List<Attribute> list = new List<Attribute>();
			list.Add(ProtocolHelper.CreateAttribute((!partyData.m_maker) ? "d2" : "d1", deckId));
			this.m_battleNet.Channel.UpdateChannelAttributes((ChannelAPI.ChannelData)channelReferenceObject.m_channelData, list, null);
			if (!partyData.m_maker)
			{
				return;
			}
			partyData.m_makerDeck = deckId;
			if (deckId != 0L)
			{
				partyData.StartFriendlyChallengeGameIfReady();
			}
		}

		public int GetPartyUpdateCount()
		{
			return this.m_partyEvents.get_Count();
		}

		public void GetPartyUpdates([Out] PartyEvent[] updates)
		{
			this.m_partyEvents.CopyTo(updates);
		}

		public void ClearPartyUpdates()
		{
			this.m_partyEvents.Clear();
		}

		public void GetPartyListenerEvents(out PartyListenerEvent[] updates)
		{
			updates = new PartyListenerEvent[this.m_partyListenerEvents.get_Count()];
			this.m_partyListenerEvents.CopyTo(updates);
		}

		public void ClearPartyListenerEvents()
		{
			this.m_partyListenerEvents.Clear();
		}

		private void PushPartyEvent(bnet.protocol.EntityId partyId, string type, string data, bnet.protocol.EntityId friendGameAccount)
		{
			PartyEvent partyEvent = default(PartyEvent);
			partyEvent.partyId.hi = partyId.High;
			partyEvent.partyId.lo = partyId.Low;
			partyEvent.eventName = type;
			partyEvent.eventData = data;
			partyEvent.otherMemberId.hi = friendGameAccount.High;
			partyEvent.otherMemberId.lo = friendGameAccount.Low;
			this.m_partyEvents.Add(partyEvent);
		}

		private void FriendlyChallenge_PushStateChange(bnet.protocol.EntityId partyId, string state, bool onlyIfMaker = false)
		{
			PartyAPI.PartyData partyData = this.GetPartyData(partyId);
			ChannelAPI.ChannelReferenceObject channelReferenceObject = this.m_battleNet.Channel.GetChannelReferenceObject(partyId);
			if (partyData == null || channelReferenceObject == null)
			{
				return;
			}
			if (onlyIfMaker && !partyData.m_maker)
			{
				return;
			}
			List<Attribute> list = new List<Attribute>();
			list.Add(ProtocolHelper.CreateAttribute((!partyData.m_maker) ? "s2" : "s1", state));
			this.m_battleNet.Channel.UpdateChannelAttributes((ChannelAPI.ChannelData)channelReferenceObject.m_channelData, list, null);
		}

		private void PushPartyError(bnet.protocol.EntityId partyId, BattleNetErrors errorCode, BnetFeatureEvent featureEvent)
		{
			PartyEvent partyEvent = default(PartyEvent);
			if (partyId != null)
			{
				partyEvent.partyId.hi = partyId.High;
				partyEvent.partyId.lo = partyId.Low;
			}
			partyEvent.errorInfo = new BnetErrorInfo(BnetFeature.Party, featureEvent, errorCode);
			this.m_partyEvents.Add(partyEvent);
		}

		private void PushPartyErrorEvent(PartyListenerEventType evtType, string szDebugContext, Error error, BnetFeatureEvent featureEvent, bnet.protocol.EntityId partyId = null, string szStringData = null)
		{
			PartyListenerEvent partyListenerEvent = default(PartyListenerEvent);
			partyListenerEvent.Type = evtType;
			partyListenerEvent.PartyId = ((partyId != null) ? PartyId.FromProtocol(partyId) : new PartyId());
			partyListenerEvent.UintData = error.Code;
			partyListenerEvent.UlongData = (17179869184uL | (ulong)featureEvent);
			partyListenerEvent.StringData = ((szDebugContext != null) ? szDebugContext : string.Empty);
			partyListenerEvent.StringData2 = ((szStringData != null) ? szStringData : string.Empty);
			this.m_partyListenerEvents.Add(partyListenerEvent);
		}

		public void SendFriendlyChallengeInvite(bnet.protocol.EntityId friendEntityId, int scenarioId)
		{
			PartyAPI.PartyData partyData = new PartyAPI.PartyData(this.m_battleNet, friendEntityId);
			partyData.m_scenarioId = scenarioId;
			CreateChannelRequest createChannelRequest = new CreateChannelRequest();
			createChannelRequest.SetObjectId(ChannelAPI.GetNextObjectId());
			partyData.SetSubscriberObjectId(createChannelRequest.ObjectId);
			ChannelState channelState = new ChannelState();
			channelState.SetName("FriendlyGame");
			channelState.SetPrivacyLevel(ChannelState.Types.PrivacyLevel.PRIVACY_LEVEL_OPEN);
			channelState.AddAttribute(ProtocolHelper.CreateAttribute("WTCG.Party.Type", "FriendlyGame"));
			channelState.AddAttribute(ProtocolHelper.CreateAttribute("WTCG.Game.ScenarioId", (long)scenarioId));
			createChannelRequest.SetChannelState(channelState);
			this.m_rpcConnection.QueueRequest(this.m_battleNet.Channel.ChannelOwnerService.Id, 2u, createChannelRequest, delegate(RPCContext ctx)
			{
				partyData.CreateChannelCallback(ctx, "FriendlyGame");
			}, 2u);
		}

		public void CreateParty(string szPartyType, int privacyLevel, byte[] creatorBlob)
		{
			PartyAPI.PartyData partyData = new PartyAPI.PartyData(this.m_battleNet);
			CreateChannelRequest createChannelRequest = new CreateChannelRequest();
			createChannelRequest.SetObjectId(ChannelAPI.GetNextObjectId());
			partyData.SetSubscriberObjectId(createChannelRequest.ObjectId);
			ChannelState channelState = new ChannelState();
			channelState.SetChannelType(szPartyType);
			channelState.SetPrivacyLevel((ChannelState.Types.PrivacyLevel)privacyLevel);
			channelState.AddAttribute(ProtocolHelper.CreateAttribute("WTCG.Party.Type", szPartyType));
			channelState.AddAttribute(ProtocolHelper.CreateAttribute("WTCG.Party.Creator", creatorBlob));
			createChannelRequest.SetChannelState(channelState);
			this.m_rpcConnection.QueueRequest(this.m_battleNet.Channel.ChannelOwnerService.Id, 2u, createChannelRequest, delegate(RPCContext ctx)
			{
				partyData.CreateChannelCallback(ctx, szPartyType);
			}, 2u);
		}

		public void JoinParty(bnet.protocol.EntityId partyId, string szPartyType)
		{
			PartyAPI.PartyData partyData = this.GetPartyData(partyId);
			if (partyData != null)
			{
				this.PushPartyError(partyId, BattleNetErrors.ERROR_PARTY_ALREADY_IN_PARTY, BnetFeatureEvent.Party_Join_Callback);
				return;
			}
			partyData = new PartyAPI.PartyData(this.m_battleNet, partyId, null);
			JoinChannelRequest joinChannelRequest = new JoinChannelRequest();
			joinChannelRequest.SetChannelId(partyId);
			joinChannelRequest.SetObjectId(ChannelAPI.GetNextObjectId());
			partyData.SetSubscriberObjectId(joinChannelRequest.ObjectId);
			partyData.m_partyId = partyId;
			this.m_rpcConnection.QueueRequest(this.m_battleNet.Channel.ChannelOwnerService.Id, 3u, joinChannelRequest, delegate(RPCContext ctx)
			{
				partyData.JoinChannelCallback(ctx, partyId, szPartyType);
			}, 2u);
		}

		public void LeaveParty(bnet.protocol.EntityId partyId)
		{
			string szPartyType = this.GetPartyType(partyId);
			if (this.GetPartyData(partyId) == null)
			{
				this.GenericPartyRequestCallback_Internal(BattleNetErrors.ERROR_INVALID_ARGS, "LeaveParty no PartyData", BnetFeatureEvent.Party_Leave_Callback, partyId, szPartyType);
				return;
			}
			ChannelAPI.ChannelReferenceObject channelReferenceObject = this.m_battleNet.Channel.GetChannelReferenceObject(partyId);
			if (channelReferenceObject == null)
			{
				this.GenericPartyRequestCallback_Internal(BattleNetErrors.ERROR_INVALID_ARGS, "LeaveParty no channelRefObject", BnetFeatureEvent.Party_Leave_Callback, partyId, szPartyType);
				return;
			}
			RemoveMemberRequest removeMemberRequest = new RemoveMemberRequest();
			removeMemberRequest.MemberId = this.m_battleNet.GetMyGameAccountId().ToProtocol();
			this.m_rpcConnection.QueueRequest(this.m_battleNet.Channel.ChannelService.Id, 2u, removeMemberRequest, delegate(RPCContext ctx)
			{
				this.GenericPartyRequestCallback(ctx, "LeaveParty", BnetFeatureEvent.Party_Leave_Callback, partyId, szPartyType);
			}, (uint)channelReferenceObject.m_channelData.m_objectId);
		}

		public void DissolveParty(bnet.protocol.EntityId partyId)
		{
			string szPartyType = this.GetPartyType(partyId);
			if (this.GetPartyData(partyId) == null)
			{
				this.GenericPartyRequestCallback_Internal(BattleNetErrors.ERROR_INVALID_ARGS, "DissolveParty no PartyData", BnetFeatureEvent.Party_Dissolve_Callback, partyId, szPartyType);
				return;
			}
			ChannelAPI.ChannelReferenceObject channelReferenceObject = this.m_battleNet.Channel.GetChannelReferenceObject(partyId);
			if (channelReferenceObject == null)
			{
				this.GenericPartyRequestCallback_Internal(BattleNetErrors.ERROR_INVALID_ARGS, "DissolveParty no channelRefObject", BnetFeatureEvent.Party_Dissolve_Callback, partyId, szPartyType);
				return;
			}
			this.m_rpcConnection.QueueRequest(this.m_battleNet.Channel.ChannelService.Id, 6u, new DissolveRequest(), delegate(RPCContext ctx)
			{
				this.GenericPartyRequestCallback(ctx, "DissolveParty", BnetFeatureEvent.Party_Dissolve_Callback, partyId, szPartyType);
			}, (uint)channelReferenceObject.m_channelData.m_objectId);
		}

		public void SetPartyPrivacy(bnet.protocol.EntityId partyId, int privacyLevel)
		{
			ChannelState channelState = new ChannelState();
			channelState.PrivacyLevel = (ChannelState.Types.PrivacyLevel)privacyLevel;
			this.UpdatePartyState_Internal("SetPartyPrivacy privacy=" + privacyLevel, BnetFeatureEvent.Party_SetPrivacy_Callback, partyId, channelState);
		}

		public void AssignPartyRole(bnet.protocol.EntityId partyId, bnet.protocol.EntityId memberId, uint roleId)
		{
			string szPartyType = this.GetPartyType(partyId);
			if (this.GetPartyData(partyId) == null)
			{
				this.GenericPartyRequestCallback_Internal(BattleNetErrors.ERROR_INVALID_ARGS, "AssignPartyRole no PartyData", BnetFeatureEvent.Party_AssignRole_Callback, partyId, szPartyType);
				return;
			}
			ChannelAPI.ChannelReferenceObject channelReferenceObject = this.m_battleNet.Channel.GetChannelReferenceObject(partyId);
			if (channelReferenceObject == null)
			{
				this.GenericPartyRequestCallback_Internal(BattleNetErrors.ERROR_INVALID_ARGS, "AssignPartyRole no channelRefObject", BnetFeatureEvent.Party_AssignRole_Callback, partyId, szPartyType);
				return;
			}
			UpdateMemberStateRequest updateMemberStateRequest = new UpdateMemberStateRequest();
			Member member = new Member();
			Identity identity = new Identity();
			MemberState memberState = new MemberState();
			memberState.AddRole(roleId);
			identity.SetGameAccountId(memberId);
			member.SetIdentity(identity);
			member.SetState(memberState);
			updateMemberStateRequest.AddStateChange(member);
			this.m_rpcConnection.QueueRequest(this.m_battleNet.Channel.ChannelService.Id, 5u, updateMemberStateRequest, delegate(RPCContext ctx)
			{
				this.GenericPartyRequestCallback(ctx, string.Concat(new object[]
				{
					"AssignPartyRole memberId=",
					memberId.Low,
					" roleId=",
					roleId
				}), BnetFeatureEvent.Party_AssignRole_Callback, partyId, szPartyType);
			}, (uint)channelReferenceObject.m_channelData.m_objectId);
		}

		public void SendPartyInvite(bnet.protocol.EntityId partyId, bnet.protocol.EntityId inviteeId, bool isReservation)
		{
			if (this.GetPartyData(partyId) == null)
			{
				return;
			}
			string szPartyType = this.GetPartyType(partyId);
			this.m_battleNet.Channel.SendInvitation(partyId, inviteeId, ChannelAPI.InvitationServiceType.INVITATION_SERVICE_TYPE_PARTY, delegate(RPCContext ctx)
			{
				this.SendPartyInviteCallback(ctx, partyId, inviteeId, szPartyType);
			});
		}

		public void SendPartyInviteCallback(RPCContext context, bnet.protocol.EntityId partyId, bnet.protocol.EntityId inviteeId, string szPartyType)
		{
			string text = "SendPartyInvite inviteeId=" + inviteeId.Low;
			BattleNetErrors status = (BattleNetErrors)context.Header.Status;
			this.m_battleNet.Party.PushPartyErrorEvent(PartyListenerEventType.OPERATION_CALLBACK, text, status, BnetFeatureEvent.Party_SendInvite_Callback, partyId, szPartyType);
			if (status != BattleNetErrors.ERROR_OK)
			{
				if (partyId != null)
				{
					text = string.Format("PartyRequestError: {0} {1} {2} partyId={3} type={4}", new object[]
					{
						(int)status,
						status.ToString(),
						text,
						partyId.Low,
						szPartyType
					});
				}
				else
				{
					text = string.Format("PartyRequestError: {0} {1} {2} type={3}", new object[]
					{
						(int)status,
						status.ToString(),
						text,
						szPartyType
					});
				}
				this.m_battleNet.Party.ApiLog.LogError(text);
				return;
			}
			if (partyId != null)
			{
				text = string.Format("PartyRequest {0} status={1} partyId={2} type={3}", new object[]
				{
					text,
					status.ToString(),
					partyId.Low,
					szPartyType
				});
			}
			else
			{
				text = string.Format("PartyRequest {0} status={1} type={2}", text, status.ToString(), szPartyType);
			}
			this.m_battleNet.Party.ApiLog.LogDebug(text);
			this.m_battleNet.Channel.RemoveInviteRequestsFor(partyId, inviteeId, 0u);
		}

		public void AcceptPartyInvite(ulong invitationId)
		{
			ChannelAPI.ReceivedInvite receivedInvite = this.m_battleNet.Channel.GetReceivedInvite(ChannelAPI.InvitationServiceType.INVITATION_SERVICE_TYPE_PARTY, invitationId);
			if (receivedInvite == null)
			{
				return;
			}
			string szPartyType = this.GetReceivedInvitationPartyType(invitationId);
			this.m_battleNet.Channel.AcceptInvitation(invitationId, receivedInvite.ChannelId, ChannelAPI.ChannelType.PARTY_CHANNEL, delegate(RPCContext ctx)
			{
				this.GenericPartyRequestCallback(ctx, "AcceptPartyInvite inviteId=" + invitationId, BnetFeatureEvent.Party_AcceptInvite_Callback, null, szPartyType);
			});
		}

		public void DeclinePartyInvite(ulong invitationId)
		{
			ChannelAPI.ReceivedInvite receivedInvite = this.m_battleNet.Channel.GetReceivedInvite(ChannelAPI.InvitationServiceType.INVITATION_SERVICE_TYPE_PARTY, invitationId);
			if (receivedInvite == null)
			{
				return;
			}
			string szPartyType = this.GetReceivedInvitationPartyType(invitationId);
			this.m_battleNet.Channel.DeclineInvitation(invitationId, receivedInvite.ChannelId, delegate(RPCContext ctx)
			{
				this.GenericPartyRequestCallback(ctx, "DeclinePartyInvite inviteId=" + invitationId, BnetFeatureEvent.Party_DeclineInvite_Callback, null, szPartyType);
			});
		}

		public void RevokePartyInvite(bnet.protocol.EntityId partyId, ulong invitationId)
		{
			string szPartyType = this.GetPartyType(partyId);
			this.m_battleNet.Channel.RevokeInvitation(invitationId, partyId, delegate(RPCContext ctx)
			{
				this.GenericPartyRequestCallback(ctx, "RevokePartyInvite inviteId=" + invitationId, BnetFeatureEvent.Party_RevokeInvite_Callback, partyId, szPartyType);
			});
		}

		public void RequestPartyInvite(bnet.protocol.EntityId partyId, bnet.protocol.EntityId whomToAskForApproval, bnet.protocol.EntityId whomToInvite, string szPartyType)
		{
			this.m_battleNet.Channel.SuggestInvitation(partyId, whomToAskForApproval, whomToInvite, delegate(RPCContext ctx)
			{
				this.GenericPartyRequestCallback(ctx, string.Concat(new object[]
				{
					"RequestPartyInvite whomToInvite=",
					whomToInvite,
					" whomToAskForApproval=",
					whomToAskForApproval
				}), BnetFeatureEvent.Party_RequestPartyInvite_Callback, partyId, szPartyType);
			});
		}

		public void IgnoreInviteRequest(bnet.protocol.EntityId partyId, bnet.protocol.EntityId requestedTargetId)
		{
			this.m_battleNet.Channel.RemoveInviteRequestsFor(partyId, requestedTargetId, 1u);
		}

		public void KickPartyMember(bnet.protocol.EntityId partyId, bnet.protocol.EntityId memberId)
		{
			string szPartyType = this.GetPartyType(partyId);
			if (this.GetPartyData(partyId) == null)
			{
				this.GenericPartyRequestCallback_Internal(BattleNetErrors.ERROR_INVALID_ARGS, "KickPartyMember no PartyData", BnetFeatureEvent.Party_KickMember_Callback, partyId, szPartyType);
				return;
			}
			ChannelAPI.ChannelReferenceObject channelReferenceObject = this.m_battleNet.Channel.GetChannelReferenceObject(partyId);
			if (channelReferenceObject == null)
			{
				this.GenericPartyRequestCallback_Internal(BattleNetErrors.ERROR_INVALID_ARGS, "KickPartyMember no channelRefObject", BnetFeatureEvent.Party_KickMember_Callback, partyId, szPartyType);
				return;
			}
			RemoveMemberRequest removeMemberRequest = new RemoveMemberRequest();
			removeMemberRequest.MemberId = memberId;
			this.m_rpcConnection.QueueRequest(this.m_battleNet.Channel.ChannelService.Id, 2u, removeMemberRequest, delegate(RPCContext ctx)
			{
				this.GenericPartyRequestCallback(ctx, "KickPartyMember memberId=" + memberId.Low, BnetFeatureEvent.Party_KickMember_Callback, partyId, szPartyType);
			}, (uint)channelReferenceObject.m_channelData.m_objectId);
		}

		public void SendPartyChatMessage(bnet.protocol.EntityId partyId, string message)
		{
			string szPartyType = this.GetPartyType(partyId);
			if (this.GetPartyData(partyId) == null)
			{
				this.GenericPartyRequestCallback_Internal(BattleNetErrors.ERROR_INVALID_ARGS, "SendPartyChatMessage no PartyData", BnetFeatureEvent.Party_SendChatMessage_Callback, partyId, szPartyType);
				return;
			}
			ChannelAPI.ChannelReferenceObject channelReferenceObject = this.m_battleNet.Channel.GetChannelReferenceObject(partyId);
			if (channelReferenceObject == null)
			{
				this.GenericPartyRequestCallback_Internal(BattleNetErrors.ERROR_INVALID_ARGS, "SendPartyChatMessage no channelRefObject", BnetFeatureEvent.Party_SendChatMessage_Callback, partyId, szPartyType);
				return;
			}
			SendMessageRequest sendMessageRequest = new SendMessageRequest();
			Message message2 = new Message();
			message2.AddAttribute(ProtocolHelper.CreateAttribute(attribute.TEXT_ATTRIBUTE, message));
			sendMessageRequest.SetMessage(message2);
			this.m_rpcConnection.QueueRequest(this.m_battleNet.Channel.ChannelService.Id, 3u, sendMessageRequest, delegate(RPCContext ctx)
			{
				this.GenericPartyRequestCallback(ctx, "SendPartyChatMessage", BnetFeatureEvent.Party_SendChatMessage_Callback, partyId, szPartyType);
			}, (uint)channelReferenceObject.m_channelData.m_objectId);
		}

		public void ClearPartyAttribute(bnet.protocol.EntityId partyId, string attributeKey)
		{
			Attribute attribute = new Attribute();
			Variant value = new Variant();
			attribute.SetName(attributeKey);
			attribute.SetValue(value);
			this.SetPartyAttribute_Internal("ClearPartyAttribute key=" + attributeKey, BnetFeatureEvent.Party_ClearAttribute_Callback, partyId, attribute);
		}

		public void SetPartyAttributeLong(bnet.protocol.EntityId partyId, string attributeKey, long value)
		{
			this.SetPartyAttribute_Internal(string.Concat(new object[]
			{
				"SetPartyAttributeLong key=",
				attributeKey,
				" val=",
				value
			}), BnetFeatureEvent.Party_SetAttribute_Callback, partyId, ProtocolHelper.CreateAttribute(attributeKey, value));
		}

		public void SetPartyAttributeString(bnet.protocol.EntityId partyId, string attributeKey, string value)
		{
			this.SetPartyAttribute_Internal("SetPartyAttributeString key=" + attributeKey + " val=" + value, BnetFeatureEvent.Party_SetAttribute_Callback, partyId, ProtocolHelper.CreateAttribute(attributeKey, value));
		}

		public void SetPartyAttributeBlob(bnet.protocol.EntityId partyId, string attributeKey, byte[] value)
		{
			this.SetPartyAttribute_Internal("SetPartyAttributeBlob key=" + attributeKey + " val=" + ((value != null) ? (value.Length + " bytes") : "null"), BnetFeatureEvent.Party_SetAttribute_Callback, partyId, ProtocolHelper.CreateAttribute(attributeKey, value));
		}

		private void SetPartyAttribute_Internal(string debugMessage, BnetFeatureEvent featureEvent, bnet.protocol.EntityId partyId, Attribute attr)
		{
			ChannelState channelState = new ChannelState();
			channelState.AddAttribute(attr);
			this.UpdatePartyState_Internal(debugMessage, featureEvent, partyId, channelState);
		}

		private void UpdatePartyState_Internal(string debugMessage, BnetFeatureEvent featureEvent, bnet.protocol.EntityId partyId, ChannelState state)
		{
			string szPartyType = this.GetPartyType(partyId);
			if (this.GetPartyData(partyId) == null)
			{
				this.GenericPartyRequestCallback_Internal(BattleNetErrors.ERROR_INVALID_ARGS, string.Format("{0} no PartyData", debugMessage), featureEvent, partyId, szPartyType);
				return;
			}
			ChannelAPI.ChannelReferenceObject channelReferenceObject = this.m_battleNet.Channel.GetChannelReferenceObject(partyId);
			if (channelReferenceObject == null)
			{
				this.GenericPartyRequestCallback_Internal(BattleNetErrors.ERROR_INVALID_ARGS, string.Format("{0} no channelRefObject", debugMessage), featureEvent, partyId, szPartyType);
				return;
			}
			UpdateChannelStateRequest updateChannelStateRequest = new UpdateChannelStateRequest();
			updateChannelStateRequest.StateChange = state;
			this.m_rpcConnection.QueueRequest(this.m_battleNet.Channel.ChannelService.Id, 4u, updateChannelStateRequest, delegate(RPCContext ctx)
			{
				this.GenericPartyRequestCallback(ctx, debugMessage, featureEvent, partyId, szPartyType);
			}, (uint)channelReferenceObject.m_channelData.m_objectId);
		}

		public int GetPartyPrivacy(bnet.protocol.EntityId partyId)
		{
			int result = 4;
			ChannelAPI.ChannelReferenceObject channelReferenceObject = this.m_battleNet.Channel.GetChannelReferenceObject(partyId);
			if (channelReferenceObject == null)
			{
				return result;
			}
			ChannelAPI.ChannelData channelData = channelReferenceObject.m_channelData as ChannelAPI.ChannelData;
			if (channelData != null && channelData.m_channelState != null && channelData.m_channelState.HasPrivacyLevel)
			{
				result = (int)channelData.m_channelState.PrivacyLevel;
			}
			return result;
		}

		public int GetCountPartyMembers(bnet.protocol.EntityId partyId)
		{
			ChannelAPI.ChannelReferenceObject channelReferenceObject = this.m_battleNet.Channel.GetChannelReferenceObject(partyId);
			if (channelReferenceObject != null)
			{
				ChannelAPI.ChannelData channelData = channelReferenceObject.m_channelData as ChannelAPI.ChannelData;
				if (channelData != null && channelData.m_members != null)
				{
					return channelData.m_members.Count;
				}
			}
			return 0;
		}

		public int GetMaxPartyMembers(bnet.protocol.EntityId partyId)
		{
			ChannelState partyState = this.GetPartyState(partyId);
			if (partyState != null && partyState.HasMaxMembers)
			{
				return (int)partyState.MaxMembers;
			}
			return 0;
		}

		public void GetPartyMembers(bnet.protocol.EntityId partyId, out bgs.types.PartyMember[] members)
		{
			members = null;
			ChannelAPI.ChannelReferenceObject channelReferenceObject = this.m_battleNet.Channel.GetChannelReferenceObject(partyId);
			if (channelReferenceObject != null)
			{
				ChannelAPI.ChannelData channelData = channelReferenceObject.m_channelData as ChannelAPI.ChannelData;
				if (channelData != null && channelData.m_members != null)
				{
					members = new bgs.types.PartyMember[channelData.m_members.Count];
					int num = 0;
					foreach (KeyValuePair<bnet.protocol.EntityId, Member> current in channelData.m_members)
					{
						bnet.protocol.EntityId key = current.get_Key();
						Member value = current.get_Value();
						bgs.types.PartyMember partyMember = default(bgs.types.PartyMember);
						partyMember.memberGameAccountId = new bgs.types.EntityId(key);
						if (value.State.RoleCount > 0)
						{
							partyMember.firstMemberRole = value.State.RoleList.get_Item(0);
						}
						members[num] = partyMember;
						num++;
					}
				}
			}
			if (members == null)
			{
				members = new bgs.types.PartyMember[0];
			}
		}

		public void GetReceivedPartyInvites(out PartyInvite[] invites)
		{
			ChannelAPI.ReceivedInvite[] receivedInvites = this.m_battleNet.Channel.GetReceivedInvites(ChannelAPI.InvitationServiceType.INVITATION_SERVICE_TYPE_PARTY);
			invites = new PartyInvite[receivedInvites.Length];
			for (int i = 0; i < invites.Length; i++)
			{
				ChannelAPI.ReceivedInvite receivedInvite = receivedInvites[i];
				Invitation invitation = receivedInvite.Invitation;
				string receivedInvitationPartyType = this.GetReceivedInvitationPartyType(invitation.Id);
				PartyType partyTypeFromString = BnetParty.GetPartyTypeFromString(receivedInvitationPartyType);
				PartyInvite partyInvite = new PartyInvite();
				partyInvite.InviteId = invitation.Id;
				partyInvite.PartyId = PartyId.FromProtocol(receivedInvite.ChannelId);
				partyInvite.PartyType = partyTypeFromString;
				partyInvite.InviterName = invitation.InviterName;
				partyInvite.InviterId = BnetGameAccountId.CreateFromProtocol(invitation.InviterIdentity.GameAccountId);
				partyInvite.InviteeId = BnetGameAccountId.CreateFromProtocol(invitation.InviteeIdentity.GameAccountId);
				invites[i] = partyInvite;
			}
		}

		private ChannelState GetPartyState(bnet.protocol.EntityId partyId)
		{
			ChannelAPI.ChannelReferenceObject channelReferenceObject = this.m_battleNet.Channel.GetChannelReferenceObject(partyId);
			if (channelReferenceObject != null)
			{
				ChannelAPI.ChannelData channelData = channelReferenceObject.m_channelData as ChannelAPI.ChannelData;
				return channelData.m_channelState;
			}
			return null;
		}

		public void GetPartySentInvites(bnet.protocol.EntityId partyId, out PartyInvite[] invites)
		{
			invites = null;
			ChannelState partyState = this.GetPartyState(partyId);
			if (partyState != null)
			{
				invites = new PartyInvite[partyState.InvitationCount];
				string partyType = this.GetPartyType(partyId);
				PartyType partyTypeFromString = BnetParty.GetPartyTypeFromString(partyType);
				for (int i = 0; i < invites.Length; i++)
				{
					Invitation invitation = partyState.InvitationList.get_Item(i);
					PartyInvite partyInvite = new PartyInvite();
					partyInvite.InviteId = invitation.Id;
					partyInvite.PartyId = PartyId.FromProtocol(partyId);
					partyInvite.PartyType = partyTypeFromString;
					partyInvite.InviterName = invitation.InviterName;
					partyInvite.InviterId = BnetGameAccountId.CreateFromProtocol(invitation.InviterIdentity.GameAccountId);
					partyInvite.InviteeId = BnetGameAccountId.CreateFromProtocol(invitation.InviteeIdentity.GameAccountId);
					invites[i] = partyInvite;
				}
			}
			if (invites == null)
			{
				invites = new PartyInvite[0];
			}
		}

		public void GetPartyInviteRequests(bnet.protocol.EntityId partyId, out InviteRequest[] requests)
		{
			Suggestion[] inviteRequests = this.m_battleNet.Channel.GetInviteRequests(partyId);
			requests = new InviteRequest[inviteRequests.Length];
			for (int i = 0; i < requests.Length; i++)
			{
				Suggestion suggestion = inviteRequests[i];
				InviteRequest inviteRequest = new InviteRequest();
				inviteRequest.TargetName = suggestion.SuggesteeName;
				inviteRequest.TargetId = BnetGameAccountId.CreateFromProtocol(suggestion.SuggesteeId);
				inviteRequest.RequesterName = suggestion.SuggesterName;
				inviteRequest.RequesterId = BnetGameAccountId.CreateFromProtocol(suggestion.SuggesterId);
				requests[i] = inviteRequest;
			}
		}

		public void GetAllPartyAttributes(bnet.protocol.EntityId partyId, out string[] allKeys)
		{
			ChannelAPI.ReceivedInvite receivedInvite = Enumerable.FirstOrDefault<ChannelAPI.ReceivedInvite>(this.m_battleNet.Channel.GetAllReceivedInvites(), (ChannelAPI.ReceivedInvite i) => i.ChannelId != null && i.ChannelId.High == partyId.High && i.ChannelId.Low == partyId.Low);
			ChannelState channelState;
			if (receivedInvite != null && receivedInvite.State != null)
			{
				channelState = receivedInvite.State;
			}
			else
			{
				channelState = this.GetPartyState(partyId);
			}
			if (channelState == null)
			{
				allKeys = new string[0];
			}
			else
			{
				allKeys = new string[channelState.AttributeList.get_Count()];
				for (int j = 0; j < channelState.AttributeList.get_Count(); j++)
				{
					Attribute attribute = channelState.AttributeList.get_Item(j);
					allKeys[j] = attribute.Name;
				}
			}
		}

		public bool GetPartyAttributeLong(bnet.protocol.EntityId partyId, string attributeKey, out long value)
		{
			value = 0L;
			Attribute attribute = this.GetReceivedInvitationAttribute(partyId, attributeKey);
			if (attribute != null && attribute.Value.HasIntValue)
			{
				value = attribute.Value.IntValue;
				return true;
			}
			ChannelState partyState = this.GetPartyState(partyId);
			if (partyState != null)
			{
				for (int i = 0; i < partyState.AttributeList.get_Count(); i++)
				{
					attribute = partyState.AttributeList.get_Item(i);
					if (attribute.Name == attributeKey && attribute.Value.HasIntValue)
					{
						value = attribute.Value.IntValue;
						return true;
					}
				}
			}
			return false;
		}

		public void GetPartyAttributeString(bnet.protocol.EntityId partyId, string attributeKey, out string value)
		{
			value = null;
			Attribute attribute = this.GetReceivedInvitationAttribute(partyId, attributeKey);
			if (attribute != null && attribute.Value.HasStringValue)
			{
				value = attribute.Value.StringValue;
				return;
			}
			ChannelState partyState = this.GetPartyState(partyId);
			if (partyState != null)
			{
				for (int i = 0; i < partyState.AttributeList.get_Count(); i++)
				{
					attribute = partyState.AttributeList.get_Item(i);
					if (attribute.Name == attributeKey && attribute.Value.HasStringValue)
					{
						value = attribute.Value.StringValue;
						break;
					}
				}
			}
		}

		public void GetPartyAttributeBlob(bnet.protocol.EntityId partyId, string attributeKey, out byte[] value)
		{
			value = null;
			Attribute attribute = this.GetReceivedInvitationAttribute(partyId, attributeKey);
			if (attribute != null && attribute.Value.HasBlobValue)
			{
				value = attribute.Value.BlobValue;
				return;
			}
			ChannelState partyState = this.GetPartyState(partyId);
			if (partyState != null)
			{
				for (int i = 0; i < partyState.AttributeList.get_Count(); i++)
				{
					attribute = partyState.AttributeList.get_Item(i);
					if (attribute.Name == attributeKey && attribute.Value.HasBlobValue)
					{
						value = attribute.Value.BlobValue;
						break;
					}
				}
			}
		}

		public void PartyJoined(ChannelAPI.ChannelReferenceObject channelRefObject, AddNotification notification)
		{
			ChannelAPI.ChannelData channelData = (ChannelAPI.ChannelData)channelRefObject.m_channelData;
			bnet.protocol.EntityId channelId = channelData.m_channelId;
			string partyType = this.GetPartyType(channelId);
			PartyAPI.PartyData partyData = this.GetPartyData(channelId);
			bool flag = false;
			if (partyData == null)
			{
				PartyType partyTypeFromString = BnetParty.GetPartyTypeFromString(partyType);
				if (partyTypeFromString == PartyType.FRIENDLY_CHALLENGE)
				{
					flag = true;
					using (List<Member>.Enumerator enumerator = notification.MemberList.GetEnumerator())
					{
						while (enumerator.MoveNext())
						{
							Member current = enumerator.get_Current();
							bnet.protocol.EntityId gameAccountId = current.Identity.GameAccountId;
							if (!this.m_battleNet.GameAccountId.Equals(gameAccountId))
							{
								PartyAPI.PartyData partyData2 = new PartyAPI.PartyData(this.m_battleNet, channelId, gameAccountId);
								this.m_activeParties.Add(channelId, partyData2);
								partyData2.FriendlyChallenge_HandleChannelAttributeUpdate(notification.ChannelState.AttributeList);
								break;
							}
						}
					}
				}
				else
				{
					PartyAPI.PartyData value = new PartyAPI.PartyData(this.m_battleNet, channelId, null);
					this.m_activeParties.Add(channelId, value);
				}
			}
			else
			{
				flag = (partyData != null && partyData.m_friendGameAccount != null);
			}
			if (flag)
			{
				this.FriendlyChallenge_PushStateChange(channelId, "wait", false);
			}
			PartyListenerEvent partyListenerEvent = default(PartyListenerEvent);
			partyListenerEvent.Type = PartyListenerEventType.JOINED_PARTY;
			partyListenerEvent.PartyId = PartyId.FromProtocol(channelId);
			partyListenerEvent.StringData = partyType;
			this.m_partyListenerEvents.Add(partyListenerEvent);
		}

		public void PartyLeft(ChannelAPI.ChannelReferenceObject channelRefObject, RemoveNotification notification)
		{
			ChannelAPI.ChannelData channelData = (ChannelAPI.ChannelData)channelRefObject.m_channelData;
			bnet.protocol.EntityId channelId = channelRefObject.m_channelData.m_channelId;
			string partyType = this.GetPartyType(channelId);
			PartyAPI.PartyData partyData = this.GetPartyData(channelData.m_channelId);
			if (partyData != null)
			{
				if (partyData.m_friendGameAccount != null)
				{
					string data = "NO_SUPPLIED_REASON";
					if (notification.HasReason)
					{
						data = notification.Reason.ToString();
					}
					this.PushPartyEvent(partyData.m_partyId, "left", data, partyData.m_friendGameAccount);
				}
				this.m_activeParties.Remove(partyData.m_partyId);
			}
			PartyListenerEvent partyListenerEvent = default(PartyListenerEvent);
			partyListenerEvent.Type = PartyListenerEventType.LEFT_PARTY;
			partyListenerEvent.PartyId = PartyId.FromProtocol(channelId);
			partyListenerEvent.StringData = partyType;
			partyListenerEvent.UintData = ((!notification.HasReason) ? 0u : notification.Reason);
			this.m_partyListenerEvents.Add(partyListenerEvent);
		}

		public void PartyMemberJoined(ChannelAPI.ChannelReferenceObject channelRefObject, JoinNotification notification)
		{
			bnet.protocol.EntityId channelId = channelRefObject.m_channelData.m_channelId;
			PartyListenerEvent partyListenerEvent = default(PartyListenerEvent);
			partyListenerEvent.Type = PartyListenerEventType.MEMBER_JOINED;
			partyListenerEvent.PartyId = PartyId.FromProtocol(channelId);
			partyListenerEvent.SubjectMemberId = BnetGameAccountId.CreateFromProtocol(notification.Member.Identity.GameAccountId);
			this.m_partyListenerEvents.Add(partyListenerEvent);
		}

		public void MemberRolesChanged(ChannelAPI.ChannelReferenceObject channelRefObject, IEnumerable<bnet.protocol.EntityId> membersWithRoleChanges)
		{
			bnet.protocol.EntityId channelId = channelRefObject.m_channelData.m_channelId;
			string partyType = this.GetPartyType(channelId);
			PartyListenerEvent partyListenerEvent = default(PartyListenerEvent);
			partyListenerEvent.Type = PartyListenerEventType.MEMBER_ROLE_CHANGED;
			partyListenerEvent.PartyId = PartyId.FromProtocol(channelId);
			partyListenerEvent.StringData = partyType;
			using (IEnumerator<bnet.protocol.EntityId> enumerator = membersWithRoleChanges.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					bnet.protocol.EntityId current = enumerator.get_Current();
					partyListenerEvent.SubjectMemberId = BnetGameAccountId.CreateFromProtocol(current);
					this.m_partyListenerEvents.Add(partyListenerEvent);
				}
			}
		}

		public void PartyMemberLeft(ChannelAPI.ChannelReferenceObject channelRefObject, LeaveNotification notification)
		{
			bnet.protocol.EntityId channelId = channelRefObject.m_channelData.m_channelId;
			PartyAPI.PartyData partyData = this.GetPartyData(channelId);
			if (partyData != null && partyData.m_friendGameAccount != null)
			{
				this.m_rpcConnection.QueueRequest(this.m_battleNet.Channel.ChannelService.Id, 6u, new DissolveRequest(), new RPCContextDelegate(partyData.PartyMemberLeft_DissolvePartyInviteCallback), (uint)channelRefObject.m_channelData.m_objectId);
			}
			PartyListenerEvent partyListenerEvent = default(PartyListenerEvent);
			partyListenerEvent.Type = PartyListenerEventType.MEMBER_LEFT;
			partyListenerEvent.PartyId = PartyId.FromProtocol(channelId);
			partyListenerEvent.SubjectMemberId = BnetGameAccountId.CreateFromProtocol(notification.MemberId);
			partyListenerEvent.UintData = ((!notification.HasReason) ? 0u : notification.Reason);
			this.m_partyListenerEvents.Add(partyListenerEvent);
		}

		public void PreprocessPartyChannelUpdated(ChannelAPI.ChannelReferenceObject channelRefObject, UpdateChannelStateNotification notification)
		{
			bnet.protocol.EntityId channelId = channelRefObject.m_channelData.m_channelId;
			PartyAPI.PartyData partyData = this.GetPartyData(channelId);
			if (partyData != null && partyData.m_friendGameAccount != null)
			{
				partyData.FriendlyChallenge_HandleChannelAttributeUpdate(notification.StateChange.AttributeList);
			}
		}

		public void ReceivedInvitationAdded(InvitationAddedNotification notification, ChannelInvitation channelInvitation)
		{
			string receivedInvitationPartyType = this.GetReceivedInvitationPartyType(notification.Invitation.Id);
			PartyType partyTypeFromString = BnetParty.GetPartyTypeFromString(receivedInvitationPartyType);
			if (partyTypeFromString == PartyType.FRIENDLY_CHALLENGE)
			{
				this.m_battleNet.Channel.AcceptInvitation(notification.Invitation.Id, channelInvitation.ChannelDescription.ChannelId, ChannelAPI.ChannelType.PARTY_CHANNEL, null);
			}
			PartyListenerEvent partyListenerEvent = default(PartyListenerEvent);
			partyListenerEvent.Type = PartyListenerEventType.RECEIVED_INVITE_ADDED;
			partyListenerEvent.PartyId = PartyId.FromProtocol(channelInvitation.ChannelDescription.ChannelId);
			partyListenerEvent.UlongData = notification.Invitation.Id;
			partyListenerEvent.SubjectMemberId = BnetGameAccountId.CreateFromProtocol(notification.Invitation.InviterIdentity.GameAccountId);
			partyListenerEvent.TargetMemberId = BnetGameAccountId.CreateFromProtocol(notification.Invitation.InviteeIdentity.GameAccountId);
			partyListenerEvent.StringData = receivedInvitationPartyType;
			partyListenerEvent.StringData2 = notification.Invitation.InviterName;
			partyListenerEvent.UintData = 0u;
			if (channelInvitation.HasReserved && channelInvitation.Reserved)
			{
				partyListenerEvent.UintData |= 1u;
			}
			if (channelInvitation.HasRejoin && channelInvitation.Rejoin)
			{
				partyListenerEvent.UintData |= 1u;
			}
			this.m_partyListenerEvents.Add(partyListenerEvent);
		}

		public void ReceivedInvitationRemoved(string szPartyType, InvitationRemovedNotification notification, ChannelInvitation channelInvitation)
		{
			PartyListenerEvent partyListenerEvent = default(PartyListenerEvent);
			partyListenerEvent.Type = PartyListenerEventType.RECEIVED_INVITE_REMOVED;
			partyListenerEvent.PartyId = PartyId.FromProtocol(channelInvitation.ChannelDescription.ChannelId);
			partyListenerEvent.UlongData = notification.Invitation.Id;
			partyListenerEvent.SubjectMemberId = BnetGameAccountId.CreateFromProtocol(notification.Invitation.InviterIdentity.GameAccountId);
			partyListenerEvent.TargetMemberId = BnetGameAccountId.CreateFromProtocol(notification.Invitation.InviteeIdentity.GameAccountId);
			partyListenerEvent.StringData = szPartyType;
			partyListenerEvent.StringData2 = notification.Invitation.InviterName;
			if (notification.HasReason)
			{
				partyListenerEvent.UintData = notification.Reason;
			}
			this.m_partyListenerEvents.Add(partyListenerEvent);
		}

		public void PartyInvitationDelta(bnet.protocol.EntityId partyId, Invitation invite, uint? removeReason)
		{
			string partyType = this.GetPartyType(partyId);
			PartyListenerEvent partyListenerEvent = default(PartyListenerEvent);
			partyListenerEvent.Type = ((!removeReason.get_HasValue()) ? PartyListenerEventType.PARTY_INVITE_SENT : PartyListenerEventType.PARTY_INVITE_REMOVED);
			partyListenerEvent.PartyId = PartyId.FromProtocol(partyId);
			partyListenerEvent.UlongData = invite.Id;
			partyListenerEvent.SubjectMemberId = BnetGameAccountId.CreateFromProtocol(invite.InviterIdentity.GameAccountId);
			partyListenerEvent.TargetMemberId = BnetGameAccountId.CreateFromProtocol(invite.InviteeIdentity.GameAccountId);
			partyListenerEvent.StringData = partyType;
			partyListenerEvent.StringData2 = invite.InviterName;
			if (removeReason.get_HasValue())
			{
				partyListenerEvent.UintData = removeReason.get_Value();
			}
			else
			{
				partyListenerEvent.UintData = 0u;
				if (invite.HasChannelInvitation)
				{
					ChannelInvitation channelInvitation = invite.ChannelInvitation;
					if (channelInvitation.HasReserved && channelInvitation.Reserved)
					{
						partyListenerEvent.UintData |= 1u;
					}
					if (channelInvitation.HasRejoin && channelInvitation.Rejoin)
					{
						partyListenerEvent.UintData |= 1u;
					}
				}
			}
			this.m_partyListenerEvents.Add(partyListenerEvent);
		}

		public void ReceivedInviteRequestDelta(bnet.protocol.EntityId partyId, Suggestion suggestion, uint? removeReason)
		{
			PartyListenerEvent partyListenerEvent = default(PartyListenerEvent);
			partyListenerEvent.Type = ((!removeReason.get_HasValue()) ? PartyListenerEventType.INVITE_REQUEST_ADDED : PartyListenerEventType.INVITE_REQUEST_REMOVED);
			partyListenerEvent.PartyId = PartyId.FromProtocol(partyId);
			partyListenerEvent.SubjectMemberId = BnetGameAccountId.CreateFromProtocol(suggestion.SuggesterId);
			partyListenerEvent.TargetMemberId = BnetGameAccountId.CreateFromProtocol(suggestion.SuggesteeId);
			partyListenerEvent.StringData = suggestion.SuggesterName;
			partyListenerEvent.StringData2 = suggestion.SuggesteeName;
			if (removeReason.get_HasValue())
			{
				partyListenerEvent.UintData = removeReason.get_Value();
			}
			this.m_partyListenerEvents.Add(partyListenerEvent);
		}

		public void PartyMessageReceived(ChannelAPI.ChannelReferenceObject channelRefObject, SendMessageNotification notification)
		{
			bnet.protocol.EntityId channelId = channelRefObject.m_channelData.m_channelId;
			string text = null;
			for (int i = 0; i < notification.Message.AttributeCount; i++)
			{
				Attribute attribute = notification.Message.AttributeList.get_Item(i);
				if (attribute.TEXT_ATTRIBUTE.Equals(attribute.Name) && attribute.Value.HasStringValue)
				{
					text = attribute.Value.StringValue;
					break;
				}
			}
			if (string.IsNullOrEmpty(text))
			{
				return;
			}
			PartyListenerEvent partyListenerEvent = default(PartyListenerEvent);
			partyListenerEvent.Type = PartyListenerEventType.CHAT_MESSAGE_RECEIVED;
			partyListenerEvent.PartyId = PartyId.FromProtocol(channelId);
			partyListenerEvent.SubjectMemberId = BnetGameAccountId.CreateFromProtocol(notification.AgentId);
			partyListenerEvent.StringData = text;
			this.m_partyListenerEvents.Add(partyListenerEvent);
		}

		public void PartyPrivacyChanged(bnet.protocol.EntityId channelId, ChannelState.Types.PrivacyLevel newPrivacyLevel)
		{
			string partyType = this.GetPartyType(channelId);
			PartyListenerEvent partyListenerEvent = default(PartyListenerEvent);
			partyListenerEvent.Type = PartyListenerEventType.PRIVACY_CHANGED;
			partyListenerEvent.PartyId = PartyId.FromProtocol(channelId);
			partyListenerEvent.UintData = (uint)newPrivacyLevel;
			partyListenerEvent.StringData = partyType;
			this.m_partyListenerEvents.Add(partyListenerEvent);
		}

		public void PartyAttributeChanged(bnet.protocol.EntityId channelId, string attributeKey, Variant attributeValue)
		{
			PartyListenerEvent partyListenerEvent = default(PartyListenerEvent);
			partyListenerEvent.Type = PartyListenerEventType.PARTY_ATTRIBUTE_CHANGED;
			partyListenerEvent.PartyId = PartyId.FromProtocol(channelId);
			partyListenerEvent.StringData = attributeKey;
			if (attributeValue.IsNone())
			{
				partyListenerEvent.UintData = 0u;
			}
			else if (attributeValue.HasIntValue)
			{
				partyListenerEvent.UintData = 1u;
				partyListenerEvent.UlongData = (ulong)attributeValue.IntValue;
			}
			else if (attributeValue.HasStringValue)
			{
				partyListenerEvent.UintData = 2u;
				partyListenerEvent.StringData2 = attributeValue.StringValue;
			}
			else if (attributeValue.HasBlobValue)
			{
				partyListenerEvent.UintData = 3u;
				partyListenerEvent.BlobData = attributeValue.BlobValue;
			}
			else
			{
				partyListenerEvent.UintData = 0u;
			}
			this.m_partyListenerEvents.Add(partyListenerEvent);
		}
	}
}

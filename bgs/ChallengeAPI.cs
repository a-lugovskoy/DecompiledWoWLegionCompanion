using bgs.RPCServices;
using bgs.types;
using bnet.protocol.challenge;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

namespace bgs
{
	public class ChallengeAPI : BattleNetAPI
	{
		private const uint PWD_FOURCC = 5265220u;

		private ServiceDescriptor m_challengeService = new ChallengeService();

		private ServiceDescriptor m_challengeNotifyService = new ChallengeNotify();

		private List<ChallengeInfo> m_challengeUpdateList = new List<ChallengeInfo>();

		private Dictionary<uint, ChallengeInfo> m_challengePendingList = new Dictionary<uint, ChallengeInfo>();

		private Dictionary<uint, ulong> s_pendingAnswers = new Dictionary<uint, ulong>();

		private ExternalChallenge m_nextExternalChallenge;

		public ServiceDescriptor ChallengeService
		{
			get
			{
				return this.m_challengeService;
			}
		}

		public ServiceDescriptor ChallengeNotifyService
		{
			get
			{
				return this.m_challengeNotifyService;
			}
		}

		public ChallengeAPI(BattleNetCSharp battlenet) : base(battlenet, "Challenge")
		{
		}

		public override void InitRPCListeners(RPCConnection rpcConnection)
		{
			base.InitRPCListeners(rpcConnection);
			rpcConnection.RegisterServiceMethodListener(this.m_challengeNotifyService.Id, 1u, new RPCContextDelegate(this.ChallengeUserCallback));
			rpcConnection.RegisterServiceMethodListener(this.m_challengeNotifyService.Id, 2u, new RPCContextDelegate(this.ChallengeResultCallback));
			rpcConnection.RegisterServiceMethodListener(this.m_challengeNotifyService.Id, 3u, new RPCContextDelegate(this.OnExternalChallengeCallback));
			rpcConnection.RegisterServiceMethodListener(this.m_challengeNotifyService.Id, 4u, new RPCContextDelegate(this.OnExternalChallengeResultCallback));
		}

		public override void Initialize()
		{
			base.Initialize();
		}

		public override void OnDisconnected()
		{
			base.OnDisconnected();
		}

		public int NumChallenges()
		{
			return this.m_challengeUpdateList.get_Count();
		}

		public void ClearChallenges()
		{
			this.m_challengeUpdateList.Clear();
		}

		public ExternalChallenge GetNextExternalChallenge()
		{
			ExternalChallenge nextExternalChallenge = this.m_nextExternalChallenge;
			if (this.m_nextExternalChallenge != null)
			{
				this.m_nextExternalChallenge = this.m_nextExternalChallenge.Next;
			}
			return nextExternalChallenge;
		}

		public void GetChallenges([Out] ChallengeInfo[] challenges)
		{
			this.m_challengeUpdateList.CopyTo(challenges);
		}

		public void AnswerChallenge(ulong challengeID, string answer)
		{
			ChallengeAnsweredRequest challengeAnsweredRequest = new ChallengeAnsweredRequest();
			challengeAnsweredRequest.SetAnswer("pass");
			byte[] data = new byte[]
			{
				2,
				3,
				4,
				5,
				6,
				7
			};
			challengeAnsweredRequest.SetData(data);
			if (!challengeAnsweredRequest.IsInitialized)
			{
				return;
			}
			RPCContext rPCContext = this.m_rpcConnection.QueueRequest(this.ChallengeService.Id, 2u, challengeAnsweredRequest, new RPCContextDelegate(this.ChallengeAnsweredCallback), 0u);
			this.s_pendingAnswers.Add(rPCContext.Header.Token, challengeID);
		}

		public void CancelChallenge(ulong challengeID)
		{
			this.AbortChallenge(challengeID);
		}

		private void ChallengeUserCallback(RPCContext context)
		{
			ChallengeUserRequest challengeUserRequest = ChallengeUserRequest.ParseFrom(context.Payload);
			if (!challengeUserRequest.IsInitialized)
			{
				return;
			}
			ulong num = (ulong)challengeUserRequest.Id;
			bool flag = false;
			for (int i = 0; i < challengeUserRequest.ChallengesCount; i++)
			{
				Challenge challenge = challengeUserRequest.Challenges.get_Item(i);
				if (challenge.Type == 5265220u)
				{
					flag = true;
					break;
				}
			}
			if (!flag)
			{
				this.AbortChallenge(num);
				return;
			}
			ChallengePickedRequest challengePickedRequest = new ChallengePickedRequest();
			challengePickedRequest.SetChallenge(5265220u);
			challengePickedRequest.SetId((uint)num);
			challengePickedRequest.SetNewChallengeProtocol(true);
			RPCContext rPCContext = this.m_rpcConnection.QueueRequest(this.ChallengeService.Id, 1u, challengePickedRequest, new RPCContextDelegate(this.ChallengedPickedCallback), 0u);
			ChallengeInfo challengeInfo = default(ChallengeInfo);
			challengeInfo.challengeId = num;
			challengeInfo.isRetry = false;
			this.m_challengePendingList.Add(rPCContext.Header.Token, challengeInfo);
		}

		private void ChallengeResultCallback(RPCContext context)
		{
		}

		private void ChallengedPickedCallback(RPCContext context)
		{
			ChallengeInfo challengeInfo;
			if (!this.m_challengePendingList.TryGetValue(context.Header.Token, ref challengeInfo))
			{
				base.ApiLog.LogWarning("Battle.net Challenge API C#: Received unexpected ChallengedPicked.");
				return;
			}
			BattleNetErrors status = (BattleNetErrors)context.Header.Status;
			if (status != BattleNetErrors.ERROR_OK)
			{
				this.m_challengePendingList.Remove(context.Header.Token);
				base.ApiLog.LogWarning("Battle.net Challenge API C#: Failed ChallengedPicked. " + status);
				return;
			}
			this.m_challengeUpdateList.Add(challengeInfo);
			this.m_challengePendingList.Remove(context.Header.Token);
		}

		private void ChallengeAnsweredCallback(RPCContext context)
		{
			ChallengeAnsweredResponse challengeAnsweredResponse = ChallengeAnsweredResponse.ParseFrom(context.Payload);
			if (!challengeAnsweredResponse.IsInitialized)
			{
				return;
			}
			ulong challengeId = 0uL;
			if (!this.s_pendingAnswers.TryGetValue(context.Header.Token, ref challengeId))
			{
				return;
			}
			if (challengeAnsweredResponse.HasDoRetry && challengeAnsweredResponse.DoRetry)
			{
				ChallengeInfo challengeInfo = default(ChallengeInfo);
				challengeInfo.challengeId = challengeId;
				challengeInfo.isRetry = true;
				this.m_challengeUpdateList.Add(challengeInfo);
			}
			this.s_pendingAnswers.Remove(context.Header.Token);
		}

		private void AbortChallengeCallback(RPCContext context)
		{
		}

		private void AbortChallenge(ulong id)
		{
			ChallengeCancelledRequest challengeCancelledRequest = new ChallengeCancelledRequest();
			challengeCancelledRequest.SetId((uint)id);
			this.m_rpcConnection.QueueRequest(this.ChallengeService.Id, 3u, challengeCancelledRequest, new RPCContextDelegate(this.AbortChallengeCallback), 0u);
			uint num = 0u;
			bool flag = false;
			using (Dictionary<uint, ChallengeInfo>.Enumerator enumerator = this.m_challengePendingList.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					KeyValuePair<uint, ChallengeInfo> current = enumerator.get_Current();
					if (current.get_Value().challengeId == id)
					{
						num = current.get_Key();
						flag = true;
						break;
					}
				}
			}
			if (flag)
			{
				this.m_challengePendingList.Remove(num);
			}
		}

		private void OnExternalChallengeCallback(RPCContext context)
		{
			ChallengeExternalRequest challengeExternalRequest = ChallengeExternalRequest.ParseFrom(context.Payload);
			if (!challengeExternalRequest.IsInitialized || !challengeExternalRequest.HasPayload)
			{
				base.ApiLog.LogWarning("Bad ChallengeExternalRequest received IsInitialized={0} HasRequestToken={1} HasPayload={2} HasPayloadType={3}", new object[]
				{
					challengeExternalRequest.IsInitialized,
					challengeExternalRequest.HasRequestToken,
					challengeExternalRequest.HasPayload,
					challengeExternalRequest.HasPayloadType
				});
				return;
			}
			if (challengeExternalRequest.PayloadType != "web_auth_url")
			{
				base.ApiLog.LogWarning("Received a PayloadType we don't know how to handle PayloadType={0}", new object[]
				{
					challengeExternalRequest.PayloadType
				});
				return;
			}
			ExternalChallenge externalChallenge = new ExternalChallenge();
			externalChallenge.PayLoadType = challengeExternalRequest.PayloadType;
			externalChallenge.URL = Encoding.get_ASCII().GetString(challengeExternalRequest.Payload);
			base.ApiLog.LogDebug("Received external challenge PayLoadType={0} URL={1}", new object[]
			{
				externalChallenge.PayLoadType,
				externalChallenge.URL
			});
			if (this.m_nextExternalChallenge == null)
			{
				this.m_nextExternalChallenge = externalChallenge;
			}
			else
			{
				this.m_nextExternalChallenge.Next = externalChallenge;
			}
		}

		private void OnExternalChallengeResultCallback(RPCContext context)
		{
		}
	}
}

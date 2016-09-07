using System;
using System.Collections.Generic;
using WowJamMessages;

public class PersistentFollowerData
{
	private static PersistentFollowerData s_instance;

	private Dictionary<int, JamGarrisonFollower> m_followerDictionary;

	private Dictionary<int, JamGarrisonFollower> m_preMissionFollowerDictionary;

	private static PersistentFollowerData instance
	{
		get
		{
			if (PersistentFollowerData.s_instance == null)
			{
				PersistentFollowerData.s_instance = new PersistentFollowerData();
				PersistentFollowerData.s_instance.m_followerDictionary = new Dictionary<int, JamGarrisonFollower>();
				PersistentFollowerData.s_instance.m_preMissionFollowerDictionary = new Dictionary<int, JamGarrisonFollower>();
			}
			return PersistentFollowerData.s_instance;
		}
	}

	public static Dictionary<int, JamGarrisonFollower> followerDictionary
	{
		get
		{
			return PersistentFollowerData.instance.m_followerDictionary;
		}
	}

	public static Dictionary<int, JamGarrisonFollower> preMissionFollowerDictionary
	{
		get
		{
			return PersistentFollowerData.instance.m_preMissionFollowerDictionary;
		}
	}

	public static void AddOrUpdateFollower(JamGarrisonFollower follower)
	{
		if (PersistentFollowerData.instance.m_followerDictionary.ContainsKey(follower.GarrFollowerID))
		{
			PersistentFollowerData.instance.m_followerDictionary.Remove(follower.GarrFollowerID);
		}
		PersistentFollowerData.instance.m_followerDictionary.Add(follower.GarrFollowerID, follower);
	}

	public static void ClearData()
	{
		PersistentFollowerData.instance.m_followerDictionary.Clear();
	}

	public static void UpdateFollower(JamGarrisonMissionFollowerInfo followerInfo)
	{
		using (Dictionary<int, JamGarrisonFollower>.Enumerator enumerator = PersistentFollowerData.instance.m_followerDictionary.GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				KeyValuePair<int, JamGarrisonFollower> current = enumerator.get_Current();
				if (current.get_Value().DbID == followerInfo.FollowerDBID)
				{
					current.get_Value().CurrentMissionID = 0;
					break;
				}
			}
		}
	}

	public static void CachePreMissionFollower(JamGarrisonFollower follower)
	{
		if (PersistentFollowerData.instance.m_preMissionFollowerDictionary.ContainsKey(follower.GarrFollowerID))
		{
			PersistentFollowerData.instance.m_preMissionFollowerDictionary.Remove(follower.GarrFollowerID);
		}
		PersistentFollowerData.instance.m_preMissionFollowerDictionary.Add(follower.GarrFollowerID, follower);
	}
}

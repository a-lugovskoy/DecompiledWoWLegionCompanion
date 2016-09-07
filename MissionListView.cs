using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using WowJamMessages;
using WowStaticData;

public class MissionListView : MonoBehaviour
{
	private class MissionLevelComparer : IComparer<JamGarrisonMobileMission>
	{
		public int Compare(JamGarrisonMobileMission m1, JamGarrisonMobileMission m2)
		{
			GarrMissionRec record = StaticDB.garrMissionDB.GetRecord(m1.MissionRecID);
			GarrMissionRec record2 = StaticDB.garrMissionDB.GetRecord(m2.MissionRecID);
			if (record == null || record2 == null)
			{
				return 0;
			}
			return record2.TargetLevel - record.TargetLevel;
		}
	}

	public class MissionTimeComparer : IComparer<JamGarrisonMobileMission>
	{
		public int Compare(JamGarrisonMobileMission m1, JamGarrisonMobileMission m2)
		{
			long num = GarrisonStatus.CurrentTime() - m1.StartTime;
			long num2 = m1.MissionDuration - num;
			long num3 = GarrisonStatus.CurrentTime() - m2.StartTime;
			long num4 = m2.MissionDuration - num3;
			return (int)(num2 - num4);
		}
	}

	private PersistentMissionData missionData;

	public GameObject missionListViewContents;

	public GameObject missionListItemPrefab;

	public GameObject collectLootListItemPrefab;

	public bool isInProgressMissionList;

	public GameObject missionRewardDisplayPrefab;

	private void Start()
	{
	}

	private void Update()
	{
	}

	private void InitMissionList()
	{
		RectTransform[] componentsInChildren = this.missionListViewContents.GetComponentsInChildren<RectTransform>(true);
		for (int i = 0; i < componentsInChildren.Length; i++)
		{
			if (componentsInChildren[i] != null && componentsInChildren[i] != this.missionListViewContents.get_transform())
			{
				Object.DestroyImmediate(componentsInChildren[i].get_gameObject());
			}
		}
		List<JamGarrisonMobileMission> list = Enumerable.ToList<JamGarrisonMobileMission>(Enumerable.OfType<JamGarrisonMobileMission>(PersistentMissionData.missionDictionary.get_Values()));
		if (this.isInProgressMissionList)
		{
			list.Sort(new MissionListView.MissionTimeComparer());
		}
		else
		{
			list.Sort(new MissionListView.MissionLevelComparer());
		}
		using (List<JamGarrisonMobileMission>.Enumerator enumerator = list.GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				JamGarrisonMobileMission current = enumerator.get_Current();
				GarrMissionRec record = StaticDB.garrMissionDB.GetRecord(current.MissionRecID);
				if (record != null)
				{
					if (record.GarrFollowerTypeID == 4u)
					{
						if (this.isInProgressMissionList)
						{
							if (current.MissionState == 0)
							{
								continue;
							}
							if (current.MissionState == 1)
							{
								long num = GarrisonStatus.CurrentTime() - current.StartTime;
								long num2 = current.MissionDuration - num;
								if (num2 <= 0L)
								{
									continue;
								}
							}
						}
						if (this.isInProgressMissionList || current.MissionState == 0)
						{
							GameObject gameObject = Object.Instantiate<GameObject>(this.missionListItemPrefab);
							gameObject.get_transform().SetParent(this.missionListViewContents.get_transform(), false);
							MissionListItem component = gameObject.GetComponent<MissionListItem>();
							component.Init(record.ID);
						}
					}
				}
			}
		}
	}

	public void OnUIRefresh()
	{
		this.InitMissionList();
	}
}

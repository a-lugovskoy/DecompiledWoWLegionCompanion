using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using WowJamMessages;
using WowStatConstants;
using WowStaticData;

public class FollowerListView : MonoBehaviour
{
	private class FollowerComparer : IComparer<KeyValuePair<int, JamGarrisonFollower>>
	{
		public MissionDetailView m_missionDetailViewForComparer;

		private bool HasUsefulAbility(JamGarrisonFollower follower)
		{
			bool hasUsefulAbility = false;
			if (this.m_missionDetailViewForComparer == null)
			{
				return false;
			}
			MissionMechanic[] mechanics = this.m_missionDetailViewForComparer.get_gameObject().GetComponentsInChildren<MissionMechanic>(true);
			if (mechanics == null)
			{
				return false;
			}
			for (int i = 0; i < follower.AbilityID.Length; i++)
			{
				GarrAbilityRec record = StaticDB.garrAbilityDB.GetRecord(follower.AbilityID[i]);
				if (record != null)
				{
					if ((record.Flags & 1u) == 0u)
					{
						StaticDB.garrAbilityEffectDB.EnumRecordsByParentID(record.ID, delegate(GarrAbilityEffectRec garrAbilityEffectRec)
						{
							if (garrAbilityEffectRec.GarrMechanicTypeID == 0u)
							{
								return true;
							}
							if (garrAbilityEffectRec.AbilityAction != 0u)
							{
								return true;
							}
							GarrMechanicTypeRec record2 = StaticDB.garrMechanicTypeDB.GetRecord((int)garrAbilityEffectRec.GarrMechanicTypeID);
							if (record2 == null)
							{
								return true;
							}
							bool flag = false;
							for (int j = 0; j < mechanics.Length; j++)
							{
								if (mechanics[j].m_missionMechanicTypeID == record2.ID)
								{
									flag = true;
									break;
								}
							}
							if (!flag)
							{
								return true;
							}
							hasUsefulAbility = true;
							return false;
						});
						if (hasUsefulAbility)
						{
							break;
						}
					}
				}
			}
			return hasUsefulAbility;
		}

		public int Compare(KeyValuePair<int, JamGarrisonFollower> follower1, KeyValuePair<int, JamGarrisonFollower> follower2)
		{
			JamGarrisonFollower value = follower1.get_Value();
			JamGarrisonFollower value2 = follower2.get_Value();
			FollowerStatus followerStatus = GeneralHelpers.GetFollowerStatus(value);
			FollowerStatus followerStatus2 = GeneralHelpers.GetFollowerStatus(value2);
			if (followerStatus != followerStatus2)
			{
				return followerStatus - followerStatus2;
			}
			bool flag = this.HasUsefulAbility(value);
			bool flag2 = this.HasUsefulAbility(value2);
			if (flag != flag2)
			{
				return (!flag) ? 1 : -1;
			}
			int num = (value.ItemLevelArmor + value.ItemLevelWeapon) / 2;
			int num2 = (value2.ItemLevelArmor + value2.ItemLevelWeapon) / 2;
			if (num2 != num)
			{
				return num2 - num;
			}
			if (value.Quality != value2.Quality)
			{
				return value2.Quality - value.Quality;
			}
			bool flag3 = (value.Flags & 8) != 0;
			bool flag4 = (value2.Flags & 8) != 0;
			if (flag3 != flag4)
			{
				return (!flag4) ? 1 : -1;
			}
			return 0;
		}
	}

	private List<KeyValuePair<int, JamGarrisonFollower>> m_sortedFollowerList;

	public GameObject m_followerListViewContents;

	public GameObject m_followerListItemPrefab;

	public MissionDetailView m_missionDetailView;

	public bool m_isCombatAllyList;

	public bool m_usedForMissionList;

	private void Start()
	{
		this.InitFollowerList();
		Main expr_0B = Main.instance;
		expr_0B.GarrisonDataResetFinishedAction = (Action)Delegate.Combine(expr_0B.GarrisonDataResetFinishedAction, new Action(this.InitFollowerList));
	}

	private void OnEnable()
	{
		if (this.m_usedForMissionList)
		{
			if (AdventureMapPanel.instance != null)
			{
				AdventureMapPanel expr_20 = AdventureMapPanel.instance;
				expr_20.MissionSelectedFromListAction = (Action<int>)Delegate.Combine(expr_20.MissionSelectedFromListAction, new Action<int>(this.HandleMissionChanged));
			}
		}
		else if (AdventureMapPanel.instance != null)
		{
			AdventureMapPanel expr_5B = AdventureMapPanel.instance;
			expr_5B.MissionMapSelectionChangedAction = (Action<int>)Delegate.Combine(expr_5B.MissionMapSelectionChangedAction, new Action<int>(this.HandleMissionChanged));
		}
	}

	private void OnDisable()
	{
		if (this.m_usedForMissionList)
		{
			if (AdventureMapPanel.instance != null)
			{
				AdventureMapPanel expr_20 = AdventureMapPanel.instance;
				expr_20.MissionSelectedFromListAction = (Action<int>)Delegate.Remove(expr_20.MissionSelectedFromListAction, new Action<int>(this.HandleMissionChanged));
			}
		}
		else if (AdventureMapPanel.instance != null)
		{
			AdventureMapPanel expr_5B = AdventureMapPanel.instance;
			expr_5B.MissionMapSelectionChangedAction = (Action<int>)Delegate.Remove(expr_5B.MissionMapSelectionChangedAction, new Action<int>(this.HandleMissionChanged));
		}
	}

	private void SyncVisibleListOrderToSortedFollowerList()
	{
		FollowerListItem[] componentsInChildren = this.m_followerListViewContents.GetComponentsInChildren<FollowerListItem>(true);
		for (int i = 0; i < this.m_sortedFollowerList.get_Count(); i++)
		{
			FollowerListItem[] array = componentsInChildren;
			for (int j = 0; j < array.Length; j++)
			{
				FollowerListItem followerListItem = array[j];
				if (followerListItem.m_followerID == this.m_sortedFollowerList.get_Item(i).get_Value().GarrFollowerID)
				{
					followerListItem.get_transform().SetSiblingIndex(i);
					break;
				}
			}
		}
	}

	private void HandleMissionChanged(int garrMissionID)
	{
		if (garrMissionID == 0)
		{
			return;
		}
		this.SortFollowerListData();
		this.SyncVisibleListOrderToSortedFollowerList();
		this.UpdateUsefulAbilitiesDisplay();
		this.RemoveAllFromParty();
		this.UpdateAllAvailabilityStatus();
	}

	private FollowerListItem InsertFollowerIntoListView(JamGarrisonFollower follower)
	{
		GarrFollowerRec record = StaticDB.garrFollowerDB.GetRecord(follower.GarrFollowerID);
		if (record == null)
		{
			return null;
		}
		if (record.GarrFollowerTypeID != 4u)
		{
			return null;
		}
		if (this.m_isCombatAllyList)
		{
			bool flag = (follower.Flags & 8) != 0;
			FollowerStatus followerStatus = GeneralHelpers.GetFollowerStatus(follower);
			if (flag || follower.ZoneSupportSpellID <= 0 || followerStatus == FollowerStatus.inactive || followerStatus == FollowerStatus.fatigued || followerStatus == FollowerStatus.inBuilding)
			{
				return null;
			}
		}
		GameObject gameObject = Object.Instantiate<GameObject>(this.m_followerListItemPrefab);
		gameObject.get_transform().SetParent(this.m_followerListViewContents.get_transform());
		gameObject.get_transform().set_localScale(Vector3.get_one());
		gameObject.get_transform().set_localPosition(Vector3.get_zero());
		FollowerListItem component = gameObject.GetComponent<FollowerListItem>();
		component.SetFollower(follower);
		AutoHide autoHide = gameObject.AddComponent<AutoHide>();
		autoHide.m_clipRT = base.get_gameObject().GetComponent<RectTransform>();
		return component;
	}

	private void SortFollowerListData()
	{
		this.m_sortedFollowerList = Enumerable.ToList<KeyValuePair<int, JamGarrisonFollower>>(PersistentFollowerData.followerDictionary);
		FollowerListView.FollowerComparer followerComparer = new FollowerListView.FollowerComparer();
		followerComparer.m_missionDetailViewForComparer = this.m_missionDetailView;
		this.m_sortedFollowerList.Sort(followerComparer);
	}

	private void InitFollowerList()
	{
		FollowerListItem[] componentsInChildren = this.m_followerListViewContents.GetComponentsInChildren<FollowerListItem>(true);
		FollowerListItem[] array = componentsInChildren;
		for (int i = 0; i < array.Length; i++)
		{
			FollowerListItem followerListItem = array[i];
			if (!PersistentFollowerData.followerDictionary.ContainsKey(followerListItem.m_followerID))
			{
				followerListItem.get_gameObject().SetActive(false);
				followerListItem.get_transform().SetParent(Main.instance.get_transform());
			}
			else
			{
				JamGarrisonFollower jamGarrisonFollower = PersistentFollowerData.followerDictionary.get_Item(followerListItem.m_followerID);
				bool flag = (jamGarrisonFollower.Flags & 8) != 0;
				if (flag && jamGarrisonFollower.Durability <= 0)
				{
					followerListItem.get_gameObject().SetActive(false);
					followerListItem.get_transform().SetParent(Main.instance.get_transform());
				}
				else
				{
					followerListItem.SetFollower(jamGarrisonFollower);
				}
			}
		}
		this.m_followerListViewContents.get_transform().set_localPosition(new Vector3(this.m_followerListViewContents.get_transform().get_localPosition().x, 0f, this.m_followerListViewContents.get_transform().get_localPosition().z));
		this.SortFollowerListData();
		componentsInChildren = this.m_followerListViewContents.GetComponentsInChildren<FollowerListItem>(true);
		using (List<KeyValuePair<int, JamGarrisonFollower>>.Enumerator enumerator = this.m_sortedFollowerList.GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				KeyValuePair<int, JamGarrisonFollower> current = enumerator.get_Current();
				bool flag2 = false;
				FollowerListItem[] array2 = componentsInChildren;
				for (int j = 0; j < array2.Length; j++)
				{
					FollowerListItem followerListItem2 = array2[j];
					if (followerListItem2.m_followerID == current.get_Value().GarrFollowerID)
					{
						flag2 = true;
						break;
					}
				}
				if (!flag2)
				{
					bool flag3 = (current.get_Value().Flags & 8) != 0;
					if (!flag3 || current.get_Value().Durability > 0)
					{
						this.InsertFollowerIntoListView(current.get_Value());
					}
				}
			}
		}
	}

	public void UpdateUsefulAbilitiesDisplay()
	{
		FollowerListItem[] componentsInChildren = this.m_followerListViewContents.GetComponentsInChildren<FollowerListItem>(true);
		for (int i = 0; i < componentsInChildren.Length; i++)
		{
			componentsInChildren[i].UpdateUsefulAbilitiesDisplay((!this.m_usedForMissionList) ? AdventureMapPanel.instance.GetCurrentMapMission() : AdventureMapPanel.instance.GetCurrentListMission());
		}
	}

	public void RemoveAllFromParty()
	{
		FollowerListItem[] componentsInChildren = this.m_followerListViewContents.GetComponentsInChildren<FollowerListItem>(true);
		for (int i = 0; i < componentsInChildren.Length; i++)
		{
			componentsInChildren[i].RemoveFromParty();
		}
	}

	private void UpdateAllAvailabilityStatus()
	{
		FollowerListItem[] componentsInChildren = this.m_followerListViewContents.GetComponentsInChildren<FollowerListItem>(true);
		for (int i = 0; i < componentsInChildren.Length; i++)
		{
			componentsInChildren[i].SetAvailabilityStatus(PersistentFollowerData.followerDictionary.get_Item(componentsInChildren[i].m_followerID));
		}
	}
}

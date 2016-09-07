using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using WowJamMessages;
using WowStatConstants;
using WowStaticData;

public class OrderHallFollowersPanel : MonoBehaviour
{
	private class FollowerComparer : IComparer<KeyValuePair<int, JamGarrisonFollower>>
	{
		public int Compare(KeyValuePair<int, JamGarrisonFollower> follower1, KeyValuePair<int, JamGarrisonFollower> follower2)
		{
			JamGarrisonFollower value = follower1.get_Value();
			JamGarrisonFollower value2 = follower2.get_Value();
			FollowerStatus followerStatus = GeneralHelpers.GetFollowerStatus(value);
			FollowerStatus followerStatus2 = GeneralHelpers.GetFollowerStatus(value2);
			bool flag = (value.Flags & 8) != 0;
			bool flag2 = (value2.Flags & 8) != 0;
			bool flag3 = !flag && followerStatus != FollowerStatus.inactive;
			bool flag4 = !flag2 && followerStatus2 != FollowerStatus.inactive;
			if (flag3 != flag4)
			{
				return (!flag3) ? 1 : -1;
			}
			if (followerStatus != followerStatus2)
			{
				return followerStatus - followerStatus2;
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
			return 0;
		}
	}

	public RectTransform m_parentViewRT;

	public RectTransform m_panelViewRT;

	public FollowerListItem m_followerDetailListItemPrefab;

	public GameObject m_followerDetailListContent;

	public FollowerListHeader m_followerListHeaderPrefab;

	public ScrollRect m_listScrollRect;

	public static OrderHallFollowersPanel instance;

	public Action<int> FollowerDetailListItemSelectedAction;

	private Vector2 m_multiPanelViewSizeDelta;

	private List<KeyValuePair<int, JamGarrisonFollower>> m_sortedFollowerList;

	private float m_scrollListToOffset;

	private FollowerListHeader m_championsHeader;

	private FollowerListHeader m_troopsHeader;

	private FollowerListHeader m_inactiveHeader;

	private void Awake()
	{
		OrderHallFollowersPanel.instance = this;
	}

	private void Start()
	{
		this.InitFollowerList();
		Main expr_0B = Main.instance;
		expr_0B.GarrisonDataResetFinishedAction = (Action)Delegate.Combine(expr_0B.GarrisonDataResetFinishedAction, new Action(this.InitFollowerList));
		Main expr_31 = Main.instance;
		expr_31.FollowerDataChangedAction = (Action)Delegate.Combine(expr_31.FollowerDataChangedAction, new Action(this.InitFollowerList));
	}

	private void Update()
	{
		if (this.m_panelViewRT.get_sizeDelta().x != this.m_parentViewRT.get_rect().get_width())
		{
			this.m_multiPanelViewSizeDelta = this.m_panelViewRT.get_sizeDelta();
			this.m_multiPanelViewSizeDelta.x = this.m_parentViewRT.get_rect().get_width();
			this.m_panelViewRT.set_sizeDelta(this.m_multiPanelViewSizeDelta);
		}
	}

	private void SortFollowerListData()
	{
		this.m_sortedFollowerList = Enumerable.ToList<KeyValuePair<int, JamGarrisonFollower>>(PersistentFollowerData.followerDictionary);
		OrderHallFollowersPanel.FollowerComparer followerComparer = new OrderHallFollowersPanel.FollowerComparer();
		this.m_sortedFollowerList.Sort(followerComparer);
	}

	private void InsertFollowerIntoListView(JamGarrisonFollower follower, FollowerCategory followerCategory)
	{
		GarrFollowerRec record = StaticDB.garrFollowerDB.GetRecord(follower.GarrFollowerID);
		if (record == null)
		{
			return;
		}
		if (record.GarrFollowerTypeID != 4u)
		{
			return;
		}
		bool flag = (follower.Flags & 8) != 0;
		bool flag2 = !flag;
		FollowerStatus followerStatus = GeneralHelpers.GetFollowerStatus(follower);
		switch (followerCategory)
		{
		case FollowerCategory.ActiveChampion:
			if (!flag2 || followerStatus == FollowerStatus.inactive)
			{
				return;
			}
			break;
		case FollowerCategory.InactiveChampion:
			if (!flag2 || followerStatus != FollowerStatus.inactive)
			{
				return;
			}
			break;
		case FollowerCategory.Troop:
			if (!flag || follower.Durability <= 0)
			{
				return;
			}
			break;
		default:
			return;
		}
		FollowerListItem followerListItem = Object.Instantiate<FollowerListItem>(this.m_followerDetailListItemPrefab);
		followerListItem.get_transform().SetParent(this.m_followerDetailListContent.get_transform(), false);
		followerListItem.SetFollower(follower);
		AutoHide autoHide = followerListItem.m_followerDetailView.get_gameObject().AddComponent<AutoHide>();
		autoHide.m_clipRT = this.m_panelViewRT;
		AutoHide autoHide2 = followerListItem.m_listItemArea.get_gameObject().AddComponent<AutoHide>();
		autoHide2.m_clipRT = this.m_panelViewRT;
	}

	private void SyncVisibleListOrderToSortedFollowerList()
	{
		FollowerListItem[] componentsInChildren = this.m_followerDetailListContent.GetComponentsInChildren<FollowerListItem>(true);
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

	private void InitFollowerList()
	{
		FollowerListItem[] componentsInChildren = this.m_followerDetailListContent.GetComponentsInChildren<FollowerListItem>(true);
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
		this.SortFollowerListData();
		if (this.m_championsHeader == null)
		{
			this.m_championsHeader = Object.Instantiate<FollowerListHeader>(this.m_followerListHeaderPrefab);
		}
		this.m_championsHeader.get_transform().SetParent(this.m_followerDetailListContent.get_transform(), false);
		this.m_championsHeader.m_title.set_font(GeneralHelpers.LoadStandardFont());
		this.m_championsHeader.m_title.set_text(StaticDB.GetString("CHAMPIONS", null) + ": ");
		int numActiveChampions = GeneralHelpers.GetNumActiveChampions();
		int maxActiveChampions = GeneralHelpers.GetMaxActiveChampions();
		if (numActiveChampions <= maxActiveChampions)
		{
			this.m_championsHeader.m_count.set_text(string.Concat(new object[]
			{
				string.Empty,
				numActiveChampions,
				"/",
				maxActiveChampions
			}));
		}
		else
		{
			this.m_championsHeader.m_count.set_text(string.Concat(new object[]
			{
				"<color=#ff0000ff>",
				numActiveChampions,
				"/",
				maxActiveChampions,
				"</color>"
			}));
		}
		AutoHide autoHide = this.m_championsHeader.get_gameObject().AddComponent<AutoHide>();
		autoHide.m_clipRT = this.m_panelViewRT;
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
					this.InsertFollowerIntoListView(current.get_Value(), FollowerCategory.ActiveChampion);
				}
			}
		}
		int numTroops = GeneralHelpers.GetNumTroops();
		if (this.m_troopsHeader == null)
		{
			this.m_troopsHeader = Object.Instantiate<FollowerListHeader>(this.m_followerListHeaderPrefab);
		}
		this.m_troopsHeader.get_transform().SetParent(this.m_followerDetailListContent.get_transform(), false);
		this.m_troopsHeader.m_title.set_font(GeneralHelpers.LoadStandardFont());
		this.m_troopsHeader.m_title.set_text(StaticDB.GetString("TROOPS", null) + ": ");
		this.m_troopsHeader.m_count.set_font(GeneralHelpers.LoadStandardFont());
		this.m_troopsHeader.m_count.set_text(string.Empty + numTroops);
		autoHide = this.m_troopsHeader.get_gameObject().AddComponent<AutoHide>();
		autoHide.m_clipRT = this.m_panelViewRT;
		using (List<KeyValuePair<int, JamGarrisonFollower>>.Enumerator enumerator2 = this.m_sortedFollowerList.GetEnumerator())
		{
			while (enumerator2.MoveNext())
			{
				KeyValuePair<int, JamGarrisonFollower> current2 = enumerator2.get_Current();
				bool flag3 = false;
				FollowerListItem[] array3 = componentsInChildren;
				for (int k = 0; k < array3.Length; k++)
				{
					FollowerListItem followerListItem3 = array3[k];
					if (followerListItem3.m_followerID == current2.get_Value().GarrFollowerID)
					{
						flag3 = true;
						break;
					}
				}
				if (!flag3)
				{
					this.InsertFollowerIntoListView(current2.get_Value(), FollowerCategory.Troop);
				}
			}
		}
		int numInactiveChampions = GeneralHelpers.GetNumInactiveChampions();
		if (this.m_inactiveHeader == null)
		{
			this.m_inactiveHeader = Object.Instantiate<FollowerListHeader>(this.m_followerListHeaderPrefab);
		}
		this.m_inactiveHeader.get_transform().SetParent(this.m_followerDetailListContent.get_transform(), false);
		this.m_inactiveHeader.m_title.set_font(GeneralHelpers.LoadStandardFont());
		this.m_inactiveHeader.m_title.set_text(StaticDB.GetString("INACTIVE", null) + ": ");
		this.m_inactiveHeader.m_count.set_font(GeneralHelpers.LoadStandardFont());
		this.m_inactiveHeader.m_count.set_text(string.Empty + numInactiveChampions);
		autoHide = this.m_inactiveHeader.get_gameObject().AddComponent<AutoHide>();
		autoHide.m_clipRT = this.m_panelViewRT;
		using (List<KeyValuePair<int, JamGarrisonFollower>>.Enumerator enumerator3 = this.m_sortedFollowerList.GetEnumerator())
		{
			while (enumerator3.MoveNext())
			{
				KeyValuePair<int, JamGarrisonFollower> current3 = enumerator3.get_Current();
				bool flag4 = false;
				FollowerListItem[] array4 = componentsInChildren;
				for (int l = 0; l < array4.Length; l++)
				{
					FollowerListItem followerListItem4 = array4[l];
					if (followerListItem4.m_followerID == current3.get_Value().GarrFollowerID)
					{
						flag4 = true;
						break;
					}
				}
				if (!flag4)
				{
					this.InsertFollowerIntoListView(current3.get_Value(), FollowerCategory.InactiveChampion);
				}
			}
		}
		this.SyncVisibleListOrderToSortedFollowerList();
		this.m_championsHeader.get_gameObject().SetActive(numActiveChampions > 0);
		this.m_troopsHeader.get_gameObject().SetActive(numTroops > 0);
		this.m_inactiveHeader.get_gameObject().SetActive(numInactiveChampions > 0);
		this.m_championsHeader.get_transform().SetSiblingIndex(0);
		this.m_troopsHeader.get_transform().SetSiblingIndex(numActiveChampions + 1);
		this.m_inactiveHeader.get_transform().SetSiblingIndex(numActiveChampions + numTroops + 2);
	}

	private void ScrollListTo_Update(float offsetY)
	{
		Vector3 localPosition = this.m_followerDetailListContent.get_transform().get_localPosition();
		localPosition.y = offsetY;
		this.m_followerDetailListContent.get_transform().set_localPosition(localPosition);
	}

	private void ScrollListTo_Complete()
	{
		Vector3 localPosition = this.m_followerDetailListContent.get_transform().get_localPosition();
		localPosition.y = this.m_scrollListToOffset;
		this.m_followerDetailListContent.get_transform().set_localPosition(localPosition);
		this.m_listScrollRect.set_enabled(true);
	}

	public void ScrollListTo(float offsetY)
	{
		this.m_scrollListToOffset = offsetY;
		this.m_listScrollRect.set_enabled(false);
		iTween.StopByName(base.get_gameObject(), "ScrollListTo");
		iTween.ValueTo(base.get_gameObject(), iTween.Hash(new object[]
		{
			"name",
			"ScrollListTo",
			"from",
			this.m_followerDetailListContent.get_transform().get_localPosition().y,
			"to",
			offsetY,
			"time",
			0.25f,
			"easetype",
			iTween.EaseType.easeOutCubic,
			"onupdate",
			"ScrollListTo_Update",
			"oncomplete",
			"ScrollListTo_Complete"
		}));
	}

	public void FollowerDetailListItemSelected(int garrFollowerID)
	{
		if (this.FollowerDetailListItemSelectedAction != null)
		{
			this.FollowerDetailListItemSelectedAction.Invoke(garrFollowerID);
		}
	}
}

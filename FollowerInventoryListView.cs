using System;
using System.Collections;
using UnityEngine;
using WowJamMessages.MobileClientJSON;

public class FollowerInventoryListView : MonoBehaviour
{
	public GameObject m_headerPrefab;

	public GameObject m_followerInventoryListItemPrefab;

	public GameObject m_equipmentInventoryContent;

	private FollowerDetailView m_followerDetailView;

	private int m_abilityToReplace;

	private void OnEnable()
	{
		Main expr_05 = Main.instance;
		expr_05.ArmamentInventoryChangedAction = (Action)Delegate.Combine(expr_05.ArmamentInventoryChangedAction, new Action(this.HandleInventoryChanged));
		Main expr_2B = Main.instance;
		expr_2B.EquipmentInventoryChangedAction = (Action)Delegate.Combine(expr_2B.EquipmentInventoryChangedAction, new Action(this.HandleInventoryChanged));
	}

	private void OnDisable()
	{
		Main expr_05 = Main.instance;
		expr_05.ArmamentInventoryChangedAction = (Action)Delegate.Remove(expr_05.ArmamentInventoryChangedAction, new Action(this.HandleInventoryChanged));
		Main expr_2B = Main.instance;
		expr_2B.EquipmentInventoryChangedAction = (Action)Delegate.Remove(expr_2B.EquipmentInventoryChangedAction, new Action(this.HandleInventoryChanged));
	}

	private void HandleInventoryChanged()
	{
		if (this.m_followerDetailView != null)
		{
			this.Init(this.m_followerDetailView, this.m_abilityToReplace);
		}
	}

	public void Init(FollowerDetailView followerDetailView, int abilityToReplace)
	{
		this.m_followerDetailView = followerDetailView;
		this.m_abilityToReplace = abilityToReplace;
		FollowerInventoryListItem[] componentsInChildren = this.m_equipmentInventoryContent.GetComponentsInChildren<FollowerInventoryListItem>(true);
		FollowerInventoryListItem[] array = componentsInChildren;
		for (int i = 0; i < array.Length; i++)
		{
			FollowerInventoryListItem followerInventoryListItem = array[i];
			Object.DestroyImmediate(followerInventoryListItem.get_gameObject());
		}
		int num = 0;
		IEnumerator enumerator = PersistentEquipmentData.equipmentDictionary.get_Values().GetEnumerator();
		try
		{
			while (enumerator.MoveNext())
			{
				MobileFollowerEquipment item = (MobileFollowerEquipment)enumerator.get_Current();
				if (num == 0)
				{
					GameObject gameObject = Object.Instantiate<GameObject>(this.m_headerPrefab);
					gameObject.get_transform().SetParent(this.m_equipmentInventoryContent.get_transform(), false);
					FollowerInventoryListItem component = gameObject.GetComponent<FollowerInventoryListItem>();
					component.SetHeaderText("Equipment");
				}
				GameObject gameObject2 = Object.Instantiate<GameObject>(this.m_followerInventoryListItemPrefab);
				gameObject2.get_transform().SetParent(this.m_equipmentInventoryContent.get_transform(), false);
				FollowerInventoryListItem component2 = gameObject2.GetComponent<FollowerInventoryListItem>();
				component2.SetEquipment(item, followerDetailView, abilityToReplace);
				num++;
			}
		}
		finally
		{
			IDisposable disposable = enumerator as IDisposable;
			if (disposable != null)
			{
				disposable.Dispose();
			}
		}
		if (num == 0)
		{
			GameObject gameObject3 = Object.Instantiate<GameObject>(this.m_headerPrefab);
			gameObject3.get_transform().SetParent(this.m_equipmentInventoryContent.get_transform(), false);
			FollowerInventoryListItem component3 = gameObject3.GetComponent<FollowerInventoryListItem>();
			component3.SetHeaderText(StaticDB.GetString("NO_EQUIPMENT", null));
		}
		int num2 = 0;
		IEnumerator enumerator2 = PersistentArmamentData.armamentDictionary.get_Values().GetEnumerator();
		try
		{
			while (enumerator2.MoveNext())
			{
				MobileFollowerArmament item2 = (MobileFollowerArmament)enumerator2.get_Current();
				if (num2 == 0)
				{
					GameObject gameObject4 = Object.Instantiate<GameObject>(this.m_headerPrefab);
					gameObject4.get_transform().SetParent(this.m_equipmentInventoryContent.get_transform(), false);
					FollowerInventoryListItem component4 = gameObject4.GetComponent<FollowerInventoryListItem>();
					component4.SetHeaderText("Armaments");
				}
				GameObject gameObject5 = Object.Instantiate<GameObject>(this.m_followerInventoryListItemPrefab);
				gameObject5.get_transform().SetParent(this.m_equipmentInventoryContent.get_transform(), false);
				FollowerInventoryListItem component5 = gameObject5.GetComponent<FollowerInventoryListItem>();
				component5.SetArmament(item2, followerDetailView);
				num2++;
			}
		}
		finally
		{
			IDisposable disposable2 = enumerator2 as IDisposable;
			if (disposable2 != null)
			{
				disposable2.Dispose();
			}
		}
		if (num == 0)
		{
			GameObject gameObject6 = Object.Instantiate<GameObject>(this.m_headerPrefab);
			gameObject6.get_transform().SetParent(this.m_equipmentInventoryContent.get_transform(), false);
			FollowerInventoryListItem component6 = gameObject6.GetComponent<FollowerInventoryListItem>();
			component6.SetHeaderText(StaticDB.GetString("NO_ARMAMENTS", null));
		}
	}
}

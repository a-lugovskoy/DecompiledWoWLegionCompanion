using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using WowJamMessages.MobileClientJSON;
using WowJamMessages.MobilePlayerJSON;
using WowStatConstants;

public class ArmamentDialog : MonoBehaviour
{
	public FollowerInventoryListItem m_armamentListItemPrefab;

	public GameObject m_armamentListContent;

	public Text m_titleText;

	public Text m_emptyMessage;

	private FollowerDetailView m_currentFollowerDetailView;

	private void Awake()
	{
		this.m_titleText.set_font(GeneralHelpers.LoadFancyFont());
		this.m_titleText.set_text(StaticDB.GetString("CHAMPION_ENHANCEMENT", null));
		this.m_emptyMessage.set_font(GeneralHelpers.LoadStandardFont());
		this.m_emptyMessage.set_text(StaticDB.GetString("NO_ARMAMENTS2", "You do not have any armaments to equip."));
	}

	public void OnEnable()
	{
		Main.instance.m_backButtonManager.PushBackAction(BackAction.hideAllPopups, null);
		Main.instance.m_UISound.Play_ShowGenericTooltip();
		Main.instance.m_canvasBlurManager.AddBlurRef_MainCanvas();
		Main expr_34 = Main.instance;
		expr_34.ArmamentInventoryChangedAction = (Action)Delegate.Combine(expr_34.ArmamentInventoryChangedAction, new Action(this.HandleArmamentsChanged));
		MobilePlayerFollowerArmamentsRequest mobilePlayerFollowerArmamentsRequest = new MobilePlayerFollowerArmamentsRequest();
		mobilePlayerFollowerArmamentsRequest.GarrFollowerTypeID = 4;
		Login.instance.SendToMobileServer(mobilePlayerFollowerArmamentsRequest);
	}

	private void OnDisable()
	{
		Main.instance.m_backButtonManager.PopBackAction();
		Main.instance.m_canvasBlurManager.RemoveBlurRef_MainCanvas();
		Main expr_24 = Main.instance;
		expr_24.ArmamentInventoryChangedAction = (Action)Delegate.Remove(expr_24.ArmamentInventoryChangedAction, new Action(this.HandleArmamentsChanged));
		this.m_currentFollowerDetailView = null;
	}

	public void Init(FollowerDetailView followerDetailView)
	{
		this.m_currentFollowerDetailView = followerDetailView;
		FollowerInventoryListItem[] componentsInChildren = this.m_armamentListContent.GetComponentsInChildren<FollowerInventoryListItem>(true);
		FollowerInventoryListItem[] array = componentsInChildren;
		for (int i = 0; i < array.Length; i++)
		{
			FollowerInventoryListItem followerInventoryListItem = array[i];
			Object.DestroyImmediate(followerInventoryListItem.get_gameObject());
		}
		bool active = true;
		IEnumerator enumerator = PersistentArmamentData.armamentDictionary.get_Values().GetEnumerator();
		try
		{
			while (enumerator.MoveNext())
			{
				MobileFollowerArmament item = (MobileFollowerArmament)enumerator.get_Current();
				FollowerInventoryListItem followerInventoryListItem2 = Object.Instantiate<FollowerInventoryListItem>(this.m_armamentListItemPrefab);
				followerInventoryListItem2.get_transform().SetParent(this.m_armamentListContent.get_transform(), false);
				followerInventoryListItem2.SetArmament(item, followerDetailView);
				active = false;
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
		this.m_emptyMessage.get_gameObject().SetActive(active);
	}

	private void HandleArmamentsChanged()
	{
		if (this.m_currentFollowerDetailView != null)
		{
			this.Init(this.m_currentFollowerDetailView);
		}
	}
}

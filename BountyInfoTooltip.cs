using System;
using UnityEngine;
using UnityEngine.UI;
using WowJamMessages.MobileClientJSON;
using WowStatConstants;
using WowStaticData;

public class BountyInfoTooltip : MonoBehaviour
{
	public Image m_bountyIcon;

	public Text m_bountyIconInvalidFileDataID;

	public Text m_bountyName;

	public Text m_timeLeft;

	public Text m_bountyDescription;

	public GameObject m_bountyQuestCompleteIconPrefab;

	public GameObject m_bountyQuestAvailableIconPrefab;

	public Transform m_bountyQuestIconArea;

	public Image m_lootIcon;

	public Text m_lootIconInvalidFileDataID;

	public Text m_lootName;

	public Text m_lootDescription;

	public Text m_rewardsLabel;

	private MobileWorldQuestBounty m_bounty;

	public void OnEnable()
	{
		Main.instance.m_UISound.Play_ShowGenericTooltip();
		Main.instance.m_canvasBlurManager.AddBlurRef_MainCanvas();
		this.m_rewardsLabel.set_text(StaticDB.GetString("REWARDS", null));
		Main.instance.m_backButtonManager.PushBackAction(BackAction.hideAllPopups, null);
	}

	private void OnDisable()
	{
		Main.instance.m_canvasBlurManager.RemoveBlurRef_MainCanvas();
		Main.instance.m_backButtonManager.PopBackAction();
	}

	public void SetBounty(MobileWorldQuestBounty bounty)
	{
		this.m_bounty = bounty;
		Sprite sprite = GeneralHelpers.LoadIconAsset(AssetBundleType.Icons, bounty.IconFileDataID);
		if (sprite != null)
		{
			this.m_bountyIconInvalidFileDataID.get_gameObject().SetActive(false);
			this.m_bountyIcon.set_sprite(sprite);
		}
		else
		{
			this.m_bountyIconInvalidFileDataID.get_gameObject().SetActive(true);
			this.m_bountyIconInvalidFileDataID.set_text(string.Empty + bounty.IconFileDataID);
		}
		QuestV2Rec record = StaticDB.questDB.GetRecord(bounty.QuestID);
		if (record != null)
		{
			this.m_bountyName.set_text(record.QuestTitle);
			this.m_bountyDescription.set_text(string.Concat(new object[]
			{
				string.Empty,
				bounty.NumCompleted,
				"/",
				bounty.NumNeeded,
				" ",
				record.LogDescription
			}));
		}
		else
		{
			this.m_bountyName.set_text("Unknown Quest ID " + bounty.QuestID);
			this.m_bountyDescription.set_text("Unknown Quest ID " + bounty.QuestID);
		}
		this.m_timeLeft.set_text(StaticDB.GetString("TIME_REMAINING", null));
		RectTransform[] componentsInChildren = this.m_bountyQuestIconArea.GetComponentsInChildren<RectTransform>(true);
		RectTransform[] array = componentsInChildren;
		for (int i = 0; i < array.Length; i++)
		{
			RectTransform rectTransform = array[i];
			if (rectTransform != null && rectTransform.get_gameObject() != this.m_bountyQuestIconArea.get_gameObject())
			{
				Object.DestroyImmediate(rectTransform.get_gameObject());
			}
		}
		for (int j = 0; j < bounty.NumCompleted; j++)
		{
			GameObject gameObject = Object.Instantiate<GameObject>(this.m_bountyQuestCompleteIconPrefab);
			gameObject.get_transform().SetParent(this.m_bountyQuestIconArea.get_transform(), false);
		}
		for (int k = 0; k < bounty.NumNeeded - bounty.NumCompleted; k++)
		{
			GameObject gameObject2 = Object.Instantiate<GameObject>(this.m_bountyQuestAvailableIconPrefab);
			gameObject2.get_transform().SetParent(this.m_bountyQuestIconArea.get_transform(), false);
		}
		this.UpdateTimeRemaining();
		if (bounty.Item.Length > 0)
		{
			ItemRec record2 = StaticDB.itemDB.GetRecord(bounty.Item[0].RecordID);
			if (record2 != null)
			{
				this.m_lootName.set_text(record2.Display);
				this.m_lootDescription.set_text(GeneralHelpers.GetItemDescription(record2));
				Sprite sprite2 = GeneralHelpers.LoadIconAsset(AssetBundleType.Icons, record2.IconFileDataID);
				if (sprite2 != null)
				{
					this.m_lootIcon.set_sprite(sprite2);
				}
				else if (this.m_lootIconInvalidFileDataID != null)
				{
					this.m_lootIconInvalidFileDataID.get_gameObject().SetActive(true);
					this.m_lootIconInvalidFileDataID.set_text(string.Empty + record2.IconFileDataID);
				}
			}
			else
			{
				this.m_lootName.set_text("Unknown item " + bounty.Item[0].RecordID);
				this.m_lootDescription.set_text("Unknown item " + bounty.Item[0].RecordID);
			}
		}
		else
		{
			this.m_lootName.set_text("ERROR: Loot Not Specified");
			this.m_lootDescription.set_text("ERROR: Loot Not Specified");
		}
	}

	private void UpdateTimeRemaining()
	{
		long num = (long)this.m_bounty.EndTime - GarrisonStatus.CurrentTime();
		num = ((num <= 0L) ? 0L : num);
		Duration duration = new Duration((int)num);
		this.m_timeLeft.set_text(StaticDB.GetString("TIME_REMAINING", null) + " " + duration.DurationString);
	}

	private void Update()
	{
		this.UpdateTimeRemaining();
	}
}

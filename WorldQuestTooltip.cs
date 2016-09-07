using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using WowJamMessages.MobileClientJSON;
using WowStatConstants;
using WowStaticData;

public class WorldQuestTooltip : MonoBehaviour
{
	private const int WORLD_QUEST_TIME_LOW_MINUTES = 75;

	[Header("World Quest Icon Layers")]
	public Image m_dragonFrame;

	public Image m_background;

	public Image m_main;

	public Image m_expiringSoon;

	[Header("World Quest Info")]
	public Text m_worldQuestNameText;

	public Text m_worldQuestTimeText;

	public MissionRewardDisplay m_missionRewardDisplayPrefab;

	public GameObject m_worldQuestObjectiveRoot;

	public GameObject m_worldQuestObjectiveDisplayPrefab;

	[Header("Misc")]
	public RewardInfoPopup m_rewardInfo;

	public Text m_rewardsLabel;

	private int m_questID;

	private long m_endTime;

	private string m_timeLeftString;

	private void Start()
	{
		this.m_rewardsLabel.set_font(GeneralHelpers.LoadStandardFont());
		this.m_rewardsLabel.set_text(StaticDB.GetString("REWARDS", "Rewards"));
		this.m_timeLeftString = StaticDB.GetString("TIME_REMAINING", null);
	}

	public void OnEnable()
	{
		Main.instance.m_canvasBlurManager.AddBlurRef_MainCanvas();
		Main.instance.m_backButtonManager.PushBackAction(BackAction.hideAllPopups, null);
	}

	private void OnDisable()
	{
		Main.instance.m_canvasBlurManager.RemoveBlurRef_MainCanvas();
		Main.instance.m_backButtonManager.PopBackAction();
	}

	private void InitRewardInfoDisplay(MobileWorldQuest worldQuest)
	{
		if (worldQuest.Item != null && Enumerable.Count<MobileWorldQuestReward>(worldQuest.Item) > 0)
		{
			MobileWorldQuestReward[] item = worldQuest.Item;
			int num = 0;
			if (num < item.Length)
			{
				MobileWorldQuestReward mobileWorldQuestReward = item[num];
				Sprite rewardSprite = GeneralHelpers.LoadIconAsset(AssetBundleType.Icons, mobileWorldQuestReward.FileDataID);
				this.m_rewardInfo.SetReward(MissionRewardDisplay.RewardType.item, mobileWorldQuestReward.RecordID, mobileWorldQuestReward.Quantity, rewardSprite, mobileWorldQuestReward.ItemContext);
			}
		}
		else if (worldQuest.Money > 0)
		{
			Sprite iconSprite = Resources.Load<Sprite>("MiscIcons/INV_Misc_Coin_01");
			this.m_rewardInfo.SetGold(worldQuest.Money / 10000, iconSprite);
		}
		else if (worldQuest.Experience > 0)
		{
			Sprite localizedFollowerXpIcon = GeneralHelpers.GetLocalizedFollowerXpIcon();
			this.m_rewardInfo.SetFollowerXP(worldQuest.Experience, localizedFollowerXpIcon);
		}
		else
		{
			MobileWorldQuestReward[] currency = worldQuest.Currency;
			int num2 = 0;
			if (num2 < currency.Length)
			{
				MobileWorldQuestReward mobileWorldQuestReward2 = currency[num2];
				Sprite iconSprite2 = GeneralHelpers.LoadCurrencyIcon(mobileWorldQuestReward2.RecordID);
				CurrencyTypesRec record = StaticDB.currencyTypesDB.GetRecord(mobileWorldQuestReward2.RecordID);
				int quantity = mobileWorldQuestReward2.Quantity / (((record.Flags & 8u) == 0u) ? 1 : 100);
				this.m_rewardInfo.SetCurrency(mobileWorldQuestReward2.RecordID, quantity, iconSprite2);
			}
		}
	}

	public void SetQuest(int questID)
	{
		this.m_expiringSoon.get_gameObject().SetActive(false);
		this.m_questID = questID;
		Transform[] componentsInChildren = this.m_worldQuestObjectiveRoot.GetComponentsInChildren<Transform>(true);
		Transform[] array = componentsInChildren;
		for (int i = 0; i < array.Length; i++)
		{
			Transform transform = array[i];
			if (transform != null && transform != this.m_worldQuestObjectiveRoot.get_transform())
			{
				Object.DestroyImmediate(transform.get_gameObject());
			}
		}
		MobileWorldQuest mobileWorldQuest = (MobileWorldQuest)WorldQuestData.worldQuestDictionary.get_Item(this.m_questID);
		this.m_worldQuestNameText.set_text(mobileWorldQuest.QuestTitle);
		using (IEnumerator<MobileWorldQuestObjective> enumerator = Enumerable.AsEnumerable<MobileWorldQuestObjective>(mobileWorldQuest.Objective).GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				MobileWorldQuestObjective current = enumerator.get_Current();
				GameObject gameObject = Object.Instantiate<GameObject>(this.m_worldQuestObjectiveDisplayPrefab);
				gameObject.get_transform().SetParent(this.m_worldQuestObjectiveRoot.get_transform(), false);
				Text component = gameObject.GetComponent<Text>();
				component.set_text("-" + current.Text);
			}
		}
		this.InitRewardInfoDisplay(mobileWorldQuest);
		this.m_endTime = (long)(mobileWorldQuest.EndTime - 900);
		QuestInfoRec record = StaticDB.questInfoDB.GetRecord(mobileWorldQuest.QuestInfoID);
		if (record == null)
		{
			return;
		}
		bool active = (record.Modifiers & 2) != 0;
		this.m_dragonFrame.get_gameObject().SetActive(active);
		this.m_background.set_sprite(Resources.Load<Sprite>("NewWorldQuest/Mobile-NormalQuest"));
		bool flag = (record.Modifiers & 1) != 0;
		if (flag && record.Type != 3)
		{
			this.m_background.set_sprite(Resources.Load<Sprite>("NewWorldQuest/Mobile-RareQuest"));
		}
		bool flag2 = (record.Modifiers & 4) != 0;
		if (flag2 && record.Type != 3)
		{
			this.m_background.set_sprite(Resources.Load<Sprite>("NewWorldQuest/Mobile-EpicQuest"));
		}
		int uITextureAtlasMemberID;
		string text;
		switch (record.Type)
		{
		case 1:
		{
			int profession = record.Profession;
			switch (profession)
			{
			case 182:
				uITextureAtlasMemberID = TextureAtlas.GetUITextureAtlasMemberID("worldquest-icon-herbalism");
				text = "Mobile-Herbalism";
				goto IL_46E;
			case 183:
			case 184:
				IL_259:
				if (profession == 164)
				{
					uITextureAtlasMemberID = TextureAtlas.GetUITextureAtlasMemberID("worldquest-icon-blacksmithing");
					text = "Mobile-Blacksmithing";
					goto IL_46E;
				}
				if (profession == 165)
				{
					uITextureAtlasMemberID = TextureAtlas.GetUITextureAtlasMemberID("worldquest-icon-leatherworking");
					text = "Mobile-Leatherworking";
					goto IL_46E;
				}
				if (profession == 129)
				{
					uITextureAtlasMemberID = TextureAtlas.GetUITextureAtlasMemberID("worldquest-icon-firstaid");
					text = "Mobile-FirstAid";
					goto IL_46E;
				}
				if (profession == 171)
				{
					uITextureAtlasMemberID = TextureAtlas.GetUITextureAtlasMemberID("worldquest-icon-alchemy");
					text = "Mobile-Alchemy";
					goto IL_46E;
				}
				if (profession == 197)
				{
					uITextureAtlasMemberID = TextureAtlas.GetUITextureAtlasMemberID("worldquest-icon-tailoring");
					text = "Mobile-Tailoring";
					goto IL_46E;
				}
				if (profession == 202)
				{
					uITextureAtlasMemberID = TextureAtlas.GetUITextureAtlasMemberID("worldquest-icon-engineering");
					text = "Mobile-Engineering";
					goto IL_46E;
				}
				if (profession == 333)
				{
					uITextureAtlasMemberID = TextureAtlas.GetUITextureAtlasMemberID("worldquest-icon-enchanting");
					text = "Mobile-Enchanting";
					goto IL_46E;
				}
				if (profession == 356)
				{
					uITextureAtlasMemberID = TextureAtlas.GetUITextureAtlasMemberID("worldquest-icon-fishing");
					text = "Mobile-Fishing";
					goto IL_46E;
				}
				if (profession == 393)
				{
					uITextureAtlasMemberID = TextureAtlas.GetUITextureAtlasMemberID("worldquest-icon-skinning");
					text = "Mobile-Skinning";
					goto IL_46E;
				}
				if (profession == 755)
				{
					uITextureAtlasMemberID = TextureAtlas.GetUITextureAtlasMemberID("worldquest-icon-jewelcrafting");
					text = "Mobile-Jewelcrafting";
					goto IL_46E;
				}
				if (profession == 773)
				{
					uITextureAtlasMemberID = TextureAtlas.GetUITextureAtlasMemberID("worldquest-icon-inscription");
					text = "Mobile-Inscription";
					goto IL_46E;
				}
				if (profession != 794)
				{
					uITextureAtlasMemberID = TextureAtlas.GetUITextureAtlasMemberID("worldquest-questmarker-questbang");
					text = "Mobile-QuestExclamationIcon";
					goto IL_46E;
				}
				uITextureAtlasMemberID = TextureAtlas.GetUITextureAtlasMemberID("worldquest-icon-archaeology");
				text = "Mobile-Archaeology";
				goto IL_46E;
			case 185:
				uITextureAtlasMemberID = TextureAtlas.GetUITextureAtlasMemberID("worldquest-icon-cooking");
				text = "Mobile-Cooking";
				goto IL_46E;
			case 186:
				uITextureAtlasMemberID = TextureAtlas.GetUITextureAtlasMemberID("worldquest-icon-mining");
				text = "Mobile-Mining";
				goto IL_46E;
			}
			goto IL_259;
			IL_46E:
			goto IL_4BB;
		}
		case 3:
			uITextureAtlasMemberID = TextureAtlas.GetUITextureAtlasMemberID("worldquest-icon-pvp-ffa");
			text = "Mobile-PVP";
			goto IL_4BB;
		case 4:
			uITextureAtlasMemberID = TextureAtlas.GetUITextureAtlasMemberID("worldquest-icon-petbattle");
			text = "Mobile-Pets";
			goto IL_4BB;
		}
		uITextureAtlasMemberID = TextureAtlas.GetUITextureAtlasMemberID("worldquest-questmarker-questbang");
		text = "Mobile-QuestExclamationIcon";
		IL_4BB:
		if (text != null)
		{
			this.m_main.set_sprite(Resources.Load<Sprite>("NewWorldQuest/" + text));
		}
		else if (uITextureAtlasMemberID > 0)
		{
			this.m_main.set_sprite(TextureAtlas.instance.GetAtlasSprite(uITextureAtlasMemberID));
			this.m_main.SetNativeSize();
		}
		this.UpdateTimeRemaining();
	}

	private void UpdateTimeRemaining()
	{
		int num = (int)(this.m_endTime - GarrisonStatus.CurrentTime());
		if (num < 0)
		{
			num = 0;
		}
		Duration duration = new Duration(num);
		this.m_worldQuestTimeText.set_text(this.m_timeLeftString + " " + duration.DurationString);
		bool active = num < 4500;
		this.m_expiringSoon.get_gameObject().SetActive(active);
	}

	private void Update()
	{
		this.UpdateTimeRemaining();
	}
}

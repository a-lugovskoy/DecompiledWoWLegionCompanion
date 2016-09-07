using System;
using UnityEngine;
using UnityEngine.UI;
using WowJamMessages.MobileClientJSON;
using WowStatConstants;
using WowStaticData;

public class AdventureMapWorldQuest : MonoBehaviour
{
	private const int WORLD_QUEST_TIME_LOW_MINUTES = 75;

	public Image m_errorImage;

	public Image m_dragonFrame;

	public Image m_background;

	public Image m_main;

	public Image m_highlight;

	public Image m_expiringSoon;

	public int m_areaID;

	public GameObject m_zoomScaleRoot;

	private int m_questID;

	private ITEM_QUALITY m_lootQuality;

	private long m_endTime;

	public bool m_showLootIconInsteadOfMain;

	public int QuestID
	{
		get
		{
			return this.m_questID;
		}
	}

	private void OnEnable()
	{
		AdventureMapPanel expr_05 = AdventureMapPanel.instance;
		expr_05.TestIconSizeChanged = (Action<float>)Delegate.Combine(expr_05.TestIconSizeChanged, new Action<float>(this.OnTestIconSizeChanged));
		PinchZoomContentManager expr_30 = AdventureMapPanel.instance.m_pinchZoomContentManager;
		expr_30.ZoomFactorChanged = (Action)Delegate.Combine(expr_30.ZoomFactorChanged, new Action(this.HandleZoomChanged));
		this.m_showLootIconInsteadOfMain = true;
	}

	private void OnDisable()
	{
		AdventureMapPanel expr_05 = AdventureMapPanel.instance;
		expr_05.TestIconSizeChanged = (Action<float>)Delegate.Remove(expr_05.TestIconSizeChanged, new Action<float>(this.OnTestIconSizeChanged));
		PinchZoomContentManager expr_30 = AdventureMapPanel.instance.m_pinchZoomContentManager;
		expr_30.ZoomFactorChanged = (Action)Delegate.Remove(expr_30.ZoomFactorChanged, new Action(this.HandleZoomChanged));
	}

	private void OnTestIconSizeChanged(float newScale)
	{
		base.get_transform().set_localScale(Vector3.get_one() * newScale);
	}

	private void HandleZoomChanged()
	{
		if (this.m_zoomScaleRoot != null)
		{
			this.m_zoomScaleRoot.get_transform().set_localScale(Vector3.get_one() * AdventureMapPanel.instance.m_pinchZoomContentManager.m_zoomFactor);
		}
	}

	public void SetQuestID(int questID)
	{
		this.m_questID = questID;
		base.get_gameObject().set_name("WorldQuest " + this.m_questID);
		MobileWorldQuest mobileWorldQuest = (MobileWorldQuest)WorldQuestData.worldQuestDictionary.get_Item(this.m_questID);
		if (mobileWorldQuest == null || mobileWorldQuest.Item == null)
		{
			return;
		}
		MobileWorldQuestReward[] item = mobileWorldQuest.Item;
		for (int i = 0; i < item.Length; i++)
		{
			MobileWorldQuestReward mobileWorldQuestReward = item[i];
			ItemRec record = StaticDB.itemDB.GetRecord(mobileWorldQuestReward.RecordID);
			if (record == null)
			{
				Debug.LogWarning(string.Concat(new object[]
				{
					"Invalid Item ID ",
					mobileWorldQuestReward.RecordID,
					" from Quest ID ",
					this.m_questID,
					". Ignoring for loot quality check."
				}));
			}
			else
			{
				if (record.OverallQualityID > (int)this.m_lootQuality)
				{
					this.m_lootQuality = (ITEM_QUALITY)record.OverallQualityID;
				}
				if (this.m_showLootIconInsteadOfMain)
				{
					this.m_main.set_sprite(GeneralHelpers.LoadIconAsset(AssetBundleType.Icons, mobileWorldQuestReward.FileDataID));
				}
			}
		}
		if (this.m_showLootIconInsteadOfMain)
		{
			if (mobileWorldQuest.Money > 0)
			{
				this.m_main.set_sprite(Resources.Load<Sprite>("MiscIcons/INV_Misc_Coin_01"));
			}
			if (mobileWorldQuest.Experience > 0)
			{
				this.m_main.set_sprite(GeneralHelpers.GetLocalizedFollowerXpIcon());
			}
			MobileWorldQuestReward[] currency = mobileWorldQuest.Currency;
			for (int j = 0; j < currency.Length; j++)
			{
				MobileWorldQuestReward mobileWorldQuestReward2 = currency[j];
				CurrencyTypesRec record2 = StaticDB.currencyTypesDB.GetRecord(mobileWorldQuestReward2.RecordID);
				if (record2 != null)
				{
					this.m_main.set_sprite(GeneralHelpers.LoadCurrencyIcon(mobileWorldQuestReward2.RecordID));
				}
			}
		}
		this.m_endTime = (long)mobileWorldQuest.EndTime;
		int areaID = 0;
		WorldMapAreaRec record3 = StaticDB.worldMapAreaDB.GetRecord(mobileWorldQuest.WorldMapAreaID);
		if (record3 != null)
		{
			areaID = record3.AreaID;
		}
		this.m_areaID = areaID;
		QuestInfoRec record4 = StaticDB.questInfoDB.GetRecord(mobileWorldQuest.QuestInfoID);
		if (record4 == null)
		{
			return;
		}
		bool active = (record4.Modifiers & 2) != 0;
		this.m_dragonFrame.get_gameObject().SetActive(active);
		bool flag = (record4.Modifiers & 1) != 0;
		if (flag && record4.Type != 3)
		{
			this.m_background.set_sprite(Resources.Load<Sprite>("NewWorldQuest/Mobile-RareQuest"));
		}
		bool flag2 = (record4.Modifiers & 4) != 0;
		if (flag2 && record4.Type != 3)
		{
			this.m_background.set_sprite(Resources.Load<Sprite>("NewWorldQuest/Mobile-EpicQuest"));
		}
		int uITextureAtlasMemberID;
		string text;
		switch (record4.Type)
		{
		case 1:
		{
			int profession = record4.Profession;
			switch (profession)
			{
			case 182:
				uITextureAtlasMemberID = TextureAtlas.GetUITextureAtlasMemberID("worldquest-icon-herbalism");
				text = "Mobile-Herbalism";
				goto IL_50E;
			case 183:
			case 184:
				IL_2F9:
				if (profession == 164)
				{
					uITextureAtlasMemberID = TextureAtlas.GetUITextureAtlasMemberID("worldquest-icon-blacksmithing");
					text = "Mobile-Blacksmithing";
					goto IL_50E;
				}
				if (profession == 165)
				{
					uITextureAtlasMemberID = TextureAtlas.GetUITextureAtlasMemberID("worldquest-icon-leatherworking");
					text = "Mobile-Leatherworking";
					goto IL_50E;
				}
				if (profession == 129)
				{
					uITextureAtlasMemberID = TextureAtlas.GetUITextureAtlasMemberID("worldquest-icon-firstaid");
					text = "Mobile-FirstAid";
					goto IL_50E;
				}
				if (profession == 171)
				{
					uITextureAtlasMemberID = TextureAtlas.GetUITextureAtlasMemberID("worldquest-icon-alchemy");
					text = "Mobile-Alchemy";
					goto IL_50E;
				}
				if (profession == 197)
				{
					uITextureAtlasMemberID = TextureAtlas.GetUITextureAtlasMemberID("worldquest-icon-tailoring");
					text = "Mobile-Tailoring";
					goto IL_50E;
				}
				if (profession == 202)
				{
					uITextureAtlasMemberID = TextureAtlas.GetUITextureAtlasMemberID("worldquest-icon-engineering");
					text = "Mobile-Engineering";
					goto IL_50E;
				}
				if (profession == 333)
				{
					uITextureAtlasMemberID = TextureAtlas.GetUITextureAtlasMemberID("worldquest-icon-enchanting");
					text = "Mobile-Enchanting";
					goto IL_50E;
				}
				if (profession == 356)
				{
					uITextureAtlasMemberID = TextureAtlas.GetUITextureAtlasMemberID("worldquest-icon-fishing");
					text = "Mobile-Fishing";
					goto IL_50E;
				}
				if (profession == 393)
				{
					uITextureAtlasMemberID = TextureAtlas.GetUITextureAtlasMemberID("worldquest-icon-skinning");
					text = "Mobile-Skinning";
					goto IL_50E;
				}
				if (profession == 755)
				{
					uITextureAtlasMemberID = TextureAtlas.GetUITextureAtlasMemberID("worldquest-icon-jewelcrafting");
					text = "Mobile-Jewelcrafting";
					goto IL_50E;
				}
				if (profession == 773)
				{
					uITextureAtlasMemberID = TextureAtlas.GetUITextureAtlasMemberID("worldquest-icon-inscription");
					text = "Mobile-Inscription";
					goto IL_50E;
				}
				if (profession != 794)
				{
					uITextureAtlasMemberID = TextureAtlas.GetUITextureAtlasMemberID("worldquest-questmarker-questbang");
					text = "Mobile-QuestExclamationIcon";
					goto IL_50E;
				}
				uITextureAtlasMemberID = TextureAtlas.GetUITextureAtlasMemberID("worldquest-icon-archaeology");
				text = "Mobile-Archaeology";
				goto IL_50E;
			case 185:
				uITextureAtlasMemberID = TextureAtlas.GetUITextureAtlasMemberID("worldquest-icon-cooking");
				text = "Mobile-Cooking";
				goto IL_50E;
			case 186:
				uITextureAtlasMemberID = TextureAtlas.GetUITextureAtlasMemberID("worldquest-icon-mining");
				text = "Mobile-Mining";
				goto IL_50E;
			}
			goto IL_2F9;
			IL_50E:
			goto IL_55B;
		}
		case 3:
			uITextureAtlasMemberID = TextureAtlas.GetUITextureAtlasMemberID("worldquest-icon-pvp-ffa");
			text = "Mobile-PVP";
			goto IL_55B;
		case 4:
			uITextureAtlasMemberID = TextureAtlas.GetUITextureAtlasMemberID("worldquest-icon-petbattle");
			text = "Mobile-Pets";
			goto IL_55B;
		}
		uITextureAtlasMemberID = TextureAtlas.GetUITextureAtlasMemberID("worldquest-questmarker-questbang");
		text = "Mobile-QuestExclamationIcon";
		IL_55B:
		if (!this.m_showLootIconInsteadOfMain)
		{
			if (text != null)
			{
				this.m_main.set_sprite(Resources.Load<Sprite>("NewWorldQuest/" + text));
			}
			else if (uITextureAtlasMemberID > 0)
			{
				this.m_main.set_sprite(TextureAtlas.instance.GetAtlasSprite(uITextureAtlasMemberID));
				this.m_main.SetNativeSize();
			}
		}
	}

	public void OnClick()
	{
		Main.instance.m_UISound.Play_SelectWorldQuest();
		UiAnimMgr.instance.PlayAnim("MinimapPulseAnim", base.get_transform(), Vector3.get_zero(), 2f, 0f);
		AllPopups.instance.ShowWorldQuestTooltip(this.m_questID);
		StackableMapIcon component = base.GetComponent<StackableMapIcon>();
		if (component != null)
		{
			StackableMapIconContainer container = component.GetContainer();
			AdventureMapPanel.instance.SetSelectedIconContainer(container);
		}
	}

	private void Awake()
	{
		this.m_errorImage.get_gameObject().SetActive(false);
		this.m_dragonFrame.get_gameObject().SetActive(false);
		this.m_highlight.get_gameObject().SetActive(false);
		this.m_expiringSoon.get_gameObject().SetActive(false);
	}

	private void Update()
	{
		long num = this.m_endTime - GarrisonStatus.CurrentTime();
		bool active = num < 4500L;
		this.m_expiringSoon.get_gameObject().SetActive(active);
		if (num <= 0L)
		{
			Object.DestroyImmediate(base.get_gameObject());
			return;
		}
	}
}

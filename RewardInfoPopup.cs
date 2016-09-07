using System;
using UnityEngine;
using UnityEngine.UI;
using WowJamMessages.MobileClientJSON;
using WowStatConstants;
using WowStaticData;

public class RewardInfoPopup : MonoBehaviour
{
	public Text m_rewardName;

	public Text m_rewardDescription;

	public Text m_rewardQuantity;

	public Image m_rewardIcon;

	private int m_rewardID;

	private MissionRewardDisplay.RewardType m_rewardType;

	public bool m_muteEnableSFX;

	public bool m_disableScreenBlurEffect;

	private void Start()
	{
		this.m_rewardName.set_font(GeneralHelpers.LoadStandardFont());
		this.m_rewardDescription.set_font(GeneralHelpers.LoadStandardFont());
		this.m_rewardQuantity.set_font(GeneralHelpers.LoadStandardFont());
	}

	public void OnEnable()
	{
		if (!this.m_muteEnableSFX)
		{
			Main.instance.m_UISound.Play_ShowGenericTooltip();
		}
		if (!this.m_disableScreenBlurEffect)
		{
			Main.instance.m_canvasBlurManager.AddBlurRef_MainCanvas();
			Main.instance.m_canvasBlurManager.AddBlurRef_Level2Canvas();
		}
		Main.instance.m_backButtonManager.PushBackAction(BackAction.hideAllPopups, null);
	}

	private void OnDisable()
	{
		if (!this.m_disableScreenBlurEffect)
		{
			Main.instance.m_canvasBlurManager.RemoveBlurRef_MainCanvas();
			Main.instance.m_canvasBlurManager.RemoveBlurRef_Level2Canvas();
		}
		if (this.m_rewardType == MissionRewardDisplay.RewardType.item)
		{
			ItemStatCache expr_39 = ItemStatCache.instance;
			expr_39.ItemStatCacheUpdateAction = (Action<int, int, MobileItemStats>)Delegate.Remove(expr_39.ItemStatCacheUpdateAction, new Action<int, int, MobileItemStats>(this.ItemStatsUpdated));
		}
		Main.instance.m_backButtonManager.PopBackAction();
	}

	public void SetReward(MissionRewardDisplay.RewardType rewardType, int rewardID, int rewardQuantity, Sprite rewardSprite, int itemContext)
	{
		this.m_rewardType = rewardType;
		this.m_rewardID = rewardID;
		switch (rewardType)
		{
		case MissionRewardDisplay.RewardType.item:
		{
			ItemStatCache expr_34 = ItemStatCache.instance;
			expr_34.ItemStatCacheUpdateAction = (Action<int, int, MobileItemStats>)Delegate.Combine(expr_34.ItemStatCacheUpdateAction, new Action<int, int, MobileItemStats>(this.ItemStatsUpdated));
			this.SetItem(rewardID, itemContext, rewardSprite);
			break;
		}
		case MissionRewardDisplay.RewardType.gold:
			this.SetGold(rewardQuantity, rewardSprite);
			break;
		case MissionRewardDisplay.RewardType.followerXP:
			this.SetFollowerXP(rewardQuantity, rewardSprite);
			break;
		case MissionRewardDisplay.RewardType.currency:
			this.SetCurrency(rewardID, rewardQuantity, rewardSprite);
			break;
		case MissionRewardDisplay.RewardType.faction:
			this.SetFaction(rewardID, rewardQuantity, rewardSprite);
			break;
		}
	}

	private void ItemStatsUpdated(int itemID, int itemContext, MobileItemStats itemStats)
	{
		if (this.m_rewardType == MissionRewardDisplay.RewardType.item)
		{
			this.SetItem(this.m_rewardID, itemContext, this.m_rewardIcon.get_sprite());
		}
	}

	public void SetItem(int itemID, int itemContext, Sprite iconSprite)
	{
		this.m_rewardQuantity.set_text(string.Empty);
		this.m_rewardName.set_text(string.Empty);
		this.m_rewardDescription.set_text(string.Empty);
		this.m_rewardIcon.set_sprite(iconSprite);
		ItemRec record = StaticDB.itemDB.GetRecord(itemID);
		if (record != null)
		{
			MobileItemStats itemStats = ItemStatCache.instance.GetItemStats(itemID, itemContext);
			if (itemStats != null)
			{
				this.m_rewardName.set_text(GeneralHelpers.GetItemQualityColorTag(itemStats.Quality) + record.Display + "</color>");
			}
			else
			{
				this.m_rewardName.set_text(GeneralHelpers.GetItemQualityColorTag(record.OverallQualityID) + record.Display + "</color>");
			}
			this.m_rewardName.set_supportRichText(true);
			if (record.ItemNameDescriptionID > 0)
			{
				ItemNameDescriptionRec record2 = StaticDB.itemNameDescriptionDB.GetRecord(record.ItemNameDescriptionID);
				if (record2 != null)
				{
					Text expr_E7 = this.m_rewardName;
					string text = expr_E7.get_text();
					expr_E7.set_text(string.Concat(new string[]
					{
						text,
						"\n<color=#",
						GeneralHelpers.GetColorFromInt(record2.Color),
						"ff>",
						record2.Description,
						"</color>"
					}));
				}
			}
			if (record.ClassID == 2 || record.ClassID == 3 || record.ClassID == 4 || record.ClassID == 5 || record.ClassID == 6)
			{
				int itemLevel = record.ItemLevel;
				if (itemStats != null)
				{
					itemLevel = itemStats.ItemLevel;
				}
				Text expr_189 = this.m_rewardName;
				string text = expr_189.get_text();
				expr_189.set_text(string.Concat(new string[]
				{
					text,
					"\n<color=#",
					GeneralHelpers.s_defaultColor,
					">",
					StaticDB.GetString("ITEM_LEVEL", null),
					" ",
					itemLevel.ToString(),
					"</color>"
				}));
			}
			if (record.Bonding > 0)
			{
				string text2 = string.Empty;
				if ((record.Flags & 134217728) != 0)
				{
					if ((record.Flags1 & 131072) != 0)
					{
						text2 = StaticDB.GetString("ITEM_BIND_TO_BNETACCOUNT", null);
					}
					else
					{
						text2 = StaticDB.GetString("ITEM_BIND_TO_ACCOUNT", null);
					}
				}
				else if (record.Bonding == 1)
				{
					text2 = StaticDB.GetString("ITEM_BIND_ON_PICKUP", null);
				}
				else if (record.Bonding == 4)
				{
					text2 = StaticDB.GetString("ITEM_BIND_QUEST", null);
				}
				else if (record.Bonding == 2)
				{
					text2 = StaticDB.GetString("ITEM_BIND_ON_EQUIP", null);
				}
				else if (record.Bonding == 3)
				{
					text2 = StaticDB.GetString("ITEM_BIND_ON_USE", null);
				}
				if (text2 != string.Empty)
				{
					Text expr_2C9 = this.m_rewardName;
					string text = expr_2C9.get_text();
					expr_2C9.set_text(string.Concat(new string[]
					{
						text,
						"\n<color=#",
						GeneralHelpers.s_normalColor,
						">",
						text2,
						"</color>"
					}));
				}
			}
			ItemSubClassRec itemSubclass = StaticDB.GetItemSubclass(record.ClassID, record.SubclassID);
			if (itemSubclass != null && itemSubclass.DisplayName != null && itemSubclass.DisplayName != string.Empty && (itemSubclass.DisplayFlags & 1) == 0 && record.InventoryType != 16)
			{
				if (this.m_rewardDescription.get_text() != string.Empty)
				{
					Text expr_382 = this.m_rewardDescription;
					expr_382.set_text(expr_382.get_text() + "\n");
				}
				Text expr_39D = this.m_rewardDescription;
				string text = expr_39D.get_text();
				expr_39D.set_text(string.Concat(new string[]
				{
					text,
					"<color=#",
					GeneralHelpers.s_normalColor,
					">",
					itemSubclass.DisplayName,
					"</color>"
				}));
			}
			string inventoryTypeString = GeneralHelpers.GetInventoryTypeString((INVENTORY_TYPE)record.InventoryType);
			if (inventoryTypeString != null && inventoryTypeString != string.Empty)
			{
				if (this.m_rewardDescription.get_text() != string.Empty)
				{
					Text expr_429 = this.m_rewardDescription;
					expr_429.set_text(expr_429.get_text() + "\n");
				}
				Text expr_444 = this.m_rewardDescription;
				string text = expr_444.get_text();
				expr_444.set_text(string.Concat(new string[]
				{
					text,
					"<color=#",
					GeneralHelpers.s_normalColor,
					">",
					inventoryTypeString,
					"</color>"
				}));
			}
			if (itemStats != null)
			{
				if (itemStats.MinDamage != 0 || itemStats.MaxDamage != 0)
				{
					if (this.m_rewardDescription.get_text() != string.Empty)
					{
						Text expr_4C2 = this.m_rewardDescription;
						expr_4C2.set_text(expr_4C2.get_text() + "\n");
					}
					if (itemStats.MinDamage == itemStats.MaxDamage)
					{
						Text expr_4EE = this.m_rewardDescription;
						expr_4EE.set_text(expr_4EE.get_text() + GeneralHelpers.TextOrderString(itemStats.MinDamage.ToString(), StaticDB.GetString("DAMAGE", null)));
					}
					else
					{
						Text expr_528 = this.m_rewardDescription;
						expr_528.set_text(expr_528.get_text() + GeneralHelpers.TextOrderString(itemStats.MinDamage.ToString() + " - " + itemStats.MaxDamage.ToString(), StaticDB.GetString("DAMAGE", null)));
					}
				}
				if (itemStats.EffectiveArmor > 0)
				{
					if (this.m_rewardDescription.get_text() != string.Empty)
					{
						Text expr_59C = this.m_rewardDescription;
						expr_59C.set_text(expr_59C.get_text() + "\n");
					}
					Text expr_5B7 = this.m_rewardDescription;
					string text = expr_5B7.get_text();
					expr_5B7.set_text(string.Concat(new string[]
					{
						text,
						"<color=#",
						GeneralHelpers.s_normalColor,
						">",
						GeneralHelpers.TextOrderString(itemStats.EffectiveArmor.ToString(), StaticDB.GetString("ARMOR", null)),
						"</color>"
					}));
				}
				MobileItemBonusStat[] bonusStat = itemStats.BonusStat;
				for (int i = 0; i < bonusStat.Length; i++)
				{
					MobileItemBonusStat mobileItemBonusStat = bonusStat[i];
					if (mobileItemBonusStat.BonusAmount != 0)
					{
						if (this.m_rewardDescription.get_text() != string.Empty)
						{
							Text expr_659 = this.m_rewardDescription;
							expr_659.set_text(expr_659.get_text() + "\n");
						}
						Text expr_674 = this.m_rewardDescription;
						expr_674.set_text(expr_674.get_text() + "<color=#" + GeneralHelpers.GetMobileStatColorString(mobileItemBonusStat.Color) + ">");
						string text3;
						if (mobileItemBonusStat.BonusAmount > 0)
						{
							text3 = "+";
						}
						else
						{
							text3 = "-";
						}
						Text expr_6C0 = this.m_rewardDescription;
						expr_6C0.set_text(expr_6C0.get_text() + GeneralHelpers.TextOrderString(text3 + mobileItemBonusStat.BonusAmount.ToString(), GeneralHelpers.GetBonusStatString((BonusStatIndex)mobileItemBonusStat.StatID)) + "</color>");
					}
				}
			}
			int requiredLevel = record.RequiredLevel;
			if (itemStats != null)
			{
				requiredLevel = itemStats.RequiredLevel;
			}
			if (requiredLevel > 1)
			{
				if (this.m_rewardDescription.get_text() != string.Empty)
				{
					Text expr_74C = this.m_rewardDescription;
					expr_74C.set_text(expr_74C.get_text() + "\n");
				}
				string text4 = GeneralHelpers.s_normalColor;
				if (GarrisonStatus.CharacterLevel() < requiredLevel)
				{
					text4 = GeneralHelpers.GetMobileStatColorString(MobileStatColor.MOBILE_STAT_COLOR_ERROR);
				}
				Text expr_782 = this.m_rewardDescription;
				string text = expr_782.get_text();
				expr_782.set_text(string.Concat(new object[]
				{
					text,
					"<color=#",
					text4,
					">",
					StaticDB.GetString("ITEM_MIN_LEVEL", null),
					" ",
					requiredLevel,
					"</color>"
				}));
			}
			string itemDescription = GeneralHelpers.GetItemDescription(record);
			if (itemDescription != null && itemDescription != string.Empty)
			{
				if (this.m_rewardDescription.get_text() != string.Empty)
				{
					Text expr_81C = this.m_rewardDescription;
					expr_81C.set_text(expr_81C.get_text() + "\n");
				}
				Text expr_837 = this.m_rewardDescription;
				expr_837.set_text(expr_837.get_text() + itemDescription);
			}
			else if (itemStats == null)
			{
				if (this.m_rewardDescription.get_text() != string.Empty)
				{
					Text expr_874 = this.m_rewardDescription;
					expr_874.set_text(expr_874.get_text() + "\n");
				}
				Text expr_88F = this.m_rewardDescription;
				expr_88F.set_text(expr_88F.get_text() + "...");
			}
		}
		else
		{
			this.m_rewardName.set_text("Unknown Item" + itemID);
			this.m_rewardDescription.set_text(string.Empty);
		}
	}

	public void SetFollowerXP(int quantity, Sprite iconSprite)
	{
		this.m_rewardName.set_text(StaticDB.GetString("EXPERIENCE", null));
		this.m_rewardDescription.set_text(StaticDB.GetString("EXPERIENCE_DESCRIPTION", null));
		this.m_rewardQuantity.set_text((quantity <= 1) ? string.Empty : (string.Empty + quantity));
		this.m_rewardIcon.set_sprite(iconSprite);
	}

	public void SetCurrency(int currencyID, int quantity, Sprite iconSprite)
	{
		CurrencyTypesRec record = StaticDB.currencyTypesDB.GetRecord(currencyID);
		if (record != null)
		{
			this.m_rewardName.set_text(record.Name);
			this.m_rewardDescription.set_text(record.Description);
		}
		this.m_rewardQuantity.set_text((quantity <= 1) ? string.Empty : (string.Empty + quantity));
		this.m_rewardIcon.set_sprite(iconSprite);
	}

	public void SetGold(int quantity, Sprite iconSprite)
	{
		this.m_rewardName.set_text(StaticDB.GetString("GOLD", null));
		this.m_rewardDescription.set_text(StaticDB.GetString("GOLD_DESCRIPTION", null));
		this.m_rewardQuantity.set_text((quantity <= 1) ? string.Empty : (string.Empty + quantity));
		this.m_rewardIcon.set_sprite(iconSprite);
	}

	public void SetFaction(int factionID, int quantity, Sprite iconSprite)
	{
		FactionRec record = StaticDB.factionDB.GetRecord(factionID);
		if (record != null)
		{
			this.m_rewardName.set_text(string.Concat(new object[]
			{
				StaticDB.GetString("REPUTATION_AWARD", null),
				"\n<color=#",
				GeneralHelpers.s_defaultColor,
				">",
				record.Name,
				" +",
				quantity,
				"</color>"
			}));
			this.m_rewardName.set_supportRichText(true);
			this.m_rewardDescription.set_text(string.Empty);
		}
		this.m_rewardQuantity.set_text((quantity <= 1) ? string.Empty : (string.Empty + quantity));
		this.m_rewardIcon.set_sprite(iconSprite);
	}
}

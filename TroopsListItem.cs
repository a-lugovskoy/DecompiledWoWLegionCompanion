using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using WowJamMessages;
using WowJamMessages.MobileClientJSON;
using WowJamMessages.MobilePlayerJSON;
using WowStatConstants;
using WowStaticData;

public class TroopsListItem : MonoBehaviour
{
	public GameObject m_troopSpecificArea;

	public GameObject m_itemSpecificArea;

	public Image m_troopSnapshotImage;

	public GameObject m_troopHeartContainer;

	public GameObject m_troopHeartPrefab;

	public Text m_troopName;

	public GameObject m_traitsAndAbilitiesRootObject;

	public GameObject m_abilityDisplayPrefab;

	public GameObject m_troopSlotsRootObject;

	public GameObject m_troopSlotPrefab;

	public Image m_troopResourceIcon;

	public Text m_troopResourceCostText;

	public Image m_itemResourceIcon;

	public Text m_itemResourceCostText;

	public Button m_recruitTroopsButton;

	public Text m_recruitButtonText;

	public Text m_itemName;

	public MissionRewardDisplay m_itemDisplay;

	private MobileClientShipmentType m_shipmentType;

	private bool m_isTroop;

	private int m_shipmentCost;

	private GarrFollowerRec m_followerRec;

	private CharShipmentRec m_charShipmentRec;

	public void HandleFollowerDataChanged()
	{
		if (this.m_shipmentType == null)
		{
			return;
		}
		if (this.m_isTroop)
		{
			this.UpdateTroopSlots();
		}
		else
		{
			this.UpdateItemSlots();
		}
		this.UpdateRecruitButtonState();
	}

	private void UpdateRecruitButtonState()
	{
		TroopSlot[] componentsInChildren = this.m_troopSlotsRootObject.GetComponentsInChildren<TroopSlot>();
		bool flag = false;
		TroopSlot[] array = componentsInChildren;
		for (int i = 0; i < array.Length; i++)
		{
			TroopSlot troopSlot = array[i];
			if (troopSlot.IsEmpty())
			{
				flag = true;
				break;
			}
		}
		bool flag2 = GarrisonStatus.Resources() >= this.m_shipmentCost;
		this.m_itemResourceCostText.set_color((!flag2) ? Color.get_red() : Color.get_white());
		this.m_recruitButtonText.set_color(new Color(1f, 0.82f, 0f, 1f));
		if (!flag)
		{
			this.m_recruitButtonText.set_text(StaticDB.GetString("SLOTS_FULL", null));
			this.m_recruitButtonText.set_color(new Color(0.5f, 0.5f, 0.5f, 1f));
		}
		else if (!flag2)
		{
			this.m_recruitButtonText.set_text(StaticDB.GetString("CANT_AFFORD", "Can't Afford"));
			this.m_recruitButtonText.set_color(new Color(0.5f, 0.5f, 0.5f, 1f));
		}
		else if (this.m_isTroop)
		{
			this.m_recruitButtonText.set_text(StaticDB.GetString("RECRUIT", null));
		}
		else
		{
			this.m_recruitButtonText.set_text(StaticDB.GetString("PLACE_ORDER", null));
		}
		if (flag && flag2)
		{
			this.m_recruitTroopsButton.set_interactable(true);
		}
		else
		{
			this.m_recruitTroopsButton.set_interactable(false);
		}
	}

	private void Start()
	{
		Main expr_05 = Main.instance;
		expr_05.ShipmentAddedAction = (Action<int, ulong>)Delegate.Combine(expr_05.ShipmentAddedAction, new Action<int, ulong>(this.HandleShipmentAdded));
		this.m_troopName.set_font(GeneralHelpers.LoadStandardFont());
		this.m_troopResourceCostText.set_font(GeneralHelpers.LoadStandardFont());
		this.m_itemResourceCostText.set_font(GeneralHelpers.LoadStandardFont());
		this.m_recruitButtonText.set_font(GeneralHelpers.LoadStandardFont());
		this.m_itemName.set_font(GeneralHelpers.LoadStandardFont());
	}

	private void OnDestroy()
	{
		Main expr_05 = Main.instance;
		expr_05.ShipmentAddedAction = (Action<int, ulong>)Delegate.Remove(expr_05.ShipmentAddedAction, new Action<int, ulong>(this.HandleShipmentAdded));
	}

	public void SetCharShipment(MobileClientShipmentType shipmentType)
	{
		this.m_shipmentCost = shipmentType.CurrencyCost;
		this.m_shipmentType = shipmentType;
		Transform[] componentsInChildren = this.m_troopHeartContainer.GetComponentsInChildren<Transform>();
		Transform[] array = componentsInChildren;
		for (int i = 0; i < array.Length; i++)
		{
			Transform transform = array[i];
			if (transform != this.m_troopHeartContainer.get_transform())
			{
				Object.DestroyImmediate(transform.get_gameObject());
			}
		}
		AbilityDisplay[] componentsInChildren2 = this.m_traitsAndAbilitiesRootObject.GetComponentsInChildren<AbilityDisplay>();
		AbilityDisplay[] array2 = componentsInChildren2;
		for (int j = 0; j < array2.Length; j++)
		{
			AbilityDisplay abilityDisplay = array2[j];
			Object.DestroyImmediate(abilityDisplay.get_gameObject());
		}
		TroopSlot[] componentsInChildren3 = this.m_troopSlotsRootObject.GetComponentsInChildren<TroopSlot>();
		TroopSlot[] array3 = componentsInChildren3;
		for (int k = 0; k < array3.Length; k++)
		{
			TroopSlot troopSlot = array3[k];
			Object.DestroyImmediate(troopSlot.get_gameObject());
		}
		CharShipmentRec record = StaticDB.charShipmentDB.GetRecord(shipmentType.CharShipmentID);
		if (record == null)
		{
			Debug.LogError("Invalid Shipment ID: " + shipmentType.CharShipmentID);
			this.m_troopName.set_text("Invalid Shipment ID: " + shipmentType.CharShipmentID);
			return;
		}
		if (record.GarrFollowerID > 0u)
		{
			this.SetCharShipmentTroop(shipmentType, record);
		}
		else if (record.DummyItemID > 0)
		{
			this.SetCharShipmentItem(shipmentType, record);
		}
	}

	private void UpdateItemSlots()
	{
		int maxShipments = (int)this.m_charShipmentRec.MaxShipments;
		TroopSlot[] componentsInChildren = this.m_troopSlotsRootObject.GetComponentsInChildren<TroopSlot>(true);
		if (componentsInChildren.Length < maxShipments)
		{
			for (int i = componentsInChildren.Length; i < maxShipments; i++)
			{
				GameObject gameObject = Object.Instantiate<GameObject>(this.m_troopSlotPrefab);
				gameObject.get_transform().SetParent(this.m_troopSlotsRootObject.get_transform(), false);
				TroopSlot component = gameObject.GetComponent<TroopSlot>();
				component.SetCharShipment(this.m_shipmentType.CharShipmentID, 0uL, 0, false, 0);
			}
		}
		if (componentsInChildren.Length > maxShipments)
		{
			for (int j = maxShipments; j < componentsInChildren.Length; j++)
			{
				Object.DestroyImmediate(componentsInChildren[j].get_gameObject());
			}
		}
		componentsInChildren = this.m_troopSlotsRootObject.GetComponentsInChildren<TroopSlot>(true);
		TroopSlot[] array = componentsInChildren;
		for (int k = 0; k < array.Length; k++)
		{
			TroopSlot troopSlot = array[k];
			if (troopSlot.GetDBID() != 0uL && !PersistentShipmentData.shipmentDictionary.ContainsKey(troopSlot.GetDBID()))
			{
				troopSlot.SetCharShipment(this.m_shipmentType.CharShipmentID, 0uL, 0, false, 0);
			}
		}
		IEnumerator enumerator = PersistentShipmentData.shipmentDictionary.get_Values().GetEnumerator();
		try
		{
			while (enumerator.MoveNext())
			{
				JamCharacterShipment jamCharacterShipment = (JamCharacterShipment)enumerator.get_Current();
				if (jamCharacterShipment.ShipmentRecID == this.m_shipmentType.CharShipmentID)
				{
					this.SetTroopSlotForPendingShipment(componentsInChildren, jamCharacterShipment.ShipmentID);
				}
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
	}

	private void SetCharShipmentItem(MobileClientShipmentType shipmentType, CharShipmentRec charShipmentRec)
	{
		this.m_isTroop = false;
		this.m_charShipmentRec = charShipmentRec;
		this.m_troopSpecificArea.SetActive(false);
		this.m_itemSpecificArea.SetActive(true);
		this.m_troopName.get_gameObject().SetActive(false);
		this.m_itemName.get_gameObject().SetActive(true);
		ItemRec record = StaticDB.itemDB.GetRecord(charShipmentRec.DummyItemID);
		if (record == null)
		{
			Debug.LogError("Invalid Item ID: " + charShipmentRec.DummyItemID);
			this.m_troopName.set_text("Invalid Item ID: " + charShipmentRec.DummyItemID);
			return;
		}
		this.m_itemDisplay.InitReward(MissionRewardDisplay.RewardType.item, charShipmentRec.DummyItemID, 1, 0, record.IconFileDataID);
		this.m_itemName.set_text(record.Display);
		Sprite sprite = GeneralHelpers.LoadIconAsset(AssetBundleType.Icons, record.IconFileDataID);
		if (sprite != null)
		{
			this.m_troopSnapshotImage.set_sprite(sprite);
		}
		this.m_itemResourceCostText.set_text(string.Empty + shipmentType.CurrencyCost);
		Sprite sprite2 = GeneralHelpers.LoadCurrencyIcon(shipmentType.CurrencyTypeID);
		if (sprite2 != null)
		{
			this.m_itemResourceIcon.set_sprite(sprite2);
		}
		this.UpdateItemSlots();
		this.UpdateRecruitButtonState();
	}

	private void SetTroopSlotForExistingFollower(TroopSlot[] troopSlots, JamGarrisonFollower follower)
	{
		if (follower.Durability <= 0)
		{
			return;
		}
		for (int i = 0; i < troopSlots.Length; i++)
		{
			TroopSlot troopSlot = troopSlots[i];
			int ownedFollowerID = troopSlot.GetOwnedFollowerID();
			if (ownedFollowerID != 0 && ownedFollowerID == follower.GarrFollowerID)
			{
				return;
			}
		}
		for (int j = 0; j < troopSlots.Length; j++)
		{
			TroopSlot troopSlot2 = troopSlots[j];
			if (troopSlot2.IsCollected())
			{
				GarrFollowerRec record = StaticDB.garrFollowerDB.GetRecord(follower.GarrFollowerID);
				int iconFileDataID = (GarrisonStatus.Faction() != PVP_FACTION.HORDE) ? record.AllianceIconFileDataID : record.HordeIconFileDataID;
				troopSlot2.SetCharShipment(this.m_shipmentType.CharShipmentID, 0uL, follower.GarrFollowerID, false, iconFileDataID);
				return;
			}
		}
		for (int k = 0; k < troopSlots.Length; k++)
		{
			TroopSlot troopSlot3 = troopSlots[k];
			if (troopSlot3.IsEmpty())
			{
				GarrFollowerRec record2 = StaticDB.garrFollowerDB.GetRecord(follower.GarrFollowerID);
				int iconFileDataID2 = (GarrisonStatus.Faction() != PVP_FACTION.HORDE) ? record2.AllianceIconFileDataID : record2.HordeIconFileDataID;
				troopSlot3.SetCharShipment(this.m_shipmentType.CharShipmentID, 0uL, follower.GarrFollowerID, false, iconFileDataID2);
				return;
			}
		}
	}

	private void SetTroopSlotForPendingShipment(TroopSlot[] troopSlots, ulong shipmentDBID)
	{
		for (int i = 0; i < troopSlots.Length; i++)
		{
			TroopSlot troopSlot = troopSlots[i];
			if (troopSlot.GetDBID() == shipmentDBID)
			{
				return;
			}
		}
		for (int j = 0; j < troopSlots.Length; j++)
		{
			TroopSlot troopSlot2 = troopSlots[j];
			if (troopSlot2.IsEmpty())
			{
				troopSlot2.SetCharShipment(this.m_shipmentType.CharShipmentID, shipmentDBID, 0, true, 0);
				return;
			}
		}
	}

	private void UpdateTroopSlots()
	{
		int maxTroops = this.GetMaxTroops((int)((GarrisonStatus.Faction() != PVP_FACTION.HORDE) ? this.m_followerRec.AllianceGarrClassSpecID : this.m_followerRec.HordeGarrClassSpecID));
		TroopSlot[] componentsInChildren = this.m_troopSlotsRootObject.GetComponentsInChildren<TroopSlot>(true);
		if (componentsInChildren.Length < maxTroops)
		{
			for (int i = componentsInChildren.Length; i < maxTroops; i++)
			{
				GameObject gameObject = Object.Instantiate<GameObject>(this.m_troopSlotPrefab);
				gameObject.get_transform().SetParent(this.m_troopSlotsRootObject.get_transform(), false);
				TroopSlot component = gameObject.GetComponent<TroopSlot>();
				component.SetCharShipment(this.m_shipmentType.CharShipmentID, 0uL, 0, false, 0);
			}
		}
		if (componentsInChildren.Length > maxTroops)
		{
			for (int j = maxTroops; j < componentsInChildren.Length; j++)
			{
				Object.DestroyImmediate(componentsInChildren[j].get_gameObject());
			}
		}
		componentsInChildren = this.m_troopSlotsRootObject.GetComponentsInChildren<TroopSlot>(true);
		TroopSlot[] array = componentsInChildren;
		for (int k = 0; k < array.Length; k++)
		{
			TroopSlot troopSlot = array[k];
			int ownedFollowerID = troopSlot.GetOwnedFollowerID();
			if (ownedFollowerID != 0 && (!PersistentFollowerData.followerDictionary.ContainsKey(ownedFollowerID) || PersistentFollowerData.followerDictionary.get_Item(ownedFollowerID).Durability == 0))
			{
				troopSlot.SetCharShipment(this.m_shipmentType.CharShipmentID, 0uL, 0, false, 0);
			}
		}
		uint num = (GarrisonStatus.Faction() != PVP_FACTION.HORDE) ? this.m_followerRec.AllianceGarrClassSpecID : this.m_followerRec.HordeGarrClassSpecID;
		using (Dictionary<int, JamGarrisonFollower>.ValueCollection.Enumerator enumerator = PersistentFollowerData.followerDictionary.get_Values().GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				JamGarrisonFollower current = enumerator.get_Current();
				GarrFollowerRec record = StaticDB.garrFollowerDB.GetRecord(current.GarrFollowerID);
				uint num2 = (GarrisonStatus.Faction() != PVP_FACTION.HORDE) ? record.AllianceGarrClassSpecID : record.HordeGarrClassSpecID;
				if (num2 == num && current.Durability > 0)
				{
					this.SetTroopSlotForExistingFollower(componentsInChildren, current);
				}
			}
		}
		CharShipmentRec record2 = StaticDB.charShipmentDB.GetRecord(this.m_shipmentType.CharShipmentID);
		IEnumerator enumerator2 = PersistentShipmentData.shipmentDictionary.get_Values().GetEnumerator();
		try
		{
			while (enumerator2.MoveNext())
			{
				JamCharacterShipment jamCharacterShipment = (JamCharacterShipment)enumerator2.get_Current();
				if (jamCharacterShipment.ShipmentRecID == this.m_shipmentType.CharShipmentID)
				{
					this.SetTroopSlotForPendingShipment(componentsInChildren, jamCharacterShipment.ShipmentID);
				}
				else
				{
					CharShipmentRec record3 = StaticDB.charShipmentDB.GetRecord(jamCharacterShipment.ShipmentRecID);
					if (record3.ContainerID == record2.ContainerID)
					{
						this.SetTroopSlotForPendingShipment(componentsInChildren, jamCharacterShipment.ShipmentID);
					}
				}
			}
		}
		finally
		{
			IDisposable disposable = enumerator2 as IDisposable;
			if (disposable != null)
			{
				disposable.Dispose();
			}
		}
	}

	private void SetCharShipmentTroop(MobileClientShipmentType shipmentType, CharShipmentRec charShipmentRec)
	{
		this.m_isTroop = true;
		this.m_shipmentType = shipmentType;
		this.m_troopSpecificArea.SetActive(true);
		this.m_itemSpecificArea.SetActive(false);
		this.m_troopName.get_gameObject().SetActive(true);
		this.m_itemName.get_gameObject().SetActive(false);
		GarrFollowerRec record = StaticDB.garrFollowerDB.GetRecord((int)charShipmentRec.GarrFollowerID);
		if (record == null)
		{
			Debug.LogError("Invalid Follower ID: " + charShipmentRec.GarrFollowerID);
			this.m_troopName.set_text("Invalid Follower ID: " + charShipmentRec.GarrFollowerID);
			return;
		}
		this.m_followerRec = record;
		int num = (GarrisonStatus.Faction() != PVP_FACTION.HORDE) ? record.AllianceCreatureID : record.HordeCreatureID;
		CreatureRec record2 = StaticDB.creatureDB.GetRecord(num);
		if (record2 == null)
		{
			Debug.LogError("Invalid Creature ID: " + num);
			this.m_troopName.set_text("Invalid Creature ID: " + num);
			return;
		}
		string text = "Assets/BundleAssets/PortraitIcons/cid_" + record2.ID.ToString("D8") + ".png";
		Sprite sprite = AssetBundleManager.portraitIcons.LoadAsset<Sprite>(text);
		if (sprite != null)
		{
			this.m_troopSnapshotImage.set_sprite(sprite);
		}
		for (int i = 0; i < record.Vitality; i++)
		{
			GameObject gameObject = Object.Instantiate<GameObject>(this.m_troopHeartPrefab);
			gameObject.get_transform().SetParent(this.m_troopHeartContainer.get_transform(), false);
		}
		this.m_troopName.set_text(record2.Name);
		StaticDB.garrFollowerXAbilityDB.EnumRecordsByParentID((int)charShipmentRec.GarrFollowerID, delegate(GarrFollowerXAbilityRec xAbilityRec)
		{
			if (xAbilityRec.FactionIndex == (int)GarrisonStatus.Faction())
			{
				GameObject gameObject2 = Object.Instantiate<GameObject>(this.m_abilityDisplayPrefab);
				gameObject2.get_transform().SetParent(this.m_traitsAndAbilitiesRootObject.get_transform(), false);
				AbilityDisplay component = gameObject2.GetComponent<AbilityDisplay>();
				component.SetAbility(xAbilityRec.GarrAbilityID, true, true, null);
			}
			return true;
		});
		this.UpdateTroopSlots();
		this.m_troopResourceCostText.set_text(string.Empty + shipmentType.CurrencyCost);
		Sprite sprite2 = GeneralHelpers.LoadCurrencyIcon(shipmentType.CurrencyTypeID);
		if (sprite2 != null)
		{
			this.m_troopResourceIcon.set_sprite(sprite2);
		}
		this.UpdateRecruitButtonState();
	}

	public void Recruit()
	{
		Debug.Log("Attempting To Recruit! CharShipment ID = " + this.m_shipmentType.CharShipmentID);
		MobilePlayerCreateShipment mobilePlayerCreateShipment = new MobilePlayerCreateShipment();
		mobilePlayerCreateShipment.CharShipmentID = this.m_shipmentType.CharShipmentID;
		mobilePlayerCreateShipment.NumShipments = 1;
		Login.instance.SendToMobileServer(mobilePlayerCreateShipment);
		Main.instance.m_UISound.Play_RecruitTroop();
	}

	private void HandleShipmentAdded(int charShipmentID, ulong shipmentDBID)
	{
		if (charShipmentID == this.m_shipmentType.CharShipmentID)
		{
			TroopSlot[] componentsInChildren = this.m_troopSlotsRootObject.GetComponentsInChildren<TroopSlot>();
			TroopSlot[] array = componentsInChildren;
			for (int i = 0; i < array.Length; i++)
			{
				TroopSlot troopSlot = array[i];
				if (troopSlot.GetDBID() == shipmentDBID)
				{
					return;
				}
			}
			TroopSlot[] array2 = componentsInChildren;
			for (int j = 0; j < array2.Length; j++)
			{
				TroopSlot troopSlot2 = array2[j];
				if (troopSlot2.IsEmpty())
				{
					troopSlot2.SetCharShipment(charShipmentID, shipmentDBID, 0, true, 0);
					break;
				}
			}
			this.UpdateRecruitButtonState();
		}
	}

	private int GetMaxTroops(int garrClassSpecID)
	{
		GarrClassSpecRec record = StaticDB.garrClassSpecDB.GetRecord(garrClassSpecID);
		int maxTroops = 0;
		if (record != null)
		{
			maxTroops = (int)record.FollowerClassLimit;
		}
		IEnumerator enumerator = PersistentTalentData.talentDictionary.get_Values().GetEnumerator();
		try
		{
			while (enumerator.MoveNext())
			{
				JamGarrisonTalent jamGarrisonTalent = (JamGarrisonTalent)enumerator.get_Current();
				if ((jamGarrisonTalent.Flags & 1) != 0)
				{
					GarrTalentRec record2 = StaticDB.garrTalentDB.GetRecord(jamGarrisonTalent.GarrTalentID);
					if (record2 != null)
					{
						StaticDB.garrAbilityEffectDB.EnumRecordsByParentID((int)record2.GarrAbilityID, delegate(GarrAbilityEffectRec effectRec)
						{
							if (effectRec.AbilityAction == 34u && (ulong)effectRec.ActionRecordID == (ulong)((long)garrClassSpecID))
							{
								maxTroops += (int)effectRec.ActionValueFlat;
							}
							return true;
						});
					}
				}
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
		return maxTroops;
	}

	public void PlayClickSound()
	{
		Main.instance.m_UISound.Play_ButtonRedClick();
	}

	public int GetCharShipmentTypeID()
	{
		if (this.m_shipmentType == null)
		{
			return 0;
		}
		return this.m_shipmentType.CharShipmentID;
	}
}

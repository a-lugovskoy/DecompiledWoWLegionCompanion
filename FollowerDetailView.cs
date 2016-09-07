using System;
using UnityEngine;
using UnityEngine.UI;
using WowJamMessages;
using WowJamMessages.MobilePlayerJSON;
using WowStatConstants;
using WowStaticData;

public class FollowerDetailView : MonoBehaviour
{
	[Header("Spec and Abilities")]
	public GameObject traitsAndAbilitiesRootObject;

	public GameObject specializationHeaderPrefab;

	public GameObject abilitiesHeaderPrefab;

	public GameObject zoneSupportAbilityHeaderPrefab;

	public GameObject abilityDisplayPrefab;

	public GameObject m_spellDisplayPrefab;

	public GameObject m_equipmentSlotPrefab;

	[Header("Follower Snapshot")]
	public Image followerSnapshot;

	[Header("Equipment Slots")]
	public GameObject m_equipmentSlotsRootObject;

	public GameObject m_equipmentSlotsHeader;

	[Header("Misc")]
	public FollowerListView m_followerListView;

	public FollowerInventoryListView m_followerInventoryListView;

	[Header("Not Obsolete?")]
	public Text iLevelText;

	[Header("More Text")]
	public Text m_equipmentSlotsText;

	public Text m_inventoryText;

	[Header("Champion Only")]
	public GameObject m_activateChampionButton;

	public GameObject m_deactivateChampionButton;

	[Header("Troops Only")]
	public Text m_troopDescriptionPrefab;

	private int m_garrFollowerID;

	private string m_ilvlString;

	private void Start()
	{
		this.m_equipmentSlotsText.set_font(GeneralHelpers.LoadStandardFont());
		this.m_inventoryText.set_font(GeneralHelpers.LoadStandardFont());
		this.m_ilvlString = StaticDB.GetString("ILVL", null);
		this.m_equipmentSlotsText.set_text(StaticDB.GetString("EQUIPMENT_SLOTS", null));
		this.m_inventoryText.set_text(StaticDB.GetString("INVENTORY", null));
		Text componentInChildren = this.m_activateChampionButton.GetComponentInChildren<Text>();
		if (componentInChildren != null)
		{
			componentInChildren.set_text(StaticDB.GetString("ACTIVATE", null));
		}
		componentInChildren = this.m_deactivateChampionButton.GetComponentInChildren<Text>();
		if (componentInChildren != null)
		{
			componentInChildren.set_text(StaticDB.GetString("DEACTIVATE", null));
		}
	}

	private void OnEnable()
	{
		Main expr_05 = Main.instance;
		expr_05.UseEquipmentResultAction = (Action<JamGarrisonFollower, JamGarrisonFollower>)Delegate.Combine(expr_05.UseEquipmentResultAction, new Action<JamGarrisonFollower, JamGarrisonFollower>(this.HandleUseEquipmentResult));
		AdventureMapPanel expr_2B = AdventureMapPanel.instance;
		expr_2B.FollowerToInspectChangedAction = (Action<int>)Delegate.Combine(expr_2B.FollowerToInspectChangedAction, new Action<int>(this.HandleFollowerToInspectChanged));
	}

	private void OnDisable()
	{
		Main expr_05 = Main.instance;
		expr_05.UseEquipmentResultAction = (Action<JamGarrisonFollower, JamGarrisonFollower>)Delegate.Remove(expr_05.UseEquipmentResultAction, new Action<JamGarrisonFollower, JamGarrisonFollower>(this.HandleUseEquipmentResult));
		AdventureMapPanel expr_2B = AdventureMapPanel.instance;
		expr_2B.FollowerToInspectChangedAction = (Action<int>)Delegate.Remove(expr_2B.FollowerToInspectChangedAction, new Action<int>(this.HandleFollowerToInspectChanged));
	}

	private void HandleFollowerToInspectChanged(int garrFollowerID)
	{
		this.SetFollower(garrFollowerID);
	}

	public void HandleFollowerDataChanged()
	{
		this.SetFollower(this.m_garrFollowerID);
	}

	private void HandleUseEquipmentResult(JamGarrisonFollower oldFollower, JamGarrisonFollower newFollower)
	{
		if (this.m_garrFollowerID != newFollower.GarrFollowerID)
		{
			return;
		}
		int[] abilityID = newFollower.AbilityID;
		for (int i = 0; i < abilityID.Length; i++)
		{
			int num = abilityID[i];
			GarrAbilityRec record = StaticDB.garrAbilityDB.GetRecord(num);
			if ((record.Flags & 1u) != 0u)
			{
				bool flag = true;
				int[] abilityID2 = oldFollower.AbilityID;
				for (int j = 0; j < abilityID2.Length; j++)
				{
					int num2 = abilityID2[j];
					if (num2 == num)
					{
						flag = false;
						break;
					}
				}
				if (flag)
				{
					AbilityDisplay[] componentsInChildren = this.m_equipmentSlotsRootObject.GetComponentsInChildren<AbilityDisplay>(true);
					AbilityDisplay[] array = componentsInChildren;
					for (int k = 0; k < array.Length; k++)
					{
						AbilityDisplay abilityDisplay = array[k];
						bool flag2 = true;
						int[] abilityID3 = newFollower.AbilityID;
						for (int l = 0; l < abilityID3.Length; l++)
						{
							int num3 = abilityID3[l];
							if (abilityDisplay.GetAbilityID() == num3)
							{
								flag2 = false;
								break;
							}
						}
						if (flag2)
						{
							Debug.Log(string.Concat(new object[]
							{
								"New ability is ",
								num,
								" replacing ability ID ",
								abilityDisplay.GetAbilityID()
							}));
							abilityDisplay.SetAbility(num, true, true, this);
							Main.instance.m_UISound.Play_UpgradeEquipment();
							UiAnimMgr.instance.PlayAnim("FlameGlowPulse", abilityDisplay.get_transform(), Vector3.get_zero(), 2f, 0f);
						}
					}
				}
			}
		}
	}

	public int GetCurrentFollower()
	{
		return this.m_garrFollowerID;
	}

	private void InitEquipmentSlots(JamGarrisonFollower follower)
	{
		AbilityDisplay[] componentsInChildren = this.m_equipmentSlotsRootObject.GetComponentsInChildren<AbilityDisplay>(true);
		for (int i = 0; i < componentsInChildren.Length; i++)
		{
			Object.DestroyImmediate(componentsInChildren[i].get_gameObject());
		}
		this.m_equipmentSlotsHeader.get_gameObject().SetActive(false);
		for (int j = 0; j < follower.AbilityID.Length; j++)
		{
			GarrAbilityRec record = StaticDB.garrAbilityDB.GetRecord(follower.AbilityID[j]);
			if ((record.Flags & 1u) != 0u)
			{
				this.m_equipmentSlotsHeader.get_gameObject().SetActive(true);
				GameObject gameObject = Object.Instantiate<GameObject>(this.m_equipmentSlotPrefab);
				gameObject.get_transform().SetParent(this.m_equipmentSlotsRootObject.get_transform(), false);
				AbilityDisplay component = gameObject.GetComponent<AbilityDisplay>();
				component.SetAbility(follower.AbilityID[j], true, true, this);
			}
		}
	}

	public void SetFollower(int followerID)
	{
		this.m_garrFollowerID = followerID;
		if (followerID == 0)
		{
			this.iLevelText.set_text(this.m_ilvlString + " ???");
			RectTransform[] componentsInChildren = this.traitsAndAbilitiesRootObject.GetComponentsInChildren<RectTransform>(true);
			for (int i = 0; i < componentsInChildren.Length; i++)
			{
				if (componentsInChildren[i] != null && componentsInChildren[i] != this.traitsAndAbilitiesRootObject.get_transform())
				{
					Object.DestroyImmediate(componentsInChildren[i].get_gameObject());
				}
			}
			AbilityDisplay[] componentsInChildren2 = this.m_equipmentSlotsRootObject.GetComponentsInChildren<AbilityDisplay>(true);
			for (int j = 0; j < componentsInChildren2.Length; j++)
			{
				Object.DestroyImmediate(componentsInChildren2[j].get_gameObject());
			}
		}
		if (!PersistentFollowerData.followerDictionary.ContainsKey(followerID))
		{
			return;
		}
		GarrFollowerRec record = StaticDB.garrFollowerDB.GetRecord(followerID);
		if (record == null)
		{
			return;
		}
		CreatureRec record2 = StaticDB.creatureDB.GetRecord((GarrisonStatus.Faction() != PVP_FACTION.HORDE) ? record.AllianceCreatureID : record.HordeCreatureID);
		if (record2 == null)
		{
			return;
		}
		JamGarrisonFollower jamGarrisonFollower = PersistentFollowerData.followerDictionary.get_Item(followerID);
		this.iLevelText.set_text(this.m_ilvlString + " " + (jamGarrisonFollower.ItemLevelWeapon + jamGarrisonFollower.ItemLevelArmor) / 2);
		string text = "Assets/BundleAssets/PortraitIcons/cid_" + record2.ID.ToString("D8") + ".png";
		Sprite sprite = AssetBundleManager.portraitIcons.LoadAsset<Sprite>(text);
		if (sprite != null)
		{
			this.followerSnapshot.set_sprite(sprite);
		}
		RectTransform[] componentsInChildren3 = this.traitsAndAbilitiesRootObject.GetComponentsInChildren<RectTransform>(true);
		for (int k = 0; k < componentsInChildren3.Length; k++)
		{
			if (componentsInChildren3[k] != null && componentsInChildren3[k] != this.traitsAndAbilitiesRootObject.get_transform())
			{
				Object.DestroyImmediate(componentsInChildren3[k].get_gameObject());
			}
		}
		bool flag = false;
		for (int l = 0; l < jamGarrisonFollower.AbilityID.Length; l++)
		{
			GarrAbilityRec record3 = StaticDB.garrAbilityDB.GetRecord(jamGarrisonFollower.AbilityID[l]);
			if ((record3.Flags & 512u) != 0u)
			{
				if (!flag)
				{
					GameObject gameObject = Object.Instantiate<GameObject>(this.specializationHeaderPrefab);
					gameObject.get_transform().SetParent(this.traitsAndAbilitiesRootObject.get_transform(), false);
					flag = true;
					Text component = gameObject.GetComponent<Text>();
					if (component != null)
					{
						component.set_text(StaticDB.GetString("SPECIALIZATION", null));
					}
				}
				GameObject gameObject2 = Object.Instantiate<GameObject>(this.abilityDisplayPrefab);
				gameObject2.get_transform().SetParent(this.traitsAndAbilitiesRootObject.get_transform(), false);
				AbilityDisplay component2 = gameObject2.GetComponent<AbilityDisplay>();
				component2.SetAbility(record3.ID, false, false, null);
			}
		}
		bool flag2 = false;
		for (int m = 0; m < jamGarrisonFollower.AbilityID.Length; m++)
		{
			GarrAbilityRec record4 = StaticDB.garrAbilityDB.GetRecord(jamGarrisonFollower.AbilityID[m]);
			if ((record4.Flags & 1u) == 0u)
			{
				if ((record4.Flags & 512u) == 0u)
				{
					if (!flag2)
					{
						GameObject gameObject3 = Object.Instantiate<GameObject>(this.abilitiesHeaderPrefab);
						gameObject3.get_transform().SetParent(this.traitsAndAbilitiesRootObject.get_transform(), false);
						flag2 = true;
						Text component3 = gameObject3.GetComponent<Text>();
						if (component3 != null)
						{
							component3.set_text(StaticDB.GetString("ABILITIES", null));
						}
					}
					GameObject gameObject4 = Object.Instantiate<GameObject>(this.abilityDisplayPrefab);
					gameObject4.get_transform().SetParent(this.traitsAndAbilitiesRootObject.get_transform(), false);
					AbilityDisplay component4 = gameObject4.GetComponent<AbilityDisplay>();
					component4.SetAbility(jamGarrisonFollower.AbilityID[m], false, false, null);
				}
			}
		}
		if (jamGarrisonFollower.ZoneSupportSpellID > 0)
		{
			GameObject gameObject5 = Object.Instantiate<GameObject>(this.zoneSupportAbilityHeaderPrefab);
			gameObject5.get_transform().SetParent(this.traitsAndAbilitiesRootObject.get_transform(), false);
			GameObject gameObject6 = Object.Instantiate<GameObject>(this.m_spellDisplayPrefab);
			gameObject6.get_transform().SetParent(this.traitsAndAbilitiesRootObject.get_transform(), false);
			SpellDisplay component5 = gameObject6.GetComponent<SpellDisplay>();
			component5.SetSpell(jamGarrisonFollower.ZoneSupportSpellID);
			Text componentInChildren = gameObject5.GetComponentInChildren<Text>();
			if (componentInChildren != null)
			{
				componentInChildren.set_text(StaticDB.GetString("COMBAT_ALLY", null));
			}
		}
		bool flag3 = (jamGarrisonFollower.Flags & 8) != 0;
		if (flag3)
		{
			GarrStringRec record5 = StaticDB.garrStringDB.GetRecord((GarrisonStatus.Faction() != PVP_FACTION.ALLIANCE) ? record.HordeFlavorGarrStringID : record.AllianceFlavorGarrStringID);
			if (record5 != null)
			{
				Text text2 = Object.Instantiate<Text>(this.m_troopDescriptionPrefab);
				text2.get_transform().SetParent(this.traitsAndAbilitiesRootObject.get_transform(), false);
				text2.set_text(record5.Text);
			}
		}
		this.InitEquipmentSlots(jamGarrisonFollower);
		this.UpdateChampionButtons(jamGarrisonFollower);
	}

	public void ShowChampionActivationConfirmationDialog()
	{
		AllPopups.instance.ShowChampionActivationConfirmationDialog(this);
	}

	public void ShowChampionDeactivationConfirmationDialog()
	{
		AllPopups.instance.ShowChampionDeactivationConfirmationDialog(this);
	}

	public void ActivateFollower()
	{
		Main.instance.m_UISound.Play_ActivateChampion();
		Debug.Log("Attempting to Activate follower " + this.m_garrFollowerID);
		MobilePlayerChangeFollowerActive mobilePlayerChangeFollowerActive = new MobilePlayerChangeFollowerActive();
		mobilePlayerChangeFollowerActive.SetInactive = false;
		mobilePlayerChangeFollowerActive.GarrFollowerID = this.m_garrFollowerID;
		Login.instance.SendToMobileServer(mobilePlayerChangeFollowerActive);
	}

	public void DeactivateFollower()
	{
		Main.instance.m_UISound.Play_DeactivateChampion();
		Debug.Log("Attempting to Deactivate follower " + this.m_garrFollowerID);
		MobilePlayerChangeFollowerActive mobilePlayerChangeFollowerActive = new MobilePlayerChangeFollowerActive();
		mobilePlayerChangeFollowerActive.SetInactive = true;
		mobilePlayerChangeFollowerActive.GarrFollowerID = this.m_garrFollowerID;
		Login.instance.SendToMobileServer(mobilePlayerChangeFollowerActive);
	}

	private void UpdateChampionButtons(JamGarrisonFollower follower)
	{
		if (this.m_activateChampionButton == null || this.m_deactivateChampionButton == null)
		{
			return;
		}
		bool flag = (follower.Flags & 8) != 0;
		if (flag)
		{
			this.m_activateChampionButton.SetActive(false);
			this.m_deactivateChampionButton.SetActive(false);
		}
		else
		{
			bool flag2 = (follower.Flags & 4) != 0;
			bool flag3 = GarrisonStatus.GetRemainingFollowerActivations() > 0;
			this.m_activateChampionButton.SetActive(flag2 && flag3);
			int numActiveChampions = GeneralHelpers.GetNumActiveChampions();
			int maxActiveChampions = GeneralHelpers.GetMaxActiveChampions();
			this.m_deactivateChampionButton.SetActive(!flag2 && numActiveChampions > maxActiveChampions);
		}
	}
}

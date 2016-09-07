using System;
using UnityEngine;
using UnityEngine.UI;
using WowJamMessages.MobileClientJSON;

public class AllPopups : MonoBehaviour
{
	public static AllPopups instance;

	public AbilityInfoPopup m_abilityInfoPopup;

	public RewardInfoPopup m_rewardInfoPopup;

	public GameObject m_cheatCompleteMissionPopup;

	public DebugOptionsPopup m_debugOptionsPopup;

	public ChatPopup m_chatPopup;

	public EmissaryPopup m_emissaryPopup;

	public MechanicInfoPopup m_mechanicInfoPopup;

	public OptionsDialog m_optionsDialog;

	public ConnectionPopup m_connectionPopup;

	public TalentTooltip m_talentTooltip;

	public BountyInfoTooltip m_bountyInfoTooltip;

	public WorldQuestTooltip m_worldQuestTooltip;

	public GenericPopup m_genericPopup;

	public MissionDescriptionTooltip m_missionDescriptionTooltip;

	public ArmamentDialog m_armamentDialog;

	public EquipmentDialog m_equipmentDialog;

	public ActivationConfirmationDialog m_championActivationConfirmationDialog;

	public DeactivationConfirmationDialog m_championDeactivationConfirmationDialog;

	public CombatAllyDialog m_combatAllyDialog;

	public PartyBuffsPopup m_partyBuffsPopup;

	public UnassignCombatAllyConfirmationDialog m_unassignCombatAllyConfirmationDialog;

	public LogoutConfirmation m_logoutConfirmation;

	public RegionConfirmation m_regionConfirmation;

	private FollowerDetailView m_currentFollowerDetailView;

	private void Awake()
	{
		AllPopups.instance = this;
	}

	private void Start()
	{
		this.HideAllPopups();
	}

	private void Update()
	{
	}

	public void HideAllPopups()
	{
		this.m_abilityInfoPopup.get_gameObject().SetActive(false);
		this.m_rewardInfoPopup.get_gameObject().SetActive(false);
		this.m_cheatCompleteMissionPopup.get_gameObject().SetActive(false);
		this.m_debugOptionsPopup.get_gameObject().SetActive(false);
		this.m_chatPopup.get_gameObject().SetActive(false);
		this.m_emissaryPopup.get_gameObject().SetActive(false);
		this.m_mechanicInfoPopup.get_gameObject().SetActive(false);
		this.m_optionsDialog.get_gameObject().SetActive(false);
		this.m_connectionPopup.get_gameObject().SetActive(false);
		this.m_bountyInfoTooltip.get_gameObject().SetActive(false);
		this.m_worldQuestTooltip.get_gameObject().SetActive(false);
		this.m_genericPopup.get_gameObject().SetActive(false);
		this.m_missionDescriptionTooltip.get_gameObject().SetActive(false);
		this.m_armamentDialog.get_gameObject().SetActive(false);
		this.m_equipmentDialog.get_gameObject().SetActive(false);
		this.m_championActivationConfirmationDialog.get_gameObject().SetActive(false);
		this.m_championDeactivationConfirmationDialog.get_gameObject().SetActive(false);
		this.m_partyBuffsPopup.get_gameObject().SetActive(false);
		this.m_unassignCombatAllyConfirmationDialog.get_gameObject().SetActive(false);
		this.m_logoutConfirmation.get_gameObject().SetActive(false);
		this.m_regionConfirmation.get_gameObject().SetActive(false);
		this.m_talentTooltip.get_gameObject().SetActive(false);
		this.HideCombatAllyDialog();
	}

	public void ShowUnassignCombatAllyConfirmationDialog()
	{
		this.m_unassignCombatAllyConfirmationDialog.Show();
	}

	public void ShowPartyBuffsPopup(int[] buffIDs)
	{
		this.m_partyBuffsPopup.get_gameObject().SetActive(true);
		this.m_partyBuffsPopup.Init(buffIDs);
	}

	public void ShowCombatAllyDialog()
	{
		this.m_combatAllyDialog.get_gameObject().SetActive(true);
		this.m_combatAllyDialog.Init();
	}

	public void HideCombatAllyDialog()
	{
		this.m_combatAllyDialog.get_gameObject().SetActive(false);
	}

	public void ShowMissionDescriptionTooltip(int garrMissionID)
	{
		this.HideAllPopups();
		this.m_missionDescriptionTooltip.get_gameObject().SetActive(true);
		this.m_missionDescriptionTooltip.SetMission(garrMissionID);
	}

	public void ShowWorldQuestTooltip(int questID)
	{
		this.HideAllPopups();
		this.m_worldQuestTooltip.get_gameObject().SetActive(true);
		this.m_worldQuestTooltip.SetQuest(questID);
	}

	public void ShowBountyInfoTooltip(MobileWorldQuestBounty bounty)
	{
		this.HideAllPopups();
		this.m_bountyInfoTooltip.get_gameObject().SetActive(true);
		this.m_bountyInfoTooltip.SetBounty(bounty);
	}

	public void ShowRewardTooltip(MissionRewardDisplay.RewardType rewardType, int rewardID, int rewardQuantity, Image rewardImage, int itemContext)
	{
		this.m_rewardInfoPopup.get_gameObject().SetActive(true);
		this.m_rewardInfoPopup.SetReward(rewardType, rewardID, rewardQuantity, rewardImage.get_sprite(), itemContext);
	}

	public void ShowAbilityInfoPopup(int garrAbilityID)
	{
		this.HideAllPopups();
		this.m_abilityInfoPopup.get_gameObject().SetActive(true);
		this.m_abilityInfoPopup.SetAbility(garrAbilityID);
	}

	public void ShowSpellInfoPopup(int spellID)
	{
		this.HideAllPopups();
		this.m_abilityInfoPopup.get_gameObject().SetActive(true);
		this.m_abilityInfoPopup.SetSpell(spellID);
	}

	public void ShowMechanicInfoPopup(Image mechanicImage, string mechanicName, string mechanicDescription)
	{
		this.HideAllPopups();
		this.m_mechanicInfoPopup.get_gameObject().SetActive(true);
		this.m_mechanicInfoPopup.m_mechanicIcon.set_sprite(mechanicImage.get_sprite());
		this.m_mechanicInfoPopup.m_mechanicIcon.set_overrideSprite(mechanicImage.get_overrideSprite());
		this.m_mechanicInfoPopup.m_mechanicName.set_text(mechanicName);
		this.m_mechanicInfoPopup.m_mechanicDescription.set_text(mechanicDescription);
	}

	public void ShowOptionsMenuPopup()
	{
		this.HideAllPopups();
		this.m_debugOptionsPopup.get_gameObject().SetActive(true);
		Main.instance.m_UISound.Play_ButtonBlackClick();
	}

	public void ShowOptionsDialog()
	{
		this.HideAllPopups();
		this.m_optionsDialog.get_gameObject().SetActive(true);
	}

	public void ShowChatPopup()
	{
		this.HideAllPopups();
		this.m_chatPopup.get_gameObject().SetActive(true);
	}

	public void ShowEmissaryPopup()
	{
		this.HideAllPopups();
		this.m_emissaryPopup.get_gameObject().SetActive(true);
		Main.instance.RequestEmissaryFactions();
	}

	public void EmissaryFactionUpdate(MobileClientEmissaryFactionUpdate msg)
	{
		this.m_emissaryPopup.FactionUpdate(msg);
	}

	public void OnClickConnectionPopupClose()
	{
		this.m_connectionPopup.get_gameObject().SetActive(false);
	}

	public void ShowTalentTooltip(TalentTreeItemAbilityButton abilityButton)
	{
		this.m_talentTooltip.get_gameObject().SetActive(true);
		this.m_talentTooltip.SetTalent(abilityButton);
	}

	public void ShowGenericPopup(string headerText, string descriptionText)
	{
		this.HideAllPopups();
		this.m_genericPopup.SetText(headerText, descriptionText);
		this.m_genericPopup.get_gameObject().SetActive(true);
	}

	public void ShowGenericPopupFull(string fullText)
	{
		this.HideAllPopups();
		this.m_genericPopup.SetFullText(fullText);
		this.m_genericPopup.get_gameObject().SetActive(true);
	}

	public void SetCurrentFollowerDetailView(FollowerDetailView followerDetailView)
	{
		this.m_currentFollowerDetailView = followerDetailView;
	}

	public FollowerDetailView GetCurrentFollowerDetailView()
	{
		return this.m_currentFollowerDetailView;
	}

	public void HideChampionUpgradeDialogs()
	{
		this.m_armamentDialog.get_gameObject().SetActive(false);
		this.m_equipmentDialog.get_gameObject().SetActive(false);
	}

	public void ShowArmamentDialog(FollowerDetailView followerDetailView, bool show)
	{
		if (show)
		{
			this.m_armamentDialog.Init(followerDetailView);
		}
		this.m_armamentDialog.get_gameObject().SetActive(show);
	}

	public void ShowEquipmentDialog(int garrAbilityID, FollowerDetailView followerDetailView, bool show)
	{
		if (show)
		{
			this.m_equipmentDialog.SetAbility(garrAbilityID, followerDetailView);
		}
		this.m_equipmentDialog.get_gameObject().SetActive(show);
	}

	public void ShowChampionActivationConfirmationDialog(FollowerDetailView followerDetailView)
	{
		this.m_championActivationConfirmationDialog.Show(followerDetailView);
	}

	public void ShowChampionDeactivationConfirmationDialog(FollowerDetailView followerDetailView)
	{
		this.m_championDeactivationConfirmationDialog.Show(followerDetailView);
	}

	public void ShowLogoutConfirmationPopup()
	{
		this.m_logoutConfirmation.get_gameObject().SetActive(true);
	}

	public void ShowRegionConfirmationPopup(int index)
	{
		this.m_regionConfirmation.get_gameObject().SetActive(true);
	}
}

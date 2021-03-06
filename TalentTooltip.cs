using System;
using UnityEngine;
using UnityEngine.UI;
using WowStatConstants;
using WowStaticData;

public class TalentTooltip : MonoBehaviour
{
	[Header("Common")]
	public TalentTreePanel m_talentTreePanel;

	public Image m_abilityIcon;

	public Text m_talentName;

	public Text m_talentDescription;

	public GameObject m_yourResourcesDisplayObj;

	[Header("Research Time And Cost")]
	public GameObject m_researchTimeAndCostSection;

	public Text m_researchOrRespecText;

	public Image m_resourceIcon;

	public Text m_resourceCostText;

	public Text m_researchDurationText;

	[Header("Available For Research")]
	public GameObject m_availableForResearchSection;

	[Header("Unavailable For Research")]
	public GameObject m_unavailableForResearchSection;

	public Text m_statusText;

	private TalentTreeItemAbilityButton m_abilityButton;

	private GarrTalentRec m_garrTalentRec;

	private void Start()
	{
		this.m_talentName.set_font(GeneralHelpers.LoadStandardFont());
		this.m_talentDescription.set_font(GeneralHelpers.LoadStandardFont());
		this.m_researchOrRespecText.set_font(GeneralHelpers.LoadStandardFont());
		this.m_resourceCostText.set_font(GeneralHelpers.LoadStandardFont());
		this.m_researchDurationText.set_font(GeneralHelpers.LoadStandardFont());
		this.m_statusText.set_font(GeneralHelpers.LoadStandardFont());
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

	public void SetTalent(TalentTreeItemAbilityButton abilityButton)
	{
		this.m_abilityButton = abilityButton;
		this.m_garrTalentRec = StaticDB.garrTalentDB.GetRecord(abilityButton.GetTalentID());
		this.m_talentName.set_text(this.m_garrTalentRec.Name);
		this.m_talentDescription.set_text(WowTextParser.parser.Parse(this.m_garrTalentRec.Description, 0));
		this.m_talentDescription.set_supportRichText(WowTextParser.parser.IsRichText());
		Sprite sprite = GeneralHelpers.LoadIconAsset(AssetBundleType.Icons, this.m_garrTalentRec.IconFileDataID);
		if (sprite != null)
		{
			this.m_abilityIcon.set_sprite(sprite);
		}
		this.m_researchTimeAndCostSection.SetActive(false);
		int num = (!abilityButton.CanRespec()) ? this.m_garrTalentRec.ResearchCost : this.m_garrTalentRec.RespecCost;
		this.m_resourceCostText.set_text(((GarrisonStatus.Resources() >= num) ? "<color=#ffffffff>" : "<color=#FF0000FF>") + ((!abilityButton.CanRespec()) ? this.m_garrTalentRec.ResearchCost : this.m_garrTalentRec.RespecCost) + "</color>");
		Sprite sprite2 = GeneralHelpers.LoadCurrencyIcon((int)this.m_garrTalentRec.ResearchCostCurrencyTypesID);
		if (sprite2 != null)
		{
			this.m_resourceIcon.set_sprite(sprite2);
		}
		Duration duration = new Duration((!abilityButton.CanRespec()) ? this.m_garrTalentRec.ResearchDurationSecs : this.m_garrTalentRec.RespecDurationSecs);
		this.m_researchDurationText.set_text(duration.DurationString);
		this.m_yourResourcesDisplayObj.SetActive(false);
		if (abilityButton.CanResearch() || abilityButton.CanRespec())
		{
			this.m_availableForResearchSection.SetActive(true);
			this.m_unavailableForResearchSection.SetActive(false);
			this.m_researchOrRespecText.set_text((!abilityButton.CanRespec()) ? StaticDB.GetString("RESEARCH", null) : StaticDB.GetString("RESPEC", null));
			this.m_yourResourcesDisplayObj.SetActive(true);
			this.m_researchTimeAndCostSection.SetActive(true);
		}
		else
		{
			this.m_availableForResearchSection.SetActive(false);
			this.m_unavailableForResearchSection.SetActive(true);
			if (this.m_abilityButton.IsOwned())
			{
				this.m_yourResourcesDisplayObj.SetActive(false);
				this.m_statusText.set_text("<color=#ffffffff>" + StaticDB.GetString("TALENT_OWNED", null) + "</color>");
			}
			else if (this.m_abilityButton.IsResearching())
			{
				Duration duration2 = new Duration((!abilityButton.IsRespec()) ? ((int)this.m_abilityButton.GetRemainingResearchTime()) : ((int)this.m_abilityButton.GetRemainingRespecTime()));
				this.m_statusText.set_text(string.Concat(new string[]
				{
					"<color=#FFC600FF>",
					StaticDB.GetString("TIME_REMAINING", null),
					"</color> <color=#ffffffff>",
					duration2.DurationString,
					"</color>"
				}));
			}
			else if (GarrisonStatus.Resources() < num)
			{
				this.m_researchTimeAndCostSection.SetActive(true);
				this.m_yourResourcesDisplayObj.SetActive(true);
				this.m_statusText.set_text("<color=#FF0000FF>" + StaticDB.GetString("NEED_MORE_RESOURCES", null) + "</color>");
			}
			else if (this.m_talentTreePanel.AnyTalentIsResearching())
			{
				this.m_statusText.set_text("<color=#FF0000FF>" + StaticDB.GetString("ALREADY_RESEARCHING", null) + "</color>");
			}
			else
			{
				string whyCantResearch = this.m_abilityButton.GetWhyCantResearch();
				if (whyCantResearch != null && whyCantResearch != string.Empty)
				{
					this.m_statusText.set_text("<color=#FF0000FF>" + whyCantResearch + "</color>");
				}
				else
				{
					this.m_statusText.set_text("<color=#FF0000FF>" + StaticDB.GetString("MUST_RESEARCH_PREVIOUS_TIER", null) + "</color>");
				}
			}
		}
	}

	public void StartResearch()
	{
		this.m_abilityButton.StartResearch();
		base.get_gameObject().SetActive(false);
	}
}

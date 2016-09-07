using System;
using UnityEngine;
using UnityEngine.UI;
using WowStatConstants;

public class DeactivationConfirmationDialog : MonoBehaviour
{
	public Text m_areYouSureLabel;

	public Text m_reactivationCostLabel;

	public Text m_cancelButtonLabel;

	public Text m_okButtonLabel;

	public Text m_reactivationCostText;

	private FollowerDetailView m_followerDetailView;

	private void OnEnable()
	{
		Main.instance.m_UISound.Play_ShowGenericTooltip();
		Main.instance.m_canvasBlurManager.AddBlurRef_MainCanvas();
		Main.instance.m_backButtonManager.PushBackAction(BackAction.hideAllPopups, null);
	}

	private void OnDisable()
	{
		Main.instance.m_canvasBlurManager.RemoveBlurRef_MainCanvas();
		Main.instance.m_backButtonManager.PopBackAction();
	}

	private void Start()
	{
		this.m_areYouSureLabel.set_font(GeneralHelpers.LoadStandardFont());
		this.m_reactivationCostLabel.set_font(GeneralHelpers.LoadStandardFont());
		this.m_cancelButtonLabel.set_font(GeneralHelpers.LoadStandardFont());
		this.m_okButtonLabel.set_font(GeneralHelpers.LoadStandardFont());
		this.m_reactivationCostText.set_font(GeneralHelpers.LoadStandardFont());
	}

	public void Show(FollowerDetailView followerDetailView)
	{
		base.get_gameObject().SetActive(true);
		this.m_followerDetailView = followerDetailView;
		this.m_areYouSureLabel.set_text(StaticDB.GetString("ARE_YOU_SURE", null));
		this.m_reactivationCostLabel.set_text(StaticDB.GetString("CHAMPION_REACTIVATION_COST", null));
		this.m_cancelButtonLabel.set_text(StaticDB.GetString("NO", null));
		this.m_okButtonLabel.set_text(StaticDB.GetString("YES_DEACTIVATE", null));
		this.m_reactivationCostText.set_text(string.Empty + GarrisonStatus.GetFollowerActivationGoldCost());
	}

	public void ConfirmDeactivate()
	{
		this.m_followerDetailView.DeactivateFollower();
		base.get_gameObject().SetActive(false);
	}
}

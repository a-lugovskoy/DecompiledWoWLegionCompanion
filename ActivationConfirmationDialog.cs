using System;
using UnityEngine;
using UnityEngine.UI;
using WowStatConstants;

public class ActivationConfirmationDialog : MonoBehaviour
{
	public Text m_areYouSureLabel;

	public Text m_activationsRemainingLabel;

	public Text m_activationCostLabel;

	public Text m_cancelButtonLabel;

	public Text m_okButtonLabel;

	public Button m_okButton;

	public Text m_activationsRemainingText;

	public Text m_activationCostText;

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

	public void Start()
	{
		this.m_areYouSureLabel.set_font(GeneralHelpers.LoadStandardFont());
		this.m_activationsRemainingLabel.set_font(GeneralHelpers.LoadStandardFont());
		this.m_activationCostLabel.set_font(GeneralHelpers.LoadStandardFont());
		this.m_cancelButtonLabel.set_font(GeneralHelpers.LoadStandardFont());
		this.m_okButtonLabel.set_font(GeneralHelpers.LoadStandardFont());
		this.m_activationsRemainingText.set_font(GeneralHelpers.LoadStandardFont());
		this.m_activationCostText.set_font(GeneralHelpers.LoadStandardFont());
	}

	public void Show(FollowerDetailView followerDetailView)
	{
		base.get_gameObject().SetActive(true);
		this.m_followerDetailView = followerDetailView;
		this.m_areYouSureLabel.set_text(StaticDB.GetString("ARE_YOU_SURE", null));
		this.m_activationsRemainingLabel.set_text(StaticDB.GetString("ACTIVATIONS_LEFT_TODAY", null));
		this.m_activationCostLabel.set_text(StaticDB.GetString("CHAMPION_ACTIVATION_COST", null));
		this.m_cancelButtonLabel.set_text(StaticDB.GetString("NO", null));
		this.m_okButtonLabel.set_text(StaticDB.GetString("YES_ACTIVATE", null));
		if (GarrisonStatus.Gold() < 250)
		{
			this.m_okButtonLabel.set_text(StaticDB.GetString("CANT_AFFORD", null));
			this.m_okButton.set_interactable(false);
		}
		else
		{
			this.m_okButton.set_interactable(true);
		}
		this.m_activationsRemainingText.set_text(string.Empty + GarrisonStatus.GetRemainingFollowerActivations());
		this.m_activationCostText.set_text(string.Empty + GarrisonStatus.GetFollowerActivationGoldCost());
	}

	public void ConfirmActivate()
	{
		this.m_followerDetailView.ActivateFollower();
		base.get_gameObject().SetActive(false);
	}
}

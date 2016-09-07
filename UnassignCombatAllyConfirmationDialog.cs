using System;
using UnityEngine;
using UnityEngine.UI;
using WowStatConstants;

public class UnassignCombatAllyConfirmationDialog : MonoBehaviour
{
	public Text m_areYouSureLabel;

	public Text m_cancelButtonLabel;

	public Text m_okButtonLabel;

	public CombatAllyListItem m_combatAllyListItem;

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
		this.m_cancelButtonLabel.set_font(GeneralHelpers.LoadStandardFont());
		this.m_okButtonLabel.set_font(GeneralHelpers.LoadStandardFont());
	}

	public void Show()
	{
		base.get_gameObject().SetActive(true);
		this.m_areYouSureLabel.set_text(StaticDB.GetString("ARE_YOU_SURE", null));
		this.m_cancelButtonLabel.set_text(StaticDB.GetString("NO", null));
		this.m_okButtonLabel.set_text(StaticDB.GetString("YES_UNASSIGN", "Yes, Unassign!"));
	}

	public void ConfirmUnassign()
	{
		this.m_combatAllyListItem.UnassignCombatAlly();
		base.get_gameObject().SetActive(false);
	}
}

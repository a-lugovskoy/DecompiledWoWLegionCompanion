using System;
using UnityEngine;
using UnityEngine.UI;
using WowStatConstants;

public class LogoutConfirmation : MonoBehaviour
{
	public Text m_logoutText;

	public Text m_sureText;

	public Text m_okayText;

	public Text m_cancelText;

	private void OnEnable()
	{
		this.m_logoutText.set_text(StaticDB.GetString("LOG_OUT", null));
		this.m_sureText.set_text(StaticDB.GetString("ARE_YOU_SURE", null));
		this.m_okayText.set_text(StaticDB.GetString("OK", null));
		this.m_cancelText.set_text(StaticDB.GetString("CANCEL", null));
		Main.instance.m_canvasBlurManager.AddBlurRef_MainCanvas();
		Main.instance.m_backButtonManager.PushBackAction(BackAction.hideAllPopups, null);
	}

	private void OnDisable()
	{
		Main.instance.m_canvasBlurManager.RemoveBlurRef_MainCanvas();
		Main.instance.m_backButtonManager.PopBackAction();
	}

	public void OnClickOkay()
	{
		AllPopups.instance.HideAllPopups();
		Login.instance.BackToAccountSelect();
	}

	public void OnClickCancel()
	{
		AllPopups.instance.HideAllPopups();
	}
}

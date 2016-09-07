using bnet.protocol;
using System;
using UnityEngine;
using UnityEngine.UI;

public class BnGameAccountButton : MonoBehaviour
{
	public Text m_buttonText;

	private EntityId m_gameAccount;

	private string m_accountName;

	public void SetInfo(EntityId gameAccount, string accountName, bool isBanned, bool isSuspended)
	{
		this.m_gameAccount = gameAccount;
		this.m_accountName = accountName;
		this.m_buttonText.set_text(this.m_accountName);
	}

	public void OnClick()
	{
		Login.instance.SelectGameAccount(this.m_gameAccount);
	}

	public void PlayClickSound()
	{
		Main.instance.m_UISound.Play_ButtonBlackClick();
	}
}

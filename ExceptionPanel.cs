using System;
using UnityEngine;
using UnityEngine.UI;

public class ExceptionPanel : MonoBehaviour
{
	public Text m_exceptionText;

	public void OnDismiss()
	{
		base.get_gameObject().SetActive(false);
	}

	public void SetExceptionText(string text)
	{
		this.m_exceptionText.set_text(text);
	}
}

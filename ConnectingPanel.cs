using System;
using UnityEngine;
using UnityEngine.UI;

public class ConnectingPanel : MonoBehaviour
{
	public Text m_statusText;

	public Text m_cancelText;

	public GameObject m_cancelButton;

	public float m_fadeInDuration;

	public float m_fadeOutDuration;

	public CanvasGroup[] m_fadeCanvasGroups;

	private bool m_isFadingIn;

	private bool m_isFadingOut;

	private float m_fadeInTimeElapsed;

	private float m_fadeOutTimeElapsed;

	private void Start()
	{
		this.m_cancelText.set_font(GeneralHelpers.LoadStandardFont());
	}

	private void OnEnable()
	{
		this.m_isFadingIn = true;
		this.m_isFadingOut = false;
		this.m_fadeInTimeElapsed = 0f;
		CanvasGroup[] fadeCanvasGroups = this.m_fadeCanvasGroups;
		for (int i = 0; i < fadeCanvasGroups.Length; i++)
		{
			CanvasGroup canvasGroup = fadeCanvasGroups[i];
			canvasGroup.set_alpha(0f);
		}
		if (StaticDB.StringsAvailable())
		{
			this.m_cancelText.set_text(StaticDB.GetString("CANCEL", null));
		}
	}

	public void Hide()
	{
		this.m_isFadingIn = false;
		this.m_isFadingOut = true;
		this.m_fadeOutTimeElapsed = 0f;
	}

	private void Update()
	{
		if (StaticDB.StringsAvailable())
		{
			if (!this.m_cancelButton.get_activeSelf())
			{
				this.m_cancelButton.SetActive(true);
				this.m_cancelText.set_text(StaticDB.GetString("CANCEL", null));
			}
		}
		else if (this.m_cancelButton.get_activeSelf())
		{
			this.m_cancelButton.SetActive(false);
		}
		if (this.m_isFadingIn && this.m_fadeInTimeElapsed < this.m_fadeInDuration)
		{
			this.m_fadeInTimeElapsed += Time.get_deltaTime();
			float alpha = Mathf.Clamp(this.m_fadeInTimeElapsed / this.m_fadeInDuration, 0f, 1f);
			CanvasGroup[] fadeCanvasGroups = this.m_fadeCanvasGroups;
			for (int i = 0; i < fadeCanvasGroups.Length; i++)
			{
				CanvasGroup canvasGroup = fadeCanvasGroups[i];
				canvasGroup.set_alpha(alpha);
			}
		}
		if (this.m_isFadingOut && this.m_fadeOutTimeElapsed < this.m_fadeOutDuration)
		{
			this.m_fadeOutTimeElapsed += Time.get_deltaTime();
			float alpha2 = 1f - Mathf.Clamp(this.m_fadeOutTimeElapsed / this.m_fadeOutDuration, 0f, 1f);
			CanvasGroup[] fadeCanvasGroups2 = this.m_fadeCanvasGroups;
			for (int j = 0; j < fadeCanvasGroups2.Length; j++)
			{
				CanvasGroup canvasGroup2 = fadeCanvasGroups2[j];
				canvasGroup2.set_alpha(alpha2);
			}
			if (this.m_fadeOutTimeElapsed > this.m_fadeOutDuration)
			{
				this.m_isFadingOut = false;
				base.get_gameObject().SetActive(false);
			}
		}
	}
}

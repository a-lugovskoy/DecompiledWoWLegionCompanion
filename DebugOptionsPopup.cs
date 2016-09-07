using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using WowStatConstants;

public class DebugOptionsPopup : MonoBehaviour
{
	public Toggle m_enableDetailedZoneMaps;

	public Toggle m_enableAutoZoomInOut;

	public Toggle m_enableTapToZoomOut;

	public Toggle m_enableCheatCompleteMissionButton;

	public GameObject m_cheatCompleteButton;

	public Dropdown m_localeDropdown;

	private void Start()
	{
		this.m_enableDetailedZoneMaps.onValueChanged.AddListener(new UnityAction<bool>(this.OnValueChanged_EnableDetailedZoneMaps));
		this.m_enableAutoZoomInOut.onValueChanged.AddListener(new UnityAction<bool>(this.OnValueChanged_EnableAutoZoomInOut));
		this.m_enableTapToZoomOut.onValueChanged.AddListener(new UnityAction<bool>(this.OnValueChanged_EnableTapToZoomOut));
		this.m_enableCheatCompleteMissionButton.onValueChanged.AddListener(new UnityAction<bool>(this.OnValueChanged_EnableCheatCompleteButton));
	}

	private void OnEnable()
	{
		this.m_enableDetailedZoneMaps.set_isOn(AdventureMapPanel.instance.m_testEnableDetailedZoneMaps);
		this.m_enableAutoZoomInOut.set_isOn(AdventureMapPanel.instance.m_testEnableAutoZoomInOut);
		this.m_enableTapToZoomOut.set_isOn(AdventureMapPanel.instance.m_testEnableTapToZoomOut);
		this.m_enableCheatCompleteMissionButton.set_isOn(this.m_cheatCompleteButton.get_activeSelf());
		for (int i = 0; i < this.m_localeDropdown.get_options().get_Count(); i++)
		{
			if (this.m_localeDropdown.get_options().ToArray()[i].get_text() == Main.instance.GetLocale())
			{
				this.m_localeDropdown.set_value(i);
				break;
			}
		}
		Main.instance.m_backButtonManager.PushBackAction(BackAction.hideAllPopups, null);
	}

	private void OnDisable()
	{
		Main.instance.m_backButtonManager.PopBackAction();
	}

	private void OnValueChanged_EnableDetailedZoneMaps(bool isOn)
	{
		AdventureMapPanel.instance.m_testEnableDetailedZoneMaps = isOn;
	}

	private void OnValueChanged_EnableAutoZoomInOut(bool isOn)
	{
		AdventureMapPanel.instance.m_testEnableAutoZoomInOut = isOn;
	}

	private void OnValueChanged_EnableTapToZoomOut(bool isOn)
	{
		AdventureMapPanel.instance.m_testEnableTapToZoomOut = isOn;
	}

	private void OnValueChanged_EnableCheatCompleteButton(bool isOn)
	{
		this.m_cheatCompleteButton.SetActive(isOn);
	}

	public void OnValueChanged_LocaleDropdown(int index)
	{
		string text = this.m_localeDropdown.get_options().ToArray()[this.m_localeDropdown.get_value()].get_text();
		Debug.Log("Locale option is now " + text);
		SecurePlayerPrefs.SetString("locale", text, Main.uniqueIdentifier);
		PlayerPrefs.Save();
	}
}

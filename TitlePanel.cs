using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TitlePanel : MonoBehaviour
{
	public GameObject m_legionLogo;

	public GameObject m_legionLogo_CN;

	public GameObject m_legionLogo_TW;

	public GameObject m_legionLogoGlowRoot;

	public Text m_buildText;

	public Text m_briefBuildText;

	public Text m_loginButtonText;

	public Text m_legalText;

	public Dropdown m_portalDropdown;

	public Button m_resumeButton;

	private bool m_showDialog = true;

	private string[] m_regionOptions;

	public int CancelIndex
	{
		get;
		set;
	}

	private void Start()
	{
		if (Login.instance.IsDevRegionList())
		{
			this.m_regionOptions = new string[]
			{
				"WoW-Dev",
				"Beta",
				"ST-US",
				"ST-EU",
				"ST-KR",
				"ST-CN",
				"US",
				"EU",
				"CN",
				"KR"
			};
		}
		else
		{
			this.m_regionOptions = new string[5];
		}
		this.m_briefBuildText.set_text(string.Format("{0:D2}", BuildNum.CodeBuildNum));
		this.m_buildText.set_text(string.Empty);
		this.m_loginButtonText.set_font(GeneralHelpers.LoadStandardFont());
		this.m_loginButtonText.set_text(StaticDB.GetString("LOGIN", null));
		this.m_legalText.set_font(GeneralHelpers.LoadStandardFont());
		this.m_legalText.set_text(StaticDB.GetString("LEGAL_TEXT", "(c) 2016 Blizzard Entertainment, Inc. All rights reserved."));
		List<Dropdown.OptionData> list = new List<Dropdown.OptionData>();
		if (!Login.instance.IsDevRegionList())
		{
			this.m_regionOptions[0] = StaticDB.GetString("AMERICAS_AND_OCEANIC", "Americas and Oceanic");
			this.m_regionOptions[1] = StaticDB.GetString("EUROPE", "Europe");
			this.m_regionOptions[2] = StaticDB.GetString("KOREA_AND_TAIWAN", "Korea and Taiwan");
			this.m_regionOptions[3] = StaticDB.GetString("CHINA", "China");
		}
		for (int i = 0; i < this.m_regionOptions.Length; i++)
		{
			Dropdown.OptionData optionData = new Dropdown.OptionData();
			optionData.set_text(this.m_regionOptions[i]);
			list.Add(optionData);
		}
		this.m_portalDropdown.ClearOptions();
		this.m_portalDropdown.AddOptions(list);
	}

	private void Update()
	{
	}

	private void OnEnable()
	{
		this.m_legionLogo.SetActive(false);
		this.m_legionLogo_CN.SetActive(false);
		this.m_legionLogo_TW.SetActive(false);
		string locale = Main.instance.GetLocale();
		if (locale == "zhCN")
		{
			this.m_legionLogo_CN.SetActive(true);
		}
		else if (locale == "zhTW")
		{
			this.m_legionLogo_TW.SetActive(true);
		}
		else
		{
			this.m_legionLogo.SetActive(true);
		}
		if (Login.instance.IsDevRegionList())
		{
			for (int i = 0; i < this.m_portalDropdown.get_options().get_Count(); i++)
			{
				if (this.m_portalDropdown.get_options().ToArray()[i].get_text().ToLower() == Login.instance.GetBnPortal())
				{
					this.m_showDialog = false;
					this.m_portalDropdown.set_value(i);
					this.m_showDialog = true;
					break;
				}
			}
		}
		else
		{
			int value = 0;
			string bnPortal = Login.instance.GetBnPortal();
			if (bnPortal != null)
			{
				if (TitlePanel.<>f__switch$map8 == null)
				{
					Dictionary<string, int> dictionary = new Dictionary<string, int>(4);
					dictionary.Add("us", 0);
					dictionary.Add("eu", 1);
					dictionary.Add("kr", 2);
					dictionary.Add("cn", 3);
					TitlePanel.<>f__switch$map8 = dictionary;
				}
				int num;
				if (TitlePanel.<>f__switch$map8.TryGetValue(bnPortal, ref num))
				{
					switch (num)
					{
					case 0:
						value = 0;
						break;
					case 1:
						value = 1;
						break;
					case 2:
						value = 2;
						break;
					case 3:
						value = 3;
						break;
					}
				}
			}
			this.m_showDialog = false;
			this.m_portalDropdown.set_value(value);
			this.m_showDialog = true;
		}
		this.CancelIndex = this.m_portalDropdown.get_value();
	}

	public void PortalDropdownChanged(int index)
	{
		Debug.Log(string.Concat(new object[]
		{
			"New index ",
			index,
			", cancelIndex ",
			this.CancelIndex
		}));
		if (this.m_showDialog)
		{
			AllPopups.instance.ShowRegionConfirmationPopup(index);
		}
	}

	private string GetDropdownPortalText()
	{
		string result;
		if (!Login.instance.IsDevRegionList())
		{
			switch (this.m_portalDropdown.get_value())
			{
			case 0:
				IL_62:
				result = "us";
				return result;
			case 1:
				result = "eu";
				return result;
			case 2:
				result = "kr";
				return result;
			case 3:
				result = "cn";
				return result;
			}
			goto IL_62;
		}
		result = this.m_portalDropdown.get_options().ToArray()[this.m_portalDropdown.get_value()].get_text().ToLower();
		return result;
	}

	public void SetRegionIndex()
	{
		Debug.Log("Set index " + this.m_portalDropdown.get_value());
		this.CancelIndex = this.m_portalDropdown.get_value();
		string dropdownPortalText = this.GetDropdownPortalText();
		bool flag = Login.instance.GetBnPortal() != dropdownPortalText;
		Login.instance.SetPortal(dropdownPortalText);
		if (flag)
		{
			Login.instance.ClearAllCachedTokens();
			Debug.Log("Quitting");
			Application.Quit();
		}
	}

	public void CancelRegionIndex()
	{
		Debug.Log("Canceled index " + this.CancelIndex);
		this.m_showDialog = false;
		this.m_portalDropdown.set_value(this.CancelIndex);
		this.m_showDialog = true;
	}

	public void UpdateResumeButtonVisiblity()
	{
		this.m_resumeButton.get_gameObject().SetActive(Login.instance.HaveCachedWebToken());
		Text componentInChildren = this.m_resumeButton.GetComponentInChildren<Text>();
		if (componentInChildren != null)
		{
			componentInChildren.set_font(GeneralHelpers.LoadStandardFont());
			componentInChildren.set_text(StaticDB.GetString("CHARACTERS", null));
		}
	}
}

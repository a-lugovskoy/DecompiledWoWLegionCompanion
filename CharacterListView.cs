using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using WowJamMessages.JSONRealmList;

public class CharacterListView : MonoBehaviour
{
	private class CharacterButtonComparer : IComparer<CharacterListButton>
	{
		public int Compare(CharacterListButton char1, CharacterListButton char2)
		{
			Button component = char1.get_gameObject().GetComponent<Button>();
			Button component2 = char2.get_gameObject().GetComponent<Button>();
			if (component.get_interactable() && !component2.get_interactable())
			{
				return -1;
			}
			if (component2.get_interactable() && !component.get_interactable())
			{
				return 1;
			}
			if (char1.m_characterEntry.ExperienceLevel != char2.m_characterEntry.ExperienceLevel)
			{
				return (int)(char2.m_characterEntry.ExperienceLevel - char1.m_characterEntry.ExperienceLevel);
			}
			return string.Compare(char1.m_characterName.get_text(), char2.m_characterName.get_text());
		}
	}

	public GameObject charListItemPrefab;

	public GameObject charListContents;

	public float m_listItemInitialEntranceDelay;

	public float m_listItemEntranceDelay;

	public JamJSONCharacterEntry m_characterEntry;

	private static string m_levelText;

	private bool m_initialized;

	private void Start()
	{
	}

	private void Update()
	{
		if (!this.m_initialized && AssetBundleManager.instance.IsInitialized())
		{
			this.m_initialized = true;
			this.SetLevelText();
		}
	}

	public void ClearList()
	{
		CharacterListButton[] componentsInChildren = this.charListContents.get_transform().GetComponentsInChildren<CharacterListButton>(true);
		for (int i = 0; i < componentsInChildren.Length; i++)
		{
			Object.DestroyImmediate(componentsInChildren[i].get_gameObject());
		}
	}

	private void SetLevelText()
	{
		if (CharacterListView.m_levelText == null)
		{
			CharacterListView.m_levelText = StaticDB.GetString("LEVEL", null);
		}
	}

	private void OnEnable()
	{
		this.m_initialized = false;
	}

	public void CharacterSelected()
	{
		this.ClearList();
		Main.instance.allPanels.ShowConnectingPanel();
	}

	public void AddCharacterButton(JamJSONCharacterEntry charData, string subRegion, string realmName, bool online)
	{
		this.m_characterEntry = charData;
		GameObject gameObject = Object.Instantiate<GameObject>(this.charListItemPrefab);
		gameObject.get_transform().SetParent(this.charListContents.get_transform(), false);
		CharacterListButton component = gameObject.GetComponent<CharacterListButton>();
		component.SetGUID(charData.PlayerGuid);
		component.m_characterEntry = charData;
		component.m_subRegion = subRegion;
		Sprite sprite = GeneralHelpers.LoadClassIcon((int)charData.ClassID);
		if (sprite != null)
		{
			component.m_characterClassIcon.set_sprite(sprite);
		}
		component.m_characterName.set_text(charData.Name);
		bool flag = online;
		if (!charData.HasMobileAccess)
		{
			component.m_missingRequirement.set_text(StaticDB.GetString("REQUIRES_CLASS_HALL", null));
			component.m_missingRequirement.set_color(Color.get_red());
			flag = false;
		}
		else if (realmName == "unknown")
		{
			component.m_missingRequirement.set_text(string.Empty);
			flag = false;
		}
		else
		{
			if (online)
			{
				component.m_missingRequirement.set_text(realmName);
			}
			else
			{
				component.m_missingRequirement.set_text(realmName + " (" + StaticDB.GetString("OFFLINE", null) + ")");
			}
			component.m_missingRequirement.set_color(Color.get_yellow());
		}
		component.m_missingRequirement.get_gameObject().SetActive(true);
		if (!flag)
		{
			Button component2 = gameObject.GetComponent<Button>();
			component2.set_interactable(false);
			component.m_characterName.set_color(Color.get_grey());
			component.m_characterLevel.set_color(Color.get_grey());
		}
		int num = (int)charData.ExperienceLevel;
		if (num < 1)
		{
			num = 1;
		}
		component.m_characterLevel.set_text(GeneralHelpers.TextOrderString(CharacterListView.m_levelText, num.ToString()));
	}

	public void SortCharacterList()
	{
		CharacterListButton[] componentsInChildren = this.charListContents.get_transform().GetComponentsInChildren<CharacterListButton>(true);
		List<CharacterListButton> list = new List<CharacterListButton>();
		CharacterListButton[] array = componentsInChildren;
		for (int i = 0; i < array.Length; i++)
		{
			CharacterListButton characterListButton = array[i];
			list.Add(characterListButton);
		}
		CharacterListView.CharacterButtonComparer characterButtonComparer = new CharacterListView.CharacterButtonComparer();
		list.Sort(characterButtonComparer);
		for (int j = 0; j < list.get_Count(); j++)
		{
			CharacterListButton[] array2 = componentsInChildren;
			for (int k = 0; k < array2.Length; k++)
			{
				CharacterListButton characterListButton2 = array2[k];
				if (characterListButton2.m_characterEntry.PlayerGuid == list.get_Item(j).m_characterEntry.PlayerGuid)
				{
					characterListButton2.get_transform().SetSiblingIndex(j);
					break;
				}
			}
		}
		for (int l = 0; l < list.get_Count(); l++)
		{
			FancyEntrance component = list.get_Item(l).GetComponent<FancyEntrance>();
			component.m_timeToDelayEntrance = this.m_listItemInitialEntranceDelay + this.m_listItemEntranceDelay * (float)l;
			component.Activate();
		}
	}
}

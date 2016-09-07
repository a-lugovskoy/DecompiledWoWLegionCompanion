using System;
using UnityEngine;
using UnityEngine.UI;

public class PlayerInfoDisplay : MonoBehaviour
{
	public Text m_characterName;

	public Text m_characterClassName;

	public Text m_characterListButton;

	public Image m_classIcon;

	public GameObject m_recentCharactersPanel;

	private void OnEnable()
	{
		this.m_characterName.set_text(GarrisonStatus.CharacterName());
		if (Main.instance.GetLocale() == "frFR")
		{
			this.m_characterClassName.set_text(string.Concat(new string[]
			{
				GarrisonStatus.CharacterClassName(),
				" ",
				StaticDB.GetString("LEVEL", null),
				" ",
				GarrisonStatus.CharacterLevel().ToString()
			}));
		}
		else
		{
			this.m_characterClassName.set_text(GeneralHelpers.TextOrderString(StaticDB.GetString("LEVEL", null), GarrisonStatus.CharacterLevel().ToString()) + " " + GarrisonStatus.CharacterClassName());
		}
		this.m_characterListButton.set_text(StaticDB.GetString("CHARACTER_LIST", null));
		Sprite sprite = GeneralHelpers.LoadClassIcon(GarrisonStatus.CharacterClassID());
		if (sprite != null)
		{
			this.m_classIcon.set_sprite(sprite);
		}
	}

	public void ToggleRecentCharacterPanel()
	{
		Main.instance.m_UISound.Play_ButtonBlackClick();
		this.m_recentCharactersPanel.SetActive(!this.m_recentCharactersPanel.get_activeSelf());
	}

	public void HideRecentCharacterPanel()
	{
		this.m_recentCharactersPanel.SetActive(false);
	}
}

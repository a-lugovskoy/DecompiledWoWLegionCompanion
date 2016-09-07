using System;
using UnityEngine;
using UnityEngine.UI;
using WowJamMessages;

public class RecentCharacterButton : MonoBehaviour
{
	public Image m_characterClassIcon;

	public Text m_characterName;

	public Text m_characterLevel;

	private RecentCharacter m_recentCharacter;

	private void Start()
	{
	}

	private void Update()
	{
	}

	public void SetRecentCharacter(RecentCharacter recentChar)
	{
		this.m_recentCharacter = recentChar;
		this.m_characterName.set_text(this.m_recentCharacter.Entry.Name);
		this.m_characterLevel.set_text(this.m_recentCharacter.Entry.ExperienceLevel.ToString());
		Sprite sprite = GeneralHelpers.LoadClassIcon((int)this.m_recentCharacter.Entry.ClassID);
		if (sprite != null)
		{
			this.m_characterClassIcon.set_sprite(sprite);
		}
	}

	public void OnRecentCharacterClick()
	{
		Login.instance.SelectRecentCharacter(this.m_recentCharacter, this.m_recentCharacter.SubRegion);
	}
}

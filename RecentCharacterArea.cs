using System;
using UnityEngine;
using WowJamMessages;

public class RecentCharacterArea : MonoBehaviour
{
	public RecentCharacterButton[] m_charButtons;

	private void Start()
	{
	}

	private void Update()
	{
	}

	public void SetRecentCharacter(int index, RecentCharacter recentChar)
	{
		if (index < 0 || index >= this.m_charButtons.Length)
		{
			Debug.Log("SetRecentCharacter: invalid index " + index);
			return;
		}
		if (recentChar != null)
		{
			this.m_charButtons[index].get_gameObject().SetActive(true);
			this.m_charButtons[index].SetRecentCharacter(recentChar);
		}
		else
		{
			this.m_charButtons[index].get_gameObject().SetActive(false);
		}
	}
}

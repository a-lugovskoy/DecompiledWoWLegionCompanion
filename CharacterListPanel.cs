using System;
using UnityEngine;
using UnityEngine.UI;

public class CharacterListPanel : MonoBehaviour
{
	public Text m_titleText;

	public Text m_cancelText;

	public CharacterListView m_characterListView;

	private void Start()
	{
		this.m_titleText.set_font(GeneralHelpers.LoadFancyFont());
		this.m_titleText.set_text(StaticDB.GetString("CHARACTER_SELECTION", null));
		this.m_cancelText.set_font(GeneralHelpers.LoadStandardFont());
		this.m_cancelText.set_text(StaticDB.GetString("LOG_OUT", null));
	}

	private void Update()
	{
	}
}

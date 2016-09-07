using System;
using UnityEngine;
using UnityEngine.UI;

public class CreateNewLoginPanel : MonoBehaviour
{
	public InputField newLoginNameEditText;

	public InputField newLoginDataEditText;

	public GameObject loginListItemPrefab;

	public GameObject loginListContents;

	public void SaveNewLogin()
	{
		GameObject gameObject = Object.Instantiate<GameObject>(this.loginListItemPrefab);
		gameObject.get_transform().SetParent(this.loginListContents.get_transform(), false);
		LoginButton component = gameObject.GetComponent<LoginButton>();
		component.m_loginButtonNameText.set_text(this.newLoginNameEditText.get_text());
		if (this.newLoginDataEditText.get_text().StartsWith("http://localhost:0/?ST="))
		{
			component.m_token = this.newLoginDataEditText.get_text().Substring(23);
		}
		else
		{
			component.m_token = this.newLoginDataEditText.get_text();
		}
		bool flag = false;
		for (int i = 0; i < 10; i++)
		{
			string @string = SecurePlayerPrefs.GetString("DevAccount" + i, Main.uniqueIdentifier);
			if (@string != null && !(@string != this.newLoginNameEditText.get_text()))
			{
				SecurePlayerPrefs.SetString("DevToken" + i, component.m_token, Main.uniqueIdentifier);
				flag = true;
				break;
			}
		}
		if (!flag)
		{
			for (int j = 0; j < 10; j++)
			{
				string string2 = SecurePlayerPrefs.GetString("DevAccount" + j, Main.uniqueIdentifier);
				string string3 = SecurePlayerPrefs.GetString("DevToken" + j, Main.uniqueIdentifier);
				if ((string2 == null || string2 == string.Empty) && (string3 == null || string3 == string.Empty))
				{
					SecurePlayerPrefs.SetString("DevAccount" + j, this.newLoginNameEditText.get_text(), Main.uniqueIdentifier);
					SecurePlayerPrefs.SetString("DevToken" + j, component.m_token, Main.uniqueIdentifier);
					break;
				}
			}
		}
		Main.instance.allPanels.ShowRealmListPanel();
	}
}

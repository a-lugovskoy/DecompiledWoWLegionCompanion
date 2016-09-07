using System;
using UnityEngine;
using UnityEngine.UI;

public class ChatPopup : MonoBehaviour
{
	public Text conversationText;

	public InputField textToSend;

	private void Start()
	{
		TouchScreenKeyboard.set_hideInput(true);
	}

	private void Update()
	{
		if (TouchScreenKeyboard.get_visible())
		{
			Vector3 vector;
			RectTransformUtility.ScreenPointToWorldPointInRectangle(this.textToSend.get_gameObject().GetComponent<RectTransform>(), TouchScreenKeyboard.get_area().get_max(), null, ref vector);
			base.get_transform().set_position(new Vector3(base.get_transform().get_position().x, vector.y, base.get_transform().get_position().z));
		}
		else
		{
			base.get_transform().set_localPosition(Vector3.get_zero());
		}
		TouchScreenKeyboard.set_hideInput(true);
	}

	public void OnSendText()
	{
		if (this.textToSend.get_text().get_Length() == 0)
		{
			return;
		}
		Main.instance.SendGuildChat(this.textToSend.get_text());
		this.textToSend.set_text(string.Empty);
		this.textToSend.Select();
		this.textToSend.ActivateInputField();
	}

	public void OnReceiveText(string sender, string text)
	{
		Text expr_06 = this.conversationText;
		string text2 = expr_06.get_text();
		expr_06.set_text(string.Concat(new string[]
		{
			text2,
			"[",
			sender,
			"]: ",
			text,
			"\n"
		}));
	}
}

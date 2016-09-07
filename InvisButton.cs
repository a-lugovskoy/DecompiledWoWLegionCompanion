using System;
using UnityEngine;
using UnityEngine.UI;

public class InvisButton : MonoBehaviour
{
	public Text buttonText;

	public void OnClick()
	{
		this.buttonText.set_text("Down");
	}

	public void OnRelease()
	{
		this.buttonText.set_text("Up");
	}
}

using System;
using UnityEngine;

public class MultipanelDefaultPositioner : MonoBehaviour
{
	private void Start()
	{
		Vector3 localPosition = base.get_gameObject().get_transform().get_localPosition();
		localPosition.x = -1690f;
		base.get_gameObject().get_transform().set_localPosition(localPosition);
	}
}

using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LogPanel : MonoBehaviour
{
	public Text m_logText;

	public Logger m_logger;

	public void ShowPanel()
	{
		base.get_gameObject().SetActive(true);
		this.UpdateLogDisplay();
	}

	public void ClosePanel()
	{
		base.get_gameObject().SetActive(false);
	}

	public void ClearLog()
	{
		this.m_logger.ClearLog();
		this.UpdateLogDisplay();
	}

	public void UpdateLogDisplay()
	{
		int num = 1;
		this.m_logText.set_text(string.Empty);
		using (List<string>.Enumerator enumerator = this.m_logger.LogLines.GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				string current = enumerator.get_Current();
				if (num > 1)
				{
					Text expr_3D = this.m_logText;
					expr_3D.set_text(expr_3D.get_text() + "\n\n");
				}
				Text expr_58 = this.m_logText;
				string text = expr_58.get_text();
				expr_58.set_text(string.Concat(new object[]
				{
					text,
					num,
					") ",
					current
				}));
				num++;
			}
		}
	}
}

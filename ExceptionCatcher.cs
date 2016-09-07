using System;
using System.IO;
using UnityEngine;

public class ExceptionCatcher : MonoBehaviour
{
	public ExceptionPanel exceptionPanel;

	private void Awake()
	{
	}

	private void HandleLogMessage(string condition, string stackTrace, LogType type)
	{
		if (type == 4)
		{
			this.exceptionPanel.get_gameObject().SetActive(true);
			Debug.Log(condition + "\n" + stackTrace);
			FileStream fileStream = new FileStream(Application.get_persistentDataPath() + "/exceptions.log", 6);
			StreamWriter streamWriter = new StreamWriter(fileStream);
			streamWriter.WriteLine(condition + "\n" + stackTrace + "\n\n");
			streamWriter.Close();
			fileStream.Close();
			this.exceptionPanel.m_exceptionText.set_text(condition + "\n" + stackTrace);
		}
	}

	public void ShowExceptionLog()
	{
		this.exceptionPanel.get_gameObject().SetActive(true);
		FileStream fileStream = new FileStream(Application.get_persistentDataPath() + "/exceptions.log", 4);
		StreamReader streamReader = new StreamReader(fileStream);
		string exceptionText = streamReader.ReadToEnd();
		streamReader.Close();
		fileStream.Close();
		this.exceptionPanel.SetExceptionText(exceptionText);
	}

	public void ClearLog()
	{
		FileStream fileStream = new FileStream(Application.get_persistentDataPath() + "/exceptions.log", 2);
		fileStream.Close();
		this.exceptionPanel.SetExceptionText(string.Empty);
	}
}

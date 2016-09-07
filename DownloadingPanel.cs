using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DownloadingPanel : MonoBehaviour
{
	public Image m_progressBarFillImage;

	public Text m_downloadText;

	private void Start()
	{
		this.m_downloadText.set_font(GeneralHelpers.LoadStandardFont());
		string locale = Main.instance.GetLocale();
		string text = locale;
		if (text != null)
		{
			if (DownloadingPanel.<>f__switch$map4 == null)
			{
				Dictionary<string, int> dictionary = new Dictionary<string, int>(10);
				dictionary.Add("koKR", 0);
				dictionary.Add("frFR", 1);
				dictionary.Add("deDE", 2);
				dictionary.Add("zhCN", 3);
				dictionary.Add("zhTW", 4);
				dictionary.Add("esES", 5);
				dictionary.Add("esMX", 6);
				dictionary.Add("ruRU", 7);
				dictionary.Add("ptBR", 8);
				dictionary.Add("itIT", 9);
				DownloadingPanel.<>f__switch$map4 = dictionary;
			}
			int num;
			if (DownloadingPanel.<>f__switch$map4.TryGetValue(text, ref num))
			{
				switch (num)
				{
				case 0:
					this.m_downloadText.set_text("다운로드 중...");
					break;
				case 1:
					this.m_downloadText.set_text("Téléchargement…");
					break;
				case 2:
					this.m_downloadText.set_text("Lade herunter...");
					break;
				case 3:
					this.m_downloadText.set_text("下载中……");
					break;
				case 4:
					this.m_downloadText.set_text("下載中...");
					break;
				case 5:
					this.m_downloadText.set_text("Descargando...");
					break;
				case 6:
					this.m_downloadText.set_text("Descargando...");
					break;
				case 7:
					this.m_downloadText.set_text("Загрузка...");
					break;
				case 8:
					this.m_downloadText.set_text("Baixando...");
					break;
				case 9:
					this.m_downloadText.set_text("Download...");
					break;
				}
			}
		}
	}

	private void Update()
	{
		this.m_progressBarFillImage.set_fillAmount(AssetBundleManager.instance.GetDownloadProgress());
	}
}

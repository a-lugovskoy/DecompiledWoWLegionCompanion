using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

public class AssetBundleManager : MonoBehaviour
{
	private const int HASH_LENGTH = 32;

	private const string m_versionFile = "version.txt";

	private static AssetBundleManager s_instance;

	private static bool s_initialized;

	private string m_assetServerIpAddress = "blzddist2-a.akamaihd.net";

	private string m_assetServerIpAddress_CN = "client02.pdl.wow.battlenet.com.cn";

	private AssetBundle m_portraitIconsBundle;

	private AssetBundle m_iconsBundle;

	public string m_devAssetServerURL;

	private string m_assetServerURL;

	private Dictionary<string, string> m_manifest;

	public Action InitializedAction;

	private WWW m_currentWWW;

	private float m_priorProgress;

	private float m_progressMultiplier;

	private float m_progressStartTime;

	private string m_assetBundleDirectory = "ab";

	private string m_platform = "a";

	public int LatestVersion
	{
		get;
		set;
	}

	public bool ForceUpgrade
	{
		get;
		set;
	}

	public string AppStoreUrl
	{
		get;
		set;
	}

	public string AppStoreUrl_CN
	{
		get;
		set;
	}

	public static AssetBundleManager instance
	{
		get
		{
			return AssetBundleManager.s_instance;
		}
	}

	public static AssetBundle portraitIcons
	{
		get
		{
			return AssetBundleManager.instance.m_portraitIconsBundle;
		}
	}

	public static AssetBundle Icons
	{
		get
		{
			return AssetBundleManager.instance.m_iconsBundle;
		}
	}

	private void Awake()
	{
		if (AssetBundleManager.s_instance == null)
		{
			AssetBundleManager.s_instance = this;
			this.m_manifest = new Dictionary<string, string>();
		}
		this.LatestVersion = 0;
		this.ForceUpgrade = false;
	}

	private void Start()
	{
		this.InitAssetBundleManager();
	}

	public bool IsInitialized()
	{
		return AssetBundleManager.s_initialized;
	}

	private void InitAssetBundleManager()
	{
		if (AssetBundleManager.s_initialized)
		{
			return;
		}
		base.StartCoroutine(this.InternalInitAssetBundleManager());
	}

	[DebuggerHidden]
	private IEnumerator InternalInitAssetBundleManager()
	{
		AssetBundleManager.<InternalInitAssetBundleManager>c__Iterator7 <InternalInitAssetBundleManager>c__Iterator = new AssetBundleManager.<InternalInitAssetBundleManager>c__Iterator7();
		<InternalInitAssetBundleManager>c__Iterator.<>f__this = this;
		return <InternalInitAssetBundleManager>c__Iterator;
	}

	[DebuggerHidden]
	private IEnumerator InternalInitAssetBundleManagerLocal()
	{
		AllPanels.instance.SetConnectingPanelStatus("Loading...");
		AllPanels.instance.connectingPanel.m_cancelButton.get_gameObject().SetActive(false);
		string text = string.Concat(new string[]
		{
			this.m_assetBundleDirectory,
			"/",
			this.m_platform,
			"/",
			this.m_platform
		});
		TextAsset textAsset = Resources.Load<TextAsset>(text);
		if (textAsset == null)
		{
			throw new Exception("Could not load manifest at path " + text);
		}
		this.BuildManifest(textAsset.get_text());
		string fileIdentifier = "staticdb_" + Main.instance.GetLocale().ToLower();
		yield return base.StartCoroutine(this.LoadAssetBundleLocal("icons", delegate(AssetBundle value)
		{
			this.<>f__this.m_iconsBundle = value;
		}));
		yield return base.StartCoroutine(this.LoadAssetBundleLocal("portraiticons", delegate(AssetBundle value)
		{
			this.<>f__this.m_portraitIconsBundle = value;
		}));
		AssetBundle assetBundle = null;
		AssetBundle assetBundle2 = null;
		yield return base.StartCoroutine(this.LoadAssetBundleLocal("staticdb_gnrc", delegate(AssetBundle value)
		{
			this.<genericStaticDB>__3 = value;
		}));
		yield return base.StartCoroutine(this.LoadAssetBundleLocal(fileIdentifier, delegate(AssetBundle value)
		{
			this.<localizedStaticDB>__4 = value;
		}));
		StaticDB.instance.InitDBs(assetBundle, assetBundle2);
		if (assetBundle != null)
		{
			assetBundle.Unload(true);
		}
		if (assetBundle2 != null)
		{
			assetBundle2.Unload(true);
		}
		AssetBundleManager.s_initialized = true;
		if (this.InitializedAction != null)
		{
			this.InitializedAction.Invoke();
		}
		yield break;
	}

	private void BuildManifest(string manifestText)
	{
		int num = 0;
		int num2;
		do
		{
			num2 = manifestText.IndexOf('\n', num);
			if (num2 >= 0)
			{
				string lineText = manifestText.Substring(num, num2 - num + 1).Trim();
				this.ParseManifestLine(lineText);
				num = num2 + 1;
			}
		}
		while (num2 > 0);
	}

	private void ParseManifestLine(string lineText)
	{
		int num = 0;
		int num2;
		do
		{
			string text = "Name: ";
			num2 = lineText.IndexOf(text, num);
			if (num2 >= 0)
			{
				int num3 = num2 + text.get_Length();
				string text2 = lineText.Substring(num3, lineText.get_Length() - num3).Trim();
				string text3 = text2.Substring(0, text2.get_Length() - 33);
				this.m_manifest.Add(text3, text2);
			}
			num = num2 + 1;
		}
		while (num2 > 0);
	}

	private string GetBundleFileName(string fileIdentifier)
	{
		string result;
		if (this.m_manifest.TryGetValue(fileIdentifier, ref result))
		{
			return result;
		}
		return null;
	}

	[DebuggerHidden]
	public IEnumerator LoadAssetBundle(string fileIdentifier, Action<AssetBundle> resultCallback)
	{
		string bundleFileName = this.GetBundleFileName(fileIdentifier);
		if (bundleFileName != null)
		{
			while (!Caching.get_ready())
			{
				yield return null;
			}
			string text = this.m_assetServerURL + bundleFileName;
			if (!Caching.IsVersionCached(text, 0))
			{
				Debug.Log("File " + fileIdentifier + " not cached. Will now load new file " + bundleFileName);
				if (!AllPanels.instance.IsShowingDownloadingPanel())
				{
					AllPanels.instance.ShowDownloadingPanel(true);
				}
			}
			WWW wWW = WWW.LoadFromCacheOrDownload(text, 0);
			this.m_currentWWW = wWW;
			yield return wWW;
			resultCallback.Invoke(wWW.get_assetBundle());
			yield break;
		}
		SecurePlayerPrefs.DeleteKey("locale");
		throw new Exception("LoadAssetBundle: Error, file identifier " + fileIdentifier + " is unknown.");
	}

	[DebuggerHidden]
	public IEnumerator LoadAssetBundleLocal(string fileIdentifier, Action<AssetBundle> resultCallback)
	{
		string bundleFileName = this.GetBundleFileName(fileIdentifier);
		string text = string.Concat(new string[]
		{
			this.m_assetBundleDirectory,
			"/",
			this.m_platform,
			"/",
			bundleFileName
		});
		if (bundleFileName == null)
		{
			SecurePlayerPrefs.DeleteKey("locale");
			throw new Exception("LoadAssetBundle: Error, file identifier " + fileIdentifier + " is unknown.");
		}
		ResourceRequest resourceRequest = Resources.LoadAsync<TextAsset>(text);
		while (!resourceRequest.get_isDone())
		{
			yield return 0;
		}
		TextAsset textAsset = resourceRequest.get_asset() as TextAsset;
		if (textAsset == null)
		{
			throw new Exception("Unable to load asset bundle " + text);
		}
		AssetBundle assetBundle = AssetBundle.LoadFromMemory(textAsset.get_bytes());
		yield return assetBundle;
		resultCallback.Invoke(assetBundle);
		yield break;
	}

	public float GetDownloadProgress()
	{
		float num = this.m_priorProgress;
		if (this.m_currentWWW != null && Time.get_timeSinceLevelLoad() > this.m_progressStartTime + 1f)
		{
			num += this.m_currentWWW.get_progress() * this.m_progressMultiplier;
		}
		return num;
	}

	public bool IsDevAssetBundles()
	{
		return false;
	}

	[DebuggerHidden]
	public IEnumerator FetchLatestVersion(string url)
	{
		AssetBundleManager.<FetchLatestVersion>c__IteratorB <FetchLatestVersion>c__IteratorB = new AssetBundleManager.<FetchLatestVersion>c__IteratorB();
		<FetchLatestVersion>c__IteratorB.url = url;
		<FetchLatestVersion>c__IteratorB.<$>url = url;
		<FetchLatestVersion>c__IteratorB.<>f__this = this;
		return <FetchLatestVersion>c__IteratorB;
	}
}

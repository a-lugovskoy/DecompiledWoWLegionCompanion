using System;
using UnityEngine;

public class GameClient : MonoBehaviour
{
	public GameObject pushManager;

	private void Start()
	{
		if (BLPushManager.instance == null)
		{
			Object.Instantiate<GameObject>(this.pushManager);
		}
	}

	public void Register(string token, string locale)
	{
		BLPushManagerBuilder bLPushManagerBuilder = ScriptableObject.CreateInstance<BLPushManagerBuilder>();
		if (Login.m_portal.ToLower() == "wow-dev")
		{
			bLPushManagerBuilder.isDebug = true;
			bLPushManagerBuilder.applicationName = "test.wowcompanion";
		}
		else
		{
			bLPushManagerBuilder.isDebug = false;
			bLPushManagerBuilder.applicationName = "wowcompanion";
		}
		bLPushManagerBuilder.shouldRegisterwithBPNS = true;
		bLPushManagerBuilder.region = "US";
		bLPushManagerBuilder.locale = locale;
		bLPushManagerBuilder.authToken = token;
		bLPushManagerBuilder.authRegion = "US";
		bLPushManagerBuilder.appAccountID = string.Empty;
		bLPushManagerBuilder.senderId = "952133414280";
		bLPushManagerBuilder.didReceiveRegistrationTokenDelegate = new DidReceiveRegistrationTokenDelegate(this.DidReceiveRegistrationTokenHandler);
		bLPushManagerBuilder.didReceiveDeeplinkURLDelegate = new DidReceiveDeeplinkURLDelegate(this.DidReceiveDeeplinkURLDelegateHandler);
		BLPushManager.instance.InitWithBuilder(bLPushManagerBuilder);
		BLPushManager.instance.RegisterForPushNotifications();
	}

	public void DidReceiveRegistrationTokenHandler(string deviceToken)
	{
		Debug.Log("DidReceiveRegistrationTokenHandler: device token " + deviceToken);
	}

	public void DidReceiveDeeplinkURLDelegateHandler(string url)
	{
		Debug.Log("DidReceiveDeeplinkURLDelegateHandler: url " + url);
	}
}

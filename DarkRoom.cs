using System;
using UnityEngine;
using UnityEngine.UI;

public class DarkRoom
{
	public static void MakeSnapshot(FreezeFrame freezeFrame)
	{
		GameObject gameObject = GameObject.Find("MainCanvas");
		if (gameObject == null)
		{
			Debug.LogError("Could not find MainCanvas, did you rename it?");
			return;
		}
		GameObject gameObject2 = new GameObject(freezeFrame.get_name() + "_SnaphotCamera");
		gameObject2.get_transform().SetParent(null, false);
		gameObject2.get_transform().Translate(0f, 0f, -1f);
		Camera camera = gameObject2.AddComponent<Camera>();
		RenderTexture renderTexture = new RenderTexture(320, 320, 24);
		Material material = new Material(Shader.Find("UI/Unlit/Transparent"));
		material.SetTexture("_MainTex", renderTexture);
		camera.set_orthographic(true);
		camera.set_orthographicSize(100f);
		camera.set_targetTexture(renderTexture);
		camera.set_depth(-1f);
		camera.set_clearFlags(2);
		camera.set_backgroundColor(new Color(0f, 0f, 0f, 1f));
		GameObject gameObject3 = new GameObject(freezeFrame.get_name() + "_DarkRoomCanvas");
		Canvas canvas = gameObject3.AddComponent<Canvas>();
		canvas.set_planeDistance(500f);
		canvas.set_renderMode(1);
		canvas.set_worldCamera(camera);
		CanvasScaler canvasScaler = gameObject3.AddComponent<CanvasScaler>();
		canvasScaler.set_uiScaleMode(1);
		canvasScaler.set_referenceResolution(new Vector2(1200f, 900f));
		canvasScaler.set_matchWidthOrHeight(1f);
		GameObject gameObject4 = new GameObject(freezeFrame.get_name() + "_Snapshot");
		gameObject4.get_transform().SetParent(gameObject.get_transform(), false);
		Image image = gameObject4.AddComponent<Image>();
		gameObject4.GetComponent<RectTransform>().set_sizeDelta(new Vector2(320f, 320f));
		image.set_material(material);
		freezeFrame.get_transform().SetParent(gameObject3.get_transform(), false);
		camera.Render();
	}
}

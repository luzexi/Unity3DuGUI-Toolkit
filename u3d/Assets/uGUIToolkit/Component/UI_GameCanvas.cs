using UnityEngine;
using UnityEngine.UI;
using System.Collections;
/*
 * Canvas helper componenet that sets up some default parameters for Canvas and Canvas Scaler.
 */ 

public enum eCamType
{
	Scene,
	OverlayUI,
	UI,
}


[AddComponentMenu("UI/UI GameCanvas")]
public class UI_GameCanvas : MonoBehaviour {

//    public bool m_UseSceneCam = false;
	public eCamType m_CamType = eCamType.UI;
	public bool m_WorldSpace = false;
	public Vector2 mResolution = new Vector2(960,640);

    protected static Camera s_SceneCam;
	protected static Camera s_OverlayUICam;
	protected static Camera s_UICam;

    protected Canvas m_Canvas;
    protected CanvasScaler m_CanvasScaler;



	// Use this for initialization
	void Start () {
		//Debug.Log(gameObject.name);
        if (s_SceneCam == null)
		{
			GameObject obj = GameObject.Find("SceneCam");
			if (obj)
            	s_SceneCam = obj.GetComponent<Camera>();
		}
		if (s_OverlayUICam == null)
		{
			GameObject obj = GameObject.Find("OverlayUICam");
			if (obj)
				s_OverlayUICam = obj.GetComponent<Camera>();
		}
		if (s_UICam == null)
		{
			GameObject uiCam = GameObject.Find("UICam");
			if (uiCam != null)
				s_UICam = GameObject.Find("UICam").GetComponent<Camera>();
		}

        m_Canvas = GetComponent<Canvas>();
        m_CanvasScaler = GetComponent<CanvasScaler>();

        if (m_Canvas == null) Debug.LogError("KumaCanvas requires a Canvas component. Please fix in editor.");
        if (m_CanvasScaler == null) Debug.LogError("KumaCanvas requires a Canvas Scaler component. Please fix in editor.");

		m_Canvas.renderMode = m_WorldSpace ? RenderMode.WorldSpace : RenderMode.ScreenSpaceCamera;
		m_Canvas.worldCamera = (m_CamType == eCamType.Scene) ? s_SceneCam : (m_CamType == eCamType.OverlayUI) ? s_OverlayUICam : s_UICam;
        m_Canvas.pixelPerfect = true;
        m_Canvas.planeDistance = 100;
        m_Canvas.worldCamera = s_UICam;

        //m_CanvasScaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
        //m_CanvasScaler.screenMatchMode = CanvasScaler.ScreenMatchMode.MatchWidthOrHeight;

        //TODO might need to change this for HD screen resolutions.
        m_CanvasScaler.referenceResolution = mResolution;
	}

}

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
	private Vector2 mResolution = new Vector2(UIDefine.SCREEN_WIDTH, UIDefine.SCREEN_HEIGHT);

    protected static Camera s_SceneCam;
	protected static Camera s_OverlayUICam;
	protected static Camera s_UICam;
	protected static bool mInit = false;

    protected Canvas m_Canvas;
    protected RectTransform m_CanvasRectTransform;
    protected CanvasScaler m_CanvasScaler;


	// Use this for initialization
	void Awake () {
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
			GameObject uiCam = GameObject.Find("UICamera");
			if (uiCam != null)
				s_UICam = GameObject.Find("UICamera").GetComponent<Camera>();
		}

		if(m_Canvas == null)
        	m_Canvas = GetComponent<Canvas>();
        if(m_CanvasRectTransform == null)
        {
        	m_CanvasRectTransform = GetComponent<RectTransform>();
        }
        if(m_CanvasScaler == null)
        	m_CanvasScaler = GetComponent<CanvasScaler>();

        if (m_Canvas == null) Debug.LogError("KumaCanvas requires a Canvas component. Please fix in editor.");
        if (m_CanvasScaler == null) Debug.LogError("KumaCanvas requires a Canvas Scaler component. Please fix in editor.");

		// m_Canvas.renderMode = m_WorldSpace ? RenderMode.WorldSpace : RenderMode.ScreenSpaceCamera;
		m_Canvas.worldCamera = (m_CamType == eCamType.Scene) ? s_SceneCam : (m_CamType == eCamType.OverlayUI) ? s_OverlayUICam : s_UICam;
        // m_Canvas.pixelPerfect = true;
        // m_Canvas.planeDistance = 100;
        m_Canvas.worldCamera = s_UICam;

        bool check = true;
        if(m_CanvasScaler.uiScaleMode != CanvasScaler.ScaleMode.ScaleWithScreenSize)
        {
        	Debug.LogError("uiScaleMode maybe error please check");
        	check = false;
        }

        if(m_CanvasScaler.screenMatchMode != CanvasScaler.ScreenMatchMode.MatchWidthOrHeight)
        {
        	Debug.LogError("screenMatchMode maybe error please check");
        	check = false;
        }
        // if(m_CanvasScaler.matchWidthOrHeight != 0.5f)
        // {
        // 	Debug.LogError("matchWidthOrHeight maybe error please check");
        // 	check = false;
        // }
        if(m_CanvasScaler.referenceResolution != mResolution)
        {
        	Debug.LogError("referenceResolution maybe error please check");
        	check = false;
        }

        if(UIDefine.SCREEN_WIDTH*1f/UIDefine.SCREEN_HEIGHT > Screen.width*1f/Screen.height)
        {
            m_CanvasScaler.matchWidthOrHeight = 0;
        }
        else
        {
            m_CanvasScaler.matchWidthOrHeight = 1;
        }
  //       m_CanvasScaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
  //       m_CanvasScaler.screenMatchMode = CanvasScaler.ScreenMatchMode.MatchWidthOrHeight;
  //       m_CanvasScaler.matchWidthOrHeight = 0.5f;

  //       //TODO might need to change this for HD screen resolutions.
  //       m_CanvasScaler.referenceResolution = mResolution;
        if(check)
        {
        	InitScreenWidthHeight();
        }
	}

	public void InitScreenWidthHeight()
	{
		if(mInit) return;
		mInit = true;
		Vector2 resWidthHeight;
		RectTransformUtility.ScreenPointToLocalPointInRectangle(m_CanvasRectTransform, Vector2.zero, s_UICam, out resWidthHeight);

		resWidthHeight = resWidthHeight * 2;
		UIDefine.UI_SCREEN_WIDTH = Mathf.RoundToInt(Mathf.Abs(resWidthHeight.x));
		UIDefine.UI_SCREEN_HEIGHT = Mathf.RoundToInt(Mathf.Abs(resWidthHeight.y));
	}

}

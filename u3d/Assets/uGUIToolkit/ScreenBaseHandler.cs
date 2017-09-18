using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;
using System;

public class ScreenBaseHandler : MonoBehaviour
{
	public bool m_DestroyOnClose = false;
    protected bool m_Initialized = false; // Prevent unintentional, double initialization.
    private bool mIsShow = false;
	public bool IsShow
	{
		get
		{
			return mIsShow;
		}
	}

    public delegate void OnScreenHandlerEventHandler(ScreenBaseHandler screenHandler);
    public event OnScreenHandlerEventHandler OnCloseAndDestroy;
    public event OnScreenHandlerEventHandler OnShowScreen;
    public event OnScreenHandlerEventHandler OnHideScreen;

    // Use this for initialization
    void Awake()
    {
        if (m_Initialized == false)
        {
		    Init();
        }
    }

	public virtual void Init()
	{	
        m_Initialized = true;
	}

    public virtual void HideScreen()
    {
        mIsShow = false;

        if (OnCloseAndDestroy != null)
            OnCloseAndDestroy(this);

        if (OnHideScreen != null)
            OnHideScreen(this);
    }

    public virtual void ShowScreen()
    {
        mIsShow = true;
        if (OnShowScreen != null)
            OnShowScreen(this);
    }

}

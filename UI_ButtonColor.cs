using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;


//  UI_ButtonColor.cs
//  Author: Lu Zexi
//  2015-02-06



[AddComponentMenu("uGUI/UI_Button Color")]
public class UI_ButtonColor : MonoBehaviour
{
    /// <summary>
    /// Target with a widget, renderer, or light that will have its color tweened.
    /// </summary>

    public GameObject tweenTarget;

    /// <summary>
    /// Color to apply on hover event (mouse only).
    /// </summary>
    public Color hover = Color.white;

    /// <summary>
    /// Color to apply on the pressed event.
    /// </summary>
    public Color pressed = new Color(0.75f, 0.75f, 0.75f, 1f);

    /// <summary>
    /// Duration of the tween process.
    /// </summary>
    public float duration = 0.2f;

    Color mColor;
    Image mImage;
    bool mStarted = false;
    bool m_bStartTween = false;
    float m_fStartTime = -1;
    

    /// <summary>
    /// UI_ButtonColor's default (starting) color. It's useful to be able to change it, just in case.
    /// </summary>
    public Color defaultColor
    {
        get
        {
            Start();
            return mColor;
        }
        set
        {
            Start();
            mColor = value;
        }
    }

    void Start ()
    {
        if (!mStarted)
        {
            mStarted = true;
            Init();
            var ev = UI_Event.Get(tweenTarget);
            ev.onClick += OnClick;
        }
    }

    void Update()
    {
        if( this.m_bStartTween )
        {
            float disTime = Time.realtimeSinceStartup - this.m_fStartTime;
            float rate = disTime / duration;
            if( rate > 1f )
                rate = 1f;
            if(rate >= 1f)
            {
                this.m_bStartTween = false;
                if(this.mImage != null)
                    this.mImage.color = mColor;
            }
            else
            {
                if( rate >= 0.5f )
                    rate = 1f - rate;
                if(this.mImage != null)
                    this.mImage.color = Color.Lerp( mColor , pressed , rate );
            }
        }
    }

    void OnDisable ()
    {
        if (mStarted && tweenTarget != null)
        {
            if( this.mImage != null )
                this.mImage.color = mColor;
        }
    }

    void Init ()
    {
        if (tweenTarget == null) tweenTarget = gameObject;
        Image img = tweenTarget.GetComponent<Image>();
        this.mImage = img;
        if(img != null)
        {
            mColor = img.color;
        }
    }

    void OnClick ( PointerEventData eventData , GameObject go , string[] args )
    {
        if (enabled)
        {
            if (!mStarted) Start();
            this.m_bStartTween = true;
            this.m_fStartTime = Time.realtimeSinceStartup;
            if( this.mImage != null )
                this.mImage.color = mColor;
            if(duration <= 0)
            {
                if( this.mImage != null )
                    this.mImage.color = pressed;
            }
        }
    }
}

using UnityEngine;
using UnityEngine.EventSystems;


/// <summary>
/// Simple example script of how a button can be offset visibly when the mouse hovers over it or it gets pressed.
/// </summary>
[AddComponentMenu("uGUI/UI_Button Offset")]
public class UI_ButtonOffset : MonoBehaviour
{
    public Transform tweenTarget;
    public Vector3 hover = Vector3.zero;
    public Vector3 pressed = new Vector3(2f, -2f);
    public float duration = 0.1f;

    Vector3 mPos;
    bool mStarted = false;

    bool m_bStartTween = false;
    float m_fStartTime = -1f;

    void Start ()
    {
        if (!mStarted)
        {
            mStarted = true;
            if (tweenTarget == null) tweenTarget = transform;
            mPos = tweenTarget.localPosition;
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
                this.tweenTarget.localPosition = mPos;
            }
            else
            {
                if( rate >= 0.5f )
                    rate = 1f - rate;
                 this.tweenTarget.localPosition = Vector3.Lerp( mPos , pressed , rate );
            }
        }
    }

    void OnEnable ()
    {
        // if (mStarted) OnHover(UICamera.IsHighlighted(gameObject));
    }

    void OnDisable ()
    {
        if (mStarted && tweenTarget != null)
        {
            tweenTarget.localPosition = mPos;
        }
    }

    void OnClick (PointerEventData eventData , GameObject go , string[] args)
    {
        if (enabled)
        {
            if (!mStarted) Start();
            this.m_bStartTween = true;
            this.m_fStartTime = Time.realtimeSinceStartup;
            tweenTarget.localPosition = mPos;
            if(duration <= 0)
                tweenTarget.localPosition = pressed;
        }
    }
}

using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;


//  UI_ButtonScale.cs
//  Author: Lu Zexi
//  2015-02-06


[AddComponentMenu("uGUI/UI_Button Scale")]
public class UI_ButtonScale : MonoBehaviour
{
    public Transform tweenTarget;
    public Vector3 hover = Vector3.one;
    public Vector3 pressed = new Vector3(0.95f, 0.95f, 0.95f);
    public float duration = 0.1f;

    Vector3 mScale;
    bool mStarted = false;

    bool m_bStartTween = false;
    float m_fStartTime = -1f;

    void Start ()
    {
        if (!mStarted)
        {
            mStarted = true;
            if (tweenTarget == null) tweenTarget = transform;
            mScale = tweenTarget.localScale;
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
                this.tweenTarget.localScale = mScale;
            }
            else
            {
                if( rate >= 0.5f )
                    rate = 1f - rate;
                 tweenTarget.localScale = Vector3.Lerp( mScale , pressed , rate );
            }
        }
    }

    void OnDisable ()
    {
        if (mStarted && tweenTarget != null)
        {
            tweenTarget.localScale = mScale;
        }
    }

    void OnClick ( PointerEventData eventData , GameObject go , string[] args )
    {
        if (enabled)
        {
            if (!mStarted) Start();
            this.m_bStartTween = true;
            this.m_fStartTime = Time.realtimeSinceStartup;
            tweenTarget.localScale = mScale;
            if(duration <= 0)
                tweenTarget.localScale = pressed;
        }
    }
}

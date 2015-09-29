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
    public float duration = 0f;

    Vector3 mScale;
    bool mStarted = false;

    void Awake ()
    {
        if (!mStarted)
        {
            mStarted = true;
            if (tweenTarget == null) tweenTarget = transform;
            mScale = tweenTarget.localScale;
            var ev = UI_Event.Get(transform);
            ev.onDown += OnDown;
            ev.onUp += OnUp;
        }
    }

    void OnDisable ()
    {
        if (mStarted && tweenTarget != null)
        {
            tweenTarget.localScale = mScale;
        }
    }

    void OnDown ( PointerEventData eventData , GameObject go , string[] args )
    {
        if (enabled)
        {
            UI_TweenScale.Begin(tweenTarget.gameObject , duration , pressed);
        }
    }

    void OnUp ( PointerEventData eventData , GameObject go , string[] args )
    {
        if (enabled)
        {
            UI_TweenScale.Begin(tweenTarget.gameObject , duration , mScale).from = pressed;
        }
    }
}

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
    public float duration = 0f;

    Vector3 mPos;
    bool mStarted = false;

    void Awake ()
    {
        if (!mStarted)
        {
            mStarted = true;
            if (tweenTarget == null) tweenTarget = transform;
            mPos = tweenTarget.localPosition;
            var ev = UI_Event.Get(tweenTarget);
            ev.onDown += OnDown;
            ev.onUp += OnUp;
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

    void OnDown ( PointerEventData eventData , GameObject go , string[] args )
    {
        if (enabled)
        {
            UI_TweenPosition.Begin(tweenTarget.gameObject , duration , mPos + pressed);
        }
    }

    void OnUp ( PointerEventData eventData , GameObject go , string[] args )
    {
        if (enabled)
        {
            UI_TweenPosition.Begin(tweenTarget.gameObject , duration , mPos).from = mPos + pressed;
        }
    }
}

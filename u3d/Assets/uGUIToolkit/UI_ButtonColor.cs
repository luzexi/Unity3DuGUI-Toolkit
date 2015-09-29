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
    public float duration = 0f;

    Color mColor;
    Image mImage;
    bool mStarted = false;

    void Awake ()
    {
        if (!mStarted)
        {
            mStarted = true;
            Init();
            var ev = UI_Event.Get(tweenTarget);
            ev.onDown += OnDown;
            ev.onUp += OnUp;
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

    void OnDown ( PointerEventData eventData , GameObject go , string[] args )
    {
        if (enabled)
        {
            UI_TweenColor.Begin(tweenTarget , duration , pressed);
        }
    }

    void OnUp ( PointerEventData eventData , GameObject go , string[] args )
    {
        if (enabled)
        {
            UI_TweenColor.Begin(tweenTarget , duration , mColor).from = pressed;
        }
    }
}

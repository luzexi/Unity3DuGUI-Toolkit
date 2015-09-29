using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;


//  UI_ButtonImage.cs
//  Author: Lu Zexi
//  2015-02-06



[AddComponentMenu("uGUI/UI_Button Image")]
public class UI_ButtonImage : MonoBehaviour
{
    /// <summary>
    /// Target with a widget, renderer, or light that will have its Sprite tweened.
    /// </summary>

    public GameObject tweenTarget;

    /// <summary>
    /// Sprite to apply on normal.
    /// </summary>
    public Sprite normal = null;

    /// <summary>
    /// Sprite to apply on the pressed event.
    /// </summary>
    public Sprite pressed = null;

    Sprite mSprite;
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
                this.mImage.sprite = mSprite;
        }
    }

    void Init ()
    {
        if (tweenTarget == null) tweenTarget = gameObject;
        Image img = tweenTarget.GetComponent<Image>();
        this.mImage = img;
        if(img != null)
        {
            mSprite = img.sprite;
        }
    }

    void OnDown ( PointerEventData eventData , GameObject go , string[] args )
    {
        if (enabled)
        {
            this.mImage.sprite = pressed;
        }
    }

    void OnUp ( PointerEventData eventData , GameObject go , string[] args )
    {
        if (enabled)
        {
            this.mImage.sprite = normal;
        }
    }
}

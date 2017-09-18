
using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;


//  UI_TweenColor.cs
//  Author: Lu Zexi
//  2015-02-07


//ui tween color
[AddComponentMenu("UI/Tween/UI_Tween Color")]
public class UI_TweenColor : UI_Tween
{
    public Color from = Color.white;
    public Color to = Color.grey;
    public LoopType loop = LoopType.PingPong;
    public float duration = 1f;
    public EaseType easeType = EaseType.easeOutExpo;

    [HideInInspector]
    public Image m_cImage; //image
    float m_fStartTime; //start time
    EasingFunction m_delEase;   //delegate function


    void Start()
    {
        if(tweenTarget == null) tweenTarget = this.gameObject;
        this.m_fStartTime = Time.realtimeSinceStartup;
        this.m_cImage = tweenTarget.GetComponent<Image>();
        this.m_delEase = GetEasingFunction(easeType);
    }

    void Update()
    {
        if( enabled && this.m_cImage != null )
        {
            float disTime = Time.realtimeSinceStartup - this.m_fStartTime;
            float rate = 0;
            switch( loop )
            {
                case LoopType.Once:
                    {
                        rate = disTime/duration;
                        if( rate > 1 ) {rate = 1;this.enabled = false;}
                        rate = this.m_delEase(0,1f,rate);
                        this.m_cImage.color = Color.Lerp(from , to , rate);
                    }
                    break;
                case LoopType.Loop:
                    {
                        disTime = (disTime - ((int)(disTime/duration))*duration);
                        rate = disTime / duration;
                        rate = this.m_delEase(0,1f,rate);
                        this.m_cImage.color = Color.Lerp(from , to , rate);
                    }
                    break;
                case LoopType.PingPong:
                    {
                        int loopNum = (int)(disTime/duration);
                        disTime = disTime - loopNum*duration;
                        rate = disTime / duration;
                        if(loopNum%2 == 1)
                            rate = 1f - rate;
                        rate = this.m_delEase(0,1f,rate);
                        this.m_cImage.color = Color.Lerp(from , to , rate);
                    }
                    break;
            }
        }
    }

    /// <summary>
    /// Start the tweening operation.
    /// </summary>
    static public UI_TweenColor Begin (GameObject go, float duration, Color color)
    {
        UI_TweenColor comp = UI_Tween.Begin<UI_TweenColor>(go);
        comp.duration = duration;
        comp.to = color;

        if (duration <= 0f)
        {
            if( comp.m_cImage != null )
                comp.m_cImage.color = color;
            comp.enabled = false;
        }
        return comp;
    }

}


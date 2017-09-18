
using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;


//  UI_TweenGroup.cs
//  Author: Lu Zexi
//  2015-02-07


//ui tween group alpha
[AddComponentMenu("UI/Tween/UI_TweenGroup")]
[RequireComponent (typeof (CanvasGroup))]
public class UI_TweenGroup : UI_Tween
{
    public float from = 0;
    public float to = 1;
    public LoopType loop = LoopType.Once;
    public float duration = 1f;
    public EaseType easeType = EaseType.easeOutExpo;

    [HideInInspector]
    public CanvasGroup m_cCanvasGroup; //image
    float m_fStartTime; //start time
    EasingFunction m_delEase;   //delegate function


    void Start()
    {
        if(tweenTarget == null) tweenTarget = this.gameObject;
        this.m_fStartTime = Time.time;
        this.m_cCanvasGroup = tweenTarget.GetComponent<CanvasGroup>();
        this.m_delEase = GetEasingFunction(easeType);
    }

    void Update()
    {
        if( enabled && this.m_cCanvasGroup != null )
        {
            float disTime = Time.time - this.m_fStartTime;
            float rate = 0;
            switch( loop )
            {
                case LoopType.Once:
                    {
                        rate = disTime/duration;
                        if( rate > 1 ) {rate = 1;this.enabled = false;}
                        rate = this.m_delEase(0,1f,rate);
                        this.m_cCanvasGroup.alpha = Mathf.Lerp(from , to , rate);
                    }
                    break;
                case LoopType.Loop:
                    {
                        disTime = (disTime - ((int)(disTime/duration))*duration);
                        rate = disTime / duration;
                        rate = this.m_delEase(0,1f,rate);
                        this.m_cCanvasGroup.alpha = Mathf.Lerp(from , to , rate);
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
                        this.m_cCanvasGroup.alpha = Mathf.Lerp(from , to , rate);
                    }
                    break;
            }
        }
    }

    /// <summary>
    /// Start the tweening operation.
    /// </summary>
    static public UI_TweenGroup Begin (GameObject go, float duration, float alpha)
    {
        UI_TweenGroup comp = UI_Tween.Begin<UI_TweenGroup>(go);
        comp.duration = duration;
        comp.to = alpha;

        if (duration <= 0f)
        {
            if( comp.m_cCanvasGroup != null )
                comp.m_cCanvasGroup.alpha = alpha;
            comp.enabled = false;
        }
        return comp;
    }

}


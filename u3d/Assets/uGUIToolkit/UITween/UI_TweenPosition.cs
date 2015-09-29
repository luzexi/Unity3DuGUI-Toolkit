
using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;


//  UI_TweenPosition.cs
//  Author: Lu Zexi
//  2015-02-07


//ui tween position
[AddComponentMenu("uGUI/Tween/UI_Tween Position")]
public class UI_TweenPosition : UI_Tween
{
    public bool fromnow = true;
    public Vector3 from = Vector3.zero;
    public Vector3 to = Vector3.one;
    public LoopType loop = LoopType.PingPong;
    public float duration = 1f;
    public EaseType easeType = EaseType.easeOutExpo;
    public bool isglobal = false;

    float m_fStartTime; //start time
    EasingFunction m_delEase;   //delegate function

    void Awake()
    {
        if(tweenTarget == null) tweenTarget = this.gameObject;
        this.m_fStartTime = Time.realtimeSinceStartup;
        this.m_delEase = GetEasingFunction(easeType);
        if(fromnow)
        {
            if(!isglobal)
                from = tweenTarget.transform.localPosition;
            else
                from = tweenTarget.transform.position;
        }
    }

    void Update()
    {
        if( enabled )
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
                        if(isglobal)
                            tweenTarget.transform.position = Vector3.Lerp(from , to , rate);
                        else
                            tweenTarget.transform.localPosition = Vector3.Lerp(from , to , rate);
                    }
                    break;
                case LoopType.Loop:
                    {
                        disTime = (disTime - ((int)(disTime/duration))*duration);
                        rate = disTime / duration;
                        rate = this.m_delEase(0,1f,rate);
                        if(isglobal)
                            tweenTarget.transform.position = Vector3.Lerp(from , to , rate);
                        else
                            tweenTarget.transform.localPosition = Vector3.Lerp(from , to , rate);
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
                        if(isglobal)
                            tweenTarget.transform.position = Vector3.Lerp(from , to , rate);
                        else
                            tweenTarget.transform.localPosition = Vector3.Lerp(from , to , rate);
                    }
                    break;
            }
        }
    }

    /// <summary>
    /// Start the tweening operation.
    /// </summary>
    static public UI_TweenPosition Begin(GameObject go, float duration, Vector3 pos)
    {
        UI_TweenPosition comp = UI_Tween.Begin<UI_TweenPosition>(go);
        comp.duration = duration;
        comp.to = pos;

        if (duration <= 0f)
        {
            comp.tweenTarget.transform.localPosition = pos;
            comp.enabled = false;
        }
        return comp;
    }

}


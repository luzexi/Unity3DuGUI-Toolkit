
using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;


//  UI_TweenColor.cs
//  Author: Lu Zexi
//  2015-02-07


//ui tween color
[AddComponentMenu("uGUI/Tween/UI_Tween Color")]
public class UI_TweenColor : UI_Tween
{
    public Color from = Color.white;
    public Color to = Color.grey;
    public LoopType loop = LoopType.PingPong;
    public float duration = 1f;
    public EaseType easeType = EaseType.easeOutExpo;

    float m_fStartTime; //start time
    Image m_cImage; //image
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

}


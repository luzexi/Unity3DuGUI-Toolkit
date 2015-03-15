
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;



//  UI_Event.cs
//  Author: Lu Zexi
//  2014-07-06


/// <summary>
/// ui event.
/// </summary>
[CustomLuaClassAttribute]
public class UI_Event : UnityEngine.EventSystems.EventTrigger
{
    private const float CLICK_INTERVAL = 0.2f;
    // private const float CLICK_INTERVAL_POS = 1;

    public delegate void PointerEventDelegate ( PointerEventData eventData , GameObject go , string[] args );
    public delegate void BaseEventDelegate ( BaseEventData eventData , GameObject go , string[] args );
    public delegate void AxisEventDelegate ( AxisEventData eventData , GameObject go , string[] args );

    public string[] m_vecArg = null;

    public BaseEventDelegate onDeselect = null;
    public PointerEventDelegate onDrag = null;
    public PointerEventDelegate onDrop = null;
    public AxisEventDelegate onMove = null;
    public PointerEventDelegate onClick = null;
    public PointerEventDelegate onDown = null;
    public PointerEventDelegate onEnter = null;
    public PointerEventDelegate onExit = null;
    public PointerEventDelegate onUp = null;
    public PointerEventDelegate onScroll = null;
    public BaseEventDelegate onSelect = null;
    public BaseEventDelegate onUpdateSelect = null;

    private float m_fOnDowntime;  //on down time
    private Vector2 m_vecOnDownpos; //on down pos

    public static UI_Event Get(Transform go)
    {
        return Get(go,"");
    }

    public static UI_Event Get(Transform go , string args)
    {
        return Get(go.gameObject , args);
    }

    public static UI_Event Get(MonoBehaviour go)
    {
        return Get(go,"");
    }

    public static UI_Event Get(MonoBehaviour go , string args)
    {
        return Get(go.gameObject,args);
    }

    public static UI_Event Get(GameObject go)
    {
        return Get(go,"");
    }

    public static UI_Event Get(GameObject go , string args)
    {
        UI_Event listener = go.GetComponent<UI_Event>();
        if (listener == null) listener = go.AddComponent<UI_Event>();
        if(!string.IsNullOrEmpty(args))
            listener.m_vecArg = args.Split(new char[]{';'});
        return listener;
    }

    public override void OnDeselect( BaseEventData eventData )
    {
        if(onDeselect != null) onDeselect(eventData , gameObject , this.m_vecArg);
    }

    public override void OnDrag( PointerEventData eventData )
    {
        if(onDrag != null) onDrag(eventData , gameObject , this.m_vecArg);
    }

    public override void OnDrop( PointerEventData eventData )
    {
        if(onDrop != null) onDrop(eventData , gameObject , this.m_vecArg);
    }

    public override void OnMove( AxisEventData eventData )
    {
        if(onMove != null) onMove(eventData , gameObject , this.m_vecArg);
    }

    public override void OnPointerClick(PointerEventData eventData)
    {
        if( Time.realtimeSinceStartup -  this.m_fOnDowntime > CLICK_INTERVAL )
        {
            return;
        }
        // if( (eventData.position - this.m_vecOnDownpos).magnitude > CLICK_INTERVAL_POS )
        // {
        //     return;
        // }
        if(onClick != null) onClick(eventData , gameObject , this.m_vecArg);
    }

    public override void OnPointerDown (PointerEventData eventData)
    {
        this.m_fOnDowntime = Time.realtimeSinceStartup;
        this.m_vecOnDownpos = eventData.position;
        if(onDown != null) onDown(eventData , gameObject , this.m_vecArg);
    }

    public override void OnPointerEnter (PointerEventData eventData)
    {
        if(onEnter != null) onEnter(eventData , gameObject , this.m_vecArg);
    }

    public override void OnPointerExit (PointerEventData eventData)
    {
        if(onExit != null) onExit(eventData , gameObject , this.m_vecArg);
    }
    public override void OnPointerUp (PointerEventData eventData)
    {
        if(onUp != null) onUp(eventData , gameObject , this.m_vecArg);
    }

    public override void OnScroll( PointerEventData eventData )
    {
        if(onScroll != null) onScroll(eventData , gameObject , this.m_vecArg);
    }
    public override void OnSelect (BaseEventData eventData)
    {
        if(onSelect != null) onSelect(eventData , gameObject , this.m_vecArg);
    }

    public override void OnUpdateSelected (BaseEventData eventData)
    {
        if(onUpdateSelect != null) onUpdateSelect(eventData , gameObject , this.m_vecArg);
    }
}


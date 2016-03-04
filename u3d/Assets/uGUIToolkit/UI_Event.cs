
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
public class UI_Event : UnityEngine.EventSystems.EventTrigger
{
    private const float CLICK_INTERVAL_TIME = 0.2f; //const click interval time
    private const float CLICK_INTERVAL_POS = 2; //const click interval pos

    public delegate void PointerEventDelegate ( PointerEventData eventData , UI_Event go);
    public delegate void BaseEventDelegate ( BaseEventData eventData , UI_Event go);
    public delegate void AxisEventDelegate ( AxisEventData eventData , UI_Event go);

    public Dictionary<string,object> mArg = new Dictionary<string,object>();

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

    public void AddData(string key , object val)
    {
        mArg[key] = val;
    }

    public D GetData<D>(string key)
    {
        if(mArg.ContainsKey(key))
        {
            return (D)mArg[key];
        }
        return default(D);
    }

    public static UI_Event Get(Transform go)
    {
        return Get(go.gameObject);
    }

    public static UI_Event Get(Component go)
    {
        return Get(go.gameObject);
    }

    public static UI_Event Get(GameObject go)
    {
        UI_Event listener = go.GetComponent<UI_Event>();
        if (listener == null) listener = go.AddComponent<UI_Event>();
        return listener;
    }

    public static U Get<U>(Transform go)
        where U: UI_Event
    {
        return Get<U>(go.gameObject);
    }

    public static U Get<U>(Component go)
        where U: UI_Event
    {
        return Get<U>(go.gameObject);
    }

    public static U Get<U>(GameObject go)
        where U: UI_Event
    {
        U listener = go.GetComponent<U>();
        if (listener == null) listener = go.AddComponent<U>();
        return listener;
    }

    public override void OnDeselect( BaseEventData eventData )
    {
        if(onDeselect != null) onDeselect(eventData , this);
    }

    public override void OnDrag( PointerEventData eventData )
    {
        if(onDrag != null) onDrag(eventData , this);
    }

    public override void OnDrop( PointerEventData eventData )
    {
        if(onDrop != null) onDrop(eventData , this);
    }

    public override void OnMove( AxisEventData eventData )
    {
        if(onMove != null) onMove(eventData , this);
    }

    public override void OnPointerClick(PointerEventData eventData)
    {
        // if( Time.realtimeSinceStartup -  this.m_fOnDowntime > CLICK_INTERVAL_TIME )
        // {
        //     return;
        // }
        // // if( (eventData.position - this.m_vecOnDownpos).magnitude > CLICK_INTERVAL_POS )
        // if( eventData.delta.magnitude > CLICK_INTERVAL_POS )
        // {
        //     return;
        // }
        if(onClick != null) onClick(eventData , this);
    }

    public override void OnPointerDown (PointerEventData eventData)
    {
        this.m_fOnDowntime = Time.realtimeSinceStartup;
        this.m_vecOnDownpos = eventData.position;
        if(onDown != null) onDown(eventData , this);
    }

    public override void OnPointerEnter (PointerEventData eventData)
    {
        if(onEnter != null) onEnter(eventData , this);
    }

    public override void OnPointerExit (PointerEventData eventData)
    {
        if(onExit != null) onExit(eventData , this);
    }
    public override void OnPointerUp (PointerEventData eventData)
    {
        if(onUp != null) onUp(eventData , this);
    }

    public override void OnScroll( PointerEventData eventData )
    {
        if(onScroll != null) onScroll(eventData , this);
    }
    public override void OnSelect (BaseEventData eventData)
    {
        if(onSelect != null) onSelect(eventData , this);
    }

    public override void OnUpdateSelected (BaseEventData eventData)
    {
        if(onUpdateSelect != null) onUpdateSelect(eventData , this);
    }
}



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
    protected const float CLICK_INTERVAL_TIME = 0.2f; //const click interval time
    protected const float CLICK_INTERVAL_POS = 2; //const click interval pos

    public static bool sIsEvent = false;    //can use in 3d camera to point out which event should be use
    public static bool sDisableEvent = false;   //can use in toturial when u wanna allow user only can click this button
    public static bool sDisableEvent2 = false;  //can use in toturial when u wanna allow user only can click this button
    public bool mIgnoreDisable = false;
    protected bool mAnyMove = false;

    public delegate void PointerEventDelegate ( PointerEventData eventData , UI_Event ev);
    public delegate void BaseEventDelegate ( BaseEventData eventData , UI_Event ev);
    public delegate void AxisEventDelegate ( AxisEventData eventData , UI_Event ev);

    public Dictionary<string,object> mArg = new Dictionary<string,object>();

    public BaseEventDelegate onDeselect = null;
    public PointerEventDelegate onBeginDrag = null;
    public PointerEventDelegate onDrag = null;
    public PointerEventDelegate onEndDrag = null;
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
    public BaseEventDelegate onCancel = null;
    public PointerEventDelegate onInitializePotentialDrag = null;
    public BaseEventDelegate onSubmit = null;

    protected float m_fOnDowntime;  //on down time
    protected Vector2 m_vecOnDownpos; //on down pos

    //set arg
    public void SetData(string key , object val)
    {
        mArg[key] = val;
    }

    public void RemoveData(string key)
    {
        if(mArg.ContainsKey(key))
            mArg.Remove(key);
    }

    // get arg
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
        if(sDisableEvent2) return;
        if(sDisableEvent) return;
        // sIsEvent = true;
        if(onDeselect != null)
        {
            sIsEvent = true;
            onDeselect(eventData , this);
        }
    }

    public override void OnBeginDrag( PointerEventData eventData )
    {
        mAnyMove = true;
        if(sDisableEvent2 && !mIgnoreDisable) return;
        if(sDisableEvent && !mIgnoreDisable) return;
        sIsEvent = true;
        if(onBeginDrag != null)
        {
            // sIsEvent = true;
            onBeginDrag(eventData , this);
        }
    }

    public override void OnDrag( PointerEventData eventData )
    {
        if(sDisableEvent2 && !mIgnoreDisable) return;
        if(sDisableEvent && !mIgnoreDisable) return;
        sIsEvent = true;
        if(onDrag != null)
        {
            // sIsEvent = true;
            onDrag(eventData , this);
        }
    }

    public override void OnEndDrag( PointerEventData eventData )
    {
        mAnyMove = false;
        if(sDisableEvent2 && !mIgnoreDisable) return;
        if(sDisableEvent && !mIgnoreDisable) return;
        sIsEvent = true;
        if(onEndDrag != null)
        {
            // sIsEvent = true;
            onEndDrag(eventData , this);
        }
    }

    public override void OnDrop( PointerEventData eventData )
    {
        if(sDisableEvent2) return;
        if(sDisableEvent) return;
        sIsEvent = true;
        if(onDrop != null)
        {
            // sIsEvent = true;
            onDrop(eventData , this);
        }
    }

    public override void OnMove( AxisEventData eventData )
    {
        if(sDisableEvent2) return;
        if(sDisableEvent) return;
        // sIsEvent = true;
        if(onMove != null)
        {
            sIsEvent = true;
            onMove(eventData , this);
        }
    }

    public override void OnPointerClick(PointerEventData eventData)
    {
        if(mAnyMove) return;
        if(sDisableEvent && !mIgnoreDisable) return;
        sIsEvent = true;
        if(onClick != null)
        {
            // sIsEvent = true;
            onClick(eventData , this);
        }
    }

    public override void OnPointerDown (PointerEventData eventData)
    {
        if(sDisableEvent2) return;
        if(sDisableEvent) return;
        this.m_fOnDowntime = Time.realtimeSinceStartup;
        this.m_vecOnDownpos = eventData.position;
        sIsEvent = true;
        if(onDown != null)
        {
            // sIsEvent = true;
            onDown(eventData , this);
        }
    }

    public override void OnPointerEnter (PointerEventData eventData)
    {
        if(sDisableEvent2) return;
        if(sDisableEvent) return;
        // sIsEvent = true;
        if(onEnter != null)
        {
            sIsEvent = true;
            onEnter(eventData , this);
        }
    }

    public override void OnPointerExit (PointerEventData eventData)
    {
        if(sDisableEvent2) return;
        if(sDisableEvent) return;
        // sIsEvent = true;
        if(onExit != null)
        {
            sIsEvent = true;
            onExit(eventData , this);
        }
    }
    public override void OnPointerUp (PointerEventData eventData)
    {
        if(sDisableEvent2) return;
        if(sDisableEvent) return;
        sIsEvent = true;
        if(onUp != null)
        {
            // sIsEvent = true;
            onUp(eventData , this);
        }
    }

    public override void OnScroll( PointerEventData eventData )
    {
        if(sDisableEvent2) return;
        if(sDisableEvent) return;
        // sIsEvent = true;
        if(onScroll != null)
        {
            sIsEvent = true;
            onScroll(eventData , this);
        }
    }
    public override void OnSelect (BaseEventData eventData)
    {
        if(sDisableEvent2) return;
        if(sDisableEvent) return;
        // sIsEvent = true;
        if(onSelect != null)
        {
            sIsEvent = true;
            onSelect(eventData , this);
        }
    }

    public override void OnUpdateSelected (BaseEventData eventData)
    {
        if(sDisableEvent2) return;
        if(sDisableEvent) return;
        // sIsEvent = true;
        if(onUpdateSelect != null)
        {
            sIsEvent = true;
            onUpdateSelect(eventData , this);
        }
    }

    public override void OnCancel (BaseEventData eventData)
    {
        if(sDisableEvent2) return;
        if(sDisableEvent) return;
        // sIsEvent = true;
        if(onCancel != null)
        {
            sIsEvent = true;
            onCancel(eventData , this);
        }
    }

    public override void OnInitializePotentialDrag (PointerEventData eventData)
    {
        if(sDisableEvent2) return;
        if(sDisableEvent) return;
        // sIsEvent = true;
        if(onInitializePotentialDrag != null)
        {
            sIsEvent = true;
            onInitializePotentialDrag(eventData , this);
        }
    }

    public override void OnSubmit(BaseEventData eventData)
    {
        if(sDisableEvent2) return;
        if(sDisableEvent) return;
        // sIsEvent = true;
        if(onSubmit != null)
        {
            sIsEvent = true;
            onSubmit(eventData , this);
        }
    }
}



using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;
using System.Collections.Generic;


//  UI_Scroll_Horizontal_Item.cs
//  Author: Lu Zexi
//  2015-02-17



[AddComponentMenu("uGUI/UI_Scroll_Horizontal Item")]
public class UI_Scroll_Horizontal_Item : MonoBehaviour
{
    public List<GameObject> ScrollItems = new List<GameObject>();

    public int MaxPosX; //max position x
    public int MinPosX; //min position x

    public int RepositionMax;   //reposition max x
    public int RepositionMin;   //reposition min x

    private float m_fMove_rate = 1f;    //move rate

    private float m_fMove_speed = 1f;   //move speed

    void Awake()
    {
        //regist event
        for( int i = 0 ; i<ScrollItems.Count ; i++ )
        {
            GameObject go = ScrollItems[i];
            UI_Event ev = UI_Event.Get(go);
            ev.onDown += OnDown;
            ev.onUp += OnUp;
            ev.onDrag += OnDrag;
        }

        //calculate move rate
        Transform node = this.transform;
        while(true)
        {
            CanvasScaler cs = node.GetComponent<CanvasScaler>();
            if(cs != null)
            {
                this.m_fMove_rate = cs.referenceResolution.x / Screen.width;
                break;
            }
            node = node.parent;
            if( node == null )
                break;
        }
    }

    void OnDrag( PointerEventData eventData , GameObject go , string[] args )
    {
        foreach( GameObject obj in this.ScrollItems )
        {
            obj.transform.localPosition += new Vector3(eventData.delta.x * this.m_fMove_rate , 0 , 0);
        }
        foreach( GameObject obj in this.ScrollItems )
        {
            if( obj.transform.localPosition.x < this.MinPosX )
            {
                obj.transform.localPosition = new Vector3( this.RepositionMax + (obj.transform.localPosition.x - this.MinPosX) , obj.transform.localPosition.y , obj.transform.localPosition.z );
            }
            if( obj.transform.localPosition.x > this.MaxPosX )
            {
                obj.transform.localPosition = new Vector3( this.RepositionMin + (obj.transform.localPosition.x - this.MaxPosX) , obj.transform.localPosition.y , obj.transform.localPosition.z );
            }
        }
    }

    void OnDown( PointerEventData eventData , GameObject go , string[] args )
    {
        //
    }

    void OnUp( PointerEventData eventData , GameObject go , string[] args )
    {
        //
    }

    void Update()
    {
        //
    }
}


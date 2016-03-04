
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;
using System.Collections.Generic;


//  UI_Scroll_List.cs
//  Author: Lu Zexi
//  2015-02-17



[AddComponentMenu("uGUI/UI_Scroll_List")]
public class UI_Scroll_List : UI_ComponentBase
{
    public enum Movement
    {
        Horizontal,
        Vertical,
    }

    private List<GameObject> ScrollItems = new List<GameObject>();

    public Movement moveType = Movement.Horizontal;

    public float MaxSpeed = 50;    //max speed
    public float MinFixSpeed = 5;   //min fix speed

    public int MaxIndex;    //max index
    public int MaxFixPos;  //fix position x|y
    public int MinFixPos;  //fix position x|y
    public int ItemNum; //scroll item num
    public int ItemCost;    //scroll item cost space

    public System.Action<GameObject,int> onChange = null;

    private int MaxPos; //max position x|y
    private int MinPos; //min position x|y
    private int RepositionMax;   //reposition max x|y
    private int RepositionMin;   //reposition min x|y

    private float m_fMove_rate = 1f;    //move rate
    private int m_iNowIndex = 0;    //now index

    private float m_fMove_speed = 0f;   //move speed
    private int m_iMoveDir = 1;    //move dir
    private float m_fMoveStartTime = 0; //move start time
    private bool m_bDrag = false;   //is drag

    private bool m_bFixPos = false; //is fix pos
    private GameObject m_cFixObj = null;    //fix gameobjct
    private float m_fFixSpeed = 0; //fix speed

    //init
    public void Init()
    {
        this.RepositionMax = this.MaxFixPos + this.ItemCost;
        this.RepositionMin = this.MinFixPos - this.ItemCost;
        this.MaxPos = this.MinFixPos + this.ItemCost * (this.ItemNum-1);
        this.MinPos = this.MaxFixPos - this.ItemCost * (this.ItemNum-1);
    }

    //add drag event
    public void AddDragEvent( GameObject go )
    {
        UI_Event ev = UI_Event.Get(go);
        ev.onDown += OnDown;
        ev.onUp += OnUp;
        ev.onDrag += OnDrag;
    }

    void Awake()
    {
        this.RepositionMax = this.MaxFixPos + this.ItemCost;
        this.RepositionMin = this.MinFixPos - this.ItemCost;
        this.MaxPos = this.MinFixPos + this.ItemCost * (this.ItemNum-1);
        this.MinPos = this.MaxFixPos - this.ItemCost * (this.ItemNum-1);
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

    void Start()
    {
        this.ScrollItems.Clear();
        if(this.ScrollItems.Count <= 0 )
        {
            foreach( Transform item in this.transform )
            {
                this.ScrollItems.Add(item.gameObject);
            }
            //regist event
            for( int i = 0 ; i<ScrollItems.Count ; i++ )
            {
                GameObject go = ScrollItems[i];
                UI_Event ev = UI_Event.Get(go);
                ev.onDown += OnDown;
                ev.onUp += OnUp;
                ev.onDrag += OnDrag;
            }
        }
        UI_Event ev1 = UI_Event.Get(this.gameObject);
        ev1.onDown += OnDown;
        ev1.onUp += OnUp;
        ev1.onDrag += OnDrag;
    }

    void OnDrag( PointerEventData eventData , UI_Event go )
    {
        this.m_fMove_speed = 0;
        this.m_iMoveDir = 0;
        if(this.moveType == Movement.Horizontal)
        {
            this.m_fMove_speed = Mathf.Abs(eventData.delta.x);
            this.m_iMoveDir = eventData.delta.x > 0 ? 1 : -1;
        }
        else
        {
            this.m_fMove_speed = Mathf.Abs(eventData.delta.y);
            this.m_iMoveDir = eventData.delta.y > 0 ? 1 : -1;
        }
        if( this.m_fMove_speed > MaxSpeed )
            this.m_fMove_speed = MaxSpeed;

        foreach( GameObject obj in this.ScrollItems )
        {
            Vector3 mv = Vector3.zero;
            if( this.moveType == Movement.Horizontal )
                mv = new Vector3(eventData.delta.x * this.m_fMove_rate , 0 , 0);
            else
                mv = new Vector3( 0 , eventData.delta.y * this.m_fMove_rate , 0);
            obj.transform.localPosition += mv;
        }
        foreach( GameObject obj in this.ScrollItems )
        {
            if( moveType == Movement.Horizontal )
            {
                if( obj.transform.localPosition.x < this.MinPos && this.m_iNowIndex > 0 )
                {
                    obj.transform.localPosition = new Vector3( this.RepositionMax + (obj.transform.localPosition.x - this.MinPos) , obj.transform.localPosition.y , obj.transform.localPosition.z );
                    if(this.onChange != null)
                    {
                        this.onChange(obj , 0);
                    }
                    this.m_iNowIndex--;
                }
                if( obj.transform.localPosition.x > this.MaxPos && this.m_iNowIndex < this.MaxIndex)
                {
                    obj.transform.localPosition = new Vector3( this.RepositionMin + (obj.transform.localPosition.x - this.MaxPos) , obj.transform.localPosition.y , obj.transform.localPosition.z );
                    if(this.onChange != null)
                    {
                        this.onChange(obj , 1);
                    }
                    this.m_iNowIndex++;
                }
            }
            else
            {
                if( obj.transform.localPosition.y < this.MinPos && this.m_iNowIndex > 0 )
                {
                    obj.transform.localPosition = new Vector3( obj.transform.localPosition.x , this.RepositionMax + (obj.transform.localPosition.y - this.MinPos) , obj.transform.localPosition.z );
                    if(this.onChange != null)
                    {
                        this.onChange(obj , 0);
                    }
                    this.m_iNowIndex--;
                }
                if( obj.transform.localPosition.y > this.MaxPos && this.m_iNowIndex < this.MaxIndex )
                {
                    obj.transform.localPosition = new Vector3( obj.transform.localPosition.x , this.RepositionMin + (obj.transform.localPosition.y - this.MaxPos) , obj.transform.localPosition.z );
                    if(this.onChange != null)
                    {
                        this.onChange(obj , 1);
                    }
                    this.m_iNowIndex++;
                }
            }
        }
    }

    void OnDown( PointerEventData eventData , UI_Event go )
    {
        this.m_bDrag = true;
        this.m_fMove_speed = 0;
        this.m_bFixPos = false;
    }

    void OnUp( PointerEventData eventData , UI_Event go )
    {
        this.m_bDrag = false;
        this.m_fMoveStartTime = Time.time;
        if(this.m_iNowIndex <= 0 && this.m_iMoveDir < 0)
        {
            this.m_fMove_speed = 0;
            this.m_bFixPos = true;
            this.m_fFixSpeed = 0;
            return;
        }

        if(this.m_iNowIndex >= this.MaxIndex && this.m_iMoveDir > 0)
        {
            this.m_fMove_speed = 0;
            this.m_bFixPos = true;
            this.m_fFixSpeed = 0;
            return;
        }
    }

    void LateUpdate()
    {
        if( !this.m_bDrag && this.m_fMove_speed > 0)
        {
            if(this.m_iNowIndex <= 0 && this.m_iMoveDir < 0)
            {
                this.m_fMove_speed = 0;
                this.m_bFixPos = true;
                this.m_fFixSpeed = 0;
                return;
            }

            if(this.m_iNowIndex >= this.MaxIndex && this.m_iMoveDir > 0)
            {
                this.m_fMove_speed = 0;
                this.m_bFixPos = true;
                this.m_fFixSpeed = 0;
                return;
            }

            foreach( GameObject obj in this.ScrollItems )
            {
                Vector3 mv = Vector3.zero;
                if( this.moveType == Movement.Horizontal )
                    mv = new Vector3(this.m_fMove_speed * this.m_iMoveDir * this.m_fMove_rate , 0 , 0);
                else
                    mv = new Vector3( 0 , this.m_fMove_speed * this.m_iMoveDir * this.m_fMove_rate , 0);
                obj.transform.localPosition += mv;
            }
            foreach( GameObject obj in this.ScrollItems )
            {
                if( moveType == Movement.Horizontal )
                {
                    if( obj.transform.localPosition.x < this.MinPos )
                    {
                        if(this.m_iNowIndex > 0)
                        {
                            obj.transform.localPosition = new Vector3( this.RepositionMax + (obj.transform.localPosition.x - this.MinPos) , obj.transform.localPosition.y , obj.transform.localPosition.z );
                            if(this.onChange != null)
                                this.onChange(obj , 0);
                            this.m_iNowIndex--;
                        }
                        else
                        {
                            this.m_fMove_speed = 0;
                            this.m_bFixPos = true;
                            this.m_fFixSpeed = 0;
                        }
                    }
                    if( obj.transform.localPosition.x > this.MaxPos )
                    {
                        if(this.m_iNowIndex < this.MaxIndex)
                        {
                            obj.transform.localPosition = new Vector3( this.RepositionMin + (obj.transform.localPosition.x - this.MaxPos) , obj.transform.localPosition.y , obj.transform.localPosition.z );
                            if(this.onChange != null)
                                this.onChange(obj , 1);
                            this.m_iNowIndex++;
                        }
                        else
                        {
                            this.m_fMove_speed = 0;
                            this.m_bFixPos = true;
                            this.m_fFixSpeed = 0;
                        }
                    }
                }
                else
                {
                    if( obj.transform.localPosition.y < this.MinPos )
                    {
                        if(this.m_iNowIndex > 0 )
                        {
                            obj.transform.localPosition = new Vector3( obj.transform.localPosition.x , this.RepositionMax + (obj.transform.localPosition.y - this.MinPos) , obj.transform.localPosition.z );
                            if(this.onChange != null)
                                this.onChange(obj , 0);
                            this.m_iNowIndex--;
                        }
                        else
                        {
                            this.m_fMove_speed = 0;
                            this.m_bFixPos = true;
                            this.m_fFixSpeed = 0;
                        }
                    }
                    if( obj.transform.localPosition.y > this.MaxPos )
                    {
                        if(this.m_iNowIndex < this.MaxIndex)
                        {
                            obj.transform.localPosition = new Vector3( obj.transform.localPosition.x , this.RepositionMin + (obj.transform.localPosition.y - this.MaxPos) , obj.transform.localPosition.z );
                            if(this.onChange != null)
                                this.onChange(obj , 1);
                            this.m_iNowIndex++;
                        }
                        else
                        {
                            this.m_fMove_speed = 0;
                            this.m_bFixPos = true;
                            this.m_fFixSpeed = 0;
                        }
                    }
                }
            }
            float disTime = Time.time - this.m_fMoveStartTime;
            this.m_fMove_speed -= Mathf.Lerp(0,15,disTime);
        }
        if(this.m_bFixPos)
        {
            float FixPos = 0;
            if(this.m_iNowIndex >= this.MaxIndex)
                FixPos = this.MinFixPos;
            if(this.m_iNowIndex <= 0)
                FixPos = this.MaxFixPos;

            this.m_fFixSpeed += this.MinFixSpeed;
            float fixspeed = this.m_fFixSpeed;
            float minDis = 0xffffff;
            if(this.m_iNowIndex <= 0 )
                minDis *= -1;
            else
                minDis *= 1;
            foreach( GameObject obj in this.ScrollItems )
            {
                float dis = 0;
                if( this.moveType == Movement.Horizontal )
                    dis = obj.transform.localPosition.x;
                else
                    dis = obj.transform.localPosition.y;
                if(this.m_iNowIndex <= 0)
                {
                    if( dis > minDis )
                    {
                        this.m_cFixObj = obj;
                        minDis = dis;
                    }
                }
                else
                {
                    if( dis < minDis )
                    {
                        this.m_cFixObj = obj;
                        minDis = dis;
                    }
                }
            }
            float main_pos = 0;
            if( this.moveType == Movement.Horizontal )
                main_pos = FixPos - this.m_cFixObj.transform.localPosition.x;
            else
                main_pos = FixPos - this.m_cFixObj.transform.localPosition.y;
            if( main_pos < 0 )
                fixspeed *= -1;

            //
            Vector3 fixOffset = Vector3.zero;
            if(this.moveType == Movement.Horizontal)
            {
                float dis = FixPos - this.m_cFixObj.transform.localPosition.x;
                if( Mathf.Abs(fixspeed) > Mathf.Abs(dis) )
                {
                    fixspeed = dis;
                }
                fixOffset = new Vector3( fixspeed , 0 , 0 );
            }
            else
            {
                float dis = FixPos - this.m_cFixObj.transform.localPosition.y;
                if( Mathf.Abs(fixspeed) > Mathf.Abs(dis) )
                {
                    fixspeed = dis;
                }
                fixOffset = new Vector3( 0 , fixspeed , 0 );
            }
            foreach( GameObject obj in this.ScrollItems )
            {
                obj.transform.localPosition += fixOffset;
            }
            if( this.moveType == Movement.Horizontal )
            {
                if(this.m_cFixObj.transform.localPosition.x == FixPos)
                    this.m_bFixPos = false;
            }
            else
            {
                if(this.m_cFixObj.transform.localPosition.y == FixPos)
                    this.m_bFixPos = false;
            }
        }
    }
}


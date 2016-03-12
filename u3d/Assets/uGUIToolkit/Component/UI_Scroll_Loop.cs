
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;
using System.Collections.Generic;


//  UI_Scroll_Loop.cs
//  Author: Lu Zexi
//  2015-02-17



[AddComponentMenu("uGUI/UI_Scroll_Loop")]
public class UI_Scroll_Loop : MonoBehaviour
{
    public enum Movement
    {
        Horizontal,
        Vertical,
    }

    [SerializeField]
    public List<GameObject> ScrollItems = new List<GameObject>();   //object list
    [SerializeField]
    public Movement moveType = Movement.Horizontal; //move type
    [SerializeField]
    public int StartIndex = 0;  //start index
    [SerializeField]
    public float MaxSpeed = 50;    //max speed
    [SerializeField]
    public float MinFixSpeed = 5;   //min fix speed
    [SerializeField]
    public int Size = 200;    //size of item
    [SerializeField]
    public int LeftCount = 1; //remain left count
    [SerializeField]
    public int RightCount = 1;    //remain right count

    private int MaxPos; //max position x
    private int MinPos; //min position x

    private int RepositionMax;   //reposition max x
    private int RepositionMin;   //reposition min x

    private float m_fMove_rate = 1f;    //move rate

    private float m_fMove_speed = 0f;   //move speed
    private int m_iMoveDir = 1;    //move dir
    private float m_fMoveStartTime = 0;
    private bool m_bDrag = false;   //is drag

    private bool m_bFixPos = false; //fix pos
    private GameObject m_cFixObj = null;    //fix gameobjct
    private float m_fFixSpeed = 0; //fix speed

    void Awake()
    {
        Refresh();
    }

    public void Refresh()
    {
        this.m_fMove_rate = 1;
        this.m_fMove_speed = 0;
        this.m_iMoveDir = 1;
        this.m_fMoveStartTime = 0;
        this.m_bDrag = false;
        this.m_bFixPos = false;
        this.m_cFixObj = null;
        this.m_fFixSpeed = 0;

        // this.Size = this.ScrollItems[0].rect.width;
        MaxPos = (int)(0 + this.Size * (this.ScrollItems.Count-this.LeftCount));
        MinPos = (int)(0 - this.Size * (this.ScrollItems.Count-this.RightCount));

        RepositionMax = (int)(0 + this.Size*(this.RightCount));
        RepositionMin = (int)(0 - this.Size*(this.LeftCount));

        for(int i = this.StartIndex ; i<this.ScrollItems.Count-this.LeftCount ;i++)
        {
            if(moveType == Movement.Horizontal)
                this.ScrollItems[i-this.StartIndex].transform.localPosition = new Vector3((i-this.StartIndex)*this.Size, 0 , 0);
            else
                this.ScrollItems[i-this.StartIndex].transform.localPosition = new Vector3(0, (i-this.StartIndex)*this.Size , 0);
        }
        for(int i = 0 ; i<this.LeftCount ;i++)
        {
            int index = this.StartIndex - 1 - i;
            if(index < 0)
            {
                index = this.ScrollItems.Count + index;
            }
            if(moveType == Movement.Horizontal)
                this.ScrollItems[index].transform.localPosition = new Vector3((i+1)*this.Size*-1, 0 , 0);
            else
                this.ScrollItems[index].transform.localPosition = new Vector3(0, (i+1)*this.Size*-1 , 0);
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

    void OnDrag( PointerEventData eventData , UI_Event go )
    {
        this.m_fMove_speed = 0;
        this.m_iMoveDir = 1;
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
                if( obj.transform.localPosition.x < this.MinPos )
                {
                    obj.transform.localPosition = new Vector3( this.RepositionMax + (obj.transform.localPosition.x - this.MinPos) , obj.transform.localPosition.y , obj.transform.localPosition.z );
                }
                if( obj.transform.localPosition.x > this.MaxPos )
                {
                    obj.transform.localPosition = new Vector3( this.RepositionMin + (obj.transform.localPosition.x - this.MaxPos) , obj.transform.localPosition.y , obj.transform.localPosition.z );
                }
            }
            else
            {
                if( obj.transform.localPosition.y < this.MinPos )
                {
                    obj.transform.localPosition = new Vector3( obj.transform.localPosition.x , this.RepositionMax + (obj.transform.localPosition.y - this.MinPos) , obj.transform.localPosition.z );
                }
                if( obj.transform.localPosition.y > this.MaxPos )
                {
                    obj.transform.localPosition = new Vector3( obj.transform.localPosition.x , this.RepositionMin + (obj.transform.localPosition.y - this.MaxPos) , obj.transform.localPosition.z );
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
        if( this.m_fMove_speed == 0 )
        {
            this.m_bFixPos = true;
            this.m_fFixSpeed = this.m_fMove_speed;
            if( this.m_fFixSpeed < MinFixSpeed )
                this.m_fFixSpeed = MinFixSpeed;
            float minDis = 0xffffff;
            foreach( GameObject obj in this.ScrollItems )
            {
                float dis = 0;
                if( this.moveType == Movement.Horizontal )
                    dis = Mathf.Abs(obj.transform.localPosition.x);
                else
                    dis = Mathf.Abs(obj.transform.localPosition.y);
                if( dis < minDis )
                {
                    this.m_cFixObj = obj;
                    minDis = dis;
                }
            }
            float main_pos = 0;
            if( this.moveType == Movement.Horizontal )
                main_pos = 0 - this.m_cFixObj.transform.localPosition.x;
            else
                main_pos = 0 - this.m_cFixObj.transform.localPosition.y;
            if( main_pos < 0 )
                this.m_fFixSpeed *= -1;
        }
    }

    void LateUpdate()
    {
        if( !this.m_bDrag && this.m_fMove_speed > 0)
        {
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
                        obj.transform.localPosition = new Vector3( this.RepositionMax + (obj.transform.localPosition.x - this.MinPos) , obj.transform.localPosition.y , obj.transform.localPosition.z );
                    }
                    if( obj.transform.localPosition.x > this.MaxPos )
                    {
                        obj.transform.localPosition = new Vector3( this.RepositionMin + (obj.transform.localPosition.x - this.MaxPos) , obj.transform.localPosition.y , obj.transform.localPosition.z );
                    }
                }
                else
                {
                    if( obj.transform.localPosition.y < this.MinPos )
                    {
                        obj.transform.localPosition = new Vector3( obj.transform.localPosition.x , this.RepositionMax + (obj.transform.localPosition.y - this.MinPos) , obj.transform.localPosition.z );
                    }
                    if( obj.transform.localPosition.y > this.MaxPos )
                    {
                        obj.transform.localPosition = new Vector3( obj.transform.localPosition.x , this.RepositionMin + (obj.transform.localPosition.y - this.MaxPos) , obj.transform.localPosition.z );
                    }
                }
            }
            float disTime = Time.time - this.m_fMoveStartTime;
            if(this.m_fMove_speed - Mathf.Lerp(0,15,disTime) < 0 )
            {
                this.m_bFixPos = true;
                this.m_fFixSpeed = this.m_fMove_speed;
                if( this.m_fFixSpeed < MinFixSpeed )
                    this.m_fFixSpeed = MinFixSpeed;
                float minDis = 0xffffff;
                foreach( GameObject obj in this.ScrollItems )
                {
                    float dis = 0;
                    if( this.moveType == Movement.Horizontal )
                        dis = Mathf.Abs(obj.transform.localPosition.x);
                    else
                        dis = Mathf.Abs(obj.transform.localPosition.y);
                    if( dis < minDis )
                    {
                        this.m_cFixObj = obj;
                        minDis = dis;
                    }
                }
                float main_pos = 0;
                if( this.moveType == Movement.Horizontal )
                    main_pos = 0 - this.m_cFixObj.transform.localPosition.x;
                else
                    main_pos = 0 - this.m_cFixObj.transform.localPosition.y;
                if( main_pos < 0 )
                    this.m_fFixSpeed *= -1;
            }
            this.m_fMove_speed -= Mathf.Lerp(0,15,disTime);
        }
        if(this.m_bFixPos)
        {
            Vector3 fixOffset = Vector3.zero;
            if(this.moveType == Movement.Horizontal)
            {
                float dis = 0 - this.m_cFixObj.transform.localPosition.x;
                if( Mathf.Abs(this.m_fFixSpeed) > Mathf.Abs(dis) )
                {
                    this.m_fFixSpeed = dis;
                }
                fixOffset = new Vector3( this.m_fFixSpeed , 0 , 0 );
            }
            else
            {
                float dis = 0 - this.m_cFixObj.transform.localPosition.y;
                if( Mathf.Abs(this.m_fFixSpeed) > Mathf.Abs(dis) )
                {
                    this.m_fFixSpeed = dis;
                }
                fixOffset = new Vector3( 0 , this.m_fFixSpeed , 0 );
            }
            foreach( GameObject obj in this.ScrollItems )
            {
                obj.transform.localPosition += fixOffset;
            }
            if( this.moveType == Movement.Horizontal )
            {
                if(this.m_cFixObj.transform.localPosition.x == 0)
                    this.m_bFixPos = false;
            }
            else
            {
                if(this.m_cFixObj.transform.localPosition.y == 0)
                    this.m_bFixPos = false;
            }
        }
    }
}


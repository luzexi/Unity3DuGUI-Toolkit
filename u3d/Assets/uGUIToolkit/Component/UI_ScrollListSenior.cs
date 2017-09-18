
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;
using System.Collections.Generic;


//  UI_Scroll_List.cs
//  Author: Lu Zexi
//  2015-02-17



[AddComponentMenu("UI/UI ScrollListSenior")]
[RequireComponent (typeof(UI_ListGrid))]
public class UI_ScrollListSenior : MonoBehaviour
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

    private UI_ListGrid Grid = null;    //grid component
    public int Total=100;    //max index
    private int MaxIndex=0;    //max index
    private int MaxFixPos=0;  //fix position x|y
    private int MinFixPos=0;  //fix position x|y
    private int ItemNum=0; //scroll item num
    private int ItemCost=100;    //scroll item cost space

    private System.Action<GameObject,int> onChange = null;

    private int MaxPos=0; //max position x|y
    private int MinPos=0; //min position x|y
    private int RepositionMax=0;   //reposition max x|y
    private int RepositionMin=0;   //reposition min x|y

    private float m_fMove_rate = 1f;    //move rate
    private int m_iNowIndex = 0;    //now index

    private float m_fMove_speed = 0f;   //move speed
    private int m_iMoveDir = 1;    //move dir
    private float m_fMoveStartTime = 0; //move start time
    private bool m_bDrag = false;   //is drag

    private bool m_bFixPos = false; //is fix pos
    private GameObject m_cFixObj = null;    //fix gameobjct
    private float m_fFixSpeed = 0; //fix speed

    public void SetInitUnitHandle(System.Action<GameObject,int> change)
    {
        this.onChange = change;
    }

    private void OnTest(GameObject go , int index)
    {
        Debug.LogError("change index " + index);
    }

    //init
    public void Init()
    {
        // this.onChange = OnTest;
        this.ScrollItems.Clear();
        for(int i = 0 ; i<this.transform.childCount ;i++)
        {
            GameObject go = this.transform.GetChild(i).gameObject;
            this.ScrollItems.Add(go);
            if(onChange!=null)
            {
                onChange(go,i);
            }
            if(i>=this.Total)
            {
                go.SetActive(false);
            }
        }

        this.Grid = this.gameObject.GetComponent<UI_ListGrid>();
        this.Grid.Refresh();
        this.ItemCost = Grid.IntervalLine;
        this.ItemNum = this.ScrollItems.Count/Grid.LineMaxCount;
        if(this.ScrollItems.Count%Grid.LineMaxCount>0)
        {
            Debug.LogError("The Scroll item number must be the multiple of the grid line max count");
            ItemNum++;
        }
        this.MaxIndex = this.Total/Grid.LineMaxCount;
        if(this.Total%this.Grid.LineMaxCount > 0)
        {
            this.MaxIndex++;
        }
        this.MaxIndex -= this.ItemNum;
        for(int i = 0 ; i<this.ScrollItems.Count ; i++)
        {
            GameObject go = this.ScrollItems[i];
            AddDragEvent(go);
        }
        AddDragEvent(this.gameObject);
        this.MaxFixPos = 0;
        this.MinFixPos = 0;
        this.RepositionMax = this.MaxFixPos + this.ItemCost;
        this.RepositionMin = this.MinFixPos - this.ItemCost;
        this.MaxPos = this.MinFixPos + this.ItemCost * (this.ItemNum-1);
        this.MinPos = this.MaxFixPos - this.ItemCost * (this.ItemNum-1);

        this.m_fMove_rate = 1f;
        this.m_iNowIndex = 0;
        this.m_fMove_speed = 0f;
        this.m_iMoveDir = 1;
        this.m_fMoveStartTime = 0;
        this.m_bDrag = false;
        this.m_bFixPos = false;
        this.m_cFixObj = null;
        this.m_fFixSpeed = 0;
    }

    //add drag event
    void AddDragEvent( GameObject go )
    {
        UI_Event ev = UI_Event.Get(go);
        ev.onDown += OnDown;
        ev.onUp += OnUp;
        ev.onDrag += OnDrag;
    }

    void Start()
    {
        // Init();
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

        for(int i = 0 ; i<this.ScrollItems.Count ; i++)
        {
            GameObject obj = this.ScrollItems[i];
            Vector3 mv = Vector3.zero;
            if( this.moveType == Movement.Horizontal )
                mv = new Vector3(eventData.delta.x * this.m_fMove_rate , 0 , 0);
            else
                mv = new Vector3( 0 , eventData.delta.y * this.m_fMove_rate , 0);
            obj.transform.localPosition += mv;
        }
        for(int i = 0 ; i<this.ScrollItems.Count ; i+=this.Grid.LineMaxCount)
        {
            GameObject obj = this.ScrollItems[i];
            if( moveType == Movement.Horizontal )
            {
                if( obj.transform.localPosition.x < this.MinPos && this.m_iNowIndex > 0 )
                {
                    this.m_iNowIndex--;
                    for(int j = i ; j<i+this.Grid.LineMaxCount && j < this.ScrollItems.Count ;j++)
                    {
                        GameObject obj1 = this.ScrollItems[j];
                        obj1.transform.localPosition = new Vector3( this.RepositionMax + (obj1.transform.localPosition.x - this.MinPos) , obj1.transform.localPosition.y , obj1.transform.localPosition.z );
                        if(this.onChange != null)
                        {
                            obj1.SetActive(true);
                            this.onChange(obj1,(this.m_iNowIndex)*this.Grid.LineMaxCount + j - i);
                        }
                    }
                }
                if( obj.transform.localPosition.x > this.MaxPos && this.m_iNowIndex < this.MaxIndex)
                {
                    this.m_iNowIndex++;
                    for(int j = i ; j<i+this.Grid.LineMaxCount && j < this.ScrollItems.Count ;j++)
                    {
                        GameObject obj1 = this.ScrollItems[j];
                        obj1.transform.localPosition = new Vector3( this.RepositionMin + (obj1.transform.localPosition.x - this.MaxPos) , obj1.transform.localPosition.y , obj1.transform.localPosition.z );
                        int index = (this.ItemNum + this.m_iNowIndex-1)*this.Grid.LineMaxCount + j - i;
                        if( index >= this.Total)
                        {
                            obj1.SetActive(false);
                        }
                        else
                        {
                            obj1.SetActive(true);
                            if(this.onChange != null)
                            {
                                this.onChange(obj1,index);
                            }
                        }
                    }
                }
            }
            else
            {
                if( obj.transform.localPosition.y < this.MinPos && this.m_iNowIndex > 0 )
                {
                    this.m_iNowIndex--;
                    for(int j = i ; j<i+this.Grid.LineMaxCount && j < this.ScrollItems.Count ;j++)
                    {
                        GameObject obj1 = this.ScrollItems[j];
                        obj1.transform.localPosition = new Vector3( obj1.transform.localPosition.x , this.RepositionMax + (obj1.transform.localPosition.y - this.MinPos) , obj1.transform.localPosition.z );
                        obj1.SetActive(true);
                        if(this.onChange != null)
                        {
                            this.onChange(obj1,(this.m_iNowIndex)*this.Grid.LineMaxCount + j - i);
                        }
                    }
                    // obj.transform.localPosition = new Vector3( obj.transform.localPosition.x , this.RepositionMax + (obj.transform.localPosition.y - this.MinPos) , obj.transform.localPosition.z );
                    // if(this.onChange != null)
                    // {
                    //     this.onChange(obj , 0);
                    // }
                }
                if( obj.transform.localPosition.y > this.MaxPos && this.m_iNowIndex < this.MaxIndex )
                {
                    this.m_iNowIndex++;
                    for(int j = i ; j<i+this.Grid.LineMaxCount && j < this.ScrollItems.Count ;j++)
                    {
                        GameObject obj1 = this.ScrollItems[j];
                        obj1.transform.localPosition = new Vector3( obj1.transform.localPosition.x , this.RepositionMin + (obj1.transform.localPosition.y - this.MaxPos) , obj1.transform.localPosition.z );
                        int index = (this.ItemNum + this.m_iNowIndex-1)*this.Grid.LineMaxCount + j - i;
                        if( index >= this.Total)
                        {
                            obj1.SetActive(false);
                        }
                        else
                        {
                            obj1.SetActive(true);
                            if(this.onChange != null)
                            {
                                this.onChange(obj1,index);
                            }
                        }
                    }
                    // obj.transform.localPosition = new Vector3( obj.transform.localPosition.x , this.RepositionMin + (obj.transform.localPosition.y - this.MaxPos) , obj.transform.localPosition.z );
                    // if(this.onChange != null)
                    // {
                    //     this.onChange(obj , 1);
                    // }
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

            for(int i = 0 ; i<this.ScrollItems.Count ; i++)
            {
                GameObject obj = this.ScrollItems[i];
                Vector3 mv = Vector3.zero;
                if( this.moveType == Movement.Horizontal )
                    mv = new Vector3(this.m_fMove_speed * this.m_iMoveDir * this.m_fMove_rate , 0 , 0);
                else
                    mv = new Vector3( 0 , this.m_fMove_speed * this.m_iMoveDir * this.m_fMove_rate , 0);
                obj.transform.localPosition += mv;
            }
            for(int i = 0 ; i<this.ScrollItems.Count ; i+=this.Grid.LineMaxCount)
            {
                GameObject obj = this.ScrollItems[i];
                if( moveType == Movement.Horizontal )
                {
                    if( obj.transform.localPosition.x < this.MinPos )
                    {
                        if(this.m_iNowIndex > 0)
                        {
                            this.m_iNowIndex--;
                            for(int j = i ; j<i+this.Grid.LineMaxCount && j < this.ScrollItems.Count ;j++)
                            {
                                GameObject obj1 = this.ScrollItems[j];
                                obj1.transform.localPosition = new Vector3( this.RepositionMax + (obj1.transform.localPosition.x - this.MinPos) , obj1.transform.localPosition.y , obj1.transform.localPosition.z );
                                if(this.onChange != null)
                                {
                                    obj1.SetActive(true);
                                    this.onChange(obj1,(this.m_iNowIndex)*this.Grid.LineMaxCount + j - i);
                                }
                            }
                            // obj.transform.localPosition = new Vector3( this.RepositionMax + (obj.transform.localPosition.x - this.MinPos) , obj.transform.localPosition.y , obj.transform.localPosition.z );
                            // if(this.onChange != null)
                            //     this.onChange(obj , 0);
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
                            this.m_iNowIndex++;
                            for(int j = i ; j<i+this.Grid.LineMaxCount && j < this.ScrollItems.Count ;j++)
                            {
                                GameObject obj1 = this.ScrollItems[j];
                                obj1.transform.localPosition = new Vector3( this.RepositionMin + (obj1.transform.localPosition.x - this.MaxPos) , obj1.transform.localPosition.y , obj1.transform.localPosition.z );
                                int index = (this.ItemNum + this.m_iNowIndex-1)*this.Grid.LineMaxCount + j - i;
                                if( index >= this.Total)
                                {
                                    obj1.SetActive(false);
                                }
                                else
                                {
                                    obj1.SetActive(true);
                                    if(this.onChange != null)
                                    {
                                        this.onChange(obj1,index);
                                    }
                                }
                            }
                            // obj.transform.localPosition = new Vector3( this.RepositionMin + (obj.transform.localPosition.x - this.MaxPos) , obj.transform.localPosition.y , obj.transform.localPosition.z );
                            // if(this.onChange != null)
                            //     this.onChange(obj , 1);
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
                            this.m_iNowIndex--;
                            for(int j = i ; j<i+this.Grid.LineMaxCount && j < this.ScrollItems.Count ;j++)
                            {
                                GameObject obj1 = this.ScrollItems[j];
                                obj1.transform.localPosition = new Vector3( obj1.transform.localPosition.x , this.RepositionMax + (obj1.transform.localPosition.y - this.MinPos) , obj1.transform.localPosition.z );
                                if(this.onChange != null)
                                {
                                    obj1.SetActive(true);
                                    this.onChange(obj1,(this.m_iNowIndex)*this.Grid.LineMaxCount + j - i);
                                }
                            }
                            // obj.transform.localPosition = new Vector3( obj.transform.localPosition.x , this.RepositionMax + (obj.transform.localPosition.y - this.MinPos) , obj.transform.localPosition.z );
                            // if(this.onChange != null)
                            //     this.onChange(obj , 0);
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
                            this.m_iNowIndex++;
                            for(int j = i ; j<i+this.Grid.LineMaxCount && j < this.ScrollItems.Count ;j++)
                            {
                                GameObject obj1 = this.ScrollItems[j];
                                obj1.transform.localPosition = new Vector3( obj1.transform.localPosition.x , this.RepositionMin + (obj1.transform.localPosition.y - this.MaxPos) , obj1.transform.localPosition.z );
                                int index = (this.ItemNum + this.m_iNowIndex-1)*this.Grid.LineMaxCount + j - i;
                                if( index >= this.Total)
                                {
                                    obj1.SetActive(false);
                                }
                                else
                                {
                                    obj1.SetActive(true);
                                    if(this.onChange != null)
                                    {
                                        this.onChange(obj1,index);
                                    }
                                }
                            }
                            // obj.transform.localPosition = new Vector3( obj.transform.localPosition.x , this.RepositionMin + (obj.transform.localPosition.y - this.MaxPos) , obj.transform.localPosition.z );
                            // if(this.onChange != null)
                            //     this.onChange(obj , 1);
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
            for(int i = 0 ; i<this.ScrollItems.Count ; i+=this.Grid.LineMaxCount)
            {
                GameObject obj = this.ScrollItems[i];
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
            for(int i = 0 ; i<this.ScrollItems.Count ; i++)
            {
                GameObject obj = this.ScrollItems[i];
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


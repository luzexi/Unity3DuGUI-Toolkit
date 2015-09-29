
using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;


//  UI_FontNum.cs
//  Author: Lu Zexi
//  2015-03-31


//font num
[AddComponentMenu("uGUI/UI_FontNum")]
public class UI_FontNum : MonoBehaviour
{
    public Sprite[] Num = new Sprite[10];
    public TextAnchor alignment = TextAnchor.MiddleCenter; //alignment
    public int number = 999;    //number
    public int interval = 1;    //interval

    private List<GameObject> m_lstObj = new List<GameObject>();

    void Awake()
    {
        Setup(this.number);
    }

    public void Setup( int num )
    {
        List<int> lst = new List<int>();

        do
        {
            int tmp_num = num%10;
            num = num/10;
            lst.Add(tmp_num);
        }while(num>0);

        lst.Reverse();

        float x = 0;
        for(int i = 0 ; i<this.m_lstObj.Count ; i++)
        {
            this.m_lstObj[i].SetActive(false);
        }

        //
        for(int i=0 ; i<lst.Count ; i++)
        {
            GameObject obj = null;
            Image tmpImg = null;
            if(i<m_lstObj.Count)
            {
                obj = m_lstObj[i];
                obj.SetActive(true);
                tmpImg = obj.GetComponent<Image>();
            }
            else
            {
                obj = new GameObject("num"+lst[i]);
                obj.layer = 5;
                obj.transform.SetParent(this.transform);
                obj.transform.localScale = Vector3.one;
                obj.transform.localPosition = Vector3.zero;
                tmpImg = obj.AddComponent<Image>();

                this.m_lstObj.Add(obj);
            }

            tmpImg.sprite = Num[lst[i]];
            tmpImg.SetNativeSize();

            if(i>0) x+=interval;
            tmpImg.transform.localPosition += new Vector3(x,0,0);
            if(i<lst.Count-1)
            x = x + Num[lst[i]].textureRect.width;
        }

        int offset_x = 0;
        int offset_y = 0;
        switch(this.alignment)
        {
            case TextAnchor.UpperLeft:
                break;
            case TextAnchor.UpperCenter:
                break;
            case TextAnchor.UpperRight:
                break;
            case TextAnchor.MiddleLeft:
                break;
            case TextAnchor.MiddleCenter:
                offset_x = (int)(x/2);
                break;
            case TextAnchor.MiddleRight:
                break;
            case TextAnchor.LowerLeft:
                break;
            case TextAnchor.LowerCenter:
                break;
            case TextAnchor.LowerRight:
                break;
        }
        for(int i=0 ; i<lst.Count ; i++)
        {
            this.m_lstObj[i].transform.localPosition -= new Vector3(offset_x,offset_y);
        }
    }
}


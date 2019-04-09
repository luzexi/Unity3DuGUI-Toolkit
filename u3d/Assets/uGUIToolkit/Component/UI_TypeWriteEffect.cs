using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

[AddComponentMenu("UI/UI TypeWriteEffect")]
public class UI_TypeWriteEffect : MonoBehaviour
{
    public float charsPerSecond = 0.2f; //打字时间间隔...
    public bool autoWrite;
    private string words;               //保存需要显示的文字...
    private bool isActive = false;
    private float timer;                //计时器...
    private Text myText;
    private int currentPos = 0;         //当前打字位置...
    private System.Action mFinishCall = null;

    // Use this for initialization...
    void Start()
    {
    }

    // Update is called once per frame...
    void Update()
    {
        OnStartWriter();
    }

    public void SetText(string _strText)
    {
        timer = 0;
        if (myText == null)
            myText = gameObject.GetComponent<Text>();
        isActive = autoWrite;

        if (_strText == null)
            _strText = "";
        words = _strText;
    }

    public void StartEffect()
    {
        isActive = true;
    }

    public void StartEffect(System.Action _action = null)
    {
        mFinishCall = _action;
        timer = 0;
        isActive = true;
    }

    public void SetEndLinstener(System.Action _action = null)
    {
        mFinishCall = _action;
    }

    //执行打字任务...
    void OnStartWriter()
    {
        if (isActive)
        {
            timer += Time.deltaTime;
            if (timer >= charsPerSecond)
            {
                //判断计时器时间是否到达...
                timer = 0;
                currentPos++;
                //刷新文本显示内容...
                myText.text = words.Substring(0, currentPos);
                if (currentPos >= words.Length)
                {
                    OnFinish();
                }
            }
        }
    }

    //结束打字，初始化数据...
    void OnFinish()
    {
        isActive = false;
        timer = 0;
        currentPos = 0;
        myText.text = words;
        //结束回调执行...
        if (mFinishCall!=null)
            mFinishCall();
    }
}
using UnityEngine;
using UnityEngine.UI;
using System.Collections;



[AddComponentMenu("UI/UI Number Count")]
[RequireComponent (typeof(UnityEngine.UI.Text))]
public class UI_NumberCounter : MonoBehaviour {

    protected UnityEngine.UI.Text m_DisplayText;

    protected float COUNT_ANIM_DURATION = 1.0f;

    protected float m_CurrCount=0;
    protected float m_InitCount=0;
    protected float m_FinalCount=0;

    protected int m_CountSpeed;
	
	// Update is called once per frame
	void Update ()
    {
        DoUpdate();
    }

    protected virtual void DoUpdate()
    {
        if (m_CurrCount != m_FinalCount)
        {
            // Determine count speed.
            float speed = (m_FinalCount - m_InitCount) / COUNT_ANIM_DURATION;

            // Update value, clamping on final count.
            if (m_FinalCount > m_CurrCount)
                m_CurrCount = Mathf.Clamp(m_CurrCount + Time.unscaledDeltaTime * speed, 0.0f, m_FinalCount);
            else
                m_CurrCount = Mathf.Clamp(m_CurrCount + Time.unscaledDeltaTime * speed, m_FinalCount, float.MaxValue);

            // Update display text.
            UpdateDisplayText();
        }
	}

    protected virtual void UpdateDisplayText()
    {
        if(this.m_DisplayText == null)
        {
            this.m_DisplayText = GetComponent<Text>();
        }
        if (m_DisplayText != null)
            m_DisplayText.text = "" + (int)m_CurrCount;
    }

    public virtual void OnValueChanged(int oldValue, int neoValue)
    {
        m_FinalCount = neoValue;
        m_InitCount = oldValue;
        m_CurrCount = oldValue;
        UpdateDisplayText();
    }
}

using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

/// <summary>
/// sound component
/// </summary>
[AddComponentMenu("UI/Button PlaySound")]
public class UI_ButtonPlaySound : UI_Event
{
    [SerializeField]
    public AudioClip audioClip;
    private  AudioClip audioClipNew = null;

    public override void OnPointerClick(PointerEventData eventData)
    {
        // base.OnPointerClick(eventData);
        // OnClick();
        if(mAnyMove) return;
        if(sDisableEvent2 && !mIgnoreDisable) return;
        if(sDisableEvent && !mIgnoreDisable) return;
        sIsEvent = true;
        if(onClick != null)
        {
            // sIsEvent = true;
            onClick(eventData , this);
            OnClick();
        }
    }

    public void RefreshSetAudioClip(AudioClip clip)
    {
        audioClipNew = clip;
    }

    void OnClick ()
    {
        if(audioClip != null)
        {
            if (audioClipNew == null)
            {
                PlaySound(audioClip);
            }
            else
            {
                PlaySound(audioClipNew);
                audioClipNew = null;
            }
        }
    }

    static public AudioSource PlaySound (AudioClip clip)
    {
        // if(clip == null || GlobalObject.soundPlayer == null)
        // {
        //     return null;
        // }
        // GlobalObject.soundPlayer.Play(clip);
        return null;
    }
}

// #define GAME_MEDIA


using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

#if GAME_MEDIA
using Game.Media;
#endif

/// <summary>
/// Plays the specified sound.
/// </summary>
[AddComponentMenu("uGUI/UI_Button PlaySound")]
public class UI_ButtonPlaySound : MonoBehaviour
{
    public AudioClip audioClip;

    [Range(0f, 1f)] public float volume = 1f;
    [Range(0f, 2f)] public float pitch = 1f;

    bool mStarted = false;

    void Awake()
    {
        if(!mStarted)
        {
            mStarted = true;
            var ev = UI_Event.Get(this);
            ev.onClick += OnClick;
        }
    }

    void OnClick (PointerEventData eventData , GameObject go , string[] args)
    {
        if(audioClip != null)
        {
#if GAME_MEDIA
            MediaMgr.sInstance.PlaySE(audioClip);
#else
            PlaySound(audioClip , volume , pitch);
#endif
        }
    }

    static AudioListener mListener;
    static public AudioSource PlaySound (AudioClip clip, float volume, float pitch)
    {
        // volume *= soundVolume;

        if (clip != null && volume > 0.01f)
        {
            if (mListener == null || !(mListener.enabled && mListener.gameObject.activeSelf))
            {
                mListener = GameObject.FindObjectOfType(typeof(AudioListener)) as AudioListener;

                if (mListener == null)
                {
                    Camera cam = Camera.main;
                    if (cam == null) cam = GameObject.FindObjectOfType(typeof(Camera)) as Camera;
                    if (cam != null) mListener = cam.gameObject.AddComponent<AudioListener>();
                }
            }

            if (mListener != null && mListener.enabled && mListener.gameObject.activeSelf)
            {
                AudioSource source = mListener.audio;
                if (source == null) source = mListener.gameObject.AddComponent<AudioSource>();
                source.pitch = pitch;
                source.PlayOneShot(clip, volume);
                return source;
            }
        }
        return null;
    }
}

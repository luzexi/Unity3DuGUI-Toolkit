using UnityEngine;
using System.Collections;
using System.Collections.Generic;



//ui animator
[AddComponentMenu("uGUI/UI Animator")]
[RequireComponent (typeof(Animator))]
public class UI_Animator : MonoBehaviour
{
	public bool AutoPlay = false;
	public string AutoPlayName = "open";
	public List<UI_Animator> PlayList = new List<UI_Animator>();
	Animator mAnimator;
	string mStateName="";
	bool mIsCallback = false;
	System.Action mCallback;
	bool mbStuck = false;

	Vector3 mScale;
	Vector3 mPosition;
	Vector3 mRotation;

	bool mStartAni = false;
	float mCallbackRate = 1;
	float mAnimationStartTime = 0;
	float mAnimationLength = 0;

	void OnEnable()
	{
		if(this.AutoPlay)
		{
			Play(this.AutoPlayName);
		}
	}

	public void Play(string ani_name , System.Action callback = null , float callback_rate = 1, bool stuck = false)
	{
		if(this.mAnimator == null)
		{
			this.mAnimator = GetComponent<Animator>();
		}
		// Debug.LogError("ani name " + ani_name);
		// this.mAnimator.SetInteger("ani",index);
		this.mStateName = ani_name;
		this.mCallbackRate = callback_rate;
		this.mAnimator.Play(ani_name,0,0);
		for(int i = 0 ; i<this.PlayList.Count ; i++)
		{
			this.PlayList[i].Play(ani_name);
		}
		this.mAnimationStartTime = Time.time;
		this.mAnimationLength = 0;
		AnimatorClipInfo[] infos = this.mAnimator.GetCurrentAnimatorClipInfo(0);
		if(infos != null && infos.Length > 0)
		{
			// Debug.LogError("info name " + infos[0].clip.name);
			this.mAnimationLength = infos[0].clip.length;
		}
		this.mCallback = callback;
		if(this.mCallback != null)
		{
			this.mIsCallback = true;
		}
		this.mbStuck = stuck;
		if(this.mbStuck)
		{
			// MessageBox.instance.OpenWaitingBoard();
		}
		this.mStartAni = true;
	}

	void Update()
	{
		if(this.mStartAni && this.mAnimator != null)
		{
			AnimatorStateInfo info = this.mAnimator.GetCurrentAnimatorStateInfo(0);
			if(info.IsName(mStateName))
			{
				// Debug.LogError("normalizedTime " + info.normalizedTime);
				if(info.normalizedTime >= 1)
				{
					// Debug.LogError("normalizedTime " + info.normalizedTime + " name " + this.mStateName + " is "+ info.IsName(mStateName));
					this.mStartAni = false;
					OnAnimationExit();
					return;
				}
				if(info.normalizedTime >= this.mCallbackRate)
				{
					if(this.mCallback != null && this.mIsCallback)
					{
						this.mIsCallback = false;
						this.mCallback();
					}
				}
			}
			// AnimatorClipInfo[] infos = this.mAnimator.GetCurrentAnimatorClipInfo(0);
			// if(infos != null && infos.Length > 0)
			// {
			// 	Debug.LogError("rate " + infos[0].clip.frameRate);
			// }
		}
	}

	public void OnAnimationEnter()
	{
		// base.OnAnimationEnter();
	}

	public void OnAnimationExit()
	{
		// Debug.LogError("animator exit");
		if(this.mAnimator == null)
			return;
		// base.OnAnimationExit();
		this.mAnimator.Play("null",0,0);

		if(this.mbStuck)
		{
			this.mbStuck = false;
			// MessageBox.instance.CloseWaitingBoard();
		}

		if(this.mCallback != null && this.mIsCallback)
		{
			this.mIsCallback = false;
			this.mCallback();
		}
	}

	// void OnAnimatorMove()
	// {
	// 	Debug.Log("ok animator move");
	// }
}

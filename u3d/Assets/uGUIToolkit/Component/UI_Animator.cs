using UnityEngine;
using System.Collections;
using System.Collections.Generic;



//ui animator
[AddComponentMenu("UI/UI Animator")]
[RequireComponent (typeof(Animator))]
public class UI_Animator : MonoBehaviour
{
	public System.Action mOnEnter = null;
	public System.Action mOnExit = null;

	public bool AutoPlay = false;
	public string AutoPlayName = "open";
	public bool AutoPlayLoop = false;
	public int AutoPlayLoopNumber = 1;
	public float AutoPlayLoopInterval = 2f;
	public List<UI_Animator> PlayList = new List<UI_Animator>();
	Animator mAnimator;
	string mStateName="";
	bool mIsCallback = false;
	System.Action mCallback;
	bool mbStuck = false;

	Vector3 mScale;
	Vector3 mPosition;
	Vector3 mRotation;

	int mAutoPlayTime = 0;
	float mAutoPlayIntervalStart = -1;
	bool mStartAni = false;
	float mCallbackRate = 1;
	float mAnimationStartTime = 0;
	float mAnimationLength = 0;

	void OnEnable()
	{
		if(this.AutoPlay)
		{
			mAutoPlayTime = 0;
			Play(this.AutoPlayName,LoopCallback);
		}
	}

	void LoopCallback()
	{
		if(!AutoPlayLoop) return;
		mAutoPlayTime++;
		if(mAutoPlayTime >= AutoPlayLoopNumber)
		{
			Play("null");
			mAutoPlayIntervalStart = Time.time;
		}
		else
		{
			Play(this.AutoPlayName,LoopCallback);
		}
	}

	public void Play(string ani_name ,
		System.Action callback = null ,
		float callback_rate = 1,
		bool stuck = false)
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
		}
		AutoUpdate();
	}

	void AutoUpdate()
	{
		if(this.AutoPlay && mAutoPlayIntervalStart> 0)
		{
			float disTime = Time.time - mAutoPlayIntervalStart;
			if(disTime >= AutoPlayLoopInterval)
			{
				mAutoPlayIntervalStart = -1;
				mAutoPlayTime = 0;
				Play(this.AutoPlayName,LoopCallback);
			}
		}
	}

	public void OnAnimationEnter()
	{
		if(this.mOnEnter != null)
		{
			this.mOnEnter();
		}
	}

	public void OnAnimationExit()
	{
		// Debug.LogError("animator exit");
		if(this.mAnimator == null)
			return;
		
		if(this.mOnExit != null)
		{
			this.mOnExit();
		}
		// this.mAnimator.Play("null",0,0);

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

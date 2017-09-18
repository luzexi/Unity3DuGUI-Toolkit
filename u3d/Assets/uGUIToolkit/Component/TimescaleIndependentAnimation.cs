using UnityEngine;
using System;
using System.Collections;

// [AddComponentMenu("UI/Animation/Timescale Independent Animation")]
[RequireComponent(typeof(Animation))]
/**
	TimescaleIndependentAnimations are just that, an Animation that ignores Time.timeScale and will play regardless of what Time.timeScale is.
	This is achieved by a custom deltaTime variable, found in QuadUI.deltaTime.
	
	Note: This component invokes RequireComponent, and requires an Animation component attached to its GameObject. It is also important to note that AnimationEvents are not currently available through this method of playback. There are a few hooks provided for you to attach code at certain points of the animation in this class.
*/
public class TimescaleIndependentAnimation : MonoBehaviour 
{	
	protected Action mOnAnimationBegin;
	protected Action mOnAnimationStop;
	protected Action mOnAnimationComplete;

	internal float _accumulatedTime = 0F;
	public AnimationState _currentState;
	protected bool _animating = false;
	protected bool _animationStart = false;
	protected bool _animationEnd = false;
	private const float MAX_STEP_TIME = 0.1f;
	
	/**
		Takes the <string> name of the AnimationState.
		Internally updates the normalized time with a Time.timeScale independent delta (QuadUI.deltaTime).
		The provided AnimationState will have a weight of 1, be on layer 1, use AnimationBlendMode.Blend. Calling this function sets the normalizedTime to 0 before playing if the animation is not Paused, otherwise it picks up where it left off.
		
	Note: The provided AnimationState will retain the wrapMode defined in its creation.
		The hook OnAnimationBegin() is called from this function.
	*/
	public void Play(string state,Action _onAnimationComplete = null,
		Action _onAnimationStop = null, Action _onAnimationBegin = null)
	{
		if (_currentState != null)
		{
			if(_currentState == GetComponent<Animation>()[state] && _currentState.normalizedTime > 0 && _accumulatedTime > 0)
			{
				_animating = true;
				return;
			}
			else if(_currentState != GetComponent<Animation>()[state] && _accumulatedTime > 0)
			{
				Stop();
			}
		}
		
		_currentState = GetComponent<Animation>()[state];
		if(!_currentState) return;

		mOnAnimationComplete = _onAnimationComplete;
		mOnAnimationBegin = _onAnimationBegin;
		mOnAnimationStop = _onAnimationStop;
		
		_currentState.weight = 1F;
		_currentState.wrapMode = _currentState.clip.wrapMode;//_currentState.clip.wrapMode;
		_currentState.layer = 1;
		_currentState.blendMode = AnimationBlendMode.Blend;
		_currentState.normalizedTime = 0;
		_currentState.speed = 0; //important for stop animation self
		_currentState.enabled = true;
		_accumulatedTime = 0F;
		OnAnimationBegin();
		
		_animating = true;
		_animationStart = true;
		_animationEnd = false;
	}
	
	/* Play the default animation */
	public void Play()
	{
		Play(GetComponent<Animation>().clip.name);
	}
	
	public void Replay()
	{
		Stop();
		Play(GetComponent<Animation>().clip.name);
	}
	
	/**
		Stops the current AnimationState from updating and returns the animation to frame 1.
		Note: When calling Stop() all data set when calling Play() is reset, and normalizedTime will be set to 0 resulting in the animation returning to frame 1.
		
		The hook OnAnimationStop() is called from this function.
	*/
	public void Stop()
	{
		if(!_currentState) return;
		
		_currentState.enabled = false;
		_currentState.weight = 0F;
		_currentState.layer = 0;
		_currentState.normalizedTime = 0;
		_accumulatedTime = 0F;
		OnAnimationStop();
		
		_animating = false;
	}
	
	/**
		Stops the current AnimationState from updating, but retains its current frame.
		Note: There is no hook called from Pause.
	*/
	public void Pause()
	{
		_animating = false;
	}
	
	protected virtual void Update()
	{
		if(_animating) UpdateAnimation();
	}
	
	internal virtual void UpdateAnimation()
	{		
		if (_animationEnd)
		{
			OnAnimationComplete();
			// maybe we want to Play() another animation after current animation end, so _animationEnd will be false
			if (_animationEnd)
			{
				_currentState.enabled = false;
				_animating = false;
				_accumulatedTime = 0F;
			}
			return;
		}
		// if disable this gameobject, then enable it, the _currentState.enabled will be false after this
		if (!_currentState.enabled) _currentState.enabled = true;
		
		//accumulate time
		//force first frame is time 0
		if (_animationStart)
		{
			_animationStart = false;
		}
		else
		{
			// _accumulatedTime += PandaUI.deltaTime > MAX_STEP_TIME? MAX_STEP_TIME: PandaUI.deltaTime;
			_accumulatedTime += Time.deltaTime > MAX_STEP_TIME? MAX_STEP_TIME: Time.deltaTime;
		}
		//print("acc time: "+_accumulatedTime+" delta time: "+PandaUI.deltaTime);
		
		
		//check for completion
		if(_accumulatedTime >= _currentState.length) 
		{
			if(_currentState.wrapMode == WrapMode.Loop ||
			   _currentState.wrapMode == WrapMode.PingPong)
			{
				_currentState.normalizedTime = _accumulatedTime/_currentState.length; 
				OnLoop();
			}
			else
			{
				_currentState.normalizedTime = 1;
				_animationEnd = true;
			}
		}
		else
		{
			//place where the animation should be.
			_currentState.normalizedTime = _accumulatedTime/_currentState.length; 
		}
	}
	
	/**
		Takes the <string> name of the AnimationState and the <float> 0.0F-1.0F desired normalizedTime of the animation you wish to move to.
		Allows you to manually set the normalizedTime of the provided AnimationState.
	*/
	public void ManuallySetNormalizedTime(string animState, float _normalizedTime)
	{
		if(_currentState)_currentState.enabled = false;
		_currentState = GetComponent<Animation>()[animState];
		_currentState.enabled = true;
		_currentState.weight = 1F;
		_currentState.wrapMode = WrapMode.Once;
		_currentState.layer = 1;
		_currentState.blendMode = AnimationBlendMode.Blend;
		_currentState.normalizedTime = _normalizedTime;
	}
	
	/**
		This function is virtual and can be overridden when extending this class.
		
		This function is intended to be a hook and is called from Play(string state). You should use this function to execute any code you want to happen when the AnimationState begins playing.
	*/
	protected virtual void OnAnimationBegin()
	{	
		if(mOnAnimationBegin != null)
		{
			mOnAnimationBegin();
			mOnAnimationBegin = null;
		}
	}
	
	/**
		This function is virtual and can be overridden when extending this class.
		
		This function is intended to be a hook and is called from Stop(). You should use this function to execute any code you want to happen when the AnimationState is told to stop playing.
	*/
	protected virtual void OnAnimationStop()
	{	
		if(mOnAnimationStop != null)
		{
			mOnAnimationStop();
			mOnAnimationStop = null;
		}
	}
	
	/**
		This function is virtual and can be overridden when extending this class.
		
		This function is intended to be a hook and is called when the AnimationState successfully reaches a normalizedTime of 1. You should use this function to execute any code you want to happen when the AnimationState finishes playing.
		
		Note: This hook will only be called if the wrapMode of the current AnimationState is set to Default or Once.
	*/
	protected virtual void OnAnimationComplete()
	{
		if(mOnAnimationComplete != null)
		{
			mOnAnimationComplete();
			mOnAnimationComplete = null;
		}
	}
	
	/**
		This function is virtual and can be overridden when extending this class.
		
		This function is intended to be a hook and is called when the AnimationState successfully reaches a normalizedTime of 1, before it loops back to 0 and continues playing. You should use this function to execute any code you want to happen when the AnimationState loops.
		
		Note: This hook will only be called if the wrapMode of the current AnimationState is set to Loop.
	*/
	protected virtual void OnLoop()
	{
	}
	
	/**
		Read only. Returns true if the animation is playing, false if it is not.
	*/
	public bool isPlaying
	{
		get
		{
			return _animating;
		}
	}
	
	/**
		Read only. Returns the current AnimationState of the animation.
	*/
	public AnimationState currentState
	{
		get
		{
			return _currentState;
		}
	}
	
	/**
		Read only. Returns the current name of the animation.
	*/
	public string currentClip
	{
		get
		{
			if (_currentState != null)
				return _currentState.name;
			else
				return null;
		}
	}
	
	internal void ManualTrack(bool b)
	{
		_animating = b;
	}
}

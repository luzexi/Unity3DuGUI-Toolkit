using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;

/* 
 * This script is used for UI animations, like transform animation, color, alpha, or frame based animation
 * */

[AddComponentMenu("UI/UI Animation")]
public sealed class UI_Animation : TimescaleIndependentAnimation 
{
	public Action mOnAnimationComplete;
	public bool playAutomatically = false;
	public bool restoreAfterAnimationEnd = true;
	
	/* Animation Properties Except Transform.
	 * Transform animation can be edit in Animation Editor self
	 * Other animations' proterties setting here*/
	public bool animatedColor = false;
	public Color original_colorTint = Color.white;
	public Color _colorTint = Color.white;
	// public bool animatedAdditiveColor = false;
	// public Color _colorAdditive = Color.clear;
	// public bool animatedQuads = false;
	// public float _quadIndex = 0;
	// public Quad2D[] quadFrames = new Quad2D[0];
	// public float _quadAnimTimeInterval = 0.1f;
	
	private MaskableGraphic _element;
	// private float originalAlpha;
	// private int quadFramesCount = 0;
	// private float quadAnimationTime = 0;
	private bool mStarted = false;
	
	void Awake()
	{
		_element = GetComponent<MaskableGraphic>();
		if (_element != null)
		{
			enabled = true;
		}
		else
		{
			enabled = false;
			return;
		}
		
		// quadFramesCount = quadFrames.Length;
		// originalAlpha = _colorTint.a;
		original_colorTint = _element.color;
	}
	
	void Start()
	{
		if (playAutomatically && !_animating) 
		{
			Play();

			if (animatedColor && _element != null)
			{
				// _element.SetAlpha(originalAlpha);
				// _element.color.a = originalAlpha;
			}
		}
		mStarted = true;
	}
	
	void OnEnable()
	{
		if (mStarted && playAutomatically && !_animating)
		{
			Play();
			
			if (animatedColor && _element != null)
			{
				// _element.color.a = originalAlpha;
			}
		}
	}
	
	void OnDisable()
	{
		if (playAutomatically && _animating)
		{
			Stop();
		}
	}
	
	protected override void Update()
	{
		if (_animating)
		{
			// Update the animation
			base.Update();
			
			if (_element == null)
				return;
			
			// still animating
			if (_animating)
			{
				// use the animated color
				if (animatedColor)
				{
					// _element.SetTint(_colorTint);
					_element.color = _colorTint;
				}
				
				// // use the animated Additive color
				// if (animatedAdditiveColor)
				// {
				// 	// _element.SetAdditive(_colorAdditive);
				// 	_element.color = _colorAdditive;
				// }
				
				// // use the animated Quads array
				// if (animatedQuads)
				// {
				// 	quadAnimationTime += PandaUI.deltaTime;
					
				// 	if (quadAnimationTime > _quadAnimTimeInterval)
				// 	{
				// 		quadAnimationTime -= _quadAnimTimeInterval;
						
				// 		int int_quadIndex = Mathf.RoundToInt(_quadIndex);
				// 		if (int_quadIndex < 0 || int_quadIndex >= quadFramesCount)
				// 			return;
						
				// 		Quad2D curQuad = quadFrames[int_quadIndex];
				// 		if (curQuad)
				// 		{
				// 			((Label2D)_element).ChangeQuad(curQuad);
				// 		}
				// 	}
				// }
			}
		}
	}
	
	/**
		This function is overridden and calls the method Element2D.AnimationCompleted().	
	*/
	sealed override protected void OnAnimationComplete()
	{
		if (_element)
		{
			// if true, restore the material to the shared one
			// if (restoreAfterAnimationEnd && (animatedColor || animatedAdditiveColor))
			if (restoreAfterAnimationEnd && (animatedColor))
			{
				// _element.Restore();
				_element.color = original_colorTint;
			}
			if(mOnAnimationComplete != null)
			{
				mOnAnimationComplete();
			}
		}
	}
	
	sealed override protected void OnAnimationStop()
	{
		if (_element)
		{
			// if (restoreAfterAnimationEnd && (animatedColor || animatedAdditiveColor))
			if (restoreAfterAnimationEnd && (animatedColor))
			{
				// _element.Restore();
				_element.color = original_colorTint;
			}
		}
	}
}

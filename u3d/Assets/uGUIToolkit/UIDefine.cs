using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class UIDefine
{
	public const int SCREEN_WIDTH = 1920;
	public const int SCREEN_HEIGHT = 1080;

	private static int mScreenWidth = 0;
	public static int UI_SCREEN_WIDTH
	{
		get
		{
			return mScreenWidth;
		}
		set
		{
			mScreenWidth = value;
		}
	}

	private static int mScreenHeight = 0;
	public static int UI_SCREEN_HEIGHT
	{
		get
		{
			return mScreenHeight;
		}
		set
		{
			mScreenHeight = value;
		}
	}
}
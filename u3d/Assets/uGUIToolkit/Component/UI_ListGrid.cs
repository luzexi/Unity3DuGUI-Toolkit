using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;
using System.Collections;
using System.Collections.Generic;


[AddComponentMenu("uGUI/UI_ListGrid")]
public class UI_ListGrid : MonoBehaviour
{
	public enum DIR
	{
		UP_RIGHT,
		UP_LEFT,
		DOWN_RIGHT,
		DOWN_LEFT,
		LEFT_UP,
		LEFT_DOWN,
		RIGHT_UP,
		RIGHT_DOWN
	}

	[SerializeField]
	public int Interval;
	[SerializeField]
	public int LineMaxCount;
	[SerializeField]
	public DIR Dir = DIR.DOWN_RIGHT;

	void Awake()
	{
		Refresh();
	}

	public void Refresh()
	{
		int childCount = this.transform.childCount;
		int dirx = 1;
		int diry = 1;

		switch(this.Dir)
		{
			case DIR.UP_RIGHT:
				dirx = 1;
				diry = 1;
				break;
			case DIR.UP_LEFT:
				dirx = -1;
				diry = 1;
				break;
			case DIR.DOWN_RIGHT:
				dirx = 1;
				diry = -1;
				break;
			case DIR.DOWN_LEFT:
				dirx = -1;
				diry = -1;
				break;
			case DIR.LEFT_UP:
				dirx = -1;
				diry = 1;
				break;
			case DIR.LEFT_DOWN:
				dirx = -1;
				diry = -1;
				break;
			case DIR.RIGHT_UP:
				dirx = 1;
				diry = 1;
				break;
			case DIR.RIGHT_DOWN:
				dirx = 1;
				diry = -1;
				break;
		}

		if(this.Dir <= DIR.DOWN_LEFT)
		{
			for(int i = 0 ; i<childCount ; i++)
			{
				Vector3 pos = new Vector3(i%LineMaxCount*Interval*dirx,i/LineMaxCount*Interval*diry,0);
				Transform child = this.transform.GetChild(i);
				child.localPosition = pos;
			}
		}
		else
		{
			for(int i = 0 ; i<childCount ; i++)
			{
				Vector3 pos = new Vector3(i/LineMaxCount*Interval*dirx,i%LineMaxCount*Interval*diry,0);
				Transform child = this.transform.GetChild(i);
				child.localPosition = pos;
			}
		}
	}
}




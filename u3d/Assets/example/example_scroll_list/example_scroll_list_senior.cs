using UnityEngine;
using System.Collections;

public class example_scroll_list_senior : MonoBehaviour
{
	public UI_ScrollListSenior scroll;

	// Use this for initialization
	void Start ()
	{
		if(scroll != null)
		{
			scroll.Total = 54;
			scroll.SetChangeHandle(InitItem);
			scroll.Init();
		}
	}

	void InitItem(GameObject go , int index)
	{
		Debug.Log("item index" + index);
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}

using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;





public class example_ui_animation_screenHandle : MonoBehaviour
{
	public GameObject button;
	private bool isopen = true;
	public UI_Animator exampleImage;

	void Start()
	{
		UI_Event e = UI_Event.Get(button);
		e.onClick = button_click;
	}


	void button_click( PointerEventData eventData , UI_Event ev)
	{
		if(!isopen)
		{
			exampleImage.Play("open",on_animator_finish);
			isopen = true;
		}
		else
		{
			exampleImage.Play("close",on_animator_finish);
			isopen = false;
		}
	}

	void on_animator_finish()
	{
		Debug.LogError("on_animator_finish in example");
	}
}
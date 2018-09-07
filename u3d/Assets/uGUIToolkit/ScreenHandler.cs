// using UnityEngine;
// using System.Collections;

// /* 
//  * This is an abstract class. Implement base logic of screen, like open, close etc.
//  * Extend this class if more logic needed.
//  * */
// public class ScreenHandler: MonoBehaviour
// {	
// 	public enum ScreenState
// 	{
// 		Open = 0,
// 		Closed,
// 		Opening,
// 		Closing,
// 		Collapsed,
// 		Preopen,
// 	}

// 	[HideInInspector] public int id;
	
// 	private bool inited = false;
	
// 	[HideInInspector] public ScreenState state;
	
// 	public delegate void CloseHandler(ScreenHandler _handler);
// 	[HideInInspector] public CloseHandler closeHandler = null;
	
// 	protected virtual void Awake()
// 	{
// 		state = ScreenState.Closed;
// 	}
	
// 	protected virtual void Start()
// 	{
// 		//
// 	}
	
// 	public virtual void Init() 
// 	{
// 		inited = true;
// 	}
	
// 	// default is no animated open screen
// 	public virtual void OpenScreen()
// 	{
// 		if (!inited)
// 		{
// 			Init();
// 		}
// 		state = ScreenState.Open;
// 	}

//     public bool IsOpen { get { return state == ScreenState.Open; } }
	
// 	// no animated close screen
// 	public virtual void CloseScreen()
// 	{
// 		if(closeHandler != null)
// 		{
// 			closeHandler(this);
// 		}
// 		state = ScreenState.Closed;
// 	}
	
// 	// public virtual void CollapseScreen()
// 	// {
// 	// 	if (state == ScreenState.Open)
// 	// 	{
// 	// 		state = ScreenState.Collapsed;
// 	// 	}
// 	// }
	
// 	// public virtual void ExpandScreen()
// 	// {
// 	// 	if (state == ScreenState.Collapsed)
// 	// 	{
// 	// 		state = ScreenState.Open;
// 	// 	}
// 	// }

// 	public virtual void SetActive(bool _active)
// 	{
// 		gameObject.SetActive(_active);
// 	}
	
	
// 	public void RegisterCloseCallback(CloseHandler _handler)
// 	{
// 		closeHandler = _handler;
// 	}

// #if UNITY_ANDROID
//     protected void LateUpdate()
//     {
//         if (Input.GetKeyDown(KeyCode.Escape))
//         {
//             CloseScreen();
//         }
//     }
// #endif
// }
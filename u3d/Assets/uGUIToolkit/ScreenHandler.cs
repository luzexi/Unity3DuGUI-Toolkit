
using UnityEngine;
using UnityEngine.UI;


public class ScreenHandler : MonoBehaviour
{
	protected bool mInitialized = false;

	void Awake()
	{
		if(mInitialized)
			Init();
	}

	public virtual void Init()
	{
		this.mInitialized = true;
	}
}

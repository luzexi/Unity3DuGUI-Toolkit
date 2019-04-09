using UnityEngine;
using System.Collections;

[AddComponentMenu("UI/UI Follow3D")]
/**
 * A UILabel will follow a 3D position in 2D space. Simply attach to the Element2D's Game Object and reference a target Transform, and you're good!
 * Currently only uses the Main Camera's screen coordinates.
 * */
public class UI_Follow3D : MonoBehaviour
{	
	private Transform target;
	private Canvas mCanvas;
	private RectTransform mCanvasRectTransform;
	private Camera _cam3d;
	public Transform BodyTransform;

	// private float UI_SCREEN_WIDTH = 1920;
	// private float UI_SCREEN_HEIGHT = 1080;
		
	public Vector2 pixelOffset = Vector2.zero;
	public Vector3 offset3D = Vector3.zero;
	
	public bool scaleInWorld = false;
	public float ScaleOneDistance = 10;
	public float ScaleMinDistance = 20;
	public float minScale = 0.5f;
	
	public bool targetIsVector = false;
	public Vector3 _targetVector = Vector3.zero;
	
	
	float scaleFactor = 1;
	Vector3 screenpos;
	
	Vector3 lastTargePosition = Vector3.zero;
	Vector3 lastCameraPosition = Vector3.zero;
	Quaternion lastCameraRotation = Quaternion.identity;
	float lastScreenPosZ = 0;
	
	Transform _camTransform;
	
	// Use this for initialization
	void Awake() 
	{	
		if (ScaleMinDistance - ScaleOneDistance > 0)
		{
			scaleFactor = (1 - minScale) / (ScaleMinDistance - ScaleOneDistance);
		}
		if (_cam3d != null)
		{
			_camTransform = _cam3d.transform;
		}

		Transform trans = transform;
		for(;trans != null ; trans = trans.parent)
		{
			mCanvas = trans.gameObject.GetComponent<Canvas>();
			if(mCanvas != null)
			{
				SetCanvas(mCanvas);
				break;
			}
		}
		// visible = false;
	}

	void Start()
	{
		// visible = true;
	}
	
	public bool visible
	{
		set
		{
			// foreach (Transform _t in _transform)
			// {
			// 	Element2D element = _t.GetComponent<Element2D>();
			// 	if (element != null)
			// 	{
			// 		element.visible = value;
			// 	}
			// 	else
			// 	{
			// 		_t.gameObject.SetActive(value);
			// 	}
			// }
			gameObject.SetActive(value);
		}
	}
	
	public Transform targetTransform
	{
		set
		{
			target = value;
			targetIsVector = false;
		}
		get
		{
			return target;
		}
	}
	
	public Vector3 targetVector
	{
		set
		{
			_targetVector = value;
			targetIsVector = true;
		}
	}
	
	public Camera cam_3d
	{
		set
		{
			_cam3d = value;
			if (_cam3d != null)
			{
				_camTransform = _cam3d.transform;
			}
		}
		get
		{
			return _cam3d;
		}
	}

	public void SetCanvas(Canvas _canvas)
	{
		mCanvas = _canvas;
		mCanvasRectTransform = _canvas.GetComponent<RectTransform>();
		// UI_SCREEN_WIDTH = mCanvasRectTransform.sizeDelta.x;
		// UI_SCREEN_HEIGHT = mCanvasRectTransform.sizeDelta.y;
	}
	
	// Update is called once per frame
	void LateUpdate() 
	{
		if((target == null && !targetIsVector) || !_camTransform) return;
		
		if (lastCameraPosition == _camTransform.position
			&& lastCameraRotation == _camTransform.rotation)
		{
		    if (targetIsVector)
			{
				if (lastTargePosition == _targetVector)
					return;
				else
					lastTargePosition = _targetVector;
			}
			else
			{
				if (lastTargePosition == target.position)
					return;
				else
					lastTargePosition = target.position;
			}
		}
		else
		{
			lastCameraPosition = _camTransform.position;
			lastCameraRotation = _camTransform.rotation;
		}
		
		if (targetIsVector)
			screenpos = _cam3d.WorldToViewportPoint(_targetVector + offset3D);
		else 
			screenpos = _cam3d.WorldToViewportPoint(target.position + offset3D);

		// Debug.LogError("follow " + screenpos);
		screenpos.x = screenpos.x * mCanvasRectTransform.sizeDelta.x - mCanvasRectTransform.sizeDelta.x/2f;
		screenpos.y = screenpos.y * mCanvasRectTransform.sizeDelta.y - mCanvasRectTransform.sizeDelta.y/2f;
		// Debug.LogError("follow " + screenpos);
		
		transform.localPosition = new Vector3(screenpos.x + pixelOffset.x, screenpos.y + pixelOffset.y, transform.localPosition.z);
		//print(screenpos + "|" + _transform.position);
		if (scaleInWorld)
		{
			if (lastScreenPosZ != screenpos.z)
			{
				lastScreenPosZ = screenpos.z;
				if (screenpos.z < ScaleOneDistance)
				{
					transform.localScale = Vector3.one;
				}
				else if (screenpos.z > ScaleMinDistance)
				{
					transform.localScale = new Vector3(minScale, minScale, minScale);
				}
				else if (ScaleMinDistance - ScaleOneDistance > 0)
				{
					float s = (ScaleMinDistance - screenpos.z) * scaleFactor + minScale;
					transform.localScale = new Vector3(s, s, s);
				}
			}
		}

		if(transform.localPosition.x > mCanvasRectTransform.sizeDelta.x/2f
			|| transform.localPosition.x < mCanvasRectTransform.sizeDelta.x/-2f
			|| transform.localPosition.y > mCanvasRectTransform.sizeDelta.y/2f
			|| transform.localPosition.y < mCanvasRectTransform.sizeDelta.y/-2f)
		{
			if(BodyTransform != null &&BodyTransform.gameObject.activeSelf)
			{
				BodyTransform.gameObject.SetActive(false);
			}
		}
		else
		{
			if(BodyTransform != null && !BodyTransform.gameObject.activeSelf)
			{
				BodyTransform.gameObject.SetActive(true);
			}
		}
	}
	
	protected void OnResolutionChange()
	{
		#if UNITY_EDITOR
		//_screenHeight = PandaUI.Instance.screenDimensions.y;
		#else
		//_screenHeight = Screen.currentResolution.height;
		#endif
	}
	
	// public Transform GetTransform()
	// {
	// 	return BodyTransform;
	// }
}

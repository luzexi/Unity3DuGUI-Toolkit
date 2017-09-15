using UnityEngine;
using System.Collections;

[AddComponentMenu("UI/Follow3D")]
public class UI_Follow3D : MonoBehaviour
{	
	public Transform target;
	private Camera _cam3d;
	private Transform _transform;
	public RectTransform m_CanvasTransform;
		
	public Vector2 pixelOffset = Vector2.zero;
	public Vector3 offset3D = Vector3.zero;
	
	public bool scaleInWorld = false;
	public float ScaleOneDistance = 10;
	public float ScaleMinDistance = 20;
	public float minScale = 0.5f;
	// public bool IsLockY = false;
	public bool targetIsVector = false;
	public Vector3 _targetVector = Vector3.zero;
	
	
	float scaleFactor = 1;
	Vector3 screenpos;
	
	Vector3 lastTargePosition = Vector3.zero;
	Vector3 lastCameraPosition = Vector3.zero;
	float lastFieldOfView = 0;
	Quaternion lastCameraRotation = Quaternion.identity;
	float lastScreenPosZ = 0;
	
	Transform _camTransform;

	bool mChanged = false;
	
	// Use this for initialization
	void Awake() 
	{
		_transform = transform;
		
		if (ScaleMinDistance - ScaleOneDistance > 0)
		{
			scaleFactor = (1 - minScale) / (ScaleMinDistance - ScaleOneDistance);
		}
		if(_cam3d == null)
		{
			_cam3d = Camera.main;
		}
		if (_cam3d != null)
		{
			_camTransform = _cam3d.transform;
		}

		if(m_CanvasTransform == null)
		{
			Canvas can = GetComponentInParent<Canvas>();
			if(can != null)
			{
				m_CanvasTransform = can.GetComponent<RectTransform>();
			}
		}
	}

	void OnEnable()
	{
		mChanged = true;
	}

	public virtual void Init()
	{
		if(m_CanvasTransform == null)
		{
			Canvas can = GetComponentInParent<Canvas>();
			if(can != null)
			{
				m_CanvasTransform = can.GetComponent<RectTransform>();
			}
		}
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
	
	// Update is called once per frame
	void LateUpdate() 
	{
		if((target == null && !targetIsVector) || !_camTransform) return;
		
		if (lastCameraPosition == _camTransform.position
			&& lastCameraRotation == _camTransform.rotation
			&& lastFieldOfView == _cam3d.fieldOfView)
		{
		    if (targetIsVector)
			{
				if (lastTargePosition == _targetVector && !mChanged)
					return;
				else
					lastTargePosition = _targetVector;
			}
			else
			{
				if (lastTargePosition == target.position && !mChanged)
					return;
				else
					lastTargePosition = target.position;
			}
		}
		else
		{
			lastCameraPosition = _camTransform.position;
			lastCameraRotation = _camTransform.rotation;
			lastFieldOfView = _cam3d.fieldOfView;
		}

		mChanged = false;

		if (targetIsVector)
			screenpos = _cam3d.WorldToViewportPoint(_targetVector + offset3D);
		else 
			screenpos = _cam3d.WorldToViewportPoint(target.position + offset3D);

		float canvasWidth = m_CanvasTransform.sizeDelta.x;
        float canvasWidthDiv2 = canvasWidth / 2.0f;
        float canvasHeight = m_CanvasTransform.sizeDelta.y;
        float canvasHeightDiv2 = canvasHeight / 2.0f;
        screenpos.x = screenpos.x * canvasWidth - canvasWidthDiv2;
		screenpos.y = screenpos.y * canvasHeight - canvasHeightDiv2;
		// Debug.LogError("after follow " + screenpos);

		_transform.localPosition = new Vector3(screenpos.x + pixelOffset.x, screenpos.y + pixelOffset.y, _transform.localPosition.z);
		//print(screenpos + "|" + _transform.position);
		if (scaleInWorld)
		{
			if (lastScreenPosZ != screenpos.z)
			{
				lastScreenPosZ = screenpos.z;
				if (screenpos.z < ScaleOneDistance)
				{
					_transform.localScale = Vector3.one;
				}
				else if (screenpos.z > ScaleMinDistance)
				{
					_transform.localScale = new Vector3(minScale, minScale, minScale);
				}
				else if (ScaleMinDistance - ScaleOneDistance > 0)
				{
					float s = (ScaleMinDistance - screenpos.z) * scaleFactor + minScale;
					_transform.localScale = new Vector3(s, s, s);
				}
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
	
	public Transform GetTransform()
	{
		return _transform;
	}
}

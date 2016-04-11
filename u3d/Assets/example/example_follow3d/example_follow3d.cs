using UnityEngine;
using System.Collections;

public class example_follow3d : MonoBehaviour
{
	public UI_Follow3D follow;

	// Use this for initialization
	void Start ()
	{
		follow.targetTransform = this.transform;
		follow.cam_3d = Camera.main;
	}
	
	// Update is called once per frame
	void Update ()
	{
		//
	}
}

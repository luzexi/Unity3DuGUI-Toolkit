using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;



[RequireComponent(typeof(RawImage))]
[AddComponentMenu("UI/UI RawImage Material")]
public class UI_RawImageMat : MonoBehaviour
{
	private RawImage mImage;
	private Material mMat;
	public Color mColor = Color.white;

	void Start()
	{
		mImage = GetComponent<RawImage>();
		mImage.material = Instantiate(mImage.material) as Material;
	}

	void LateUpdate()
	{
		mImage.material.SetColor("_Color",mColor);
	}
}
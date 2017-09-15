using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;
using System.Collections.Generic;



//gui util
public class GUIUtil
{
	public static T Find<T>(Component parent , string name)
		where T: Component
	{
		return Find<T>(parent.transform , name);
	}
	
	public static T Find<T>(GameObject parent , string name)
		where T: Component
	{
		return Find<T>(parent.transform , name);
	}

	public static T Find<T>(Transform parent , string name)
		where T: Component
	{
		if(name == "" || name == string.Empty)
		{
			return parent.GetComponent<T>();
		}

		Transform tr = parent.Find(name);
		if(tr == null)
		{
			Debug.LogError("Can't find the node " + name + " in " + parent.name);
			return default(T);
		}
		T t = tr.GetComponent<T>();
		return t;
	}

	public static Transform Find(Component parent , string name)
	{
		return Find(parent.transform , name);
	}

	public static Transform Find(GameObject parent , string name)
	{
		return Find(parent.transform , name);
	}

	public static Transform Find(Transform parent , string name)
	{
		if(name == "" || name == string.Empty)
		{
			return parent;
		}
		Transform tr = parent.Find(name);
		if(tr == null)
		{
			Debug.LogError("Can't find the node " + name + " in " + parent.name);
		}
		return tr;
	}
}

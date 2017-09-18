using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MenuManager
{
    protected Transform _transform = null;
    protected Dictionary<string, ScreenBaseHandler> _dicScreens = new Dictionary<string, ScreenBaseHandler>();

    public int GetScreenNum()
    {
        return _dicScreens.Count;
    }
    public Dictionary<string, ScreenBaseHandler> GetScreens()
    {
        return _dicScreens;
    }

    public void SetTransform(Transform _t)
    {
        _transform = _t;
    }

    public void Destroy()
    {
        _dicScreens.Clear();
        _transform = null;
    }

    public T CreateMenu<T>() where T : ScreenBaseHandler
    {
        string key = typeof(T).ToString();
        if (_dicScreens.ContainsKey(key))
        {
            if (((T)_dicScreens[key]).gameObject.activeSelf == false)
                ((T)_dicScreens[key]).gameObject.SetActive(true);
            return (T)_dicScreens[key];
        }

        GameObject prefab = GetPrefabFromType(typeof(T));

        if (prefab != null)
        {
            GameObject obj = GameObject.Instantiate(prefab) as GameObject;
            obj.transform.SetParent(_transform);
            obj.transform.localScale = Vector3.one;
            obj.transform.localPosition = Vector3.zero;
            
            T result = obj.GetComponent<T>();

            if (result.m_DestroyOnClose)
                result.OnCloseAndDestroy += RemoveMenu;
            else
                result.OnCloseAndDestroy += DisableMenu;

            _dicScreens.Add(key, result);
            // result.Init();
            return result;
        }

        return null;
    }

    void RemoveMenu(ScreenBaseHandler _handler)
    {
        System.Type type = _handler.GetType();
        string key = type.ToString();
        if (_dicScreens.ContainsKey(key))
        {
            _dicScreens.Remove(key);
        }
        GameObject.Destroy(_handler.gameObject);
    }

    void DisableMenu(ScreenBaseHandler _handler)
    {
        _handler.gameObject.SetActive(false);
    }

    public T FindMenu<T>() where T : ScreenBaseHandler
    {
        string key = typeof(T).ToString();
        if (_dicScreens.ContainsKey(key))
        {
            if(((T)_dicScreens[key]).IsShow)
                return (T)_dicScreens[key];
        }

        return null;
    }

    protected virtual GameObject GetPrefabFromType(System.Type _type)
    {
        if (_type.Equals(typeof(ScreenBaseHandler)))
            return Resources.Load("Prefabs/ExampleUI") as GameObject;

		return null;
    }
}
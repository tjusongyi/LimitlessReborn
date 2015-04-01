using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class ObjectHelper
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="go"></param>
    /// <param name="name">支持目录式的(x/y/z)路径</param>
    /// <returns></returns>
    public static GameObject GetChild(GameObject go, string name)
    {
        if (go == null || string.IsNullOrEmpty(name))
        {
            return null;
        }

        Transform rootTrans = go.transform;
        Transform childTrans = rootTrans.FindChild(name);       //内置支持路径
        return childTrans == null ? null : childTrans.gameObject;

    }

    public static T GetChildComponent<T>(GameObject go, string name) where T : Component
    {
        if (go == null || string.IsNullOrEmpty(name))
        {
            return null;
        }
        GameObject child = GetChild(go, name);
        return child == null ? null : child.GetComponent<T>();

    }

    public static T AddChildComponent<T>(GameObject go, string name) where T : Component
    {
        if (go == null || string.IsNullOrEmpty(name))
        {
            return null;
        }
        GameObject child = GetChild(go, name);
        if (child != null)
        {
            T component = child.GetComponent<T>();
            return component == null ? child.AddComponent<T>() : component;
        }
        else
        {
            return null;
        }

    }
    /// <summary>
    /// 包括所有children
    /// usage : GameObjectUtility.SetLayer(gameObject,LayerMask.NameToLayer("UI"));
    /// </summary>
    /// <param name="go"></param>
    /// <param name="layer"></param>
    /// <returns></returns>
    public static bool SetLayer(GameObject go, int layer)
    {
        if (go == null)
            return false;
        go.layer = layer;
        Transform rootTrans = go.transform;
        for (int i = 0; i < rootTrans.childCount; i++)
        {
            GameObject child = rootTrans.GetChild(i).gameObject;
            SetLayer(child, layer);
            child.layer = layer;
        }
        return true;

    }

    public static void DestroyChildren(GameObject go)
    {
        if (go == null)
            return;
        Transform rootTrans = go.transform;
        for (int i = 0; i < rootTrans.childCount; i++)
        {
            GameObject child = rootTrans.GetChild(i).gameObject;
            DestroyChildren(child);
            UnityEngine.Object.Destroy(child);
        }

    }

    public static GameObject GetParent(GameObject go, string name)
    {
        if (go == null)
        {
            return null;
        }

        Transform parent = go.transform.parent;
        while (parent != null)
        {
            if (parent.name == name)
            {
                return parent.gameObject;
            }
            parent = parent.parent;
        }
        return null;
    }
}




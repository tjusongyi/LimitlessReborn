using UnityEngine;
using System.Collections;
using cn.bmob.api;
using cn.bmob.tools;

/// <summary>
/// 管理云平台
/// </summary>
public class CloudManager : Singleton<CloudManager>
{

    public static BmobUnity bmobUnity;

	// Use this for initialization
	public void Init (System.Action<object> print) {
        //注册调试打印对象
        //获取Bmob的服务组件        
        BmobDebug.Register(print);
        bmobUnity = GameObject.Find("GameManager").GetComponent<BmobUnity>();
	}
	

}

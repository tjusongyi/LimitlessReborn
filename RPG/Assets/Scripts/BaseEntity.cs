using UnityEngine;
using System.Collections;

/// <summary>
/// 所有实体的基类
/// </summary>
public class BaseEntity{

    public Vector3 Postion;
    public Vector3 Rotaion;
    public uint Id;
    public string Name;

    public bool IsReal;   //true才表示这个物品实际存在，才会被创建。服务器的字段，这里只是提示

    protected BaseEntity()
    {
        SyncData(true);
    }

    public void SyncData(bool isGet)
    {

    }

    

    
}

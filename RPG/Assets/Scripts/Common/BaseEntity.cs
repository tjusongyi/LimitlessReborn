using UnityEngine;
using System.Collections;
using cn.bmob.io;
using cn.bmob.json;

/// <summary>
/// 所有实体的基类
/// </summary>
public class BaseEntity : BmobTable{

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
        if(isGet)
        {
           
        }
        else
        {

        }
    }

    public override void readFields(BmobInput input)
    {
        base.readFields(input);

        //this.Postion = input.getInt("score");
        //this.Rotaion = input.getBoolean("cheatMode");
        //this.playerName = input.getString("playerName");
    }

    public override void write(BmobOutput output, bool all)
    {
        base.write(output, all);
        

        output.Put("Position", this.Postion);
        //output.Put("score", this.score);
        //output.Put("cheatMode", this.cheatMode);
        //output.Put("playerName", this.playerName);
    }
    
}

using UnityEngine;
using System.Collections;
using cn.bmob.io;


/// <summary>
/// User类是用来存储账户信息的
/// </summary>
public class User : BmobUser
{

    public override void write(BmobOutput output, bool all)
    {
        base.write(output, all);

        //output.Put("life", this.life);
        //output.Put("attack", this.attack);
    }

    public override void readFields(BmobInput input)
    {
        base.readFields(input);

        //this.life = input.getInt("life");
        //this.attack = input.getInt("attack");
    }

    
}

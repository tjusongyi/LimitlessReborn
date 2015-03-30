using UnityEngine;
using System.Collections;


/// <summary>
/// 所有生物的实体基类，包括玩家和怪物，还有NPC。实体类只用来存取数据。
/// </summary>
public class CreatureEntity : BaseEntity{

    public class BaseAttribute   //不计算装备与技能
    {
        public int level, hp, mp, atack, defend, speed, hit;
        public float criticalRate, atkSpd, atkRange, movespeed, exp;
    }
    public string name;
    public BaseAttribute baseAtrr;

    public void InitEntity()  //如果是一级的话在基础属性的基础上将属性随机化
    {
        if(baseAtrr.level == 1)
        {

        }
        else
        {

        }
    }

    public void Hp()
    {

    }
}

using UnityEngine;
using System.Collections;


/// <summary>
/// 所有生物的实体基类，包括玩家和怪物，还有NPC。实体类只用来存取数据。
/// </summary>
public class CreatureEntity : BaseEntity{

    /// <summary>
    /// searchRange:搜索附近生物的范围，小地图显示时根据生物等级显示不同颜色
    /// 
    /// </summary>
    public struct BaseAttribute   //不计算装备与技能
    {
        public uint hp, mp, attack, defend, hpRegenrate, mpRegenerate, searchRange;
        public float criticalRate,criticalDamage, atkSpd, atkRange, moveSpeed;
    }

    public struct BaseAttibuteDisturbance
    {
        public uint hp, mp, attack, defend, hpRegenrate, mpRegenerate, searchRange;
        public float criticalRate, criticalDamage, atkSpd, atkRange, moveSpeed;
    }

    public class FinalAttribute
    {
        public uint hp, mp, attack, defend, hpRegenrate, mpRegenerate, searchRange;
        public float criticalRate,criticalDamage, atkSpd, atkRange, moveSpeed;
    }
    
    public int MajorLevel;
    public int MinorLevel;
    public uint[,] ExpForLevel;
    public uint Exp;
    public int Species;
    public uint ExpGive;
    public BaseAttribute BaseAtrr;
    public FinalAttribute FinalAtrr;
    //public uint GeneralCombatPoint; //根据基础属性计算出来的综合战力

    public SkillEntity Skills;

    public CreatureEntity()
    {
        ExpForLevel = new uint[9,9];
        ReadFormConfig(Species);
    }

    public void SychData(bool isGet) 
    {
        

    }

    void ReadFormConfig(int species)
    {

    }

    public CreatureEntity InitAttribute()   //如果是一级的话在基础属性的基础上将属性随机化
    {

        return this;
    }
}

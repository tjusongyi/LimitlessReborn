using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Mono.Xml;
using System.Security;


[System.Serializable]
/// <summary>
/// searchRange:搜索附近生物的范围，小地图显示时根据生物等级显示不同颜色
/// 
/// </summary>
public struct BaseAttribute   //不计算装备与技能
{
    public uint hp, mp, attack, defend, hpRegenerate, mpRegenerate, searchRange;
    public float criticalRate, criticalDamage, atkSpd, atkRange, moveSpeed;
}
[System.Serializable]
/// <summary>
/// 所有生物的实体基类，包括玩家和怪物，还有NPC。实体类只用来存取数据。
/// </summary>
public class CreatureEntity : BaseEntity
{
    public int MajorLevel;
    public int MinorLevel;
    public uint[,] ExpForLevel;
    public uint Exp;
    public string Species;
    public uint ExpGive;
    /// <summary>
    /// 从配置中读出来的每个种族的属性
    /// </summary>
    public class AttrForSpecies
    {
        public BaseAttribute BaseAtrr;
        public BaseAttribute BaseAttrDist; //干扰值
    }
    private static Dictionary<string, AttrForSpecies> attrFSpe = new Dictionary<string, AttrForSpecies>();

    public BaseAttribute FinalAtrr;   //计算各种加成之后
    //public uint GeneralCombatPoint; //根据基础属性计算出来的综合战力

    public SkillEntity Skills;

    public CreatureEntity(string species)
    {
        ExpForLevel = new uint[9, 9];
        Species = species;
        if (!attrFSpe.ContainsKey(species))
        {
            ReadFormConfig(species);
        }
        else
        {
            InitAttribute();
        }

    }

    public void SychData(bool isGet)
    {


    }

    void ReadFormConfig(string species)
    {
        SecurityParser sp = new SecurityParser();
        string content = Resources.Load(CommonDef.ConfigPath.CreatureConfig).ToString();
        Debug.Log(content);
        sp.LoadXml(content);
        SecurityElement se = sp.ToXml();
        foreach (SecurityElement elem in se.Children)
        {
            if (elem.Tag == "Creature")
            {
                if (elem.Attribute("species") == species)
                {
                    AttrForSpecies afs = new AttrForSpecies();
                    SecurityElement attrElem = elem.SearchForChildByTag("BaseAttribute");
                    uint.TryParse(attrElem.Attribute("hp"), out afs.BaseAtrr.hp);
                    uint.TryParse(attrElem.Attribute("mp"), out afs.BaseAtrr.mp);
                    uint.TryParse(attrElem.Attribute("attack"), out afs.BaseAtrr.attack);
                    uint.TryParse(attrElem.Attribute("defend"), out afs.BaseAtrr.defend);
                    uint.TryParse(attrElem.Attribute("hpRegenerate"), out afs.BaseAtrr.hpRegenerate);
                    uint.TryParse(attrElem.Attribute("mpRegenerate"), out afs.BaseAtrr.mpRegenerate);
                    uint.TryParse(attrElem.Attribute("searchRange"), out afs.BaseAtrr.searchRange);
                    float.TryParse(attrElem.Attribute("criticalRate"), out afs.BaseAtrr.criticalRate);
                    float.TryParse(attrElem.Attribute("criticalDamage"), out afs.BaseAtrr.criticalDamage);
                    float.TryParse(attrElem.Attribute("atkSpd"), out afs.BaseAtrr.atkSpd);
                    float.TryParse(attrElem.Attribute("moveSpeed"), out afs.BaseAtrr.moveSpeed);
                    float.TryParse(attrElem.Attribute("atkRange"), out afs.BaseAtrr.atkRange);
                    attrElem = elem.SearchForChildByTag("BaseAttributeDisturbance");
                    uint.TryParse(attrElem.Attribute("hp"), out afs.BaseAttrDist.hp);
                    uint.TryParse(attrElem.Attribute("mp"), out afs.BaseAttrDist.mp);
                    uint.TryParse(attrElem.Attribute("attack"), out afs.BaseAttrDist.attack);
                    uint.TryParse(attrElem.Attribute("defend"), out afs.BaseAttrDist.defend);
                    uint.TryParse(attrElem.Attribute("hpRegenerate"), out afs.BaseAttrDist.hpRegenerate);
                    uint.TryParse(attrElem.Attribute("mpRegenerate"), out afs.BaseAttrDist.mpRegenerate);
                    uint.TryParse(attrElem.Attribute("searchRange"), out afs.BaseAttrDist.searchRange);
                    float.TryParse(attrElem.Attribute("criticalRate"), out afs.BaseAttrDist.criticalRate);
                    float.TryParse(attrElem.Attribute("criticalDamage"), out afs.BaseAttrDist.criticalDamage);
                    float.TryParse(attrElem.Attribute("atkSpd"), out afs.BaseAttrDist.atkSpd);
                    float.TryParse(attrElem.Attribute("moveSpeed"), out afs.BaseAttrDist.moveSpeed);
                    float.TryParse(attrElem.Attribute("atkRange"), out afs.BaseAttrDist.atkRange);
                    attrFSpe.Add(species, afs);
                }
            }
        }

    }

    void InitAttribute()   //如果是一级的话在基础属性的基础上将属性随机化
    {


    }
}

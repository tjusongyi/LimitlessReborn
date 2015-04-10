using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Mono.Xml;
using System.Security;

public class EquipmentEntity : BaseEntity
{

    public CommonDef.EquipType type;

    public class AttrForEquipment
    {
        public BaseAttribute BaseAtrr;
        public BaseAttribute BaseAttrDist; //干扰值
    }

    private static Dictionary<string, AttrForEquipment> attrFEquip = new Dictionary<string, AttrForEquipment>();

    public EquipmentEntity(string classname)
    {
        if (!attrFEquip.ContainsKey(classname))
        {
            ReadFormConfig(classname);
        }
        else
        {

        }
    }

    void ReadFormConfig(string classname)
    {
        SecurityParser sp = new SecurityParser();
        string content = Resources.Load(CommonDef.ConfigPath.EquipmentConfig).ToString();
        Debug.Log(content);
        sp.LoadXml(content);
        SecurityElement se = sp.ToXml();
        foreach (SecurityElement elem in se.Children)
        {
            if (elem.Tag == "Creature")
            {
                if (elem.Attribute("species") == classname)
                {
                    AttrForEquipment afe = new AttrForEquipment();
                    SecurityElement attrElem = elem.SearchForChildByTag("BaseAttribute");
                    uint.TryParse(attrElem.Attribute("hp"), out afe.BaseAtrr.hp);
                    uint.TryParse(attrElem.Attribute("mp"), out afe.BaseAtrr.mp);
                    uint.TryParse(attrElem.Attribute("attack"), out afe.BaseAtrr.attack);
                    uint.TryParse(attrElem.Attribute("defend"), out afe.BaseAtrr.defend);
                    uint.TryParse(attrElem.Attribute("hpRegenerate"), out afe.BaseAtrr.hpRegenerate);
                    uint.TryParse(attrElem.Attribute("mpRegenerate"), out afe.BaseAtrr.mpRegenerate);
                    uint.TryParse(attrElem.Attribute("searchRange"), out afe.BaseAtrr.searchRange);
                    float.TryParse(attrElem.Attribute("criticalRate"), out afe.BaseAtrr.criticalRate);
                    float.TryParse(attrElem.Attribute("criticalDamage"), out afe.BaseAtrr.criticalDamage);
                    float.TryParse(attrElem.Attribute("atkSpd"), out afe.BaseAtrr.atkSpd);
                    float.TryParse(attrElem.Attribute("moveSpeed"), out afe.BaseAtrr.moveSpeed);
                    float.TryParse(attrElem.Attribute("atkRange"), out afe.BaseAtrr.atkRange);
                    attrElem = elem.SearchForChildByTag("BaseAttributeDisturbance");
                    uint.TryParse(attrElem.Attribute("hp"), out afe.BaseAttrDist.hp);
                    uint.TryParse(attrElem.Attribute("mp"), out afe.BaseAttrDist.mp);
                    uint.TryParse(attrElem.Attribute("attack"), out afe.BaseAttrDist.attack);
                    uint.TryParse(attrElem.Attribute("defend"), out afe.BaseAttrDist.defend);
                    uint.TryParse(attrElem.Attribute("hpRegenerate"), out afe.BaseAttrDist.hpRegenerate);
                    uint.TryParse(attrElem.Attribute("mpRegenerate"), out afe.BaseAttrDist.mpRegenerate);
                    uint.TryParse(attrElem.Attribute("searchRange"), out afe.BaseAttrDist.searchRange);
                    float.TryParse(attrElem.Attribute("criticalRate"), out afe.BaseAttrDist.criticalRate);
                    float.TryParse(attrElem.Attribute("criticalDamage"), out afe.BaseAttrDist.criticalDamage);
                    float.TryParse(attrElem.Attribute("atkSpd"), out afe.BaseAttrDist.atkSpd);
                    float.TryParse(attrElem.Attribute("moveSpeed"), out afe.BaseAttrDist.moveSpeed);
                    float.TryParse(attrElem.Attribute("atkRange"), out afe.BaseAttrDist.atkRange);
                    attrFEquip.Add(classname, afe);
                }
            }
        }

    }
    void InitAttribute()
    {

    }
}

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CommonDef {

	public enum ResultCodes
    {
        #region login
        EmptyUserName = 0,
        EmptyPassWord,
        InvalidUsername,
        InvalidPassWord,


        #endregion
    }

    public enum ItemType
    {
        Default,

    }

    public enum EquipType
    {
        None = 0, 
        Head_Gear = 1, 
        Armor = 2, 
        Shoes = 3, 
        Accessory = 4, 
        Left_Hand = 5, 
        Right_Hand = 6, 
        Two_Hand = 7
    }

    public enum Species
    {
        SkeletonKing,
    }

    public class ConfigPath
    {
        public static string CreatureConfig = "Config/Creature";
        public static string EquipmentConfig = "Config/Equipment";
    }

}

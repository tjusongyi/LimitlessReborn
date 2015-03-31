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

    public enum Species
    {
        SkeletonKing,
    }

    public class ConfigPath
    {
        public static string creatureConfig = "Config/Creature";
    }

}

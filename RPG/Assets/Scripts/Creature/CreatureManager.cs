using UnityEngine;
using System.Collections;
using System.Collections.Generic;

//Manager类维护该类别所有的实体，比如这里用来维护所有怪物的产生和销毁
public class CreatureManager : Singleton<CreatureManager>
{
    public List<CreatureController> CCList;

    public void Init()
    {
        CCList = new List<CreatureController>();
    }

    public void Create(Vector3 position, Vector3 rotation,string species)
    {
       
        GameObject creature = GameObject.Instantiate(GameObject.CreatePrimitive(PrimitiveType.Cube), position, Quaternion.Euler(rotation)) as GameObject;
        CreatureController cc = creature.AddComponent<CreatureController>();
        cc.Init(species);
        CCList.Add(cc);

        


    }

    public void Delete(CreatureController cc)
    {
        cc.Delete();
        CCList.Remove(cc);

    }
}

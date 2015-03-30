using UnityEngine;
using System.Collections;
using System.Collections.Generic;

//Manager类维护该类别所有的实体，比如这里用来维护所有怪物的产生和销毁
public class CreatureManager {
    public List<CreatureController> CCList;

    CreatureManager()
    {
        CCList = new List<CreatureController>();
    }

    public void Create(Vector3 position, Vector3 rotation,int species)
    {
        CreatureController cc = new CreatureController(species);
        CCList.Add(cc);

    }

    public void Delete(CreatureController cc)
    {
        cc.Delete();
        CCList.Remove(cc);

    }
}

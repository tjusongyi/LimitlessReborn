using UnityEngine;
using System.Collections;
using System.Collections.Generic;

//controller是业务逻辑的封装，对数据的处理和更改，维护单个实体。比如这个controller只维护某一个怪物的状态。
public class CreatureController : MonoBehaviour {

    public CreatureEntity CE;

    [HideInInspector]
    public List<CreatureController> CreaturesInSearchArea;
    //private bool HasTarget;
    private CreatureController target;

    public void Init(string species)
    {
        CE = new CreatureEntity(species);
        CE.InitAttribute();
       
    }

    public CreatureController()
    {
        CreaturesInSearchArea = new List<CreatureController>();
    }

    public void TakeDamage(CreatureController enemy)
    {
        if(true)
        {

        }
    }

    public void Attack(CreatureController enemy)
    {
        enemy.TakeDamage(this);
        if(CE.FinalAtrr.hp <= 0)
        {
            enemy.target = null;
            GainExp(enemy.CE.ExpGive);
        }
    }

    void Update()
    {
        //if(null == target)
        //{
        //    SearchForEnemy();
        //}
        //else
        //{
        //    Attack(target);
        //}
       
    }

    void  SearchForEnemy()
    {
        for(int i = 0;i < CreaturesInSearchArea.Count;i++)
        {
            if(CreaturesInSearchArea[i].CE.MajorLevel < CE.MajorLevel && CreaturesInSearchArea[i].CE.MinorLevel < CE.MinorLevel)
            {
                target = CreaturesInSearchArea[i];
                return;
            }
        }
        target = CreaturesInSearchArea[0];
    }

    public void GainExp(uint exp)
    {
        CE.Exp += exp;
        if (CE.MinorLevel < 9)
        {
            if (CE.Exp > CE.ExpForLevel[CE.MajorLevel - 1, CE.MinorLevel - 1])
            {

                Upgrade();
            }
        }
        

    }

    private void Upgrade()
    {
        CE.MinorLevel++;
    }

    public void Save()
    {
        CE.SychData(false);
    }

    public void Delete()
    {
        CE.IsReal = false;
        CE.SychData(false);
        GameObject.Destroy(gameObject);
    }
}

using UnityEngine;
using System.Collections;
using cn.bmob.tools;

public class GameLogicManager: MonoBehaviour{

    void Awake()
    {
        GameObject managerObject = GameObject.Find("GameManager");
        GameObject.DontDestroyOnLoad(managerObject);
        
        
    }
	// Use this for initialization
	void Start () {
        CloudManager.Instance.Init(print);
        CreatureManager.Instance.Init();
        UserManager.Instance.Init();
        CreatureManager.Instance.Create(Vector3.zero, Vector3.zero, CommonDef.Species.SkeletonKing.ToString());
	}
	
	// Update is called once per frame
	void Update () {

       
	}
}

using UnityEngine;
using System.Collections;

public class GameLogicManager: MonoBehaviour{

    void Awake()
    {
        GameObject managerObject = new GameObject("ManagerRoot");
        GameObject.DontDestroyOnLoad(managerObject);
        //managerObject.AddComponent<CreatureManager>(); //继承Mono的Manger可以作为Component
    }
	// Use this for initialization
	void Start () {
        CreatureManager.Instance.Init();
        UserManager.Instance.Init();
        CreatureManager.Instance.Create(Vector3.zero, Vector3.zero, CommonDef.Species.SkeletonKing.ToString());
	}
	
	// Update is called once per frame
	void Update () {

       
	}
}

/// <summary>
/// Spawner.
/// This script use to spawn a enemy
/// </summary>

using UnityEngine;
using System.Collections;

public class Spawner : MonoBehaviour {
	
	public GameObject[] monsterList; //monster list to spawn
	
	[HideInInspector]
	public Object[] spawnList;
	
	public float spawnTimer; //a time to spawn
	public int limitSpawn; //limit monster in this area
	
	public float areaSpawn; //a radius spawn area
	
	public bool ShowArea; //show gizmos area
	public Color areaColor; //area gizmos color
	
	//Private Variable
	private Vector3 randomSpawnVector;
	private float randomAngle;
	private int countSpawn;
	
	
	
	// Use this for initialization
	void Start () {
		//Start spawn monster
		spawnList = new Object[limitSpawn];
		InvokeRepeating("SpawnMonster",spawnTimer,spawnTimer);
	
	}
	
	// Update is called once per frame
	void Update () {
		//Check limit spawn
		CheckSpawnLimit();
	
	}
	
	//Check limit spawn
	void CheckSpawnLimit()
	{
		if(countSpawn >= limitSpawn)
		{
			CancelInvoke("SpawnMonster");
			FindMissingList();
		}
	}
	
	//spawn monster
	void SpawnMonster()
	{
		Object monSpawn = Instantiate(monsterList[Random.Range(0,monsterList.Length)],RandomPostion(),Quaternion.identity);
		
		for(int i=0;i < spawnList.Length;i++)
		{
			if(spawnList[i] == null)
			{
				spawnList[i] = monSpawn;
				break;	
			}
		}
		
		countSpawn++;
	}
	
	//find missing list(enemy dead)
	void FindMissingList()
	{
		for(int i=0;i < spawnList.Length;i++)
		{
			if(spawnList[i] == null)
			{
				Invoke("SpawnMonster",spawnTimer);
				countSpawn--;
			}
		}
	}
	
	//random spawn enemy in area
	Vector3 RandomPostion()
	{		
		randomAngle = Random.Range(0f,91);
		randomSpawnVector.x = Mathf.Sin(randomAngle) * areaSpawn + transform.position.x;
		randomSpawnVector.z = Mathf.Cos(randomAngle) * areaSpawn + transform.position.z;
		randomSpawnVector.y = transform.position.y;	
		
		return randomSpawnVector;
	}
	
	
	//Draw gizmos area
	void OnDrawGizmosSelected()
	{
		if(!ShowArea)
		{
			Gizmos.color = new Color(0.0f,0.5f,0.5f,0.3f);
			Gizmos.DrawSphere(transform.position,areaSpawn);
		}
	}
	
	void OnDrawGizmos()
	{
		if(ShowArea)
		{
			Gizmos.color = areaColor;
			Gizmos.DrawSphere(transform.position,areaSpawn);
		}
		
	}
	
}

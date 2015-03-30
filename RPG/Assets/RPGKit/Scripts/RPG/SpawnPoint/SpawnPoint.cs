using UnityEngine;
using System.Collections;

public class SpawnPoint : MonoBehaviour 
{
	public int ID;
	public RPGSpawnPoint spawnPoint;
	
	private float currentTimer;
    private Player player;
	
	// Use this for initialization
	void Start () 
	{
		Vector3 position = transform.position;
		position.y = Terrain.activeTerrain.SampleHeight(transform.position) + 0.25f;
		
		transform.position = position;
		currentTimer = 300;

        GameObject go = GameObject.FindGameObjectWithTag("Player");
        player = go.GetComponent<Player>();
	}
	
	public void Init()
	{
		if (ID != 0)
		{
			spawnPoint = Player.Data.GetSpawnPointByID(ID);
		}
		else
		{
			spawnPoint = new RPGSpawnPoint();
		}
	}
	
	
	public void CheckSpawnTime(float amountTime, Vector3 position)
	{
		currentTimer += amountTime;
		
		if (currentTimer >= spawnPoint.Timer && !spawnPoint.IsOnlyEvent)
		{
			float distance = Vector3.Distance(transform.position, position);
			
			if (distance >= spawnPoint.MinPlayerRange && distance <= spawnPoint.MaxPlayerRange)
			{
				currentTimer = 0;
				
				if (!spawnPoint.IsSpawn(player))
					return;	
				
				SpawnCreature();	
			}
		}
	}
	
	private void SpawnCreature()
	{
		int EnemyID = spawnPoint.SelectCreature;
		RPGEnemy enemy =  Storage.LoadById<RPGEnemy>(EnemyID, new RPGEnemy());
		enemy.LoadPrefab();
		GameObject go = (GameObject)Instantiate(enemy.Prefab, transform.position, transform.rotation);
		
		go.GetComponent<Enemy>().IsSpawned = true;
	}
	
	public void EventSpawn()
	{
		int EnemyID = spawnPoint.SelectCreature;
		RPGEnemy enemy =  Storage.LoadById<RPGEnemy>(EnemyID, new RPGEnemy());
		enemy.LoadPrefab();
        Instantiate(enemy.Prefab, transform.position, transform.rotation);
	}
}

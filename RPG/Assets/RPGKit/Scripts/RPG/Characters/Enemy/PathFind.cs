using UnityEngine;
using System.Collections;

//your enemy class
public class PathFind : MonoBehaviour {
	//class for pathfinding
	PathFinder pathFinder;
	//player position
	Transform player;
	// Use this for initialization
	void Start () {
		
		GameObject go = GameObject.FindGameObjectWithTag("Player");
		player = go.transform;
		pathFinder = GetComponent<PathFinder>();
	}
	
	// Update is called once per frame
	void Update () 
	{
		//here you must set from which range can enemy attack
		//I have value 10 but you have to change it depends on enemy weapon (melee, range...)
		if (DistanceFromPlayer > 2)
		{
			//play movement animation for enemy
			
			//be sure that pathfinder is not null, you can remove this condition if you put it for every enemy
			if (pathFinder != null)
			{
				pathFinder.GoTo(player.position);
			}
		}
	}
	
	//calculate distance from player
	float DistanceFromPlayer
	{
		get
		{
			return Vector3.Distance(player.position, transform.position);
		}
	}
}

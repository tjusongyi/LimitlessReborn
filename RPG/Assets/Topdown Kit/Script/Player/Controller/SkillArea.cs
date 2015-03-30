/// <summary>
/// Skill area.
/// This script use for calculate damage skill multiple target
/// </summary>

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SkillArea : MonoBehaviour {
	
	public List<GameObject> monsterInArea = new List<GameObject>();
	public float radiusSkill; //radius skill
	[HideInInspector]
	public Collider[] groupNearbyObject;
	
	public bool showRadiusSkill; //enable/disable radius skill
	
	[HideInInspector]
	public float targetAttack;
	[HideInInspector]
	public float multipleDamage;
	[HideInInspector]
	public float targetHit;
	[HideInInspector]
	public float flichRate;
	[HideInInspector]
	public bool startSkill;
	[HideInInspector]
	public GameObject skillFx;
	[HideInInspector]
	public AudioClip soundFx;
	
	[HideInInspector]
	public EnemyController enemyController;
	
	// Use this for initialization
	void Start () {
		
		monsterInArea.Clear();
		groupNearbyObject = Physics.OverlapSphere(transform.position,radiusSkill);
		foreach(Collider groupObj in groupNearbyObject)
		{
			if(groupObj.gameObject.tag == "Enemy")
			monsterInArea.Add(groupObj.transform.gameObject);	
		}

	}
	
	// Update is called once per frame
	void Update () {
		
		if(startSkill)
		{
			for(int i=0;i<monsterInArea.Count;i++)
			{
				enemyController = monsterInArea[i].GetComponent<EnemyController>();
				enemyController.GetDamage(targetAttack * multipleDamage,targetHit,flichRate,skillFx,soundFx);
				GameObject hero = GameObject.FindGameObjectWithTag("Player");
				enemyController.EnemyLockTarget(hero);
			}
			Destroy(this.gameObject);
			startSkill = false;
			
		}
	
	}
	
	void OnDrawGizmos()
	{
		if(showRadiusSkill)
		{
			Gizmos.color = new Color(1f,0,0,0.5f);
			Gizmos.DrawSphere(transform.position,radiusSkill);
		}
		
	}
	
	public void ReciveParameter(float _targetAttack,float _multipleDamage,float _targetHit,float _flichRate,GameObject _skillFx,AudioClip _soundFx)
	{
		targetAttack = _targetAttack;
		multipleDamage = _multipleDamage;
		targetHit = _targetHit;
		flichRate = _flichRate;
		skillFx = _skillFx;
		soundFx = _soundFx;
	}
	
}

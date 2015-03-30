using UnityEngine;
using System.Collections;

public class SpellProjectile : BaseGameObject 
{
	public RPGSpell spell;
	bool flying = true;
    public Player player;
	// Use this for initialization
	void Start () 
	{
	
	}
	
	// Update is called once per frame
	void Update () 
	{
		if (flying)
            transform.position += transform.forward * player.Hero.Settings.BasicMagicProjectileSpeed * Time.deltaTime;
	}
	
	void OnCollisionEnter(Collision collision)
	{
		GameObject go = collision.gameObject;
		
		if (go == null)
			return;
		Enemy enemy = (Enemy)go.GetComponent<Enemy>();
		
		if (enemy != null && spell.Target == SpelLTargetType.Character)
		{
			
			enemy.SpellDamage(spell);
		}
		flying = false;
		Destroy(gameObject);
	}	
	
	void EnableRigidBody()
	{
		Rigidbody rigid = (Rigidbody)GetComponent<Rigidbody>();
		rigid.useGravity = true;
	}
}

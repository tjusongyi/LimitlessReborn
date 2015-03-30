using UnityEngine;
using System.Collections;

public class Projectile : BaseGameObject 
{
    public Player player;
	public string PrefabName;
	bool flying = true;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () 
	{
		if (flying)
			transform.position += transform.forward * player.Hero.Settings.BasicProjectileSpeed * Time.deltaTime;
	}
	
	void OnCollisionEnter(Collision collision)
	{
		GameObject go = collision.gameObject;
		
		if (go == null)
			return;
		
		Enemy enemy = (Enemy)go.GetComponent<Enemy>();
		if (enemy != null)
		{
			enemy.Damage();
			flying = false;
			
			bool arrowStay = false;
			if (BasicRandom.Instance.Next(100) < player.Hero.Settings.ChanceArrowStay)
				arrowStay = true;
			if (arrowStay && !player.Hero.Settings.UnilimitedAmmo)
			{
				Weapon weapon = GetComponent<Weapon>();
				if (weapon != null)
				{
					enemy.enemy.Container.OnlyLoot = false;
					enemy.enemy.Container.DropItemFromInventory(weapon.rpgItem, player);
				}
			}
			Destroy(gameObject);
		}
		else
		{
			flying = false;
			Destroy(gameObject);
		}
	}	
}

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlayerAttack : BasicGUI
{
	public List<Enemy> CloseEnemies;
    private Texture2D HpBar; 

	public Enemy LockedTarget;
	
	// Use this for initialization
	void Start () 
	{
		CloseEnemies = new List<Enemy>();
        Prepare();
        HpBar = (Texture2D)Resources.Load("GUI/Images/HP");
	}
	
	// Update is called once per frame
	void Update () 
	{
		if (Input.GetKey(KeyCode.Tab))
			TargetEnemy();

        if (LockedTarget == null && CloseEnemies.Count > 0 && CloseEnemies[0].distance < 5)
        {
            TargetEnemy();
        }

        if (LockedTarget != null && LockedTarget.distance > 40)
        {
            CloseEnemies.Remove(LockedTarget);
            LockedTarget = null;
        }

		if (IsPlayerAttack)
		{
			Attack();
		}
	}

    void OnGUI()
    {
        if (LockedTarget != null && LockedTarget.state != EnemyState.Dead && LockedTarget.state != EnemyState.Dying)
        {
            Rect rect = new Rect(ScreenWidth - 150, 30, 200, 30);
            Label(rect, LockedTarget.enemy.Name, Skin.customStyles[0]);

            int maximum = 250;
            float hpBarSize = maximum * ((float)LockedTarget.CurrentHp / (float)LockedTarget.enemy.MaximumHp);
            if (hpBarSize > maximum)
                hpBarSize = maximum;
            float x = ScreenWidth - (260 - (maximum - hpBarSize));
            DrawTexture(x, 5, hpBarSize, 20, HpBar);
        }
    }
	
	void OnLevelWasLoaded(int level)
	{
		CloseEnemies = new List<Enemy>();
	}
	
	public void AddEnemy(Enemy enemy)
	{
		if (CloseEnemies.Contains(enemy))
			return;
		CloseEnemies.Add(enemy);
	}
	
	public void RemoveEnemy(Enemy enemy)
	{
		CloseEnemies.Remove(enemy);
	}

    private void TargetEnemy()
    {
        if (CloseEnemies.Count == 0)
        {
            LockedTarget = null;
            return;
        }

        if (LockedTarget == null)
        {
            LockedTarget = CloseEnemies[0];
        }
        else
        {
            int index = CloseEnemies.IndexOf(LockedTarget);

            if (index < CloseEnemies.Count - 1)
            {
                index++;
            }
            else
            {
                index = 0;
            }
            LockedTarget = CloseEnemies[index];
            if (LockedTarget.state == EnemyState.Dead || LockedTarget.state == EnemyState.Dying)
            {
                CloseEnemies.Remove(LockedTarget);
                LockedTarget = null;
            }
        }
    }

	private void Attack()
	{
		if (CloseEnemies.Count == 0)
			return;
		
		RPGWeapon currentWeapon = player.Hero.Equip.GetCurrentWeapon;
       
		if (currentWeapon == null)
			return;
		Enemy attackTarget = CloseEnemies[0];
		float angle = 180;
		
		foreach(Enemy e in CloseEnemies)
		{
			if (e.OutOfGame)
				continue;
			
			float newAngle = Quaternion.Angle(transform.rotation, Quaternion.LookRotation(e.transform.position - transform.position));
			if (newAngle < angle)
			{
				angle = newAngle;
				attackTarget = e;
			}
		}
        
		if (attackTarget.distance < currentWeapon.MaximumRange && angle < 40)
		{
			//for melee weapons we need to raycast
			if (currentWeapon.Attack == WeaponCombatSkillType.Melee)
			{
				RaycastHit hit = new RaycastHit();
				Ray r = new Ray(transform.position, attackTarget.transform.position - transform.position);
				if (Physics.Raycast(r, out hit))
				{
					if (hit.transform.gameObject != attackTarget.gameObject)
					{
						if (hit.transform.gameObject.GetComponent<Enemy>() == null)
						{
							
						}
						else
						{
							Enemy differentEnemy = hit.transform.gameObject.GetComponent<Enemy>();
                            if (!differentEnemy.OutOfGame)
                            {
                                HitEnemy(differentEnemy, currentWeapon);
                                return;
                            }
						}
					}
					else
					{
						//spawn blood
					}
				}
			}
            HitEnemy(attackTarget, currentWeapon);
		}
		
		//play attack animation
	}

    private void HitEnemy(Enemy e, RPGWeapon currentWeapon)
	{
		//reseting delay
		player.Hero.AttackDelay = 0;
		//range weapon
		if (currentWeapon.NeedAmmo && currentWeapon.Attack == WeaponCombatSkillType.Range)
		{
			Vector3 newPosition = Camera.mainCamera.transform.position + Camera.mainCamera.transform.forward * 2;
			GameObject projectile;
			RPGWeapon currentAmmo = player.Hero.Equip.GetCurrentAmmo;
			if (currentAmmo.ID == 0)
			{
				projectile = (GameObject)Instantiate(Resources.Load(player.Hero.Settings.DefaultArrow), newPosition, Camera.mainCamera.transform.rotation);
				//adding prefab name for storing
				Projectile p = (Projectile)projectile.AddComponent<Projectile>();
				p.PrefabName = player.Hero.Settings.DefaultArrow;
			}
			else
			{
				projectile = (GameObject)Instantiate(Resources.Load(currentAmmo.PrefabName), newPosition, Camera.mainCamera.transform.rotation);
				//adding prefab name for storing
				Projectile p = (Projectile)projectile.AddComponent<Projectile>();
				p.PrefabName = currentAmmo.PrefabName;
                p.player = player;
				//attach weapon script
				Weapon weapon = projectile.AddComponent<Weapon>();
				weapon.ID = currentAmmo.ID;

				weapon.rpgItem = currentAmmo;
				//check if we have to remove ammo
				if (currentWeapon.NeedAmmo || !player.Hero.Settings.UnilimitedAmmo)
					player.Hero.Equip.RemoveOneAmmo(player);
			}
			
		}
		else if (currentWeapon.Attack == WeaponCombatSkillType.Melee)
		{
            AttackStatusEnum attackStatus = e.Damage();
            switch (attackStatus)
            { 
                case AttackStatusEnum.NoDamage:
                    break;
                case AttackStatusEnum.AlreadyDead:
                    RemoveEnemy(e);
                    break;

                case AttackStatusEnum.Damage:
                    player.Hero.Equip.DamageWeapon(player);
                    break;
                case AttackStatusEnum.Killed:
                    player.Hero.Equip.DamageWeapon(player);
                    if (player.Hero.Settings.Leveling == LevelingSystem.XP)
				        player.Hero.AddXp(e.enemy.Experience);
			        player.Hero.Quest.KillEnemy(e.ID);
                    RemoveEnemy(e);
                    break;
            }
           
		}
	}
	
	bool IsPlayerAttack
	{
		get
		{
			if (player.Hero.AttackDelay >= player.Hero.Equip.Weapon.AttackSpeed && Input.GetMouseButton(0))
			{
				player.Hero.AttackDelay = 0;
				return true;
			}
			return false;
		}
	}
}

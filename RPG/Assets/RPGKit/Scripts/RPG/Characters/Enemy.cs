using UnityEngine;
using System.Collections;

public class Enemy : Character
{
	[HideInInspector]
	public RPGEnemy enemy;
	
	[HideInInspector]
	public float distance;
	
	[HideInInspector]
	public EnemyState state;
	
	[HideInInspector]
	public bool IsSpawned;
	
	
	public float MoveY;
	
	public Vector3 Position
	{
		get
		{
			if (MoveY == 0)
				return transform.position;
			
			Vector3 pos = transform.position;
			pos.y += MoveY;
			return pos;
		}
	}
	
	RandomPatrol randomPatrol;
	PathFinder pathFinder;
	float alertRange;
	float timeDelay; 
	bool isContainerInitialized;
	
	private Player player;
	private PlayerAttack playerAttack;
	
	public string WalkAnim;
	public float WalkAnimSpeed = 1;
	
	public float RunAnimSpeed = 1;
	public string RunAnim;
	
	public float Attack1AnimSpeed = 1;
	public string Attack1Anim;
	
	public float Attack2AnimSpeed = 1;
	public string Attack2Anim;
	
	public float DieAnimSpeed = 1;
	public string DieAnim;
	
	public float HitAnimSpeed = 1;
	public string HitAnim;
	
	public float IdleAnimSpeed = 1;
	public string IdleAnim;
	
	private CharacterController c;
	private float rayCastTimer = 0.5f;
	private bool forwardMove = false;
	
	private bool tooClose;
	
	// Use this for initialization
	void Start () 
	{
		enemy = Storage.LoadById<RPGEnemy>(ID, new RPGEnemy());
        if (gameObject.GetComponent<CharacterController>() == null)
		    c = gameObject.AddComponent<CharacterController>();
        else
            c = gameObject.GetComponent<CharacterController>();
		CurrentHp = enemy.MaximumHp;
	
		
		startPosition = transform.position;
		startRotation = transform.rotation;
		
		GameObject go = GameObject.FindGameObjectWithTag("Player");
		player = go.GetComponent<Player>();
		playerAttack = go.GetComponent<PlayerAttack>(); 
		
		randomPatrol = new RandomPatrol(transform, GetComponent<CharacterController>());
		state = EnemyState.Patroling;
		if (alertRange == 0)
			alertRange = 20;
		
		pathFinder = gameObject.AddComponent<PathFinder>();
		
		if (CurrentHp == 0)
			Die();
		else
			enemy.Container.Initialize(player);

        rayCastTimer = 1;
        distance = DistanceFromPlayer;
		
		if (string.IsNullOrEmpty(RunAnim))
			RunAnim = GlobalSettings.animRun;
		
		if (string.IsNullOrEmpty(WalkAnim))
			WalkAnim = GlobalSettings.animWalk;
		
		if (string.IsNullOrEmpty(Attack1Anim))
			Attack1Anim = GlobalSettings.animAttack1;
		
		if (string.IsNullOrEmpty(Attack2Anim))
			Attack2Anim = GlobalSettings.animAttack2;
		
		if (string.IsNullOrEmpty(DieAnim))
			DieAnim = GlobalSettings.animDeath1;


        if (animation == null)
            return;
        if (animation != null)
        {
            animation[DieAnim].wrapMode = WrapMode.ClampForever;
            animation[RunAnim].speed = RunAnimSpeed;
            animation[WalkAnim].speed = WalkAnimSpeed;
            animation[DieAnim].speed = DieAnimSpeed;
        }
	}
	
	public bool IsStored
	{
		get
		{
			if (!IsSpawned)
				return true;
			
			if (CurrentHp != enemy.MaximumHp)
				return true;
			
			if (tooClose)
				return true;
			
			return false;
		}
	}
	
	// Update is called once per frame
	void Update () 
	{
        if (Time.time < 2f)
            return;

		timeDelay += Time.deltaTime;
		distance = DistanceFromPlayer;
		
		if (distance > 90)
			return;
		else if (distance < 40 && (state != EnemyState.Dead && state != EnemyState.Dying))
			playerAttack.AddEnemy(this);
		
		if (CurrentHp <= 0 && state == EnemyState.Dying)
		{
			if (animation == null)
			{
				state = EnemyState.Dead;
			}
			
			if (animation != null && timeDelay > 1)
			{
				state = EnemyState.Dead;
			}
			return;
		}
		
		if (state == EnemyState.Dead)
		{
			Destroy(c);
			if (player.Hero.Settings.DeadBodyWithoutItemVanish && enemy.Container.Items.Count == 0)
			{
				Destroy(gameObject);
				return;
			}
			ContainerControl();
			return;
		}
		
		if (distance < 3f)
		{
			pathFinder.Stop();

			if (timeDelay > enemy.AttackSpeed)
				state = EnemyState.Attacking;
			else
			{
				//waiting for another attack
				state = EnemyState.Idle;
			}
		}
		else if (distance < alertRange)
		{
			//far from player
			state = EnemyState.RunningToPlayer;
		}
		switch(state)
		{
			case EnemyState.Patroling :	
				randomPatrol.Step();
				PlayAnimation(WalkAnim);
				break;
			
			case EnemyState.RunningToPlayer:
				rayCastTimer += Time.deltaTime;
				tooClose = true;
				if (forwardMove || !pathFinder.Moving)
				{
					RotateTransform(player.transform.position);
					c.SimpleMove(transform.forward * 250 * Time.deltaTime);
                    //pathFinder.GoTo(player.transform.position);
				}
				else
				{
					pathFinder.GoTo(player.transform.position);
				}
				PlayAnimation(RunAnim);
				
				break;
			
			case EnemyState.Attacking:
				Attack();
				break;
			
			case EnemyState.Idle:
				PlayAnimation(GlobalSettings.animIdle);
				break;
		}
	}
	
	void LateUpdate()
	{
		distance = DistanceFromPlayer;
		if (rayCastTimer > 0.75f)
		{
			rayCastTimer = 0;
			RaycastHit hit;
					
			if (Physics.Raycast(transform.position, player.transform.position - transform.position, out hit, 55))
			{
					
				if (hit.transform.gameObject == player.gameObject)
				{
					forwardMove = true;
				}	
				else
				{
					forwardMove = false;
				}
			}
			else
			{
				forwardMove = false;
			}
		}
	}
	
	private void RotateTransform(Vector3 lp)
	{
		transform.LookAt(lp);
		
		Vector3 euler = transform.rotation.eulerAngles;
		euler.z = 0;
		euler.x = 0;
			
		transform.rotation = Quaternion.Euler(euler);
	} 
	
	//container control (display it only when dead)
	void ContainerControl()
	{
		if (state != EnemyState.Dead)
			return;
		
		if (BasicGUI.isMainMenuDisplayed || (BasicGUI.isContainerDisplayed && player.rpgContainer.ID != this.ID))
			return;
		
		//if container allow to destroy empty, remove it from game
		if (enemy.Container.Items.Count == 0 && enemy.Container.DestroyEmpty)
			Destroy(gameObject);

        if (Input.GetKey(player.Hero.Controls.ActivateKey) && timeDelay > 0.5f
            && distance <= player.Hero.Settings.ObjectActivateRange && BasicGUI.isContainerDisplayed)
		{
			BasicGUI.isContainerDisplayed = false;
			player.rpgContainer = null;
			timeDelay = 0;
		}

        if (Input.GetKey(player.Hero.Controls.ActivateKey) && timeDelay > 0.5f
            && distance <= player.Hero.Settings.ObjectActivateRange)
		{
			timeDelay = 0;
			BasicGUI.isContainerDisplayed = !BasicGUI.isContainerDisplayed;
			
			if (BasicGUI.isContainerDisplayed)
			{
				Player.Container = ContainerType.Container;
				player.rpgContainer = enemy.Container;
			}
		}
		
		if (Input.GetKey(KeyCode.Escape))
		{
			BasicGUI.isContainerDisplayed = false;
		}
	}
	
	void PlayAnimation(string nameAnimation)
	{
		PlayAnimation(nameAnimation, WrapMode.Loop);
	}
	
	void PlayAnimation(string nameAnimation, WrapMode wrapMode)
	{
		if (animation == null)
			return;
		if (nameAnimation == IdleAnim && animation.isPlaying)
		{
			return;
		}
		if (animation[nameAnimation] != null)
		{
			animation.Play(nameAnimation);
			animation.wrapMode = wrapMode;
		}
	}
	
	float DistanceFromPlayer
	{
		get
		{
			return Vector3.Distance(player.transform.position, transform.position);
		}
	}
	
	void Attack()
	{
		RotateTransform(player.transform.position);
		PlayAnimation(Attack1Anim, WrapMode.Once);
		timeDelay = 0;
		int dmgDone = EnemyStats.CalculateDmg(Level, enemy, CombatSkillType.Melee, player);
		if (dmgDone == -1)
			return;
        
		//do effects from armor
		player.Hero.Equip.OnHitEffects(this, player);

        player.Hero.ChangeHitPoint(dmgDone * -1);	
	}

    public AttackStatusEnum Damage()
	{
		if (state == EnemyState.Dead || state == EnemyState.Dying)
			return AttackStatusEnum.AlreadyDead;
		int dmgDone = PlayerAttackStats.CalculateDmg(Level, enemy, player);

        player.Hero.Equip.HitEffects(this, player);
		return ReceiveDamage(dmgDone);
	}
	
	public void SpellDamage(RPGSpell spell)
	{
		if (state == EnemyState.Dead || state == EnemyState.Dying)
			return;
        if (spell.Spelltype == SpellTypeEnum.Projectile)
        {
            //SPELL DAMAGE EFFECT
            GameObject o = (GameObject)Resources.Load("Spells/Simple/hitEffect4Base");
            if (o != null)
            {
                Instantiate(o, transform.position, transform.rotation);
            }
        }

		int dmgDone = PlayerAttackStats.CalculateSpellDamage(Level, enemy, spell.Effects);
		ReceiveDamage(dmgDone);
	}

    private AttackStatusEnum ReceiveDamage(int dmgDone)
	{
		if (state == EnemyState.Idle || state == EnemyState.Patroling)
		{
			state = EnemyState.RunningToPlayer;
		}
		tooClose = true;
		if (dmgDone == -1)
			return AttackStatusEnum.NoDamage; 
		
		PlayAnimation(HitAnim, WrapMode.Once);
		
		CurrentHp = CurrentHp - dmgDone;
		if (CurrentHp <= 0)
		{
			
			Destroy(pathFinder);
			Die();
            return AttackStatusEnum.Killed;
		}
        return AttackStatusEnum.Damage;
	}
	
	void Die()
	{
		timeDelay = 0;
		if (state == EnemyState.Dead || state == EnemyState.Dying)
			return;
		playerAttack.RemoveEnemy(this);
		PlayAnimation(DieAnim, WrapMode.ClampForever);
		this.CurrentHp = 0;
		
		state = EnemyState.Dying;
	}

    public bool OutOfGame
    {
        get
        {
            if (state == EnemyState.Dead || state == EnemyState.Dying)
                return true;

            return false;
        }
    }
}

public enum EnemyState
{
	Idle = 0,
	Patroling = 1,
	RunningToPlayer = 2,
	Attacking = 3,
	Dying = 4,
	Dead = 5
}

public enum AttackStatusEnum
{ 
    AlreadyDead,
    NoDamage,
    Damage,
    Killed
}
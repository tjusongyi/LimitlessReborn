/// <summary>
/// Enemy controller.
/// This is a main script of enemy use to control enemy
/// </summary>

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[RequireComponent (typeof (AnimationManagerEnemy))]
[RequireComponent (typeof (EnemyStatus))]
[RequireComponent (typeof (CharacterController))]
[RequireComponent (typeof (DropItem))]
public class EnemyController : MonoBehaviour {
	
	public enum ControlAnimationState {Idle,Move,WaitAttack,Attack,TakeAtk,Death}; //enemy state
	public enum EnemyBehavior {Standing,MoveAround} //enemy behavior
	public enum EnemyNature {Natural,Wild} //enemy nature
	
	public enum MoveAroundBehavior {MoveToNext,Waiting}
	
	public EnemyBehavior behavior; //enemy behavior
	public EnemyNature nature;   //enemy nature
	public MoveAroundBehavior moveBehavior; //movement behavior
	
	public GameObject target;     //Target enemy
	public bool chaseTarget;	  //Chase Enemy
	public List<GameObject> modelMesh = new List<GameObject>(); //model mesh
	public Color colorTakeDamage; //color model when take attack
	public float deadTimer; //destroy timer when enemy dead
	public bool deadTransperent; //if enable this when enemy dead it will transperent
	public float speedFade; //speed transperent
	public bool regenHP;		//regen hp when enemy return to spawn point
	public bool regenMP;		//regen mp when enemy return to spawn point
	
	public ControlAnimationState ctrlAnimState; //Control Animation State
	
	public float movePhase;  //area enemy move
	public float returnPhase; //range when enemy return to spawn point
	public float delayNextTargetMin; //delay when idle - min
	public float delayNextTargetMax; //delay when idel - max
	
	
	
	//Private variable
	//Get component other script
	private AnimationManagerEnemy animationManager;
	private EnemyStatus enemyStatus;
	private CharacterController controller;
	
	private float defaultReturnPhase;  //default return phase
	private Vector3 spawnPoint; 		//first spawn point
	private float timeToWaitBeforeNextMove;  //Time to wait before go to next target
	private float delayAttack = 100;		//Delay Attack speed
	private Vector3 destinationPosition;		// The destination Point
	private float destinationDistance;			// The distance between this.transform and destinationPosition
	private Vector3 movedir;
	private float moveSpeed;						// The Speed the character will move
	private Vector3 ctargetPos;					//Convert Target Position
	private Vector3 targetPos;					//Target Pos
	private Quaternion targetRotation;			//Rotation
	private bool checkCritical;					//Check Critical
	private float flinchValue = 100;						//Check Enemy flinch
	private Vector3 randomMoveVector;			//Randome Move Position
	private float randomAngle;					//Random Angle Move
	private Shader alphaShader;				 //Use to transperent model
	private Color[] defaultColor;				//Default Material Color
	private bool enableRegen;					//Check Regen HP/MP
	private GameObject detectArea;				//get detectArea if natural is wild
	
	private float fadeValue;					// Fade Value
	[HideInInspector]
	public float defaultHP,defaultMP;			//DefaultHP,MP
	
	[HideInInspector]
	public int typeAttack;						//Type Attack
	[HideInInspector]
	public int typeTakeAttack;					//Type TakeAttack
	[HideInInspector]
	public bool startFade;					//Check setup fade material
	
	
	//Editor Variable
	[HideInInspector]
	public int sizeMesh;

	
	// Use this for initialization
	void Start () {	
		
		//set spawn point
		destinationPosition = this.transform.position;
		
		//get anathor component
		animationManager = this.GetComponent<AnimationManagerEnemy>();
		enemyStatus = this.GetComponent<EnemyStatus>();
		controller = this.GetComponent<CharacterController>();
		
		delayAttack = 100;	//Declare delay 100 sec
		flinchValue = 100; //Declare flinch value (if zero it will flinch)
		fadeValue = 1; //Set fade value
		
		//Set first spawn point
		spawnPoint = transform.position;
		
		//set default value
		defaultReturnPhase = returnPhase;	
		defaultHP = enemyStatus.status.hp;
		defaultMP = enemyStatus.status.mp;
		
		defaultColor = new Color[modelMesh.Count];
		SetDefualtColor();	
		
		if(behavior == EnemyBehavior.MoveAround)
		{
			moveBehavior = MoveAroundBehavior.Waiting;
		}
		
		//warning when enemy isn't detect area
		if(nature == EnemyNature.Wild)
		{
			detectArea = GameObject.Find("DetectArea");
			
			if(detectArea == null)
			{
				Debug.LogWarning("Don't found DetectArea in Enemy -" + enemyStatus.name);	
			}
			
		}
		
	}
	
	// Update is called once per frame
	void Update () {
		
		EnemyAnimationState();
		
		if(ctrlAnimState != ControlAnimationState.Death)
		{
			if(behavior == EnemyBehavior.MoveAround && !chaseTarget && !target)
			{
				 if (moveBehavior == MoveAroundBehavior.MoveToNext)
			        {
			            UpdateMovingToNextPatrolPoint();
			        }
			        else if (moveBehavior == MoveAroundBehavior.Waiting)
			        {
			            UpdateWaitingForNextMove();
			        }
				
			}else
			{
				EnemyMovementBattle();	
			}
		}
		
		
	}

	//State Enemy
	void EnemyAnimationState() {
		
		if(ctrlAnimState == ControlAnimationState.Idle)
		{
			animationManager.animationState = animationManager.Idle;
		}
		
		if(ctrlAnimState == ControlAnimationState.Move)
		{
			animationManager.animationState = animationManager.Move;
		}
		if(ctrlAnimState == ControlAnimationState.WaitAttack)
		{
			animationManager.animationState = animationManager.Idle;
			WaitAttack();

		}
		if(ctrlAnimState == ControlAnimationState.Attack)
		{
			LookAtTarget(target.transform.position);
			
			if(checkCritical)
			{
				animationManager.animationState = animationManager.CriticalAttack;
				delayAttack = 100;
			}else if(!checkCritical)
			{
				animationManager.animationState = animationManager.Attack;
				delayAttack = 100;
			}
		}
		
		if(ctrlAnimState == ControlAnimationState.TakeAtk)
		{
			animationManager.animationState = animationManager.TakeAttack;
		}
		
		if(ctrlAnimState == ControlAnimationState.Death)
		{
			
			if(this.gameObject.tag == "Enemy")
			{
				this.gameObject.tag = "Untagged";
				this.GetComponent<DropItem>().UseDropItem();
				
			}
			

			animationManager.animationState = animationManager.Death;
			
			if(startFade)
			{
				DeadTransperentAlpha(speedFade);	
			}
			
		}
		
		
	}
	
	//randomw move position
	void RandomPostion()
	{
		randomAngle = Random.Range(0f,91);
		randomMoveVector.x = Mathf.Sin(randomAngle) * movePhase + spawnPoint.x;
		randomMoveVector.z = Mathf.Cos(randomAngle) * movePhase + spawnPoint.z;
		randomMoveVector.y = transform.position.y;

		targetRotation = Quaternion.LookRotation(randomMoveVector - transform.position);
		destinationPosition = randomMoveVector;	
	}
	
	//move to next target
	void UpdateMovingToNextPatrolPoint()
    {
        destinationDistance = Vector3.Distance(destinationPosition,this.transform.position); //Check Distance Spawnpoint to Enemy

        if (destinationDistance < 1f)
        {
            timeToWaitBeforeNextMove = Random.Range(delayNextTargetMin, delayNextTargetMax);
             moveBehavior = MoveAroundBehavior.Waiting;
            ctrlAnimState = ControlAnimationState.Idle;
			moveSpeed = 0;
        }
		
		// Move to destination
		if(ctrlAnimState == ControlAnimationState.Move){
			
			this.transform.rotation = Quaternion.Lerp(this.transform.rotation,targetRotation,Time.deltaTime *25);
			
			if(controller.isGrounded){
				movedir = Vector3.zero;
				movedir = transform.TransformDirection(Vector3.forward*moveSpeed);
			}
			
		}else
		{
			movedir = Vector3.Lerp(movedir,Vector3.zero,Time.deltaTime * 10);
		}
		movedir.y -= 20 * Time.deltaTime;
		controller.Move(movedir*Time.deltaTime);
    }
	
	//wait for next move
	void UpdateWaitingForNextMove()
    {
        timeToWaitBeforeNextMove -= Time.deltaTime;
        if (timeToWaitBeforeNextMove < 0.0f)
        {
            RandomPostion();
            moveBehavior = MoveAroundBehavior.MoveToNext;
            ctrlAnimState = ControlAnimationState.Move;
			moveSpeed = enemyStatus.status.movespd;
        }
    }
	
	
	//Enemy movement when enemy found target
	void EnemyMovementBattle()
	{
		if(chaseTarget == true)
		{
			LookAtTarget(target.transform.position);
						
			destinationDistance = Vector3.Distance(target.transform.position, this.transform.position); //Check Distance Enemy to Hero
				
			if(destinationDistance <= enemyStatus.status.atkRange){		// Reset speed to 0
					
				if(ctrlAnimState == ControlAnimationState.Move || ctrlAnimState == ControlAnimationState.Idle)
				ctrlAnimState = ControlAnimationState.WaitAttack;
					
					moveSpeed = 0;
			}
				else if(destinationDistance > enemyStatus.status.atkRange){			// To Reset Speed to default
					
					if(ctrlAnimState == ControlAnimationState.Move || ctrlAnimState == ControlAnimationState.Idle || ctrlAnimState == ControlAnimationState.WaitAttack)
					ctrlAnimState = ControlAnimationState.Move;
					moveSpeed = enemyStatus.status.movespd;
			}
		}else if(chaseTarget == false)
		{
			LookAtTarget(spawnPoint);			
						
			destinationDistance = Vector3.Distance(spawnPoint, this.transform.position); //Check Distance Enemy to Hero
				
			if(destinationDistance <= enemyStatus.status.atkRange){		// Reset speed to 0
					
				if(ctrlAnimState == ControlAnimationState.Move || ctrlAnimState == ControlAnimationState.Idle)
				ctrlAnimState = ControlAnimationState.Idle;
						
				moveSpeed = 0;
				
				returnPhase = defaultReturnPhase;
				
				if(enableRegen)
				{
					if(regenHP)
						enemyStatus.status.hp = Mathf.FloorToInt(defaultHP);
					
					if(regenMP)
						enemyStatus.status.mp = Mathf.FloorToInt(defaultMP);
					
					destinationPosition = this.transform.position;
					target = null;
				}
			}
				else if(destinationDistance > enemyStatus.status.atkRange){			// To Reset Speed to default
						
				if(ctrlAnimState == ControlAnimationState.Move || ctrlAnimState == ControlAnimationState.Idle || ctrlAnimState == ControlAnimationState.WaitAttack)
				ctrlAnimState = ControlAnimationState.Move;
				moveSpeed = enemyStatus.status.movespd;
			}
		}
		
		//Check distance spawn
		float distanceSpawn = Vector3.Distance(spawnPoint, this.transform.position);
		if(distanceSpawn > returnPhase)
		{
			if(regenHP || regenMP)
			enableRegen = true;
			
			chaseTarget = false;
			
			returnPhase += 3;
		}
		
		
		// Move to destination
		if(ctrlAnimState == ControlAnimationState.Move){
			
			if(controller.isGrounded){
				movedir = Vector3.zero;
				movedir = transform.TransformDirection(Vector3.forward*moveSpeed);
			}
			
		}else
		{
			movedir = Vector3.Lerp(movedir,Vector3.zero,Time.deltaTime * 10);
		}
		movedir.y -= 20 * Time.deltaTime;
		controller.Move(movedir*Time.deltaTime);
	}
	
	
	//Wait before attack
	void WaitAttack()
	{	
		PlayerStatus playerStatus;
		playerStatus = target.GetComponent<PlayerStatus>();
		if(playerStatus.statusCal.hp <= 0)
		{
			ResetState();	
		}
		

		if(delayAttack > 0)
		{
			delayAttack -= Time.deltaTime * enemyStatus.status.atkSpd;	
		}else if(delayAttack <= 0)
		{
			checkCritical = CriticalCal(enemyStatus.status.criticalRate);
				
			if(checkCritical)
			{
				typeAttack = Random.Range(0,animationManager.criticalAttack.Count);
				animationManager.checkAttack = false;
			}else if(!checkCritical)
			{
				typeAttack = Random.Range(0,animationManager.normalAttack.Count);
				animationManager.checkAttack = false;
			}
			
			ctrlAnimState = ControlAnimationState.Attack;
			
		}
	}
	
	
	//Look at target method
	void LookAtTarget(Vector3 _targetPos)
	{
		targetPos.x = _targetPos.x;
		targetPos.y = this.transform.position.y;
		targetPos.z = _targetPos.z;
		this.transform.LookAt(targetPos);
	}
	
	//Critical Calculate
	bool CriticalCal(float criticalStat)
	{
		float calCritical = criticalStat - Random.Range(0,101f);
		
		if(calCritical > 0)
		{
			return true; //Critical
		}else
		{
			return false; //Not Critical
		}
	}
	
	
	//Get damage method
	public void GetDamage(float targetAttack,float targetHit,float flichRate,GameObject atkEffect,AudioClip atksfx)
	{
		//Calculate Hit
		targetHit += Random.Range(-10,30);
		
		//Random take damage animation
		typeTakeAttack = Random.Range(0,animationManager.takeAttack.Count);
		
		if(enemyStatus.status.spd - targetHit > 0) //Attack Miss
		{
			SoundManager.instance.PlayingSound("Attack_Miss");
			InitTextDamage(Color.white,"Miss");
			
			//Lock Target
			if(!chaseTarget)
				chaseTarget = true;
			
			if(ctrlAnimState == ControlAnimationState.Idle || ctrlAnimState == ControlAnimationState.Move)
			ctrlAnimState = ControlAnimationState.WaitAttack;
			
		}else
		{
			int damage = Mathf.FloorToInt((targetAttack - enemyStatus.status.def) * Random.Range(0.8f,1.2f));
			
			if(damage <= 5)
			{
				damage = Random.Range(1,11); // if def < enemy attack
			}
			
			//Play SFX
			if(atksfx)
			AudioSource.PlayClipAtPoint(atksfx,transform.position);
			//Spawn Effect
			if(atkEffect)
			Instantiate(atkEffect,transform.position,Quaternion.identity);
			
			InitTextDamage(Color.white,damage.ToString());
			
			enemyStatus.status.hp -= damage;
			GetDamageColorReset();
			
			if(enemyStatus.status.hp <= 0)
			{
				enemyStatus.status.hp = 0;
				
				//Reset State Hero when kill this enemy
				
					if(target)
					{
						HeroController enemy;
						enemy = target.GetComponent<HeroController>();
						enemy.ResetState();
						PlayerStatus pStatus;
						pStatus = target.GetComponent<PlayerStatus>();
						pStatus.status.exp += enemyStatus.expGive;
					
					}
					Destroy(this.gameObject,deadTimer);

				ctrlAnimState = ControlAnimationState.Death;
			}else
			{
				if(!chaseTarget)
					chaseTarget = true;
				
				flinchValue -= flichRate;
				
				if(flinchValue <= 0)
				{
					ctrlAnimState = ControlAnimationState.TakeAtk;
					flinchValue = 100;
				}else
				{
					ctrlAnimState = ControlAnimationState.WaitAttack;
				}
				
			}
			
		}
	}
	
	//reset all state of enemy
	public void ResetState()
	{
		target = null;
		chaseTarget = false;
		destinationPosition = this.transform.position;
		ctrlAnimState = ControlAnimationState.Idle;	
		
	}
	
	
	void SetDefualtColor()
	{
		int index = 0;
		while(index < modelMesh.Count){
			defaultColor[index] = modelMesh[index].renderer.material.color;
			index++;
		}
	}
	
	void GetDamageColorReset()
	{
		int index = 0;
		while(index < modelMesh.Count){
			modelMesh[index].renderer.material.color = defaultColor[index];
			index++;
		}
		
		StartCoroutine(GetDamageColor(0.2f));
	}
	
	
	private IEnumerator GetDamageColor(float time){
		//if take damage material monster will change to setting color
		int index = 0;
		Color[] colorDef = new Color[modelMesh.Count];
		while(index < modelMesh.Count){
			colorDef[index] = modelMesh[index].renderer.material.color;
			modelMesh[index].renderer.material.color = colorTakeDamage;
			index++;
		}
		yield return new WaitForSeconds(time);
		index = 0;
		while(index < modelMesh.Count){
			modelMesh[index].renderer.material.color = colorDef[index];
			index++;
		}
		yield return 0;
		StopCoroutine("GetDamageColor");
	}
	
	public void InitTextDamage(Color colorText,string damageGet){
		// Init text damage
		GameObject loadPref = (GameObject)Resources.Load("TextDamage");
		GameObject go = (GameObject)Instantiate(loadPref, transform.position + (Vector3.up*1.0f), Quaternion.identity);
		go.GetComponentInChildren<TextDamage>().SetDamage(damageGet, colorText);
	}
	
	public void EnemyLockTarget(GameObject targetlock)
	{
		target = targetlock;
	}
	
	public void DeadTransperentSetup()
	{
		int index = 0;
		
		alphaShader = Shader.Find("Transparent/Diffuse");	
		
		while(index < modelMesh.Count)
		{
			modelMesh[index].renderer.material.shader = alphaShader; 
			index++;	
		}
		
		startFade = true;
		deadTransperent = false;
	}
	
	void DeadTransperentAlpha(float speedFade){
		//Model enemy will fade alpha to zero when dead
		int index = 0;
		Color[] colorDef = new Color[modelMesh.Count];
		while(index < modelMesh.Count){
			
			colorDef[index] = modelMesh[index].renderer.material.color;
			Color alphaColor = new Color(modelMesh[index].renderer.material.color.r,modelMesh[index].renderer.material.color.g,modelMesh[index].renderer.material.color.b,fadeValue);
			modelMesh[index].gameObject.renderer.material.color = alphaColor;
			
			if(modelMesh[index].gameObject.renderer.material.color.a > 0)
			{
				if(fadeValue > 0)
					fadeValue -= Time.deltaTime * speedFade;
				if(fadeValue < 0)
				{
					fadeValue = 0;
				}
			}
			
			index++;
			
		}
		
		

	}
	
	
	void OnControllerColliderHit(ControllerColliderHit hit)
	{	
		//Random move again when collision collider
		if(hit.gameObject.tag == "Collider" && behavior == EnemyBehavior.MoveAround && !chaseTarget && !target)
		{
			ctrlAnimState = ControlAnimationState.Idle;
			moveBehavior = MoveAroundBehavior.Waiting;	
		}
	}
	
	//Gizmoz Variable
	
	private float movePhaseCurrent;
	private float returnPhaseCurrent;
	
	//Draw Gizmoz
	[ExecuteInEditMode]
	void OnDrawGizmos()
	{
		if(movePhase > 0 && movePhase != movePhaseCurrent)
		{
			Gizmos.color = new Color(0f,0.5f,0f,0.3f);
			Gizmos.DrawSphere(transform.position,movePhase);
		}else
		
		if(returnPhase > 0 && returnPhase != returnPhaseCurrent)
		{
			Gizmos.color = new Color(0.5f,0,0,0.3f);
			Gizmos.DrawSphere(transform.position,returnPhase);
		}
		
		movePhaseCurrent = movePhase;
		returnPhaseCurrent = returnPhase;
	}
}

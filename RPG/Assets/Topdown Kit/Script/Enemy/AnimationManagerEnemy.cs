/// <summary>
/// Animation manager enemy.
/// This script use to control a enemy animation
/// </summary>

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AnimationManagerEnemy : MonoBehaviour {
	
	
	public delegate void AnimationHandle();
	public AnimationHandle animationState;
		
	
	[System.Serializable]
	public class AnimationType01
	{
		public AnimationClip animation;
		public float speedAnimation = 1.0f;
	}
	[System.Serializable]
	public class AnimationType02
	{
		public AnimationClip animation;
		public float speedAnimation = 1.0f;
		public bool speedTuning;
	}
	
	[System.Serializable]
	public class AnimationNormalAttack
	{
		public string _name;
		public AnimationClip animation;
		public float speedAnimation = 1.0f;
		public float attackTimer = 0.5f;
		public float multipleDamage = 1f;
		public float flinchValue;
		public bool speedTuning;
		
		public GameObject attackFX;
		public AudioClip soundFX;
		
	}
	[System.Serializable]
	public class AnimationCritAttack
	{
		public string _name;
		public AnimationClip animation;
		public float speedAnimation = 1.0f;
		public float attackTimer = 0.5f;
		public float multipleDamage = 1f;
		public float flinchValue;
		public bool speedTuning;
		
		public GameObject attackFX;
		public AudioClip soundFX;
		
	}
	
	[System.Serializable]
	public class AnimationTakeAttack
	{
		public string _name;
		public AnimationClip animation;
		public float speedAnimation = 1.0f;
		
	}
	
	public AnimationType01 idle,death;  //Idle , death animation
	public AnimationType02 move; //Move animation
	public List<AnimationNormalAttack> normalAttack; //normal attack animation
	public List<AnimationCritAttack> criticalAttack; //critical attack animation
	public List<AnimationTakeAttack> takeAttack; //take attack animation
	
	
	//Private variable
	private EnemyController enemyController;
	private EnemyStatus enemyStatus;
	[HideInInspector]
	public bool checkAttack;
	
	//Editor Variable
	[HideInInspector]
	public int sizeNAtk=0;
	[HideInInspector]
	public int sizeCritAtk=0;
	[HideInInspector]
	public int sizeTakeDmg=0;
	[HideInInspector]
	public List<bool> showNormalAtkSize = new List<bool>();
	[HideInInspector]
	public List<bool> showCritSize = new List<bool>();
	[HideInInspector]
	public List<bool> showTakeDmgSize = new List<bool>();
	
	
	// Use this for initialization
	void Start () {
		
		enemyController = this.GetComponent<EnemyController>();
		enemyStatus = this.GetComponent<EnemyStatus>();
	
	}
	
	// Update is called once per frame
	void Update () {
		
		if(animationState != null){
			animationState();	
		}
	
	}
	
	//Idle method
	public void Idle(){
		animation.CrossFade(idle.animation.name);
		animation[idle.animation.name].speed = idle.speedAnimation;
	}
	
	//Move method
	public void Move(){
		animation.Play(move.animation.name);
		
		if(move.speedTuning)  //Enable Speed Tuning
		{
			animation[move.animation.name].speed = (enemyStatus.status.movespd/3f)/move.speedAnimation;	
		}else
		{
			animation[move.animation.name].speed = move.speedAnimation;
		}
		
		
	}
	
	//Attack method
	public void Attack()
	{	
		animation.Play(normalAttack[enemyController.typeAttack].animation.name);
		
		if(normalAttack[enemyController.typeAttack].speedTuning)  //Enable Speed Tuning
		{
			animation[normalAttack[enemyController.typeAttack].animation.name].speed = (enemyStatus.status.atkSpd/100f)/normalAttack[enemyController.typeAttack].speedAnimation;	
		}else
		{
			animation[normalAttack[enemyController.typeAttack].animation.name].speed = normalAttack[enemyController.typeAttack].speedAnimation;
		}
			
		//Calculate Attack
		if(animation[normalAttack[enemyController.typeAttack].animation.name].normalizedTime > normalAttack[enemyController.typeAttack].attackTimer && !checkAttack)
		{
			//Attack Damage
			HeroController enemy;
			enemy = enemyController.target.GetComponent<HeroController>();
			enemy.GetDamage(enemyStatus.status.atk * normalAttack[enemyController.typeAttack].multipleDamage ,enemyStatus.status.hit,normalAttack[enemyController.typeAttack].flinchValue
							,normalAttack[enemyController.typeAttack].attackFX,normalAttack[enemyController.typeAttack].soundFX);
			checkAttack = true;
		}
			
		if(animation[normalAttack[enemyController.typeAttack].animation.name].normalizedTime > 0.9f)
		{
			enemyController.ctrlAnimState = EnemyController.ControlAnimationState.WaitAttack;
			checkAttack = false;
		}
	}
	
	//Critical method
	public void CriticalAttack()
	{	
		animation.Play(criticalAttack[enemyController.typeAttack].animation.name);
		
		if(criticalAttack[enemyController.typeAttack].speedTuning)  //Enable Speed Tuning
		{
			animation[criticalAttack[enemyController.typeAttack].animation.name].speed = (enemyStatus.status.atkSpd/100f)/criticalAttack[enemyController.typeAttack].speedAnimation;	
		}else
		{
			animation[criticalAttack[enemyController.typeAttack].animation.name].speed = criticalAttack[enemyController.typeAttack].speedAnimation;
		}
			
		//Calculate Attack
		if(animation[criticalAttack[enemyController.typeAttack].animation.name].normalizedTime > criticalAttack[enemyController.typeAttack].attackTimer && !checkAttack)
		{
			//Attack Damage
			HeroController enemy;
			enemy = enemyController.target.GetComponent<HeroController>();
			
			enemy.GetDamage(enemyStatus.status.atk * criticalAttack[enemyController.typeAttack].multipleDamage ,10000,criticalAttack[enemyController.typeAttack].flinchValue
							,criticalAttack[enemyController.typeAttack].attackFX,criticalAttack[enemyController.typeAttack].soundFX);
		
			checkAttack = true;
		}
			
		if(animation[criticalAttack[enemyController.typeAttack].animation.name].normalizedTime > 0.9f)
		{
			enemyController.ctrlAnimState = EnemyController.ControlAnimationState.WaitAttack;
			checkAttack = false;
		}
	}
	
	//Take attack method
	public void TakeAttack(){
		animation.CrossFade(takeAttack[enemyController.typeTakeAttack].animation.name);
		animation[takeAttack[enemyController.typeTakeAttack].animation.name].speed = takeAttack[enemyController.typeTakeAttack].speedAnimation;
		
		if(animation[takeAttack[enemyController.typeTakeAttack].animation.name].normalizedTime > 0.9f)
		{
			enemyController.ctrlAnimState = EnemyController.ControlAnimationState.WaitAttack;
		}
	}
	
	//Death method	
	public void Death()
	{
		animation.CrossFade(death.animation.name);
		animation[death.animation.name].speed = death.speedAnimation;
		
		if(animation[death.animation.name].normalizedTime > 0.9f)
		{
			if(enemyController.deadTransperent)
			{
				enemyController.DeadTransperentSetup();
			}
		}
	}
}

using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

[CustomEditor(typeof(AnimationManagerEnemy))]
public class AnimationManagerEnemyEditor : Editor {
	
	private bool showIdle = true,showDeath = true,showMove = true,showNormalAtk = true,showCritAtk = true,showTakeDmg = true;
	
	public override void OnInspectorGUI()
	{	
		AnimationManagerEnemy animManagerEnemy = (AnimationManagerEnemy)target;
		
		showIdle = EditorGUILayout.Foldout(showIdle,"Idle Animation");
		if(showIdle)
		{
			animManagerEnemy.idle.animation = (AnimationClip)EditorGUILayout.ObjectField("Animation",animManagerEnemy.idle.animation,typeof(AnimationClip),true);
			animManagerEnemy.idle.speedAnimation = EditorGUILayout.Slider("Speed Animation",animManagerEnemy.idle.speedAnimation,0,3);
		}
		
		showDeath = EditorGUILayout.Foldout(showDeath,"Death Animation");
		if(showDeath)
		{
			animManagerEnemy.death.animation = (AnimationClip)EditorGUILayout.ObjectField("Animation",animManagerEnemy.death.animation,typeof(AnimationClip),true);
			animManagerEnemy.death.speedAnimation = EditorGUILayout.Slider("Speed Animation",animManagerEnemy.death.speedAnimation,0,3);
		}
		showMove = EditorGUILayout.Foldout(showMove,"Move Animation");
		if(showMove)
		{
			animManagerEnemy.move.animation = (AnimationClip)EditorGUILayout.ObjectField("Animation",animManagerEnemy.move.animation,typeof(AnimationClip),true);
			animManagerEnemy.move.speedAnimation = EditorGUILayout.Slider("Speed Animation",animManagerEnemy.move.speedAnimation,0,3);
			animManagerEnemy.move.speedTuning = EditorGUILayout.Toggle("Speed Tuning",animManagerEnemy.move.speedTuning);
		}
		
		EditorGUILayout.Space();
		EditorGUILayout.Space();

		
		//Normal Atk 
		showNormalAtk = EditorGUILayout.Foldout(showNormalAtk,"Normal Attack Animation");
		EditorGUI.indentLevel++;
		if(showNormalAtk)
		{
			animManagerEnemy.sizeNAtk = EditorGUILayout.IntField("Normal Attack Size",animManagerEnemy.sizeNAtk);
			
			while(animManagerEnemy.sizeNAtk != animManagerEnemy.normalAttack.Count)
			{
				if(animManagerEnemy.sizeNAtk > animManagerEnemy.normalAttack.Count)
				{
					animManagerEnemy.normalAttack.Add(new AnimationManagerEnemy.AnimationNormalAttack());
					animManagerEnemy.showNormalAtkSize.Add(true);
				}
				else
				{
					animManagerEnemy.normalAttack.RemoveAt(animManagerEnemy.normalAttack.Count-1);
					animManagerEnemy.showNormalAtkSize.RemoveAt(animManagerEnemy.showNormalAtkSize.Count-1);
				}
			}
			
			for(int i = 0;i<animManagerEnemy.normalAttack.Count;i++)
			{
				animManagerEnemy.showNormalAtkSize[i] = EditorGUILayout.Foldout(animManagerEnemy.showNormalAtkSize[i],animManagerEnemy.normalAttack[i]._name);
				
				if(animManagerEnemy.showNormalAtkSize[i])
				{
					animManagerEnemy.normalAttack[i]._name = EditorGUILayout.TextField("Attack Name",animManagerEnemy.normalAttack[i]._name);
					animManagerEnemy.normalAttack[i].animation = (AnimationClip)EditorGUILayout.ObjectField("Animation",animManagerEnemy.normalAttack[i].animation,typeof(AnimationClip),true);
					animManagerEnemy.normalAttack[i].speedAnimation = EditorGUILayout.Slider("Speed Animation",animManagerEnemy.normalAttack[i].speedAnimation,0,3);
					animManagerEnemy.normalAttack[i].attackTimer = EditorGUILayout.Slider("Attack Timer",animManagerEnemy.normalAttack[i].attackTimer,0,0.99f);
					animManagerEnemy.normalAttack[i].multipleDamage = EditorGUILayout.Slider("Multiple Damage",animManagerEnemy.normalAttack[i].multipleDamage,0,10);
					animManagerEnemy.normalAttack[i].flinchValue = EditorGUILayout.Slider("Flinch Value",animManagerEnemy.normalAttack[i].flinchValue,0,100);
					animManagerEnemy.normalAttack[i].speedTuning = EditorGUILayout.Toggle("Speed Tuning",animManagerEnemy.normalAttack[i].speedTuning);
					animManagerEnemy.normalAttack[i].attackFX = (GameObject)EditorGUILayout.ObjectField("Attack Effect",animManagerEnemy.normalAttack[i].attackFX,typeof(GameObject),true);
					animManagerEnemy.normalAttack[i].soundFX = (AudioClip)EditorGUILayout.ObjectField("Sound Effect",animManagerEnemy.normalAttack[i].soundFX ,typeof(AudioClip),true);
				}
					
			}
			
		}
		EditorGUI.indentLevel--;
		EditorGUILayout.Space();
		EditorGUILayout.Space();
		
		//Critical Attack
		showCritAtk = EditorGUILayout.Foldout(showCritAtk,"Critical Attack Animation");
		EditorGUI.indentLevel++;
		if(showCritAtk)
		{
			animManagerEnemy.sizeCritAtk = EditorGUILayout.IntField("Critical Attack Size",animManagerEnemy.sizeCritAtk);
			
			while(animManagerEnemy.sizeCritAtk != animManagerEnemy.criticalAttack.Count)
			{
				if(animManagerEnemy.sizeCritAtk > animManagerEnemy.criticalAttack.Count)
				{
					animManagerEnemy.criticalAttack.Add(new AnimationManagerEnemy.AnimationCritAttack());
					animManagerEnemy.showCritSize.Add(true);
				}
				else
				{
					animManagerEnemy.criticalAttack.RemoveAt(animManagerEnemy.criticalAttack.Count-1);
					animManagerEnemy.showCritSize.RemoveAt(animManagerEnemy.showCritSize.Count-1);
				}
			}
			
			for(int i = 0;i<animManagerEnemy.criticalAttack.Count;i++)
			{
				animManagerEnemy.showCritSize[i] = EditorGUILayout.Foldout(animManagerEnemy.showCritSize[i],animManagerEnemy.criticalAttack[i]._name);
				
				if(animManagerEnemy.showCritSize[i])
				{
					animManagerEnemy.criticalAttack[i]._name = EditorGUILayout.TextField("Attack Name",animManagerEnemy.criticalAttack[i]._name);
					animManagerEnemy.criticalAttack[i].animation = (AnimationClip)EditorGUILayout.ObjectField("Animation",animManagerEnemy.criticalAttack[i].animation,typeof(AnimationClip),true);
					animManagerEnemy.criticalAttack[i].speedAnimation = EditorGUILayout.Slider("Speed Animation",animManagerEnemy.criticalAttack[i].speedAnimation,0,3);
					animManagerEnemy.criticalAttack[i].attackTimer = EditorGUILayout.Slider("Attack Timer",animManagerEnemy.criticalAttack[i].attackTimer,0,0.99f);
					animManagerEnemy.criticalAttack[i].multipleDamage = EditorGUILayout.Slider("Multiple Damage",animManagerEnemy.criticalAttack[i].multipleDamage,0,10);
					animManagerEnemy.criticalAttack[i].flinchValue = EditorGUILayout.Slider("Flinch Value",animManagerEnemy.criticalAttack[i].flinchValue,0,100);
					animManagerEnemy.criticalAttack[i].speedTuning = EditorGUILayout.Toggle("Speed Tuning",animManagerEnemy.criticalAttack[i].speedTuning);
					animManagerEnemy.criticalAttack[i].attackFX = (GameObject)EditorGUILayout.ObjectField("Attack Effect",animManagerEnemy.criticalAttack[i].attackFX,typeof(GameObject),true);
					animManagerEnemy.criticalAttack[i].soundFX = (AudioClip)EditorGUILayout.ObjectField("Sound Effect",animManagerEnemy.criticalAttack[i].soundFX ,typeof(AudioClip),true);
				}
					
			}
			
		}
		
		EditorGUILayout.Space();
		EditorGUILayout.Space();
		EditorGUI.indentLevel--;
		//Take Attack
		showTakeDmg = EditorGUILayout.Foldout(showCritAtk,"Take Attack Animation");
		EditorGUI.indentLevel++;
		if(showTakeDmg)
		{
			animManagerEnemy.sizeTakeDmg = EditorGUILayout.IntField("Take Attack Size",animManagerEnemy.sizeTakeDmg);
			
			while(animManagerEnemy.sizeTakeDmg != animManagerEnemy.takeAttack.Count)
			{
				if(animManagerEnemy.sizeTakeDmg > animManagerEnemy.takeAttack.Count)
				{
					animManagerEnemy.takeAttack.Add(new AnimationManagerEnemy.AnimationTakeAttack());
					animManagerEnemy.showTakeDmgSize.Add(true);
				}
				else
				{
					animManagerEnemy.takeAttack.RemoveAt(animManagerEnemy.takeAttack.Count-1);
					animManagerEnemy.showTakeDmgSize.RemoveAt(animManagerEnemy.showTakeDmgSize.Count-1);
				}
			}
			
			for(int i = 0;i<animManagerEnemy.takeAttack.Count;i++)
			{
				animManagerEnemy.showTakeDmgSize[i] = EditorGUILayout.Foldout(animManagerEnemy.showTakeDmgSize[i],animManagerEnemy.takeAttack[i]._name);
				
				if(animManagerEnemy.showTakeDmgSize[i])
				{
					animManagerEnemy.takeAttack[i]._name = EditorGUILayout.TextField("Take Attack Name",animManagerEnemy.takeAttack[i]._name);
					animManagerEnemy.takeAttack[i].animation = (AnimationClip)EditorGUILayout.ObjectField("Animation",animManagerEnemy.takeAttack[i].animation,typeof(AnimationClip),true);
					animManagerEnemy.takeAttack[i].speedAnimation = EditorGUILayout.Slider("Speed Animation",animManagerEnemy.takeAttack[i].speedAnimation,0,3);
				}
					
			}
			
		}
		
		if(GUI.changed)
			EditorUtility.SetDirty(animManagerEnemy);
	}
}

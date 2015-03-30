using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

[CustomEditor(typeof(AnimationManager))]
public class AnimationManagerEditor : Editor {
	
	private bool showIdle = true,showCast = true,showDeath = true,showMove = true,showNormalAtk = true,showCritAtk = true,showTakeDmg = true;
	
	public override void OnInspectorGUI()
	{	
		AnimationManager animManager = (AnimationManager)target;
		
		showIdle = EditorGUILayout.Foldout(showIdle,"Idle Animation");
		if(showIdle)
		{
			animManager.idle.animation = (AnimationClip)EditorGUILayout.ObjectField("Animation",animManager.idle.animation,typeof(AnimationClip),true);
			animManager.idle.speedAnimation = EditorGUILayout.Slider("Speed Animation",animManager.idle.speedAnimation,0,3);
		}
		showCast = EditorGUILayout.Foldout(showCast,"Cast Animation");
		if(showCast)
		{
			animManager.cast.animation = (AnimationClip)EditorGUILayout.ObjectField("Animation",animManager.cast.animation,typeof(AnimationClip),true);
			animManager.cast.speedAnimation = EditorGUILayout.Slider("Speed Animation",animManager.cast.speedAnimation,0,3);
		}
		showDeath = EditorGUILayout.Foldout(showDeath,"Death Animation");
		if(showDeath)
		{
			animManager.death.animation = (AnimationClip)EditorGUILayout.ObjectField("Animation",animManager.death.animation,typeof(AnimationClip),true);
			animManager.death.speedAnimation = EditorGUILayout.Slider("Speed Animation",animManager.death.speedAnimation,0,3);
		}
		showMove = EditorGUILayout.Foldout(showMove,"Move Animation");
		if(showMove)
		{
			animManager.move.animation = (AnimationClip)EditorGUILayout.ObjectField("Animation",animManager.move.animation,typeof(AnimationClip),true);
			animManager.move.speedAnimation = EditorGUILayout.Slider("Speed Animation",animManager.move.speedAnimation,0,3);
			animManager.move.speedTuning = EditorGUILayout.Toggle("Speed Tuning",animManager.move.speedTuning);
		}
		
		EditorGUILayout.Space();
		EditorGUILayout.Space();

		
		//Normal Atk 
		showNormalAtk = EditorGUILayout.Foldout(showNormalAtk,"Normal Attack Animation");
		EditorGUI.indentLevel++;
		if(showNormalAtk)
		{
			animManager.sizeNAtk = EditorGUILayout.IntField("Normal Attack Size",animManager.sizeNAtk);
			
			while(animManager.sizeNAtk != animManager.normalAttack.Count)
			{
				if(animManager.sizeNAtk > animManager.normalAttack.Count)
				{
					animManager.normalAttack.Add(new AnimationManager.AnimationNormalAttack());
					animManager.showNormalAtkSize.Add(true);
				}
				else
				{
					animManager.normalAttack.RemoveAt(animManager.normalAttack.Count-1);
					animManager.showNormalAtkSize.RemoveAt(animManager.showNormalAtkSize.Count-1);
				}
			}
			
			for(int i = 0;i<animManager.normalAttack.Count;i++)
			{
				animManager.showNormalAtkSize[i] = EditorGUILayout.Foldout(animManager.showNormalAtkSize[i],animManager.normalAttack[i]._name);
				
				if(animManager.showNormalAtkSize[i])
				{
					animManager.normalAttack[i]._name = EditorGUILayout.TextField("Attack Name",animManager.normalAttack[i]._name);
					animManager.normalAttack[i].animation = (AnimationClip)EditorGUILayout.ObjectField("Animation",animManager.normalAttack[i].animation,typeof(AnimationClip),true);
					animManager.normalAttack[i].speedAnimation = EditorGUILayout.Slider("Speed Animation",animManager.normalAttack[i].speedAnimation,0,3);
					animManager.normalAttack[i].attackTimer = EditorGUILayout.Slider("Attack Timer",animManager.normalAttack[i].attackTimer,0,0.99f);
					animManager.normalAttack[i].multipleDamage = EditorGUILayout.Slider("Multiple Damage",animManager.normalAttack[i].multipleDamage,0,10);
					animManager.normalAttack[i].flichValue = EditorGUILayout.Slider("Flinch Value",animManager.normalAttack[i].flichValue,0,100);
					animManager.normalAttack[i].speedTuning = EditorGUILayout.Toggle("Speed Tuning",animManager.normalAttack[i].speedTuning);
					animManager.normalAttack[i].attackFX = (GameObject)EditorGUILayout.ObjectField("Attack Effect",animManager.normalAttack[i].attackFX,typeof(GameObject),true);
					animManager.normalAttack[i].soundFX = (AudioClip)EditorGUILayout.ObjectField("Sound Effect",animManager.normalAttack[i].soundFX ,typeof(AudioClip),true);
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
			animManager.sizeCritAtk = EditorGUILayout.IntField("Critical Attack Size",animManager.sizeCritAtk);
			
			while(animManager.sizeCritAtk != animManager.criticalAttack.Count)
			{
				if(animManager.sizeCritAtk > animManager.criticalAttack.Count)
				{
					animManager.criticalAttack.Add(new AnimationManager.AnimationCritAttack());
					animManager.showCritSize.Add(true);
				}
				else
				{
					animManager.criticalAttack.RemoveAt(animManager.criticalAttack.Count-1);
					animManager.showCritSize.RemoveAt(animManager.showCritSize.Count-1);
				}
			}
			
			for(int i = 0;i<animManager.criticalAttack.Count;i++)
			{
				animManager.showCritSize[i] = EditorGUILayout.Foldout(animManager.showCritSize[i],animManager.criticalAttack[i]._name);
				
				if(animManager.showCritSize[i])
				{
					animManager.criticalAttack[i]._name = EditorGUILayout.TextField("Attack Name",animManager.criticalAttack[i]._name);
					animManager.criticalAttack[i].animation = (AnimationClip)EditorGUILayout.ObjectField("Animation",animManager.criticalAttack[i].animation,typeof(AnimationClip),true);
					animManager.criticalAttack[i].speedAnimation = EditorGUILayout.Slider("Speed Animation",animManager.criticalAttack[i].speedAnimation,0,3);
					animManager.criticalAttack[i].attackTimer = EditorGUILayout.Slider("Attack Timer",animManager.criticalAttack[i].attackTimer,0,0.99f);
					animManager.criticalAttack[i].multipleDamage = EditorGUILayout.Slider("Multiple Damage",animManager.criticalAttack[i].multipleDamage,0,10);
					animManager.criticalAttack[i].flichValue = EditorGUILayout.Slider("Flinch Value",animManager.criticalAttack[i].flichValue,0,100);
					animManager.criticalAttack[i].speedTuning = EditorGUILayout.Toggle("Speed Tuning",animManager.criticalAttack[i].speedTuning);
					animManager.criticalAttack[i].attackFX = (GameObject)EditorGUILayout.ObjectField("Attack Effect",animManager.criticalAttack[i].attackFX,typeof(GameObject),true);
					animManager.criticalAttack[i].soundFX = (AudioClip)EditorGUILayout.ObjectField("Sound Effect",animManager.criticalAttack[i].soundFX ,typeof(AudioClip),true);
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
			animManager.sizeTakeDmg = EditorGUILayout.IntField("Take Attack Size",animManager.sizeTakeDmg);
			
			while(animManager.sizeTakeDmg != animManager.takeAttack.Count)
			{
				if(animManager.sizeTakeDmg > animManager.takeAttack.Count)
				{
					animManager.takeAttack.Add(new AnimationManager.AnimationTakeAttack());
					animManager.showTakeDmgSize.Add(true);
				}
				else
				{
					animManager.takeAttack.RemoveAt(animManager.takeAttack.Count-1);
					animManager.showTakeDmgSize.RemoveAt(animManager.showTakeDmgSize.Count-1);
				}
			}
			
			for(int i = 0;i<animManager.takeAttack.Count;i++)
			{
				animManager.showTakeDmgSize[i] = EditorGUILayout.Foldout(animManager.showTakeDmgSize[i],animManager.takeAttack[i]._name);
				
				if(animManager.showTakeDmgSize[i])
				{
					animManager.takeAttack[i]._name = EditorGUILayout.TextField("Take Attack Name",animManager.takeAttack[i]._name);
					animManager.takeAttack[i].animation = (AnimationClip)EditorGUILayout.ObjectField("Animation",animManager.takeAttack[i].animation,typeof(AnimationClip),true);
					animManager.takeAttack[i].speedAnimation = EditorGUILayout.Slider("Speed Animation",animManager.takeAttack[i].speedAnimation,0,3);
				}
					
			}
			
		}
		
		if(GUI.changed)
			EditorUtility.SetDirty(animManager);
	}
}

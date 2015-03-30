using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

[CustomEditor(typeof(EnemyController))]
public class EnemyControllerEditor : Editor {
	
	EnemyController controller;
	
 	 public override void OnInspectorGUI()
	{
		
		controller = (EnemyController)target;
		
		controller.behavior = (EnemyController.EnemyBehavior)EditorGUILayout.EnumPopup("Behavior",controller.behavior);
		controller.nature = (EnemyController.EnemyNature)EditorGUILayout.EnumPopup("Nature",controller.nature);
		
		EditorGUILayout.Space();
		
		controller.sizeMesh = EditorGUILayout.IntField("Size Mesh",controller.sizeMesh);
			
			while(controller.sizeMesh != controller.modelMesh.Count)
			{
				if(controller.sizeMesh > controller.modelMesh.Count)
				{
					controller.modelMesh.Add(null);
				}
				else
				{
					controller.modelMesh.RemoveAt(controller.modelMesh.Count-1);
				}
			}
		EditorGUI.indentLevel++;
		
		for(int i=0;i<controller.modelMesh.Count;i++)
		{
			controller.modelMesh[i] = (GameObject)EditorGUILayout.ObjectField("Model Mesh " + (i+1).ToString(),controller.modelMesh[i],typeof(GameObject),true);
		}
		
		EditorGUI.indentLevel--;
		
		EditorGUILayout.Space();
		
		controller.colorTakeDamage = EditorGUILayout.ColorField("Take Attack Color",controller.colorTakeDamage);
		
		EditorGUILayout.Space();
		
		//controller.DeadSpawnPoint = (GameObject)EditorGUILayout.ObjectField("Re Spawn Point",controller.DeadSpawnPoint,typeof(GameObject),true);
		
		controller.deadTimer = EditorGUILayout.FloatField("Dead Timer",controller.deadTimer);
		controller.deadTransperent = EditorGUILayout.Toggle("Dead Transparent",controller.deadTransperent);
		if(controller.deadTransperent)
		controller.speedFade = EditorGUILayout.FloatField("Speed Transparent",controller.speedFade);
		
		EditorGUILayout.Space();
		
		controller.regenHP = EditorGUILayout.Toggle("Regen HP",controller.regenHP);
		controller.regenMP = EditorGUILayout.Toggle("Regen MP",controller.regenMP);
		
		EditorGUILayout.Space();
		
		if(controller.behavior == EnemyController.EnemyBehavior.MoveAround)
		controller.movePhase = EditorGUILayout.FloatField("Move Phase",controller.movePhase);
		controller.returnPhase = EditorGUILayout.FloatField("Return Phase",controller.returnPhase);
		
		if(controller.behavior == EnemyController.EnemyBehavior.MoveAround)
		{
			controller.delayNextTargetMin = EditorGUILayout.FloatField("Delay Next Target Min",controller.delayNextTargetMin);
			controller.delayNextTargetMax = EditorGUILayout.FloatField("Delay Next Target Max",controller.delayNextTargetMax);
		}

		if(GUI.changed)
			EditorUtility.SetDirty(controller);
	}
	
	
 
	    
	
}

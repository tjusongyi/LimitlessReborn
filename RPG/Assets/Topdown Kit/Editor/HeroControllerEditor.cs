using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

[CustomEditor(typeof(HeroController))]
public class HeroControllerEditor : Editor {
	
	public override void OnInspectorGUI(){
		
		HeroController controller = (HeroController)target;
		
		controller.classType = (ClassType)EditorGUILayout.EnumPopup("Class",controller.classType);
		
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
			controller.modelMesh[i] = (GameObject)EditorGUILayout.ObjectField("Model Mesh" + (i+1).ToString(),controller.modelMesh[i],typeof(GameObject),true);
		}
		
		EditorGUI.indentLevel--;
		
		EditorGUILayout.Space();
		
		
		controller.colorTakeDamage = EditorGUILayout.ColorField("Take Attack Color",controller.colorTakeDamage);
		
		EditorGUILayout.Space();
		
		controller.autoAttack = EditorGUILayout.Toggle("Auto Attack",controller.autoAttack);
		
		EditorGUILayout.Space();
		
		if(GUI.changed)
			EditorUtility.SetDirty(controller);
 
	    
	}
}

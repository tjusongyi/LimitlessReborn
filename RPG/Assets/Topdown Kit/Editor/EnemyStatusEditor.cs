using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(EnemyStatus))]
public class EnemyStatusEditor : Editor {
	
	private bool showStatus = true;
	
	public override void OnInspectorGUI(){
		
		
		EnemyStatus enemyStatus = (EnemyStatus)target;

		GUI.changed = false;
		
		enemyStatus.enemyName = EditorGUILayout.TextField("Enemy Name",enemyStatus.enemyName);
	
		EditorGUILayout.Space();
		EditorGUI.indentLevel--;

		
		EditorGUILayout.Space();
		showStatus = EditorGUILayout.Foldout(showStatus,"Status");
		if(showStatus)
		{
			enemyStatus.status.hp = (int)EditorGUILayout.Slider("HP",enemyStatus.status.hp,1,9999);
			ProgressBar(enemyStatus.status.hp/9999f,"HP");
			enemyStatus.status.mp = (int)EditorGUILayout.Slider("MP",enemyStatus.status.mp,1,9999);
			ProgressBar(enemyStatus.status.mp/9999f,"MP");
			enemyStatus.status.atk = (int)EditorGUILayout.Slider("Attack",enemyStatus.status.atk,1,999);
			ProgressBar(enemyStatus.status.atk/999f,"Attack");
			enemyStatus.status.def = (int)EditorGUILayout.Slider("Defend",enemyStatus.status.def,1,999);
			ProgressBar(enemyStatus.status.def/999f,"Defend");
			enemyStatus.status.spd = (int)EditorGUILayout.Slider("Speed",enemyStatus.status.spd,1,999);
			ProgressBar(enemyStatus.status.spd/999f,"Speed");
			enemyStatus.status.hit = (int)EditorGUILayout.Slider("Hit",enemyStatus.status.hit,1,999);
			ProgressBar(enemyStatus.status.hit/999f,"Hit");
			enemyStatus.status.criticalRate = EditorGUILayout.Slider("Critical Rate",enemyStatus.status.criticalRate,1,100);
			ProgressBar(enemyStatus.status.criticalRate/100f,"Critical Rate");
			enemyStatus.status.atkSpd = EditorGUILayout.Slider("Attack Speed",enemyStatus.status.atkSpd,1,300);
			ProgressBar(enemyStatus.status.atkSpd/300f,"Attack Speed");
			enemyStatus.status.atkRange = EditorGUILayout.Slider("Attack Range",enemyStatus.status.atkRange,0.5f,10);
			ProgressBar(enemyStatus.status.atkRange/10f,"Attack Range");
			enemyStatus.status.movespd = EditorGUILayout.Slider("Movement Speed",enemyStatus.status.movespd,1,10);
			ProgressBar(enemyStatus.status.movespd/10f,"Move Speed");
		}
		
		EditorGUILayout.Space();
		enemyStatus.expGive = EditorGUILayout.IntField("Exp Give",enemyStatus.expGive);
		
		
		if(GUI.changed)
			EditorUtility.SetDirty(enemyStatus);
	}
	
	
	public void ProgressBar (float val,string label) {
		Rect rect = GUILayoutUtility.GetRect (18, 18, "TextField");
		EditorGUI.ProgressBar (rect, val, label);
		EditorGUILayout.Space ();
	}
}

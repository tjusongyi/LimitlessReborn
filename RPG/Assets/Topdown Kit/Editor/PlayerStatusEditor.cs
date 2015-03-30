using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(PlayerStatus))]
public class PlayerStatusEditor : Editor {
	
	private bool showStatus = true;
	private bool showStatusGrowth = true;
	private bool showExpCal = false;
	private bool showSummaryStatus = true;
	private bool oneCalExp = false;
	private float baseExpMax;
	
	public override void OnInspectorGUI(){
		
		
		PlayerStatus playerStatus = (PlayerStatus)target;
		
		if(!oneCalExp)
		{
			//playerStatus.expMax = playerStatus.startExp;
			baseExpMax = playerStatus.startExp;
			oneCalExp = true;	
			
		}
		
		
		
		//GUI.changed = false;
		
		playerStatus.playerName = EditorGUILayout.TextField("Player Name",playerStatus.playerName);
		
		EditorGUILayout.Space();
		
		playerStatus.status.lv = EditorGUILayout.IntField("Level",playerStatus.status.lv);
		playerStatus.startExp = EditorGUILayout.IntField("Start EXP",playerStatus.startExp);
		playerStatus.multipleExp = EditorGUILayout.Slider("Multiple Exp",playerStatus.multipleExp,1f,10);
		playerStatus.pointPerLv = EditorGUILayout.IntField("Point Per Level",playerStatus.pointPerLv);
		playerStatus.maxLv = EditorGUILayout.IntField("Max Level",playerStatus.maxLv);
		showExpCal = EditorGUILayout.Foldout(showExpCal,"Exp Calculator");
		
		if(showExpCal)
		{
			EditorGUI.indentLevel++;
			if(playerStatus.maxLv > 0)
			{
				EditorGUILayout.LabelField("Level", "Exp Use");
				EditorGUI.indentLevel++;
				for(int i = 0;i<=playerStatus.maxLv;i++)
				{
					EditorGUILayout.LabelField((1 + i).ToString(), (baseExpMax *(playerStatus.multipleExp) * i).ToString());
				}
			}
		}
		
		EditorGUILayout.Space();
		EditorGUI.indentLevel--;
		EditorGUI.indentLevel--;
		EditorGUI.indentLevel--;
		
		EditorGUILayout.Space();
		showStatus = EditorGUILayout.Foldout(showStatus,"Status");
		if(showStatus)
		{
			playerStatus.status.hp = (int)EditorGUILayout.Slider("HP",playerStatus.status.hp,1,9999);
			ProgressBar(playerStatus.status.hp/9999f,"HP");
			playerStatus.status.mp = (int)EditorGUILayout.Slider("MP",playerStatus.status.mp,1,9999);
			ProgressBar(playerStatus.status.mp/9999f,"MP");
			playerStatus.status.atk = (int)EditorGUILayout.Slider("Attack",playerStatus.status.atk,1,99);
			ProgressBar(playerStatus.status.atk/99f,"Attack");
			playerStatus.status.def = (int)EditorGUILayout.Slider("Defend",playerStatus.status.def,1,99);
			ProgressBar(playerStatus.status.def/99f,"Defend");
			playerStatus.status.spd = (int)EditorGUILayout.Slider("Speed",playerStatus.status.spd,1,99);
			ProgressBar(playerStatus.status.spd/99f,"Speed");
			playerStatus.status.hit = (int)EditorGUILayout.Slider("Hit",playerStatus.status.hit,1,99);
			ProgressBar(playerStatus.status.hit/99f,"Hit");
			playerStatus.status.criticalRate = EditorGUILayout.Slider("Critical Rate",playerStatus.status.criticalRate,1,100);
			ProgressBar(playerStatus.status.criticalRate/100f,"Critical Rate");
			playerStatus.status.atkSpd = EditorGUILayout.Slider("Attack Speed",playerStatus.status.atkSpd,1,300);
			ProgressBar(playerStatus.status.atkSpd/300f,"Attack Speed");
			playerStatus.status.atkRange = EditorGUILayout.Slider("Attack Range",playerStatus.status.atkRange,0.5f,10);
			ProgressBar(playerStatus.status.atkRange/10f,"Attack Range");
			playerStatus.status.movespd = EditorGUILayout.Slider("Movement Speed",playerStatus.status.movespd,1,10);
			ProgressBar(playerStatus.status.movespd/10f,"Move Speed");
		}
		
		playerStatus.hpRegenTime = EditorGUILayout.FloatField("HP Regen Timer",playerStatus.hpRegenTime);
		playerStatus.mpRegenTime = EditorGUILayout.FloatField("MP Regen Timer",playerStatus.mpRegenTime);
		
		EditorGUILayout.Space();
		showStatusGrowth = EditorGUILayout.Foldout(showStatusGrowth,"Status Growth Per Level");
		if(showStatusGrowth)
		{
			playerStatus.growthSetting.hp = EditorGUILayout.IntField("HP",playerStatus.growthSetting.hp);
			playerStatus.growthSetting.mp = EditorGUILayout.IntField("MP",playerStatus.growthSetting.mp);
			playerStatus.growthSetting.atk = EditorGUILayout.IntField("Attack",playerStatus.growthSetting.atk);
			playerStatus.growthSetting.def = EditorGUILayout.IntField("Defend",playerStatus.growthSetting.def);
			playerStatus.growthSetting.spd = EditorGUILayout.IntField("Speed",playerStatus.growthSetting.spd);
			playerStatus.growthSetting.hit = EditorGUILayout.IntField("Hit",playerStatus.growthSetting.hit);
			playerStatus.growthSetting.criticalRate = EditorGUILayout.FloatField("CriticalRate",playerStatus.growthSetting.criticalRate);
			playerStatus.growthSetting.atkSpd = EditorGUILayout.FloatField("Attack Speed",playerStatus.growthSetting.atkSpd);
			playerStatus.growthSetting.atkRange = EditorGUILayout.FloatField("Attack Range",playerStatus.growthSetting.atkRange);
			playerStatus.growthSetting.movespd = EditorGUILayout.FloatField("Move Speed",playerStatus.growthSetting.movespd);
			
		}
		
		EditorGUILayout.Space();
		showSummaryStatus = EditorGUILayout.Foldout(showSummaryStatus,"Summary Status");
		EditorGUI.indentLevel++;
		EditorGUI.indentLevel++;
		
		if(showSummaryStatus)
		{
			EditorGUILayout.LabelField("Name :", playerStatus.playerName.ToString());
			EditorGUILayout.LabelField("Level :", playerStatus.status.lv.ToString());
			EditorGUILayout.LabelField("HP :", playerStatus.status.hp.ToString() + " + " + (playerStatus.statusAdd.hp + playerStatus.statusGrowth.hp));
			EditorGUILayout.LabelField("MP :", playerStatus.status.mp.ToString() + " + " + (playerStatus.statusAdd.mp + playerStatus.statusGrowth.mp));
			EditorGUILayout.LabelField("Attack :", playerStatus.status.atk.ToString() + " + " + (playerStatus.statusAdd.atk + playerStatus.statusGrowth.atk));
			EditorGUILayout.LabelField("Defend :", playerStatus.status.def.ToString() + " + " + (playerStatus.statusAdd.def + playerStatus.statusGrowth.def));
			EditorGUILayout.LabelField("Speed :", playerStatus.status.spd.ToString() + " + " + (playerStatus.statusAdd.spd + playerStatus.statusGrowth.spd));
			EditorGUILayout.LabelField("Hit :", playerStatus.status.hit.ToString() + " + " + (playerStatus.statusAdd.hit + playerStatus.statusGrowth.hit));
			EditorGUILayout.LabelField("Critical Rate :", playerStatus.status.criticalRate.ToString() + " + " + (playerStatus.statusAdd.criticalRate + playerStatus.statusGrowth.criticalRate));
			EditorGUILayout.LabelField("Attack Speed :", playerStatus.status.atkSpd.ToString() + " + " + (playerStatus.statusAdd.atkSpd + playerStatus.statusGrowth.atkSpd));
			EditorGUILayout.LabelField("Attack Range :", playerStatus.status.atkRange.ToString() + " + " + (playerStatus.statusAdd.atkRange + playerStatus.statusGrowth.atkRange));
			EditorGUILayout.LabelField("Move Speed :", playerStatus.status.movespd.ToString() + " + " + (playerStatus.statusAdd.movespd + playerStatus.statusGrowth.movespd));
			
		}
		
		
		
		
		
		if(GUI.changed)
			EditorUtility.SetDirty(playerStatus);
	}
	
	
	public void ProgressBar (float val,string label) {
		Rect rect = GUILayoutUtility.GetRect (18, 18, "TextField");
		EditorGUI.ProgressBar (rect, val, label);
		EditorGUILayout.Space ();
	}
}

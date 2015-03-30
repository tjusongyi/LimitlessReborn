using UnityEngine;
using UnityEditor;
using System;

public class StatisticIncreaseUtils
{
	public static void DisplayForm(StatisticIncrease increase, bool isAttribute)
	{
		EditorUtils.Separator();
		
		increase.BaseValue = EditorUtils.IntField(increase.BaseValue, "Base value", 50, FieldTypeEnum.BeginningOnly);
		
		increase.Maximum = EditorUtils.IntField(increase.Maximum, "Maximum value", 50, FieldTypeEnum.EndOnly);
		
		EditorUtils.Separator();
		
		if (isAttribute)
		{
			EditorUtils.Label("How much one point of attribute will increase in-game statistic.");
		}
		else
		{
			EditorUtils.Label("How much one point of skill will increase in-game statistic.");
		}
		
		
		EditorUtils.Label("Melee");
		
		increase.MeleeDmg = EditorUtils.FloatField(increase.MeleeDmg,"Damage");
		
		increase.MeleeAccuracy = EditorUtils.FloatField(increase.MeleeAccuracy, "Accuracy");
		
		increase.MeleeAvoidance = EditorUtils.FloatField(increase.MeleeAvoidance ,"Avoidance");
		
		increase.MeleeCrit = EditorUtils.FloatField(increase.MeleeCrit ,"Critical chance");
		
		EditorUtils.Separator();
		
		/* range */
		EditorUtils.Label("Range");
	
		
		increase.RangeDmg = EditorUtils.FloatField(increase.RangeDmg ,"Damage");
		
		increase.RangeAccuracy = EditorUtils.FloatField(increase.RangeAccuracy ,"Accuracy");
		
		increase.RangeAvoidance = EditorUtils.FloatField(increase.RangeAvoidance ,"Avoidance");
		
		increase.RangeCrit = EditorUtils.FloatField(increase.RangeCrit ,"Critical chance");
		
		EditorUtils.Separator();
		/* magic */
		EditorUtils.Label("Magic");
		
		increase.MagicDmg = EditorUtils.FloatField(increase.MagicDmg ,"Damage");
		
		increase.MagicAccuracy = EditorUtils.FloatField(increase.MagicAccuracy ,"Accuracy");
		
		increase.MagicResistance = EditorUtils.FloatField(increase.MagicResistance ,"Resistance");
		
		increase.MagicCrit = EditorUtils.FloatField(increase.MagicCrit ,"Critical chance");
		
		EditorUtils.Separator();
		/* hit point */
		increase.HitPoint = EditorUtils.IntField(increase.HitPoint ,"HP");
		
		increase.HitPointRegen = EditorUtils.FloatField(increase.HitPointRegen ,"HP regeneration");
		
		EditorUtils.Separator();
		/* mana */
		increase.Mana = EditorUtils.IntField(increase.Mana ,"Mana");
		
		increase.ManaRegen = EditorUtils.FloatField(increase.ManaRegen ,"Mana regeneration");
		
		EditorUtils.Separator();
		/* vitality */
		increase.Vitality = EditorUtils.IntField(increase.Vitality ,"Vitality");
		
		increase.VitalityRegen = EditorUtils.FloatField(increase.VitalityRegen, "Vitality regen");
		
		increase.Armor = EditorUtils.FloatField(increase.Armor, "Armor");
		
		increase.MovementSpeed = EditorUtils.FloatField(increase.MovementSpeed, "Speed");
	}
}

using UnityEngine;
using System.Collections.Generic;
using UnityEditor;
using System;

public class EffectUtils 
{
	public static void EffectsEditor(List<Effect> effects, MainWindowEditor Data, EffectTypeUsage effectType)
	{
		if (GUILayout.Button("Add effect", GUILayout.Width(250)))
		{
			effects.Add(new Effect());
		}

        if (effects == null)
            return;
		
		foreach(Effect effect in effects)
		{
			AddEffect(effect, Data, effectType);
			
			if (GUILayout.Button("Delete", GUILayout.Width(150)))
			{
				effects.Remove(effect);
				break;
			}
			EditorGUILayout.EndHorizontal();
			EditorUtils.Separator();
		}
		
		EditorUtils.Separator();
	}
	
	public static void AddEffect(Effect effect, MainWindowEditor Data , EffectTypeUsage effectType)
	{
		EditorUtils.Label("Duration -1 is infinite/permanent effect");
		EditorUtils.Label("Duration 0 is once time effect");
        EditorUtils.Label("");
        EditorUtils.Label("For damage effect you must use negative numbers");

        string label = "Effect type " + effect.EffectType.ToString() + " - ";
        switch (effect.EffectType)
        { 
            case EffectTypeEnum.Attribute:
                label += " change value of some attribute";
                break;
            case EffectTypeEnum.CastSpell:
                label += " cast spell (NOT IMPLEMENTED)";
                break;
            case EffectTypeEnum.HitPoint:
                label += " increase bonus hit points";
                break;
            case EffectTypeEnum.HitPointChange:
                label += " change hit points of target heal or damage";
                break;
            case EffectTypeEnum.HitPointRegen:
                label += " change hit point regeneration of target heal over item / damage over item";
                break;
            case EffectTypeEnum.Mana:
                label += " increase bonus mana point";
                break;
            case EffectTypeEnum.ManaPointChange:
                label += " increase current mana";
                break;
            case EffectTypeEnum.ManaRegen:
                label += " change current mana rengeration of target";
                break;
            case EffectTypeEnum.Skill:
                label += " change value of some skill";
                break;
        }

		EditorGUILayout.Separator();
		EditorGUILayout.BeginHorizontal();
		
		EditorGUILayout.PrefixLabel("Target");
		effect.Target = (TargetType)EditorGUILayout.EnumPopup(effect.Target ,GUILayout.Width(200));
		
		EditorGUILayout.PrefixLabel("Effect type");
		effect.EffectType = (EffectTypeEnum)EditorGUILayout.EnumPopup(effect.EffectType ,GUILayout.Width(200));
        if (effectType == EffectTypeUsage.Equiped || effectType == EffectTypeUsage.RaceBonus || effectType == EffectTypeUsage.EffectArea)
        {
            effect.Target = TargetType.Self;
        }

        if (effectType == EffectTypeUsage.RaceBonus)
        {
            if (effect.EffectType == EffectTypeEnum.HitPointChange)
            {
                effect.EffectType = EffectTypeEnum.HitPoint;
            }

            if (effect.EffectType == EffectTypeEnum.ManaPointChange)
            {
                effect.EffectType = EffectTypeEnum.Mana;
            }
        }
		if (effect.Target == TargetType.Other)
			effect.EffectType = EffectTypeEnum.HitPointChange;
		
		EditorGUILayout.EndHorizontal();
        EditorUtils.Label(label);
		EditorGUILayout.Separator();
		EditorGUILayout.BeginHorizontal();
		
		
		EditorGUILayout.PrefixLabel("Value");
		effect.Value = EditorGUILayout.IntField(effect.Value ,GUILayout.Width(50));
		
		
		EditorGUILayout.PrefixLabel("Duration");
		effect.Duration = EditorGUILayout.IntField(effect.Duration ,GUILayout.Width(50));
        
		
		switch(effect.EffectType)
		{
			case EffectTypeEnum.Attribute:
				effect.ID = EditorUtils.IntPopup(effect.ID, Data.attributeEditor.items, "Attribute", 100, FieldTypeEnum.Middle, false);
				break;
			
			case EffectTypeEnum.Skill:
				effect.ID = EditorUtils.IntPopup(effect.ID, Data.skillEditor.items, "Skill", 100, FieldTypeEnum.Middle, false);
				break;
			
			case EffectTypeEnum.CastSpell:
				effect.ID = EditorUtils.IntPopup(effect.ID, Data.spellEditor.items, "Spell", 100, FieldTypeEnum.Middle, false);
				break;
		}
		
	}
}

using UnityEngine;
using System.Collections;

public class GeneratorConfig
{
	public static int Frequency = 10;
	public static int MaximumLevel = 90;
	public static bool UseNames = true;
	public static bool FirstItemUseCustomName = true;
	
	public static bool Price = true;
	public static IncresingTypeEnum ValueType = IncresingTypeEnum.Linear;
	public static int IncreasingPrice = 100;
	
	public static bool Effects = true;
	public static IncresingTypeEnum EffectType = IncresingTypeEnum.Linear;
	public static int IncreasingEffect = 100;
	
	public static string[] Words = { "Damaged", "Weak", "Common", "Normal", "Good", "Quality", "Brilliant", "Superior", "Excellent" };
	public static string[] WordsMask = { "{0} {1}", "{0} {1}", "{0} {1}", "{0} {1}", "{0} {1}", "{0} {1}", "{0} {1}", "{0} {1}", "{0} {1}" };
	
	public static bool WeaponDamage = true;
	public static IncresingTypeEnum WeaponIncreasingDamageType = IncresingTypeEnum.Linear;
	public static int WeaponAmount = 100;
	
	public static bool ArmorClassValue = true;
	public static IncresingTypeEnum ArmorIncreasingDamageType = IncresingTypeEnum.Linear;
	public static int ArmorAmount = 100;
}

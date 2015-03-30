using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Xml;

public class RPGAttribute : StatisticIncrease
{
	public List<AttributeFormula> Skills;
	
	public RPGAttribute()
	{
		preffix = "ATTRIBUTE";
	}
}

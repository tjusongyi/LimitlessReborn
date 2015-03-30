using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class RPGCharacterRace : BasicItem 
{
	public List<Effect> Effects;
	
	public RPGCharacterRace()
	{
        Name = string.Empty;
        Description = string.Empty;
        preffix = "RACE";
		Effects = new List<Effect>();
	}
}

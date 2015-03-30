using UnityEngine;
using System.Collections;

public class RPGEquipmentSlot : BasicItem  
{
	public RPGEquipmentSlot()
	{
		Name = string.Empty;
		Description = string.Empty;
		SystemDescription = string.Empty;
        preffix = "EQSLOT";
	}
	
    //position for character customization
	public int PosX;
	public int PosY;
}

using UnityEngine;
using System.Collections;

public class HotBarGUI : BasicGUI 
{
	bool isItemActive;
	HotBarItem hotBarItem;
	int currentIndex;
	
	void Start()
	{
		Prepare();
	}
	
	void Update()
	{
		if (BasicGUI.isContainerDisplayed || BasicGUI.isMainMenuDisplayed)
			return;
		
		if (player.scene.SceneType == SceneTypeEnum.MainMenu)
			return;
		
		//button actions
		if (Input.GetKey(KeyCode.Alpha1))
			DoAction(0);
		if (Input.GetKey(KeyCode.Alpha2))
			DoAction(1);
		if (Input.GetKey(KeyCode.Alpha3))
			DoAction(2);
		if (Input.GetKey(KeyCode.Alpha4))
			DoAction(3);
		if (Input.GetKey(KeyCode.Alpha5))
			DoAction(4);
		
		if (Input.GetKey(KeyCode.Alpha6))
			DoAction(5);
		if (Input.GetKey(KeyCode.Alpha7))
			DoAction(6);
		if (Input.GetKey(KeyCode.Alpha8))
			DoAction(7);
		if (Input.GetKey(KeyCode.Alpha9))
			DoAction(8);
		if (Input.GetKey(KeyCode.Alpha0))
			DoAction(9);
	}
	
	void OnGUI()
	{
        if (player.scene.SceneType == SceneTypeEnum.MainMenu || BasicGUI.isMainMenuDisplayed)
            return;

        DisplayInfoMessage();

		if (BasicGUI.isContainerDisplayed)
			return;
		
		Box(PosX-5, PosY-5, HotbarWidth, HotbarHeight,"");
		
		int ypos = PosY + GUIItemIndent;
		for(int x = 0; x <= player.Hero.Bar.Size -1; x++)
		{
			int xpos = PosX + GUIItemIndent + (x * (GUIItemButon + GUIItemIndent));
			Rect r = new Rect(xpos,ypos, GUIItemButon, GUIItemButon);
			
			HotBarItem item = player.Hero.Bar.GetByPosition(x);
			if (!string.IsNullOrEmpty(item.Usable.IconPath))
			{
				if (item.Usable.Icon == null)
					item.Usable.Icon  = (Texture2D)Resources.Load(item.Usable.IconPath, typeof(Texture2D)); 
				if (Button(r, item.Usable.Icon, "TOOLTIPICON"))
				{
					if (Event.current.button == 0)
					{
						DoAction(x);							
					}
					else if (Event.current.button == 1)
					{
						player.Hero.Bar.RemoveUsable(x);
					}
				}
			}
			else
			{
				int displayNumber = x + 1;
				if (displayNumber == 10)
					displayNumber = 0;
				if (Button(r, displayNumber.ToString()) && Player.currentItem != null)
				{
					//add item to hotbar
					if (Player.currentItem.IsUsable)
					{
						player.Hero.Bar.EquipUsable(Player.currentItem, x);
						Player.currentItem = null;
					}
				}
			}
		}
	}

    void DisplayInfoMessage()
    {
        if (playerController.selectedObject == null || !IsAllWindowsClosed)
            return;

        float distance = Vector3.Distance(transform.position, playerController.selectedObject.transform.position);
        string message = playerController.selectedObject.DisplayCloseInfo(player);
        
        

        if (distance > playerController.selectedObject.GetActivateRange(player))
        {
            message = playerController.selectedObject.DisplayInfo(player);
        }
        message = message.Trim();
        if (string.IsNullOrEmpty(message))
            return;

        switch (playerController.selectedObject.InfoPosition())
        {
            case InfoPositionEnum.MiddleDown:
                Box(PosX - 5, PosY - 100, HotbarWidth, HotbarHeight, "");

                Label(PosX + 20, PosY - 80, HotbarWidth - 40, 30, message, Color.black);
                break;
        }
    }

	void DoAction(int index)
	{
		HotBarItem item = player.Hero.Bar.GetByPosition(index);
		currentIndex = index;
		if (item.Index == -1)
			return;
		//check if you can act action
		if (!item.Usable.IsUsable || !item.Usable.CheckRequirements(player))
			return;
		
		//check delay for item	
		switch(item.Usable.UsageSkill)
		{
			case UsageSkillType.Combat: 
				if (player.Hero.AttackDelay > item.Usable.Recharge)
					player.Hero.AttackDelay = 0;
				else
					return;
				break;
			case UsageSkillType.Item: 
				if (player.Hero.UseItemDelay > item.Usable.Recharge)
					player.Hero.UseItemDelay = 0;
				else
					return;
				break;
			case UsageSkillType.Spell: 
				if (player.Hero.SpellDelay > item.Usable.Recharge)
				{
					player.Hero.SpellDelay = 0;
				}
				else
					return;
				break;
		}
		//used that item
		UseItem(item);
	}
	
	private void UseItem(HotBarItem item)
	{
		switch(item.Usable.Preffix)
		{
			case "SPELL": CastSpell(item.Usable);
				break;
			case "ITEM": UseItem(item.Usable);
				break;
		} 
	}
	
	private void UseItem(UsableItem i)
	{
		RPGItem item = (RPGItem)i;
		
		foreach(Effect effect in item.Effects)
		{
			if (effect.Target == TargetType.Self)
			{
				player.Hero.Buffs.AddEffect(effect);
			}
		}
		player.Hero.Inventory.RemoveItem(item, player);
		if (!player.Hero.Inventory.DoYouHaveThisItem(item.UniqueId))
			player.Hero.Bar.RemoveUsable(currentIndex);
		currentIndex = -1;
	}
	
	private void CastSpell(UsableItem item)
	{
		RPGSpell spell = (RPGSpell)item;
		player.Hero.CurrentMana -= spell.ManaCost;
		switch(spell.Spelltype)
		{
			case SpellTypeEnum.Projectile: CastProjectile(spell);
				break;
			case SpellTypeEnum.SelfSpell: SelfSpell(spell);
				break;
		}
	}
	
	private void SelfSpell(RPGSpell spell)
	{
		foreach(Effect effect in spell.Effects)
		{
			player.Hero.Buffs.AddEffect(effect);
		}
	}
	
	private void CastProjectile(RPGSpell spell)
	{
		//start position of projectile
		Vector3 newPosition = Camera.mainCamera.transform.position + Camera.mainCamera.transform.forward * 2;
		Quaternion newRotation = Camera.mainCamera.transform.rotation;
		//creating projectile
		GameObject go = (GameObject)Instantiate(Resources.Load(spell.PrefabName), newPosition,newRotation);
		
		
		//attach script projectile
		SpellProjectile spellProjectile = (SpellProjectile)go.AddComponent<SpellProjectile>();
		spellProjectile.spell = spell;
        spellProjectile.player = player;
		
		//attach script rigidbody
		if (go.renderer == null)
		{
			Rigidbody gameObjectsRigidBody = go.AddComponent<Rigidbody>();
			gameObjectsRigidBody.useGravity = false;
		}
	}
	
	public int PosY
	{
		get
		{
			return ScreenHeight - HotbarHeight - 10;
		}
	}
	
	public int PosX
	{
		get
		{
			return (ScreenWidth / 2) - HotbarWidth / 2;
		}
	}
}

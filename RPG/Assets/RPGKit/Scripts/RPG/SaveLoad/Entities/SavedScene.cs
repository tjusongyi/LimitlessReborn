using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SavedScene
{
	public List<SavedEnemy> Enemies;
	public List<SavedItem> Items;
	public List<SavedContainer> Containers;
	public List<SavedArmor> Armors;
	public List<SavedWeapon> Weapons;
	public List<SavedProjectile> Projectiles;
	public List<SavedSpellProjectile> SpellProjectiles;
	public List<SavedWorldObject> WorldObjects;
	public int SceneId;
	
	public int AllItems
	{
		get
		{
			return Enemies.Count + Items.Count + Containers.Count + Armors.Count + Weapons.Count + Projectiles.Count + SpellProjectiles.Count + WorldObjects.Count;
		}
	}
	
	public SavedScene()
	{
		Enemies = new List<SavedEnemy>();
		Items = new List<SavedItem>();
		Containers = new List<SavedContainer>();
		Armors = new List<SavedArmor>();
		Weapons = new List<SavedWeapon>();
		Projectiles = new List<SavedProjectile>();
		SpellProjectiles = new List<SavedSpellProjectile>();
		WorldObjects = new List<SavedWorldObject>();
		SceneId = -1;
	}
}

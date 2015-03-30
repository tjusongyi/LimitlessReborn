using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SaveLoadObjects
{
	private SavedScene scene;
	private List<GameObject> objectsToRemove;
	
	public SaveLoadObjects()
	{
		scene = new SavedScene();
		objectsToRemove = new List<GameObject>();
	}
	
	/*
	 * Saving objects
	 * */
	
	public SavedScene StoreScene(GameObject[] objects, int currentScene)
	{
		foreach(GameObject go in objects)
		{
			//enemy
			Enemy enemy = go.GetComponent<Enemy>();
			if (enemy != null)
			{
				StoreEnemy(enemy);
				continue;
			}
			//item
			Item item = go.GetComponent<Item>();
			if (item != null)
			{
				StoreItem(item);
				continue;
			}
			//weapon
			Weapon weapon = go.GetComponent<Weapon>();
			if (weapon != null)
			{
				StoreWeapon(weapon);
				continue;
			}
			//armor
			Armor armor = go.GetComponent<Armor>();
			if (armor != null)
			{
				StoreArmor(armor);
				continue;
			}
			//container
			Container container = go.GetComponent<Container>();
			if (container != null)
			{
				StoreContainer(container);
				continue;
			}
			
			Projectile projectile = go.GetComponent<Projectile>();
			if (projectile != null)
			{
				StoreProjectile(projectile);
				continue;
			}
			
			SpellProjectile spellProjectile = go.GetComponent<SpellProjectile>();
			if (spellProjectile != null)
			{
				StoreSpellProjectile(spellProjectile);
				continue;
			}
			
			WorldObject worldObject = go.GetComponent<WorldObject>();
			if (worldObject != null)
			{
				StoreWorldObject(worldObject);
				continue;
			}
		}
		scene.SceneId = currentScene;
		return scene;
	}
	
	private void StoreEnemy(Enemy enemy)
	{
		SavedEnemy sc = new SavedEnemy();
		sc.FillBasicEntity(enemy.transform, enemy.startPosition, enemy.startRotation);
		sc.CurrentHp = enemy.CurrentHp;
		sc.Items = enemy.enemy.Container.Items;
		sc.ID = enemy.ID;
		scene.Enemies.Add(sc);
	}
	
	private void StoreProjectile(Projectile projectile)
	{
		SavedProjectile sc = new SavedProjectile();
		sc.FillBasicEntity(projectile.transform, projectile.startPosition, projectile.startRotation);
		sc.PrefabName = projectile.PrefabName;
		scene.Projectiles.Add(sc);
	}
	
	private void StoreWorldObject(WorldObject worldObject)
	{
		SavedWorldObject s = new SavedWorldObject();
		s.FillBasicEntity(worldObject.transform, worldObject.startPosition, worldObject.startRotation);
		scene.WorldObjects.Add(s);
	}
	
	private void StoreSpellProjectile(SpellProjectile projectile)
	{
		SavedSpellProjectile sc = new SavedSpellProjectile();
		sc.FillBasicEntity(projectile.transform, projectile.startPosition, projectile.startRotation);
		sc.ID = projectile.spell.ID;
		scene.SpellProjectiles.Add(sc);
	}
	
	private void StoreContainer(Container item)
	{
		SavedContainer si = new SavedContainer();
		si.FillBasicEntity(item.transform, item.startPosition, item.startRotation);
		si.StoreItems(item.container.Items);
		si.ID = item.ID;
		scene.Containers.Add(si);
	}
	
	private void StoreItem(Item item)
	{
		SavedItem si = new SavedItem();
		si.FillBasicEntity(item.transform, item.startPosition, item.startRotation);
		si.ID = item.ID;
		scene.Items.Add(si);
	}
	
	private void StoreWeapon(Weapon item)
	{
		SavedWeapon si = new SavedWeapon();
		si.FillBasicEntity(item.transform, item.startPosition, item.startRotation);
		si.CurrentDurability = item.rpgItem.CurrentDurability;
		si.ID = item.ID;
		scene.Weapons.Add(si);
	}
	
	private void StoreArmor(Armor item)
	{
		SavedArmor si = new SavedArmor();
		si.FillBasicEntity(item.transform, item.startPosition, item.startRotation);
		si.CurrentDurability = item.rpgItem.CurrentDurability;
		si.ID = item.ID;
		scene.Armors.Add(si);
	}
	
	
	/*
	 * Loading / restoring objects
	 * */
	
	public List<GameObject> LoadObjects(GameObject[] objects, SavedScene selected)
	{
		foreach(GameObject go in objects)
		{
			if (RestoreEnemy(go, selected))
				continue;
			
			if (RestoreItem(go, selected))
				continue;
			
			if (RestoreWeapon(go, selected))
				continue;
			
			if (RestoreArmor(go, selected))
				continue;
			
			if (RestoreContainer(go, selected))
				continue;
			
			if (RestoreWorldObject(go, selected))
				continue;
		}
		return objectsToRemove;
	}
	
	private bool RestoreEnemy(GameObject go, SavedScene selected)
	{
		//characters
		Enemy item = go.GetComponent<Enemy>();
		if (item != null)
		{
			bool founded = false;
			foreach(SavedEnemy saveItem in selected.Enemies)
			{
				if (saveItem.CheckPosition(item.startPosition, item.startRotation))
				{
					founded = true;
					//adjust settings for founded character
					go.transform.position = saveItem.Position.GetPosition();
					go.transform.rotation = saveItem.Position.GetRotation();
					item.CurrentHp = saveItem.CurrentHp;
					item.enemy.Container.Items = saveItem.Items;
					item.enemy.Container.LoadInitialize();
					//if enemy is dead and no items in the body = remove it from the world
					if (item.CurrentHp <= 0 && item.enemy.Container.Items.Count == 0)
						founded = false;
					selected.Enemies.Remove(saveItem);
					break;
				}
			}
			if (founded == false)
			{
				objectsToRemove.Add(go);
			}
			return true;
		}
		return false;
	}
		
	private bool RestoreItem(GameObject go, SavedScene selected)
	{
		Item item = go.GetComponent<Item>();
		//items
		if (item != null)
		{
			bool founded = false;
			foreach(SavedItem saveItem in selected.Items)
			{
				if (saveItem.CheckPosition(item.startPosition, item.startRotation))
				{
					founded = true;
					//adjust settings for founded character
					go.transform.position = saveItem.Position.GetPosition();
					go.transform.rotation = saveItem.Position.GetRotation();
					item.rpgItem.CurrentDurability = saveItem.CurrentDurability;
					selected.Items.Remove(saveItem);
					break;
				}
			}
			if (founded == false)
			{
				objectsToRemove.Add(go);
			}
			return true;
		}
		return false;
	}
	
	private bool RestoreWeapon(GameObject go, SavedScene selected)
	{
		Weapon item = go.GetComponent<Weapon>();
		//weapon
		if (item != null)
		{
			bool founded = false;
			foreach(SavedWeapon saveItem in selected.Weapons)
			{
				if (saveItem.CheckPosition(item.startPosition, item.startRotation))
				{
					founded = true;
					//adjust settings for founded character
					go.transform.position = saveItem.Position.GetPosition();
					go.transform.rotation = saveItem.Position.GetRotation();
					item.rpgItem.CurrentDurability = saveItem.CurrentDurability;
					selected.Weapons.Remove(saveItem);
					break;
				}
			}
			if (founded == false)
			{
				objectsToRemove.Add(go);
			}
			return true;
		}
		return false;
	}
	
	private bool RestoreArmor(GameObject go, SavedScene selected)
	{
		Armor item = go.GetComponent<Armor>();
		//armor
		if (item != null)
		{
			bool founded = false;
			foreach(SavedArmor saveItem in selected.Armors)
			{
				if (saveItem.CheckPosition(item.startPosition, item.startRotation))
				{
					founded = true;
					//adjust settings for founded character
					go.transform.position = saveItem.Position.GetPosition();
					go.transform.rotation = saveItem.Position.GetRotation();
					item.rpgItem.CurrentDurability = saveItem.CurrentDurability;
					selected.Armors.Remove(saveItem);
					break;
				}
			}
			if (founded == false)
			{
				objectsToRemove.Add(go);
			}
			return true;
		}
		return false;
	}
	
	private bool RestoreContainer(GameObject go, SavedScene selected)
	{
		Container item = go.GetComponent<Container>();
		//items
		if (item != null)
		{
			bool founded = false;
			foreach(SavedContainer saveItem in selected.Containers)
			{
				if (saveItem.CheckPosition(item.startPosition, item.startRotation))
				{
					founded = true;
					//adjust settings
					go.transform.position = saveItem.Position.GetPosition();
					go.transform.rotation = saveItem.Position.GetRotation();
					item.container.Items = new List<ItemInWorld>();
					item.container.Items = saveItem.Items;
					selected.Containers.Remove(saveItem);
					break;
				}
			}
			if (founded == false)
			{
				objectsToRemove.Add(go);
			}
			return true;
		}
		return false;
	}
	
	private bool RestoreWorldObject(GameObject go, SavedScene selected)
	{
		WorldObject item = go.GetComponent<WorldObject>();
		//items
		if (item != null)
		{
			bool founded = false;
			foreach(SavedWorldObject saveItem in selected.WorldObjects)
			{
				if (saveItem.CheckPosition(item.startPosition, item.startRotation))
				{
					founded = true;
					//adjust settings
					go.transform.position = saveItem.Position.GetPosition();
					go.transform.rotation = saveItem.Position.GetRotation();
					selected.WorldObjects.Remove(saveItem);
					break;
				}
			}
			if (founded == false)
			{
				objectsToRemove.Add(go);
			}
			return true;
		}
		return false;
	}
}

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SavedContent
{
	public List<SavedScene> Scenes;
	public PlayerInformation HeroStats;
	
	public SavedContent(Player player)
	{
        HeroStats = player.Hero;
		Scenes = new List<SavedScene>();
	}
	
	public void AddCurrentScene(SavedScene scene)
	{
		//remove scene from previous saving
		foreach(SavedScene s in Scenes)
		{
			if (s.SceneId == scene.SceneId)
			{
				Scenes.Remove(s);
				break;
			}
		}
		//add scene to collection
		Scenes.Add(scene);
	}
}

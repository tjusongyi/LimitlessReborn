using UnityEngine;
using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using System.Xml;
using System.Xml.Serialization;

public class SaveLoad
{
	private string saveDir = Application.dataPath + "/SavePositions/";
	private string extension = ".dat";
	//select your own key
	//private string key = "rpgKit";
	public static SavedContent content;
	
	public SaveLoad()
	{
		
		objectsToRemove = new List<GameObject>();
	}
	public List<GameObject> objectsToRemove;
	
	private void CheckDir(string directory)
	{
		if (!Directory.Exists(directory))
			Directory.CreateDirectory(directory);
	}
	
	private string CompleteFileName(string fileName)
	{
		return saveDir + fileName + extension;
	}
	
	private string SerializeStats<T>(T playerStats)
	{
		XmlSerializer serializer = new XmlSerializer(typeof(T));
		StringWriter xml = new StringWriter();
		
		serializer.Serialize(xml, playerStats);
		string result = xml.ToString();
		//result = Encryption.Decrypt(result, key);
		return result;
	}
	
	private string BasicCrypting(string content)
	{
		return string.Empty;
	}
	
	public bool CheckFileName(string fileName)
	{
		if (File.Exists(CompleteFileName(fileName)))
		    return true;
		
		return false;
	}
	
	public bool Save(string fileName, Transform transform, GameObject[] gameObjects, int currentScene, Player player)
	{
		//save the game
        player.Hero.StorePlayerPosition(transform);
		CheckDir(saveDir);
		RemoveCurrentScene(currentScene);
		StoreContent(gameObjects, currentScene);
		string completeFileName = CompleteFileName(fileName);
		
		TextWriter tw = new StreamWriter(completeFileName);
		string stringContent = SerializeStats(content);
		tw.Write(stringContent);
		tw.Close();
		
		return true;
	}
	
	private void RemoveCurrentScene(int currentScene)
	{
		foreach(SavedScene scene in content.Scenes)
		{
			if (scene.SceneId == currentScene)
			{
				content.Scenes.Remove(scene);
				break;
			}
		}
	}
	
	public void Load(string fileName, Player player)
	{
		XmlSerializer serializer = new XmlSerializer(typeof(SavedContent));
		TextReader reader = new StreamReader(CompleteFileName(fileName));
		if (reader != null)
		{
			content = (SavedContent)serializer.Deserialize(reader);
			reader.Close();
		}
		//hero stats
        player.Hero = content.HeroStats;
	}
	
	public List<SavePosition> GetLoadPositions()
	{
		CheckDir(saveDir);
		string[] filePaths = Directory.GetFiles(saveDir);
		List<SavePosition> sp = new List<SavePosition>();
		foreach(string filePath in filePaths)
		{
			if (filePath.IndexOf(".dat") == -1)
				continue;
			
			SavePosition s = new SavePosition();
			s.FileName = filePath.Remove(0,filePath.LastIndexOf("/") + 1);
			s.FileName = s.FileName.Replace(".dat", string.Empty);
			sp.Add(s);
		}
		return sp;
	}

	//update this method for storing game object in save position
	public void StoreContent(GameObject[] objects, int currentScene)
	{
		SaveLoadObjects so = new SaveLoadObjects();
		
		SavedScene scene = so.StoreScene(objects, currentScene);
		foreach(SavedScene ss in content.Scenes)
		{
			if (ss.SceneId == scene.SceneId)
			{
				content.Scenes.Remove(ss);
				break;
			}
		}
		content.AddCurrentScene(scene);
	}
	
	//update this method for loading game obejct
	public SavedScene LoadWorldItems(GameObject[] objects, int currentScene)
	{
		SavedScene selected = new SavedScene();
		foreach(SavedScene scene in content.Scenes)
		{
			if (scene.SceneId == currentScene)
				selected = scene;
		}
		if (selected.SceneId == -1)
			return selected;
		SaveLoadObjects so = new SaveLoadObjects();
		objectsToRemove = so.LoadObjects(objects, selected);
		return selected;
	}
}



using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Player : MonoBehaviour
{
	public PlayerInformation Hero;
	public GeneralPlayerSounds sounds;
	public RPGScene scene;
	public static bool isChangingScene;
	public bool doEvents;
	
	public static ContainerType Container;
	public RPGContainer rpgContainer;
    public static GatheringItem gatheringItem;
	
	public static float sellPriceModifier;
	
	public static UsableItem currentItem;
	public static int activeNPC;
	public static string ActiveNPCName;
	
	private bool minimapCameraStatus = true;
	private GameObject miniMap;
	public static GeneralData Data;
	
	public Texture2D cursorImage;
	
	public float effectTimer;
    private List<SpawnPoint> spawnPoints;
    private float checkTimer;

    public static bool IsLoading;

    void Awake()
    {
        DontDestroyOnLoad(gameObject);
        ChangeMinimap(false);
        gameObject.AddComponent<GUIScale>();
    }
	
	void Start()
	{
		Data = new GeneralData();
		Hero = new PlayerInformation();
		SaveLoad.content = new SavedContent(this);
        spawnPoints = new List<SpawnPoint>();
        sounds = new GeneralPlayerSounds();
		LoadScene();
	}

    void InitSpawnPoints()
    {
        spawnPoints = new List<SpawnPoint>();
        try
        {
            GameObject[] spawns = GameObject.FindGameObjectsWithTag("SpawnPoint");
            foreach (GameObject g in spawns)
            {
                SpawnPoint spawnPoint = (SpawnPoint)g.GetComponent<SpawnPoint>();
                spawnPoint.Init();

                if (spawnPoint.spawnPoint.Enemies.Count == 0)
                {
                    spawnPoint.spawnPoint.Enemies = scene.Enemies;
                }
                spawnPoints.Add(spawnPoint);
            }
        }
        catch
        { 
        
        }
    }
	
	public GameObject[] GameObjects
	{
		get
		{
			return FindObjectsOfType(typeof(GameObject)) as GameObject[];
		}
	}

    void CheckSpawnPoints()
    {
        foreach (SpawnPoint sp in spawnPoints)
        {
            sp.CheckSpawnTime(5, transform.position);
        }
    }

	private void LoadScene()
	{
		foreach(RPGScene s in Data.Scenes)
		{
			if (s.SceneId == Application.loadedLevel)
			{
				scene = s;
			}
		}
	}

    void OnLevelWasLoaded(int level)
    {
        LoadScene();

        InitSpawnPoints();

        DoEvents();
    }

	void Update()
	{
        if (scene == null || scene.SceneType == SceneTypeEnum.MainMenu || !BasicGUI.IsAllWindowsClosed)
            return;


		Hero.AttackDelay += Time.deltaTime;
        Hero.SpellDelay += Time.deltaTime;
        Hero.UseItemDelay += Time.deltaTime;
		effectTimer += Time.deltaTime;
        checkTimer += Time.deltaTime;

        //spawn points
        if (checkTimer >= 5)
        {
            CheckSpawnPoints();
            checkTimer = 0;
        }

		
		if (effectTimer >= 1)
		{
            Hero.Quest.CheckInventoryItem(this);

			Hero.Buffs.DoEffects(this);
			effectTimer = 0;
			
			Hero.TotalHp = Hero.BaseHp + Hero.BonusHp;
			Hero.TotalMana = Hero.BaseMana + Hero.BonusMana;
			
			if (scene.SceneType != SceneTypeEnum.MainMenu)
			{
				//regen mana
				if (Hero.CurrentMana >= Hero.TotalMana)
				{
					Hero.CurrentMana = Hero.TotalMana;
				}
				else
				{
					Hero.CurrentMana += Hero.ManaRegeneration;
				}
				//regen hitpoint
				if (Hero.CurrentHp >= Hero.TotalHp)
				{
					Hero.CurrentHp = Hero.TotalHp;
				}
				else
				{
					Hero.CurrentHp += Hero.HpRegeneration;
				}
			}
		}
		
		if (doEvents)
			DoEvents();
		
		//if player is dead (0 HP)
		if (Hero.CurrentHp <= 0)
		{
			Destroy(this);
			Application.LoadLevel(0);
		}
		
		//reset GUI
		if (!BasicGUI.isConversationDisplayed)
			NPCGUI.MenuMode = NPCMenuType.Conversation;
		
		//when we are changing scene we must load current state
		if (isChangingScene && Application.loadedLevel == Hero.CurrentScene)
		{
			SaveLoad sl = new SaveLoad();
			SavedScene currentScene = sl.LoadWorldItems(GameObjects, Hero.CurrentScene);
			isChangingScene = false;
			
			SpawnObjects(currentScene);
			//remove objects that dont exits in saved position
			foreach(GameObject go in sl.objectsToRemove)
				Destroy(go);
		}
	}
	
	void OnGUI()
	{
		if (IsPossibleToAct)
		{
			//draw cursor
			Rect rect = new Rect((Screen.width/2 )- 10, (Screen.height/2) - 10,21,21);
			GUI.Label(rect,cursorImage);
			activeNPC = 0;
		}
		
		if (BasicGUI.isMainMenuDisplayed || !BasicGUI.IsAllWindowsClosed)
		{
			Time.timeScale = 0.0001f;
		}
		else
		{
			Time.timeScale = 1;
		}
	}
	
	private void ChangeMinimap(bool Enabled)
	{
		if (Enabled && !Hero.Settings.UseMinimap || minimapCameraStatus == Enabled)
			return;
		
		if (miniMap == null)
		{
			miniMap = GameObject.Find("Minimap");
		}
		if (miniMap == null)
			return;
		miniMap.camera.enabled = Enabled;
		minimapCameraStatus = Enabled;
	}
	
    /// <summary>
    /// Enabling or disabling mouse look
    /// </summary>
    /// <param name="enabled">true if you want to enable mouse look</param>
	public void ChangeMouseControl(bool enabled)
	{
        FPSMouseLook mouse = (FPSMouseLook)GameObject.Find("Player").GetComponent<FPSMouseLook>();
        
		if (mouse == null)
			return;
		mouse.enabled = enabled;
		Screen.lockCursor = enabled;
        mouse = (FPSMouseLook)GameObject.Find("Main Camera").GetComponent<FPSMouseLook>();
		mouse.enabled = enabled;
		Screen.showCursor = !enabled;
	}
	
	private bool IsPossibleToAct
	{
		get
		{
			if (!BasicGUI.IsAllWindowsClosed || BasicGUI.isMainMenuDisplayed || scene.SceneType == SceneTypeEnum.MainMenu)
				return false;
			else
				return true;
		}
	}
	
	private void SpawnObjects(SavedScene currentScene)
	{
		GameObject go;
		
		foreach(SavedWeapon weapon in currentScene.Weapons)
		{
			foreach(RPGWeapon w in Data.Weapons)
			{
				if (weapon.ID == w.ID)
				{
					go = (GameObject)Instantiate(Resources.Load(w.PrefabName), weapon.Position.GetPosition(), weapon.Position.GetRotation());
					Weapon weaponScript = go.AddComponent<Weapon>();
					weaponScript.ID = w.ID;			
					//weaponScript.rpgItem = w;
					break;
				}
			}
		}

        foreach (SavedEnemy enemy in currentScene.Enemies)
        {
            foreach (RPGEnemy e in Data.Enemies)
            {
                if (enemy.ID == e.ID)
                {
                    e.LoadPrefab();
                    go = (GameObject)Instantiate(e.Prefab, enemy.Position.GetPosition(), enemy.Position.GetRotation());
                    Enemy et;
                    if (go.GetComponent<Enemy>() == null)
                    {
                        et = go.AddComponent<Enemy>();
                    }
                    else
                    {
                        et = go.GetComponent<Enemy>();
                    }
                    et.enemy = e;
                    et.CurrentHp = enemy.CurrentHp;
                    if (et.CurrentHp <= 0)
                    {
                        et.enemy.Container.Initialize(this);
                        et.enemy.Container.Items = enemy.Items;
                    }
                    break;
                }
            }
        }

		foreach(SavedArmor armor in currentScene.Armors)
		{
			foreach(RPGArmor a in Data.Armors)
			{
				/*if (armor.ID == a.ID)
				{
					go = (GameObject)Instantiate(Resources.Load(a.PrefabName), armor.Position.GetPosition(), armor.Position.GetRotation());
					Armor script = go.AddComponent<Armor>();
					script.ID = a.ID;			
					script.rpgItem = a;
					break;
				}*/
			}
		}
		
		foreach(SavedItem item in currentScene.Items)
		{
			/*foreach(RPGItem i in Data.Items)
			{
				if (item.ID == i.ID)
				{
					go = (GameObject)Instantiate(Resources.Load(i.PrefabName), item.Position.GetPosition(), item.Position.GetRotation());
					Item script = go.AddComponent<Item>();
					script.ID = i.ID;			
					script.rpgItem = i;
					break;
				}
			}*/
		}
		
		foreach(SavedProjectile projectile in currentScene.Projectiles)
		{
			go = (GameObject)Instantiate(Resources.Load(projectile.PrefabName), projectile.Position.GetPosition(), projectile.Position.GetRotation());
			Projectile script = go.AddComponent<Projectile>();
			script.PrefabName = projectile.PrefabName;
			break;
		}
		
		foreach(SavedSpellProjectile item in currentScene.SpellProjectiles)
		{
			foreach(RPGSpell s in Data.Spells)
			{
				if (item.ID == s.ID)
				{
					go = (GameObject)Instantiate(Resources.Load(s.PrefabName), item.Position.GetPosition(), item.Position.GetRotation());
					SpellProjectile script = go.AddComponent<SpellProjectile>();
					script.spell = s;
                    script.player = this;
					break;
				}
			}
		}
	}

    private void DoEvents()
    {
        doEvents = false;

        if (Hero.ActionsToDo.Count == 0)
            return;
        GameObject[] objects = GameObjects;
        int count = Hero.ActionsToDo.Count;
        for (int x = 0; x < count; x++)
        {
            foreach (ActionEvent e in Hero.ActionsToDo)
            {
                bool breakFor = false;

                switch (e.ActionType)
                {
                    case ActionEventType.RemoveWorldObject:
                        foreach (GameObject go in objects)
                        {
                            WorldObject wo = go.GetComponent<WorldObject>();
                            if (wo == null)
                                continue;
                            if (e.Item == wo.ID)
                            {
                                Destroy(go);
                                breakFor = true;
                            }
                        }
                        break;

                    case ActionEventType.SpawnCreature:
                        foreach (SpawnPoint sp in spawnPoints)
                        {
                            if (sp.ID == e.Item)
                            {
                                sp.EventSpawn();
                                breakFor = true;
                            }
                        }
                        break;

                    case ActionEventType.NoteDisplay:
                        InfoNoteGUI.isNoteDisplayed = true;
                        InfoNoteGUI.Text = e.Text;
                        breakFor = true;
                        break;

                    case ActionEventType.UseTeleport:
                        RPGTeleport teleport = Storage.LoadById<RPGTeleport>(e.Item, new RPGTeleport());
                        //if teleport is in the same scene
                        if (scene.SceneId != teleport.SceneId)
                        {
                            Player.IsLoading = true;
                            //save current objects
                            SaveLoad sl = new SaveLoad();
                            sl.StoreContent(GameObjects, Application.loadedLevel);
                            Player.isChangingScene = true;

                            if (Hero.CurrentScene > -1)
                            {
                                Hero.CurrentScene = teleport.SceneId;
                                Application.LoadLevel(Hero.CurrentScene);
                            }
                        }
                        transform.position = new Vector3(teleport.ArriveX, teleport.ArriveY, teleport.ArriveZ);
                        breakFor = true;
                        break;
                }
                if (breakFor)
                {
                    Hero.ActionsToDo.Remove(e);
                    break;
                }
            }
        }
    }
}

public enum ContainerType
{
	Container = 1,
	CharacterBody = 2
}
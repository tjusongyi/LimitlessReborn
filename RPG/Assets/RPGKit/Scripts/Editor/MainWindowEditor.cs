using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;

public class MainWindowEditor : EditorWindow 
{
	public MainWindowTypeEnum MainWindowType;
	
	private GUISkin Skin;
	private bool loadWindows = true; 
	
	private List<BaseEditorWindow> FirstButtonRow;
	private List<BaseEditorWindow> SecondButtonRow;
	
	private Validate validate;
	public bool isValidateDisplay;
	
	//windows
	public ItemCategoryEditor itemCategory;
	public SkillEditor skillEditor;
	public AttributeEditor attributeEditor;
	public ArmorEditor armorEditor;
	public EquipmentSlotEditor equipmentSlotEditor;
	public TeleportEditor teleportEditor;
	public ConversationEditor conversationEditor;
	public ShopEditor shopEditor;
	public SpellEditor spellEditor;
	public EnemyEditor enemyEditor;
	public ContainerEditor containerEditor;
	public QuestEditor questEditor;
	public ItemEditor itemEditor;
	public WorldObjectEditor worldObjectEditor;
	public SpellShopEditor spellShop;
	public WeaponEditor weaponEditor;
	public NPCEditor npcEditor;
	public RaceEditor raceEditor;
	public ClassEditor classEditor;
	public GuildEditor guildEditor;
	public ReputationEditor reputationEditor;
	public SceneEditor sceneEditor;
	public SpawnPointEditor spawnEditor;
    public QuestCategoryEditor questCategoryEditor;
	
	[MenuItem("Window/RPG")] 	
	static void Init()
	{
		MainWindowEditor window = (MainWindowEditor)EditorWindow.GetWindow(typeof(MainWindowEditor));
		window.autoRepaintOnSceneChange = false;
		window.Show();
		
	}
	
	public void InitWindows()
	{
		Skin = (GUISkin)Resources.Load("GUI/EditorGUI");
		
		//windows init
		npcEditor = new NPCEditor(Skin, this);
		
		weaponEditor = new WeaponEditor(Skin, this);
		
		worldObjectEditor = new WorldObjectEditor(Skin, this);
		
		questEditor = new QuestEditor(Skin, this);
			
		itemCategory = new ItemCategoryEditor(Skin, this);
		
		skillEditor = new SkillEditor(Skin, this);
		
		attributeEditor = new AttributeEditor(Skin, this);
		
		armorEditor = new ArmorEditor(Skin, this);
		
		equipmentSlotEditor = new EquipmentSlotEditor(Skin, this);
		
		teleportEditor = new TeleportEditor(Skin, this);
		
		conversationEditor = new ConversationEditor(Skin, this);
		
		shopEditor = new ShopEditor(Skin, this);
		
		spellEditor = new SpellEditor(Skin, this);
		
		enemyEditor = new EnemyEditor(Skin, this);
		
		containerEditor = new ContainerEditor(Skin, this);
		
		itemEditor = new  ItemEditor(Skin, this);
		
		spellShop = new SpellShopEditor(Skin, this);
		
		classEditor = new ClassEditor(Skin, this);
		
		raceEditor = new RaceEditor(Skin, this);
		
		guildEditor = new GuildEditor(Skin, this);
		
		reputationEditor = new ReputationEditor(Skin, this);
		
		sceneEditor = new SceneEditor(Skin, this);
		
		spawnEditor = new SpawnPointEditor(Skin, this);

        questCategoryEditor = new QuestCategoryEditor(Skin, this);
		
		validate = new Validate();
		loadWindows = false;
	}
	
	void OnGUI()
	{
		if (loadWindows)
			InitWindows();
		
		
		GUILayout.BeginArea(new Rect(5,5, position.width - 10, 140), Skin.box);
		GUILayout.BeginHorizontal();
		if (GUILayout.Button("Reload data", GUILayout.Width(200)))
		{
			InitWindows();
		}
		
		if (GUILayout.Button("Validate", GUILayout.Width(200)))
		{
			InitWindows();
			validate.ValidateXML(this);
			isValidateDisplay = true;
		}
		GUILayout.EndHorizontal();
		
		
		GUILayout.BeginHorizontal();
		
		if (armorEditor == null)
		{
			InitWindows();
		}
		
		if (GUILayout.Button(armorEditor.EditorName, GUILayout.Width(100)))
		{
			MainWindowType = MainWindowTypeEnum.Armor;
		}
		
		if (GUILayout.Button(attributeEditor.EditorName, GUILayout.Width(100)))
		{
			MainWindowType = MainWindowTypeEnum.Attribute;
		}
		
		if (GUILayout.Button(classEditor.EditorName, GUILayout.Width(100)))
		{
			MainWindowType = MainWindowTypeEnum.Class;
		}
		
		if (GUILayout.Button(containerEditor.EditorName, GUILayout.Width(100)))
		{
			MainWindowType = MainWindowTypeEnum.Container;
		}
		
		if (GUILayout.Button(conversationEditor.EditorName, GUILayout.Width(100)))
		{
			MainWindowType = MainWindowTypeEnum.Conversation;
		}
		
		if (GUILayout.Button(enemyEditor.EditorName, GUILayout.Width(100)))
		{
			MainWindowType = MainWindowTypeEnum.Enemy;
		}
        if (GUILayout.Button(equipmentSlotEditor.EditorName, GUILayout.Width(100)))
        {
            MainWindowType = MainWindowTypeEnum.EquipmentSlot;
        }

        if (GUILayout.Button(guildEditor.EditorName, GUILayout.Width(100)))
        {
            MainWindowType = MainWindowTypeEnum.Guild;
        }

        if (GUILayout.Button(itemEditor.EditorName, GUILayout.Width(100)))
        {
            MainWindowType = MainWindowTypeEnum.Item;
        }
		
		GUILayout.EndHorizontal();
		GUILayout.BeginHorizontal();

       

        if (GUILayout.Button(itemCategory.EditorName, GUILayout.Width(100)))
        {
            MainWindowType = MainWindowTypeEnum.ItemCategory;
        }

        if (GUILayout.Button(npcEditor.EditorName, GUILayout.Width(100)))
        {
            MainWindowType = MainWindowTypeEnum.NPC;
        }

        if (GUILayout.Button(questEditor.EditorName, GUILayout.Width(100)))
        {
            MainWindowType = MainWindowTypeEnum.Quest;
        }

        if (GUILayout.Button(questCategoryEditor.EditorName, GUILayout.Width(100)))
        {
            MainWindowType = MainWindowTypeEnum.QuestCategory;
        }

        if (GUILayout.Button(raceEditor.EditorName, GUILayout.Width(100)))
        {
            MainWindowType = MainWindowTypeEnum.Race;
        }

        if (GUILayout.Button(reputationEditor.EditorName, GUILayout.Width(100)))
        {
            MainWindowType = MainWindowTypeEnum.Reputation;
        }

        if (GUILayout.Button(sceneEditor.EditorName, GUILayout.Width(100)))
        {
            MainWindowType = MainWindowTypeEnum.Scene;
        }

        if (GUILayout.Button(shopEditor.EditorName, GUILayout.Width(100)))
        {
            MainWindowType = MainWindowTypeEnum.Shop;
        }

        if (GUILayout.Button(skillEditor.EditorName, GUILayout.Width(100)))
        {
            MainWindowType = MainWindowTypeEnum.Skill;
        }

       

        GUILayout.EndHorizontal();
        GUILayout.BeginHorizontal();

        if (GUILayout.Button(spawnEditor.EditorName, GUILayout.Width(100)))
        {
            MainWindowType = MainWindowTypeEnum.SpawnPoint;
        }

        if (GUILayout.Button(spellEditor.EditorName, GUILayout.Width(100)))
        {
            MainWindowType = MainWindowTypeEnum.Spell;
        }

        if (GUILayout.Button(spellShop.EditorName, GUILayout.Width(100)))
        {
            MainWindowType = MainWindowTypeEnum.SpellShop;
        }
		
		if (GUILayout.Button(teleportEditor.EditorName, GUILayout.Width(100)))
		{
			MainWindowType = MainWindowTypeEnum.Teleport;
		}
		
		if (GUILayout.Button(weaponEditor.EditorName, GUILayout.Width(100)))
		{
			MainWindowType = MainWindowTypeEnum.Weapon;
		}
		
		if (GUILayout.Button(worldObjectEditor.EditorName, GUILayout.Width(100)))
		{
			MainWindowType = MainWindowTypeEnum.WorldObject;
		}
		
		GUILayout.EndHorizontal();
		GUILayout.EndArea();
		
		if (isValidateDisplay)
		{
			validate.position = position;
			validate.DisplayErrors();
			return;
		}
		
		switch(MainWindowType)
		{
			case MainWindowTypeEnum.Class:			
				classEditor.DisplayWindow(position);
				break;
			
			case MainWindowTypeEnum.Race:
				raceEditor.DisplayWindow(position);
				break;
			
			case MainWindowTypeEnum.Weapon:	
				weaponEditor.DisplayWindow(position);
				break;
			
			case MainWindowTypeEnum.NPC:	
				npcEditor.DisplayWindow(position);
				break;
			
			case MainWindowTypeEnum.WorldObject:	
				worldObjectEditor.DisplayWindow(position);
				break;
			
			case MainWindowTypeEnum.Quest:	
				questEditor.DisplayWindow(position);
				break;
			
			case MainWindowTypeEnum.Guild:	
				guildEditor.DisplayWindow(position);
				break;
			
			case MainWindowTypeEnum.Container:	
				containerEditor.DisplayWindow(position);
				break;
			
			case MainWindowTypeEnum.Enemy:	
				enemyEditor.DisplayWindow(position);
				break;
			
			case MainWindowTypeEnum.Spell:	
				spellEditor.DisplayWindow(position);
				break;
			
			case MainWindowTypeEnum.ItemCategory:	
				itemCategory.DisplayWindow(position);
				break;
			
			case MainWindowTypeEnum.Skill:
				skillEditor.DisplayWindow(position);
				break;
			
			case MainWindowTypeEnum.Attribute:
				attributeEditor.DisplayWindow(position);
				break;
			
			case MainWindowTypeEnum.Armor:
				armorEditor.DisplayWindow(position);
				break;
			
			case MainWindowTypeEnum.EquipmentSlot:
				equipmentSlotEditor.DisplayWindow(position);
				break;
			
			case MainWindowTypeEnum.Reputation:
				reputationEditor.DisplayWindow(position);
				break;
			
			case MainWindowTypeEnum.Teleport:
				teleportEditor.DisplayWindow(position);
				break;
			
			case MainWindowTypeEnum.Conversation:
				conversationEditor.DisplayWindow(position);
				break;
			
			case MainWindowTypeEnum.Scene:
				sceneEditor.DisplayWindow(position);
				break;
			
			case MainWindowTypeEnum.SpawnPoint:
				spawnEditor.DisplayWindow(position);
				break;
			
			case MainWindowTypeEnum.Shop:
				shopEditor.DisplayWindow(position);
				break;
			
			case MainWindowTypeEnum.SpellShop:
				spellShop.DisplayWindow(position);
				break;
			
			case MainWindowTypeEnum.Item:
				itemEditor.DisplayWindow(position);
				break;

            case MainWindowTypeEnum.QuestCategory:
                questCategoryEditor.DisplayWindow(position);
                break;
		}
	}
}


public enum MainWindowTypeEnum
{
	None,
	Armor,
	Attribute,
	Class,
	Container,
	Conversation,
	Enemy,
	EquipmentSlot,
	Guild,
	Item,
	ItemCategory,
	NPC,
	Race,
	Reputation,
	Quest,
    QuestCategory,
	Scene,
	Shop,
	Skill,
	SpawnPoint,
	Spell,
	SpellShop,
	Teleport,
	Weapon,
	WorldObject
}
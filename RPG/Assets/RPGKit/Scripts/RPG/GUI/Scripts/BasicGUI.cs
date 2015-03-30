using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BasicGUI : MonoBehaviour 
{
	public static bool isCharacterDisplayed;
	public static bool isQuestLogDisplayed;
	public static bool isConversationDisplayed;
	public static bool isInventoryDisplayed;
	public static bool isMainMenuDisplayed = true;
	public static bool isShopActivated;
	public static bool isQuantityActivated;
	public static bool isContainerDisplayed;
	public static bool isSpellbookDisplayed;
	public static bool isNoteDisplayed;
	
	protected int LeftTopX = 10;
	protected int RightTopX;
	protected int LeftTopY = 10;
	public List<ActiveGUIControl> activeControls; 
	
	protected int WindowWidth;

    protected GUISkin Skin;
	
	protected int WindowHeight { get; private set;}
	
	protected int GUIItemButon { get; private set;}
	protected int GUIItemIndent = 2;
	
	
	protected int QuantityLeftTopX;
	protected int QuantityLeftTopY = 200;
	protected int QuantityWidth = 80;
	protected int QuantityHeight = 350;
	
	protected int ContainerHeight = 170;
	
	//hotbar
	protected int HotbarWidth;
	protected int HotbarHeight;
	
	protected int ScreenHeight;
	protected int ScreenWidth;
	
	protected float timeDelay = 0;
	
	protected Player player;
    protected PlayerController playerController;

	
	public void Prepare()
	{
        if (Skin == null)
        {
            Skin = (GUISkin)Resources.Load("GUI/RPGGUI");
        }

		LeftTopX = 10;
		
		ScreenWidth = 1024;
		ScreenHeight = 768;
		
		SetWindowsCoordinates();
		
		HotbarHeight = GUIItemButon + 2 * GUIItemIndent + 9;
		HotbarWidth = ((GUIItemButon + GUIItemIndent) * 10) + 9;
		player = gameObject.GetComponent<Player>();
		QuantityLeftTopX = (ScreenWidth / 2) - QuantityWidth/2 + 30;
		RightTopX = (ScreenWidth / 2) + 80;
		WindowWidth = (ScreenWidth / 2) - 30;
		WindowHeight = ScreenHeight - 200;
        playerController = gameObject.GetComponent<PlayerController>();

		activeControls = new List<ActiveGUIControl>();
	}
	
	protected void PlayGUISound(GUISound sound)
	{
		if (player == null)
			return;
		AudioClip clip = new AudioClip();
		switch(sound)
		{
			case GUISound.Gold:	
				clip = player.sounds.goldCoin;
				break;
			case GUISound.SkillUp:
				clip = player.sounds.skillUp;
				break;
			case GUISound.Repair:
				clip = player.sounds.repair;
				break;
			case GUISound.EquipWeapon:
				clip = player.sounds.equipWeapon;
				break;
			case GUISound.EquipArmor:
				clip = player.sounds.equipArmor;
				break;
		}
		audio.PlayOneShot(clip);
	}

    protected void ChangeIventoryStatus()
    {
        BasicGUI.isInventoryDisplayed = !BasicGUI.isInventoryDisplayed;
        if (BasicGUI.isInventoryDisplayed)
        {
            player.ChangeMouseControl(false);
            if (BasicGUI.isQuestLogDisplayed)
                BasicGUI.isQuestLogDisplayed = false;

            if (BasicGUI.isSpellbookDisplayed)
                BasicGUI.isSpellbookDisplayed = false;
        }
        else if (IsAllWindowsClosed)
        {
            player.ChangeMouseControl(true);
        }
        timeDelay = 0;
    }

    protected void ChangeQuestLogStatus()
    {
        BasicGUI.isQuestLogDisplayed = !BasicGUI.isQuestLogDisplayed;

        if (BasicGUI.isQuestLogDisplayed)
        {
            player.ChangeMouseControl(false);
            if (BasicGUI.isInventoryDisplayed)
                BasicGUI.isInventoryDisplayed = false;

            if (BasicGUI.isSpellbookDisplayed)
                BasicGUI.isSpellbookDisplayed = false;
        }
        else if (IsAllWindowsClosed)
        {
            player.ChangeMouseControl(true);
        }
        timeDelay = 0;
    }

    protected void ChangeSpellBookStatus()
    {
        BasicGUI.isSpellbookDisplayed = !BasicGUI.isSpellbookDisplayed;
        timeDelay = 0;

        if (BasicGUI.isSpellbookDisplayed)
        {
            player.ChangeMouseControl(false);
            if (BasicGUI.isQuestLogDisplayed)
                BasicGUI.isQuestLogDisplayed = false;

            if (BasicGUI.isInventoryDisplayed)
                BasicGUI.isInventoryDisplayed = false;
        }
        else if (IsAllWindowsClosed)
        {
            player.ChangeMouseControl(true);
        }
    }

    protected void ChangeCharacterStatus()
    {
        BasicGUI.isCharacterDisplayed = !BasicGUI.isCharacterDisplayed;
        timeDelay = 0;

        if (BasicGUI.isCharacterDisplayed)
        {
            player.ChangeMouseControl(false);
        }
        else if (IsAllWindowsClosed)
        {
            player.ChangeMouseControl(true);
        }
    }


	void LateUpdate()
	{
        timeDelay += 0.01f;

		//reset GUI
		if (!isConversationDisplayed)
			NPCGUI.MenuMode = NPCMenuType.Conversation;
			
		if (timeDelay > 1000)
			timeDelay = 1;
		
		activeControls = new List<ActiveGUIControl>();
	}
	
	void SetWindowsCoordinates()
	{
		LeftTopX = 10;
		RightTopX = (ScreenWidth / 2) + 20;
		GUIItemButon = 48;
		WindowWidth = (ScreenWidth / 2) - 30;
		WindowHeight = ScreenHeight - 120;
	}
	
	public static bool IsAllWindowsClosed
	{
		get
		{
			if (!isQuestLogDisplayed && 
			    !isConversationDisplayed && 
			    !isInventoryDisplayed && 
			    !isCharacterDisplayed && 
			    !isContainerDisplayed &&
			    !isSpellbookDisplayed &&
				!isNoteDisplayed)
				return true;
			return false;
		}
	}
	
	public static void CloseAllWindows()
	{
		isQuantityActivated = false;
		isCharacterDisplayed = false;
		isContainerDisplayed = false;
		isConversationDisplayed = false;
		isInventoryDisplayed = false;
		isQuestLogDisplayed = false;
		isSpellbookDisplayed = false;
		isShopActivated = false;
		
	}
	
	protected bool IsOpenKey(KeyCode keyCode)
	{
		
		if (Input.GetKey(keyCode) && timeDelay > 0.45f)
		{
			return true;
		}
		else
		{
			return false;
		}
	}
	
	//left box (character, NPC)
	public void LeftBox(string name)
	{
		Box(LeftTopX, LeftTopY, WindowWidth, WindowHeight,name);
	}
	
	//right box (quest log, inventory, spellbook)
	public void RightBox(string name)
	{
		Box(RightTopX, LeftTopY, WindowWidth - 60, WindowHeight,name);
	}
	
	//-----------------------------------------------------------------
	//BOX
	//-----------------------------------------------------------------
	public void Box(int x, int y, int width, int height)
	{
		Box(x, y, width, height, string.Empty);
	}
	
	public void Box(int x, int y, int width, int height, string caption)
	{
		Rect rect = new Rect(x * GUIScale.widthScale, y * GUIScale.heightScale, width * GUIScale.widthScale, height * GUIScale.heightScale);
		//Debug.Log(rect.x + " " + rect.y + " " + rect.width + " " + rect.height + " " + GUIScale.heightScale + " " + GUIScale.widthScale	);
		GUI.Box(rect,caption, Skin.box);
	}
	
	public void Box(string id, int x, int y, int width, int height, Texture2D texture)
	{
		Rect rect = new Rect(x * GUIScale.widthScale, y * GUIScale.heightScale, width * GUIScale.widthScale, height * GUIScale.heightScale);
		GUI.Box(rect,texture, Skin.box);
	}
	
	//-----------------------------------------------------------------
	//Button
	//-----------------------------------------------------------------
	public void Button(string id, int x, int y, int width, int height, Texture2D texture)
	{
		Rect rect = new Rect(x * GUIScale.widthScale, y * GUIScale.heightScale, width * GUIScale.widthScale, height * GUIScale.heightScale);
		GUI.Button(rect, texture);
	
		activeControls.Add(ActiveGUIControl.Create(rect, id));
	}
	
	public bool Button(Rect rect, string caption, GUIStyle style)
	{
		Rect newRect = new Rect(rect.x * GUIScale.widthScale, rect.y * GUIScale.heightScale, rect.width * GUIScale.widthScale, rect.height * GUIScale.heightScale);
		if (GUI.Button(newRect, caption, style))
		{
			return true;
		}
		return false;
	}
	
	public bool Button(Rect rect, string caption)
	{
		Rect newRect = new Rect(rect.x * GUIScale.widthScale, rect.y * GUIScale.heightScale, rect.width * GUIScale.widthScale, rect.height * GUIScale.heightScale);
		if (GUI.Button(newRect, caption, Skin.button))
		{
			return true;
		}
		return false;
	}
	
	public bool Button(Rect rect, string caption, string id)
	{
		Rect newRect = new Rect(rect.x * GUIScale.widthScale, rect.y * GUIScale.heightScale, rect.width * GUIScale.widthScale, rect.height * GUIScale.heightScale);
		activeControls.Add(ActiveGUIControl.Create(newRect, id));
        if (GUI.Button(newRect, caption, Skin.button))
		{
			return true;
		}
		return false;
	}
	
	public bool Button(Rect rect, Texture2D caption, string id)
	{
		Rect newRect = new Rect(rect.x	 * GUIScale.widthScale, rect.y * GUIScale.heightScale, rect.width * GUIScale.widthScale, rect.height * GUIScale.heightScale);
		
		activeControls.Add(ActiveGUIControl.Create(newRect, id));

        if (GUI.Button(newRect, caption, Skin.button))
		{
			return true;
		}
		return false;
	}
	
	public bool Button(Rect rect, Texture2D caption)
	{
		Rect newRect = new Rect(rect.x	 * GUIScale.widthScale, rect.y * GUIScale.heightScale, rect.width * GUIScale.widthScale, rect.height * GUIScale.heightScale);
        if (GUI.Button(newRect, caption, Skin.button))
		{
			return true;
		}
		return false;
	}
	
	public void Button(string idName, int x, int y, int width, int height)
	{
		Rect rect = new Rect(x * GUIScale.widthScale, y * GUIScale.heightScale, width * GUIScale.widthScale, height * GUIScale.heightScale);
		GUI.Button(rect, idName, Skin.button);
		
		activeControls.Add(ActiveGUIControl.Create(rect, idName));
	}
	
	public void Button(string id, int x, int y, int width, int height, string buttonText)
	{
		Rect rect = new Rect(x * GUIScale.widthScale, y * GUIScale.heightScale, width * GUIScale.widthScale, height * GUIScale.heightScale);
		GUI.Button(rect, buttonText, Skin.button);
		
		activeControls.Add(ActiveGUIControl.Create(rect, id));
	}
	
	public string IsButtonTriggered(Vector3 mousePosition)
	{
		string result = string.Empty;
		foreach(ActiveGUIControl ab in activeControls)
		{
			if (ab.IsMouseInsideRect(mousePosition))
			{
				result = ab.Name;
			}
		}
		return result;
	} 
	
	public bool IsMouseOverRect(Rect rect)
	{
		Rect newRect = new Rect(rect.x	 * GUIScale.widthScale, rect.y * GUIScale.heightScale, rect.width * GUIScale.widthScale, rect.height * GUIScale.heightScale);
		foreach(ActiveGUIControl ab in activeControls)
		{
			if (newRect == ab.ControlRect && ab.IsMouseInsideRect(Input.mousePosition))
			{	
				return true;
			}
		}
		return false;
	}
	
	//-----------------------------------------------------------------
	//TextField
	//-----------------------------------------------------------------
	public string TextField(int x, int y, int width, int height, string textFieldValue)
	{
		Rect rect = new Rect(x * GUIScale.widthScale, y * GUIScale.heightScale, width * GUIScale.widthScale, height * GUIScale.heightScale);
		return GUI.TextField(rect, textFieldValue);
	}
	
	public string TextField(Rect rect, string textFieldValue)
	{
		Rect newRect = new Rect(rect.x * GUIScale.widthScale, rect.y * GUIScale.heightScale, rect.width * GUIScale.widthScale, rect.height * GUIScale.heightScale);
        return GUI.TextField(newRect, textFieldValue, Skin.textField);
	}

    //-----------------------------------------------------------------
    //Label
    //-----------------------------------------------------------------
    public void Label(int x, int y, int width, int height, string labelText)
    {
        Rect rect = new Rect(x * GUIScale.widthScale, y * GUIScale.heightScale, width * GUIScale.widthScale, height * GUIScale.heightScale);
        GUI.Label(rect, labelText, Skin.label);
    }

    public void Label(int x, int y, int width, int height, string labelText, Color color)
    {
        Rect rect = new Rect(x * GUIScale.widthScale, y * GUIScale.heightScale, width * GUIScale.widthScale, height * GUIScale.heightScale);
        Color defaultColor = Skin.label.normal.textColor;

        Skin.label.normal.textColor = color;
        GUI.Label(rect, labelText, Skin.label);
        Skin.label.normal.textColor = defaultColor;
    }

    public void Label(int x, int y, int width, int height, string labelText, GUIStyle style)
    {
        Rect rect = new Rect(x * GUIScale.widthScale, y * GUIScale.heightScale, width * GUIScale.widthScale, height * GUIScale.heightScale);
        GUI.Label(rect, labelText, style);
    }

    public void Label(Rect rect, string labelText)
    {
        Rect newRect = new Rect(rect.x * GUIScale.widthScale, rect.y * GUIScale.heightScale, rect.width * GUIScale.widthScale, rect.height * GUIScale.heightScale);
        GUI.Label(newRect, labelText, Skin.label);
    }

    public void Label(Rect rect, string labelText, GUIStyle style)
    {
        Rect newRect = new Rect(rect.x * GUIScale.widthScale, rect.y * GUIScale.heightScale, rect.width * GUIScale.widthScale, rect.height * GUIScale.heightScale);
        GUI.Label(newRect, labelText, style);
    }
	
	//-----------------------------------------------------------------
	//Texture
	//-----------------------------------------------------------------
	public void DrawTexture(float x, float y, Texture2D texture)
	{
		Rect rect = new Rect(x * GUIScale.widthScale, y * GUIScale.heightScale, texture.width * GUIScale.widthScale, texture.height * GUIScale.heightScale);
		GUI.DrawTexture(rect, texture);
	}
	
	public void DrawTexture(float x, float y, float width, float height, Texture2D texture)
	{
		Rect rect = new Rect(x * GUIScale.widthScale, y * GUIScale.heightScale, width * GUIScale.widthScale, height * GUIScale.heightScale);
		GUI.DrawTexture(rect, texture);
	}
	
	//-----------------------------------------------------------------
	//BeginGroup
	//-----------------------------------------------------------------
	public void BeginGroup(int x, int y, int width, int height)
	{
		BeginGroup(x, y, width, height, string.Empty);
	}
	
	public void BeginGroup(int x, int y, int width, int height, string caption)
	{
		Rect rect = new Rect(x * GUIScale.widthScale, y * GUIScale.heightScale, width * GUIScale.widthScale, height * GUIScale.heightScale);
		
		GUI.BeginGroup(rect, caption, Skin.box);
	}
}

public enum GUISound
{
	Gold,
	SkillUp,
	Repair,
	EquipWeapon,
	EquipArmor
}

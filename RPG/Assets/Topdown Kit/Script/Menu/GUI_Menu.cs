/// <summary>
/// GUI_Menu.
/// This is a main script GUI(Status Window,Inventory Window,Equipment Window,Skill Window)
/// </summary>

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GUI_Menu : MonoBehaviour {
	
	[System.Serializable]
	public class KeyOpenWindow{
		public KeyCode keyOpenFirst;
		public KeyCode keyOenSecound;
	}
	
	[System.Serializable]
	public class Bag{
		public int item_id;
		public int item_amount;
		public Item_Data.Equipment_Type equipmentType;
		public int linkIndex;
		public bool isItem;
		public TypeSkillHover typeSkill;
	}
	
	[System.Serializable]
	public class SlotEquipment{
		public int item_id;	
		public Item_Data.Equipment_Type equipmentType;
	}
	
	[System.Serializable]
	public class SlotEquiptRect{
		public Item_Data.Equipment_Type equipType;
		public Rect rect;
	}
	
	[System.Serializable]
	public class Inventory{
		public string nameWindow; // Window name
		public int windowInventoryID;
		public int windowDropItemID;
		public int row;// row of item slot
		public int colum;// colum of item slot
		public Rect windowInventoryRect; // Window position and size
		public Rect gridRect;// Item slot position and size
		public Rect itemAmountRect;// Text amount item position and size
		public Rect windowDropRect;
		public Rect windowDropAmount;
		public Rect btnEnterDropAmount;
		public Rect goldRect;
		public Rect coinIconRect;
		public bool openInventory;// switch inventory
		
		[HideInInspector] public bool enterDrop;
	}
	
	[System.Serializable]
	public class Equipment{
		public string nameEquipmentWindow;
		public int windowEquipmentID;
		public Rect windowEquipmentRect;
		public Rect windowDescriptionRect;
		public SlotEquiptRect[] slotEquipmentRect;
		public bool openEquipment;// switch Equipment
	}
	
	[System.Serializable]
	public class SkillWindow{
		public string nameSkillWindow;
		public int windowSkillID;
		public Rect windowSkillRect;
		public Rect scrollViewPosition;
		public Rect scrollViewRect;
		public Rect slotSkillRect;
		public Rect descriptionRect;
		public Rect skillNameRect;
		public Rect skillTypeRect;
		public Rect skillManaUseRect;
		public Rect skillDescriptionRect;
		
		public bool openHittestDebug;
		public Vector2 offset;
		public float positionTunerHittest; //Use tune position for Hittest mouse
		public float space;
		public float spaceSkillName;
		public float spaceSkillType;
		public float spaceSkillManaUse;
		public float spaceDescription;
		public bool openSkillWindow;
		public Texture2D lockSkillTexture;
		
	}
	
	[System.Serializable]
	public class HotKeySlot{
		//[HideInInspector]
		public List<Bag> hotKeyslot = new List<Bag>();
		public List<KeyCode> input_key = new List<KeyCode>();
		public bool isItem;
		public Rect hot_key_Rect;
		public float space_Slot;
		public int amountSlot;
	}
	
	[System.Serializable]
	public class ItemShop{
		public string nameItemShopWindow;
		public int windowItemShopID;
		public Rect windowItemShopRect;
		
		public Rect scrollViewPosition;
		public Rect scrollViewRect;
		public Rect slotItemRect;
		public Rect descriptionRect;
		public Rect priceRect;
		public Rect buttonBuyRect;
		public Rect buttonCancelRect;
		public Rect itemNameRect;
		public Rect itemTypeRect;
		public Rect itemDescriptionRect;
		public Rect moneyRect;
		public Rect coinIconRect;
		public Vector2 offset;
		public float space;
		
		public float spacePrice;
		public float spaceButtonBuy;
		public float spaceItemName;
		public float spaceItemType;
		public float spaceItemDescription;
		public float valueTuningIconFollowMouse;
		
		public bool openItemShop;
		public List<int> itemID = new List<int>();
	}
	
	[System.Serializable]
	public class StatWindow{
		public string nameStatWindow;
		public int windowStatID;
		public Rect windowStatRect;
		
		public Rect AttackHeadRect;
		public Rect DefHeadRect;
		public Rect SpeedHeadRect;
		public Rect HitHeadRect;
		
		public Rect AttackValRect;
		public Rect DefValRect;
		public Rect SpeedValRect;
		public Rect HitValRect;
		
		public Rect PointStatRect;
		public Rect HeadSummaryRect;
		public Rect SummaryStatRectLeft;
		public Rect SummaryStatRectRight;
		
		public Rect BtnApplyRect;
		public Rect BtnResetRect;
		
		public Rect BtnAddStatAttack;
		public Rect BtnAddStatDef;
		public Rect BtnAddStatSpeed;
		public Rect BtnAddStatHit;
		
		public bool openStatWindow;
		
		[HideInInspector] public int valAtk, valDef, valSpeed, valHit;
		[HideInInspector] public int defAtk, defDef, defSpeed, defHit;
		[HideInInspector] public int defPoint;
	}
	
	[System.Serializable]
	public class GUIStyleGroup{
		public GUIStyle windowInventoryStyle;// guistyle for Inventory
		public GUIStyle gridStyle;// guistyle for item slot
		public GUIStyle textAmountItemStyle;// guistyle for text amount item
		public GUIStyle windowDropStyle;
		public GUIStyle goldStyle;
		public GUIStyle descriptionStyle;
		public GUIStyle hotkeyStyle;
		public GUIStyle btnEnterAmount;
		
		public GUIStyle windowEquipmentStyle;
		
		
		public GUIStyle textLvUnlockStyle;
		public GUIStyle windowSkillStyle;
		public GUIStyle skillSlotStyle;
		public GUIStyle descriptionSkillSlotStyle;
		public GUIStyle skillNameStyle;
		public GUIStyle skillTypeStyle;
		public GUIStyle skillManaUseStyle;
		public GUIStyle skillDescriptionStyle;
		
		public GUIStyle windowItemShopStyle;
		public GUIStyle itemShopCancel;
		public GUIStyle itemShopSlotStyle;
		public GUIStyle descriptionItemShopStyle;
		public GUIStyle itemShopPriceStyle;
		public GUIStyle itemShopButtonBuyStyle;
		public GUIStyle itemShopItemNameStyle;
		public GUIStyle itemShopItemTypeStyle;
		public GUIStyle itemShopDescriptionStyle;
		
		public GUIStyle windowStatStyle;
		public GUIStyle textStatStyle;
		public GUIStyle textPointRemainStyle;
		public GUIStyle textHeadSummaryStyle;
		public GUIStyle textSummaryStyle;
		public GUIStyle btnApplyStyle;
		public GUIStyle btnResetStyle;
		public GUIStyle btnAddStatStyle;
	}
	
	public int startMoney;
	
	public Inventory[] inventory;
	
	public Equipment[] equipment;
	
	public HotKeySlot[] hotKeySlot;
	
	public SkillWindow[] skillWindow;
	
	public ItemShop[] itemShop;
	
	public StatWindow[] statWindow;
	
	public KeyOpenWindow keyOpenInventory; //fix key open window in your game
	public KeyOpenWindow keyOpenEquipment; //fix key open window in your game
	public KeyOpenWindow keyOpenSkill;
	public KeyOpenWindow keyOpenStat;
	
	public bool useStyle;// for use guistyle
	
	public GUIStyleGroup[] guiStyleGroup;
	
	public Texture2D null_Item_Texture;// texture for null item slot
	public Texture2D coinIcon;
	[HideInInspector]
	public Texture2D imgDebugCollision;
	
	public float rangeDetect; //area detect pickup item and open shop
	
	public GameObject item_Obj_Pref;// gameobject item prefab
	[HideInInspector]
	public List<Bag> bag_item = new List<Bag>();// get item id
	[HideInInspector]
	public List<SlotEquipment> slotEquipment_item = new List<SlotEquipment>();
	[HideInInspector]
	public string hover;
	[HideInInspector]
	public string amountItemDrop;
	
	private PlayerSkill playerSkill;
	private HeroController playerController;
	private PlayerStatus playerStatus;
	
	private Ray ray;
	private RaycastHit hit;
	private bool pickItem;
	private bool pickSkill;
	private bool hoverSlot;
	private bool pickup;
	private bool pickupEquipment;
	private bool pickupHotkey;
	private bool pickupStay;
	private bool showDropAmount;
	private bool isItemFormInventory;
	private bool isBuyItem;
	private float countPickup;

	public enum TypeSkillHover{
		Null = 0, Passive = 1, Buff = 2, Attack = 3	
	}
	
	[HideInInspector]
	public TypeSkillHover typeSkillHover;
	[HideInInspector]
	public Texture2D itemImg_Pickup;
	[HideInInspector]
	public int indexInventory;
	[HideInInspector]
	public int indexEquipment;
	[HideInInspector]
	public int indexHotKey;
	[HideInInspector]
	public int id_Hover;
	private int money;
	[HideInInspector]
	public Bag item_pickup;
	
	public static GUI_Menu instance;
	
	void Start(){
		//Add Money 
		money = startMoney;
		
		GameObject rootObject = transform.parent.gameObject;
		playerSkill = rootObject.GetComponent<PlayerSkill>();
		playerController = rootObject.GetComponent<HeroController>();
		playerStatus = rootObject.GetComponent<PlayerStatus>();
		
		instance = this;
		indexHotKey = -1;
		int amountIndex = inventory[0].row * inventory[0].colum;
		bag_item.Clear();
		for(int i = 0; i < amountIndex+1; i++){
			bag_item.Add(new Bag());
			bag_item[i].linkIndex = -1;
		}
		
		for(int i = 0; i < equipment[0].slotEquipmentRect.Length; i++){
			slotEquipment_item.Add(new SlotEquipment());
		}
		
		for(int i = 0; i < hotKeySlot[0].amountSlot; i++){
			hotKeySlot[0].hotKeyslot.Add(new Bag());
			hotKeySlot[0].hotKeyslot[i].linkIndex = -1;
		}
	}
	
	/// <summary>
	/// Raises the GU event.
	/// </summary>
	public void OnGUI(){
		LinkBagToHotkey();
		ShowHotkey();
		InventoryWindow();
		EquipmentWindow();
		Skill_Window();
		ItemShopWindow();
		Stat_Window();
		
		CheckSelectWindow();
		
		CheckSlotSkill();
		
		ShowDescriptionOverAll();
	}
	
	/// <summary>
	/// Update this instance.
	/// </summary>
	private void Update(){
		
		OpenInventoryWindow();
		OpenEquipmentWindow();
		OpenSkillWindow();
		OpenStatWindow();
		UseHotKey();
		
		CheckDontMove();
		
		RayPickupItem();
	}
	
	private void CheckDontMove(){
		if(itemShop[0].openItemShop){
			playerController.dontMove = true;
		}else{
			if(CheckHoverEquipment() || CheckHoverInventory() || CheckHoverItemShop() || CheckHoverSkillWindow() || CheckHoverSlotHotkey() != -1
				|| CheckHoverStatWindow() || pickupStay == true){
				playerController.dontClick = true;	
			}else{
				playerController.dontClick = false;	
			}
			playerController.dontMove = false;
		}
	}
	
	private void Stat_Window(){
		ShowStat();	
	}
	
	private void Skill_Window(){
		ShowSkillWindow();	
	}
	
	/// <summary>
	/// Equipments the window.
	/// </summary>
	private void EquipmentWindow(){
		ShowEquipment();
	}
	
	/// <summary>
	/// Inventories the window.
	/// </summary>
	private void InventoryWindow(){
		
		ShowWindowDropAmount();
		
		ShowInventory();
		
		PickUpstay();
	}
	
	private void ItemShopWindow(){
		ShowItemShopWindow();	
	}
	
	private void ShowDescriptionOverAll(){
		if(CheckHoverSlotHotkey() == -1){
			ShowDescriptionBag(new Rect(0,0,0,0), id_Hover);
		}
	}
	
	private void LinkBagToHotkey(){
		for(int i = 0; i < hotKeySlot[0].hotKeyslot.Count; i++){
			if(hotKeySlot[0].hotKeyslot[i].linkIndex != -1){
				if(hotKeySlot[0].hotKeyslot[i].isItem == true){
					hotKeySlot[0].hotKeyslot[i].item_id = bag_item[hotKeySlot[0].hotKeyslot[i].linkIndex].item_id;
					hotKeySlot[0].hotKeyslot[i].item_amount = bag_item[hotKeySlot[0].hotKeyslot[i].linkIndex].item_amount;
					hotKeySlot[0].hotKeyslot[i].equipmentType = bag_item[hotKeySlot[0].hotKeyslot[i].linkIndex].equipmentType;
					if(hotKeySlot[0].hotKeyslot[i].item_amount <= 0){
						hotKeySlot[0].hotKeyslot[i].linkIndex = -1;
					}
				}
			}
		}
		
		for(int i = 0; i < bag_item.Count; i++){
			if(bag_item[i].linkIndex != -1){
				bag_item[i].item_id = hotKeySlot[0].hotKeyslot[bag_item[i].linkIndex].item_id;
				bag_item[i].item_amount = hotKeySlot[0].hotKeyslot[bag_item[i].linkIndex].item_amount;
				bag_item[i].equipmentType = hotKeySlot[0].hotKeyslot[bag_item[i].linkIndex].equipmentType;
				if(bag_item[i].item_amount <= 0){
					bag_item[i].linkIndex = -1;
				}
			}
		}
	}
	
	/// <summary>
	/// Shows the description bag.
	/// </summary>
	/// <param name='rect'>
	/// Rect.
	/// </param>
	/// <param name='_item_ID'>
	/// _item_ I.
	/// </param>
	private void ShowDescriptionBag(Rect rect, int _item_ID){
		if(Item_Data.instance.Get_Item(_item_ID) != null && hoverSlot == true){
			Rect rectCheck = new Rect(0,0,0,0);
			if(useStyle == false){
				if(rectCheck != rect)
					GUI.Box(new Rect(Input.mousePosition.x - rect.x,  (Screen.height - Input.mousePosition.y) - rect.y, equipment[0].windowDescriptionRect.width, equipment[0].windowDescriptionRect.height),Item_Data.instance.Get_Item(_item_ID).item_Name+"\n"+Item_Data.instance.Get_Item(_item_ID).description+"\n"+"Sell price: "+ (Item_Data.instance.Get_Item(_item_ID).price/2));
				else
					GUI.Box(new Rect(Input.mousePosition.x ,  (Screen.height - Input.mousePosition.y), equipment[0].windowDescriptionRect.width, equipment[0].windowDescriptionRect.height),Item_Data.instance.Get_Item(_item_ID).item_Name+"\n"+Item_Data.instance.Get_Item(_item_ID).description+"\n"+ "Sell price: "+(Item_Data.instance.Get_Item(_item_ID).price/2));	
			}else{
				if(rectCheck != rect)
					GUI.Box(new Rect(Input.mousePosition.x - rect.x,  (Screen.height - Input.mousePosition.y) - rect.y, equipment[0].windowDescriptionRect.width, equipment[0].windowDescriptionRect.height),Item_Data.instance.Get_Item(_item_ID).item_Name+"\n"+Item_Data.instance.Get_Item(_item_ID).description+"\n"+"Sell price: "+(Item_Data.instance.Get_Item(_item_ID).price/2), guiStyleGroup[0].descriptionStyle);
				else
					GUI.Box(new Rect(Input.mousePosition.x ,  (Screen.height - Input.mousePosition.y), equipment[0].windowDescriptionRect.width, equipment[0].windowDescriptionRect.height),Item_Data.instance.Get_Item(_item_ID).item_Name+"\n"+Item_Data.instance.Get_Item(_item_ID).description+"\n"+ "Sell price: "+(Item_Data.instance.Get_Item(_item_ID).price/2), guiStyleGroup[0].descriptionStyle);	
			}
			
		}
	}
	
	private void UseHotKey(){
		for(int i = 0; i < hotKeySlot[0].amountSlot; i++){
			if(Input.GetKeyUp(hotKeySlot[0].input_key[i]) && hotKeySlot[0].hotKeyslot[i].linkIndex != -1 && hotKeySlot[0].hotKeyslot[i].isItem == true){
				indexInventory = hotKeySlot[0].hotKeyslot[i].linkIndex;
				Use_Item(bag_item[hotKeySlot[0].hotKeyslot[i].linkIndex].item_id);
			}else if(Input.GetKeyUp(hotKeySlot[0].input_key[i]) && hotKeySlot[0].hotKeyslot[i].isItem == false && hotKeySlot[0].hotKeyslot[i].item_id != 0){
				playerController.GetCastID(hotKeySlot[0].hotKeyslot[i].item_id);
			}
		}
	}
	
	/// <summary>
	/// Checks the select inventory.
	/// </summary>
	private void CheckSelectWindow(){
		if(CheckHoverInventory()){
			for(int i = 0; i < inventory[0].row; i++){
				for(int j = 0; j < inventory[0].colum; j++){
					int index = i+(j*inventory[0].row);
					if(hover == ""+(index).ToString()){
						
						hoverSlot = true;
						
						//Check Hover Inventory and Show Description
						id_Hover = bag_item[index].item_id;
						
						if(Input.GetMouseButtonDown(0)){
							pickup = true;
							pickItem = true;
							isItemFormInventory = true;
						}
						
						if(Input.GetMouseButtonDown(1)){
							indexInventory = index;
							Use_Item(bag_item[indexInventory].item_id);
						}
						
						if(pickup){
							if(Input.GetMouseButton(0)){
								countPickup += Time.deltaTime;
								if(countPickup >= 0.1f && showDropAmount == false && pickupStay == false){
									if(Item_Data.instance.Get_Item(bag_item[index].item_id) != null){
										itemImg_Pickup = Item_Data.instance.Get_Item(bag_item[index].item_id).item_Img;
									}
									indexInventory = index;
									item_pickup = bag_item[indexInventory];
									pickupStay = true;
								}
							}
						}
					}
				}
			}
		}else if(CheckHoverEquipment()){
			if(CheckSlotEquipment() != -1){
				hoverSlot = true;	
			}else{
				hoverSlot = false;	
			}
			
			if(Input.GetMouseButtonDown(0)){
				if(CheckSlotEquipment() != -1){
						if(Item_Data.instance.Get_Item(slotEquipment_item[CheckSlotEquipment()].item_id) != null){
							itemImg_Pickup = Item_Data.instance.Get_Item(slotEquipment_item[CheckSlotEquipment()].item_id).item_Img;
						}
						item_pickup = new Bag();
						item_pickup.item_id = slotEquipment_item[CheckSlotEquipment()].item_id;
						item_pickup.item_amount = 1;
						item_pickup.equipmentType = slotEquipment_item[CheckSlotEquipment()].equipmentType;
						indexEquipment = CheckSlotEquipment();
						pickupEquipment = true;	
						pickupStay = true;
						pickItem = true;
					}
			}
			
			for(int i = 0; i < equipment[0].slotEquipmentRect.Length; i++){
				if(hover == "slotEquipment"+i.ToString()){
					id_Hover = slotEquipment_item[i].item_id;
					
					if(Input.GetMouseButtonDown(1)){
						if(slotEquipment_item[i].item_id != 0){
							Item_Obj item_obj = new Item_Obj();
							item_obj.item_id = slotEquipment_item[i].item_id;
							item_obj.item_amount = 1;
							item_obj.euipmentType = slotEquipment_item[i].equipmentType;
							RemoveStatEquipmentToPlayer(Item_Data.instance.Get_Item(slotEquipment_item[i].item_id));
							Pickup_Item(item_obj);
							
							slotEquipment_item[i].item_id = 0;
							slotEquipment_item[i].equipmentType = Item_Data.Equipment_Type.Null;
						}
					}
				}
			}
		}else if(CheckHoverItemShop()){
			if(CheckSlotShop() != -1){
				hoverSlot = true;
				id_Hover = itemShop[0].itemID[CheckSlotShop()];
			}else{
				hoverSlot = false;
			}
		}else{
			if(CheckHoverSlotHotkey() != -1){
				hoverSlot = true;
				//Do somethin for hotkey
				if(Input.GetMouseButtonDown(0)){
					if(hotKeySlot[0].hotKeyslot[CheckHoverSlotHotkey()].isItem == true){
						if(Item_Data.instance.Get_Item(hotKeySlot[0].hotKeyslot[CheckHoverSlotHotkey()].item_id) != null){
							itemImg_Pickup = Item_Data.instance.Get_Item(hotKeySlot[0].hotKeyslot[CheckHoverSlotHotkey()].item_id).item_Img;
						}
						item_pickup = new Bag();
						item_pickup.item_id = hotKeySlot[0].hotKeyslot[CheckHoverSlotHotkey()].item_id;
						item_pickup.item_amount = hotKeySlot[0].hotKeyslot[CheckHoverSlotHotkey()].item_amount;
						item_pickup.equipmentType = hotKeySlot[0].hotKeyslot[CheckHoverSlotHotkey()].equipmentType;
						indexHotKey = CheckHoverSlotHotkey();
						pickupHotkey = true;	
						pickupStay = true;
						pickItem = true;
					}else{
						pickSkill = true;
						item_pickup = new Bag();
						pickupHotkey = true;
						indexHotKey = CheckHoverSlotHotkey();
						
						if(hotKeySlot[0].hotKeyslot[CheckHoverSlotHotkey()].typeSkill == TypeSkillHover.Buff){
							itemImg_Pickup = playerSkill.activeSkillBuff[playerSkill.FindSkillIndex(hotKeySlot[0].hotKeyslot[CheckHoverSlotHotkey()].item_id)].skillIcon;
							item_pickup.item_id = playerSkill.activeSkillBuff[playerSkill.FindSkillIndex(hotKeySlot[0].hotKeyslot[CheckHoverSlotHotkey()].item_id)].skillID;
							item_pickup.typeSkill = TypeSkillHover.Buff;
						}else if(hotKeySlot[0].hotKeyslot[CheckHoverSlotHotkey()].typeSkill == TypeSkillHover.Attack){
							itemImg_Pickup = playerSkill.activeSkillAttack[playerSkill.FindSkillIndex(hotKeySlot[0].hotKeyslot[CheckHoverSlotHotkey()].item_id)].skillIcon;
							item_pickup.item_id = playerSkill.activeSkillAttack[playerSkill.FindSkillIndex(hotKeySlot[0].hotKeyslot[CheckHoverSlotHotkey()].item_id)].skillID;
							item_pickup.typeSkill = TypeSkillHover.Attack;
						}
						pickupStay = true;
					}
				}
			}else{
				hoverSlot = false;	
			}
			
			if(CheckSlotSkill() != -1 && skillWindow[0].openSkillWindow){
				if(Input.GetMouseButtonDown(0)){
					pickSkill = true;
					item_pickup = new Bag();
					
					if(typeSkillHover == TypeSkillHover.Buff){
						if(playerStatus.status.lv >= playerSkill.activeSkillBuff[CheckSlotSkill()-playerSkill.passiveSkill.Count].unlockLevel){
							itemImg_Pickup = playerSkill.activeSkillBuff[CheckSlotSkill()-playerSkill.passiveSkill.Count].skillIcon;
							item_pickup.item_id = playerSkill.activeSkillBuff[CheckSlotSkill()-playerSkill.passiveSkill.Count].skillID;
							item_pickup.typeSkill = TypeSkillHover.Buff;
						}
					}else if(typeSkillHover == TypeSkillHover.Attack){
						if(playerStatus.status.lv >= playerSkill.activeSkillAttack[CheckSlotSkill()-playerSkill.passiveSkill.Count-playerSkill.activeSkillBuff.Count].unlockLevel){
							itemImg_Pickup = playerSkill.activeSkillAttack[CheckSlotSkill()-playerSkill.passiveSkill.Count-playerSkill.activeSkillBuff.Count].skillIcon;	
							item_pickup.item_id = playerSkill.activeSkillAttack[CheckSlotSkill()-playerSkill.passiveSkill.Count-playerSkill.activeSkillBuff.Count].skillID;
							item_pickup.typeSkill = TypeSkillHover.Attack;
						}
					}
					pickupStay = true;
				}
			}
		}
	}
	
	/// <summary>
	/// Picks item stay and drop item.
	/// </summary>
	private void PickUpstay(){
		if(pickupStay){
			Vector3 mousePos = Input.mousePosition;
			
			if(itemImg_Pickup != null){
				if(CheckHoverSkillWindow() == false){
					GUI.DrawTexture(new Rect(mousePos.x-20, (Screen.height - mousePos.y)-20, 40,40),itemImg_Pickup);	
				}
			}
			
			
			if(Input.GetMouseButtonUp(0)){
				if(itemImg_Pickup != null && showDropAmount == false){
					if(!CheckHoverInventory() && !CheckHoverEquipment() && !CheckHoverItemShop()){
						if(CheckHoverSlotHotkey() == -1){
							if(pickupHotkey == false){
								//Drop Item on ground
								if(item_pickup.item_amount > 1){
									inventory[0].windowDropRect.x = Screen.width*0.5f - (inventory[0].windowDropRect.width*0.5f);
									inventory[0].windowDropRect.y = Screen.height*0.5f - (inventory[0].windowDropRect.height*0.5f);
									//amountItemDrop = "1";
									showDropAmount = true;
								}else{
									if(pickupEquipment == false && pickupHotkey == false && pickSkill == false){
										Drop_Item(indexInventory, transform.position+(Vector3.up*1.5f), 1);
										item_pickup = null;
									}
								}
							}else{
								//Drop item and skill form hotkey
								if(indexHotKey != -1 && hotKeySlot[0].hotKeyslot[indexHotKey].linkIndex != -1 && hotKeySlot[0].hotKeyslot[indexHotKey].isItem == true){
									bag_item[hotKeySlot[0].hotKeyslot[indexHotKey].linkIndex].linkIndex = -1;
									hotKeySlot[0].hotKeyslot[indexHotKey].linkIndex = -1;
									hotKeySlot[0].hotKeyslot[indexHotKey].item_id = 0;
									hotKeySlot[0].hotKeyslot[indexHotKey].item_amount = 0;
									hotKeySlot[0].hotKeyslot[indexHotKey].equipmentType = Item_Data.Equipment_Type.Null;
									pickupHotkey = false;
									indexHotKey = -1;
								}else if(indexHotKey != -1 && hotKeySlot[0].hotKeyslot[indexHotKey].isItem == false){
									hotKeySlot[0].hotKeyslot[indexHotKey].linkIndex = -1;
									hotKeySlot[0].hotKeyslot[indexHotKey].item_id = 0;
									hotKeySlot[0].hotKeyslot[indexHotKey].item_amount = 0;
									hotKeySlot[0].hotKeyslot[indexHotKey].equipmentType = Item_Data.Equipment_Type.Null;
									hotKeySlot[0].hotKeyslot[indexHotKey].typeSkill = TypeSkillHover.Null;
									pickupHotkey = false;
									indexHotKey = -1;
								}
							}
						}else{
							//Drop item and skill to hotkey
							if(pickupHotkey == false && pickupEquipment == false && pickItem == true){
								if(hotKeySlot[0].hotKeyslot[CheckHoverSlotHotkey()].linkIndex == -1){
									hotKeySlot[0].hotKeyslot[CheckHoverSlotHotkey()].linkIndex = indexInventory;
									bag_item[indexInventory].linkIndex = CheckHoverSlotHotkey();
									hotKeySlot[0].hotKeyslot[CheckHoverSlotHotkey()].isItem = true;
								}
								pickItem = false;
							}else if(pickSkill == true && pickupHotkey == false){
								if(hotKeySlot[0].hotKeyslot[CheckHoverSlotHotkey()].linkIndex != -1){
									bag_item[hotKeySlot[0].hotKeyslot[CheckHoverSlotHotkey()].linkIndex].linkIndex = -1;
								}
								hotKeySlot[0].hotKeyslot[CheckHoverSlotHotkey()].item_id = item_pickup.item_id;
								hotKeySlot[0].hotKeyslot[CheckHoverSlotHotkey()].isItem = false;
								hotKeySlot[0].hotKeyslot[CheckHoverSlotHotkey()].typeSkill = item_pickup.typeSkill;
								pickSkill = false;
							}else if(pickSkill == true && pickupHotkey == true){
								if(hotKeySlot[0].hotKeyslot[CheckHoverSlotHotkey()].linkIndex != -1){
									bag_item[hotKeySlot[0].hotKeyslot[CheckHoverSlotHotkey()].linkIndex].linkIndex = -1;
								}
								hotKeySlot[0].hotKeyslot[CheckHoverSlotHotkey()].item_id = item_pickup.item_id;
								hotKeySlot[0].hotKeyslot[CheckHoverSlotHotkey()].isItem = false;
								hotKeySlot[0].hotKeyslot[CheckHoverSlotHotkey()].typeSkill = item_pickup.typeSkill;
								
								hotKeySlot[0].hotKeyslot[indexHotKey].linkIndex = -1;
								hotKeySlot[0].hotKeyslot[indexHotKey].item_id = 0;
								hotKeySlot[0].hotKeyslot[indexHotKey].item_amount = 0;
								hotKeySlot[0].hotKeyslot[indexHotKey].equipmentType = Item_Data.Equipment_Type.Null;
								hotKeySlot[0].hotKeyslot[indexHotKey].typeSkill = TypeSkillHover.Null;
								
								pickupHotkey = false;
								indexHotKey = -1;
								pickSkill = false;
							}else if(pickupHotkey == true && pickItem == true){
								
								if(indexHotKey != CheckHoverSlotHotkey()){
									hotKeySlot[0].hotKeyslot[CheckHoverSlotHotkey()].linkIndex = hotKeySlot[0].hotKeyslot[indexHotKey].linkIndex;
									bag_item[hotKeySlot[0].hotKeyslot[indexHotKey].linkIndex].linkIndex = CheckHoverSlotHotkey();
									hotKeySlot[0].hotKeyslot[CheckHoverSlotHotkey()].isItem = true;
										
										
									hotKeySlot[0].hotKeyslot[indexHotKey].linkIndex = -1;
									hotKeySlot[0].hotKeyslot[indexHotKey].item_id = 0;
									hotKeySlot[0].hotKeyslot[indexHotKey].item_amount = 0;
									hotKeySlot[0].hotKeyslot[indexHotKey].equipmentType = Item_Data.Equipment_Type.Null;
									hotKeySlot[0].hotKeyslot[indexHotKey].typeSkill = TypeSkillHover.Null;
								}
								
								pickItem = false;
								pickupHotkey = false;
							}
						}
						
					}else{
						if(CheckHoverEquipment() && pickupEquipment == false && pickupHotkey == false){
							//put item from Inventory slot to Equipment slot 
							if(CheckSlotEquipment() != -1){
								if(equipment[0].slotEquipmentRect[CheckSlotEquipment()].equipType == item_pickup.equipmentType){
									if(slotEquipment_item[CheckSlotEquipment()].item_id == 0){
										slotEquipment_item[CheckSlotEquipment()].item_id = item_pickup.item_id;
										slotEquipment_item[CheckSlotEquipment()].equipmentType = item_pickup.equipmentType;
										bag_item[indexInventory].item_id = 0;
										bag_item[indexInventory].item_amount = 0;
										bag_item[indexInventory].equipmentType = Item_Data.Equipment_Type.Null;
									}else{
										Item_Obj _item_obj = new Item_Obj();
										_item_obj.item_id = slotEquipment_item[CheckSlotEquipment()].item_id;
										_item_obj.euipmentType = slotEquipment_item[CheckSlotEquipment()].equipmentType;
										_item_obj.item_amount = 1;
										
										RemoveStatEquipmentToPlayer(Item_Data.instance.Get_Item(slotEquipment_item[CheckSlotEquipment()].item_id));
										
										slotEquipment_item[CheckSlotEquipment()].item_id = item_pickup.item_id;
										slotEquipment_item[CheckSlotEquipment()].equipmentType = item_pickup.equipmentType;
										
										bag_item[indexInventory].item_id = 0;
										bag_item[indexInventory].item_amount = 0;
										bag_item[indexInventory].equipmentType = Item_Data.Equipment_Type.Null;
										
										Pickup_Item(_item_obj);
									}
								}
								item_pickup = null;
							}
							
						}else if(CheckHoverInventory() && pickupEquipment){
							//put item form Equipment slot to Inventory slot
							Item_Obj _item_obj = new Item_Obj();
							_item_obj.item_id = item_pickup.item_id;
							_item_obj.item_amount = item_pickup.item_amount;
							_item_obj.euipmentType = item_pickup.equipmentType;
							RemoveStatEquipmentToPlayer(Item_Data.instance.Get_Item(item_pickup.item_id));
							Pickup_Item(_item_obj);
							if(indexEquipment != -1){
								slotEquipment_item[indexEquipment].item_id = 0;	
								slotEquipment_item[indexEquipment].equipmentType = Item_Data.Equipment_Type.Null;
							}
							
							item_pickup = null;
						}else if(CheckHoverItemShop() && isItemFormInventory){
							//Drop_Item(indexInventory, , 1);
							//amountItemDrop = "1";
							showDropAmount = true;
						}
					}
				}
				pickup = false;
				pickupEquipment = false;
				pickupStay = false;
				countPickup = 0;
				itemImg_Pickup = null;
				pickSkill = false;
				pickItem = false;
			}
		}
	}
	
	private void ShowStat(){
		if(statWindow[0].openStatWindow){
			if(useStyle){
				statWindow[0].windowStatRect = GUI.Window(statWindow[0].windowStatID, statWindow[0].windowStatRect, FunctionStatWindow, statWindow[0].nameStatWindow, guiStyleGroup[0].windowStatStyle);
			}else{
				statWindow[0].windowStatRect = GUI.Window(statWindow[0].windowStatID, statWindow[0].windowStatRect, FunctionStatWindow, statWindow[0].nameStatWindow);
			}
		}
	}
	
	/// <summary>
	/// Shows the hotkey.
	/// </summary>
	private void ShowHotkey(){
		for(int i = 0; i < hotKeySlot[0].amountSlot; i++){
			if(useStyle){
				GUI.Box(GUI_Calculate.RectWithScrren_WidthAndHeight_SizeWidth(new Vector2(hotKeySlot[0].hot_key_Rect.x + ((hotKeySlot[0].hot_key_Rect.width + hotKeySlot[0].space_Slot) * i), hotKeySlot[0].hot_key_Rect.y), new Vector2(hotKeySlot[0].hot_key_Rect.width, hotKeySlot[0].hot_key_Rect.height)),"",guiStyleGroup[0].hotkeyStyle);
				
			}else{
				GUI.Box(GUI_Calculate.RectWithScrren_WidthAndHeight_SizeWidth(new Vector2(hotKeySlot[0].hot_key_Rect.x + ((hotKeySlot[0].hot_key_Rect.width + hotKeySlot[0].space_Slot) * i), hotKeySlot[0].hot_key_Rect.y), new Vector2(hotKeySlot[0].hot_key_Rect.width, hotKeySlot[0].hot_key_Rect.height)),"");
			}
			
			if(hotKeySlot[0].hotKeyslot[i].item_id == 0){
				GUI.DrawTexture(GUI_Calculate.RectWithScrren_WidthAndHeight_SizeWidth(new Vector2(hotKeySlot[0].hot_key_Rect.x + ((hotKeySlot[0].hot_key_Rect.width + hotKeySlot[0].space_Slot) * i), hotKeySlot[0].hot_key_Rect.y), new Vector2(hotKeySlot[0].hot_key_Rect.width, hotKeySlot[0].hot_key_Rect.height)), null_Item_Texture);		
			}else{
				if(hotKeySlot[0].hotKeyslot[i].isItem == true){
					GUI.DrawTexture(GUI_Calculate.RectWithScrren_WidthAndHeight_SizeWidth(new Vector2(hotKeySlot[0].hot_key_Rect.x + ((hotKeySlot[0].hot_key_Rect.width + hotKeySlot[0].space_Slot) * i), hotKeySlot[0].hot_key_Rect.y), new Vector2(hotKeySlot[0].hot_key_Rect.width, hotKeySlot[0].hot_key_Rect.height)), Item_Data.instance.Get_Item(hotKeySlot[0].hotKeyslot[i].item_id).item_Img);	
				}else{
					if(hotKeySlot[0].hotKeyslot[i].typeSkill == TypeSkillHover.Passive){
						GUI.DrawTexture(GUI_Calculate.RectWithScrren_WidthAndHeight_SizeWidth(new Vector2(hotKeySlot[0].hot_key_Rect.x + ((hotKeySlot[0].hot_key_Rect.width + hotKeySlot[0].space_Slot) * i), hotKeySlot[0].hot_key_Rect.y), new Vector2(hotKeySlot[0].hot_key_Rect.width, hotKeySlot[0].hot_key_Rect.height)), playerSkill.passiveSkill[playerSkill.FindSkillIndex(hotKeySlot[0].hotKeyslot[i].item_id)].skillIcon);	
					}else if(hotKeySlot[0].hotKeyslot[i].typeSkill == TypeSkillHover.Buff){
						GUI.DrawTexture(GUI_Calculate.RectWithScrren_WidthAndHeight_SizeWidth(new Vector2(hotKeySlot[0].hot_key_Rect.x + ((hotKeySlot[0].hot_key_Rect.width + hotKeySlot[0].space_Slot) * i), hotKeySlot[0].hot_key_Rect.y), new Vector2(hotKeySlot[0].hot_key_Rect.width, hotKeySlot[0].hot_key_Rect.height)), playerSkill.activeSkillBuff[playerSkill.FindSkillIndex(hotKeySlot[0].hotKeyslot[i].item_id)].skillIcon);	
					}else if(hotKeySlot[0].hotKeyslot[i].typeSkill == TypeSkillHover.Attack){
						GUI.DrawTexture(GUI_Calculate.RectWithScrren_WidthAndHeight_SizeWidth(new Vector2(hotKeySlot[0].hot_key_Rect.x + ((hotKeySlot[0].hot_key_Rect.width + hotKeySlot[0].space_Slot) * i), hotKeySlot[0].hot_key_Rect.y), new Vector2(hotKeySlot[0].hot_key_Rect.width, hotKeySlot[0].hot_key_Rect.height)), playerSkill.activeSkillAttack[playerSkill.FindSkillIndex(hotKeySlot[0].hotKeyslot[i].item_id)].skillIcon);	
					}
				}
			}
			if(hotKeySlot[0].hotKeyslot[i].item_id != 0 && hotKeySlot[0].hotKeyslot[i].isItem == true){
				GUI.Label(GUI_Calculate.RectWithScrren_WidthAndHeight_SizeWidth(new Vector2(hotKeySlot[0].hot_key_Rect.x + ((hotKeySlot[0].hot_key_Rect.width + hotKeySlot[0].space_Slot) * i), hotKeySlot[0].hot_key_Rect.y), new Vector2(hotKeySlot[0].hot_key_Rect.width, hotKeySlot[0].hot_key_Rect.height)), hotKeySlot[0].hotKeyslot[i].item_amount.ToString(), guiStyleGroup[0].textAmountItemStyle);
			}
		}
	}
	
	/// <summary>
	/// Shows the inventory.
	/// </summary>
	private void ShowInventory(){
		if(inventory[0].openInventory){
			if(useStyle){
				GUI.SetNextControlName("Inventory");
				inventory[0].windowInventoryRect = GUI.Window(inventory[0].windowInventoryID, inventory[0].windowInventoryRect, FunctionWindow, inventory[0].nameWindow, guiStyleGroup[0].windowInventoryStyle);
			}else{
				GUI.SetNextControlName("Inventory");
				inventory[0].windowInventoryRect = GUI.Window(inventory[0].windowInventoryID, inventory[0].windowInventoryRect, FunctionWindow, inventory[0].nameWindow);
			}
		}
	}
	
	/// <summary>
	/// Shows the Equipment.
	/// </summary>
	private void ShowEquipment(){
		if(equipment[0].openEquipment){
			if(useStyle){
				GUI.SetNextControlName("Equipment");
				equipment[0].windowEquipmentRect = GUI.Window(equipment[0].windowEquipmentID, equipment[0].windowEquipmentRect, FunctionWindowEquipment, equipment[0].nameEquipmentWindow, guiStyleGroup[0].windowEquipmentStyle);
			}else{
				GUI.SetNextControlName("Equipment");
				equipment[0].windowEquipmentRect = GUI.Window(equipment[0].windowEquipmentID, equipment[0].windowEquipmentRect, FunctionWindowEquipment, equipment[0].nameEquipmentWindow);
			}
		}
	}
	
	private void ShowSkillWindow(){
		if(skillWindow[0].openSkillWindow){
			if(useStyle){
				GUI.SetNextControlName("SkillWindow");	
				skillWindow[0].windowSkillRect = GUI.Window(skillWindow[0].windowSkillID, skillWindow[0].windowSkillRect, FunctionSkillWindow, skillWindow[0].nameSkillWindow, guiStyleGroup[0].windowSkillStyle);
			}else{
				GUI.SetNextControlName("SkillWindow");
				skillWindow[0].windowSkillRect = GUI.Window(skillWindow[0].windowSkillID, skillWindow[0].windowSkillRect, FunctionSkillWindow, skillWindow[0].nameSkillWindow);
			}
		}
	}
	
	private void ShowItemShopWindow(){
		if(itemShop[0].openItemShop){
			if(useStyle){
				GUI.SetNextControlName("ItemShopWindow");	
				itemShop[0].windowItemShopRect = GUI.Window(itemShop[0].windowItemShopID, itemShop[0].windowItemShopRect, FunctionItemShopWindow, itemShop[0].nameItemShopWindow, guiStyleGroup[0].windowItemShopStyle);
			}else{
				GUI.SetNextControlName("ItemShopWindow");
				itemShop[0].windowItemShopRect = GUI.Window(itemShop[0].windowItemShopID, itemShop[0].windowItemShopRect, FunctionItemShopWindow, itemShop[0].nameItemShopWindow);
			}
			
			if(Input.GetKeyDown(KeyCode.Escape)){
				itemShop[0].openItemShop = false;	
			}
		}
	}
	
	/// <summary>
	/// Shows the window drop amount.
	/// </summary>
	private void ShowWindowDropAmount(){
		if(showDropAmount){
			inventory[0].windowDropRect.x = Screen.width*0.5f - (inventory[0].windowDropRect.width*0.5f);
			inventory[0].windowDropRect.y = Screen.height*0.5f - (inventory[0].windowDropRect.height*0.5f);
			
			if(CheckHoverItemShop() || CheckHoverInventory())
			{
				if(Input.GetMouseButtonDown(0) && !CheckHoverAmoutDrop())
				{
					showDropAmount = false;	
					isItemFormInventory = false;
					indexInventory = -1;
					isBuyItem = false;
					amountItemDrop = "";
					item_pickup = null;
					inventory[0].enterDrop = false;
				}
			}
			
			GUI.FocusControl("Input item drop");
			
			if(useStyle){
				inventory[0].windowDropRect = GUI.Window(inventory[0].windowDropItemID, inventory[0].windowDropRect, FunctionWindowDrop, "", guiStyleGroup[0].windowDropStyle);
			}else{
				inventory[0].windowDropRect = GUI.Window(inventory[0].windowDropItemID, inventory[0].windowDropRect, FunctionWindowDrop, "");	
			}
			
			
			if (Event.current.isKey && Event.current.keyCode == KeyCode.Return || inventory[0].enterDrop){
			//if(inventory[0].enterDrop&& Input.GetMouseButtonDown(0))
			//{
				int amountDrop = 0;
				try{
					amountDrop = int.Parse(amountItemDrop);
				}catch{
					amountDrop = 0;	
				}
				
				if(amountDrop > 0 && isBuyItem == false && isItemFormInventory == false){
					Drop_Item(indexInventory, transform.position+(Vector3.up*1.5f), amountDrop);
				}else if(amountDrop > 0 && isBuyItem == true && money >= (Item_Data.instance.Get_Item(item_pickup.item_id).price * amountDrop) && isItemFormInventory == false){
					
					Debug.Log("Buy Item");
					
					Item_Obj _item_obj = new Item_Obj();
					_item_obj.item_id = item_pickup.item_id;
					_item_obj.euipmentType = item_pickup.equipmentType;
					
					if(_item_obj.euipmentType != Item_Data.Equipment_Type.Null){
						_item_obj.item_amount = 1;	
					}else{
						_item_obj.item_amount = amountDrop;
					}
					
					Pickup_Item(_item_obj);

					money -= Item_Data.instance.Get_Item(item_pickup.item_id).price*amountDrop;

				}else if(money < (Item_Data.instance.Get_Item(item_pickup.item_id).price * amountDrop)){
					
					//Show can Not Buy
					LogText.Instance.SetLog(GameSetting.Instance.logTimer,GameSetting.Instance.logSettingCantBuy);
					
				}else if(amountDrop > 0 && isBuyItem == false && isItemFormInventory == true){
					
					Debug.Log("Sell Item");
					
					if(bag_item[indexInventory].item_amount - amountDrop <= 0){
						amountDrop = bag_item[indexInventory].item_amount;
						bag_item[indexInventory].item_amount = 0;
					}else{
						bag_item[indexInventory].item_amount -= amountDrop;	
					}
						
					money += (Item_Data.instance.Get_Item(bag_item[indexInventory].item_id).price/2)*amountDrop;

					if(bag_item[indexInventory].item_amount <= 0){
						bag_item[indexInventory].item_id = 0;
						bag_item[indexInventory].isItem = false;
						bag_item[indexInventory].item_amount = 0;
						bag_item[indexInventory].linkIndex = -1;
						bag_item[indexInventory].equipmentType = Item_Data.Equipment_Type.Null;
					}
				}
				
				isItemFormInventory = false;
				indexInventory = -1;
				isBuyItem = false;
				amountItemDrop = "";
				showDropAmount = false;
				item_pickup = null;
				inventory[0].enterDrop = false;
			}
		}
	}
	
	/// <summary>
	/// Check item on the floor before pickup
	/// </summary>
	private void RayPickupItem(){
		ray = Camera.mainCamera.ScreenPointToRay(Input.mousePosition);
		if(Physics.Raycast(ray, out hit, 20)){
			
			if(hit.collider.tag == "Item" || hit.collider.tag == "Npc_Shop"){
				playerController.dontClick = true;
			}
			
			if(Input.GetMouseButtonDown(0) && Vector3.Distance(playerController.transform.position, hit.collider.gameObject.transform.position) < rangeDetect){
				//Check on Item
				if(hit.collider.tag == "Item" && !CheckHoverInventory() && !CheckHoverEquipment()){
					if(Pickup_Item(hit.collider.GetComponent<Item_Obj>())){
						Destroy(hit.collider.gameObject);
					}
				}
				
				//Check on NPC Shop
				if(hit.collider.tag == "Npc_Shop" && !CheckHoverInventory() && !CheckHoverEquipment() && !CheckHoverSkillWindow() && CheckHoverSlotHotkey() == -1 && itemShop[0].openItemShop == false){
					OpenItemShopWindow();
					itemShop[0].itemID = hit.collider.GetComponent<NpcShop>().itemID;
				}
				
				if(hit.collider.tag == "Save_Point" && !CheckHoverInventory() && !CheckHoverEquipment() && !CheckHoverSkillWindow() && CheckHoverSlotHotkey() == -1 && itemShop[0].openItemShop == false){
					SaveWindow.enableWindow = true;
					
				}
			}
		}else{
			playerController.dontClick = false;	
		}
	}
	
	private void OpenItemShopWindow(){
		if(itemShop[0].openItemShop == false){
			itemShop[0].windowItemShopRect.x = (Screen.width*0.5f) - (itemShop[0].windowItemShopRect.width*0.5f);
			itemShop[0].windowItemShopRect.y = (Screen.height*0.4f) - (itemShop[0].windowItemShopRect.height*0.5f);
			itemShop[0].openItemShop = true;
		}
	}
	
	private void OpenStatWindow(){
		if(Input.GetKey(keyOpenStat.keyOpenFirst) && Input.GetKeyDown(keyOpenStat.keyOenSecound)){
			if(statWindow[0].openStatWindow == false){
				statWindow[0].windowStatRect.x = (Screen.width*0.5f) - (statWindow[0].windowStatRect.width*0.5f);
				statWindow[0].windowStatRect.y = (Screen.height*0.4f) - (statWindow[0].windowStatRect.height*0.5f);
				GetStatFormPlayer();
				statWindow[0].openStatWindow = true;
			}else{
				statWindow[0].openStatWindow = false;	
			}
		}
	}
	
	private void OpenSkillWindow(){
		if(Input.GetKey(keyOpenSkill.keyOpenFirst) && Input.GetKeyDown(keyOpenSkill.keyOenSecound)){
			if(skillWindow[0].openSkillWindow == false){
				skillWindow[0].windowSkillRect.x = (Screen.width*0.5f) - (skillWindow[0].windowSkillRect.width*0.5f);
				skillWindow[0].windowSkillRect.y = (Screen.height*0.4f) - (skillWindow[0].windowSkillRect.height*0.5f);
				skillWindow[0].openSkillWindow = true;
			}else{
				skillWindow[0].openSkillWindow = false;	
			}
		}
	}
	
	/// <summary>
	/// Opens the inventory window.
	/// </summary>
	private void OpenInventoryWindow(){
		if(Input.GetKey(keyOpenInventory.keyOpenFirst) && Input.GetKeyDown(keyOpenInventory.keyOenSecound)){
			if(inventory[0].openInventory == false){
				if(equipment[0].openEquipment){
					inventory[0].windowInventoryRect.x = (equipment[0].windowEquipmentRect.x + equipment[0].windowEquipmentRect.width);
					inventory[0].windowInventoryRect.y = equipment[0].windowEquipmentRect.y;
				}else{
					inventory[0].windowInventoryRect.x = (Screen.width*0.7f) - (inventory[0].windowInventoryRect.width*0.5f);
					inventory[0].windowInventoryRect.y = (Screen.height*0.5f) - (inventory[0].windowInventoryRect.height*0.5f);
				}
				inventory[0].openInventory = true;	
			}else{
				inventory[0].openInventory = false;	
			}
		}
	}
	
	public void OpenShortcutMenu(string type)
	{
		if(type == "Inventory")
		{
			if(inventory[0].openInventory == false){
			if(equipment[0].openEquipment){
				inventory[0].windowInventoryRect.x = (equipment[0].windowEquipmentRect.x + equipment[0].windowEquipmentRect.width);
				inventory[0].windowInventoryRect.y = equipment[0].windowEquipmentRect.y;
			}else{
				inventory[0].windowInventoryRect.x = (Screen.width*0.7f) - (inventory[0].windowInventoryRect.width*0.5f);
				inventory[0].windowInventoryRect.y = (Screen.height*0.5f) - (inventory[0].windowInventoryRect.height*0.5f);
			}
				inventory[0].openInventory = true;	
			}else{
				inventory[0].openInventory = false;	
			}
		}else if(type == "Equipment")
		{
			if(equipment[0].openEquipment == false){
				if(inventory[0].openInventory){
					equipment[0].windowEquipmentRect.x = inventory[0].windowInventoryRect.x - (equipment[0].windowEquipmentRect.width);
					equipment[0].windowEquipmentRect.y = inventory[0].windowInventoryRect.y;
				}else{
					equipment[0].windowEquipmentRect.x = (Screen.width*0.3f) - (equipment[0].windowEquipmentRect.width*0.5f);
					equipment[0].windowEquipmentRect.y = (Screen.height*0.5f) - (equipment[0].windowEquipmentRect.height*0.5f);
				}
				equipment[0].openEquipment = true;	
			}else{
				equipment[0].openEquipment = false;	
			}	
		}else if(type == "Skill")
		{
			if(skillWindow[0].openSkillWindow == false){
				skillWindow[0].windowSkillRect.x = (Screen.width*0.5f) - (skillWindow[0].windowSkillRect.width*0.5f);
				skillWindow[0].windowSkillRect.y = (Screen.height*0.4f) - (skillWindow[0].windowSkillRect.height*0.5f);
				skillWindow[0].openSkillWindow = true;
			}else{
				skillWindow[0].openSkillWindow = false;	
			}
		}else if(type == "Status")
		{
			if(statWindow[0].openStatWindow == false){
				statWindow[0].windowStatRect.x = (Screen.width*0.5f) - (statWindow[0].windowStatRect.width*0.5f);
				statWindow[0].windowStatRect.y = (Screen.height*0.4f) - (statWindow[0].windowStatRect.height*0.5f);
				GetStatFormPlayer();
				statWindow[0].openStatWindow = true;
			}else{
				statWindow[0].openStatWindow = false;	
			}
		}
		
	}
	
	
	/// <summary>
	/// Opens the Equipment window.
	/// </summary>
	private void OpenEquipmentWindow(){
		if(Input.GetKey(keyOpenEquipment.keyOpenFirst) && Input.GetKeyDown(keyOpenEquipment.keyOenSecound)){
			if(equipment[0].openEquipment == false){
				if(inventory[0].openInventory){
					equipment[0].windowEquipmentRect.x = inventory[0].windowInventoryRect.x - (equipment[0].windowEquipmentRect.width);
					equipment[0].windowEquipmentRect.y = inventory[0].windowInventoryRect.y;
				}else{
					equipment[0].windowEquipmentRect.x = (Screen.width*0.3f) - (equipment[0].windowEquipmentRect.width*0.5f);
					equipment[0].windowEquipmentRect.y = (Screen.height*0.5f) - (equipment[0].windowEquipmentRect.height*0.5f);
				}
				equipment[0].openEquipment = true;	
			}else{
				equipment[0].openEquipment = false;	
			}
		}
	}
	
	private void FunctionStatWindow(int windowID){
		GUI.DragWindow(new Rect(0,0,250,60));	
		
		InFunctionStatWindow();
	}
	
	private void InFunctionStatWindow(){
		if(useStyle == false){
			GUI.Label(statWindow[0].AttackHeadRect, "Atk");
			GUI.Label(statWindow[0].DefHeadRect,"Def");
			GUI.Label(statWindow[0].SpeedHeadRect,"Speed");
			GUI.Label(statWindow[0].HitHeadRect,"Hit");
			
			GUI.Label(statWindow[0].AttackValRect,":          "+statWindow[0].valAtk+" + "+playerStatus.statusAdd.atk+playerStatus.statusGrowth.atk);
			GUI.Label(statWindow[0].DefValRect,":          "+statWindow[0].valDef+" + "+playerStatus.statusAdd.def+playerStatus.statusGrowth.def);
			GUI.Label(statWindow[0].SpeedValRect,":          "+statWindow[0].valSpeed+" + "+playerStatus.statusAdd.spd+playerStatus.statusGrowth.spd);
			GUI.Label(statWindow[0].HitValRect,":          "+statWindow[0].valHit+" + "+playerStatus.statusAdd.hit+playerStatus.statusGrowth.hit);
				
			if(playerStatus.alreadyApply == false){
					
				if(GUI.Button(statWindow[0].BtnAddStatAttack,"+")){
					if(statWindow[0].defPoint > 0){
						statWindow[0].valAtk++;
						statWindow[0].defPoint--;
					}
				}
					
				if(GUI.Button(statWindow[0].BtnAddStatDef,"+")){
					if(statWindow[0].defPoint > 0){
						statWindow[0].valDef++;
						statWindow[0].defPoint--;
					}
				}
					
				if(GUI.Button(statWindow[0].BtnAddStatSpeed,"+")){
					if(statWindow[0].defPoint > 0){
						statWindow[0].valSpeed++;
						statWindow[0].defPoint--;
					}
				}
					
				if(GUI.Button(statWindow[0].BtnAddStatHit,"+")){
					if(statWindow[0].defPoint > 0){
						statWindow[0].valHit++;
						statWindow[0].defPoint--;
					}
				}
					
				if(GUI.Button(statWindow[0].BtnApplyRect,"Apply")){
					playerStatus.status.atk = statWindow[0].valAtk;
					playerStatus.status.def = statWindow[0].valDef;
					playerStatus.status.spd = statWindow[0].valSpeed;
					playerStatus.status.hit = statWindow[0].valHit;
					playerStatus.pointCurrent = statWindow[0].defPoint;
					
					statWindow[0].defAtk = playerStatus.status.atk;
					statWindow[0].defDef = playerStatus.status.def;
					statWindow[0].defSpeed = playerStatus.status.spd;
					statWindow[0].defHit = playerStatus.status.hit;
					statWindow[0].defPoint = playerStatus.pointCurrent;
					
					playerStatus.UpdateAttribute();
					
					if(statWindow[0].defPoint <= 0)
					playerStatus.alreadyApply = true;
				}
					
				if(GUI.Button(statWindow[0].BtnResetRect,"Reset")){
					statWindow[0].valAtk = statWindow[0].defAtk;
					statWindow[0].valDef = statWindow[0].defDef;
					statWindow[0].valSpeed = statWindow[0].defSpeed;
					statWindow[0].valHit = statWindow[0].defHit;
					
					statWindow[0].defPoint = playerStatus.pointCurrent;
				}
					
			}
				
			GUI.Label(statWindow[0].PointStatRect,"Point Remain : "+statWindow[0].defPoint);
			
			GUI.Label(statWindow[0].HeadSummaryRect,"Summary");
			
			GUI.Label(statWindow[0].SummaryStatRectLeft,"Attack : "+playerStatus.statusCal.atk+"\n"+
													"Defend : "+playerStatus.statusCal.def+"\n"+
													"Speed : "+playerStatus.statusCal.spd+"\n"+
													"Hit : "+playerStatus.statusCal.hit);
			
			GUI.Label(statWindow[0].SummaryStatRectRight,"Critical Rate : "+playerStatus.statusCal.criticalRate+"\n"+
													"Attack Speed : "+playerStatus.statusCal.atkSpd+"\n"+
													"Attack Range : "+playerStatus.statusCal.atkRange+"\n"+
													"Move Speed : "+playerStatus.statusCal.movespd);
		}else{
			
			GUI.Label(statWindow[0].AttackHeadRect, "Atk", guiStyleGroup[0].textStatStyle);
			GUI.Label(statWindow[0].DefHeadRect,"Def", guiStyleGroup[0].textStatStyle);
			GUI.Label(statWindow[0].SpeedHeadRect,"Speed", guiStyleGroup[0].textStatStyle);
			GUI.Label(statWindow[0].HitHeadRect,"Hit", guiStyleGroup[0].textStatStyle);
			
			GUI.Label(statWindow[0].AttackValRect,":          "+statWindow[0].valAtk+" + "+ (playerStatus.statusAdd.atk+playerStatus.statusGrowth.atk).ToString(), guiStyleGroup[0].textStatStyle);
			GUI.Label(statWindow[0].DefValRect,":          "+statWindow[0].valDef+" + "+(playerStatus.statusAdd.def+playerStatus.statusGrowth.def).ToString(), guiStyleGroup[0].textStatStyle);
			GUI.Label(statWindow[0].SpeedValRect,":          "+statWindow[0].valSpeed+" + "+(playerStatus.statusAdd.spd+playerStatus.statusGrowth.spd).ToString(), guiStyleGroup[0].textStatStyle);
			GUI.Label(statWindow[0].HitValRect,":          "+statWindow[0].valHit+" + "+(playerStatus.statusAdd.hit+playerStatus.statusGrowth.hit).ToString(), guiStyleGroup[0].textStatStyle);
				
			if(playerStatus.alreadyApply == false ){
					
				if(GUI.Button(statWindow[0].BtnAddStatAttack,"+", guiStyleGroup[0].btnAddStatStyle)){
					if(statWindow[0].defPoint > 0){
						statWindow[0].valAtk++;
						statWindow[0].defPoint--;
				
					}
				}
					
				if(GUI.Button(statWindow[0].BtnAddStatDef,"+", guiStyleGroup[0].btnAddStatStyle)){
					if(statWindow[0].defPoint > 0){
						statWindow[0].valDef++;
						statWindow[0].defPoint--;
						
					}
				}
					
				if(GUI.Button(statWindow[0].BtnAddStatSpeed,"+", guiStyleGroup[0].btnAddStatStyle)){
					if(statWindow[0].defPoint > 0){
						statWindow[0].valSpeed++;
						statWindow[0].defPoint--;
						
					}
				}
					
				if(GUI.Button(statWindow[0].BtnAddStatHit,"+", guiStyleGroup[0].btnAddStatStyle)){
					if(statWindow[0].defPoint > 0){
						statWindow[0].valHit++;
						statWindow[0].defPoint--;
					
					}
				}
					
				if(GUI.Button(statWindow[0].BtnApplyRect,"Apply", guiStyleGroup[0].btnApplyStyle)){
					playerStatus.status.atk = statWindow[0].valAtk;
					playerStatus.status.def = statWindow[0].valDef;
					playerStatus.status.spd = statWindow[0].valSpeed;
					playerStatus.status.hit = statWindow[0].valHit;
					playerStatus.pointCurrent = statWindow[0].defPoint;
					
					statWindow[0].defAtk = playerStatus.status.atk;
					statWindow[0].defDef = playerStatus.status.def;
					statWindow[0].defSpeed = playerStatus.status.spd;
					statWindow[0].defHit = playerStatus.status.hit;
					statWindow[0].defPoint = playerStatus.pointCurrent;
					
					playerStatus.UpdateAttribute();
					
					if(statWindow[0].defPoint <= 0)
					playerStatus.alreadyApply = true;
				}
					
				if(GUI.Button(statWindow[0].BtnResetRect,"Reset", guiStyleGroup[0].btnResetStyle)){
					statWindow[0].valAtk = statWindow[0].defAtk;
					statWindow[0].valDef = statWindow[0].defDef;
					statWindow[0].valSpeed = statWindow[0].defSpeed;
					statWindow[0].valHit = statWindow[0].defHit;
					
					statWindow[0].defPoint = playerStatus.pointCurrent;
				}
					
			}
				
			GUI.Label(statWindow[0].PointStatRect,"Point Remain : "+statWindow[0].defPoint, guiStyleGroup[0].textPointRemainStyle);
			
			GUI.Label(statWindow[0].HeadSummaryRect,"Summary", guiStyleGroup[0].textHeadSummaryStyle);
			
			GUI.Label(statWindow[0].SummaryStatRectLeft,"Attack : "+playerStatus.statusCal.atk+"\n"+
													"Defend : "+playerStatus.statusCal.def+"\n"+
													"Speed : "+playerStatus.statusCal.spd+"\n"+
													"Hit : "+playerStatus.statusCal.hit, guiStyleGroup[0].textSummaryStyle);
			
			GUI.Label(statWindow[0].SummaryStatRectRight,"Critical Rate : "+playerStatus.statusCal.criticalRate+"\n"+
													"Attack Speed : "+playerStatus.statusCal.atkSpd+"\n"+
													"Attack Range : "+playerStatus.statusCal.atkRange+"\n"+
													"Move Speed : "+playerStatus.statusCal.movespd, guiStyleGroup[0].textSummaryStyle);
			
		}
		
		
	}
				
	private void GetStatFormPlayer(){
		statWindow[0].defAtk = playerStatus.status.atk;
		statWindow[0].defDef = playerStatus.status.def;
		statWindow[0].defSpeed = playerStatus.status.spd;
		statWindow[0].defHit = playerStatus.status.hit;
		
		statWindow[0].valAtk = statWindow[0].defAtk;
		statWindow[0].valDef = statWindow[0].defDef;
		statWindow[0].valSpeed = statWindow[0].defSpeed;
		statWindow[0].valHit = statWindow[0].defHit;
		
		
	}
	
	[HideInInspector]
	public Vector2 scrollPositionShop;
	private void FunctionItemShopWindow(int windowID){
		GUI.DragWindow(new Rect(0,0,250,60));
		
		itemShop[0].scrollViewRect.height = (((itemShop[0].itemID.Count-1) * itemShop[0].slotItemRect.height) * itemShop[0].space) + itemShop[0].offset.y + (itemShop[0].slotItemRect.height/2 + itemShop[0].offset.y/2);
		scrollPositionShop = GUI.BeginScrollView(itemShop[0].scrollViewPosition , scrollPositionShop, itemShop[0].scrollViewRect);
		for(int i = 0; i < itemShop[0].itemID.Count; i++){
			if(useStyle){
				GUI.Box(new Rect(itemShop[0].slotItemRect.x + itemShop[0].offset.x, itemShop[0].slotItemRect.y + ((i * itemShop[0].slotItemRect.height) * itemShop[0].space) + itemShop[0].offset.y, itemShop[0].slotItemRect.width, itemShop[0].slotItemRect.height), new GUIContent("","itemShopWindow"+(i).ToString()), guiStyleGroup[0].itemShopSlotStyle);
				GUI.Box(new Rect(itemShop[0].descriptionRect.x + itemShop[0].offset.x, itemShop[0].descriptionRect.y + ((i * itemShop[0].descriptionRect.height) * itemShop[0].space) + itemShop[0].offset.y, itemShop[0].descriptionRect.width, itemShop[0].descriptionRect.height), new GUIContent("","itemShopWindow"+(i).ToString()), guiStyleGroup[0].descriptionItemShopStyle);
				
				GUI.Box(new Rect(itemShop[0].itemNameRect.x + itemShop[0].offset.x, itemShop[0].itemNameRect.y + ((i * itemShop[0].itemNameRect.height) * itemShop[0].spaceItemName) + itemShop[0].offset.y, itemShop[0].itemNameRect.width, itemShop[0].itemNameRect.height), new GUIContent(Item_Data.instance.Get_Item(itemShop[0].itemID[i]).item_Name ,"itemShopWindow"+(i).ToString()), guiStyleGroup[0].itemShopItemNameStyle);
				GUI.Box(new Rect(itemShop[0].itemTypeRect.x + itemShop[0].offset.x, itemShop[0].itemTypeRect.y + ((i * itemShop[0].itemTypeRect.height) * itemShop[0].spaceItemType) + itemShop[0].offset.y, itemShop[0].itemTypeRect.width, itemShop[0].itemTypeRect.height), new GUIContent(Item_Data.instance.Get_Item(itemShop[0].itemID[i]).item_Type ,"itemShopWindow"+(i).ToString()), guiStyleGroup[0].itemShopItemTypeStyle);
				GUI.Box(new Rect(itemShop[0].itemDescriptionRect.x + itemShop[0].offset.x, itemShop[0].itemDescriptionRect.y + ((i * itemShop[0].itemDescriptionRect.height) * itemShop[0].spaceItemDescription) + itemShop[0].offset.y, itemShop[0].itemDescriptionRect.width, itemShop[0].itemDescriptionRect.height), new GUIContent(Item_Data.instance.Get_Item(itemShop[0].itemID[i]).description ,"itemShopWindow"+(i).ToString()), guiStyleGroup[0].itemShopDescriptionStyle);
				
				GUI.Box(new Rect(itemShop[0].priceRect.x + itemShop[0].offset.x, itemShop[0].priceRect.y + ((i * itemShop[0].priceRect.height) * itemShop[0].spacePrice) + itemShop[0].offset.y, itemShop[0].priceRect.width, itemShop[0].priceRect.height), new GUIContent(Item_Data.instance.Get_Item(itemShop[0].itemID[i]).price.ToString() ,"itemShopWindow"+(i).ToString()), guiStyleGroup[0].itemShopPriceStyle);
				if(GUI.Button(new Rect(itemShop[0].buttonBuyRect.x + itemShop[0].offset.x, itemShop[0].buttonBuyRect.y + ((i * itemShop[0].buttonBuyRect.height) * itemShop[0].spaceButtonBuy) + itemShop[0].offset.y, itemShop[0].buttonBuyRect.width, itemShop[0].buttonBuyRect.height), new GUIContent("BUY","itemShopWindow"+(i).ToString()), guiStyleGroup[0].itemShopButtonBuyStyle)){
					if(itemShop[0].itemID[i] != null && showDropAmount == false){
						item_pickup = new Bag();
						item_pickup.item_id = itemShop[0].itemID[i];
						item_pickup.isItem = true;
						item_pickup.equipmentType = Item_Data.instance.Get_Item(itemShop[0].itemID[i]).equipment_Type;
						isBuyItem = true;
						//amountItemDrop = "1";
						showDropAmount = true;
					}
				}
			}else{
				GUI.Box(new Rect(itemShop[0].slotItemRect.x + itemShop[0].offset.x, itemShop[0].slotItemRect.y + ((i * itemShop[0].slotItemRect.height) * itemShop[0].space) + itemShop[0].offset.y, itemShop[0].slotItemRect.width, itemShop[0].slotItemRect.height), new GUIContent("","itemShopWindow"+(i).ToString()));
				GUI.Box(new Rect(itemShop[0].descriptionRect.x + itemShop[0].offset.x, itemShop[0].descriptionRect.y + ((i * itemShop[0].descriptionRect.height) * itemShop[0].space) + itemShop[0].offset.y, itemShop[0].descriptionRect.width, itemShop[0].descriptionRect.height), new GUIContent("" ,"itemShopWindow"+(i).ToString()));
			
				GUI.Box(new Rect(itemShop[0].itemNameRect.x + itemShop[0].offset.x, itemShop[0].itemNameRect.y + ((i * itemShop[0].itemNameRect.height) * itemShop[0].spaceItemName) + itemShop[0].offset.y, itemShop[0].itemNameRect.width, itemShop[0].itemNameRect.height), new GUIContent(Item_Data.instance.Get_Item(itemShop[0].itemID[i]).item_Name ,"itemShopWindow"+(i).ToString()));
				GUI.Box(new Rect(itemShop[0].itemTypeRect.x + itemShop[0].offset.x, itemShop[0].itemTypeRect.y + ((i * itemShop[0].itemTypeRect.height) * itemShop[0].spaceItemType) + itemShop[0].offset.y, itemShop[0].itemTypeRect.width, itemShop[0].itemTypeRect.height), new GUIContent(Item_Data.instance.Get_Item(itemShop[0].itemID[i]).item_Type ,"itemShopWindow"+(i).ToString()));
				GUI.Box(new Rect(itemShop[0].itemDescriptionRect.x + itemShop[0].offset.x, itemShop[0].itemDescriptionRect.y + ((i * itemShop[0].itemDescriptionRect.height) * itemShop[0].spaceItemDescription) + itemShop[0].offset.y, itemShop[0].itemDescriptionRect.width, itemShop[0].itemDescriptionRect.height), new GUIContent(Item_Data.instance.Get_Item(itemShop[0].itemID[i]).description ,"itemShopWindow"+(i).ToString()));
				
				GUI.Box(new Rect(itemShop[0].priceRect.x + itemShop[0].offset.x, itemShop[0].priceRect.y + ((i * itemShop[0].priceRect.height) * itemShop[0].spacePrice) + itemShop[0].offset.y, itemShop[0].priceRect.width, itemShop[0].priceRect.height), new GUIContent(Item_Data.instance.Get_Item(itemShop[0].itemID[i]).price.ToString() ,"itemShopWindow"+(i).ToString()));
				if(GUI.Button(new Rect(itemShop[0].buttonBuyRect.x + itemShop[0].offset.x, itemShop[0].buttonBuyRect.y + ((i * itemShop[0].buttonBuyRect.height) * itemShop[0].spaceButtonBuy) + itemShop[0].offset.y, itemShop[0].buttonBuyRect.width, itemShop[0].buttonBuyRect.height), new GUIContent("BUY","itemShopWindow"+(i).ToString()))){
					if(itemShop[0].itemID[i] != null && showDropAmount == false){
						item_pickup = new Bag();
						item_pickup.item_id = itemShop[0].itemID[i];
						item_pickup.isItem = true;
						item_pickup.equipmentType = Item_Data.instance.Get_Item(itemShop[0].itemID[i]).equipment_Type;
						isBuyItem = true;
						//amountItemDrop = "1";
						showDropAmount = true;
					}
				}
			}
			
			
			
			if(Item_Data.instance.Get_Item(itemShop[0].itemID[i]) != null){
				GUI.DrawTexture(new Rect(itemShop[0].slotItemRect.x + itemShop[0].offset.x, itemShop[0].slotItemRect.y + ((i * itemShop[0].slotItemRect.height) * itemShop[0].space) + itemShop[0].offset.y, itemShop[0].slotItemRect.width, itemShop[0].slotItemRect.height), Item_Data.instance.Get_Item(itemShop[0].itemID[i]).item_Img);		
			}
		}
		
		if(pickupStay){
			float maxSpacePos = (itemShop[0].scrollViewRect.height - scrollPositionShop.y) / (itemShop[0].slotItemRect.height * itemShop[0].space);
			Vector3 mousePos = Input.mousePosition;
			if(itemImg_Pickup != null){
				GUI.DrawTexture(new Rect((mousePos.x - itemShop[0].scrollViewPosition.x)-20-itemShop[0].windowItemShopRect.x, (Screen.height - (mousePos.y + itemShop[0].scrollViewPosition.y - itemShop[0].valueTuningIconFollowMouse))-20-itemShop[0].windowItemShopRect.y + (scrollPositionShop.y - maxSpacePos* (-1*(itemShop[0].slotItemRect.height * itemShop[0].space/100))), 40,40),itemImg_Pickup);	
			}
		}
		GUI.EndScrollView();
		
		ShowDescriptionBag(itemShop[0].windowItemShopRect , id_Hover);
		ShowDescriptionBag(itemShop[0].windowItemShopRect , id_Hover);
		
		if(useStyle){
			GUI.Box(itemShop[0].moneyRect,money.ToString(),guiStyleGroup[0].goldStyle);
			if(GUI.Button(itemShop[0].buttonCancelRect,"",guiStyleGroup[0].itemShopCancel)){
				itemShop[0].openItemShop = false;
			}
		}else{
			GUI.Box(itemShop[0].moneyRect,money.ToString());
			if(GUI.Button(itemShop[0].buttonCancelRect,"X")){
				itemShop[0].openItemShop = false;
			}	
		}
		
		GUI.DrawTexture(itemShop[0].coinIconRect, coinIcon);
	}
	
	[HideInInspector]
	public Vector2 scrollPosition;
	//Function Skill Window
	private void FunctionSkillWindow(int windowID){
		GUI.DragWindow(new Rect(0,0,250,60));
		
		skillWindow[0].scrollViewRect.height = ((((playerSkill.activeSkillAttack.Count-1)+playerSkill.passiveSkill.Count+playerSkill.activeSkillBuff.Count) * skillWindow[0].slotSkillRect.height) * skillWindow[0].space) + skillWindow[0].offset.y + (skillWindow[0].slotSkillRect.height/2 + skillWindow[0].offset.y/2);
		
		scrollPosition = GUI.BeginScrollView(skillWindow[0].scrollViewPosition , scrollPosition, skillWindow[0].scrollViewRect);
		for(int i = 0; i < playerSkill.passiveSkill.Count; i++){
			if(useStyle){
				GUI.Box(new Rect(skillWindow[0].slotSkillRect.x + skillWindow[0].offset.x, skillWindow[0].slotSkillRect.y + ((i * skillWindow[0].slotSkillRect.height) * skillWindow[0].space) + skillWindow[0].offset.y, skillWindow[0].slotSkillRect.width, skillWindow[0].slotSkillRect.height), new GUIContent("","skillWindow"+(i).ToString()), guiStyleGroup[0].skillSlotStyle);
				GUI.Box(new Rect(skillWindow[0].descriptionRect.x + skillWindow[0].offset.x, skillWindow[0].descriptionRect.y + ((i * skillWindow[0].descriptionRect.height) * skillWindow[0].space) + skillWindow[0].offset.y, skillWindow[0].descriptionRect.width, skillWindow[0].descriptionRect.height), new GUIContent("" ,"skillWindow"+(i).ToString()), guiStyleGroup[0].descriptionSkillSlotStyle);
			
				GUI.Box(new Rect(skillWindow[0].skillNameRect.x + skillWindow[0].offset.x, skillWindow[0].skillNameRect.y + ((i * skillWindow[0].skillNameRect.height) * skillWindow[0].spaceSkillName) + skillWindow[0].offset.y, skillWindow[0].skillNameRect.width, skillWindow[0].skillNameRect.height), new GUIContent(playerSkill.passiveSkill[i].skillName ,"skillWindow"+(i).ToString()), guiStyleGroup[0].skillNameStyle);
				GUI.Box(new Rect(skillWindow[0].skillTypeRect.x + skillWindow[0].offset.x, skillWindow[0].skillTypeRect.y + ((i * skillWindow[0].skillTypeRect.height) * skillWindow[0].spaceSkillType) + skillWindow[0].offset.y, skillWindow[0].skillTypeRect.width, skillWindow[0].skillTypeRect.height), new GUIContent(playerSkill.passiveSkill[i].typeSkill ,"skillWindow"+(i).ToString()), guiStyleGroup[0].skillTypeStyle);
				GUI.Box(new Rect(skillWindow[0].skillManaUseRect.x + skillWindow[0].offset.x, skillWindow[0].skillManaUseRect.y + ((i * skillWindow[0].skillManaUseRect.height) * skillWindow[0].spaceSkillManaUse) + skillWindow[0].offset.y, skillWindow[0].skillManaUseRect.width, skillWindow[0].skillManaUseRect.height), new GUIContent("MP 0","skillWindow"+(i).ToString()), guiStyleGroup[0].skillManaUseStyle);
				GUI.Box(new Rect(skillWindow[0].skillDescriptionRect.x + skillWindow[0].offset.x, skillWindow[0].skillDescriptionRect.y + ((i * skillWindow[0].skillDescriptionRect.height) * skillWindow[0].spaceDescription) + skillWindow[0].offset.y, skillWindow[0].skillDescriptionRect.width, skillWindow[0].skillDescriptionRect.height), new GUIContent(playerSkill.passiveSkill[i].description ,"skillWindow"+(i).ToString()), guiStyleGroup[0].skillDescriptionStyle);
			}else{
				GUI.Box(new Rect(skillWindow[0].slotSkillRect.x + skillWindow[0].offset.x, skillWindow[0].slotSkillRect.y + ((i * skillWindow[0].slotSkillRect.height) * skillWindow[0].space) + skillWindow[0].offset.y, skillWindow[0].slotSkillRect.width, skillWindow[0].slotSkillRect.height), new GUIContent("","skillWindow"+(i).ToString()));
				GUI.Box(new Rect(skillWindow[0].descriptionRect.x + skillWindow[0].offset.x, skillWindow[0].descriptionRect.y + ((i * skillWindow[0].descriptionRect.height) * skillWindow[0].space) + skillWindow[0].offset.y, skillWindow[0].descriptionRect.width, skillWindow[0].descriptionRect.height), new GUIContent("" ,"skillWindow"+(i).ToString()));
				
				GUI.Box(new Rect(skillWindow[0].skillNameRect.x + skillWindow[0].offset.x, skillWindow[0].skillNameRect.y + ((i * skillWindow[0].skillNameRect.height) * skillWindow[0].spaceSkillName) + skillWindow[0].offset.y, skillWindow[0].skillNameRect.width, skillWindow[0].skillNameRect.height), new GUIContent(playerSkill.passiveSkill[i].skillName ,"skillWindow"+(i).ToString()));
				GUI.Box(new Rect(skillWindow[0].skillTypeRect.x + skillWindow[0].offset.x, skillWindow[0].skillTypeRect.y + ((i * skillWindow[0].skillTypeRect.height) * skillWindow[0].spaceSkillType) + skillWindow[0].offset.y, skillWindow[0].skillTypeRect.width, skillWindow[0].skillTypeRect.height), new GUIContent(playerSkill.passiveSkill[i].typeSkill ,"skillWindow"+(i).ToString()));
				GUI.Box(new Rect(skillWindow[0].skillManaUseRect.x + skillWindow[0].offset.x, skillWindow[0].skillManaUseRect.y + ((i * skillWindow[0].skillManaUseRect.height) * skillWindow[0].spaceSkillManaUse) + skillWindow[0].offset.y, skillWindow[0].skillManaUseRect.width, skillWindow[0].skillManaUseRect.height), new GUIContent("MP 0","skillWindow"+(i).ToString()));
				GUI.Box(new Rect(skillWindow[0].skillDescriptionRect.x + skillWindow[0].offset.x, skillWindow[0].skillDescriptionRect.y + ((i * skillWindow[0].skillDescriptionRect.height) * skillWindow[0].spaceDescription) + skillWindow[0].offset.y, skillWindow[0].skillDescriptionRect.width, skillWindow[0].skillDescriptionRect.height), new GUIContent(playerSkill.passiveSkill[i].description ,"skillWindow"+(i).ToString()));
			}
			
			GUI.DrawTexture(new Rect(skillWindow[0].slotSkillRect.x + skillWindow[0].offset.x, skillWindow[0].slotSkillRect.y + ((i * skillWindow[0].slotSkillRect.height) * skillWindow[0].space) + skillWindow[0].offset.y, skillWindow[0].slotSkillRect.width, skillWindow[0].slotSkillRect.height), playerSkill.passiveSkill[i].skillIcon);		
			
			if(playerStatus.status.lv < playerSkill.passiveSkill[i].unlockLevel){
				GUI.DrawTexture(new Rect(skillWindow[0].slotSkillRect.x + skillWindow[0].offset.x, skillWindow[0].slotSkillRect.y + ((i * skillWindow[0].slotSkillRect.height) * skillWindow[0].space) + skillWindow[0].offset.y, skillWindow[0].slotSkillRect.width, skillWindow[0].slotSkillRect.height), skillWindow[0].lockSkillTexture );
				if(useStyle){
					GUI.Label(new Rect(skillWindow[0].slotSkillRect.x + skillWindow[0].offset.x, skillWindow[0].slotSkillRect.y + ((i * skillWindow[0].slotSkillRect.height) * skillWindow[0].space) + skillWindow[0].offset.y, skillWindow[0].slotSkillRect.width, skillWindow[0].slotSkillRect.height), "LV "+ playerSkill.passiveSkill[i].unlockLevel.ToString(), guiStyleGroup[0].textLvUnlockStyle );	
				}else{
					GUI.Label(new Rect(skillWindow[0].slotSkillRect.x + skillWindow[0].offset.x, skillWindow[0].slotSkillRect.y + ((i * skillWindow[0].slotSkillRect.height) * skillWindow[0].space) + skillWindow[0].offset.y, skillWindow[0].slotSkillRect.width, skillWindow[0].slotSkillRect.height), "LV "+ playerSkill.passiveSkill[i].unlockLevel.ToString() );	
				}
			}
		}
		
		for(int i = 0; i < playerSkill.activeSkillBuff.Count; i++){
			if(useStyle){
				GUI.Box(new Rect(skillWindow[0].slotSkillRect.x + skillWindow[0].offset.x, skillWindow[0].slotSkillRect.y + (((i+playerSkill.passiveSkill.Count) * skillWindow[0].slotSkillRect.height) * skillWindow[0].space) + skillWindow[0].offset.y, skillWindow[0].slotSkillRect.width, skillWindow[0].slotSkillRect.height), new GUIContent("","skillWindow"+(i).ToString()), guiStyleGroup[0].skillSlotStyle);	
				GUI.Box(new Rect(skillWindow[0].descriptionRect.x + skillWindow[0].offset.x, skillWindow[0].descriptionRect.y + (((i+playerSkill.passiveSkill.Count) * skillWindow[0].descriptionRect.height) * skillWindow[0].space) + skillWindow[0].offset.y, skillWindow[0].descriptionRect.width, skillWindow[0].descriptionRect.height), new GUIContent("" ,"skillWindow"+(i).ToString()), guiStyleGroup[0].descriptionSkillSlotStyle);
			
				GUI.Box(new Rect(skillWindow[0].skillNameRect.x + skillWindow[0].offset.x, skillWindow[0].skillNameRect.y + (((i+playerSkill.passiveSkill.Count) * skillWindow[0].skillNameRect.height) * skillWindow[0].spaceSkillName) + skillWindow[0].offset.y, skillWindow[0].skillNameRect.width, skillWindow[0].skillNameRect.height), new GUIContent(playerSkill.activeSkillBuff[i].skillName ,"skillWindow"+(i).ToString()), guiStyleGroup[0].skillNameStyle);
				GUI.Box(new Rect(skillWindow[0].skillTypeRect.x + skillWindow[0].offset.x, skillWindow[0].skillTypeRect.y + (((i+playerSkill.passiveSkill.Count) * skillWindow[0].skillTypeRect.height) * skillWindow[0].spaceSkillType) + skillWindow[0].offset.y, skillWindow[0].skillTypeRect.width, skillWindow[0].skillTypeRect.height), new GUIContent(playerSkill.activeSkillBuff[i].typeSkill ,"skillWindow"+(i).ToString()), guiStyleGroup[0].skillTypeStyle);
				GUI.Box(new Rect(skillWindow[0].skillManaUseRect.x + skillWindow[0].offset.x, skillWindow[0].skillManaUseRect.y + (((i+playerSkill.passiveSkill.Count) * skillWindow[0].skillManaUseRect.height) * skillWindow[0].spaceSkillManaUse) + skillWindow[0].offset.y, skillWindow[0].skillManaUseRect.width, skillWindow[0].skillManaUseRect.height), new GUIContent("MP "+ playerSkill.activeSkillBuff[i].mpUse.ToString() ,"skillWindow"+(i).ToString()), guiStyleGroup[0].skillManaUseStyle);
				GUI.Box(new Rect(skillWindow[0].skillDescriptionRect.x + skillWindow[0].offset.x, skillWindow[0].skillDescriptionRect.y + (((i+playerSkill.passiveSkill.Count) * skillWindow[0].skillDescriptionRect.height) * skillWindow[0].spaceDescription) + skillWindow[0].offset.y, skillWindow[0].skillDescriptionRect.width, skillWindow[0].skillDescriptionRect.height), new GUIContent(playerSkill.activeSkillBuff[i].description ,"skillWindow"+(i).ToString()), guiStyleGroup[0].skillDescriptionStyle);
				
			}else{
				GUI.Box(new Rect(skillWindow[0].slotSkillRect.x + skillWindow[0].offset.x, skillWindow[0].slotSkillRect.y + (((i+playerSkill.passiveSkill.Count) * skillWindow[0].slotSkillRect.height) * skillWindow[0].space) + skillWindow[0].offset.y, skillWindow[0].slotSkillRect.width, skillWindow[0].slotSkillRect.height), new GUIContent("","skillWindow"+(i).ToString()));	
				GUI.Box(new Rect(skillWindow[0].descriptionRect.x + skillWindow[0].offset.x, skillWindow[0].descriptionRect.y + (((i+playerSkill.passiveSkill.Count) * skillWindow[0].descriptionRect.height) * skillWindow[0].space) + skillWindow[0].offset.y, skillWindow[0].descriptionRect.width, skillWindow[0].descriptionRect.height), new GUIContent("" ,"skillWindow"+(i).ToString()));
				
				GUI.Box(new Rect(skillWindow[0].skillNameRect.x + skillWindow[0].offset.x, skillWindow[0].skillNameRect.y + (((i+playerSkill.passiveSkill.Count) * skillWindow[0].skillNameRect.height) * skillWindow[0].spaceSkillName) + skillWindow[0].offset.y, skillWindow[0].skillNameRect.width, skillWindow[0].skillNameRect.height), new GUIContent(playerSkill.activeSkillBuff[i].skillName ,"skillWindow"+(i).ToString()));
				GUI.Box(new Rect(skillWindow[0].skillTypeRect.x + skillWindow[0].offset.x, skillWindow[0].skillTypeRect.y + (((i+playerSkill.passiveSkill.Count) * skillWindow[0].skillTypeRect.height) * skillWindow[0].spaceSkillType) + skillWindow[0].offset.y, skillWindow[0].skillTypeRect.width, skillWindow[0].skillTypeRect.height), new GUIContent(playerSkill.activeSkillBuff[i].typeSkill ,"skillWindow"+(i).ToString()));
				GUI.Box(new Rect(skillWindow[0].skillManaUseRect.x + skillWindow[0].offset.x, skillWindow[0].skillManaUseRect.y + (((i+playerSkill.passiveSkill.Count) * skillWindow[0].skillManaUseRect.height) * skillWindow[0].spaceSkillManaUse) + skillWindow[0].offset.y, skillWindow[0].skillManaUseRect.width, skillWindow[0].skillManaUseRect.height), new GUIContent("MP "+playerSkill.activeSkillBuff[i].mpUse.ToString() ,"skillWindow"+(i).ToString()));
				GUI.Box(new Rect(skillWindow[0].skillDescriptionRect.x + skillWindow[0].offset.x, skillWindow[0].skillDescriptionRect.y + (((i+playerSkill.passiveSkill.Count) * skillWindow[0].skillDescriptionRect.height) * skillWindow[0].spaceDescription) + skillWindow[0].offset.y, skillWindow[0].skillDescriptionRect.width, skillWindow[0].skillDescriptionRect.height), new GUIContent(playerSkill.activeSkillBuff[i].description ,"skillWindow"+(i).ToString()));
			}
			
			GUI.DrawTexture(new Rect(skillWindow[0].slotSkillRect.x + skillWindow[0].offset.x, skillWindow[0].slotSkillRect.y + (((i+playerSkill.passiveSkill.Count) * skillWindow[0].slotSkillRect.height) * skillWindow[0].space) + skillWindow[0].offset.y , skillWindow[0].slotSkillRect.width, skillWindow[0].slotSkillRect.height), playerSkill.activeSkillBuff[i].skillIcon);		
		
			if(playerStatus.status.lv < playerSkill.activeSkillBuff[i].unlockLevel){
				GUI.DrawTexture(new Rect(skillWindow[0].slotSkillRect.x + skillWindow[0].offset.x, skillWindow[0].slotSkillRect.y + (((i+playerSkill.passiveSkill.Count) * skillWindow[0].slotSkillRect.height) * skillWindow[0].space) + skillWindow[0].offset.y , skillWindow[0].slotSkillRect.width, skillWindow[0].slotSkillRect.height), skillWindow[0].lockSkillTexture);	
				
				if(useStyle){
					GUI.Label(new Rect(skillWindow[0].slotSkillRect.x + skillWindow[0].offset.x, skillWindow[0].slotSkillRect.y + (((i+playerSkill.passiveSkill.Count) * skillWindow[0].slotSkillRect.height) * skillWindow[0].space) + skillWindow[0].offset.y , skillWindow[0].slotSkillRect.width, skillWindow[0].slotSkillRect.height), "LV "+ playerSkill.activeSkillBuff[i].unlockLevel.ToString(), guiStyleGroup[0].textLvUnlockStyle);
				}else{
					GUI.Label(new Rect(skillWindow[0].slotSkillRect.x + skillWindow[0].offset.x, skillWindow[0].slotSkillRect.y + (((i+playerSkill.passiveSkill.Count) * skillWindow[0].slotSkillRect.height) * skillWindow[0].space) + skillWindow[0].offset.y , skillWindow[0].slotSkillRect.width, skillWindow[0].slotSkillRect.height), "LV "+ playerSkill.activeSkillBuff[i].unlockLevel.ToString());
				}
			}
		}
		
		for(int i = 0; i < playerSkill.activeSkillAttack.Count; i++){
			if(useStyle){
				GUI.Box(new Rect(skillWindow[0].slotSkillRect.x + skillWindow[0].offset.x, skillWindow[0].slotSkillRect.y + (((i+playerSkill.passiveSkill.Count+playerSkill.activeSkillBuff.Count) * skillWindow[0].slotSkillRect.height) * skillWindow[0].space) + skillWindow[0].offset.y, skillWindow[0].slotSkillRect.width, skillWindow[0].slotSkillRect.height), new GUIContent("","skillWindow"+(i).ToString()), guiStyleGroup[0].skillSlotStyle);	
				GUI.Box(new Rect(skillWindow[0].descriptionRect.x + skillWindow[0].offset.x, skillWindow[0].descriptionRect.y + (((i+playerSkill.passiveSkill.Count+playerSkill.activeSkillBuff.Count) * skillWindow[0].descriptionRect.height) * skillWindow[0].space) + skillWindow[0].offset.y, skillWindow[0].descriptionRect.width, skillWindow[0].descriptionRect.height), new GUIContent("" ,"skillWindow"+(i).ToString()), guiStyleGroup[0].descriptionSkillSlotStyle);
			
				GUI.Box(new Rect(skillWindow[0].skillNameRect.x + skillWindow[0].offset.x, skillWindow[0].skillNameRect.y + (((i+playerSkill.passiveSkill.Count+playerSkill.activeSkillBuff.Count) * skillWindow[0].skillNameRect.height) * skillWindow[0].spaceSkillName) + skillWindow[0].offset.y, skillWindow[0].skillNameRect.width, skillWindow[0].skillNameRect.height), new GUIContent(playerSkill.activeSkillAttack[i].skillName ,"skillWindow"+(i).ToString()), guiStyleGroup[0].skillNameStyle);
				GUI.Box(new Rect(skillWindow[0].skillTypeRect.x + skillWindow[0].offset.x, skillWindow[0].skillTypeRect.y + (((i+playerSkill.passiveSkill.Count+playerSkill.activeSkillBuff.Count) * skillWindow[0].skillTypeRect.height) * skillWindow[0].spaceSkillType) + skillWindow[0].offset.y, skillWindow[0].skillTypeRect.width, skillWindow[0].skillTypeRect.height), new GUIContent(playerSkill.activeSkillAttack[i].typeSkill ,"skillWindow"+(i).ToString()), guiStyleGroup[0].skillTypeStyle);
				GUI.Box(new Rect(skillWindow[0].skillManaUseRect.x + skillWindow[0].offset.x, skillWindow[0].skillManaUseRect.y + (((i+playerSkill.passiveSkill.Count+playerSkill.activeSkillBuff.Count) * skillWindow[0].skillManaUseRect.height) * skillWindow[0].spaceSkillManaUse) + skillWindow[0].offset.y, skillWindow[0].skillManaUseRect.width, skillWindow[0].skillManaUseRect.height), new GUIContent("MP "+playerSkill.activeSkillAttack[i].mpUse.ToString() ,"skillWindow"+(i).ToString()), guiStyleGroup[0].skillManaUseStyle);
				GUI.Box(new Rect(skillWindow[0].skillDescriptionRect.x + skillWindow[0].offset.x, skillWindow[0].skillDescriptionRect.y + (((i+playerSkill.passiveSkill.Count+playerSkill.activeSkillBuff.Count) * skillWindow[0].skillDescriptionRect.height) * skillWindow[0].spaceDescription) + skillWindow[0].offset.y, skillWindow[0].skillDescriptionRect.width, skillWindow[0].skillDescriptionRect.height), new GUIContent(playerSkill.activeSkillAttack[i].description ,"skillWindow"+(i).ToString()), guiStyleGroup[0].skillDescriptionStyle);
			
			}else{
				GUI.Box(new Rect(skillWindow[0].slotSkillRect.x + skillWindow[0].offset.x, skillWindow[0].slotSkillRect.y + (((i+playerSkill.passiveSkill.Count+playerSkill.activeSkillBuff.Count) * skillWindow[0].slotSkillRect.height) * skillWindow[0].space) + skillWindow[0].offset.y, skillWindow[0].slotSkillRect.width, skillWindow[0].slotSkillRect.height), new GUIContent("","skillWindow"+(i).ToString()));	
				GUI.Box(new Rect(skillWindow[0].descriptionRect.x + skillWindow[0].offset.x, skillWindow[0].descriptionRect.y + (((i+playerSkill.passiveSkill.Count+playerSkill.activeSkillBuff.Count) * skillWindow[0].descriptionRect.height) * skillWindow[0].space) + skillWindow[0].offset.y, skillWindow[0].descriptionRect.width, skillWindow[0].descriptionRect.height), new GUIContent("" ,"skillWindow"+(i).ToString()));
				
				GUI.Box(new Rect(skillWindow[0].skillNameRect.x + skillWindow[0].offset.x, skillWindow[0].skillNameRect.y + (((i+playerSkill.passiveSkill.Count+playerSkill.activeSkillBuff.Count) * skillWindow[0].skillNameRect.height) * skillWindow[0].spaceSkillName) + skillWindow[0].offset.y, skillWindow[0].skillNameRect.width, skillWindow[0].skillNameRect.height), new GUIContent(playerSkill.activeSkillAttack[i].skillName ,"skillWindow"+(i).ToString()));
				GUI.Box(new Rect(skillWindow[0].skillTypeRect.x + skillWindow[0].offset.x, skillWindow[0].skillTypeRect.y + (((i+playerSkill.passiveSkill.Count+playerSkill.activeSkillBuff.Count) * skillWindow[0].skillTypeRect.height) * skillWindow[0].spaceSkillType) + skillWindow[0].offset.y, skillWindow[0].skillTypeRect.width, skillWindow[0].skillTypeRect.height), new GUIContent(playerSkill.activeSkillAttack[i].typeSkill ,"skillWindow"+(i).ToString()));
				GUI.Box(new Rect(skillWindow[0].skillManaUseRect.x + skillWindow[0].offset.x, skillWindow[0].skillManaUseRect.y + (((i+playerSkill.passiveSkill.Count+playerSkill.activeSkillBuff.Count) * skillWindow[0].skillManaUseRect.height) * skillWindow[0].spaceSkillManaUse) + skillWindow[0].offset.y, skillWindow[0].skillManaUseRect.width, skillWindow[0].skillManaUseRect.height), new GUIContent("MP "+playerSkill.activeSkillAttack[i].mpUse.ToString() ,"skillWindow"+(i).ToString()));
				GUI.Box(new Rect(skillWindow[0].skillDescriptionRect.x + skillWindow[0].offset.x, skillWindow[0].skillDescriptionRect.y + (((i+playerSkill.passiveSkill.Count+playerSkill.activeSkillBuff.Count) * skillWindow[0].skillDescriptionRect.height) * skillWindow[0].spaceDescription) + skillWindow[0].offset.y, skillWindow[0].skillDescriptionRect.width, skillWindow[0].skillDescriptionRect.height), new GUIContent(playerSkill.activeSkillAttack[i].description ,"skillWindow"+(i).ToString()));
			
			}
			
			GUI.DrawTexture(new Rect(skillWindow[0].slotSkillRect.x + skillWindow[0].offset.x, skillWindow[0].slotSkillRect.y + (((i+playerSkill.passiveSkill.Count+playerSkill.activeSkillBuff.Count) * skillWindow[0].slotSkillRect.height) * skillWindow[0].space) + skillWindow[0].offset.y , skillWindow[0].slotSkillRect.width, skillWindow[0].slotSkillRect.height), playerSkill.activeSkillAttack[i].skillIcon);		
			
			if(playerStatus.status.lv < playerSkill.activeSkillAttack[i].unlockLevel){
				GUI.DrawTexture(new Rect(skillWindow[0].slotSkillRect.x + skillWindow[0].offset.x, skillWindow[0].slotSkillRect.y + (((i+playerSkill.passiveSkill.Count+playerSkill.activeSkillBuff.Count) * skillWindow[0].slotSkillRect.height) * skillWindow[0].space) + skillWindow[0].offset.y , skillWindow[0].slotSkillRect.width, skillWindow[0].slotSkillRect.height), skillWindow[0].lockSkillTexture);	
				
				if(useStyle){
					GUI.Label(new Rect(skillWindow[0].slotSkillRect.x + skillWindow[0].offset.x, skillWindow[0].slotSkillRect.y + (((i+playerSkill.passiveSkill.Count+playerSkill.activeSkillBuff.Count) * skillWindow[0].slotSkillRect.height) * skillWindow[0].space) + skillWindow[0].offset.y , skillWindow[0].slotSkillRect.width, skillWindow[0].slotSkillRect.height), "LV "+ playerSkill.activeSkillAttack[i].unlockLevel.ToString(), guiStyleGroup[0].textLvUnlockStyle);	
				}else{
					GUI.Label(new Rect(skillWindow[0].slotSkillRect.x + skillWindow[0].offset.x, skillWindow[0].slotSkillRect.y + (((i+playerSkill.passiveSkill.Count+playerSkill.activeSkillBuff.Count) * skillWindow[0].slotSkillRect.height) * skillWindow[0].space) + skillWindow[0].offset.y , skillWindow[0].slotSkillRect.width, skillWindow[0].slotSkillRect.height), "LV "+ playerSkill.activeSkillAttack[i].unlockLevel.ToString());	
				}
			}
		}
		
		if(pickupStay){
			float maxSpacePos = (skillWindow[0].scrollViewRect.height - scrollPosition.y) / (skillWindow[0].slotSkillRect.height * skillWindow[0].space);
			Vector3 mousePos = Input.mousePosition;
			if(itemImg_Pickup != null){
				GUI.DrawTexture(new Rect((mousePos.x - skillWindow[0].scrollViewPosition.x)-20-skillWindow[0].windowSkillRect.x, (Screen.height - (mousePos.y + skillWindow[0].scrollViewPosition.y-skillWindow[0].positionTunerHittest))-20-skillWindow[0].windowSkillRect.y + (scrollPosition.y - maxSpacePos* (-1*(skillWindow[0].slotSkillRect.height * skillWindow[0].space/100))), 40,40),itemImg_Pickup);	
			}
		}
		
		GUI.EndScrollView();
	}
	
	//Function Window
	private void FunctionWindowEquipment(int windowID){
		GUI.DragWindow(new Rect(0,0,10000,60));
		for(int i = 0; i < equipment[0].slotEquipmentRect.Length; i++){
			if(Item_Data.instance != null){
				if(useStyle){
					GUI.Box(new Rect(equipment[0].slotEquipmentRect[i].rect.x, equipment[0].slotEquipmentRect[i].rect.y, equipment[0].slotEquipmentRect[i].rect.width, equipment[0].slotEquipmentRect[i].rect.height), new GUIContent("","slotEquipment"+(i).ToString()), guiStyleGroup[0].gridStyle);
					if(Item_Data.instance.Get_Item(slotEquipment_item[i].item_id) != null){
						GUI.DrawTexture(new Rect(equipment[0].slotEquipmentRect[i].rect.x, equipment[0].slotEquipmentRect[i].rect.y, equipment[0].slotEquipmentRect[i].rect.width, equipment[0].slotEquipmentRect[i].rect.height), Item_Data.instance.Get_Item(slotEquipment_item[i].item_id).item_Img);
					}else{
						GUI.DrawTexture(new Rect(equipment[0].slotEquipmentRect[i].rect.x, equipment[0].slotEquipmentRect[i].rect.y, equipment[0].slotEquipmentRect[i].rect.width, equipment[0].slotEquipmentRect[i].rect.height), null_Item_Texture);
					}
				}else{
					GUI.Box(new Rect(equipment[0].slotEquipmentRect[i].rect.x, equipment[0].slotEquipmentRect[i].rect.y, equipment[0].slotEquipmentRect[i].rect.width, equipment[0].slotEquipmentRect[i].rect.height), new GUIContent("","slotEquipment"+(i).ToString()));
					if(Item_Data.instance.Get_Item(slotEquipment_item[i].item_id) != null){
						GUI.DrawTexture(new Rect(equipment[0].slotEquipmentRect[i].rect.x, equipment[0].slotEquipmentRect[i].rect.y, equipment[0].slotEquipmentRect[i].rect.width, equipment[0].slotEquipmentRect[i].rect.height), Item_Data.instance.Get_Item(slotEquipment_item[i].item_id).item_Img);
					}else{
						GUI.DrawTexture(new Rect(equipment[0].slotEquipmentRect[i].rect.x, equipment[0].slotEquipmentRect[i].rect.y, equipment[0].slotEquipmentRect[i].rect.width, equipment[0].slotEquipmentRect[i].rect.height), null_Item_Texture);
					}
					
				}
				hover = GUI.tooltip;
			}
		}
		if(pickupStay){
			Vector3 mousePos = Input.mousePosition;
			if(itemImg_Pickup != null){
				GUI.DrawTexture(new Rect(mousePos.x-20-equipment[0].windowEquipmentRect .x, (Screen.height - mousePos.y)-20-equipment[0].windowEquipmentRect.y, 40,40),itemImg_Pickup);	
			}
		}
		
		ShowDescriptionBag(equipment[0].windowEquipmentRect, id_Hover);
	}
	
	/// <summary>
	/// Functions the window drop.
	/// </summary>
	/// <param name='windowID'>
	/// Window I.
	/// </param>
	private void FunctionWindowDrop(int windowID){
		
		if(useStyle){
			GUI.Box(inventory[0].windowDropRect, new GUIContent("","WindowDrop"), guiStyleGroup[0].windowDropStyle);
			
			GUI.SetNextControlName("Input item drop");
			amountItemDrop = GUI.TextArea(new Rect(inventory[0].windowDropAmount.x,inventory[0].windowDropAmount.y,inventory[0].windowDropAmount.width, inventory[0].windowDropAmount.height),amountItemDrop,2);
		
			if(GUI.Button(inventory[0].btnEnterDropAmount, "", guiStyleGroup[0].btnEnterAmount)){
				inventory[0].enterDrop = true;	
			}
		}else{
			GUI.Box(inventory[0].windowDropRect, new GUIContent("","WindowDrop"));
			if(GUI.Button(inventory[0].btnEnterDropAmount, "")){
				inventory[0].enterDrop = true;	
			}
			GUI.SetNextControlName("Input item drop");
			amountItemDrop = GUI.TextArea(new Rect(inventory[0].windowDropAmount.x,inventory[0].windowDropAmount.y,inventory[0].windowDropAmount.width, inventory[0].windowDropAmount.height),amountItemDrop,2);
		}
		hover = GUI.tooltip;
	}
	
	/// <summary>
	/// Functions the inventory window.
	/// </summary>
	private void FunctionWindow(int windowID){
		GUI.DragWindow(new Rect(0,0,10000,60));
		for(int i = 0; i < inventory[0].row; i++){
			for(int j = 0; j < inventory[0].colum; j++){
				int index = i+(j*inventory[0].row);
				if(useStyle){
					GUI.Box(new Rect(inventory[0].gridRect.x+(i*inventory[0].gridRect.width),inventory[0].gridRect.y+(j*inventory[0].gridRect.height),inventory[0].gridRect.width,inventory[0].gridRect.height), new GUIContent("",""+(index).ToString()), guiStyleGroup[0].gridStyle);
					if(Item_Data.instance != null){
						if(Item_Data.instance.Get_Item(bag_item[index].item_id) != null){
							GUI.DrawTexture(new Rect(inventory[0].gridRect.x+(i*inventory[0].gridRect.width),inventory[0].gridRect.y+(j*inventory[0].gridRect.height),inventory[0].gridRect.width,inventory[0].gridRect.height), Item_Data.instance.Get_Item(bag_item[index].item_id).item_Img);
							GUI.Label(new Rect(inventory[0].itemAmountRect.x+(i*inventory[0].itemAmountRect.width),inventory[0].itemAmountRect.y+(j*inventory[0].itemAmountRect.height),inventory[0].itemAmountRect.width,inventory[0].itemAmountRect.height), bag_item[index].item_amount.ToString(), guiStyleGroup[0].textAmountItemStyle);
						}else{
							GUI.DrawTexture(new Rect(inventory[0].gridRect.x+(i*inventory[0].gridRect.width),inventory[0].gridRect.y+(j*inventory[0].gridRect.height),inventory[0].gridRect.width,inventory[0].gridRect.height), null_Item_Texture);
						}
					}
					GUI.Box(inventory[0].goldRect, money.ToString(), guiStyleGroup[0].goldStyle);
				}else{
					GUI.Box(new Rect(inventory[0].gridRect.x+(i*inventory[0].gridRect.width),inventory[0].gridRect.y+(j*inventory[0].gridRect.height),inventory[0].gridRect.width,inventory[0].gridRect.height), new GUIContent("",""+(index).ToString()));
					if(Item_Data.instance != null){
						if(Item_Data.instance.Get_Item(bag_item[index].item_id) != null){
							GUI.DrawTexture(new Rect(inventory[0].gridRect.x+(i*inventory[0].gridRect.width),inventory[0].gridRect.y+(j*inventory[0].gridRect.height),inventory[0].gridRect.width,inventory[0].gridRect.height), Item_Data.instance.Get_Item(bag_item[index].item_id).item_Img);
							GUI.Label(new Rect(inventory[0].itemAmountRect.x+(i*inventory[0].itemAmountRect.width),inventory[0].itemAmountRect.y+(j*inventory[0].itemAmountRect.height),inventory[0].itemAmountRect.width,inventory[0].itemAmountRect.height), bag_item[index].item_amount.ToString(), guiStyleGroup[0].textAmountItemStyle);
						}else{
							GUI.DrawTexture(new Rect(inventory[0].gridRect.x+(i*inventory[0].gridRect.width),inventory[0].gridRect.y+(j*inventory[0].gridRect.height),inventory[0].gridRect.width,inventory[0].gridRect.height), null_Item_Texture);
						}
					}
					GUI.Box(inventory[0].goldRect, money.ToString());
				}
				hover = GUI.tooltip;
			}
		}
		
		GUI.DrawTexture(inventory[0].coinIconRect, coinIcon);
		
		if(pickupStay){
			Vector3 mousePos = Input.mousePosition;
			if(itemImg_Pickup != null){
				GUI.DrawTexture(new Rect(mousePos.x-20-inventory[0].windowInventoryRect.x, (Screen.height - mousePos.y)-20-inventory[0].windowInventoryRect.y, 40,40),itemImg_Pickup);	
			}
		}
		
		ShowDescriptionBag(inventory[0].windowInventoryRect, id_Hover);
	}
	
	private int CheckSlotInventory(){
		for(int i = 0; i < inventory[0].row * inventory[0].colum; i++){
			Vector2 startPos = Vector3.zero;
			Vector2 endPos = Vector2.zero;
			startPos.x = inventory[0].windowInventoryRect.x + inventory[0].gridRect.x;
			startPos.y = inventory[0].windowInventoryRect.y + inventory[0].gridRect.y;
			endPos.x = inventory[0].windowInventoryRect.x + (inventory[0].gridRect.x + inventory[0].gridRect.width);
			endPos.y = inventory[0].windowInventoryRect.y + (inventory[0].gridRect.y + inventory[0].gridRect.height);
			if(HitTestWindow(startPos, endPos)){
				return i;	
			}
		}

		return -1;
	}
	
	private int CheckSlotShop(){
		float maxSpacePos = (itemShop[0].scrollViewRect.height - scrollPositionShop.y) / (itemShop[0].scrollViewRect.height * itemShop[0].space);
		for(int i = 0; i < itemShop[0].itemID.Count; i++){
			Vector2 startPos = Vector3.zero;
			Vector2 endPos = Vector2.zero;
			
			
			startPos.x = itemShop[0].windowItemShopRect.x + itemShop[0].slotItemRect.x + itemShop[0].scrollViewPosition.x;
			startPos.y = itemShop[0].windowItemShopRect.y + (itemShop[0].scrollViewPosition.y - skillWindow[0].positionTunerHittest ) + itemShop[0].slotItemRect.y + ((i * itemShop[0].slotItemRect.height) * itemShop[0].space) + itemShop[0].offset.y;
			endPos.x = itemShop[0].windowItemShopRect.x + (itemShop[0].slotItemRect.x + itemShop[0].slotItemRect.width) + itemShop[0].scrollViewPosition.x;
			endPos.y = itemShop[0].windowItemShopRect.y + (itemShop[0].scrollViewPosition.y - skillWindow[0].positionTunerHittest) + ((itemShop[0].slotItemRect.y + ((i * itemShop[0].slotItemRect.height) * itemShop[0].space) + itemShop[0].offset.y) + itemShop[0].slotItemRect.height);
			
			startPos.y -= (scrollPositionShop.y - maxSpacePos* (-1*(itemShop[0].slotItemRect.height * itemShop[0].space/100)));
			endPos.y -= (scrollPositionShop.y - maxSpacePos* (-1*(itemShop[0].slotItemRect.height * itemShop[0].space/100)));
			
			if(skillWindow[0].openHittestDebug){
				GUI.DrawTexture(new Rect(startPos.x, startPos.y, 10, 10), imgDebugCollision);
				GUI.DrawTexture(new Rect(endPos.x, endPos.y, 10, 10), imgDebugCollision);
			}
			
			if(HitTestWindow(startPos, endPos)){
				typeSkillHover = TypeSkillHover.Passive;
				return i;	
			}
		}
		
		return -1;	
	}
	
	private int CheckSlotSkill(){
		float maxSpacePos = (skillWindow[0].scrollViewRect.height - scrollPosition.y) / (skillWindow[0].slotSkillRect.height * skillWindow[0].space);
		for(int i = 0; i < playerSkill.passiveSkill.Count; i++){
			Vector2 startPos = Vector3.zero;
			Vector2 endPos = Vector2.zero;
			
			
			startPos.x = skillWindow[0].windowSkillRect.x + skillWindow[0].slotSkillRect.x + skillWindow[0].scrollViewPosition.x;
			startPos.y = skillWindow[0].windowSkillRect.y + (skillWindow[0].scrollViewPosition.y - skillWindow[0].positionTunerHittest ) + skillWindow[0].slotSkillRect.y + ((i * skillWindow[0].slotSkillRect.height) * skillWindow[0].space) + skillWindow[0].offset.y;
			endPos.x = skillWindow[0].windowSkillRect.x + (skillWindow[0].slotSkillRect.x + skillWindow[0].slotSkillRect.width) + skillWindow[0].scrollViewPosition.x;
			endPos.y = skillWindow[0].windowSkillRect.y + (skillWindow[0].scrollViewPosition.y - skillWindow[0].positionTunerHittest) + ((skillWindow[0].slotSkillRect.y + ((i * skillWindow[0].slotSkillRect.height) * skillWindow[0].space) + skillWindow[0].offset.y) + skillWindow[0].slotSkillRect.height);
			
			startPos.y -= (scrollPosition.y - maxSpacePos* (-1*(skillWindow[0].slotSkillRect.height * skillWindow[0].space/100)));
			endPos.y -= (scrollPosition.y - maxSpacePos* (-1*(skillWindow[0].slotSkillRect.height * skillWindow[0].space/100)));
			
			if(skillWindow[0].openHittestDebug){
				GUI.DrawTexture(new Rect(startPos.x, startPos.y, 10, 10), imgDebugCollision);
				GUI.DrawTexture(new Rect(endPos.x, endPos.y, 10, 10), imgDebugCollision);
			}
			
			if(HitTestWindow(startPos, endPos)){
				typeSkillHover = TypeSkillHover.Passive;
				return i;	
			}
		}
		
		for(int i = 0; i < playerSkill.activeSkillBuff.Count; i++){
			Vector2 startPos = Vector3.zero;
			Vector2 endPos = Vector2.zero;
			startPos.x = skillWindow[0].windowSkillRect.x + skillWindow[0].slotSkillRect.x + skillWindow[0].scrollViewPosition.x;
			startPos.y = skillWindow[0].windowSkillRect.y + (skillWindow[0].scrollViewPosition.y - skillWindow[0].positionTunerHittest ) + skillWindow[0].slotSkillRect.y + (((i+playerSkill.passiveSkill.Count) * skillWindow[0].slotSkillRect.height) * skillWindow[0].space) + skillWindow[0].offset.y;
			endPos.x = skillWindow[0].windowSkillRect.x + (skillWindow[0].slotSkillRect.x + skillWindow[0].slotSkillRect.width) + skillWindow[0].scrollViewPosition.x;
			endPos.y = skillWindow[0].windowSkillRect.y + (skillWindow[0].scrollViewPosition.y - skillWindow[0].positionTunerHittest ) + ((skillWindow[0].slotSkillRect.y + (((i+playerSkill.passiveSkill.Count) * skillWindow[0].slotSkillRect.height) * skillWindow[0].space) + skillWindow[0].offset.y) + skillWindow[0].slotSkillRect.height);
			
			startPos.y -= (scrollPosition.y - maxSpacePos* (-1*(skillWindow[0].slotSkillRect.height * skillWindow[0].space/100)));
			endPos.y -= (scrollPosition.y - maxSpacePos* (-1*(skillWindow[0].slotSkillRect.height * skillWindow[0].space/100)));
			
			if(skillWindow[0].openHittestDebug){
				GUI.DrawTexture(new Rect(startPos.x, startPos.y, 10, 10), imgDebugCollision);
				GUI.DrawTexture(new Rect(endPos.x, endPos.y, 10, 10), imgDebugCollision);
			}
			
			if(HitTestWindow(startPos, endPos)){
				typeSkillHover = TypeSkillHover.Buff;
				return i+playerSkill.passiveSkill.Count;	
			}
		}
		
		for(int i = 0; i < playerSkill.activeSkillAttack.Count; i++){
			Vector2 startPos = Vector3.zero;
			Vector2 endPos = Vector2.zero;
			startPos.x = skillWindow[0].windowSkillRect.x + skillWindow[0].slotSkillRect.x + skillWindow[0].scrollViewPosition.x;
			startPos.y = skillWindow[0].windowSkillRect.y + (skillWindow[0].scrollViewPosition.y - skillWindow[0].positionTunerHittest ) + skillWindow[0].slotSkillRect.y + (((i+playerSkill.passiveSkill.Count+playerSkill.activeSkillBuff.Count) * skillWindow[0].slotSkillRect.height) * skillWindow[0].space) + skillWindow[0].offset.y ;
			endPos.x = skillWindow[0].windowSkillRect.x + (skillWindow[0].slotSkillRect.x + skillWindow[0].slotSkillRect.width) + skillWindow[0].scrollViewPosition.x;
			endPos.y = skillWindow[0].windowSkillRect.y + (skillWindow[0].scrollViewPosition.y - skillWindow[0].positionTunerHittest ) + ((skillWindow[0].slotSkillRect.y + (((i+playerSkill.passiveSkill.Count+playerSkill.activeSkillBuff.Count) * skillWindow[0].slotSkillRect.height) * skillWindow[0].space) + skillWindow[0].offset.y) + skillWindow[0].slotSkillRect.height);
			
			startPos.y -= (scrollPosition.y - maxSpacePos* (-1*(skillWindow[0].slotSkillRect.height * skillWindow[0].space/100)));
			endPos.y -= (scrollPosition.y - maxSpacePos* (-1*(skillWindow[0].slotSkillRect.height * skillWindow[0].space/100)));
			
			if(skillWindow[0].openHittestDebug){
				GUI.DrawTexture(new Rect(startPos.x, startPos.y, 10, 10), imgDebugCollision);
				GUI.DrawTexture(new Rect(endPos.x, endPos.y, 10, 10), imgDebugCollision);
			}
			
			if(HitTestWindow(startPos, endPos)){
				typeSkillHover = TypeSkillHover.Attack;
				return i+playerSkill.passiveSkill.Count+playerSkill.activeSkillBuff.Count;	
			}
		}
		
		typeSkillHover = TypeSkillHover.Null;
		return -1;	
	}
	
	/// <summary>
	/// Checks the slot Equipment.
	/// </summary>
	/// <returns>
	/// The slot Equipment.
	/// </returns>
	private int CheckSlotEquipment(){
		for(int i = 0; i < equipment[0].slotEquipmentRect.Length; i++){
			Vector2 startPos = Vector3.zero;
			Vector2 endPos = Vector2.zero;
			startPos.x = equipment[0].windowEquipmentRect.x + equipment[0].slotEquipmentRect[i].rect.x;
			startPos.y = equipment[0].windowEquipmentRect.y + equipment[0].slotEquipmentRect[i].rect.y;
			endPos.x = equipment[0].windowEquipmentRect.x + (equipment[0].slotEquipmentRect[i].rect.x + equipment[0].slotEquipmentRect[i].rect.width);
			endPos.y = equipment[0].windowEquipmentRect.y + (equipment[0].slotEquipmentRect[i].rect.y + equipment[0].slotEquipmentRect[i].rect.height);
			if(HitTestWindow(startPos, endPos)){
				return i;	
			}
		}
		
		return -1;
	}
	
	/// <summary>
	/// Checks the hover inventory.
	/// </summary>
	/// <returns>
	/// The hover inventory.
	/// </returns>
	private bool CheckHoverInventory(){
		if((HitTestWindow(new Vector2(inventory[0].windowInventoryRect.x, inventory[0].windowInventoryRect.y),
							new Vector2(inventory[0].windowInventoryRect.x+inventory[0].windowInventoryRect.width, inventory[0].windowInventoryRect.y+inventory[0].windowInventoryRect.height)) && inventory[0].openInventory)){
			return true;	
		}else{
			return false;	
		}
	}
	
	private bool CheckHoverAmoutDrop(){
		if((HitTestWindow(new Vector2(inventory[0].windowDropRect.x, inventory[0].windowDropRect.y),
							new Vector2(inventory[0].windowDropRect.x+inventory[0].windowDropRect.width, inventory[0].windowDropRect.y+inventory[0].windowDropRect.height)) && showDropAmount)){
			return true;	
		}else{
			return false;	
		}
	}
	
	private bool CheckHoverStatWindow(){
		if((HitTestWindow(new Vector2(statWindow[0].windowStatRect.x, statWindow[0].windowStatRect.y),
							new Vector2(statWindow[0].windowStatRect.x+statWindow[0].windowStatRect.width, statWindow[0].windowStatRect.y+statWindow[0].windowStatRect.height)) && statWindow[0].openStatWindow)){
			return true;	
		}else{
			return false;	
		}	
	}
	
	public bool CheckHoverItemShop(){
		if((HitTestWindow(new Vector2(itemShop[0].windowItemShopRect.x, itemShop[0].windowItemShopRect.y),
							new Vector2(itemShop[0].windowItemShopRect.x+itemShop[0].windowItemShopRect.width, itemShop[0].windowItemShopRect.y+itemShop[0].windowItemShopRect.height)) && itemShop[0].openItemShop)){
			return true;	
		}else{
			return false;	
		}
	}
	
	/// <summary>
	/// Checks the hover Equipment.
	/// </summary>
	/// <returns>
	/// The hover Equipment.
	/// </returns>
	private bool CheckHoverEquipment(){
		if((HitTestWindow(new Vector2(equipment[0].windowEquipmentRect .x, equipment[0].windowEquipmentRect.y),
							new Vector2(equipment[0].windowEquipmentRect.x+equipment[0].windowEquipmentRect.width, equipment[0].windowEquipmentRect.y+equipment[0].windowEquipmentRect.height)) && equipment[0].openEquipment)){
			return true;
		}else{
			return false;	
		}
	}
	
	public bool CheckHoverSkillWindow(){
		if(HitTestWindow(new Vector2(skillWindow[0].windowSkillRect.x, skillWindow[0].windowSkillRect.y),
			new Vector2(skillWindow[0].windowSkillRect.x+skillWindow[0].windowSkillRect.width, skillWindow[0].windowSkillRect.y+skillWindow[0].windowSkillRect.height)) && skillWindow[0].openSkillWindow){
			return true;	
		}else{
			return false;	
		}
	}
	
	/// <summary>
	/// Checks the hover slot hotkey.
	/// </summary>
	/// <returns>
	/// The hover slot hotkey.
	/// </returns>
	private int CheckHoverSlotHotkey(){
		for(int i = 0; i < hotKeySlot[0].amountSlot; i++){
			Vector2 startPos = Vector3.zero;
			Vector2 endPos = Vector2.zero;
			startPos.x = GUI_Calculate.RectWithScrren_WidthAndHeight_SizeWidth(new Vector2(hotKeySlot[0].hot_key_Rect.x + ((hotKeySlot[0].hot_key_Rect.width + hotKeySlot[0].space_Slot) * i), hotKeySlot[0].hot_key_Rect.y), new Vector2(hotKeySlot[0].hot_key_Rect.width, hotKeySlot[0].hot_key_Rect.height)).x;
			startPos.y = GUI_Calculate.RectWithScrren_WidthAndHeight_SizeWidth(new Vector2(hotKeySlot[0].hot_key_Rect.x + ((hotKeySlot[0].hot_key_Rect.width + hotKeySlot[0].space_Slot) * i), hotKeySlot[0].hot_key_Rect.y), new Vector2(hotKeySlot[0].hot_key_Rect.width, hotKeySlot[0].hot_key_Rect.height)).y;
			endPos.x = GUI_Calculate.RectWithScrren_WidthAndHeight_SizeWidth(new Vector2(hotKeySlot[0].hot_key_Rect.x + ((hotKeySlot[0].hot_key_Rect.width + hotKeySlot[0].space_Slot) * i), hotKeySlot[0].hot_key_Rect.y), new Vector2(hotKeySlot[0].hot_key_Rect.width, hotKeySlot[0].hot_key_Rect.height)).x + GUI_Calculate.RectWithScrren_WidthAndHeight_SizeWidth(new Vector2(hotKeySlot[0].hot_key_Rect.x + ((hotKeySlot[0].hot_key_Rect.width + hotKeySlot[0].space_Slot) * i), hotKeySlot[0].hot_key_Rect.y), new Vector2(hotKeySlot[0].hot_key_Rect.width, hotKeySlot[0].hot_key_Rect.height)).width;
			endPos.y = GUI_Calculate.RectWithScrren_WidthAndHeight_SizeWidth(new Vector2(hotKeySlot[0].hot_key_Rect.x + ((hotKeySlot[0].hot_key_Rect.width + hotKeySlot[0].space_Slot) * i), hotKeySlot[0].hot_key_Rect.y), new Vector2(hotKeySlot[0].hot_key_Rect.width, hotKeySlot[0].hot_key_Rect.height)).y + GUI_Calculate.RectWithScrren_WidthAndHeight_SizeWidth(new Vector2(hotKeySlot[0].hot_key_Rect.x + ((hotKeySlot[0].hot_key_Rect.width + hotKeySlot[0].space_Slot) * i), hotKeySlot[0].hot_key_Rect.y), new Vector2(hotKeySlot[0].hot_key_Rect.width, hotKeySlot[0].hot_key_Rect.height)).height;
			if(HitTestWindow(startPos, endPos)){
				return i;	
			}
		}

		return -1;
	}
	
	/// <summary>
	/// Hits the test window.
	/// </summary>
	/// <returns>
	/// The test window.
	/// </returns>
	/// <param name='startPosition'>
	/// If set to <c>true</c> start position.
	/// </param>
	/// <param name='endPosition'>
	/// If set to <c>true</c> end position.
	/// </param>
	public static bool HitTestWindow(Vector2 startPosition, Vector2 endPosition){
		if(Input.mousePosition.x > startPosition.x && Input.mousePosition.x < endPosition.x
			&& Screen.height-Input.mousePosition.y > startPosition.y && Screen.height-Input.mousePosition.y < endPosition.y){
			return true;
		}else{
			return false;	
		}
	}
	
	/// <summary>
	/// Pickup the item.
	/// </summary>
	/// <param name='item_obj'>
	/// Item_obj.
	/// </param>
	public bool Pickup_Item(Item_Obj item_obj){
		int round = inventory[0].row*inventory[0].colum;
		bool removeItemOnFloor = false;
		if(item_obj.gold == false){
			for(int i = 0; i < round; i++){
				if(bag_item[i].item_id == 0){
					bag_item[i].item_id = item_obj.item_id;
					bag_item[i].item_amount += item_obj.item_amount;
					bag_item[i].equipmentType = item_obj.euipmentType;
					removeItemOnFloor = true;
					i = round;
				}else{
					if(bag_item[i].item_id == item_obj.item_id && item_obj.euipmentType == Item_Data.Equipment_Type.Null){
						bag_item[i].item_amount += item_obj.item_amount;
						removeItemOnFloor = true;
						i = round;
					}
				}
			}
		}else{
			money += item_obj.gold_price;
			removeItemOnFloor = true;
		}
		
		if(removeItemOnFloor){
			return true;	
		}else{
			return false;
		}
	}
	
	/// <summary>
	/// Drop the item.
	/// </summary>
	/// <param name='_indexInventory'>
	/// _index inventory.
	/// </param>
	/// <param name='pos'>
	/// Position.
	/// </param>
	/// <param name='amount'>
	/// Amount.
	/// </param>
	public void Drop_Item(int _indexInventory, Vector3 pos, int amount){
		Vector3 posItemDrop = Vector3.zero;
		posItemDrop.x = Random.Range(pos.x-1, pos.x+1);
		posItemDrop.y = pos.y;
		posItemDrop.z = Random.Range(pos.z-1, pos.z+1);
		GameObject go = (GameObject)Instantiate(item_Obj_Pref, posItemDrop, Quaternion.identity);
		
		int sumAmount = 0;
		if(amount > bag_item[_indexInventory].item_amount){
			sumAmount = bag_item[_indexInventory].item_amount;	
		}else{
			sumAmount = amount;	
		}
		go.GetComponent<Item_Obj>().item_id = item_pickup.item_id;
		go.GetComponent<Item_Obj>().item_amount = sumAmount;
		go.GetComponent<Item_Obj>().SetItem();
		bag_item[_indexInventory].item_amount -= amount;
		if(bag_item[_indexInventory].item_amount <= 0){
			bag_item[_indexInventory].item_id = 0;
			bag_item[_indexInventory].item_amount = 0;
			bag_item[_indexInventory].equipmentType = Item_Data.Equipment_Type.Null;
		}
		item_pickup = null;
		Debug.Log("Drop");
	}
	
	public void Use_Item(int _itemID){
		Item_Data.Item item = Item_Data.instance.Get_Item(_itemID);
		
		if(item != null){
			if(item.equipment_Type == Item_Data.Equipment_Type.Null && item.gold == false){
				if(item.potion == true){
					
					AddEffectItemToPlayer(item);
					
					bag_item[indexInventory].item_amount -= 1;
					if(bag_item[indexInventory].item_amount <= 0){
						bag_item[indexInventory].item_id = 0;
						bag_item[indexInventory].item_amount = 0;
						bag_item[indexInventory].equipmentType = Item_Data.Equipment_Type.Null;
					}
				}
			}else if(item.equipment_Type != Item_Data.Equipment_Type.Null){
				Debug.Log("Equipment");
				for(int i = 0; i < equipment[0].slotEquipmentRect.Length; i++){
					if(item.equipment_Type == equipment[0].slotEquipmentRect[i].equipType){
						
						if(playerController.classType == item.require_Class || item.require_Class == ClassType.None)
						{
							if(slotEquipment_item[i].item_id == 0){
							AddStatEquipmentToPlayer(item);
							slotEquipment_item[i].item_id = item.item_ID;
							slotEquipment_item[i].equipmentType = item.equipment_Type;
							bag_item[indexInventory].item_id = 0;
							bag_item[indexInventory].item_amount = 0;
							bag_item[indexInventory].equipmentType = Item_Data.Equipment_Type.Null;
							bag_item[indexInventory].linkIndex = -1;
						}else{
							Item_Obj _item_obj = new Item_Obj();
							_item_obj.item_id = slotEquipment_item[i].item_id;
							_item_obj.euipmentType = slotEquipment_item[i].equipmentType;
							_item_obj.item_amount = 1;
							
							RemoveStatEquipmentToPlayer(Item_Data.instance.Get_Item(slotEquipment_item[i].item_id));
							
							slotEquipment_item[i].item_id = item.item_ID;
							slotEquipment_item[i].equipmentType = item.equipment_Type;
							
							AddStatEquipmentToPlayer(item);
							
							
							bag_item[indexInventory].item_id = 0;
							bag_item[indexInventory].item_amount = 0;
							bag_item[indexInventory].equipmentType = Item_Data.Equipment_Type.Null;
							bag_item[indexInventory].linkIndex = -1;
							
							Pickup_Item(_item_obj);
							}
						}	
					}
				}
			}
		}
		
	}
	
	private void AddEffectItemToPlayer(Item_Data.Item item){
		
		if(item.item_Effect != null)
		{
			Instantiate(item.item_Effect,transform.position,transform.rotation);	
		}
		
		if(item.item_Sfx != null)
		{
			AudioSource.PlayClipAtPoint(item.item_Sfx,transform.position);
		}
		
		//Add effect item to Player
		playerStatus.statusCal.hp += item.hp;
		playerStatus.statusCal.mp += item.mp;
		playerStatus.UpdateAttribute();
	}
	
	private void AddStatEquipmentToPlayer(Item_Data.Item item){
		
		//Add effect item to Player
		playerStatus.statusAdd.atk += item.atk;
		playerStatus.statusAdd.atkSpd += item.atkSpd;
		playerStatus.statusAdd.criticalRate += item.criPercent;
		playerStatus.statusAdd.def += item.def;
		playerStatus.statusAdd.hit += item.hit;
		playerStatus.statusAdd.hp += item.hp;
		playerStatus.statusAdd.movespd += item.moveSpd;
		playerStatus.statusAdd.mp += item.mp;
		playerStatus.statusAdd.spd += item.spd;
		playerStatus.statusAdd.atkRange += item.atkRange;
		
		playerStatus.UpdateAttribute();
	}
	
	private void RemoveStatEquipmentToPlayer(Item_Data.Item item){
	
		//remove effect item to Player
		playerStatus.statusAdd.atk -= item.atk;
		playerStatus.statusAdd.atkSpd -= item.atkSpd;
		playerStatus.statusAdd.criticalRate -= item.criPercent;
		playerStatus.statusAdd.def -= item.def;
		playerStatus.statusAdd.hit -= item.hit;
		playerStatus.statusAdd.hp -= item.hp;
		playerStatus.statusAdd.movespd -= item.moveSpd;
		playerStatus.statusAdd.mp -= item.mp;
		playerStatus.statusAdd.spd -= item.spd;
		playerStatus.statusAdd.atkRange -= item.atkRange;
		
		playerStatus.UpdateAttribute();
	}
	
	public void Save(){
		for(int i = 0; i < bag_item.Count; i++){
			PlayerPrefs.SetInt("Bag_Item_ID["+i+"]", bag_item[i].item_id);
			PlayerPrefs.SetInt("Bag_Item_Amount["+i+"]", bag_item[i].item_amount);
			PlayerPrefs.SetInt("Bag_Equip_Type["+i+"]", (int)bag_item[i].equipmentType);
			PlayerPrefs.SetInt("Bag_Link_Index["+i+"]", bag_item[i].linkIndex);
			if(bag_item[i].isItem == true){
				PlayerPrefs.SetString("Bag_Is_Item["+i+"]", "True");	
			}else{
				PlayerPrefs.SetString("Bag_Is_Item["+i+"]", "False");
			}
			PlayerPrefs.SetInt("Bag_Type_Skill["+i+"]", (int)bag_item[i].typeSkill);
		}
		
		for(int i = 0; i < hotKeySlot[0].hotKeyslot.Count; i++){
			PlayerPrefs.SetInt("HotKey_Item_ID["+i+"]", hotKeySlot[0].hotKeyslot[i].item_id);	
			PlayerPrefs.SetInt("HotKey_Item_Amount["+i+"]", hotKeySlot[0].hotKeyslot[i].item_amount);
			PlayerPrefs.SetInt("HotKey_Equip_Type["+i+"]", (int)hotKeySlot[0].hotKeyslot[i].equipmentType);
			PlayerPrefs.SetInt("HotKey_Link_Index["+i+"]", hotKeySlot[0].hotKeyslot[i].linkIndex);
			if(hotKeySlot[0].hotKeyslot[i].isItem == true){
				PlayerPrefs.SetString("HotKey_Is_Item["+i+"]", "True");
			}else{
				PlayerPrefs.SetString("HotKey_Is_Item["+i+"]", "False");
			}
			PlayerPrefs.SetInt("HotKey_Type_Skill["+i+"]", (int)hotKeySlot[0].hotKeyslot[i].typeSkill);
		}
		
		for(int i = 0; i < slotEquipment_item.Count; i++){
			PlayerPrefs.SetInt("Equip_Item_ID["+i+"]", slotEquipment_item[i].item_id);
			PlayerPrefs.SetInt("Equip_Type["+i+"]", (int)slotEquipment_item[i].equipmentType);
		}
		
		PlayerPrefs.SetInt("Money", money);
		
		PlayerPrefs.SetInt("DefAtk",statWindow[0].defAtk);
		PlayerPrefs.SetInt("DefDef",statWindow[0].defDef);
		PlayerPrefs.SetInt("DefHit",statWindow[0].defHit);
		PlayerPrefs.SetInt("DefSpd",statWindow[0].defSpeed);
		PlayerPrefs.SetInt("DefPoint",statWindow[0].defPoint);
		
		
	}
	
	public void Load(){
		for(int i = 0; i < bag_item.Count; i++){
			bag_item[i].item_id = PlayerPrefs.GetInt("Bag_Item_ID["+i+"]");
			bag_item[i].item_amount = PlayerPrefs.GetInt("Bag_Item_Amount["+i+"]");
			bag_item[i].equipmentType = (Item_Data.Equipment_Type)PlayerPrefs.GetInt("Bag_Equip_Type["+i+"]");
			bag_item[i].linkIndex = PlayerPrefs.GetInt("Bag_Link_Index["+i+"]",-1);
			if(PlayerPrefs.GetString("Bag_Is_Item["+i+"]") == "True"){
				bag_item[i].isItem = true;	
			}else{
				bag_item[i].isItem = false;
			}
			bag_item[i].typeSkill = (TypeSkillHover)PlayerPrefs.GetInt("Bag_Type_Skill["+i+"]");
		}
		
		for(int i = 0; i < hotKeySlot[0].hotKeyslot.Count; i++){
			hotKeySlot[0].hotKeyslot[i].item_id = PlayerPrefs.GetInt("HotKey_Item_ID["+i+"]");	
			hotKeySlot[0].hotKeyslot[i].item_amount = PlayerPrefs.GetInt("HotKey_Item_Amount["+i+"]");
			hotKeySlot[0].hotKeyslot[i].equipmentType = (Item_Data.Equipment_Type)PlayerPrefs.GetInt("HotKey_Equip_Type["+i+"]");
			hotKeySlot[0].hotKeyslot[i].linkIndex = PlayerPrefs.GetInt("HotKey_Link_Index["+i+"]",-1);
			if(PlayerPrefs.GetString("HotKey_Is_Item["+i+"]") == "True"){
				hotKeySlot[0].hotKeyslot[i].isItem = true;
			}else{
				hotKeySlot[0].hotKeyslot[i].isItem = false;
			}
			hotKeySlot[0].hotKeyslot[i].typeSkill = (TypeSkillHover)PlayerPrefs.GetInt("HotKey_Type_Skill["+i+"]");
		}
		
		for(int i = 0; i < slotEquipment_item.Count; i++){
			slotEquipment_item[i].item_id = PlayerPrefs.GetInt("Equip_Item_ID["+i+"]");
			slotEquipment_item[i].equipmentType = (Item_Data.Equipment_Type)PlayerPrefs.GetInt("Equip_Type["+i+"]");
			if(slotEquipment_item[i].item_id != 0){
				AddStatEquipmentToPlayer(Item_Data.instance.Get_Item(slotEquipment_item[i].item_id));	
			}
		}
		
		money = PlayerPrefs.GetInt("Money");
		
		statWindow[0].defAtk = PlayerPrefs.GetInt("DefAtk");
		statWindow[0].defDef = PlayerPrefs.GetInt("DefDef");
		statWindow[0].defHit = PlayerPrefs.GetInt("DefHit");
		statWindow[0].defSpeed = PlayerPrefs.GetInt("DefSpd");
		statWindow[0].defPoint = PlayerPrefs.GetInt("DefPoint");
	}
	
	public void Reset(){
		for(int i = 0; i < bag_item.Count; i++){
			PlayerPrefs.SetInt("Bag_Item_ID["+i+"]", 0);
			PlayerPrefs.SetInt("Bag_Item_Amount["+i+"]", 0);
			PlayerPrefs.SetInt("Bag_Equip_Type["+i+"]", 0);
			PlayerPrefs.SetInt("Bag_Link_Index["+i+"]", -1);
			PlayerPrefs.SetString("Bag_Is_Item["+i+"]", "False");
			PlayerPrefs.SetInt("Bag_Type_Skill["+i+"]", 0);
		}
		
		for(int i = 0; i < hotKeySlot[0].hotKeyslot.Count; i++){
			PlayerPrefs.SetInt("HotKey_Item_ID["+i+"]", 0);	
			PlayerPrefs.SetInt("HotKey_Item_Amount["+i+"]",0);
			PlayerPrefs.SetInt("HotKey_Equip_Type["+i+"]", 0);
			PlayerPrefs.SetInt("HotKey_Link_Index["+i+"]", -1);
			PlayerPrefs.SetString("HotKey_Is_Item["+i+"]", "False");
			PlayerPrefs.SetInt("HotKey_Type_Skill["+i+"]", 0);
		}
		
		for(int i = 0; i < slotEquipment_item.Count; i++){
			PlayerPrefs.SetInt("Equip_Item_ID["+i+"]", 0);
			PlayerPrefs.SetInt("Equip_Type["+i+"]", 0);
		}
		
		PlayerPrefs.SetInt("Money", startMoney);
	}
	
	//Gizmos Variable
	public bool showAreaActive;
	
	void OnDrawGizmosSelected(){
		if(showAreaActive)
		{
			Gizmos.color = new Color(Color.green.r, Color.green.g, Color.green.b, 0.2f);
			Gizmos.DrawSphere(transform.parent.position, rangeDetect);
		}
		
	}
	
}

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

//only from Ubrin character system!
public class PlayerEquip : MonoBehaviour 
{
	public static bool IsUbrin = false;
	public static List<EquipedItem> itemToEquip;
	public static List<EquipedItem> itemsToUnequip;
	
	
	private GameObject HelmInst;
	private GameObject ChestInst;
	private GameObject ShouldInst;
	private GameObject WristsInst;
	private GameObject BootsInst;
	private GameObject RhInst;
	private GameObject LhInst;
	
	// Use this for initialization
	void Start () 
	{
		if (!IsUbrin)
			return;
		itemsToUnequip = new List<EquipedItem>();
		itemToEquip = new List<EquipedItem>();
		ChangeHeroVisibility(false);
	}
	
	// Update is called once per frame
	void Update () 
	{	
		if (!IsUbrin)
			return;
		
		
		if (itemsToUnequip != null && itemsToUnequip.Count > 0)
		{
			UnEquip();
		}
		
		if (itemToEquip != null && itemToEquip.Count > 0)
		{
			Equip(itemToEquip[0]);
		}
	}
	
	void Equip(EquipedItem equiped)
	{
		Equiped e = (Equiped)equiped.rpgItem;
		
		if (e.EquipmentSlots.Count == 1)
		{
			GameObject go = (GameObject)Instantiate(Resources.Load(e.FBXName));
			go.name = e.FBXName;
			
			UbrinEquipSystem ubrin = (UbrinEquipSystem)e.EquipmentSlots[0].ID;
			
			switch(ubrin)
			{
				case UbrinEquipSystem.RightHand:
					if (RhInst != null)
					{
						Destroy(RhInst);
					}
					RhInst = go;
					RhInst.name = e.FBXName;
					AddRhWeapons();
					RhInst.transform.parent = transform;
					break;
				
				case UbrinEquipSystem.Helmet:
					if (HelmInst != null)
					{
						Destroy(HelmInst);
					}
					HelmInst = go;
					HelmInst.name = e.FBXName;
					AddHelm();
					HelmInst.transform.parent = transform;
					break;
				
				case UbrinEquipSystem.LeftHand:
					if (LhInst != null)
					{
						Destroy(LhInst);
					}
					LhInst = go;
					LhInst.name = e.FBXName;
					AddLhWeapons();
					LhInst.transform.parent = transform;
					break;
				
				case UbrinEquipSystem.Chest:
					if (ChestInst != null)
					{
						Destroy(ChestInst);
					}
					ChestInst = go;
					ChestInst.name = e.FBXName;
					AddChest();
					ChestInst.transform.parent = transform;
					break;
				
				case UbrinEquipSystem.Wrist:
					if (WristsInst != null)
					{
						Destroy(WristsInst);
					}
					WristsInst = go;
					WristsInst.name = e.FBXName;
					WristsInst.transform.parent = transform;
					AddWrists();
					break;
				
				case UbrinEquipSystem.Boots:
					if (BootsInst != null)
					{
						Destroy(BootsInst);
					}
					BootsInst = go;
					BootsInst.name = e.FBXName;
					AddBoots();
					BootsInst.transform.parent = transform;
					break;
				
				case UbrinEquipSystem.Shoulder:
					if (ShouldInst != null)
					{
						Destroy(ShouldInst);
					}
					ShouldInst = go;
					ShouldInst.name = e.FBXName;
					AddShould();
					ShouldInst.transform.parent = transform;
					break;
			}
			
			Destroy(go);
		}
		
		itemToEquip.Remove(equiped);
	}
	
	void UnEquip()
	{
		foreach(EquipedItem i in itemsToUnequip)
		{
			Equiped e = (Equiped)i.rpgItem;
			
			UbrinEquipSystem ubrin = (UbrinEquipSystem)e.EquipmentSlots[0].ID;
			switch(ubrin)
			{
				case UbrinEquipSystem.RightHand:
					if (RhInst != null)
					{
						Destroy(RhInst);
						RhInst = null;
					}
					break;
				
				case UbrinEquipSystem.Helmet:
					if (HelmInst != null)
					{
						Destroy(HelmInst);
						HelmInst = null;
					}
					break;
				
				case UbrinEquipSystem.LeftHand:
					if (LhInst != null)
					{
						Destroy(LhInst);
						LhInst = null;
					}
					break;
				
				case UbrinEquipSystem.Chest:
					if (WristsInst != null)
					{
						Destroy(WristsInst);
						ChestInst = null;
					}
					break;
				
				case UbrinEquipSystem.Wrist:
					if (WristsInst != null)
					{
						Destroy(WristsInst);
						WristsInst = null;
					}
					break;
				
				case UbrinEquipSystem.Boots:
					if (WristsInst != null)
					{
						Destroy(BootsInst);
						BootsInst = null;
					}
					break;
				
				case UbrinEquipSystem.Shoulder:
					if (ShouldInst != null)
					{
						Destroy(ShouldInst);
						ShouldInst = null;
					}
					break;
			}
			
			itemsToUnequip.Remove(i);
			break;
		}
	}
	
	private void ChangeHeroVisibility(bool enabled)
	{
		Transform[] allChildren = GetComponentsInChildren<Transform>();
		foreach(Transform t in allChildren)
		{
			GameObject go = t.gameObject;
			if (go.renderer != null)
			{
				go.renderer.enabled = enabled;
			}
		} 
	}
	
	void OnLevelWasLoaded(int levelId)
	{
		ChangeHeroVisibility(true);
	}
	
	private void AddHelm(){
		SkinnedMeshRenderer[] BonedObjects = HelmInst.GetComponentsInChildren<SkinnedMeshRenderer>();
		foreach( SkinnedMeshRenderer smr in BonedObjects )
			ProcessBonedObjectHelm( smr );
	}
	private void AddChest(){
		SkinnedMeshRenderer[] BonedObjects = ChestInst.GetComponentsInChildren<SkinnedMeshRenderer>();
		foreach( SkinnedMeshRenderer smr in BonedObjects )
			ProcessBonedObjectChest( smr );
	}
	private void AddShould(){
		SkinnedMeshRenderer[] BonedObjects = ShouldInst.GetComponentsInChildren<SkinnedMeshRenderer>();
		foreach( SkinnedMeshRenderer smr in BonedObjects )
			ProcessBonedObjectShould( smr );
	}
	private void AddWrists(){
		SkinnedMeshRenderer[] BonedObjects = WristsInst.GetComponentsInChildren<SkinnedMeshRenderer>();
		foreach( SkinnedMeshRenderer smr in BonedObjects )
			ProcessBonedObjectWrists( smr );
	}
	private void AddBoots(){
		SkinnedMeshRenderer[] BonedObjects = BootsInst.GetComponentsInChildren<SkinnedMeshRenderer>();
		foreach( SkinnedMeshRenderer smr in BonedObjects )
			ProcessBonedObjectBoots( smr );
	}
	
	private void AddRhWeapons(){
		SkinnedMeshRenderer[] BonedObjects = RhInst.GetComponentsInChildren<SkinnedMeshRenderer>();
		foreach( SkinnedMeshRenderer smr in BonedObjects )
			ProcessBonedObjectR( smr );
	}
	private void AddLhWeapons(){
		SkinnedMeshRenderer[] BonedObjects = LhInst.GetComponentsInChildren<SkinnedMeshRenderer>();
		foreach( SkinnedMeshRenderer smr in BonedObjects )
			ProcessBonedObjectL( smr );
	}
	
	private void ProcessBonedObjectR(SkinnedMeshRenderer ThisRenderer){
	    // Create the SubObject
		RhInst = new GameObject( ThisRenderer.gameObject.name );	
	    RhInst.transform.parent = transform;
	    // Add the renderer
	    SkinnedMeshRenderer NewRenderer = RhInst.AddComponent( typeof( SkinnedMeshRenderer ) ) as SkinnedMeshRenderer;
	    // Assemble Bone Structure	
	    Transform[] MyBones = new Transform[ ThisRenderer.bones.Length ];
		// As clips are using bones by their names, we find them that way.
	    for( int i = 0; i < ThisRenderer.bones.Length; i++ )
	        MyBones[ i ] = FindChildByName( ThisRenderer.bones[ i ].name, transform );
	    // Assemble Renderer	
	    NewRenderer.bones = MyBones;	
	    NewRenderer.sharedMesh = ThisRenderer.sharedMesh;	
	    NewRenderer.materials = ThisRenderer.materials;	
	}
	
	private void ProcessBonedObjectL(SkinnedMeshRenderer ThisRenderer){
	    // Create the SubObject
		LhInst = new GameObject( ThisRenderer.gameObject.name );	
	    LhInst.transform.parent = transform;
	    // Add the renderer
	    SkinnedMeshRenderer NewRenderer = LhInst.AddComponent( typeof( SkinnedMeshRenderer ) ) as SkinnedMeshRenderer;
	    // Assemble Bone Structure	
	    Transform[] MyBones = new Transform[ ThisRenderer.bones.Length ];
		// As clips are using bones by their names, we find them that way.
	    for( int i = 0; i < ThisRenderer.bones.Length; i++ )
	        MyBones[ i ] = FindChildByName( ThisRenderer.bones[ i ].name, transform );
	    // Assemble Renderer	
	    NewRenderer.bones = MyBones;	
	    NewRenderer.sharedMesh = ThisRenderer.sharedMesh;	
	    NewRenderer.materials = ThisRenderer.materials;	
	}
	
	private void ProcessBonedObjectHelm(SkinnedMeshRenderer ThisRenderer){
	    // Create the SubObject
		HelmInst = new GameObject( ThisRenderer.gameObject.name );	
	    HelmInst.transform.parent = transform;
	    // Add the renderer
	    SkinnedMeshRenderer NewRenderer = HelmInst.AddComponent( typeof( SkinnedMeshRenderer ) ) as SkinnedMeshRenderer;
	    // Assemble Bone Structure	
	    Transform[] MyBones = new Transform[ ThisRenderer.bones.Length ];
		// As clips are using bones by their names, we find them that way.
	    for( int i = 0; i < ThisRenderer.bones.Length; i++ )
	        MyBones[ i ] = FindChildByName( ThisRenderer.bones[ i ].name, transform );
	    // Assemble Renderer	
	    NewRenderer.bones = MyBones;	
	    NewRenderer.sharedMesh = ThisRenderer.sharedMesh;	
	    NewRenderer.materials = ThisRenderer.materials;	
	}
	private void ProcessBonedObjectChest(SkinnedMeshRenderer ThisRenderer){
	    // Create the SubObject
		ChestInst = new GameObject( ThisRenderer.gameObject.name );	
	    ChestInst.transform.parent = transform;
	    // Add the renderer
	    SkinnedMeshRenderer NewRenderer = ChestInst.AddComponent( typeof( SkinnedMeshRenderer ) ) as SkinnedMeshRenderer;
	    // Assemble Bone Structure	
	    Transform[] MyBones = new Transform[ ThisRenderer.bones.Length ];
		// As clips are using bones by their names, we find them that way.
	    for( int i = 0; i < ThisRenderer.bones.Length; i++ )
	        MyBones[ i ] = FindChildByName( ThisRenderer.bones[ i ].name, transform );
	    // Assemble Renderer	
	    NewRenderer.bones = MyBones;	
	    NewRenderer.sharedMesh = ThisRenderer.sharedMesh;	
	    NewRenderer.materials = ThisRenderer.materials;	
	}
	private void ProcessBonedObjectShould(SkinnedMeshRenderer ThisRenderer){
	    // Create the SubObject
		ShouldInst = new GameObject( ThisRenderer.gameObject.name );	
	    ShouldInst.transform.parent = transform;
	    // Add the renderer
	    SkinnedMeshRenderer NewRenderer = ShouldInst.AddComponent( typeof( SkinnedMeshRenderer ) ) as SkinnedMeshRenderer;
	    // Assemble Bone Structure	
	    Transform[] MyBones = new Transform[ ThisRenderer.bones.Length ];
		// As clips are using bones by their names, we find them that way.
	    for( int i = 0; i < ThisRenderer.bones.Length; i++ )
	        MyBones[ i ] = FindChildByName( ThisRenderer.bones[ i ].name, transform );
	    // Assemble Renderer	
	    NewRenderer.bones = MyBones;	
	    NewRenderer.sharedMesh = ThisRenderer.sharedMesh;	
	    NewRenderer.materials = ThisRenderer.materials;	
	}
	private void ProcessBonedObjectWrists(SkinnedMeshRenderer ThisRenderer){
	    // Create the SubObject
		WristsInst = new GameObject( ThisRenderer.gameObject.name );	
	    WristsInst.transform.parent = transform;
	    // Add the renderer
	    SkinnedMeshRenderer NewRenderer = WristsInst.AddComponent( typeof( SkinnedMeshRenderer ) ) as SkinnedMeshRenderer;
	    // Assemble Bone Structure	
	    Transform[] MyBones = new Transform[ ThisRenderer.bones.Length ];
		// As clips are using bones by their names, we find them that way.
	    for( int i = 0; i < ThisRenderer.bones.Length; i++ )
	        MyBones[ i ] = FindChildByName( ThisRenderer.bones[ i ].name, transform );
	    // Assemble Renderer	
	    NewRenderer.bones = MyBones;	
	    NewRenderer.sharedMesh = ThisRenderer.sharedMesh;	
	    NewRenderer.materials = ThisRenderer.materials;	
	}
	private void ProcessBonedObjectBoots(SkinnedMeshRenderer ThisRenderer){
	    // Create the SubObject
		BootsInst = new GameObject( ThisRenderer.gameObject.name );	
	    BootsInst.transform.parent = transform;
	    // Add the renderer
	    SkinnedMeshRenderer NewRenderer = BootsInst.AddComponent( typeof( SkinnedMeshRenderer ) ) as SkinnedMeshRenderer;
	    // Assemble Bone Structure	
	    Transform[] MyBones = new Transform[ ThisRenderer.bones.Length ];
		// As clips are using bones by their names, we find them that way.
	    for( int i = 0; i < ThisRenderer.bones.Length; i++ )
	        MyBones[ i ] = FindChildByName( ThisRenderer.bones[ i ].name, transform );
	    // Assemble Renderer	
	    NewRenderer.bones = MyBones;	
	    NewRenderer.sharedMesh = ThisRenderer.sharedMesh;	
	    NewRenderer.materials = ThisRenderer.materials;	
	}
	
	// Recursive search of the child by name.
	private Transform FindChildByName( string ThisName, Transform ThisGObj ){	
	    Transform ReturnObj;
		// If the name match, we're return it
	    if( ThisGObj.name == ThisName )	
	        return ThisGObj.transform;
		// Else, we go continue the search horizontaly and verticaly
	    foreach( Transform child in ThisGObj ){	
	        ReturnObj = FindChildByName( ThisName, child );
	        if( ReturnObj != null )	
	            return ReturnObj;	
	    }
	return null;	
	}
}


public enum UbrinEquipSystem
{
	RightHand = 1,
	Helmet = 2,
	LeftHand = 3,
	Chest = 4,
	Wrist = 5,
	Boots = 6,
	Shoulder = 7
}
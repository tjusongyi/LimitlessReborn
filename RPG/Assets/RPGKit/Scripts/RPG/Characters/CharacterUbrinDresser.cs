using UnityEngine;
using System.Collections;

public class CharacterUbrinDresser : MonoBehaviour 
{
	public UbrinGenderEnum Gender;
	
	public UbrinHairEnum Hair;
	public UbrinFacialHairEnum FacialHair;
	public UbrinWeaponEnum Weapon;
	public UbrinShieldEnum Shield;
	
	public Color BodyColor = new Color(245, 255, 192 ,0);
	public Color HairColor = new Color(135, 123, 47 ,0);
	
	public UbrinMaleBootsEnum MaleBoots;
	public UbrinMaleChestEnum MaleChest;
	public UbrinMaleHelmEnum MaleHelm;
	public UbrinMaleShoulderEnum MaleShoulder;
	public UbrinMaleWristEnum MaleWrists;
	
	public UbrinFemaleBootsEnum FemaleBoots;
	public UbrinFemaleChestEnum FemaleChest;
	public UbrinFemaleHelmEnum FemaleHelm;
	public UbrinFemaleShoulderEnum FemaleShoulder;
	public UbrinFemaleWristEnum FemaleWrists;
	
	private GameObject RhInst;
	private GameObject ShieldInst;
	private GameObject HelmInst;
	private GameObject ChestInst;
	private GameObject ShouldInst;
	private GameObject WristsInst;
	private GameObject BootsInst;
	
	// Use this for initialization
	void Start () 
	{
		LoadCharacter();
		
		animation["Stand"].wrapMode = WrapMode.Loop;
		
		animation.Play("Stand");
	}
	
	public void LoadCharacter()
	{
		if (Gender == UbrinGenderEnum.Male)
		{
			Male();
		}
		else
		{
			Female();
		}
		
		if (Weapon != UbrinWeaponEnum.None)
		{
			RhInst = (GameObject) Instantiate(Resources.Load("Weapons/"+Weapon.ToString()));
			RhInst.transform.parent = transform;
			AddBoots();
		}
		
		if (Shield != UbrinShieldEnum.None)
		{
			ShieldInst = (GameObject) Instantiate(Resources.Load("Weapons/"+Shield.ToString()));
			ShieldInst.transform.parent = transform;
			AddBoots();
		}
	}
	
	void Female()
	{
		foreach (Transform child in transform)
		{
			GameObject go = child.gameObject;
			
			if (go.name == "BONES")
				continue;
			
			if (go.name == "Eyes")
				continue;
			
			if (go.name == "HumanBody01")
			{
				go.active = false;
				continue;
			}
			if (go.name == "HumanBody02")
			{
				go.active = true;
				go.renderer.materials[0].SetColor("_Color", BodyColor);
				go.renderer.materials[1].SetColor("_Color", BodyColor);
				continue;
			}
			
			if (go.name != Hair.ToString() && go.name != FacialHair.ToString())
			{
				go.active = false;
			}
			else
			{
				go.renderer.material.SetColor("_Color", HairColor);
			}
		}
		
		if (FemaleBoots != UbrinFemaleBootsEnum.None)
		{
			BootsInst = (GameObject) Instantiate(Resources.Load("Armor/"+FemaleBoots.ToString()));
			BootsInst.transform.parent = transform;
			AddBoots();
		}
		
		if (FemaleChest != UbrinFemaleChestEnum.None)
		{
			ChestInst = (GameObject) Instantiate(Resources.Load("Armor/"+FemaleChest.ToString()));
			ChestInst.transform.parent = transform;
			AddChest();
		}
		
		if (FemaleHelm != UbrinFemaleHelmEnum.None)
		{
			HelmInst = (GameObject) Instantiate(Resources.Load("Armor/"+FemaleHelm.ToString()));
			HelmInst.transform.parent = transform;
			AddHelm();
		}
		
		if (FemaleShoulder != UbrinFemaleShoulderEnum.None)
		{
			ShouldInst = (GameObject) Instantiate(Resources.Load("Armor/"+FemaleShoulder.ToString()));
			ShouldInst.transform.parent = transform;
			AddShould();
		}
		
		if (FemaleWrists != UbrinFemaleWristEnum.None)
		{
			WristsInst = (GameObject) Instantiate(Resources.Load("Armor/"+FemaleWrists.ToString()));
			WristsInst.transform.parent = transform;
			AddWrists();
		}
	}
	
	void Male()
	{
		foreach (Transform child in transform)
		{
			GameObject go = child.gameObject;
			
			if (go.name == "BONES")
				continue;
			
			if (go.name == "Eyes")
				continue;
			
			if (go.name == "HumanBody01")
			{
				go.renderer.materials[0].SetColor("_Color", BodyColor);
				go.renderer.materials[1].SetColor("_Color", BodyColor);
				continue;
			}
			
			if (go.name == "HumanBody02")
			{
                Destroy(go);
				continue;
			}
			
			if (go.name != Hair.ToString() && go.name != FacialHair.ToString())
			{
                Destroy(go);
			}
			else
			{
				go.renderer.material.SetColor("_Color", HairColor);
			}
		}
		
		if (MaleBoots != UbrinMaleBootsEnum.None)
		{
			BootsInst = (GameObject) Instantiate(Resources.Load("Armor/"+MaleBoots.ToString()));
			BootsInst.transform.parent = transform;
			AddBoots();
		}
		
		if (MaleChest != UbrinMaleChestEnum.None)
		{
			ChestInst = (GameObject) Instantiate(Resources.Load("Armor/"+MaleChest.ToString()));
			ChestInst.transform.parent = transform;
			AddChest();
		}
		
		if (MaleHelm != UbrinMaleHelmEnum.None)
		{
			HelmInst = (GameObject) Instantiate(Resources.Load("Armor/"+MaleHelm.ToString()));
			HelmInst.transform.parent = transform;
			AddHelm();
		}
		
		if (MaleShoulder != UbrinMaleShoulderEnum.None)
		{
			ShouldInst = (GameObject) Instantiate(Resources.Load("Armor/"+MaleShoulder.ToString()));
			ShouldInst.transform.parent = transform;
			AddShould();
		}
		
		if (MaleWrists != UbrinMaleWristEnum.None)
		{
			WristsInst = (GameObject) Instantiate(Resources.Load("Armor/"+MaleWrists.ToString()));
			WristsInst.transform.parent = transform;
			AddWrists();
		}
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
	
	private void AddShieldWeapons(){
		SkinnedMeshRenderer[] BonedObjects = ShieldInst.GetComponentsInChildren<SkinnedMeshRenderer>();
		foreach( SkinnedMeshRenderer smr in BonedObjects )
			ProcessBonedObjectShield( smr );
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
	
	
	private void ProcessBonedObjectShield(SkinnedMeshRenderer ThisRenderer){
	    // Create the SubObject
		ShieldInst = new GameObject( ThisRenderer.gameObject.name );	
	    ShieldInst.transform.parent = transform;
	    // Add the renderer
	    SkinnedMeshRenderer NewRenderer = ShieldInst.AddComponent( typeof( SkinnedMeshRenderer ) ) as SkinnedMeshRenderer;
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
}

public enum UbrinHairEnum
{
	None,
	Hair02,
	Hair03,
	Hair04,
	Hair05,
	Hair06,
	Hair07,
	Hair08,
	Hair09,
	Hair10,
	Hair11,
	Hair12,
	Hair13,
	Hair14,
	Hair15
}

public enum UbrinFacialHairEnum
{
	None,
	FacialHair01,
	FacialHair02,
	FacialHair03,
	FacialHair04,
	FacialHair05,
	FacialHair06,
	FacialHair07,
	FacialHair08,
	FacialHair09,
}

public enum UbrinGenderEnum
{
	Male,
	Female
}

public enum UbrinMaleBootsEnum
{
	None,
	Boots_1,
	Boots_2,
	Boots_3,
	Boots_4,
	Boots_5,
	BootsMage_1
}

public enum UbrinFemaleBootsEnum
{
	None,
	Boots_1w,
	Boots_2w,
	Boots_3w,
	Boots_4w,
	Boots_5w,
	BootsMage_1w
}

public enum UbrinMaleChestEnum
{
	None,
	Chest_1,
	Chest_2,
	Chest_3,
	Chest_4,
	Chest_5,
	ChestMage_1,
	ChestMage_2,
	ChestMage_3,
	ChestMage_4
}

public enum UbrinFemaleChestEnum
{
	None,
	Chest_1w,
	Chest_2w,
	Chest_3w,
	Chest_4w,
	Chest_5w,
	ChestMage_1w,
	ChestMage_2w,
	ChestMage_3w,
	ChestMage_4w
}

public enum UbrinMaleHelmEnum
{
	None,
	Helm_1,
	Helm_2,
	Helm_3,
	Helm_4,
	Helm_5,
	Helm_6,
	Helm_7,
	Helm_8,
	Helm_9,
	Helm_10,
	Helm_11,
	Helm_12,
	HelmMage_1,
	HelmMage_2,
	HelmMage_3,
	HelmMage_4
}

public enum UbrinFemaleHelmEnum
{
	None,
	Helm_1,
	Helm_2,
	Helm_3,
	Helm_4,
	Helm_5,
	Helm_6,
	Helm_7,
	Helm_8,
	Helm_9,
	Helm_10,
	Helm_11,
	Helm_12,
	HelmMage_1,
	HelmMage_2,
	HelmMage_3,
	HelmMage_4
}


public enum UbrinMaleShoulderEnum
{
	None,
	Shoulder_1,
	Shoulder_2,
	Shoulder_3
}

public enum UbrinFemaleShoulderEnum
{
	None,
	Shoulder_1w,
	Shoulder_2w,
	Shoulder_3w
}

public enum UbrinMaleWristEnum
{
	None,
	Wrists_1,
	Wrists_2,
	Wrists_3,
	Wrists_4,
	WristsMage_1
}

public enum UbrinFemaleWristEnum
{
	None,
	Wrists_1w,
	Wrists_2w,
	Wrists_3w,
	Wrists_4w,
	WristsMage_1w
}

public enum UbrinWeaponEnum
{
	None,
	off_Axe_1,
	off_Axe_2,
	off_Axe_3,
	off_Axe_4,
	off_Axe_5,
	off_Hammer_1,
	off_Hammer_2,
	off_pickage_1,
	off_pickage_2,
	off_stick_1,
	off_Sword_2,
	off_Sword_3,
	off_Sword_4,
	off_Sword_5
}

public enum UbrinShieldEnum
{
	None,
	Shield_1,
	Shield_2,
	Shield_3,
	Shield_4,
	Shield_5,
}


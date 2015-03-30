/// <summary>
/// Command menu.
/// This script use for draw command menu GUI (button inventory,status,equip,skill)
/// </summary>

using UnityEngine;
using System.Collections;

public class CommandMenu : MonoBehaviour {
	
	private Vector2 defaultScreenRes; //Screen Resolution
	
	[System.Serializable]
	public class ButtonSetting
	{
		public Vector2 position;
		public Vector2 size;
		public GUIStyle buttonStlye;
	}
	
	public ButtonSetting status,bag,equip,skill; //button setting
	
	// Use this for initialization
	void Start () {
		
		defaultScreenRes.x = 1920; //declare max screen ratio
		defaultScreenRes.y = 1080; //declare max screen ratio
	
	}

	
	// Update is called once per frame
	void OnGUI () {
		
		// Resize GUI Matrix according to screen size
        ResizeGUIMatrix();
		
		//when click button status
		if(GUI.Button(new Rect(status.position.x,status.position.y,status.size.x,status.size.y),"",status.buttonStlye))
		{
			GUI_Menu.instance.OpenShortcutMenu("Status");
			
		}
		
		//when click button inventory
		if(GUI.Button(new Rect(bag.position.x,bag.position.y,bag.size.x,bag.size.y),"",bag.buttonStlye))
		{
			GUI_Menu.instance.OpenShortcutMenu("Inventory");
		}
		
		//when click button equipment
		if(GUI.Button(new Rect(equip.position.x,equip.position.y,equip.size.x,equip.size.y),"",equip.buttonStlye))
		{
			GUI_Menu.instance.OpenShortcutMenu("Equipment");
		}
		
		//when click button skill
		if(GUI.Button(new Rect(skill.position.x,skill.position.y,skill.size.x,skill.size.y),"",skill.buttonStlye))
		{
			GUI_Menu.instance.OpenShortcutMenu("Skill");
		}
		
		
		// Reset matrix after finish
	        GUI.matrix = Matrix4x4.identity;
	
	}
	
	void ResizeGUIMatrix()
    {
       // Set matrix
       Vector2 ratio = new Vector2(Screen.width/defaultScreenRes.x , Screen.height/defaultScreenRes.y );
       Matrix4x4 guiMatrix = Matrix4x4.identity;
       guiMatrix.SetTRS(new Vector3(1, 1, 1), Quaternion.identity, new Vector3(ratio.x, ratio.y, 1));
       GUI.matrix = guiMatrix;
    }
}

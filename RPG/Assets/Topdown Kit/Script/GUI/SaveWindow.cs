/// <summary>
/// Save window.
/// This script use to draw save window
/// </summary>

using UnityEngine;
using System.Collections;

public class SaveWindow : MonoBehaviour {
	
	[System.Serializable]
	public class GUISetting
	{
		public Vector2 position;
		public Vector2 size;
		public Texture2D texture;
	}
	
	[System.Serializable]
	public class ButtonSetting
	{
		public Vector2 position;
		public Vector2 size;
		public GUIStyle buttonStlye;
	}
	private Vector2 defaultScreenRes; //Screen Resolution
	
	public GUISetting cautionWindow; //window setting
	public ButtonSetting buttonSave,buttonCancel; //button setting
	
	public HeroController controller; //script controller
	
	public static bool enableWindow; //check enable/disable window
	
	// Use this for initialization
	void Start () {
		enableWindow = false;
		defaultScreenRes.x = 1920; //declare max screen ratio
		defaultScreenRes.y = 1080; //declare max screen ratio
		
		GameObject go = GameObject.FindGameObjectWithTag("Player");  //Find player
		controller = go.GetComponent<HeroController>();
	}
	
	void OnGUI()
	{
		// Resize GUI Matrix according to screen size
        ResizeGUIMatrix();
		
		if(enableWindow)
		{
			if(!controller.dontMove)
				controller.dontMove = true;
			
			//Draw window
			GUI.DrawTexture(new Rect(cautionWindow.position.x,cautionWindow.position.y,cautionWindow.size.x,cautionWindow.size.y),cautionWindow.texture);
		
			//Draw save button
			if(GUI.Button(new Rect(buttonSave.position.x,buttonSave.position.y,buttonSave.size.x,buttonSave.size.y),"",buttonSave.buttonStlye))
			{
				CharacterData.SaveData();
				enableWindow = false;
			}
		
			//Draw cancel button
			if(GUI.Button(new Rect(buttonCancel.position.x,buttonCancel.position.y,buttonCancel.size.x,buttonCancel.size.y),"",buttonCancel.buttonStlye))
			{
				controller.dontMove = false;
				enableWindow = false;
			}
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

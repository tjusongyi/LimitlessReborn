/// <summary>
/// Dead window.
/// This script use for call a dead window when character is dead
/// </summary>

using UnityEngine;
using System.Collections;

public class DeadWindow : MonoBehaviour {
	
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
	public ButtonSetting buttonReturn,buttonQuit; //button setting
	public string sceneQuitGame; //name scene quit when you select quit button
	
	//Private variable
	private HeroController controller;
	
	public static bool enableWindow; //check enable disable window
	
	// Use this for initialization
	void Start () {
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
			//draw window
			GUI.DrawTexture(new Rect(cautionWindow.position.x,cautionWindow.position.y,cautionWindow.size.x,cautionWindow.size.y),cautionWindow.texture);
		
			//when click return
			if(GUI.Button(new Rect(buttonReturn.position.x,buttonReturn.position.y,buttonReturn.size.x,buttonReturn.size.y),"",buttonReturn.buttonStlye))
			{
				controller.Reborn();
				enableWindow = false;
			}
		
			//when click quit game
			if(GUI.Button(new Rect(buttonQuit.position.x,buttonQuit.position.y,buttonQuit.size.x,buttonQuit.size.y),"",buttonQuit.buttonStlye))
			{
				Invoke("LoadScene",0.3f);
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
	
	void LoadScene()
	{
		Application.LoadLevel(sceneQuitGame);
		enableWindow = false;
	} 
}

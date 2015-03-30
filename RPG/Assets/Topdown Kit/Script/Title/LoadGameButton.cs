/// <summary>
/// Load game button.
/// This script use for detect button load
/// To change texture button normal-down and call load scene when click this button
/// </summary>

using UnityEngine;
using System.Collections;

public class LoadGameButton : MonoBehaviour {
	
	
	public string loadSceneName; //Next load scene name
	public Texture2D buttonNormal,buttonDown,buttonDisable; //Texture button down,normal,disable
	public AudioClip buttonSfx; //button sfx
	
	//Private Variable
	private string checkData;
	
	//Check Load Data
	public static bool loadData;
	
	void Start()
	{
		//Check data if you not have data , button load change texture to disable(can't click)
		checkData = PlayerPrefs.GetString("Enable Load");
		
		if(checkData == "True")
		{
			this.guiTexture.texture = buttonNormal;
		}else
		{
			this.guiTexture.texture = buttonDisable;
		}
	}
	
	void OnMouseUp()
	{
		//Load scene
		if(checkData == "True")
		{
			this.guiTexture.texture = buttonNormal;
			Invoke("LoadScene",0.1f);
		}
	}
	
	void OnMouseDown()
	{
		if(checkData == "True")
		{
			if(buttonSfx != null)
			AudioSource.PlayClipAtPoint(buttonSfx,transform.position);
		
			this.guiTexture.texture = buttonDown;
			//enable load data
			loadData = true;
		}
		
	}
		
	void LoadScene()
	{
		Application.LoadLevel(loadSceneName);;	
	}
}

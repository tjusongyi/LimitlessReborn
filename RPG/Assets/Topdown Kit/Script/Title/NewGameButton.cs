/// <summary>
/// New game button.
/// This script use for detect button new game
/// To change texture button normal-down and call load scene when click this button
/// </summary>

using UnityEngine;
using System.Collections;

public class NewGameButton : MonoBehaviour {
	
	public string loadSceneName; //Next load scene name
	public Texture2D buttonNormal,buttonDown; //Texture button down,normal
	public AudioClip buttonSfx;//button sfx
	
	
	void OnMouseUp()
	{
		this.guiTexture.texture = buttonNormal;
		Invoke("LoadScene",0.1f);
	}
	
	void OnMouseDown()
	{
		if(buttonSfx != null)
			AudioSource.PlayClipAtPoint(buttonSfx,transform.position);
		
		this.guiTexture.texture = buttonDown;
	}
		
	void LoadScene()
	{
		Application.LoadLevel(loadSceneName);;	
	}
}

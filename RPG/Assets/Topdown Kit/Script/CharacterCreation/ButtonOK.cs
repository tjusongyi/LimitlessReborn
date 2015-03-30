/// <summary>
/// This script use for detect button ok
/// To change texture button normal-down and call load scene when click this button
/// </summary>

using UnityEngine;
using System.Collections;

public class ButtonOK : MonoBehaviour {
	
	public string loadSceneName;    //Next load scene name
	
	public Texture2D buttonOkNormal;  //Texture button normal
	public Texture2D buttonOkDown;   //Texture button down
	public Entername enterNameScript;  //script entername
	public SelectCharacter selectCharacterScript; //script select character
	public AudioClip sfxButton;  //sound effect when click this button
	
	void OnMouseUp()
	{
		//change texture to "normal"
		this.gameObject.guiTexture.texture = buttonOkNormal;
		
		//Save select character and name
		PlayerPrefs.SetString("pName",enterNameScript.defaultName);
		PlayerPrefs.SetInt("pSelect",selectCharacterScript.indexHero);
		
		SpawnPointHero.enableLoadData = true;
		
		//Load next scene
		Invoke("LoadScene",0.1f);
	}
	
	void OnMouseDown()
	{
		//change texture to "down"
		this.gameObject.guiTexture.texture = buttonOkDown;
		
		//Play sfx
		if(sfxButton != null)
		AudioSource.PlayClipAtPoint(sfxButton,transform.position);
		
	}
	
	void LoadScene()
	{
		Application.LoadLevel(loadSceneName);;	
	}
}

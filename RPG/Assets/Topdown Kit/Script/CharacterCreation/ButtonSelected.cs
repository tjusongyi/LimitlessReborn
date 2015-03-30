/// <summary>
/// Button selected.
/// This script use to select a character(button next,button previous)
/// </summary>

using UnityEngine;
using System.Collections;

public class ButtonSelected : MonoBehaviour {
	
	public string buttonName; //button name
	
	public Texture2D buttonOkNormal; //Texture button normal
	public Texture2D buttonOkDown; //Texture button down
	public SelectCharacter selectCharacterScript;//script select character
	public AudioClip sfxButton; //sound effect when click this button
	
	void OnMouseUp()
	{
		//change texture to "normal"
		this.gameObject.guiTexture.texture = buttonOkNormal;
		
		//if button name = Next when click this button, Choose next character
		if(buttonName == "Next")
		{
			selectCharacterScript.indexHero += 1;
			
			if(selectCharacterScript.indexHero >= selectCharacterScript.hero.Length)
			{
				selectCharacterScript.indexHero = 0;
			}
			
			selectCharacterScript.UpdateHero(selectCharacterScript.indexHero);
			
		}
		
		//if button name = Prev when click this button, Choose previous character
		if(buttonName == "Prev")
		{
			selectCharacterScript.indexHero -= 1;	
			
			if(selectCharacterScript.indexHero < 0)
			{
				selectCharacterScript.indexHero = selectCharacterScript.hero.Length-1;
			}
			
			selectCharacterScript.UpdateHero(selectCharacterScript.indexHero);
		}
		
		
	}
	
	void OnMouseDown()
	{
		//change texture to "down"
		this.gameObject.guiTexture.texture = buttonOkDown;
		//Play sfx
		AudioSource.PlayClipAtPoint(sfxButton,transform.position);
		
	}
	

}

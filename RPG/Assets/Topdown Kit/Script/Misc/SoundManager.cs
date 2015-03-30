/// <summary>
/// Sound manager.
/// This script use for play sound effect
/// </summary>

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SoundManager : MonoBehaviour {
	
	
	[System.Serializable]
	public class SoundGroup{
		public string soundName;
		public AudioClip audioClip;
		
	}
	
	public List<SoundGroup> sound_List = new List<SoundGroup>();
	
	public static SoundManager instance;
	
	public void Start(){
		instance = this;	
	}
	
	public void PlayingSound(string _soundName){
		AudioSource.PlayClipAtPoint(sound_List[FindSound(_soundName)].audioClip, Camera.mainCamera.transform.position);
	}
	
	private int FindSound(string _soundName){
		int i = 0;
		while( i < sound_List.Count ){
			if(sound_List[i].soundName == _soundName){
				return i;	
			}
			i++;
		}
		return i;
	}
	
}

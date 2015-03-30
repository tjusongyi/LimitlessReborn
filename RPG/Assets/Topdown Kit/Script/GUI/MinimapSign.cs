/// <summary>
/// Minimap sign.
/// This script use for setup minimap sign
/// </summary>

using UnityEngine;
using System.Collections;

public class MinimapSign : MonoBehaviour {
	
	public static MinimapSign Instance;
	
	public Texture player,enemy,boss,npc,shopWeapon,shopPotion,savePoint;
	
	[HideInInspector]
	public Material[] minimapSignMat = new Material[7];
	
	private Shader unlitShader;	
	
	// Use this for initialization
	void Awake () {
		
		//Crate new material to assign minimap sign
		
		minimapSignMat = new Material[7];
		
		Instance = this;
		
		unlitShader = Shader.Find("Unlit/Transparent");	
		
		for(int i=0;i < minimapSignMat.Length;i++)
		{
			minimapSignMat[i] = new Material(unlitShader);	
		}
		
		minimapSignMat[0].mainTexture = player;
		minimapSignMat[1].mainTexture = enemy;
		minimapSignMat[2].mainTexture = boss;
		minimapSignMat[3].mainTexture = npc;
		minimapSignMat[4].mainTexture = shopWeapon;
		minimapSignMat[5].mainTexture = shopPotion;
		minimapSignMat[6].mainTexture = savePoint;
	
	}
}

/// <summary>
/// Minimap sign setup.
/// This script use for setup minimap sign
/// </summary>

using UnityEngine;
using System.Collections;

public class MinimapSignSetup : MonoBehaviour {
	
	//type of sign
	public enum MinimapSignType{Player,Enemy,Boss,Npc,ShopWeapon,ShopPotion,SavePoint}
	
	public MinimapSignType signType;
	
	
	// Use this for initialization
	void Start () {
		TextureSetup();
		this.gameObject.layer = 12;
	}
	
	
	//change texture to selected sign
	void TextureSetup ()
	{
		if(signType == MinimapSignType.Player)
		{
			this.gameObject.renderer.material = MinimapSign.Instance.minimapSignMat[0];	
		}else if(signType == MinimapSignType.Enemy)
		{
			this.gameObject.renderer.material = MinimapSign.Instance.minimapSignMat[1];	
		}else if(signType == MinimapSignType.Boss)
		{
			this.gameObject.renderer.material = MinimapSign.Instance.minimapSignMat[2];	
		}else if(signType == MinimapSignType.Npc)
		{
			this.gameObject.renderer.material = MinimapSign.Instance.minimapSignMat[3];	
		}else if(signType == MinimapSignType.ShopWeapon)
		{
			this.gameObject.renderer.material = MinimapSign.Instance.minimapSignMat[4];	
		}else if(signType == MinimapSignType.ShopPotion)
		{
			this.gameObject.renderer.material = MinimapSign.Instance.minimapSignMat[5];	
		}else if(signType == MinimapSignType.SavePoint)
		{
			this.gameObject.renderer.material = MinimapSign.Instance.minimapSignMat[6];	
		}
			
	}
	
}

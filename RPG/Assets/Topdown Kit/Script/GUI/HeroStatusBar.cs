/// <summary>
/// Hero status bar.
/// This script use to draw a hero status bar(hp,mp,face,lv,etc)on the left-top on screen
/// </summary>

using UnityEngine;
using System.Collections;

public class HeroStatusBar : MonoBehaviour {
	
	private Vector2 defaultScreenRes; //Screen Resolution
	
	[System.Serializable]
	public class GUISetting
	{
		public Vector2 position;
		public Vector2 size;
		public Texture2D[] texture;
	}
	
	[System.Serializable]
	public class LabelSetting
	{
		public Vector2 position;
		public GUIStyle labelStyle;
	}
	
	[System.Serializable]
	public class HerofaceSetting
	{
		public Vector2 position;
		public Vector2 size;
		public Texture texture;
		public Material renderMaterial;
	}
	
	[System.Serializable]
	public class BuffSetting
	{
		public Vector2 position;
		public Vector2 size;
		public float rangeBetweenBuff;
		
	}
	
	public GUISetting hp,mp,faceFrame,nameBar; //GUI setting
	public HerofaceSetting heroFace; //Hero face setting
	public LabelSetting heroInfo,hpText,mpText; //Text setting
	public BuffSetting buffIconSetting; //Buff icon setting
	
	
	//Private variable
	private PlayerSkill playerSkill;
	private PlayerStatus playerStatus;
	
	// Use this for initialization
	void Start () {
		
		defaultScreenRes.x = 1920; //declare max screen ratio
		defaultScreenRes.y = 1080; //declare max screen ratio
		
		GameObject go = GameObject.FindGameObjectWithTag("Player");  //Find player
		playerStatus = go.GetComponent<PlayerStatus>();
		playerSkill = go.GetComponent<PlayerSkill>();
		
	}
	
	// Update is called once per frame
	void OnGUI () {
		
		// Resize GUI Matrix according to screen size
        ResizeGUIMatrix();
		
		//Hero face
		Graphics.DrawTexture(new Rect(heroFace.position.x,heroFace.position.y,heroFace.size.x ,heroFace.size.y),heroFace.texture,heroFace.renderMaterial);
		
		//HP bar
			 GUI.BeginGroup(new Rect(hp.position.x, hp.position.y,hp.size.x,hp.size.y));
		       GUI.DrawTexture(new Rect(0,0, hp.size.x ,hp.size.y), hp.texture[0]);
				
			        GUI.BeginGroup(new Rect(0,0,Convert(hp.size.x,playerStatus.hpMax,playerStatus.statusCal.hp),hp.size.y));
					
			         GUI.DrawTexture(new Rect(0,0,hp.size.x,hp.size.y), hp.texture[1]);
			         GUI.EndGroup();
				
		       GUI.EndGroup();
		//MP bar
			 GUI.BeginGroup(new Rect(mp.position.x, mp.position.y,mp.size.x,mp.size.y));
		       GUI.DrawTexture(new Rect(0,0, mp.size.x ,mp.size.y), mp.texture[0]);
				
			        GUI.BeginGroup(new Rect(0,0,Convert(mp.size.x,playerStatus.mpMax,playerStatus.statusCal.mp),mp.size.y));
					
			         GUI.DrawTexture(new Rect(0,0,mp.size.x,mp.size.y), mp.texture[1]);
			         GUI.EndGroup();
				
		       GUI.EndGroup();
		
		//Face Frame
			GUI.DrawTexture(new Rect(faceFrame.position.x,faceFrame.position.y, faceFrame.size.x ,faceFrame.size.y), faceFrame.texture[0]);
		
		//Name bar
			GUI.DrawTexture(new Rect(nameBar.position.x,nameBar.position.y,nameBar.size.x,nameBar.size.y),nameBar.texture[0]);
		
		//Name info
			TextFilter.DrawOutline(new Rect(heroInfo.position.x ,heroInfo.position.y, 1000 , 1000)
			,"Lv. " + playerStatus.status.lv.ToString() + " " + playerStatus.playerName,heroInfo.labelStyle,Color.black,Color.white,2f);
		
		//HP text
			TextFilter.DrawOutline(new Rect(hpText.position.x ,hpText.position.y, 1000 , 1000)
			,playerStatus.statusCal.hp.ToString() + "/" + playerStatus.hpMax.ToString(),hpText.labelStyle,Color.black,Color.white,2f);
		
		//MP text
			TextFilter.DrawOutline(new Rect(mpText.position.x ,mpText.position.y, 1000 , 1000)
			,playerStatus.statusCal.mp.ToString() + "/" + playerStatus.mpMax.ToString(),mpText.labelStyle,Color.black,Color.white,2f);
			
		//Buff Icon
		
		for(int i=0;i<playerSkill.durationBuff.Length;i++)
		{
			if(playerSkill.durationBuff[i].isCount)
			{
				GUI.DrawTexture(new Rect(buffIconSetting.position.x + (i*buffIconSetting.rangeBetweenBuff) ,buffIconSetting.position.y,buffIconSetting.size.x,buffIconSetting.size.y),playerSkill.durationBuff[i].skillIcon);
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
	
	float Convert(float maxWidthGUI, float maxValue, float curValue) //Calculate hp bar-mp bar
	 {
	  float val = maxWidthGUI/maxValue;
	  float load = curValue*val; 
	  return load;
	 }
	
}

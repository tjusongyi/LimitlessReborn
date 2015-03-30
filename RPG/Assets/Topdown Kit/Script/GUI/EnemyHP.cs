/// <summary>
/// This script use for draw a enemy hp bar
/// </summary>

using UnityEngine;
using System.Collections;

public class EnemyHP : MonoBehaviour {
	
	private Vector2 defaultScreenRes; //Screen Resolution
	public static EnemyHP Instance; //Declare this script to global script
	public Vector2 posHPbar; //position hp bar
	public Vector2 sizeHPBar; //size hp bar
	public Texture2D hpBar; //texture hp bar
	public Texture2D hpCurrent; //texture hp current
	
	public Vector2 posEnemyName; //pos enemy name
	public Vector2 posHPText; //pos hp text
	public GUIStyle nameStyle;	//style enemy name
	public GUIStyle hpNumberStyle; //style hp text
	
	//Private variable field
	private bool showHPBar; //Control hp GUI
	private float maxHP;	//Target maxhp
	private float curHP;	//Target curhp
	private string enemyName = "enemy name";	//Target name
	
	
	
	// Use this for initialization
	void Start () {
		
		defaultScreenRes.x = 1920; //declare screen ratio
		defaultScreenRes.y = 1080; //declare screen ratio
		
		Instance = this; //declare this script to global script
		
	}
	
	
	 void OnGUI()
     {	
		// Resize GUI Matrix according to screen size
        ResizeGUIMatrix();
		
		//Enemy HP bar
		
		if(showHPBar)
		{
		
		TextFilter.DrawOutline(new Rect(posEnemyName.x ,posEnemyName.y, 1000 , 1000)
			,enemyName,nameStyle,Color.black,Color.white,2f);

		 GUI.BeginGroup(new Rect(posHPbar.x, posHPbar.y,sizeHPBar.x,sizeHPBar.y));
	       GUI.DrawTexture(new Rect(0,0, sizeHPBar.x ,sizeHPBar.y), hpBar);
			
		        GUI.BeginGroup(new Rect(0,0,ConvertHP(sizeHPBar.x,maxHP,curHP),sizeHPBar.y));
				
		         GUI.DrawTexture(new Rect(0,0,sizeHPBar.x,sizeHPBar.y), hpCurrent);
		         GUI.EndGroup();
			
	       GUI.EndGroup();
		
		TextFilter.DrawOutline(new Rect(posHPText.x ,posHPText.y, 1000 , 1000)
			,curHP.ToString() + " / " + maxHP.ToString(),hpNumberStyle,Color.black,Color.white,2f);
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
	
	
	public void ShowHPbar(bool showHP) //Control hp bar
	{
		showHPBar = showHP; 
	}
	
	public void GetHPTarget(float maxHPf, float curHPf,string enemyNamef)
	{
		maxHP = maxHPf;
		curHP = curHPf;
		enemyName = enemyNamef;
	}
	
	float ConvertHP(float maxWidthGUI, float maxHP, float curHP) //Calculate hp bar
	 {
	  float val = maxWidthGUI/maxHP;
	  float load = curHP*val; 
	  return load;
	 }
}
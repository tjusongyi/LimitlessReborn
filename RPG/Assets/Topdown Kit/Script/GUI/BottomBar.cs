/// <summary>
/// Bottom bar.
/// This script use for draw GUI (cast bar,expbar,exp text)
/// </summary>

using UnityEngine;
using System.Collections;

public class BottomBar : MonoBehaviour {
	
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
	
	[HideInInspector]
	public float currentCastTime;
	[HideInInspector]
	public float castTime;
	[HideInInspector]
	public bool showCastBar;
	
	public GUISetting expBar; //exp bar setting
	public LabelSetting expText; //exp text setting
	public GUISetting castBar; //cast bar setting
	
	private PlayerStatus playerStatus;

	// Use this for initialization
	void Start () {
		
		defaultScreenRes.x = 1920; //declare max screen ratio
		defaultScreenRes.y = 1080; //declare max screen ratio
		
		GameObject go = GameObject.FindGameObjectWithTag("Player");  //Find player
		playerStatus = go.GetComponent<PlayerStatus>();
	
	}
	
	// Update is called once per frame
	void OnGUI () {
		
		// Resize GUI Matrix according to screen size
        ResizeGUIMatrix();
		
		//Cast bar
		if(showCastBar)
		{
			GUI.BeginGroup(new Rect(castBar.position.x, castBar.position.y,castBar.size.x,castBar.size.y));
		       GUI.DrawTexture(new Rect(0,0, castBar.size.x ,castBar.size.y), castBar.texture[0]);
				
			        GUI.BeginGroup(new Rect(0,0,Convert(castBar.size.x,castTime,currentCastTime),castBar.size.y));
					
			         GUI.DrawTexture(new Rect(0,0,castBar.size.x,castBar.size.y), castBar.texture[1]);
			         GUI.EndGroup();
				
		       GUI.EndGroup();
		}
			 
		
		//Exp bar
			 GUI.BeginGroup(new Rect(expBar.position.x, expBar.position.y,expBar.size.x,expBar.size.y));
		       GUI.DrawTexture(new Rect(0,0, expBar.size.x ,expBar.size.y), expBar.texture[0]);
				
			        GUI.BeginGroup(new Rect(0,0,Convert(expBar.size.x,playerStatus.expMax,playerStatus.status.exp),expBar.size.y));
					
			         GUI.DrawTexture(new Rect(0,0,expBar.size.x,expBar.size.y), expBar.texture[1]);
			         GUI.EndGroup();
				
		       GUI.EndGroup();
		
		//Exp text
			TextFilter.DrawOutline(new Rect(expText.position.x ,expText.position.y, 1000 , 1000)
			,Mathf.FloorToInt(playerStatus.status.exp).ToString() + "/" + Mathf.FloorToInt(playerStatus.expMax).ToString(),expText.labelStyle,Color.black,Color.white,2f);
		
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

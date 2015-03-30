/// <summary>
/// Log text.
/// This script use for show a log text(when you hero no mp,money)
/// </summary>

using UnityEngine;
using System.Collections;

public class LogText : MonoBehaviour {
	
	public static LogText Instance;
	
	private Vector2 defaultScreenRes; //Screen Resolution
	
	[HideInInspector]
	public float showLogTimer;
	[HideInInspector]
	public string logText;
	[HideInInspector]
	public bool showLog;
	
	[System.Serializable]
	public class LabelSetting
	{
		public Vector2 position;
		public GUIStyle labelStyle;
	}
	
	public LabelSetting logSetup;
	
	// Use this for initialization
	void Start () {
		
		showLogTimer = 2;
		
		//Declare this script to static
		Instance = this;
		
		defaultScreenRes.x = 1920; //declare max screen ratio
		defaultScreenRes.y = 1080; //declare max screen ratio
	
	}
	
	// Update is called once per frame
	void Update () {
		
		if(showLogTimer > 0)
		{
			showLogTimer -= Time.deltaTime;	
		}else if(showLogTimer < 0)
		{
			//Reset Show log when time out
			showLog = false;
			logText = "";
			showLogTimer = 0;	
		}
	
	}
	
	public void SetLog(float timer,string text)
	{
		logText = text;
		showLogTimer = timer;
		showLog = true;
	}
	
	// Update is called once per frame
	void OnGUI () {
		
		// Resize GUI Matrix according to screen size
        ResizeGUIMatrix();
		
		//Log text
		if(showLog)
		{
			TextFilter.DrawOutline(new Rect(logSetup.position.x ,logSetup.position.y, 1000 , 1000)
			,logText,logSetup.labelStyle,Color.black,logSetup.labelStyle.normal.textColor,2f);
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
}

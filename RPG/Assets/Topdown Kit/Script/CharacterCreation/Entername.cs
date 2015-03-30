/// <summary>
/// Entername.
/// This script use for input character name.
/// </summary>

using UnityEngine;
using System.Collections;

public class Entername : MonoBehaviour {
	
	public string defaultName; //Default name it will show when you not input anything
	public Vector2 posEnterName; //position enter name box
	public Vector2 sizeEnterName; //size enter name box
	private Vector2 defaultScreenRes; //Screen Resolution
	public GUIStyle enterNameStyle; //enter name style
	// Use this for initialization
	void Start () {
		
		defaultScreenRes.x = 1920; //declare max screen ratio
		defaultScreenRes.y = 1080; //declare max screen ratio
	
	}
	
	// Update is called once per frame
	void OnGUI () {
	
		// Resize GUI Matrix according to screen size
        ResizeGUIMatrix();
		
		// draw enter name box
		defaultName = GUI.TextArea(new Rect(posEnterName.x,posEnterName.y,sizeEnterName.x,sizeEnterName.y),defaultName,enterNameStyle);
		
		
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

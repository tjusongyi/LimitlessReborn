using UnityEngine;
using System.Collections;

public class GUIScale : MonoBehaviour 
{
	private float screenWidth;
	private float screenHeight;
	
	public static float widthScale = 1;
	public static float heightScale = 1;
	
	// Use this for initialization
	void Start () 
	{
		screenWidth = 1024;
		screenHeight = 768;
	}
	
	// Update is called once per frame
	void Update () 
	{
		if (screenWidth != Screen.width)
		{
			widthScale = Screen.width / screenWidth;
		}
		
		if (screenHeight != Screen.height)
		{
			heightScale = Screen.height / screenHeight;
		}
	}
}

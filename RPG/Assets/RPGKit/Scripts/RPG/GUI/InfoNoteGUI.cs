using UnityEngine;
using System.Collections;

public class InfoNoteGUI : BasicGUI 
{
	public static string Text;
	
	void Start()
	{
		Prepare();
	}

	void OnGUI()
	{
		if (!isNoteDisplayed || isMainMenuDisplayed)
			return;
		
		Box(ScreenWidth/2 - 300, 100, 600, 300);
		
		Label(ScreenWidth/2 - 270, 130, 540, 200, Text);
		
		if (Input.GetKey(KeyCode.Return) || Input.GetKey(KeyCode.KeypadEnter))
		{
            CloseWindow();
		}
		
		if (Button(new Rect(ScreenWidth/2 - 270, 350, 540, 40), "ok"))
		{
            CloseWindow();
		}
	}

    void CloseWindow()
    {
        isNoteDisplayed = false;
        Text = string.Empty;
        player.ChangeMouseControl(true);
    }
}

/// <summary>
/// Text filter.
/// this script use to enchant text with a filter (stroke,shadow)
/// </summary>

using UnityEngine;
using System.Collections;

public class TextFilter : MonoBehaviour {
	
	//Add stroke to text
	public static void DrawOutline(Rect rect, string text, GUIStyle style, Color outColor, Color inColor, float size) //Outline Text
        {
            float halfSize = size * 0.5F;
            GUIStyle backupStyle = new GUIStyle(style);
            Color backupColor = GUI.color;
 
            style.normal.textColor = outColor;
            GUI.color = outColor;
 
            rect.x -= halfSize;
            GUI.Label(rect, text, style);
 
            rect.x += size;
            GUI.Label(rect, text, style);
 
            rect.x -= halfSize;
            rect.y -= halfSize;
            GUI.Label(rect, text, style);
 
            rect.y += size;
            GUI.Label(rect, text, style);
 
            rect.y -= halfSize;
            style.normal.textColor = inColor;
            GUI.color = backupColor;
            GUI.Label(rect, text, style);
 
            style = backupStyle;
        }
 
	//Add shadow to text
        public static void DrawShadow(Rect rect, GUIContent content, GUIStyle style, Color txtColor, Color shadowColor,  //Shadow Text
                                        Vector2 direction)
        {
            GUIStyle backupStyle = style;
 
            style.normal.textColor = shadowColor;
            rect.x += direction.x;
            rect.y += direction.y;
            GUI.Label(rect, content, style);
 
            style.normal.textColor = txtColor;
            rect.x -= direction.x;
            rect.y -= direction.y;
            GUI.Label(rect, content, style);
 
            style = backupStyle;
        }
}

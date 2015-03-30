using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;

public class EditorUtils 
{
	// Vector3
	public static Vector3 Vector3Field(Vector3 textFieldValue, string label)
	{
		return Vector3Field(textFieldValue, label, 0, FieldTypeEnum.WholeLine, true);
	}
	
	public static Vector3 Vector3Field(Vector3 textFieldValue, string label, bool useSeparator)
	{
		return Vector3Field(textFieldValue, label, 0, FieldTypeEnum.WholeLine, useSeparator);
	}
	
	public static Vector3 Vector3Field(Vector3 textFieldValue, FieldTypeEnum fieldType)
	{
		return Vector3Field(textFieldValue, string.Empty, 0, fieldType, true);
	}
	
	public static Vector3 Vector3Field(Vector3 textFieldValue, string label, FieldTypeEnum fieldType)
	{
		return Vector3Field(textFieldValue, label, 0, fieldType, true);
	}
	
	public static Vector3 Vector3Field(Vector3 textFieldValue, string label, int width, FieldTypeEnum fieldType)
	{
		return Vector3Field(textFieldValue, label, 0, fieldType, true);
	}
	
	public static Vector3 Vector3Field(Vector3 textFieldValue, string label, int width, FieldTypeEnum fieldType, bool useSeparator)
	{
		Vector3 fieldValue = textFieldValue;
		
		BeginningField(fieldType, string.Empty, useSeparator); 
		
		if (width > 0)
		{
			fieldValue = EditorGUILayout.Vector3Field(label, fieldValue, GUILayout.Width(width));
		}
		else
		{
			fieldValue = EditorGUILayout.Vector3Field(label, fieldValue);
		}
		
		EndField(fieldType);
		
		return fieldValue;
	}
	
	// TEXT AREA
	public static string TextArea(string textFieldValue, string label)
	{
		return TextArea(textFieldValue, label, 0, FieldTypeEnum.WholeLine, true);
	}
	
	public static string TextArea(string textFieldValue, string label, bool useSeparator)
	{
		return TextArea(textFieldValue, label, 0, FieldTypeEnum.WholeLine, useSeparator);
	}
	
	public static string TextArea(string textFieldValue, FieldTypeEnum fieldType)
	{
		return TextArea(textFieldValue, string.Empty, 0, fieldType, true);
	}
	
	public static string TextArea(string textFieldValue, string label, FieldTypeEnum fieldType)
	{
		return TextArea(textFieldValue, label, 0, fieldType, true);
	}
	
	public static string TextArea(string textFieldValue, string label, int width, FieldTypeEnum fieldType)
	{
		return TextArea(textFieldValue, label, 0, fieldType, true);
	}
	
	public static string TextArea(string textFieldValue, string label, int width, FieldTypeEnum fieldType, bool useSeparator)
	{
		string fieldValue = textFieldValue;
		
		BeginningField(fieldType, label, useSeparator); 
		
		if (width > 0)
		{
			fieldValue = EditorGUILayout.TextArea(fieldValue, GUILayout.Width(width));
		}
		else
		{
			fieldValue = EditorGUILayout.TextArea(fieldValue);
		}
		
		EndField(fieldType);
		
		return fieldValue;
	}
	
	// TEXT FIELD
	public static string TextField(string textFieldValue, string label)
	{
		return TextField(textFieldValue, label, 0, FieldTypeEnum.WholeLine, true);
	}
	
	public static string TextField(string textFieldValue, string label, bool useSeparator)
	{
		return TextField(textFieldValue, label, 0, FieldTypeEnum.WholeLine, useSeparator);
	}
	
	public static string TextField(string textFieldValue, FieldTypeEnum fieldType)
	{
		return TextField(textFieldValue, string.Empty, 0, fieldType, true);
	}
	
	public static string TextField(string textFieldValue, string label, FieldTypeEnum fieldType)
	{
		return TextField(textFieldValue, label, 0, fieldType, true);
	}
	
	public static string TextField(string textFieldValue, string label, int width, FieldTypeEnum fieldType)
	{
		return TextField(textFieldValue, label, 0, fieldType, true);
	}
	
	public static string TextField(string textFieldValue, string label, int width, FieldTypeEnum fieldType, bool useSeparator)
	{
		string fieldValue = textFieldValue;
		
		BeginningField(fieldType, label, useSeparator); 
		
		if (width > 0)
		{
			fieldValue = EditorGUILayout.TextField(fieldValue, GUILayout.Width(width));
		}
		else
		{
			fieldValue = EditorGUILayout.TextField(fieldValue);
		}
		
		EndField(fieldType);
		
		return fieldValue;
	}
	
	// INT FIELD
	public static int IntField(int intFieldValue, string label)
	{
		return IntField(intFieldValue, label, 0, FieldTypeEnum.WholeLine, false);
	}
	
	public static int IntField(int intFieldValue, string label, FieldTypeEnum fieldType)
	{
		return IntField(intFieldValue, label, 0, fieldType, true);
	}
	
	public static int IntField(int intFieldValue, FieldTypeEnum fieldType)
	{
		return IntField(intFieldValue, string.Empty, 0, fieldType);
	}
	
	public static int IntField(int intFieldValue, string label, int width, FieldTypeEnum fieldType)
	{
		return IntField(intFieldValue, label, 0, fieldType, true);
	}
	
	public static int IntField(int intFieldValue, string label, int width, FieldTypeEnum fieldType, bool useSeparator)
	{
		int fieldValue = intFieldValue;
		
		BeginningField(fieldType, label, useSeparator); 
		
		if (width > 0)
		{
			fieldValue = EditorGUILayout.IntField(fieldValue, GUILayout.Width(width));
		}
		else
		{
			fieldValue = EditorGUILayout.IntField(fieldValue);
		}
		
		EndField(fieldType);
		
		return fieldValue;
	}
	
	//MIN MAX RANGE
	public static void MinMaxSlider(ref float minValue,ref  float maxValue, float min, float max, string label)
	{
		MinMaxSlider(ref minValue, ref maxValue,min, max, label, 0, FieldTypeEnum.WholeLine, false);
	}
	
	public static void MinMaxSlider(ref float minValue,ref  float maxValue, float min, float max, string label, FieldTypeEnum fieldType)
	{
		MinMaxSlider(ref minValue, ref maxValue,min, max, label, 0, fieldType, false);
	}
	
	public static void MinMaxSlider(ref float minValue,ref  float maxValue, float min, float max, FieldTypeEnum fieldType)
	{
		MinMaxSlider(ref minValue, ref maxValue,min, max, string.Empty, 0, fieldType, false);
	}
	
	public static void MinMaxSlider(ref float minValue,ref  float maxValue, float min, float max, string label, int width, FieldTypeEnum fieldType)
	{
		MinMaxSlider(ref minValue, ref maxValue,min, max, string.Empty, 0, fieldType, false);
	}
	
	public static void MinMaxSlider(ref float minValue,ref  float maxValue, float min, float max, string label, int width, FieldTypeEnum fieldType, bool useSeparator)
	{
		BeginningField(fieldType, label, useSeparator);
		
		if (width > 0)
		{
			EditorGUILayout.MinMaxSlider(ref minValue, ref maxValue,min, max , GUILayout.Width(width));
		}
		else
		{
			EditorGUILayout.MinMaxSlider(ref minValue, ref maxValue, min, max);
		}
		
		EndField(fieldType);
	}
	
	//FLOAT RANGE
	public static float Slider(float floatFieldValue, float min, float max, string label)
	{
		return Slider(floatFieldValue, min, max, label, 0, FieldTypeEnum.WholeLine, false);
	}
	
	public static float Slider(float floatFieldValue, float min, float max, string label, FieldTypeEnum fieldType)
	{
		return Slider(floatFieldValue, min, max, label, 0, fieldType, false);
	}
	
	public static float Slider(float floatFieldValue, float min, float max, FieldTypeEnum fieldType)
	{
		return Slider(floatFieldValue, min, max, string.Empty, 0, fieldType, false);
	}
	
	public static float Slider(float floatFieldValue, float min, float max, string label, int width, FieldTypeEnum fieldType)
	{
		return Slider(floatFieldValue, min, max, label, 0, fieldType, false);
	}
	
	public static float Slider(float floatFieldValue, float min, float max, string label, int width, FieldTypeEnum fieldType, bool useSeparator)
	{
		float fieldValue = floatFieldValue;
		
		BeginningField(fieldType, label, useSeparator);
		
		if (width > 0)
		{
			fieldValue = EditorGUILayout.Slider(fieldValue,min, max , GUILayout.Width(width));
		}
		else
		{
			fieldValue = EditorGUILayout.Slider(fieldValue, min, max);
		}
		
		EndField(fieldType);
		
		return fieldValue;
	}
	
	// FLOAT FIELD
	public static float FloatField(float floatFieldValue, string label)
	{
		return FloatField(floatFieldValue, label, 0, FieldTypeEnum.WholeLine, false);
	}
	
	public static float FloatField(float floatFieldValue, string label, FieldTypeEnum fieldType)
	{
		return FloatField(floatFieldValue, label, 0, fieldType, false);
	}
	
	public static float FloatField(float floatFieldValue, FieldTypeEnum fieldType)
	{
		return FloatField(floatFieldValue, string.Empty, 0, fieldType, false);
	}
	
	public static float FloatField(float floatFieldValue, string label, int width, FieldTypeEnum fieldType)
	{
		return FloatField(floatFieldValue, label, 0, fieldType, false);
	}
	
	public static float FloatField(float floatFieldValue, string label, int width, FieldTypeEnum fieldType, bool useSeparator)
	{
		float fieldValue = floatFieldValue;
		
		BeginningField(fieldType, label, useSeparator);
		
		if (width > 0)
		{
			fieldValue = EditorGUILayout.FloatField(fieldValue, GUILayout.Width(width));
		}
		else
		{
			fieldValue = EditorGUILayout.FloatField(fieldValue);
		}
		
		EndField(fieldType);
		
		return fieldValue;
	}
	
	// Toggle
	public static bool Toggle(bool floatFieldValue, string label)
	{
		return Toggle(floatFieldValue, label, 0, FieldTypeEnum.WholeLine, false);
	}
	
	public static bool Toggle(bool floatFieldValue, string label, FieldTypeEnum fieldType)
	{
		return Toggle(floatFieldValue, label, 0, fieldType);
	}
	
	public static bool Toggle(bool floatFieldValue, FieldTypeEnum fieldType)
	{
		return Toggle(floatFieldValue, string.Empty, 0, fieldType);
	}
	
	public static bool Toggle(bool floatFieldValue, string label, int width, FieldTypeEnum fieldType)
	{
		return Toggle(floatFieldValue, label, 0, fieldType, true);
	}
	
	public static bool Toggle(bool floatFieldValue, string label, int width, FieldTypeEnum fieldType, bool useSeparator)
	{
		bool fieldValue = floatFieldValue;
		
		BeginningField(fieldType, label, useSeparator);
		
		if (width > 0)
		{
			fieldValue = EditorGUILayout.Toggle(fieldValue, GUILayout.Width(width));
		}
		else
		{
			fieldValue = EditorGUILayout.Toggle(fieldValue);
		}
		
		EndField(fieldType);
		
		return fieldValue;
	}
	
	// Enum popup
	public static int IntPopup(int intFieldValue, List<IItem> list, string label)
	{
		return IntPopup(intFieldValue, list, label, 0, FieldTypeEnum.WholeLine, false);
	}
	
	public static int IntPopup(int intFieldValue, List<IItem> list, string label, FieldTypeEnum fieldType)
	{
		return IntPopup(intFieldValue, list, label, 0, fieldType);
	}
	
	public static int IntPopup(int intFieldValue, List<IItem> list, FieldTypeEnum fieldType)
	{
		return IntPopup(intFieldValue, list, string.Empty, 0, fieldType);
	}
	
	public static int IntPopup(int intFieldValue, List<IItem> list, string label, int width, FieldTypeEnum fieldType)
	{
		return IntPopup(intFieldValue, list, label, 0, fieldType, false);
	}
	
	public static int IntPopup(int intFieldValue, List<IItem> list, string label, int width, FieldTypeEnum fieldType, bool useSeparator)
	{
		int fieldValue = intFieldValue;
		
		BeginningField(fieldType, label, useSeparator);
		
		string[] names = new string[list.Count];
		int[] ID = new int[list.Count];
		int index = 0;
		foreach(IItem s in list)
		{
			names[index] = s.Name;
			ID[index] = s.ID;
			index++;
		}
		
		if (width > 0)
		{
			fieldValue = EditorGUILayout.IntPopup(fieldValue, names, ID ,GUILayout.Width(width));
		}
		else
		{
			fieldValue = EditorGUILayout.IntPopup(fieldValue, names, ID);
		}
		
		EndField(fieldType);
		
		return fieldValue;
	}
	
	// Label
	public static void Label(string label)
	{
		EditorGUILayout.Separator();
		EditorGUILayout.BeginHorizontal();
		
		EditorGUILayout.LabelField(label);
		EditorGUILayout.EndHorizontal();
	}
	
	private static void BeginningField(FieldTypeEnum fieldType, string label, bool useSeparator)
	{
		if (fieldType == FieldTypeEnum.WholeLine || fieldType == FieldTypeEnum.BeginningOnly)
		{
			if (useSeparator)
				EditorGUILayout.Separator();
			EditorGUILayout.BeginHorizontal();
		}
		if (!string.IsNullOrEmpty(label))
			EditorGUILayout.PrefixLabel(label); 
	}
	
	private static void EndField(FieldTypeEnum fieldType)
	{
		if (fieldType == FieldTypeEnum.WholeLine || fieldType == FieldTypeEnum.EndOnly)
		{
			EditorGUILayout.EndHorizontal();
		}
	}
	
	// GENERAL
	public static void Separator()
	{
		Label("**************************************************************************************************************");
	}

    public static void MainSeparator()
    {
        Label("###############################################################################################################");
    }
	
	public static void IItemEditor(IItem item)
	{
        EditorUtils.MainSeparator();
		
		item.ID = IntField(item.ID, "ID");


		item.Name = TextField(item.Name, "Name");
		
		item.Description = TextField(item.Description, "Description");

        EditorUtils.Label("Unique ID:   " + item.UniqueId);

        EditorUtils.MainSeparator();
	}
	
	public static int NewAttributeID<T>(List<T> items)
	{
		int maximum = 0;
		foreach(IItem p in items)
		{
			if (p.ID > maximum)
				maximum = p.ID;
		}
		maximum++; 
		return maximum;
	}

    public static Rect EditorRect(string label, Rect rect)
    {
        Label(label);

        rect.x = FloatField(rect.x, "x", 50, FieldTypeEnum.BeginningOnly);

        rect.y = FloatField(rect.y, "x", 50, FieldTypeEnum.Middle);

        rect.width = FloatField(rect.width, "width", 50, FieldTypeEnum.Middle);

        rect.height = FloatField(rect.height, "height", 50, FieldTypeEnum.EndOnly);

        return rect;
    } 
}

public enum FieldTypeEnum
{
	WholeLine,
	BeginningOnly,
	EndOnly,
	Middle
}



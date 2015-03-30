using UnityEngine;
using UnityEditor;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Xml;

public class ConversationEditor : BaseEditorWindow 
{
	
	public ConversationEditor(GUISkin guiSkin, MainWindowEditor data)
	{
		EditorName = "Conversation";
		
		Init(guiSkin, data);
	}
	
	protected override void LoadData()
	{
		List<RPGParagraph> list = Storage.Load<RPGParagraph>(new RPGParagraph());
		items = new List<IItem>();
		foreach(RPGParagraph category in list)
		{
			category.SystemDescription = category.ParagraphText;
			items.Add((IItem)category);
		}
	}
	
	protected override void StartNewIItem()
	{
		currentItem = new RPGParagraph();
	}
	
	public List<RPGParagraph> Paragraphs
	{
		get
		{
			List<RPGParagraph> list = new List<RPGParagraph>();
			foreach(IItem category in items)
			{
				if (string.IsNullOrEmpty(category.Name))
					category.Name = category.ID.ToString();
				list.Add((RPGParagraph)category);
			}
			return list;
		}
	}
	
	protected override void SaveCollection()
	{
		Storage.Save<RPGParagraph>(Paragraphs, new RPGParagraph());
	}
	
	protected override void EditPart()
	{
		RPGParagraph s = (RPGParagraph)currentItem;

        s.IsGeneralConversation = EditorUtils.Toggle(s.IsGeneralConversation, "General topic (for more NPC");

        if (!s.IsGeneralConversation)
        {
            s.OwnerId = EditorUtils.IntPopup(s.OwnerId, Data.npcEditor.items, "NPC");
        }
        else
        {
            s.OwnerId = 0;
        }

		EditorGUILayout.Separator();
		EditorGUILayout.BeginHorizontal();
		
		EditorGUILayout.PrefixLabel("NPC text");
		s.ParagraphText = EditorGUILayout.TextArea(s.ParagraphText, GUILayout.Width(700));
		EditorGUILayout.EndHorizontal();
		
		s.ParentLineTextId = EditorUtils.IntField(s.ParentLineTextId, "Previous line");
		
		EditorGUILayout.BeginHorizontal();
		if (s.ParentLineTextId > 0)
		{
			RPGParagraph p = GetPreviousLine(s.ParentLineTextId);
			if (p != null)
			{
				if (GUILayout.Button("Show previous line", GUILayout.Width(150)))
				{
					currentItem = p;
					return;
				}
			}
		}
		EditorGUILayout.EndHorizontal();
		
		s.CanEndParagraph = EditorUtils.Toggle(s.CanEndParagraph, "End");
		
		s.CanReturn = EditorUtils.Toggle(s.CanReturn, "Return to first");
		
		
		s.QuestID = EditorUtils.IntPopup(s.QuestID, Data.questEditor.items, "Quest", 100, FieldTypeEnum.BeginningOnly);
		
		if (GUILayout.Button("Add line text for quest", GUILayout.Width(180)))
		{
			s.AddLineText();
		}
		
		EditorGUILayout.EndHorizontal();
		
		GUIUtils.ConditionsEvents(s.Conditions, s.Actions, Data);
		EditorUtils.Separator();
		EditorUtils.Label("Player responses");
		
		
		if (s.LineTexts != null && s.LineTexts.Count >0)
		{
			foreach(LineText lineText in s.LineTexts)
			{
				EditorGUILayout.BeginVertical(skin.box);
				
				if (AddLineText(lineText, s))
					return;
				
				EditorGUILayout.BeginHorizontal();
				
				if (GUILayout.Button("Remove", GUILayout.Width(80)))
				{
					s.LineTexts.Remove(lineText);
					break;
				}
				
				EditorGUILayout.EndHorizontal();
				
				EditorGUILayout.EndVertical();
			}
		}
		
		EditorUtils.Separator();
		
		if (GUILayout.Button("Add line text",GUILayout.Width(400)))
		{
			LineText lt = new LineText();
			lt.ID = NewLineTextID() + 1;
			s.LineTexts.Add(lt);
		}
		
		currentItem = s;
	}
	
	
	RPGParagraph GetPreviousLine(int lineTextID)
	{
		foreach(RPGParagraph p in Paragraphs)
		{
			foreach(LineText lt in p.LineTexts)
			{
				if (lt.ID == lineTextID)
					return p;
			}
		}
		
		return null;
	}
	
	RPGParagraph GetByLineTextID(int lineTextID)
	{
		foreach(RPGParagraph p in Paragraphs)
		{
			if (p.ParentLineTextId == lineTextID)
				return p;
		}
		return null;
	}
	
	bool AddLineText(LineText lineText, RPGParagraph s)
	{
		EditorUtils.Label("Player text number " + lineText.ID);
		
		EditorGUILayout.BeginHorizontal();
		if (updateMode && s.OwnerId > 0)
		{
			RPGParagraph p = GetByLineTextID(lineText.ID);
			if (p != null)
			{
				if (GUILayout.Button("Show reaction - " + p.ID, GUILayout.Width(150)))
				{
					currentItem = p;
					return true;
				}
			}
			else
			{
				if (GUILayout.Button("Add reaction", GUILayout.Width(150)))
				{
					int ownerId = s.OwnerId;
					s = new RPGParagraph();
					s.ID = GUIUtils.NewAttributeID<RPGParagraph>(Paragraphs);
					s.OwnerId = ownerId;
					s.ParentLineTextId = lineText.ID;
                    items.Add(s);
					currentItem = s;
					return true;
				}
			}
		}
		
		lineText.Text = EditorUtils.TextField(lineText.Text, "Player text", FieldTypeEnum.Middle);
		EditorGUILayout.EndHorizontal();
		
		EditorGUILayout.BeginVertical(skin.customStyles[0]);
		EditorGUILayout.BeginHorizontal();
		
		if (GUILayout.Button("Add other text", GUILayout.Width(120)))
		{
			lineText.AdditionalTexts.Add(new LongLineText());
		}
		
		EditorGUILayout.EndHorizontal();
		
		foreach(LongLineText lt in lineText.AdditionalTexts)
		{
			lt.NPCText = EditorUtils.TextField(lt.NPCText, "NPC text");
			
			lt.PlayerText = EditorUtils.TextField(lt.PlayerText, "Player text");
			
			lt.IsEnd = EditorUtils.Toggle(lt.IsEnd, "End conversation");
			
			EditorGUILayout.BeginHorizontal();
			
			if (GUILayout.Button("Remove", GUILayout.Width(80)))
			{
				lineText.AdditionalTexts.Remove(lt);
				break;
			}
			
			EditorGUILayout.EndHorizontal();
		}
		EditorGUILayout.EndVertical();
		
		GUIUtils.ConditionsEvents(lineText.Conditions, lineText.GameEvents, Data);
		
		return false;
	}
	
	int NewLineTextID()
	{
		int maximum = 0;
		foreach(RPGParagraph p in Paragraphs)
		{
			foreach(LineText lt in p.LineTexts)
			{
				if (lt.ID > maximum)
					maximum = lt.ID;
			}
		}
		return maximum;
	}
	
	/*
	
	void DisplayLines()
	{
		
		EditorGUILayout.Separator();
		EditorGUILayout.BeginHorizontal();
		
		if (GUILayout.Button("Back to paragraph list", GUILayout.Width(350))) 
		{
			MenuMode = 1;
		}
		
		EditorGUILayout.PrefixLabel("NPC");
		NPC = EditorGUILayout.IntField(NPC, GUILayout.Width(200));
		
		if (GUILayout.Button("Show conversation", GUILayout.Width(300)))
		{
			LoadWindows();
		}
		EditorGUILayout.EndHorizontal();
		
		BeginWindows();
		
		foreach(ParagraphWindow window in windows)
		{
			window.position = GUI.Window(window.WindowId, window.position, DisplayWindow,window.paragraph.ID.ToString()); 
		}
		EndWindows();
	}
	
	
	
	void Delete(int paragraphId)
	{
		foreach(Paragraph p in paragraphs)
		{
			if (p.ID == paragraphId)
			{
				paragraphs.Remove(p);
				break;
			}
		}
		Storage.Save<Paragraph>(paragraphs, new Paragraph());
		
		LoadItems();
	}
	
	private void LoadWindows()
	{
		ConversationUtils utils = new ConversationUtils();
		windows = utils.DisplayParagraphWindows(Paragraph.LoadAllByOwner(NPC));
	}
	
	List<ParagraphWindow> windows = new List<ParagraphWindow>();
	
	void DisplayWindow(int id)
	{
		windows[id].ScrollPosition = EditorGUILayout.BeginScrollView(windows[id].ScrollPosition);
		Paragraph p = windows[id].paragraph;
		
		foreach(Paragraph paragraph in paragraphs)
		{
			if (p.ID != paragraph.ID)
						continue;
			p = paragraph;
		}
		if (GUI.Button(new Rect(0,5,300,30), p.ParagraphText))
		{
				currentParagraph = p;
				MenuMode = 2;
                editMode = true;
		}
		int index = 0;
		foreach(LineText lt in p.LineTexts)
		{
			if (GUI.Button(new Rect(5,55 + (index * 25), 15,20), "+")) 
			{
				currentParagraph = new Paragraph();
				currentParagraph.ID =GUIUtils.NewAttributeID<Paragraph>(paragraphs);
				currentParagraph.OwnerId = p.OwnerId;
				currentParagraph.ParentLineTextId = lt.ID;
				MenuMode = 2;
				break;
			}
			if (GUI.Button(new Rect(280, 55 + (index * 25), 20,20), "X"))
			{
				p.LineTexts.Remove(lt);
				MenuMode = 1;
				SaveParagraph();
				break;
			}
			GUI.Label(new Rect(25,55 + (index * 25),280,20),lt.ID.ToString() + ". " + lt.Text);
			index++;
		}
		EditorGUILayout.EndScrollView();
		GUI.DragWindow();
		
	}
	*/
}

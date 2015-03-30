using UnityEngine;
using UnityEditor;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Xml;

public class ConversationUtils {
	
	public static int WindowWidth = 310;
	public static int WindowHeight = 150;
	
	List<RPGParagraph> displayedForOwner = new List<RPGParagraph>();
	int Level = 1;
	int totalWindow = 0;
	List<ParagraphWindow> paragraphWindows = new List<ParagraphWindow>();
	
	public List<ParagraphWindow> DisplayParagraphWindows(List<RPGParagraph> paragraphs)
	{
		displayedForOwner = paragraphs;
		
		if (displayedForOwner == null || displayedForOwner.Count == 0)
            return paragraphWindows;
		
		totalWindow = 0;
		int index = 0;
		
		foreach(RPGParagraph para in displayedForOwner)
		{
			if (para.ParentLineTextId > 0)
				continue;
			Rect rect = new Rect(60 +(index * 50),90,WindowWidth,WindowHeight);
			
			AddParagraphWindow(rect, rect, totalWindow, para);
			AddWindowsBeneath(para, rect);
			index++;
			totalWindow++;
			Level = 1;
		}
		return paragraphWindows;
	}
	
	void AddWindowsBeneath(RPGParagraph parentParagraph, Rect parentWindow)
	{
		int index = 0;
		foreach(RPGParagraph para in displayedForOwner)
		{
			if (paragraphWindows.Count == displayedForOwner.Count)
				break;
			foreach(LineText lineText in parentParagraph.LineTexts)
			{
				if (lineText.ID != para.ParentLineTextId)
					continue;
				totalWindow++;
				Rect rect = new Rect(60 +(index * 90),90 + (Level * 150),WindowWidth,WindowHeight);
				AddParagraphWindow(rect, parentWindow, totalWindow, para);
				Level++;
				AddWindowsBeneath(para, rect);
				Level--;
				index++;
			}			        
		}
	}
	
	/*void AddWindows(Paragraph parentParagraph, Rect parentWindow)
	{
		int index = 0;
		foreach(Paragraph para in displayedForOwner)
		{
			if (paragraphWindows.Count == displayedForOwner.Count)
				break;
			foreach(LineText lineText in parentParagraph.LineTexts)
			{
				if (lineText.ID != para.ParentLineTextId)
					continue;
				totalWindow++;
				Rect rect = new Rect(60 +(index * 320),90 + (Level * 240),300,200);
				AddParagraphWindow(rect, parentWindow, totalWindow, para);
				Level++;
				//AddWindowsBeneath(para, rect);
				Level--;
				index++;
			}			        
		}
	}*/
	
	void AddParagraphWindow(Rect position, Rect parentWindowPosition, int id, RPGParagraph paragraph)
	{
		ParagraphWindow pw = new ParagraphWindow();
		pw.paragraph = paragraph;
		pw.ParentWindow = parentWindowPosition;
		pw.position = position;
		pw.WindowId = id;
		paragraphWindows.Add(pw);
	}
}

public class ParagraphWindow
{
	public Rect position;
	public int WindowId;
	public RPGParagraph paragraph;
	public Rect ParentWindow;
	public Vector2 ScrollPosition = new Vector2();
}

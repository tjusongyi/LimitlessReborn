using UnityEngine;
using System.Xml;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
using System.IO;
using System;

public class LongLineText 
{
	[XmlElement (ElementName = "NT")]
	public string NPCText;
	
	[XmlElement (ElementName = "PT")]
	public string PlayerText;
	
	[XmlElement (ElementName = "IE")]
	public bool IsEnd;
	
	public LongLineText()
	{
		NPCText = string.Empty;
		PlayerText = string.Empty;
	}
}

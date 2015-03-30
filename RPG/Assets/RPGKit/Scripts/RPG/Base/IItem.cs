using UnityEngine;
using System.Collections;

public interface IItem
{
	int ID { get; set;}
	
	string Name { get; set;}
	
	string Description { get; set;}
	
	string SystemDescription { get; set;}
	
	string Preffix{  get; }
	
	string UniqueId { get;}
}
/// <summary>
/// Detect area.
/// This script use for control area detect enemy to detect a hero
/// </summary>

using UnityEngine;
using System.Collections;

public class DetectArea : MonoBehaviour {
	
	[HideInInspector]
	public EnemyController controller;
	
	public bool drawGizmos;
	
	// Use this for initialization
	void Start () {
		
		controller = transform.root.GetComponent<EnemyController>();
	
	}
	
	void OnTriggerEnter(Collider c)
	{
		if(c.gameObject.tag == "Player")
		{
			controller.chaseTarget = true;
			controller.target = c.gameObject;
		}
	}
	
	void OnTriggerExit(Collider c)
	{
		if(c.gameObject.tag == "Player")
		{
			controller.chaseTarget = false;
			controller.target = null;
		}
	}
	
	//Draw Gizmoz
	[ExecuteInEditMode]
	void OnDrawGizmosSelected()
	{
		if(drawGizmos)
		{
			Gizmos.color = new Color(1f,0f,0f,0.3f);
			Gizmos.DrawSphere(transform.position,this.GetComponent<SphereCollider>().radius);
		}
	}
}

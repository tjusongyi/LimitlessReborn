/// <summary>
/// This script use for show text damage if hero or enemy attack
/// </summary>

using UnityEngine;
using System.Collections;

public class TextDamage : MonoBehaviour {
	
	public GameObject root;
	
	void Update () {
		
		//show text damage
		if(this.GetComponent<TextMesh>() != null){
			root.transform.LookAt(Camera.mainCamera.transform.position);
			this.GetComponent<TextMesh>().renderer.material.color -= new Color(0,0,0,2f*Time.deltaTime);
			this.transform.Translate(Vector3.up * 1.5f * Time.deltaTime);
			if(this.GetComponent<TextMesh>().renderer.material.color.a <= 0){
				Destroy(root.gameObject);	
			}
		}
	}
	
	public void SetDamage(string damage, Color colotText){
		//recive & send damage variable
		this.GetComponent<TextMesh>().text = damage.ToString();
		this.GetComponent<TextMesh>().renderer.material.color = colotText;
	}
}

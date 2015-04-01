using UnityEngine;
using System.Collections;

public abstract class PanelBase : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public abstract void OnShow(object param);
}

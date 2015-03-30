/// <summary>
/// Look at camera.
/// This script is use for item to look at camera
/// </summary>

using UnityEngine;
using System.Collections;

public class LookAtCamera : MonoBehaviour {
	
	private Vector3 targetLook;
	
	void Update () {
		
		transform.LookAt(Camera.mainCamera.transform.position);
	}
}

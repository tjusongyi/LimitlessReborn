/// <summary>
/// Mouse orbit.
/// This script use to control a main camera
/// </summary>

using UnityEngine;
using System.Collections;

public class MouseOrbit : MonoBehaviour {
	
	[HideInInspector]
	public GameObject target; //a target look at
    public float xSpeed; //speed pan x
	public float ySpeed; //speed pan y
	public float yMinLimit; //y min limit
	public float yMaxLimit; //y max limit
	
    public float scrollSpeed; //scroll speed
	public float zoomMin;  //zoom min
	public float zoomMax; //zoom max
	
	//Private variable
	private float distance;
	private float distanceLerp;
	private Vector3 position; 
    private bool isActivated;
    private float x;
	private float y;
	private bool setupCamera;

    // Use this for initialization
 
    void Start () {
		
		
		//Warning when not found target
		if(target == null)
		{
			target = GameObject.FindGameObjectWithTag("Player");
			
			if(target == null)
			{
				Debug.LogWarning("Don't found player tag please change player tag to Player");	
			}
		}
		
        
		//Setup Pos
		 Vector3 angles = transform.eulerAngles;
		 x = angles.y;
		 y = angles.x;
				
		CalDistance();
    }
 
 
 
    void LateUpdate () {
 		
		ScrollMouse();
		RotateCamera();
 
	}
	
	//Roate camera method
	void RotateCamera()
	{
		if (Input.GetMouseButtonDown(1)){
 
			isActivated = true;
 
		} 
 
		// if mouse button is let UP then stop rotating camera 
		if (Input.GetMouseButtonUp(1))
		{
			isActivated = false;
		} 
 
 
 
	    if (target && isActivated) { 
			
		  y -= Input.GetAxis("Mouse Y") * ySpeed;

		  x += Input.GetAxis("Mouse X") * xSpeed;

	        
	 
	      y = ClampAngle(y, yMinLimit, yMaxLimit);
	 
	 
	       Quaternion rotation = Quaternion.Euler(y, x, 0);
			
			Vector3 calPos = new Vector3(0, 0, -distanceLerp);

	         position = rotation * calPos + target.transform.position;
	 
	        transform.rotation = rotation;
	 
	        transform.position = position;
	
 
		} else
		{
			Quaternion rotation = Quaternion.Euler(y, x, 0);
			
			Vector3 calPos = new Vector3(0, 0, -distanceLerp);

	         position = rotation * calPos + target.transform.position;
	 
	        transform.rotation = rotation;
	 
	        transform.position = position;
		}
	}
	
 	//Calculate Distance Method
 	void CalDistance()
	{
		distance = zoomMax;
		distanceLerp = distance;
		Quaternion rotation = Quaternion.Euler(y, x, 0);	
		Vector3 calPos = new Vector3(0, 0, -distanceLerp);
	    position = rotation * calPos + target.transform.position;
	    transform.rotation = rotation;
	    transform.position = position;
	}
	
	//Scroll Mouse Method
	void ScrollMouse()
	{
		distanceLerp = Mathf.Lerp(distanceLerp,distance,Time.deltaTime * 5);
		if (Input.GetAxis("Mouse ScrollWheel") != 0 && !GUI_Menu.instance.CheckHoverItemShop() && !GUI_Menu.instance.CheckHoverSkillWindow()) 
			{	
				// get the distance between camera and target
 
				distance = Vector3.Distance (transform.position , target.transform.position);	
 
				distance = ScrollLimit(distance - Input.GetAxis("Mouse ScrollWheel")*scrollSpeed, zoomMin, zoomMax);
 
			}
	}
	
 	//Scroll Limit Method
	float ScrollLimit(float dist, float min, float max)
    {
        if (dist < min)
 
            dist= min;
 
        if (dist > max)
 
            dist= max; 
 
		return dist;
    }
	
	
	//Clamp Angle Method
	float ClampAngle(float angle,float min,float max)
	{
		if(angle < -360)
			angle += 360;
		if(angle > 360)
			angle -= 360;
		return Mathf.Clamp(angle,min,max);
	}
}

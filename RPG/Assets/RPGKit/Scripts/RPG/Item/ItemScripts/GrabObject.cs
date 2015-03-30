using UnityEngine;
using System.Collections;

public class GrabObject : MonoBehaviour 
{

	int normalCollisionCount = 1;
	float moveLimit = 1.5f;
	float collisionMoveFactor = .01f;
	int addHeightWhenClicked = 2;
	bool freezeRotationOnDrag = true;
	private Camera cam;
	private Rigidbody myRigidbody;
	private Transform myTransform;
	private bool canMove;
	private float yPos;
	private bool gravitySetting;
	private bool freezeRotationSetting;
	private float sqrMoveLimit;
	private int collisionCount;
	private Transform camTransform;
	private Transform playerTransform;
    private Player player;

	void Start()
	{
		myRigidbody = rigidbody;
    	myTransform = transform;
    
	    cam = Camera.main;
		playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
        player = playerTransform.GetComponent<Player>();
    	camTransform = cam.transform;
    	sqrMoveLimit = moveLimit * moveLimit;   // Since we're using sqrMagnitude, which is faster than magnitude
	}
	
	// Update is called once per frame
	void Update () 
	{
        if (rigidbody == null)
        {
            Destroy(this);
            return;
        }

        if (Input.GetKeyUp(player.Hero.Controls.GrabItem))
		{
			canMove = false;
    		myRigidbody.useGravity = gravitySetting;
    		myRigidbody.freezeRotation = freezeRotationSetting;
    		if (!myRigidbody.useGravity) 
			{
				Vector3 position = myTransform.position;
				position.y = yPos-addHeightWhenClicked;
        		myTransform.position = position;
    		}
			
			Destroy(this);
		}
		else
		{
			if (canMove)
				return;
			
    		myTransform.Translate(Vector3.up*addHeightWhenClicked);
    		gravitySetting = myRigidbody.useGravity;
    		freezeRotationSetting = myRigidbody.freezeRotation;
    		myRigidbody.useGravity = false;
    		myRigidbody.freezeRotation = freezeRotationOnDrag;
    		yPos = myTransform.position.y;
			canMove = true;
		}
	}
	
	void OnCollisionEnter () {
    	collisionCount++;
	}

	void OnCollisionExit () {
    	collisionCount--;
	}
	
	void FixedUpdate()
	{
		if (!canMove) 
			return;
    
	    myRigidbody.velocity = Vector3.zero;
    	myRigidbody.angularVelocity = Vector3.zero;
		Vector3 myPosition = myTransform.position;
		myPosition.y = yPos;
    	myTransform.position = myPosition;
    	Physics.IgnoreCollision(playerTransform.collider, myTransform.collider);
    	Vector3 mousePos = Input.mousePosition;
    	Vector3 move = cam.ScreenToWorldPoint(new Vector3(mousePos.x, mousePos.y, camTransform.position.y - myTransform.position.y)) - myTransform.position;
	    if (collisionCount > normalCollisionCount) {
    	    move = move.normalized*collisionMoveFactor;
    	}
    	else if (move.sqrMagnitude > sqrMoveLimit) {
        	move = move.normalized*moveLimit;
    	}
    	myRigidbody.MovePosition(myRigidbody.position + move);
	}
}

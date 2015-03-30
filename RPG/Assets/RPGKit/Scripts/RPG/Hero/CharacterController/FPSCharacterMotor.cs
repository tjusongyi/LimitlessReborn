using UnityEngine;
using System.Collections;

[RequireComponent (typeof (CharacterController))]
public class FPSCharacterMotor : MonoBehaviour 
{
    public float v;
    public float h;
    public Player player;

    Vector3 targetDirection;
    CharacterController controller;
    public Vector2 rotationVector = Vector2.zero;
    private Vector3 previousPosition;

    void Awake()
    {
        controller = (CharacterController)gameObject.GetComponent<CharacterController>();
        previousPosition = transform.position;
    }

    void OnLevelWasLoaded(int level)
    {
        player = GetComponent<Player>();
    }
    

    void UpdateSmoothedMovementDirection()
    {
        // Forward vector relative to the camera along the x-z plane	
        var forward = Camera.main.transform.TransformDirection(Vector3.forward);
        forward.y = 0;
        forward = forward.normalized;

        // Right vector relative to the camera
        // Always orthogonal to the forward vector
        var right = new Vector3(forward.z, 0, -forward.x);


        // Target direction relative to the camera
        targetDirection = h * right + v * forward;
       
    }

    void LateUpdate()
    {
        if (player != null && (player.scene == null || player.scene.SceneType == SceneTypeEnum.MainMenu || !BasicGUI.IsAllWindowsClosed || BasicGUI.isNoteDisplayed))
            return;

        UpdateSmoothedMovementDirection();
        previousPosition = transform.position;

        Vector3 movement = targetDirection * 6;

        movement.y = -8f;
        movement *= Time.deltaTime;
        controller.Move(movement);

       

        if (rotationVector != Vector2.zero)
        {
            transform.Rotate(0, rotationVector.x, 0, Space.World);
            Camera.mainCamera.transform.Rotate(-rotationVector.y, 0, 0);
            rotationVector = Vector2.zero;
        }

    }
}

public enum MovementTransferOnJump
{
	None,
	InitTransfer,
	PermaTransfer,
	PermaLocked
}



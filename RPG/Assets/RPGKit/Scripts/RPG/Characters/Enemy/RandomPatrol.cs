using UnityEngine;
using System.Collections;

public class RandomPatrol
{
    public Transform currentPosition;
	public Transform t;
	public int MaximumPatrolDistance;
	public CharacterStatus Status;
		
	//target point for position
	private Vector3 targetPosition;
	private Vector3 originalPosition;
	private int NumberOfSteps;
	CharacterController characterController;
	CollisionFlags flags;
	
	public RandomPatrol(Transform transform, CharacterController controller)
	{
		t = transform;
		targetPosition = new Vector3(0, 0,0);
		originalPosition = t.position;
		characterController = controller;
		CreateNewTargetPosition();
	}
	
	private void CreateNewTargetPosition()
	{
		int changeX = Random.Range(-20, 20);
		int changeZ = Random.Range(-20, 20);
		targetPosition = new Vector3(originalPosition.x + changeX, t.position.y, originalPosition.z + changeZ);
		t.LookAt(targetPosition);
	}

	
	public void Step()
	{
		if (Vector3.Distance(targetPosition, t.position) < 0.5f)
			CreateNewTargetPosition();
				
		DoStep();
	}
	
	private void DoStep()
	{
		if (flags != CollisionFlags.CollidedBelow)
		{
			if (Terrain.activeTerrain != null)
			{
				Vector3 position = characterController.transform.position;
				position.y = Terrain.activeTerrain.SampleHeight(characterController.transform.position) + 0.75f;
				characterController.transform.position = position;
				
				flags = characterController.Move(t.forward * 1.5f * Time.deltaTime);
			}
			else
			{
				flags = characterController.Move(Vector3.down * 10 * Time.deltaTime);	
			}
		}
		else
		{
			flags = characterController.Move(t.forward * 1.5f * Time.deltaTime);
		}
	}
}

public enum CharacterStatus
{
	Idle = 0,
	Rotating = 1,
	RotatinInCombat = 2,
	Patrol = 3,
	Run = 4,
	ChaseEnemy = 5,
	Returning = 6,
	Hit = 7,
	Dying = 8
}
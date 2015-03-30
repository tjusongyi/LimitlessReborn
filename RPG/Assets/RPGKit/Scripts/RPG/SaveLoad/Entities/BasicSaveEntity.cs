using UnityEngine;
using System.Collections;

public class BasicSaveEntity
{
	public ObjectPosition Position;
	public ObjectPosition OriginalPosition;
	public int ID;
	
	public bool CheckPosition(Vector3 position, Quaternion rotation)
	{
		if (OriginalPosition.GetPosition() == position && OriginalPosition.GetRotation() == rotation)
			return true;
		return false;	
	}
	
	public void FillBasicEntity(Transform transform, Vector3 startPosition, Quaternion startRotation)
	{
		Position = ObjectPosition.FromTransform(transform);
		OriginalPosition = ObjectPosition.FromPositionRotation(startPosition, startRotation);
	}
}

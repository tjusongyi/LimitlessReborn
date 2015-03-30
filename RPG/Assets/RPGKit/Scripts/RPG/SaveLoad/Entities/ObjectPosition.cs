using UnityEngine;
using System.Collections;

public class ObjectPosition {

	public float PositionX;
	public float PositionY;
	public float PosizionZ;
	
	public float RotationX;
	public float RotationY;
	public float RotazionZ;
	public float RotationW;
	
	
	public static ObjectPosition FromTransform(Transform transform)
	{
		ObjectPosition op = new ObjectPosition();
		
		op.PositionX = transform.position.x;
		op.PositionY = transform.position.y;
		op.PosizionZ = transform.position.z;
		
		op.RotationX = transform.rotation.x;
		op.RotationW = transform.rotation.w;
		op.RotationY = transform.rotation.y;
		op.RotazionZ = transform.rotation.z;
		
		return op;
	}
	
	public static ObjectPosition FromPositionRotation(Vector3 position, Quaternion rotation)
	{
		ObjectPosition op = new ObjectPosition();
		
		op.PositionX = position.x;
		op.PositionY = position.y;
		op.PosizionZ = position.z;
		
		op.RotationX = rotation.x;
		op.RotationW = rotation.w;
		op.RotationY = rotation.y;
		op.RotazionZ = rotation.z;
		
		return op;
	}
	
	public Vector3 GetPosition()
	{
		return new Vector3(PositionX, PositionY, PosizionZ);
	}
	
	public Quaternion GetRotation()
	{
		return new Quaternion(RotationX, RotationY, RotazionZ, RotationW);
	}
}

using UnityEngine;
using System.Collections;

public class BaseGameObject : MonoBehaviour 
{
	protected Transform target;

    [HideInInspector]
	public Vector3 startPosition;

    [HideInInspector]
	public Quaternion startRotation;
	
	void Awake()
	{
		startPosition = transform.position;
		startRotation = transform.rotation;
	}

    public virtual string DisplayInfo(Player player)
    {
        return string.Empty;
    }

    public virtual string DisplayCloseInfo(Player player)
    {
        return string.Empty;
    }

    public virtual void DoAction(Player player)
    { 
    
    }

    public virtual InfoPositionEnum InfoPosition()
    {
        return InfoPositionEnum.MiddleDown;
    }

    public virtual float GetActivateRange(Player player)
    {
        return player.Hero.Settings.ObjectActivateRange;
    }

    
}


public enum InfoPositionEnum
{ 
    MiddleDown,
    RightTop
}
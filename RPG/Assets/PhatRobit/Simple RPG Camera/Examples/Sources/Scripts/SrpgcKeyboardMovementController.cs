using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Animator))]
public class SrpgcKeyboardMovementController : MonoBehaviour
{
	public float speedThreshold = 0.2f;
	public float speedDamp = 0.05f;
	public float directionDamp = 0.05f;

	public string speedFloat = "Speed";
	public string directionFloat = "Direction";
	public string angleFloat = "Angle";

	private Animator _animator;

	private float _direction = 0;
	private float _angle = 0;

	private Vector3 _input = new Vector3();

	private Transform _t;

	private int _locomotionPivotLID = 0;
	private int _locomotionPivotRID = 0;
	private int _locomotionPivotTransLID = 0;
	private int _locomotionPivotTransRID = 0;
	private int _idlePivotLID = 0;
	private int _idlePivotRID = 0;
	private int _idlePivotTransLID = 0;
	private int _idlePivotTransRID = 0;

	private AnimatorStateInfo _stateInfo;
	private AnimatorTransitionInfo _transInfo;

	void Start()
	{
		_t = transform;
		_animator = GetComponent<Animator>();

		_locomotionPivotLID = Animator.StringToHash("Base Layer.Locomotion Pivot Left");
		_locomotionPivotRID = Animator.StringToHash("Base Layer.Locomotion Pivot Right");
		_locomotionPivotTransLID = Animator.StringToHash("Base Layer.Locomotion -> Base Layer.Locomotion Pivot Left");
		_locomotionPivotTransRID = Animator.StringToHash("Base Layer.Locomotion -> Base Layer.Locomotion Pivot Right");
		_idlePivotLID = Animator.StringToHash("Base Layer.Idle Pivot Left");
		_idlePivotRID = Animator.StringToHash("Base Layer.Idle Pivot Right");
		_idlePivotTransLID = Animator.StringToHash("Base Layer.Idle -> Base Layer.Idle Pivot Left");
		_idlePivotTransRID = Animator.StringToHash("Base Layer.Idle -> Base Layer.Idle Pivot Right");
	}

	void Update()
	{
		_stateInfo = _animator.GetCurrentAnimatorStateInfo(0);
		_transInfo = _animator.GetAnimatorTransitionInfo(0);

		_input.x = Input.GetAxis("Horizontal");
		_input.y = Input.GetAxis("Vertical");

		StickToWorldspace();

		float speed = _input.sqrMagnitude;

		_animator.SetFloat(speedFloat, speed, speedDamp, Time.deltaTime);

		if(speed < speedThreshold)
		{
			_direction = 0;
		}

		_animator.SetFloat(directionFloat, _direction, directionDamp, Time.deltaTime);

		if(!IsInPivot())
		{
			if(speed > speedThreshold)
			{
				_animator.SetFloat(angleFloat, _angle);
			}
			else
			{
				_animator.SetFloat(directionFloat, 0);
				_animator.SetFloat(angleFloat, 0);
			}
		}
	}

	private void StickToWorldspace()
	{
		Vector3 rootDirection = _t.forward;

		// Get camera direction
		Vector3 cameraDirection = Camera.main.transform.forward;
		cameraDirection.y = 0;
		Quaternion referentialShift = Quaternion.FromToRotation(Vector3.forward, cameraDirection);

		// Convert joystick input in worldspace coords
		Vector3 moveDirection = referentialShift * new Vector3(_input.x, 0, _input.y);
		Vector3 axisSign = Vector3.Cross(moveDirection, rootDirection);

		float angleRootToMove = Vector3.Angle(rootDirection, moveDirection) * (axisSign.y >= 0 ? -1f : 1f);

		if(!IsInPivot())
		{
			_angle = angleRootToMove;
		}

		angleRootToMove /= 180f;
		_direction = angleRootToMove * 3;
	}

	private bool IsInPivot()
	{
		return _stateInfo.nameHash == _locomotionPivotLID ||
				_stateInfo.nameHash == _locomotionPivotRID ||
				_transInfo.nameHash == _locomotionPivotTransLID ||
				_transInfo.nameHash == _locomotionPivotTransRID ||
				_stateInfo.nameHash == _idlePivotLID ||
				_stateInfo.nameHash == _idlePivotRID ||
				_transInfo.nameHash == _idlePivotTransLID ||
				_transInfo.nameHash == _idlePivotTransRID;
	}
}
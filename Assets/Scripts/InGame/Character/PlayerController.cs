using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour 
{
	#region Variables
	public float SensitivityX = 15F;
	public float SensitivityY = 15F;
	public float JumpForce = 200;
	public float MoveSpeed = 5;
	public float Decelration = 1;
	public Vector2 ClampCamMinMax;

	Rigidbody thisRig;
	Transform mainCam;
	Transform pTrans;

	bool canJump = true;
//	bool canCheckGr = false;
	#endregion

	#region Mono
	void Awake ( )
	{
		thisRig = GetComponent<Rigidbody> ( );
		mainCam = Camera.main.transform;
		pTrans = transform;
	}

	void Update ( )
	{
		Cursor.visible = false;
		Cursor.lockState = CursorLockMode.Locked;

		float getDelta = Time.deltaTime;
		pTrans.Rotate ( 0, Input.GetAxis ( "Mouse X" ) * SensitivityX * getDelta, 0 );
		mainCam.Rotate ( Input.GetAxis ( "Mouse Y" ) * -SensitivityY * getDelta, 0, 0 );
		mainCam.localRotation = ClampRotationAroundXAxis ( mainCam.localRotation );
		Transform transPlayer = pTrans;

		playerMove ( getDelta, transPlayer );
	}
	#endregion

	#region Public Functions
	#endregion

	#region Private Functions
	void playerMove ( float deltaT, Transform transPlayer )
	{
		float getValue;

		getValue = Input.GetAxis ( "Vertical" );
		if ( getValue > 0 )
		{
			transPlayer.Translate ( transPlayer.forward * MoveSpeed * deltaT, Space.World );
		}
		else if ( getValue < 0 )
		{
			transPlayer.Translate ( -transPlayer.forward * MoveSpeed * deltaT, Space.World );
		}

		getValue = Input.GetAxis ( "Horizontal" );

		if ( getValue < 0 )
		{
			transPlayer.Translate (- transPlayer.right * MoveSpeed * deltaT, Space.World );
		}
		else if ( getValue > 0 )
		{
			//thisRig.MovePosition ( transPlayer.localPosition + transPlayer.right * MoveSpeed * deltaT );
			transPlayer.Translate ( transPlayer.right * MoveSpeed * deltaT, Space.World );
		}

		if ( canJump && Input.GetAxis ( "Jump" ) > 0 )
		{
			canJump = false;
			//canCheckGr = false;
			thisRig.AddForce ( transPlayer.up * JumpForce, ForceMode.Impulse );
		}
	}

	void OnCollisionStay ( Collision thisColl )
	{
		if ( thisColl.gameObject.layer == 10 )
		{
			canJump = true;
		}
	}

	Quaternion ClampRotationAroundXAxis ( Quaternion q )
	{
		q.x /= q.w;
		q.y /= q.w;
		q.z /= q.w;
		q.w = 1.0f;

		float angleX = 2.0f * Mathf.Rad2Deg * Mathf.Atan (q.x);

		angleX = Mathf.Clamp ( angleX, ClampCamMinMax.x, ClampCamMinMax.y );

		q.x = Mathf.Tan (0.5f * Mathf.Deg2Rad * angleX);

		return q;
	}
	#endregion
}

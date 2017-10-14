using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour 
{
	#region Variables
	public float JumpForce = 200;
	public float MaxSpeed = 5;
	public float RotationSpeed = 1;
	public float Acceleration = 10;
	public float Deceleration = 1;
	public bool Running = true;

	Rigidbody thisRig;
	Transform pTrans;
	Direction currentDir = Direction.North;
	Direction newDir = Direction.North;
	Vector3 posDir;

	float currSpeed = 0;
	float calPos = 0;
	float newDist;
	bool canJump = true;
	bool newPos = false;
	#endregion

	#region Mono
	void Awake ( )
	{
		thisRig = GetComponent<Rigidbody> ( );
		pTrans = transform;
	}

	void FixedUpdate ( )
	{
		if ( newPos )
		{
			float getCurr = Vector3.Distance ( posDir, pTrans.position );
			if ( newDist != getCurr )
			{
				newDist = getCurr;

				if ( calPos > newDist )
				{
					calPos = newDist;
				}
				else
				{
					currentDir = newDir;
					pTrans.position = new Vector3 ( posDir.x, pTrans.position.y, posDir.z );
					calPos = 0;
					newPos = false;
				}
			}
		}

		float deltTime = Time.deltaTime;

		if ( Running )
		{
			if ( currSpeed < MaxSpeed )
			{
				currSpeed += Acceleration * deltTime;
			}
			else if ( currSpeed > MaxSpeed )
			{
				currSpeed = MaxSpeed;
			}
		}
		else
		{
			currSpeed -= Deceleration * deltTime;

			if ( currSpeed < 0 )
			{
				currSpeed = 0;
			}
		}

		playerMove ( deltTime, currSpeed );
	}
	#endregion

	#region Public Functions
	#endregion

	#region Private Functions
	void playerMove ( float delTime, float speed )
	{
		Transform transPlayer = pTrans;
		delTime = Time.deltaTime;
		if ( currentDir == Direction.North )
		{
			pTrans.Translate ( Vector3.forward * speed * delTime, Space.World );
			transPlayer.rotation = Quaternion.Slerp ( transPlayer.rotation, Quaternion.Euler ( new Vector3 ( 0, 0, 0 ) ), RotationSpeed * delTime );
		}
		else if ( currentDir == Direction.South )
		{
			pTrans.Translate ( Vector3.back  * speed * delTime, Space.World );
			transPlayer.rotation = Quaternion.Slerp ( transPlayer.rotation, Quaternion.Euler ( new Vector3 ( 0, 180, 0 ) ), RotationSpeed * delTime );
		}
		else if ( currentDir == Direction.East )
		{
			pTrans.Translate ( Vector3.right * speed * delTime, Space.World );
			transPlayer.rotation = Quaternion.Slerp ( transPlayer.rotation, Quaternion.Euler ( new Vector3 ( 0, 90, 0 ) ), RotationSpeed * delTime );
		}
		else if ( currentDir == Direction.West )
		{
			pTrans.Translate ( Vector3.left * speed * delTime, Space.World );
			transPlayer.rotation = Quaternion.Slerp ( transPlayer.rotation, Quaternion.Euler ( new Vector3 ( 0, -90, 0 ) ), RotationSpeed * delTime );
		}

		if ( canJump && Input.GetAxis ( "Jump" ) > 0 )
		{
			canJump = false;
			//canCheckGr = false;
			thisRig.AddForce ( transPlayer.up * JumpForce, ForceMode.Impulse );
		}
	}

	void OnTriggerEnter ( Collider thisColl )
	{
		if ( thisColl.tag == "ModifDirect" )
		{
			newPos = true;
			posDir = thisColl.transform.position;
			newDir = thisColl.GetComponent<NewDirect> ( ).NewDirection;
			calPos = Vector3.Distance ( posDir, pTrans.position ) + 0.1f;
		}
	}

	void OnCollisionStay ( Collision thisColl )
	{
		if ( thisColl.gameObject.layer == 9 )
		{
			canJump = true;
		}
	}
	#endregion
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour 
{
	#region Variables
	[Header ("Caract on same Line")]
	public float MaxSpeed = 5;
	public float Acceleration = 10;
	public float Deceleration = 1;

	[Header ("Caract when change Line")]
	public float MaxSpeedCL = 5;
	public float AccelerationCL = 10;
	public float ImpulsionCL = 10;
	public float DecelerationLineCL = 5;
	public float DecelerationCL = 1;

	[Header ("Caract both")]
	public float RotationSpeed = 1;
	public bool Running = true;
	//public float JumpForce = 200;

	[Space]
	[Header("Nombre de ligne bonus d'un seul coté")]
	public int NbrLine = 3;

	[HideInInspector]
	public bool playerDead = false;

	//Rigidbody thisRig;
	Transform pTrans;
	Direction currentDir = Direction.North;
	Direction newDir = Direction.North;
	//Vector3 posDir;
	Vector3 dirLine = Vector3.zero;

	float currSpeed = 0;
	float currSpLine = 0;
	//float calPos = 0;
	float newH = 0;
	float newDist;
	float saveDist;
	float befRot = 0;
	int currLine = 0;

	int distLine = 6;
	int LastImp = 0;
	int clDir = 0;

	//bool canJump = true;
	bool newPos = false;

	#endregion

	#region Mono
	void Awake ( )
	{
		//thisRig = GetComponent<Rigidbody> ( );
		pTrans = transform;
	}

	void FixedUpdate ( )
	{
		if ( playerDead )
		{
			return;
		}

		/*if ( newPos )
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
		}*/

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

			currSpLine -= DecelerationCL * deltTime;

			if ( currSpLine < 0 )
			{
				currSpLine = 0;
			}
		}

		changeLine ( deltTime );
		playerMove ( deltTime, currSpeed );
	}
	#endregion

	#region Public Functions
	#endregion

	#region Private Functions
	void playerMove ( float delTime, float speed )
	{
		Transform transPlayer = pTrans;
		Vector3 calTrans = Vector3.zero;
		delTime = Time.deltaTime;

		if ( currentDir == Direction.North )
		{
			calTrans = Vector3.forward * speed * delTime;
			transPlayer.rotation = Quaternion.Slerp ( transPlayer.rotation, Quaternion.Euler ( new Vector3 ( 0, 0, 0 ) ), RotationSpeed * delTime );
		}
		else if ( currentDir == Direction.South )
		{
			calTrans = Vector3.back  * speed * delTime;
			transPlayer.rotation = Quaternion.Slerp ( transPlayer.rotation, Quaternion.Euler ( new Vector3 ( 0, 180, 0 ) ), RotationSpeed * delTime );
		}
		else if ( currentDir == Direction.East )
		{
			calTrans = Vector3.right * speed * delTime;
			transPlayer.rotation = Quaternion.Slerp ( transPlayer.rotation, Quaternion.Euler ( new Vector3 ( 0, 90, 0 ) ), RotationSpeed * delTime );
		}
		else if ( currentDir == Direction.West )
		{
			calTrans = Vector3.left * speed * delTime;
			transPlayer.rotation = Quaternion.Slerp ( transPlayer.rotation, Quaternion.Euler ( new Vector3 ( 0, -90, 0 ) ), RotationSpeed * delTime );
		}

		if ( newPos )
		{
			befRot -= calTrans.magnitude;

			if ( befRot < 0 )
			{
				newPos = false;
				currentDir = newDir;
				pTrans.Translate ( pTrans.forward * befRot * delTime, Space.World );
			}
		}

		pTrans.Translate ( calTrans, Space.World );
			
		/*if ( canJump && Input.GetAxis ( "Jump" ) > 0 )
		{
			canJump = false;
			thisRig.AddForce ( transPlayer.up * JumpForce, ForceMode.Impulse );
		}*/
	}

	void changeLine ( float delTime )
	{
		float newImp = Input.GetAxis ( "Horizontal" );

		if ( newImp == 1 && LastImp != 1 && currLine + 1 <= NbrLine)
		{
			currLine++;
			LastImp = 1;
			clDir = 1;
			newH = newH + distLine;
			saveDist = newH;
		}
		else if ( newImp == -1 && LastImp != -1 && currLine - 1 >= -NbrLine )
		{
			currLine--;
			LastImp = -1;
			clDir = -1;
			newH = newH - distLine;
			saveDist = newH;
		}
		else if ( newImp == 0 )
		{
			LastImp = 0;
		}

		if ( newH != 0 )
		{
			if ( Running )
			{
				float accLine = 0;

				if ( saveDist < 0 && newH > saveDist / 2 || saveDist > 0 && newH < saveDist / 2 )
				{
					currSpLine -= DecelerationLineCL * delTime;
				}
				else if ( currSpLine < MaxSpeedCL )
				{
					accLine = ( currSpLine * ImpulsionCL )/ MaxSpeedCL; 

					if ( accLine > 1 || accLine == 0 )
					{
						accLine = 1;
					}

					currSpLine += AccelerationCL * accLine * delTime;
				}
				else if ( currSpLine > MaxSpeedCL )
				{
					currSpLine = MaxSpeedCL;
				}
			}

			float calTrans = clDir * currSpLine * delTime;
			newH -= calTrans;

			if ( saveDist > 0 && newH - calTrans < 0 || saveDist < 0 && newH > 0 )
			{
				calTrans += newH;
				newH = 0;
			}
			dirLine = pTrans.right * calTrans;
			pTrans.Translate ( dirLine, Space.World );
		}
		else
		{
			currSpLine = 0;
		}
	}

	void OnTriggerEnter ( Collider thisColl )
	{
		if ( thisColl.tag == Constants._NewDirec )
		{
			//posDir = thisColl.transform.position;
			//befRot = thisColl.GetComponent<BoxCollider> ( ).size.z / 2 + pTrans.GetComponent<BoxCollider> ( ).size.z ;

			newPos = true;
			newDir = thisColl.GetComponent<NewDirect> ( ).NewDirection;
			befRot = Vector3.Distance ( thisColl.transform.position, pTrans.position );
		} 
	}

	void OnCollisionEnter ( Collision thisColl )
	{
		if ( thisColl.gameObject.tag == Constants._EnnemisTag )
		{
			playerDead = true;
			GlobalManager.Ui.DisplayOver ( true );

            GlobalManager.GameCont.Restart();

		}
	}


	/*void OnCollisionStay ( Collision thisColl )
	{
		if ( thisColl.gameObject.layer == 9 )
		{
			canJump = true;
		}
	}*/
	#endregion
}

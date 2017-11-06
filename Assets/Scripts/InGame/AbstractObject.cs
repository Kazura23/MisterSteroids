using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbstractObject : MonoBehaviour 
{
	#region Variables
	[Space]
	[HideInInspector]
	public bool isDead;
	public float delayDead = 2;

    [Header ("Contact avec obs")]
	[Tooltip ("pourcentage de velocité restante en pourcentage lors d'une collision avec un ennmis ( situation ou ce gameobject est en mouvement )")]
	public float VelRestant = 5;

	[Tooltip ("force de direction lorsque en collision contre un Object / ennemis ( situation ou ce gameobject est immobile )")]
	public float onObjForward;

	[Space]
	[Header ("Contrainte axe / rotation ")]
	[Tooltip ("Si différent de 0 alors l'axe est freeze")]
	public Vector3 FreezeAxe = Vector3.zero;

	[Tooltip ("Si différent de 0 alors l'axe de rotation est freeze")]
	public Vector3 FreezeRot = Vector3.zero;

	public bool useGravity = true;

	protected Rigidbody mainCorps;
	protected Transform getTrans;

	List<Rigidbody> corps;
	Vector3 projection;
	#endregion

	#region Mono
	void Awake () 
	{
		isDead = false;
		corps = new List<Rigidbody>();

		getTrans = transform;

		mainCorps = getTrans.GetComponent<Rigidbody> ( );

		foreach ( Rigidbody thisRig in getTrans.GetComponentsInChildren<Rigidbody> ( ) )
		{
			corps.Add ( thisRig );
		}
	}
	#endregion

	#region Public Methods
	public virtual void Degat(Vector3 p_damage, int p_technic)
	{
		if ( !isDead )
		{
			projection = p_damage;
			Dead ( );
		}
	}

	public virtual void Dead ( bool enemy = false )
	{
		isDead = true;

		//StartCoroutine ( disableColl ( ) );
		getTrans.tag = Constants._ObjDeadTag;
		for ( int i = 0; i < corps.Count; i++ )
		{
			corps [ i ].useGravity = true;
		}

		mainCorps.constraints = RigidbodyConstraints.None;
		checkConstAxe ( );
		if ( useGravity )
		{
			mainCorps.useGravity = true;
		}

		//checkConstAxe ( );

		if ( enemy )
		{
			mainCorps.AddForce ( getTrans.forward * onObjForward, ForceMode.VelocityChange );
		}
		else
		{
			Vector3 getFor = getTrans.forward * projection.z;
			Vector3 getRig = getTrans.right * projection.x;
			Vector3 getUp = transform.up * projection.y;
			mainCorps.AddForce ( getFor + getRig + getUp, ForceMode.VelocityChange );
		}

		Destroy ( this.gameObject, delayDead );
	}

	protected virtual void CollDetect (  )
	{
		if ( !isDead )
		{
			Dead ( true );
		}
		else
		{
			mainCorps.velocity = mainCorps.velocity * ( VelRestant / 100 );
		}
	}

	public virtual void ForceProp ( Vector3 forceProp )
	{
		isDead = true;

		getTrans.tag = Constants._ObjDeadTag;
		for ( int i = 0; i < corps.Count; i++ )
		{
			corps [ i ].useGravity = true;
		}

		mainCorps.constraints = RigidbodyConstraints.None;
		checkConstAxe ( );
		if ( useGravity )
		{
			mainCorps.useGravity = true;
		}

		mainCorps.AddForce ( forceProp, ForceMode.VelocityChange );
		StartCoroutine ( enableColl ( ) );
	}
	#endregion

	#region Private Methods
	protected virtual void OnCollisionEnter ( Collision thisColl )
	{
		GameObject getThis = thisColl.gameObject;

		if ( getThis.tag == Constants._EnnemisTag || getThis.tag == Constants._ObjDeadTag || getThis.tag == Constants._ObsTag )
		{
			CollDetect ( );
		}

		/*else if ( getThis.tag == Constants._PlayerTag && gameObject.tag == Constants._ObjDeadTag )
		{
			Physics.IgnoreCollision ( thisColl.collider, GetComponent<Collider> ( ) );
		}*/
	}

	IEnumerator enableColl ( )
	{
		WaitForEndOfFrame thisF = new WaitForEndOfFrame ( );
		Transform savePos = transform;
		Transform playPos = GlobalManager.GameCont.Player.transform;

		while ( Vector3.Distance ( savePos.position, playPos.position ) < 1f )
		{
			yield return thisF;
		}

		GetComponent<BoxCollider> ( ).enabled = true;
	}


	void checkConstAxe ( )
	{
		if ( FreezeAxe.x != 0 )
		{
			mainCorps.constraints = RigidbodyConstraints.FreezePositionX;
		}

		if ( FreezeAxe.y != 0 )
		{
			mainCorps.constraints = RigidbodyConstraints.FreezePositionY;
		}

		if ( FreezeAxe.z != 0 )
		{
			mainCorps.constraints = RigidbodyConstraints.FreezePositionZ;
		}

		if ( FreezeRot.x != 0 )
		{
			mainCorps.constraints = RigidbodyConstraints.FreezeRotationX;
		}

		if ( FreezeRot.y != 0 )
		{
			mainCorps.constraints = RigidbodyConstraints.FreezeRotationY;
		}

		if ( FreezeRot.z != 0 )
		{
			mainCorps.constraints = RigidbodyConstraints.FreezeRotationZ;
		}
	}
		
	IEnumerator disableColl ( )
	{
		WaitForSeconds thisSec = new WaitForSeconds ( 0.0f );

		yield return thisSec;

		getTrans.tag = Constants._ObjDeadTag;
	}

	public virtual void PlayerDetected ( GameObject thisObj, bool isDetected )
	{
		
	}
	#endregion
}

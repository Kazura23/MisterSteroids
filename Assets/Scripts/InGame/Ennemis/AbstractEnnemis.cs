using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbstractEnnemis : MonoBehaviour 
{
	#region Variables
	[HideInInspector]
	public bool isDead;

	public float delayDead = 2;

	[Tooltip ("force de direction lorsque en collision contre un ennemis ( situation ou ce gameobject est immobile )")]
	public Vector3 onEnemy;

	[Tooltip ("pourcentage de velocité restante en pourcentage lors d'une collision avec un ennmis ( situation ou ce gameobject est en mouvement )")]
	public float VelRestant = 5;

	protected Rigidbody mainCorps;
	protected Transform parentTrans;

	List<Rigidbody> corps;
	Vector3 projection;
	Collider currColl;


	#endregion

	#region Mono
	void Awake () 
	{
		parentTrans = transform.parent;

		isDead = false;
		corps = new List<Rigidbody>();
		mainCorps = parentTrans.GetComponent<Rigidbody> ( );
		currColl = parentTrans.GetComponent<Collider> ( );

		foreach ( Rigidbody thisRig in parentTrans.GetComponentsInChildren<Rigidbody> ( ) )
		{
			corps.Add ( thisRig );
		}
	}

	void Update () 
	{
		if ( isDead )
		{
			Dead ( );
		}
	}
	#endregion

	#region Public Methods
	public void Degat(Vector3 p_damage)
	{
		if ( !isDead )
		{
			isDead = true;
			projection = p_damage;
		}
	}

	public virtual void Dead ( bool enemy = false )
	{
		StartCoroutine ( disableColl ( ) );
		for ( int i = 0; i < corps.Count; i++ )
		{
			corps [ i ].useGravity = true;
		}

		mainCorps.constraints = RigidbodyConstraints.None;
		mainCorps.constraints = RigidbodyConstraints.FreezePositionX;
		mainCorps.constraints = RigidbodyConstraints.FreezePositionY;

		mainCorps.useGravity = true;

		if ( enemy )
		{
			mainCorps.AddForce ( onEnemy, ForceMode.VelocityChange );
		}
		else
		{
			mainCorps.AddForce ( projection, ForceMode.VelocityChange );
		}

		Destroy ( this.gameObject, delayDead );
	}

	public void CollDetect (  )
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
	#endregion

	#region Private Methods
	protected void debrisDetected ( Collider thisColl )
	{
		Physics.IgnoreCollision ( thisColl, currColl );
	}

	IEnumerator disableColl ( )
	{
		WaitForSeconds thisSec = new WaitForSeconds ( 0.5f );

		yield return thisSec;

		parentTrans.tag = Constants._UnTagg;
	}

	protected virtual void playerDetected ( )
	{
		
	}

	protected virtual void playerUndetected ( )
	{

	}
	#endregion
}

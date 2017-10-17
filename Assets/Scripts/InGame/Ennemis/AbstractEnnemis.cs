using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbstractEnnemis : MonoBehaviour 
{
	#region Variables
	public float delayDead = 2;

	[Tooltip ("Pourcentage de réduction lorsque l'enemy encontre en collision avec un autre enemy")]
	public float velRecuced = 5;

	protected Rigidbody mainCorps;
	protected Transform parentTrans;

	List<Rigidbody> corps;
	Vector3 projection;

	bool isDead;
	#endregion

	#region Mono
	void Awake () 
	{
		parentTrans = transform.parent;

		isDead = false;
		corps = new List<Rigidbody>();
		mainCorps = parentTrans.GetComponent<Rigidbody> ();

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
		isDead = true;
		projection = p_damage;
	}

	public virtual void Dead ( bool enemy = false )
	{
		for ( int i = 0; i < corps.Count; i++ )
		{
			corps [ i ].useGravity = true;
		}

		//mainCorps.constraints = RigidbodyConstraints.None;
		mainCorps.useGravity = true;
		if ( enemy )
		{
			mainCorps.AddForce ( projection / velRecuced, ForceMode.VelocityChange );
		}
		else
		{
			mainCorps.AddForce ( projection, ForceMode.VelocityChange );
		}

		Destroy ( this.gameObject, delayDead );
	}
	#endregion

	#region Private Methods
	void OnCollisionEnter ( Collision thisColl)
	{
		if ( thisColl.gameObject.tag == Constants._EnnemisTag )
		{
			if ( !isDead )
			{
				Dead ( true );
			}
			else
			{
				mainCorps.velocity = mainCorps.velocity / velRecuced;
			}
		}
	}

	protected virtual void playerDetected ( )
	{
		
	}

	protected virtual void playerUndetected ( )
	{

	}
	#endregion
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbstractEnnemis : MonoBehaviour 
{
	#region Variables
	public float delayDead = 2;

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
			StartCoroutine("Dead");
		}
	}
	#endregion

	#region Public Methods
	public void Degat(Vector3 p_damage)
	{
		isDead = true;
		projection = p_damage;
	}

	public virtual void Dead ( )
	{
		for ( int i = 0; i < corps.Count; i++ )
		{
			corps [ i ].useGravity = true;
		}

		mainCorps.constraints = RigidbodyConstraints.None;
		mainCorps.useGravity = true;
		mainCorps.AddForce ( projection, ForceMode.Impulse );

		Destroy ( this.gameObject, delayDead );
	}
	#endregion

	#region Private Methods
	protected virtual void playerDetected ( )
	{
		
	}

	protected virtual void playerUndetected ( )
	{

	}
	#endregion
}

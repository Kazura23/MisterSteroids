using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

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
    [Tooltip ("Slow Motion Approche")]
    float distSlowMotio = 15;
    float ratioSlow = 0.25f;
    float timeSlow = 1f;

	[Space]
	[Header ("Contrainte axe / rotation ")]
	[Tooltip ("Si différent de 0 alors l'axe est freeze")]
	public Vector3 FreezeAxe = Vector3.zero;

	[Tooltip ("Si différent de 0 alors l'axe de rotation est freeze")]
	public Vector3 FreezeRot = Vector3.zero;

	public bool useGravity = true;

	protected Rigidbody mainCorps;
	protected Transform getTrans;
    protected Transform playerTrans;
    protected bool activeSlow = true;

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

    protected virtual void Start()
    {
        playerTrans = GlobalManager.GameCont.Player.transform;
    }

    protected virtual void Update()
    {
        if (Vector3.Distance(transform.position, playerTrans.position) <= distSlowMotio && activeSlow && !isDead && tag == Constants._EnnemisTag)
        {
            activeSlow = false;
            Debug.Log("Here");
            Time.timeScale = ratioSlow;
            StartCoroutine("delaySlowMotio");
            //DOTween.To(() => Time.timeScale, x => Time.timeScale = x, ratioSlow, timeSlow);
            //if(Time.timeScale <= ratioSlow)
                
        }/*else if(Time.timeScale < 1 && Vector3.Distance(transform.position, playerTrans.position) > distSlowMotio && !activeSlow)
        {
            Time.timeScale = 1;
            activeSlow = true;
        }*/
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
        Time.timeScale = 1;
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
        Debug.Log("Time = " + Time.timeScale);
        
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
			Physics.IgnoreCollision ( thisColl.collider, GetComponent<Collider> ( ) );

			if ( getThis.tag == Constants._EnnemisTag || getThis.tag == Constants._ObjDeadTag )
			{
				Debug.Log ( "ennemis touche" );
			}
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

		Physics.IgnoreCollision ( playPos.GetComponent<Collider> ( ), GetComponent<Collider> ( ) );

		yield return thisF;

		GetComponent<BoxCollider> ( ).enabled = true;
	}

    IEnumerator delaySlowMotio()
    {
        yield return new WaitForSeconds(timeSlow);
        if(Time.timeScale < 1)
        {
            Time.timeScale = 1;
        }
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

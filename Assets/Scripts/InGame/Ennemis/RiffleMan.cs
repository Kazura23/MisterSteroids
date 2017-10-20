using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RiffleMan : AbstractObject 
{
	#region Variables
	public GameObject ball;
	public int NbrBalls = 20;
	public int ForceBall; 
	public float angle = 1;
	public int angleY = 3;
	public float SpeedSpawn = 0.2f;
	public float TimeDestr = 0.4f;

	Transform player;
	Transform localShoot;
	float timer;
	#endregion

	#region Mono
	void Start () 
	{
		timer = 0;
		player = GlobalManager.GameCont.Player.transform;
		localShoot = getTrans.Find ( "SpawnShoot" );
	}
	#endregion

	#region Public Methods
	public override void PlayerDetected ( GameObject thisObj, bool isDetected )
	{
		base.PlayerDetected ( thisObj, isDetected );

		if ( isDetected && !isDead )
		{
			StartCoroutine ( shootPlayer ( new WaitForSeconds ( SpeedSpawn ), false ) );
		}
	}
	#endregion

	#region Private Methods
	IEnumerator shootPlayer ( WaitForSeconds thisF, bool checkDir )
	{
		timer = Time.timeSinceLevelLoad;

		int a;
		GameObject getCurr;
		for ( a = 0; a < NbrBalls; a++ )
		{
			yield return thisF;

			if ( isDead )
			{
				break;
			}

			getCurr = ( GameObject ) Instantiate ( ball, localShoot );
			getCurr.transform.localPosition = new Vector3 ( 0, 0, 0 );

			if ( checkDir )
			{
				getCurr.GetComponent<Rigidbody> ( ).AddForce ( new Vector3 ( -NbrBalls / 2 + a * angle, Random.Range ( -angleY, angleY + 1 ), ForceBall ), ForceMode.VelocityChange );
			}
			else
			{
				getCurr.GetComponent<Rigidbody> ( ).AddForce ( new Vector3 ( NbrBalls / 2 - a * angle, Random.Range ( -angleY, angleY + 1 ), ForceBall ), ForceMode.VelocityChange );
			}

			Destroy ( getCurr, TimeDestr );
		}

		if ( !isDead )
		{
			StartCoroutine ( shootPlayer ( thisF, !checkDir ) );
		}
	}

	public override void Dead ( bool enemy = false ) 
	{
		base.Dead ( enemy );

		//mainCorps.GetComponent<BoxCollider> ( ).enabled = false;
	}
	#endregion
}

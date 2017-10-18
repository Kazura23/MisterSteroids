using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProtoEnnemis : AbstractObject
{
	#region Variables
	[Space]
	public Color NewColor;
	Color saveCol;


	Material parMat;
	#endregion

	#region Mono
	void Start ( )
	{
		parMat = getTrans.GetComponent<MeshRenderer> ( ).material;
		saveCol = parMat.color;
	}
	#endregion

	#region Public Methods
	public override void PlayerDetected ( GameObject thisObj, bool isDetected )
	{
		base.PlayerDetected ( thisObj, isDetected );

		if ( isDetected )
		{
			parMat.color = NewColor;
		}
		else
		{
			parMat.color = saveCol;
		}
		parMat.color = NewColor;
	}

	public override void Dead ( bool enemy = false ) 
	{
		base.Dead ( enemy );

		//mainCorps.GetComponent<BoxCollider> ( ).enabled = false;
	}
	#endregion

	#region Private Methods
	void OnCollisionEnter ( Collision thisColl )
	{
		GameObject getThis = thisColl.gameObject;

		if ( getThis.tag == Constants._EnnemisTag || getThis.tag == Constants._ObjDeadTag || getThis.tag == Constants._ObsTag )
		{
			CollDetect ( );
		}
		else if ( getThis.tag == Constants._PlayerTag && gameObject.tag == Constants._ObjDeadTag )
		{
			Physics.IgnoreCollision ( thisColl.collider, GetComponent<Collider> ( ) );
		}
	}
	#endregion
}

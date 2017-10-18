using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProtoObs : AbstractObject 
{
	#region Variables
	#endregion

	#region Mono

	#endregion

	#region Public Methods
	public override void Dead ( bool enemy = false ) 
	{
		base.Dead ( enemy );
	}

	public override void PlayerDetected ( GameObject thisObj, bool isDetected )
	{
		base.PlayerDetected ( thisObj, isDetected );
	}
	#endregion

	#region Private Methods
	void OnCollisionEnter ( Collision thisColl )
	{
		if ( thisColl.gameObject.tag == Constants._EnnemisTag )
		{
			CollDetect ( );
		}
	}
	#endregion
}

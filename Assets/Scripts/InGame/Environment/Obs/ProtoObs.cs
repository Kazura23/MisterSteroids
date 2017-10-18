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
	#endregion

	#region Private Methods
	void OnCollisionEnter ( Collision thisColl )
	{
		if ( thisColl.gameObject.tag == Constants._EnnemisTag )
		{
			CollDetect ( );
		}
	}

	public override void playerDetected ( bool isDetected )
	{
		base.playerDetected ( isDetected );
	}
	#endregion
}

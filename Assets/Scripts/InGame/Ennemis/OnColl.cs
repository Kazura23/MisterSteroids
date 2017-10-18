using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnColl : MonoBehaviour 
{
	#region Variables
	AbstractEnnemis getCurr;
	#endregion

	#region Mono
	void Start ( )
	{
		getCurr = GetComponentInChildren<AbstractEnnemis> ( );
	}
	#endregion

	#region Public Methods
	#endregion

	#region Private Methods
	void OnCollisionEnter ( Collision thisColl )
	{
		if ( getCurr == null )
		{
			return;
		}

		if ( thisColl.gameObject.tag == Constants._EnnemisTag )
		{
			getCurr.CollDetect ( );
		}
		else if ( thisColl.gameObject.tag == Constants._PlayerTag && gameObject.tag != Constants._EnnemisTag )
		{
			Physics.IgnoreCollision ( thisColl.collider, GetComponent<Collider> ( ) );
		}
	}
	#endregion
}

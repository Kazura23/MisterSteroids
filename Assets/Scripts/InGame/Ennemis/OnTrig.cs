using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnTrig : MonoBehaviour 
{
	public AbstractObject GetAbs;

	void OnTriggerEnter ( Collider thisColl )
	{
		if ( thisColl.tag == Constants._PlayerTag )
		{
			if ( GetAbs.gameObject.gameObject.tag != Constants._UnTagg )
			{
				GetAbs.PlayerDetected ( thisColl.gameObject, true );
			}
		}
		/*else if ( thisColl.tag == Constants._DebrisEnv )
		{
			GetAbs.debrisDetected ( thisColl.gameObject.GetComponent<Collider> ( ) );
		}*/
	}

	void OnTriggerExit ( Collider thisColl )
	{
		if ( thisColl.tag == Constants._PlayerTag )
		{
			GetAbs.PlayerDetected ( thisColl.gameObject, false );
		}
	}
}

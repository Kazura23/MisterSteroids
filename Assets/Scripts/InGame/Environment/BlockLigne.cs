using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockLigne : MonoBehaviour 
{

	#region Variables
	#endregion

	#region Mono
	#endregion

	#region Public Methods
	#endregion

	#region Private Methods
	void OnTriggerEnter ( Collider thisColl )
	{
		if ( thisColl.tag == Constants._PlayerTag )
		{
			thisColl.GetComponent<PlayerController> ( ).blockChangeLine = true;
		} 
	}
	#endregion
}

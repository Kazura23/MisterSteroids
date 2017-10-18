using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class canBeDest : MonoBehaviour 
{	
	public float ForcePropulse = 5;
	public float DelayDestruc = 1;

	void OnCollisionEnter ( Collision collision )
	{
		if ( collision.collider.tag == Constants._PlayerTag )
		{
			StartCoroutine ( GlobalManager.GameCont.MeshDest.SplitMesh ( gameObject, ForcePropulse, DelayDestruc ) );
		}
	}
}

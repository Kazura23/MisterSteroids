using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class canBeDest : MonoBehaviour 
{	
	void OnCollisionEnter ( Collision collision ) 
	{
		if ( collision.collider.tag == Constants._PlayerTag )
		{
			StartCoroutine ( Manager.GameCont.MeshDest.SplitMesh ( collision.gameObject.transform.position, gameObject ) );
		}
	}
}

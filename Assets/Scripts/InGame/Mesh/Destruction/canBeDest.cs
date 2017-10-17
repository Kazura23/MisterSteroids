using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class canBeDest : MonoBehaviour 
{	
	void OnCollisionEnter ( Collision collision ) 
	{
		if ( collision.collider.tag == "Player" )
		{
			StartCoroutine ( Manager.GameCont.MeshDest.SplitMesh ( collision.gameObject.transform.position, gameObject ) );
		}
	}
}

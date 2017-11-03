using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeToDisable : MonoBehaviour 
{
	public void DisableThis ( float time )
	{
		Invoke ( "disable", time );
		Invoke ( "addColl", 0.2f );
	}

	void addColl () 
	{
		gameObject.AddComponent<BoxCollider> ( );
	}
		
	void disable ( )
	{
		Destroy ( gameObject );
		//gameObject.SetActive ( false );
		//GlobalManager.GameCont.MeshDest.ReAddObj ( gameObject );
	}
}

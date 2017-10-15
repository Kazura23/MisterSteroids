using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeToDisable : MonoBehaviour 
{
	public void DisableThis ( float time )
	{
		Invoke ( "disable", time );
	}

	void disable ( )
	{
		gameObject.SetActive ( false );
		Manager.GameCont.MeshDest.ReAddObj ( gameObject );
	}
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProtoEnnemis : AbstractEnnemis
{
	#region Variables
	public Color NewColor;
	Color saveCol;


	Material parMat;
	#endregion

	#region Mono
	void Start ( )
	{
		parMat = parentTrans.GetComponent<MeshRenderer> ( ).material;
		saveCol = parMat.color;
	}
	#endregion

	#region Public Methods
	#endregion

	#region Private Methods
	void OnTriggerEnter ( Collider thisColl )
	{
		if ( thisColl.tag == Constants._PlayerTag )
		{
			if ( parentTrans.tag != Constants._UnTagg)
			{
				playerDetected ( );
			}
		}
		else if ( thisColl.tag == Constants._DebrisEnv )
		{
			debrisDetected ( thisColl.gameObject.GetComponent<Collider> ( ) );
		}
	}

	void OnTriggerExit ( Collider thisColl )
	{
		if ( thisColl.tag == Constants._PlayerTag )
		{
			playerUndetected ( );
		}
	}

	public override void Dead ( bool enemy = false ) 
	{
		base.Dead ( enemy );

		//mainCorps.GetComponent<BoxCollider> ( ).enabled = false;
	}

	protected override void playerDetected ( )
	{
		base.playerDetected ( );

		parMat.color = NewColor;
	}

	protected override void playerUndetected ( )
	{
		base.playerUndetected ( );

		parMat.color = saveCol;
	}
	#endregion
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProtoEnnemis : AbstractEnnemis
{
	#region Variables
	public Color NewColor;
	Color saveCol;

	Transform parentTrans;
	Material parMat;
	#endregion

	#region Mono
	void Start ( )
	{
		parentTrans = transform.parent;
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
			playerDetected ( );
		}
	}

	void OnTriggerExit ( Collider thisColl )
	{
		if ( thisColl.tag == Constants._PlayerTag )
		{
			playerUndetected ( );
		}
	}

	protected override void playerDetected ( )
	{
		base.playerDetected ( );

		parMat.color = NewColor;
	}

	protected override void playerUndetected ( )
	{
		base.playerUndetected ( );

		saveCol = saveCol;
	}
	#endregion
}

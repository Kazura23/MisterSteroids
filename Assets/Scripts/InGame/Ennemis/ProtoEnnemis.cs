using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProtoEnnemis : AbstractObject
{
    #region Variables

    [Space]
	public Color NewColor;
	Color saveCol;


	Material parMat;


	#endregion

	#region Mono
	void Start ( )
	{
		parMat = getTrans.GetComponent<MeshRenderer> ( ).material;
		saveCol = parMat.color;

	}
	#endregion

    

	#region Public Methods
	public override void PlayerDetected ( GameObject thisObj, bool isDetected )
	{
		base.PlayerDetected ( thisObj, isDetected );

		if ( isDetected )
		{
			parMat.color = NewColor;
		}
		else
		{
			parMat.color = saveCol;
		}
		parMat.color = NewColor;
	}

	public override void Dead ( bool enemy = false ) 
	{

		base.Dead ( enemy );
		GlobalManager.Ui.BloodHit();
        //mainCorps.GetComponent<BoxCollider> ( ).enabled = false;
    }
	#endregion

	#region Private Methods
	protected override void OnCollisionEnter ( Collision thisColl )
	{
		if ( thisColl.gameObject.tag == Constants._PlayerTag && thisColl.gameObject.GetComponent<PlayerController> ( ).Dash )
		{

            GlobalManager.GameCont.FxInstanciate(new Vector3(transform.position.x, transform.position.y + .5f, transform.position.z), "EnemyNormalDeath", transform.parent);
            CollDetect ( );
		}
		else
		{

            GlobalManager.GameCont.FxInstanciate(new Vector3(transform.position.x, transform.position.y + .5f, transform.position.z), "EnemyNormalDeath", transform.parent);
            base.OnCollisionEnter ( thisColl );
		}
	}
	#endregion
}

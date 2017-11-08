﻿using System.Collections;
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

		if ( isDetected && !isDead)
		{
			parMat.color = NewColor;

            Vector3 playerPos = GlobalManager.GameCont.Player.transform.position;
            GlobalManager.GameCont.FxInstanciate(new Vector3(playerPos.x, playerPos.y + .5f, playerPos.z), "PlayerReady", GlobalManager.GameCont.Player.transform);
        }
		else
		{
			parMat.color = saveCol;
		}
		parMat.color = NewColor;

        Debug.Log("Fx");


        try {
			GetComponentInChildren<Animator>().SetTrigger("Attack");
		}
		catch{
		}
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
        base.OnCollisionEnter ( thisColl );

		if ( isDead )
		{
			GlobalManager.GameCont.FxInstanciate(new Vector3(transform.position.x, transform.position.y, transform.localPosition.z + 5f), "EnemyNormalDeath", transform.parent);
		}
	}

	protected override void CollDetect ( )
	{
		base.CollDetect ( );
		GlobalManager.GameCont.FxInstanciate(new Vector3(transform.position.x, transform.position.y + .5f, transform.position.z), "EnemyNormalDeath", transform.parent);
	}
	#endregion
}

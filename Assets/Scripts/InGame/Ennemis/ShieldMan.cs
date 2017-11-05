using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class ShieldMan : AbstractObject {

    private bool shieldActive;
    public float delay = 1;
    public float distance = 30, hauteur = 2;
    private Vector3 move;
    private float saveVal;

    #region Variables
    public Color NewColor;
    Color saveCol;

    Material parMat;
    #endregion

    #region Mono
    void Start()
    {
        shieldActive = true;
        move = new Vector3();
		parMat = getTrans.GetComponent<MeshRenderer>().material;
        saveCol = parMat.color;
    }
    #endregion

    #region Public Methods
    #endregion

    #region Private Methods 
	protected override void OnCollisionEnter ( Collision thisColl )
	{
		base.OnCollisionEnter ( thisColl );
		if ( isDead )
		{
			GlobalManager.GameCont.FxInstanciate(new Vector3(transform.position.x, transform.position.y + .5f, transform.position.z), "EnemyNormalDeath", transform.parent);
		}
	}
    #endregion
	public override void Dead(bool enemy = false)
	{
		base.Dead(enemy);

		//mainCorps.GetComponent<BoxCollider> ( ).enabled = false;
	}

	public override void ForceProp ( Vector3 forceProp )
	{
		if ( shieldActive )
		{
			GlobalManager.GameCont.Player.GetComponent<PlayerController> ( ).GameOver ( );
		}
		else
		{
			base.ForceProp ( forceProp );
		}
	}

	public override void PlayerDetected( GameObject thisObj, bool isDetected )
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
	}

    public override void Degat(Vector3 p_damage, int p_technic)
    {
        if (p_technic == 1)
        {
            if (shieldActive)
            {
                shieldActive = false;
				move = getTrans.position + (getTrans.forward * distance);
				getTrans.DOMoveX(move.x, delay);
				getTrans.DOMoveZ(move.z, delay);
				getTrans.DOMoveY((saveVal = getTrans.position.y) + hauteur, delay / 2).OnComplete<Tweener>(() => getTrans.DOMoveY(saveVal, delay / 2));
                //animation shield destroy
            }
            else
            {
                base.Degat(p_damage, p_technic);
            }
        }else
        {
            if (shieldActive)
            {
                Debug.Log("Fracas bouclier");
                // animation ou son sur le bouclier
            }
            else
            {
                base.Degat(p_damage, p_technic);
            }
        }
    }

	protected override void CollDetect ( )
	{
		base.CollDetect ( );
		GlobalManager.GameCont.FxInstanciate(new Vector3(transform.position.x, transform.position.y + .5f, transform.position.z), "EnemyNormalDeath", transform.parent);
	}
}

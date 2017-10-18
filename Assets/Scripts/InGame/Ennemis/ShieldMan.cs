using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class ShieldMan : AbstractEnnemis {

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
        parMat = parentTrans.GetComponent<MeshRenderer>().material;
        saveCol = parMat.color;
    }
    #endregion

    #region Public Methods
    #endregion

    #region Private Methods
    void OnTriggerEnter(Collider thisColl)
    {
        if (thisColl.tag == Constants._PlayerTag)
        {
            if (parentTrans.tag != Constants._UnTagg)
            {
                playerDetected();
            }
        }
        else if (thisColl.tag == Constants._DebrisEnv)
        {
            debrisDetected(thisColl.gameObject.GetComponent<Collider>());
        }
    }

    void OnTriggerExit(Collider thisColl)
    {
        if (thisColl.tag == Constants._PlayerTag)
        {
            playerUndetected();
        }
    }

    public override void Dead(bool enemy = false)
    {
        base.Dead(enemy);

        //mainCorps.GetComponent<BoxCollider> ( ).enabled = false;
    }

    protected override void playerDetected()
    {
        base.playerDetected();

        parMat.color = NewColor;
    }

    protected override void playerUndetected()
    {
        base.playerUndetected();

        parMat.color = saveCol;
    }
    #endregion

    public override void Degat(Vector3 p_damage, int p_technic)
    {
        if (p_technic == 1)
        {
            if (shieldActive)
            {
                shieldActive = false;
                move = transform.parent.position + (parentTrans.forward * distance);
                parentTrans.DOMoveX(move.x, delay);
                parentTrans.DOMoveZ(move.z, delay);
                parentTrans.DOMoveY((saveVal = parentTrans.position.y) + hauteur, delay / 2).OnComplete<Tweener>(() => parentTrans.DOMoveY(saveVal, delay / 2));
                //animation shield destroy
            }
            else
            {
                base.Degat(p_damage, p_technic);
            }
        }
    }

}

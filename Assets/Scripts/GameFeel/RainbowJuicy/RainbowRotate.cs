using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

public class RainbowRotate : MonoBehaviour
{
    int index = -1;
    public float time = .2f;
    public TypeRotate movesType;
    public Vector3[] moves;
    public Ease easeType;
	Transform currT;
	Quaternion startRot;


    void OnEnable()
    {
		currT = transform;
		startRot = transform.localRotation;
        Next();
        //transform.DOMoveY(LocalY, 0);
        //Debug.Log(LocalY);
    }

    void Next()
    {
        index = (index + 1) % moves.Length;
	
		if ( movesType == TypeRotate.Clockwise )
		{
			currT.DOLocalRotate ( moves [ index ], time, RotateMode.LocalAxisAdd ).SetEase ( easeType ).OnComplete ( ( ) => Next ( ) );
		}
    }

    void OnDisable()
    {
		currT.DOKill();

		currT.localRotation = startRot;
    }
}
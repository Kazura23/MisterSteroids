using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

public class RainbowMove : MonoBehaviour
{
    public enum Type{
        LocalVertical,
        LocalHorizontal,
        GlobalVertical,
        GlobalHorizontal
    }

    int index = -1;
    public float time = .2f;
    public Type movesType;
    public float[] moves;
    public Ease easeType;
	Transform currT;
    private float LocalY;

	Vector3 startPos;

    void OnEnable()
    {
		currT = transform;
		startPos = currT.localPosition;
        Next();
        //transform.DOMoveY(LocalY, 0);
        //Debug.Log(LocalY);
    }

    void Next()
    {
        index = (index + 1) % moves.Length;

        if (movesType == Type.LocalHorizontal)
			currT.DOLocalMoveX(moves[index], time).SetEase(easeType).OnComplete(() => Next());

        if (movesType == Type.LocalVertical)
			currT.DOLocalMoveY(moves[index], time).SetEase(easeType).OnComplete(() => Next());

        if (movesType == Type.GlobalHorizontal)
			currT.DOMoveX(moves[index], time).SetEase(easeType).OnComplete(() => Next());

        if (movesType == Type.GlobalVertical)
			currT.DOMoveY(moves[index], time).SetEase(easeType).OnComplete(() => Next());

    }

    void OnDisable()
    {
		currT.DOKill();
		currT.localPosition = startPos;
      /*  if (movesType == Type.LocalHorizontal)
			currT.DOLocalMoveX(moves[index], time).SetEase(easeType).OnComplete(() => Next());

        if (movesType == Type.LocalVertical)
        {
			currT.DOLocalMoveY(moves[index], time).SetEase(easeType).OnComplete(() => Next());

            //transform.DOMoveY(LocalY, 0);
            //Debug.Log("Disable " + LocalY);
			//
			//EmptyChild.DOKill();

            //LocalY = transform.position.y;
        }

        if (movesType == Type.GlobalHorizontal)
			currT.DOMoveX(moves[index], time).SetEase(easeType).OnComplete(() => Next());

        if (movesType == Type.GlobalVertical)
			currT.DOMoveY(moves[index], time).SetEase(easeType).OnComplete(() => Next());
		*/
    }
}
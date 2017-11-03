using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class GameOver : UiParent 
{
	#region Variables
	public override MenuType ThisMenu
	{
		get
		{
			return MenuType.GameOver;
		}
	}

	public GameObject PatternGameOver, BarGameOver;
	public Text YouGameOver, MadeGameOver, PointsGameOver, PressGameOver;
	#endregion

	#region Mono
	#endregion

	#region Public Methods
	public override void OpenThis ( MenuTokenAbstract GetTok = null )
	{
		base.OpenThis ( GetTok );

		Debug.Log("GameOver");
		GameOverTok thisTok = GetTok as GameOverTok;
		PointsGameOver.text = thisTok.totalDist.ToString ( );
		YouGameOver.DOFade(0, 0);
		MadeGameOver.DOFade(0, 0);
		PointsGameOver.DOFade(0, 0);
		YouGameOver.transform.DOScale(5, 0);
		MadeGameOver.transform.DOScale(5, 0);
		PointsGameOver.transform.DOScale(5, 0);
		BarGameOver.transform.DOScaleY(0, 0);

		PatternGameOver.transform.DOLocalMoveY(-60, 5f).SetEase(Ease.Linear).OnComplete(() => {
			PatternGameOver.transform.DOLocalMoveY(1092, 0);
		}).SetLoops(-1, LoopType.Restart);


		gameObject.GetComponent<CanvasGroup>().DOFade(1f, 1f).OnComplete(() =>
		{
			YouGameOver.DOFade(1, .25f);
			YouGameOver.transform.DOScale(1, .25f).OnComplete(()=> {
				MadeGameOver.DOFade(1, .25f);
				MadeGameOver.transform.DOScale(1, .25f).OnComplete(() =>
				{
					PointsGameOver.DOFade(1, .25f);
					BarGameOver.transform.DOScaleY(1.25f, .2f).OnComplete(() =>
					{
						BarGameOver.transform.DOScaleY(1, .05f);
					});
					PointsGameOver.transform.DOScale(1, .25f);

					DOVirtual.DelayedCall(1.5f, () => {
						PressGameOver.GetComponent<CanvasGroup>().DOFade(1, .5f);
					});
				});
			});
		});
	}
		

	public override void CloseThis ( )
	{
		base.CloseThis (  );
	}
	#endregion

	#region Private Methods
	protected override void InitializeUi()
	{
	}
	#endregion
}

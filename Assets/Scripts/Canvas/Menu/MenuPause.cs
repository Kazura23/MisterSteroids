using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class MenuPause : UiParent 
{
	#region Variables
	public override MenuType ThisMenu
	{
		get
		{
			return MenuType.Pause;
		}
	}

	public GameObject Patterns;
	public GameObject PauseObject;
	public Text PauseText;
	#endregion

	#region Mono
	#endregion

	#region Public Methods
	public override void OpenThis ( MenuTokenAbstract GetTok = null )
	{
		base.OpenThis ( GetTok );

		PauseObject.GetComponent<CanvasGroup> ( ).DOFade ( 1, .2f );

		PauseText.transform.DOScale ( 7, 0 );
		PauseText.transform.DOScale ( 1, .15f );
	}

	public override void CloseThis ( )
	{
		base.CloseThis (  );

		PauseObject.GetComponent<CanvasGroup>().DOFade(0, .2f);
		PauseText.transform.DOScale(7, .2f);
	}

	public override void Pause ( bool setPause )
	{
		base.Pause ( setPause );
	}
	#endregion

	#region Private Methods
	protected override void InitializeUi()
	{
		Patterns.transform.DOLocalMoveY(-60, 5f).SetEase(Ease.Linear).OnComplete(() => {
			Patterns.transform.DOLocalMoveY(1092, 0);
		}).SetLoops(-1, LoopType.Restart);
	}
	#endregion
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
	#endregion

	#region Mono
	#endregion

	#region Public Methods
	public override void OpenThis ( MenuTokenAbstract GetTok = null )
	{
		base.OpenThis ( GetTok );
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

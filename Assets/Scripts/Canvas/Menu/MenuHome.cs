using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuHome : UiParent 
{
	#region Variables
	public override MenuType ThisMenu
	{
		get
		{
			return MenuType.MenuHome;
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

	public override void Pause ( bool setPause )
	{
		base.Pause ( setPause );
	}

	public void ChangeMenu ( MenuType thisType )
	{
		GlobalManager.Ui.OpenThisMenu ( thisType );
	}

	public void ChangeScene ( string thisScene )
	{
		GlobalManager.Scene.LoadThisScene ( thisScene );
	}
	#endregion

	#region Private Methods
	protected override void InitializeUi()
	{
	}
	#endregion
}

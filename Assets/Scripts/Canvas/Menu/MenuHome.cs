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

	public void ChangeMenu ( string thisType )
	{
		System.Array thisArray = System.Enum.GetValues ( typeof ( MenuType ) );

		for ( int a = 0; a < thisArray.Length; a++ )
		{
			if ( thisArray.GetValue ( a ).ToString ( ) == thisType )
			{
				GlobalManager.Ui.OpenThisMenu ( (MenuType) thisArray.GetValue ( a ) );
				break;
			}
		}
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

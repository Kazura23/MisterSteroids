using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuShop : UiParent 
{
	#region Variables
	public override MenuType ThisMenu
	{
		get
		{
			return MenuType.Shop;
		}
	}

	public Transform DefCatSelected;

	Transform currCatSeled;
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

	public void NextCat ( bool right )
	{
		if (right)
		{
			
		}
	}
	#endregion

	#region Private Methods
	protected override void InitializeUi()
	{
		currCatSeled = DefCatSelected;
	}
	#endregion
}

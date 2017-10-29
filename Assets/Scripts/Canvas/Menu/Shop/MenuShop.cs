using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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

	//Object par défaut sélectionner a l'ouverture du shop
	public CatShop DefCatSelected;

	[HideInInspector]
	public CatShop currCatSeled;

	[HideInInspector]
	public ItemModif currItemSeled;

	Dictionary <string, ItemModif> allConfirm;

	bool catCurrSelected = true;
	bool waitInputH = false;
	bool waitInputV = false;
	#endregion

	#region Mono
	void Update ( )
	{
		float getH = Input.GetAxis ( "Horizontal" );
		float getV = Input.GetAxis ( "Vertical" );

		// Touche pour pouvoir selectionner les items
		if ( Input.GetAxis ( "Submit" ) == 1 )
		{
			ChangeToItem ( true );
		}

		// Touche pour sortir des items
		if ( Input.GetAxis ( "Cancel" ) == 1 )
		{
			ChangeToItem ( false );
		}

		// Navigation horizontale des catégories ou items
		if ( getH != 0 && !waitInputH )
		{
			waitInputH = true;

			if ( catCurrSelected )
			{
				if ( getH > 0 )
				{
					NextCat ( true );
				}
				else
				{
					NextCat ( false );
				}
			}
			else if ( getH == 1 || getH == -1 )
			{
				NextItem ( ( int ) getH );
			}
			else
			{
				waitInputH = false;
			}
		}
		else if ( Input.GetAxis ( "Horizontal" ) == 0 )
		{
			waitInputH = false;
		}

		// Navigation vertocal des items
		if ( !catCurrSelected && ( getV == 1 || getV == -1 ) && !waitInputV )
		{
				waitInputV = true;
				NextItem ( ( int ) getH * 2 );
		}
		else if ( Input.GetAxis ( "Vertical" ) == 0 )
		{
			waitInputV = false;
		}
	}
	#endregion

	#region Public Methods
	public override void OpenThis ( MenuTokenAbstract GetTok = null )
	{
		base.OpenThis ( GetTok );

		currCatSeled = DefCatSelected;
		if ( currItemSeled != currCatSeled.DefautItem )
		{
			CheckSelectItem ( false );
		}

		currItemSeled = currCatSeled.DefautItem;
		CheckSelectItem ( true );
	}

	public override void CloseThis ( )
	{
		base.CloseThis (  );
	}

	// Nouvelle selection de catégorie
	public void NextCat ( bool right )
	{
		CheckSelectCat ( false );

		if ( right )
		{
			currCatSeled = currCatSeled.RightCategorie;
		}
		else
		{
			currCatSeled = currCatSeled.LeftCategorie;
		}

		CheckSelectCat ( true );
	}

	// Nouvelle selection d'item
	// -1 = gauche _ 1 droite _ 2 haut _ -2 bas
	public void NextItem ( int thisDir )
	{
		CheckSelectItem ( false );

		switch ( thisDir )
		{
		case -1:
			currItemSeled = currItemSeled.LeftItem;
			break;
		case 1:
			currItemSeled = currItemSeled.RightItem;
			break;
		case -2:
			currItemSeled = currItemSeled.DownItem;
			break;
		case 2:
			currItemSeled = currItemSeled.UpItem;
			break;
		}

		CheckSelectItem ( true );
	}

	// achete ou confirme un item
	public void BuyItem ( )
	{
		string getCons = Constants.ItemBought;
		if ( AllPlayerPrefs.GetBoolValue ( getCons + currItemSeled.ItemName ) )
		{
			getCons += currItemSeled.CatName;
			AllPlayerPrefs.SetStringValue ( getCons, "Confirm" );
			ItemModif getThis;

			if ( allConfirm.TryGetValue ( getCons, out getThis ) )
			{
				if ( getThis.UseOtherSprite )
				{
					getThis.GetComponent<Image> ( ).sprite = currItemSeled.BoughtSpriteUnselected;
				}
				else
				{
					getThis.GetComponent<Image> ( ).sprite = currItemSeled.SpriteUnselected;
				}

				if ( getThis.UseColor )
				{
					if ( getThis.UseOtherColor )
					{
						getThis.GetComponent<Image> ( ).color = currItemSeled.BoughtColorUnSelected;
					}
					else
					{
						getThis.GetComponent<Image> ( ).color = currItemSeled.ColorUnSelected;
					}
				}

				allConfirm.Remove ( getCons );
			}

			allConfirm.Add ( getCons, currItemSeled );

			currItemSeled.GetComponent<Image> ( ).sprite = currItemSeled.SpriteConfirm;

			if ( currItemSeled.UseColor )
			{
				currItemSeled.GetComponent<Image> ( ).color = currItemSeled.ColorConfirm;
			}
		}
		else
		{
			if ( AllPlayerPrefs.GetIntValue ( Constants.Coin ) > currItemSeled.Price )
			{
				AllPlayerPrefs.SetIntValue ( Constants.Coin, -currItemSeled.Price );

				if ( currItemSeled.BuyForLife )
				{
					AllPlayerPrefs.SetStringValue ( getCons + currItemSeled.ItemName );
				}
			}
		}
	}
	#endregion

	#region Private Methods
	protected override void InitializeUi()
	{
		currCatSeled = DefCatSelected;
		currItemSeled = currCatSeled.DefautItem;

		ItemModif[] checkAllItem = GetComponentsInChildren<ItemModif> ( true );
		ItemModif currItem;

		string getCons = Constants.ItemBought;
		Dictionary <string, ItemModif> getItemConf = new Dictionary<string, ItemModif> ( );

		for ( int a = 0; a < checkAllItem.Length; a++ )
		{
			if ( AllPlayerPrefs.GetBoolValue ( getCons + checkAllItem [ a ].CatName ) )
			{
				currItem = checkAllItem [ a ];
				getItemConf.Add ( getCons + currItem, currItem ); 

				currItem.GetComponent<Image> ( ).sprite = currItem.SpriteConfirm;

				if ( currItem.UseColor )
				{
					currItem.GetComponent<Image> ( ).color = currItem.ColorConfirm;
				}
			}
		}

		CheckSelectItem ( true );

		allConfirm = getItemConf;
		GlobalManager.GameCont.AllModifItem = getItemConf;
	}

	//Changement de catégorie a item et inversement
	void ChangeToItem ( bool goItem )
	{
		if ( goItem && catCurrSelected ) // Changement de cat a item
		{
			catCurrSelected = false;
		}
		else if ( !goItem && !catCurrSelected ) // Changement de item a cat
		{
			catCurrSelected = true;
		}
	}

	// Selection d'une nouvelle catégorie
	void CheckSelectCat ( bool selected )
	{
		CatShop thisShop = currCatSeled;

		if ( selected )
		{
			CheckSelectItem ( false );
			currItemSeled = thisShop.DefautItem;
			CheckSelectItem ( true );
			thisShop.Selected = true;
			if ( thisShop.UseColor )
			{
				thisShop.GetComponent<Image> ( ).color = thisShop.ColorSelected;
			}

			if ( thisShop.UseSprite )
			{
				thisShop.GetComponent<Image> ( ).sprite = thisShop.SpriteSelected;
			}
		}
		else
		{
			thisShop.Selected = false;
			if ( thisShop.UseColor )
			{
				thisShop.GetComponent<Image> ( ).color = thisShop.ColorUnSelected;
			}
			if ( thisShop.UseSprite )
			{
				thisShop.GetComponent<Image> ( ).sprite = thisShop.SpriteUnSelected;
			}
		}
	}

	// Selection d'un nouvelle item
	void CheckSelectItem ( bool selected )
	{
		ItemModif thisItem = currItemSeled;

		if ( selected )
		{
			thisItem.Selected = true;
			if ( thisItem.ItemBought && thisItem.UseOtherColor )
			{
				thisItem.GetComponent<Image> ( ).color = thisItem.BoughtColorSelected;
			}
			else if ( thisItem.UseColor )
			{
				thisItem.GetComponent<Image> ( ).color = thisItem.ColorSelected;
			}

			if ( thisItem.ItemBought && thisItem.UseOtherSprite )
			{
				thisItem.GetComponent<Image> ( ).sprite = thisItem.BoughtSpriteSelected;
			}
			else
			{
				thisItem.GetComponent<Image> ( ).sprite = thisItem.SpriteSelected;
			}
		}
		else
		{
			thisItem.Selected = false;
			if ( thisItem.ItemBought && thisItem.UseOtherColor )
			{
				thisItem.GetComponent<Image> ( ).color = thisItem.BoughtColorUnSelected;
			}
			else if ( thisItem.UseColor )
			{
				thisItem.GetComponent<Image> ( ).color = thisItem.ColorUnSelected;
			}

			if ( thisItem.ItemBought && thisItem.UseOtherSprite )
			{
				thisItem.GetComponent<Image> ( ).sprite = thisItem.BoughtSpriteUnselected;
			}
			else
			{
				thisItem.GetComponent<Image> ( ).sprite = thisItem.SpriteUnselected;
			}
		}
	}
	#endregion
}

﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

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

    [Header("ALL INFO")]

    public Image iconCategory;
    public Text textCategory;
    public Image barCategory;
    public Image moleculeCategory;
    public GameObject moleculeContainer;


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
            ChangeToCat();
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
        CatShop thisShop = currCatSeled;

        if ( goItem && catCurrSelected ) // Changement de cat a item
		{
			catCurrSelected = false;

            currItemSeled = thisShop.DefautItem;

            iconCategory.DOFade(0, .1f);
            textCategory.DOFade(0, .1f);
            barCategory.DOFade(0, .1f);
        
            
            

            transform.DORotate(new Vector3(moleculeContainer.transform.localEulerAngles.x, moleculeContainer.transform.localEulerAngles.y, -130),.5f);
            transform.DOLocalMoveX(transform.localPosition.x -625, 1f);
            transform.DOLocalMoveY(transform.localPosition.y - 200, 1f);
            transform.DOScale(1.25f, 1f).OnComplete(()=> {
                thisShop.GetComponent<Image>().DOFade(1, 0.1f);
                iconCategory.transform.DORotate(Vector3.zero, 0);
                textCategory.transform.DORotate(Vector3.zero, 0);
                barCategory.transform.DORotate(Vector3.zero, 0);
                iconCategory.transform.DOMoveY(transform.position.y + 500,0);
                textCategory.transform.DOMoveY(transform.position.y + 450, 0);
                textCategory.transform.DOMoveX(transform.position.x -40, 0);
                barCategory.transform.DOMoveX(transform.position.x - 40, 0);
                barCategory.transform.DOMoveY(transform.position.y + 450, 0);
                iconCategory.DOFade(1, .25f);
                textCategory.DOFade(1, .25f);
                barCategory.DOFade(1, .25f);
            });

            //On remet les molécules de couleur au gris
            foreach (Transform cat in DefCatSelected.transform.parent)
            {
                cat.GetComponent<Image>().DOFade(0, 0.1f);
            }
            foreach (Transform trans in thisShop.transform)
            {
                trans.GetComponent<CanvasGroup>().DOFade(1, 0.5f);
                trans.DOLocalRotate(new Vector3(0, 0, 130), 0);
                trans.DOLocalMove(new Vector2(-280,600),0);
                trans.GetComponent<CanvasGroup>().DOFade(1, .1f);
            }
        }
		else if ( !goItem && !catCurrSelected ) // Changement de item a cat
		{
			catCurrSelected = true;
		}
	}

    void ChangeToCat()
    {
        iconCategory.DOFade(0, .1f);
        textCategory.DOFade(0, .1f);
        barCategory.DOFade(0, .1f);
        transform.DORotate(Vector3.zero, .5f);
        transform.DOScale(1, .5f);
        transform.DOLocalMove(Vector2.zero, .5f).OnComplete(()=> {
            iconCategory.transform.DORotate(Vector3.zero, 0);
            textCategory.transform.DORotate(Vector3.zero, 0);
            barCategory.transform.DORotate(Vector3.zero, 0);
            iconCategory.DOFade(1, .1f);
            textCategory.DOFade(1, .1f);
            barCategory.DOFade(1, .1f);
        });

        //On remet les molécules à leur couleur initiale
        foreach (Transform cat in DefCatSelected.transform.parent)
        {
            cat.GetComponent<Image>().DOFade(1, 0.1f);
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

            
            

            DOVirtual.DelayedCall(.1f, () => {
                iconCategory.GetComponent<Image>().DOFade(1, .1f);
                textCategory.DOFade(1, .1f);
                barCategory.transform.GetChild(0).transform.DOLocalMoveX(200, 0);
                barCategory.transform.GetChild(0).transform.DOLocalMoveX(0, .6f);

                textCategory.text = thisShop.NameCat;
                iconCategory.sprite = thisShop.SpriteSelected;
                thisShop.GetComponent<Image>().transform.DOScale(1.25f, .2f);
                //thisShop.GetComponent<Image>().DOFade(1f, .05f);
                iconCategory.GetComponent<Image>().sprite = thisShop.SpriteSelected;
                iconCategory.transform.DOMoveY(thisShop.GetComponent<Image>().transform.position.y + 160, 0);


                textCategory.transform.DOMoveY(thisShop.GetComponent<Image>().transform.position.y + 75, 0);

                barCategory.transform.GetChild(0).GetComponent<Image>().DOColor(thisShop.ColorSelected, 0);

                if (textCategory.text == "ABILITIES")
                {
                    textCategory.transform.DOMoveX(thisShop.GetComponent<Image>().transform.position.x - 55, 0);
                    barCategory.transform.DOMoveX(thisShop.GetComponent<Image>().transform.position.x - 55, 0);
                    iconCategory.transform.DOMoveX(thisShop.GetComponent<Image>().transform.position.x - 40, 0);
                }
                else
                {
                    textCategory.transform.DOMoveX(thisShop.GetComponent<Image>().transform.position.x, 0);
                    barCategory.transform.DOMoveX(thisShop.GetComponent<Image>().transform.position.x, 0);
                    iconCategory.transform.DOMoveX(thisShop.GetComponent<Image>().transform.position.x, 0);
                }

                barCategory.transform.DOMoveY(thisShop.GetComponent<Image>().transform.position.y + 75, 0);
            });
            /*
            iconCategory.transform.DOLocalMoveY(transform.localPosition.y + 10, .25f).OnComplete(() => {
                iconCategory.transform.DOLocalMoveY(transform.localPosition.y - 10, .25f);
            }).SetLoops(-1,LoopType.Restart);*/

            //iconCategory.transform.DOKill();
            //iconCategory.GetComponent<RainbowMove>().enabled = true;


            if ( thisShop.UseColor )
			{
				thisShop.GetComponent<Image> ( ).color = thisShop.ColorSelected;
			}

			if ( thisShop.UseSprite )
			{
				//thisShop.GetComponent<Image> ( ).sprite = thisShop.SpriteSelected;
			}
		}
		else
		{
			thisShop.Selected = false;
            //iconCategory.transform.DOKill();

            
                iconCategory.GetComponent<Image>().DOFade(0, .1f);
            textCategory.DOFade(0, .1f);
            iconCategory.GetComponent<RainbowMove>().enabled = false;
            thisShop.GetComponent<Image>().transform.DOScale(.8f, .2f);
           // thisShop.GetComponent<Image>().DOFade(0, .2f);

            if ( thisShop.UseColor )
			{
				//thisShop.GetComponent<Image> ( ).color = thisShop.ColorUnSelected;
			}
			if ( thisShop.UseSprite )
			{
				//thisShop.GetComponent<Image> ( ).sprite = thisShop.SpriteUnSelected;
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


            //Code du outline
            /*
            thisItem.GetComponent<Outline>().transform.DOScale(2, .75f).OnComplete(() => {
                thisItem.GetComponent<Outline>().DOFade(0, .25f);
                DOVirtual.DelayedCall(.25f, () => {
                    thisItem.GetComponent<Outline>().DOFade(1, 0);
                    thisItem.GetComponent<Outline>().transform.DOScale(1, 0);
                });
            }).SetLoops(-1,LoopType.Restart);*/

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

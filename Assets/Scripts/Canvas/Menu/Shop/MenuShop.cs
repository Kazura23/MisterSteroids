using System.Collections;
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
	GameObject fixBackShop;
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

		if ( Input.GetKeyDown ( KeyCode.A ) && !catCurrSelected )
		{
			BuyItem ( );
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
		fixBackShop.SetActive ( true );
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
		fixBackShop.SetActive ( false );
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
        DOVirtual.DelayedCall(.1f, () => {
            CheckSelectItem ( false );
            switch (thisDir)
            {
                case -1:
                    currItemSeled = currItemSeled.LeftItem;
                    ItemLeft();
                    break;
                case 1:
                    currItemSeled = currItemSeled.RightItem;
                    ItemRight();
                    break;
                case -2:
                    currItemSeled = currItemSeled.DownItem;
                    break;
                case 2:
                    currItemSeled = currItemSeled.UpItem;
                    break;
            }

        });
		
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
			bool checkProg = false;
			ItemModif currIT = currItemSeled;

			if ( currCatSeled.Progression )
			{
				if ( currIT.UpItem.ItemBought || currIT.DownItem.ItemBought || currIT.LeftItem.ItemBought || currIT.RightItem.ItemBought )
				{
					checkProg = true;
				}
			}
			else
			{
				checkProg = true;	
			}

			if ( checkProg && AllPlayerPrefs.GetIntValue ( Constants.Coin ) > currIT.Price )
			{
				Debug.Log ( "buy" );
				AllPlayerPrefs.SetIntValue ( Constants.Coin, -currIT.Price );

				if ( currCatSeled.BuyForLife )
				{
					AllPlayerPrefs.SetStringValue ( getCons + currIT.ItemName );
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

		fixBackShop = transform.parent.Find ( "GlobalBackGround/Shop" ).gameObject;
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




            transform.DORotate(new Vector3(moleculeContainer.transform.localEulerAngles.x, moleculeContainer.transform.localEulerAngles.y, -130),1f);
            transform.DOLocalMoveX(transform.localPosition.x -625, 1f);
            transform.DOLocalMoveY(transform.localPosition.y - 200, 1f);
            transform.DOScale(1.25f, 1f).OnComplete(()=> {
                thisShop.GetComponent<Image>().DOFade(1, 0.1f);
                iconCategory.transform.DORotate(Vector3.zero, 0);
                textCategory.transform.DORotate(new Vector3(0,0,423), 0);
                barCategory.transform.DORotate(new Vector3(0,0,423), 0);
                iconCategory.transform.DOMoveX(thisShop.transform.position.x + 200, 0);
                iconCategory.transform.DOMoveY(thisShop.transform.position.y ,0);
                textCategory.transform.DOMoveY(moleculeContainer.transform.position.y + 300, 0);
                textCategory.transform.DOMoveX(moleculeContainer.transform.position.x -90, 0);
                barCategory.transform.DOMoveX(moleculeContainer.transform.position.x - 90, 0);
                barCategory.transform.DOMoveY(moleculeContainer.transform.position.y + 300, 0);
                iconCategory.DOFade(1, .25f);
                textCategory.DOFade(1, .25f);
                barCategory.DOFade(1, .25f);
            });

            //On remet les molécules de couleur au gris
            foreach (Transform cat in DefCatSelected.transform.parent)
            {
                cat.GetComponent<Image>().DOFade(0, 0.1f);
            }

            //Seul le premier item est centré
            thisShop.transform.GetChild(0).DOLocalMove(new Vector2(-280, 600), 0);
            thisShop.transform.GetChild(1).DOLocalMove(new Vector2(-525, 895), 0);

            DOVirtual.DelayedCall(1f, () => {
                foreach (Transform trans in thisShop.transform)
                {
                    trans.GetComponent<CanvasGroup>().DOFade(1, 0.5f);
                    trans.DOLocalRotate(new Vector3(0, 0, 130), 0);
                    trans.DOScale(.75f, 0);
                }
            });

            
        }
		else if ( !goItem && !catCurrSelected ) // Changement de item a cat
		{
			catCurrSelected = true;
		}
	}

    void ChangeToCat()
    {
        CatShop thisShop = currCatSeled;

        iconCategory.DOFade(0, .05f);
        textCategory.DOFade(0, .05f);
        barCategory.DOFade(0, .05f);
        transform.DORotate(Vector3.zero, .5f);
        transform.DOScale(1, .5f);
        transform.DOLocalMove(Vector2.zero, .5f).OnComplete(()=> {
            iconCategory.transform.DORotate(Vector3.zero, 0);
            textCategory.transform.DORotate(Vector3.zero, 0);
            barCategory.transform.DORotate(Vector3.zero, 0);
            iconCategory.DOFade(1, .1f);
            textCategory.DOFade(1, .1f);
            barCategory.DOFade(1, .1f);

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
            iconCategory.transform.DOMoveY(thisShop.GetComponent<Image>().transform.position.y + 160, 0);
            textCategory.transform.DOMoveY(thisShop.GetComponent<Image>().transform.position.y + 75, 0);
            barCategory.transform.DOMoveY(thisShop.GetComponent<Image>().transform.position.y + 75, 0);
        });

        //On remet les molécules à leur couleur initiale
        foreach (Transform cat in DefCatSelected.transform.parent)
        {
            cat.GetComponent<Image>().DOFade(1, 0.1f);
        }

        foreach (Transform trans in thisShop.transform)
        {
            trans.GetComponent<CanvasGroup>().DOFade(0, 0);
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
                iconCategory.transform.DOMoveY(thisShop.GetComponent<Image>().transform.position.y + 160, 0);
                textCategory.transform.DOMoveY(thisShop.GetComponent<Image>().transform.position.y + 75, 0);
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

    void ItemLeft()
    {
        ItemModif thisItem = currItemSeled;

        thisItem.LeftItem.transform.DOLocalMove(new Vector2(-50, 340), .5f);
        thisItem.LeftItem.GetComponent<CanvasGroup>().DOFade(.75f, .2f);
        thisItem.LeftItem.transform.DOScale(.4f, .2f);

        thisItem.transform.DOLocalMove(new Vector2 (-280,600), .5f);
        thisItem.transform.DOScale(.75f, .2f);
        thisItem.GetComponent<CanvasGroup>().DOFade(1, .2f);

    }

    void ItemRight()
    {
        ItemModif thisItem = currItemSeled;

        thisItem.RightItem.transform.DOLocalMove(new Vector2(-50, 340), .5f);
        thisItem.RightItem.GetComponent<CanvasGroup>().DOFade(.75f, .2f);
        thisItem.LeftItem.transform.DOScale(.4f, .2f);

        thisItem.transform.DOLocalMove(new Vector2(-280, 600), .5f);
        thisItem.transform.DOScale(.75f, .2f);
        thisItem.GetComponent<CanvasGroup>().DOFade(1, .2f);


        //thisItem.transform.DOLocalMove(new Vector2(-448, 800), .5f);
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

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemModif : MonoBehaviour 
{
	#region Variables
	public bool ItemBought;
	public string ItemName;
	public int Price;

	public bool UseSprite;
	public bool UseOtherSprite;
	public bool UseColor;
	public bool UseOtherColor;

	public Color ColorConfirm;
	public Color ColorSelected;
	public Color ColorUnSelected;

	public Color BoughtColorSelected;
	public Color BoughtColorUnSelected;

	public Sprite SpriteConfirm;
	public Sprite SpriteSelected;
	public Sprite SpriteUnselected;

	public Sprite BoughtSpriteUnselected;
	public Sprite BoughtSpriteSelected;

	public ItemModif RightItem;
	public ItemModif LeftItem;
	public ItemModif UpItem;
	public ItemModif DownItem;

	public string CatName;

	public bool ModifVie;
	public bool ModifSpecial;

	public int NombreVie;
	public SpecialAction SpecAction;

	public bool Selected;
	#endregion

	#region Mono
	#endregion

	#region Public Methods
	#endregion

	#region Private Methods
	#endregion
}

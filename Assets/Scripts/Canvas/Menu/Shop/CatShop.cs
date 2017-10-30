using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CatShop : MonoBehaviour 
{
	#region Variables
	[Header ("Information Categorie")]

    public string NameCat;
    public bool Selected;
	public bool UseColor;
	public Color ColorSelected;
	public Color ColorUnSelected;

    
    public Sprite IconCat;

    public bool UseSprite;
	public Sprite SpriteSelected;
	public Sprite SpriteUnSelected;

	[Header ("Information Categorie Voisines")]
	public CatShop LeftCategorie;
	public CatShop RightCategorie;

	public ItemModif DefautItem;
	#endregion

	#region Mono
	#endregion

	#region Public Methods
	#endregion

	#region Private Methods
	#endregion
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEditor;

[CustomEditor (typeof (ItemModif))]
public class EditItemMod : Editor 
{
	#region Variables
	#endregion

	#region Public Methods
	public override void OnInspectorGUI()
	{
		EditorGUILayout.LabelField("Inspector Item Info", EditorStyles.boldLabel);
		ItemModif myTarget = (ItemModif)target;

		myTarget.ItemName = EditorGUILayout.TextField ( "ItemName", myTarget.ItemName );
		myTarget.Price = EditorGUILayout.IntField ( "Price", myTarget.Price );
		myTarget.BuyForLife = EditorGUILayout.Toggle ( "BuyForLife", myTarget.BuyForLife );
		myTarget.UseColor = EditorGUILayout.Toggle ( "UseColor", myTarget.UseColor );
		myTarget.UseSprite = EditorGUILayout.Toggle ( "UseSprite", myTarget.UseSprite );

		myTarget.CatName = myTarget.transform.parent.GetComponent<CatShop> ( ).NameCat;

		if ( AllPlayerPrefs.GetBoolValue ( Constants.ItemBought + myTarget.ItemName ) )
		{
			myTarget.ItemBought = true;
		}
		else
		{
			myTarget.ItemBought = false;
		}

		EditorGUILayout.Space ( );

		if ( myTarget.UseColor )
		{
			myTarget.ColorConfirm = EditorGUILayout.ColorField ( "ColorConfirm", myTarget.ColorConfirm );
			myTarget.ColorSelected = EditorGUILayout.ColorField ( "ColorSelected", myTarget.ColorSelected );
			myTarget.ColorUnSelected = EditorGUILayout.ColorField ( "ColorUnSelected", myTarget.ColorUnSelected );

			myTarget.UseOtherColor = EditorGUILayout.Toggle ( "OtherColorWhenBough", myTarget.UseOtherColor );

			if ( myTarget.UseOtherColor )
			{
				myTarget.BoughtColorSelected = EditorGUILayout.ColorField ( "BoughtColorSelected", myTarget.BoughtColorSelected );
				myTarget.BoughtColorUnSelected = EditorGUILayout.ColorField ( "BoughtColorUnSelected", myTarget.BoughtColorUnSelected );

				if ( myTarget.ItemBought )
				{
					if ( myTarget.Selected )
					{
						myTarget.GetComponent<Image> ( ).color = myTarget.BoughtColorSelected;
					}
					else
					{
						myTarget.GetComponent<Image> ( ).color = myTarget.BoughtColorUnSelected;
					}
				}
			}
			else
			{
				if ( myTarget.Selected )
				{
					myTarget.GetComponent<Image> ( ).color = myTarget.ColorSelected;
				}
				else
				{
					myTarget.GetComponent<Image> ( ).color = myTarget.ColorUnSelected;
				}
			}

			EditorGUILayout.Space ( );
		}

		if ( myTarget.SpriteConfirm == null )
		{
			myTarget.SpriteConfirm = myTarget.GetComponent<Image> ( ).sprite;
		}
		if ( myTarget.SpriteSelected == null )
		{
			myTarget.SpriteSelected = myTarget.GetComponent<Image> ( ).sprite;
		}
		if ( myTarget.SpriteUnselected == null )
		{
			myTarget.SpriteUnselected = myTarget.GetComponent<Image> ( ).sprite;
		}

		if ( myTarget.BoughtSpriteSelected == null )
		{
			myTarget.BoughtSpriteSelected = myTarget.GetComponent<Image> ( ).sprite;
		}
		if ( myTarget.BoughtSpriteUnselected == null )
		{
			myTarget.BoughtSpriteUnselected = myTarget.GetComponent<Image> ( ).sprite;
		}

		if ( myTarget.UseSprite )
		{
			myTarget.SpriteConfirm = (Sprite)EditorGUILayout.ObjectField ( "SpriteConfirm", myTarget.SpriteConfirm, typeof( Sprite ), true );
			myTarget.SpriteSelected = (Sprite)EditorGUILayout.ObjectField ( "SpriteSelected", myTarget.SpriteSelected, typeof( Sprite ), true );
			myTarget.SpriteUnselected = (Sprite)EditorGUILayout.ObjectField ( "SpriteUnselected", myTarget.SpriteUnselected, typeof( Sprite ), true );

			myTarget.UseOtherSprite = EditorGUILayout.Toggle ( "OtherSpriteWhenBough", myTarget.UseOtherSprite );

			if ( myTarget.UseOtherSprite )
			{
				myTarget.BoughtSpriteSelected = ( Sprite ) EditorGUILayout.ObjectField ( "BoughtSpriteSelected", myTarget.BoughtSpriteSelected, typeof( Sprite ), true );
				myTarget.BoughtSpriteUnselected = ( Sprite ) EditorGUILayout.ObjectField ( "BoughtSpriteUnselected", myTarget.BoughtSpriteUnselected, typeof( Sprite ), true );

				if ( myTarget.ItemBought )
				{
					if ( myTarget.Selected )
					{
						myTarget.GetComponent<Image> ( ).sprite = myTarget.BoughtSpriteSelected;
					}
					else
					{
						myTarget.GetComponent<Image> ( ).sprite = myTarget.BoughtSpriteUnselected;
					}
				}

			}
			else
			{
				if ( myTarget.Selected )
				{
					myTarget.GetComponent<Image> ( ).sprite = myTarget.SpriteSelected;
				}
				else
				{
					myTarget.GetComponent<Image> ( ).sprite = myTarget.SpriteUnselected;
				}
			}

			EditorGUILayout.Space ( );
		}

		myTarget.RightItem = (ItemModif)EditorGUILayout.ObjectField ( "RightItem", myTarget.RightItem, typeof( ItemModif ), true );
		myTarget.LeftItem = (ItemModif)EditorGUILayout.ObjectField ( "LeftItem", myTarget.LeftItem, typeof( ItemModif ), true );
		myTarget.UpItem = (ItemModif)EditorGUILayout.ObjectField ( "UpItem", myTarget.UpItem, typeof( ItemModif ), true );
		myTarget.DownItem =(ItemModif)EditorGUILayout.ObjectField ( "DownItem", myTarget.DownItem, typeof( ItemModif ), true );

		EditorGUILayout.Space ( );
		EditorGUILayout.LabelField("Modification", EditorStyles.boldLabel);

		myTarget.ModifVie = EditorGUILayout.Toggle ( "ModifieVie", myTarget.ModifVie );
		myTarget.ModifReduceMot = EditorGUILayout.Toggle ( "ModifMotionTime", myTarget.ModifReduceMot );
		myTarget.ModifRecovereMot = EditorGUILayout.Toggle ( "ModifRecovereMot", myTarget.ModifRecovereMot );

		if ( myTarget.ModifVie )
		{
			myTarget.NombreVie = EditorGUILayout.IntField ( "NombreVie", myTarget.NombreVie );

			if ( myTarget.NombreVie <= 0 )
			{
				myTarget.NombreVie = 1;
			}
		}
		if ( myTarget.ModifReduceMot )
		{
			myTarget.ReduceSlowMot = EditorGUILayout.FloatField ( "ReduceSlowMot", myTarget.ReduceSlowMot );

			if ( myTarget.ReduceSlowMot <= 0 )
			{
				myTarget.ReduceSlowMot = 0.1f;
			}
		}
		if ( myTarget.ModifRecovereMot )
		{
			myTarget.RecoverSlowMot = EditorGUILayout.FloatField ( "RecoverSlowMot", myTarget.RecoverSlowMot );

			if ( myTarget.RecoverSlowMot <= 0 )
			{
				myTarget.RecoverSlowMot = 0.1f;
			}
		}

		if ( myTarget.RightItem == null )
		{
			myTarget.RightItem = myTarget.GetComponent<ItemModif> ( );
		}
		if ( myTarget.LeftItem == null )
		{
			myTarget.LeftItem = myTarget.GetComponent<ItemModif> ( );
		}
		if ( myTarget.UpItem == null )
		{
			myTarget.UpItem = myTarget.GetComponent<ItemModif> ( );
		}
		if ( myTarget.DownItem == null )
		{
			myTarget.DownItem = myTarget.GetComponent<ItemModif> ( );
		}

		/*

		if ( myTarget.UpItem == null )
		{
			myTarget.UpItem = myTarget.GetComponent<ItemModif> ( );
		}
		if ( myTarget.DownItem == null )
		{
			myTarget.DownItem = myTarget.GetComponent<ItemModif> ( );
		}*/
	}
	#endregion

	#region Private Methods
	#endregion
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEditor;

[CustomEditor (typeof (CatShop))]
public class EditCat : Editor 
{
	#region Variables
	#endregion


	#region Public Methods
	public override void OnInspectorGUI()
	{
		CatShop myTarget = (CatShop)target;

		myTarget.NameCat = EditorGUILayout.TextField ( "NameCat", myTarget.NameCat );

		myTarget.UseColor = EditorGUILayout.Toggle ( "UseColor", myTarget.UseColor );
		myTarget.UseSprite = EditorGUILayout.Toggle ( "UseSprite", myTarget.UseSprite );

		if ( myTarget.UseColor )
		{
			myTarget.ColorSelected = EditorGUILayout.ColorField ( "ColorSelected", myTarget.ColorSelected );
			myTarget.ColorUnSelected = EditorGUILayout.ColorField ( "ColorUnSelected", myTarget.ColorUnSelected );

			if ( myTarget.Selected )
			{
				myTarget.GetComponent<Image> ( ).color = myTarget.ColorSelected;
			}
			else
			{
				myTarget.GetComponent<Image> ( ).color = myTarget.ColorUnSelected;
			}
		}

		if ( myTarget.SpriteSelected == null )
		{
			myTarget.SpriteSelected = myTarget.GetComponent<Image> ( ).sprite;
		}
		if ( myTarget.SpriteUnSelected == null )
		{
			myTarget.SpriteUnSelected = myTarget.GetComponent<Image> ( ).sprite;
		}

		if ( myTarget.UseSprite )
		{
			myTarget.SpriteSelected = (Sprite)EditorGUILayout.ObjectField ( "SpriteSelected", myTarget.SpriteSelected, typeof( Sprite ), true );
			myTarget.SpriteUnSelected = (Sprite)EditorGUILayout.ObjectField ( "SpriteUnSelected", myTarget.SpriteUnSelected, typeof( Sprite ), true );

			if ( myTarget.Selected )
			{
				myTarget.GetComponent<Image> ( ).sprite = myTarget.SpriteSelected;
			}
			else
			{
				myTarget.GetComponent<Image> ( ).sprite = myTarget.SpriteUnSelected;
			}
		}

		myTarget.LeftCategorie = (CatShop)EditorGUILayout.ObjectField ( "LeftCategorie", myTarget.LeftCategorie, typeof( CatShop ), true );
		myTarget.RightCategorie = (CatShop)EditorGUILayout.ObjectField ( "RightCategorie", myTarget.RightCategorie, typeof( CatShop ), true );
		myTarget.DefautItem =(ItemModif)EditorGUILayout.ObjectField ( "DefautItem", myTarget.DefautItem, typeof( ItemModif ), true );
	}
	#endregion

	#region Private Methods
	#endregion
}

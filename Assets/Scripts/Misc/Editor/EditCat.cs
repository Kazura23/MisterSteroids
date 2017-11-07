using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEditor;

[CanEditMultipleObjects]
[CustomEditor (typeof (CatShop))]
public class EditCat : Editor 
{
	#region Variables
	// bool var;
	SerializedProperty NameCat;
	SerializedProperty BuyForLife;
	SerializedProperty Progression;

	SerializedProperty ColorSelected;
	SerializedProperty ColorUnSelected;
	SerializedProperty Selected;
	SerializedProperty SpriteSelected;
	SerializedProperty SpriteUnSelected;

	SerializedProperty LeftCategorie;
	SerializedProperty RightCategorie;
	SerializedProperty DefautItem;
	#endregion

	#region Public Methods
	public void OnEnable ( )
	{
		NameCat = serializedObject.FindProperty("NameCat");
		BuyForLife = serializedObject.FindProperty("BuyForLife");
		Progression = serializedObject.FindProperty("Progression");

		ColorSelected = serializedObject.FindProperty("ColorSelected");
		ColorUnSelected = serializedObject.FindProperty("ColorUnSelected");
		SpriteSelected = serializedObject.FindProperty("SpriteSelected");
		SpriteUnSelected = serializedObject.FindProperty("SpriteUnSelected");

		LeftCategorie = serializedObject.FindProperty("LeftCategorie");
		RightCategorie = serializedObject.FindProperty("RightCategorie");
		DefautItem = serializedObject.FindProperty("DefautItem");
	}

	public override void OnInspectorGUI()
	{
		CatShop myTarget = ( CatShop ) target;

		serializedObject.Update ( );
		EditorGUILayout.PropertyField ( NameCat );

		EditorGUILayout.BeginHorizontal();
		EditorGUILayout.PropertyField ( BuyForLife );
		EditorGUILayout.PropertyField ( Progression );
		EditorGUILayout.EndHorizontal ( );

		EditorGUILayout.Space ( );
		EditorGUILayout.BeginHorizontal();
		if ( GUILayout.Button ( "UseColor", EditorStyles.miniButtonLeft ) )
		{
			myTarget.UseColor = !myTarget.UseColor;
		}

		if ( GUILayout.Button ( "UseSprite", EditorStyles.miniButtonRight ) )
		{
			myTarget.UseSprite = !myTarget.UseSprite;
		}
		EditorGUILayout.EndHorizontal ( );

		EditorGUI.indentLevel = 1;
		if ( myTarget.UseColor )
		{
			EditorGUILayout.Space ( );
			EditorGUILayout.LabelField("Color Information", EditorStyles.boldLabel);

			EditorGUILayout.PropertyField ( ColorSelected );
			EditorGUILayout.PropertyField ( ColorUnSelected );

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
			EditorGUILayout.Space ( );

			EditorGUILayout.LabelField("Sprite Information", EditorStyles.boldLabel);

			EditorGUILayout.PropertyField(SpriteSelected);
			EditorGUILayout.PropertyField(SpriteUnSelected);

			if ( myTarget.Selected )
			{
				myTarget.GetComponent<Image> ( ).sprite = myTarget.SpriteSelected;
			}
			else
			{
				myTarget.GetComponent<Image> ( ).sprite = myTarget.SpriteUnSelected;
			}
		}

		EditorGUI.indentLevel = 0;

		EditorGUILayout.Space ( );
		EditorGUILayout.LabelField("Around Information", EditorStyles.boldLabel);
		EditorGUILayout.PropertyField ( LeftCategorie );
		EditorGUILayout.PropertyField ( RightCategorie );
		EditorGUILayout.PropertyField ( DefautItem );
		serializedObject.ApplyModifiedProperties ( );
	}
	#endregion

	#region Private Methods
	#endregion
}

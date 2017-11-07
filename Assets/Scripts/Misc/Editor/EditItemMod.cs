using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEditor;

[CustomEditor (typeof (ItemModif))]
public class EditItemMod : Editor 
{
	#region Variables
	// bool var;
	SerializedProperty ItemName;
	SerializedProperty Price;

	SerializedProperty ColorConfirm;
	SerializedProperty ColorSelected;
	SerializedProperty ColorUnSelected;
	SerializedProperty BoughtColorSelected;
	SerializedProperty BoughtColorUnSelected;

	SerializedProperty SpriteConfirm;
	SerializedProperty SpriteSelected;
	SerializedProperty SpriteUnselected;
	SerializedProperty BoughtSpriteSelected;
	SerializedProperty BoughtSpriteUnselected;

	SerializedProperty RightItem;
	SerializedProperty LeftItem;
	SerializedProperty UpItem;
	SerializedProperty DownItem;

	SerializedProperty SpecAction;
	SerializedProperty SlowMotion;
	SerializedProperty SpeedDeacSM;
	SerializedProperty ReduceSlider;
	SerializedProperty RecovSlider;
	SerializedProperty SpeedSlowMot;

	GUIContent Confirm;
	GUIContent Select;
	GUIContent UnSelect;
	GUIContent BuySelect;
	GUIContent BuyUnSelect;
	#endregion

	#region Public Methods
	public void OnEnable ( )
	{
		Confirm = new GUIContent ( "Confirm" );
		Select = new GUIContent ( "Select" );;
		UnSelect = new GUIContent ( "UnSelect" );;
		BuySelect = new GUIContent ( "Buy Select" );;
		BuyUnSelect = new GUIContent ( "Buy UnSelect" );;

		ItemName = serializedObject.FindProperty("ItemName");
		Price = serializedObject.FindProperty("Price");

		ColorConfirm = serializedObject.FindProperty("ColorConfirm");
		ColorSelected = serializedObject.FindProperty("ColorSelected");
		ColorUnSelected = serializedObject.FindProperty("ColorUnSelected");
		BoughtColorSelected = serializedObject.FindProperty("BoughtColorSelected");
		BoughtColorUnSelected = serializedObject.FindProperty("BoughtColorUnSelected");

		SpriteConfirm = serializedObject.FindProperty("SpriteConfirm");
		SpriteSelected = serializedObject.FindProperty("SpriteSelected");
		SpriteUnselected = serializedObject.FindProperty("SpriteUnselected");
		BoughtSpriteSelected = serializedObject.FindProperty("BoughtSpriteSelected");
		BoughtSpriteUnselected = serializedObject.FindProperty("BoughtSpriteUnselected");

		RightItem = serializedObject.FindProperty("RightItem");
		LeftItem = serializedObject.FindProperty("LeftItem");
		UpItem = serializedObject.FindProperty("UpItem");
		DownItem = serializedObject.FindProperty("DownItem");

		SpecAction = serializedObject.FindProperty("SpecAction");
		SlowMotion = serializedObject.FindProperty("SlowMotion");
		SpeedSlowMot = serializedObject.FindProperty("SpeedSlowMot");
		SpeedDeacSM = serializedObject.FindProperty("SpeedDeacSM");
		ReduceSlider = serializedObject.FindProperty("ReduceSlider");
		RecovSlider = serializedObject.FindProperty("RecovSlider");
	}

	public override void OnInspectorGUI()
	{
		EditorGUILayout.LabelField("Inspector Item Info", EditorStyles.boldLabel);
		ItemModif myTarget = (ItemModif)target;

		serializedObject.Update ( );
		EditorGUILayout.PropertyField ( ItemName );
		EditorGUILayout.PropertyField ( Price );

		myTarget.CatName = myTarget.transform.parent.GetComponent<CatShop> ( ).NameCat;

		// Si l'item est acheté
		if ( AllPlayerPrefs.GetBoolValue ( Constants.ItemBought + myTarget.ItemName ) )
		{
			myTarget.ItemBought = true;
		}
		else
		{
			myTarget.ItemBought = false;
		}
		EditorGUILayout.Space ( );

		// verification de l'utilisation de la couleur
		var buttonStyle = new GUIStyle(EditorStyles.miniButtonLeft);
		if ( myTarget.UseColor )
		{
			buttonStyle.normal.textColor = Color.green;
		}
		else
		{
			buttonStyle.normal.textColor = Color.red;
		}

		EditorGUILayout.BeginHorizontal();
		// Bouton d'utilisation de la couleur
		if ( GUILayout.Button ( "UseColor", buttonStyle ) )
		{
			myTarget.UseColor = !myTarget.UseColor;
		}

		// verification de l'utilisation de sprite
		buttonStyle = new GUIStyle(EditorStyles.miniButtonRight);
		if ( myTarget.UseSprite )
		{
			buttonStyle.normal.textColor = Color.green;
		}
		else
		{
			buttonStyle.normal.textColor = Color.red;
		}

		// Bouton d'utilisation de sprite
		if ( GUILayout.Button ( "UseSprite", buttonStyle ) )
		{
			myTarget.UseSprite = !myTarget.UseSprite;
		}
		EditorGUILayout.EndHorizontal ( );

		// Ajout des différentes couleurs
		if ( myTarget.UseColor )
		{
			EditorGUI.indentLevel = 1;

			EditorGUILayout.Space ( );
			EditorGUILayout.BeginHorizontal();

			buttonStyle = new GUIStyle(EditorStyles.miniButton);
			if ( myTarget.UseOtherColor )
			{
				buttonStyle.normal.textColor = Color.green;
			}
			else
			{
				buttonStyle.normal.textColor = Color.red;
			}

			if ( GUILayout.Button ( "UseOtherColor", buttonStyle ) )
			{
				myTarget.UseOtherColor = !myTarget.UseOtherColor;
			}

			if ( myTarget.UseSprite )
			{
				EditorGUILayout.Space ( );

				buttonStyle = new GUIStyle(EditorStyles.miniButton);
				if ( myTarget.UseOtherSprite )
				{
					buttonStyle.normal.textColor = Color.green;
				}
				else
				{
					buttonStyle.normal.textColor = Color.red;
				}

				if ( GUILayout.Button ( "UseOtherSprite", buttonStyle ) )
				{
					myTarget.UseOtherSprite = !myTarget.UseOtherSprite;
				}
			}
			EditorGUILayout.EndHorizontal ( );

			EditorGUILayout.Space ( );
			EditorGUILayout.LabelField ( "Color Information", EditorStyles.boldLabel );

			EditorGUILayout.PropertyField ( ColorConfirm, Confirm );
			EditorGUILayout.PropertyField ( ColorSelected, Select );
			EditorGUILayout.PropertyField ( ColorUnSelected, UnSelect );

			if ( myTarget.UseOtherColor )
			{
				EditorGUI.indentLevel = 2;

				EditorGUILayout.Space ( );

				EditorGUILayout.PropertyField ( BoughtColorSelected, BuySelect );
				EditorGUILayout.PropertyField ( BoughtColorUnSelected, BuyUnSelect );

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
			EditorGUI.indentLevel = 1;

			EditorGUILayout.Space ( );

			if ( !myTarget.UseColor )
			{
				buttonStyle = new GUIStyle(EditorStyles.miniButton);
				if ( myTarget.UseOtherSprite )
				{
					buttonStyle.normal.textColor = Color.green;
				}
				else
				{
					buttonStyle.normal.textColor = Color.red;
				}

				if ( GUILayout.Button ( "UseOtherSprite", buttonStyle ) )
				{
					myTarget.UseOtherSprite = !myTarget.UseOtherSprite;
				}
				EditorGUILayout.Space ( );
			}

			EditorGUILayout.LabelField ( "Sprite Information", EditorStyles.boldLabel );

			EditorGUILayout.PropertyField ( SpriteConfirm, Confirm );
			EditorGUILayout.PropertyField ( SpriteSelected, Select );
			EditorGUILayout.PropertyField ( SpriteUnselected, UnSelect );

			if ( myTarget.UseOtherSprite )
			{
				EditorGUI.indentLevel = 2;

				EditorGUILayout.Space ( );

				EditorGUILayout.PropertyField ( BoughtSpriteSelected, BuySelect );
				EditorGUILayout.PropertyField ( BoughtSpriteUnselected, BuyUnSelect );

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

		EditorGUI.indentLevel = 0;

		EditorGUILayout.LabelField("Around Information", EditorStyles.boldLabel);
		EditorGUILayout.PropertyField ( RightItem );
		EditorGUILayout.PropertyField ( LeftItem );
		EditorGUILayout.PropertyField ( UpItem );
		EditorGUILayout.PropertyField ( DownItem );

		EditorGUILayout.Space ( );
		EditorGUILayout.LabelField("Modification", EditorStyles.boldLabel);

		EditorGUILayout.BeginHorizontal();

		buttonStyle = new GUIStyle(EditorStyles.miniButton);
		if ( myTarget.ModifVie )
		{
			buttonStyle.normal.textColor = Color.green;
		}
		else
		{
			buttonStyle.normal.textColor = Color.red;
		}

		if ( GUILayout.Button ( "ModifVie", buttonStyle ) )
		{
			myTarget.ModifVie = !myTarget.ModifVie;
		}

		if ( myTarget.ModifVie )
		{
			myTarget.NombreVie = EditorGUILayout.IntField ( "NombreVie", myTarget.NombreVie );

			if ( myTarget.NombreVie <= 0 )
			{
				myTarget.NombreVie = 1;
			}
		}
		EditorGUILayout.EndHorizontal ( );

		buttonStyle = new GUIStyle(EditorStyles.miniButton);
		if ( myTarget.ModifSpecial )
		{
			buttonStyle.normal.textColor = Color.green;
		}
		else
		{
			buttonStyle.normal.textColor = Color.red;
		}

		if ( GUILayout.Button ( "ModifSpecial", buttonStyle ) )
		{
			myTarget.ModifSpecial = !myTarget.ModifSpecial;
		}

		if ( myTarget.ModifSpecial )
		{
			EditorGUILayout.PropertyField ( SpecAction );
			EditorGUI.indentLevel = 1;

			if ( myTarget.SpecAction == SpecialAction.SlowMot )
			{
				EditorGUILayout.PropertyField ( SlowMotion );
				EditorGUILayout.PropertyField ( SpeedSlowMot );
				EditorGUILayout.PropertyField ( SpeedDeacSM );
				EditorGUILayout.PropertyField ( ReduceSlider );
				EditorGUILayout.PropertyField ( RecovSlider );
			}
		}
		serializedObject.ApplyModifiedProperties ( );
	
		EditorGUI.indentLevel = 0;

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

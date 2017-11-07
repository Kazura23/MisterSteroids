using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;

//[CustomPropertyDrawer (typeof(CatShop))]
public class Drawer : PropertyDrawer 
{
	public override float GetPropertyHeight ( SerializedProperty property, GUIContent contents )
	{
		return base.GetPropertyHeight ( property, contents ) * 2;
	}

	public override void OnGUI (Rect position, SerializedProperty property, GUIContent contents)
	{
		EditorGUI.BeginProperty ( position, contents, property );

		float xMin = position.xMin;
		float xMax = position.xMax;

		float yMin = position.yMin;
		float yMax = position.yMax;

		float w = position.width;
		float h = position.height;

		//Rect labelRect = new Rect ( xMin, yMin, w * 0.5f, h );
		//EditorGUI.LabelField ( labelRect, "Ceci est un label de test" );
		//EditorGUI.FloatField ( labelRect, property.Time );
		EditorGUI.EndProperty ( );
	}

}

using UnityEditor;
using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class WindowSearchObject : EditorWindow
{
	ResearcheType thisType;
	string thisStringSearch;
	string SpecificPath;
	int thisNbr;

	int aPageProj;
	List <int> bPageProj;
	bool childProj;

	int aPageScene;
	List <int> bPageScene;
	bool childScene;

	int aPagePref;

	bool getChildren;
	Object objComp;
	GameObject thispref;
	Vector2 scrollPosProj;
	Vector2 scrollPosScene;
	Vector2 scrollPosPref;

	List<List<GameObject>> AllObjectProject;
	List<List<GameObject>> AllObjectScene;
	List<GameObject> InfoOnPrefab;

	void OnEnable ()
	{
		thisType = ResearcheType.Tag;

		thisStringSearch = string.Empty;
		SpecificPath = string.Empty;

		getChildren = true;
		childProj = true;
		childScene = true;

		objComp = null;
		thispref = null;

		thisNbr = 0;
		aPageProj = 0;
		aPageScene = 0;
		aPagePref = 0;

		scrollPosProj = Vector2.zero;
		scrollPosScene = Vector2.zero;
		scrollPosPref = Vector2.zero;

		bPageProj = new List<int> ( );
		bPageScene = new List<int> ( );

		AllObjectProject = new List<List<GameObject>> ( );
		AllObjectScene = new List<List<GameObject>> ( );
		InfoOnPrefab = new List<GameObject> ( );
	}
	// Add menu item named "My Window" to the Window menu
	[MenuItem("Window/Custom/SearchTags")]
	public static void ShowWindow()
	{
		//Show existing window instance. If one doesn't exist, make one.
		EditorWindow.GetWindow(typeof(WindowSearchObject));
	}

	void OnGUI()
	{
		List<List<GameObject>> getAllOnProj = AllObjectProject;
		List<List<GameObject>> getAllOnScene = AllObjectScene;
		GameObject[] getAllOnPrefab = InfoOnPrefab.ToArray ( );
		List<int> bProj = bPageProj;
		List<int> bScene = bPageScene;

		int a; 
		int b;
		GUILayout.Label ("Get Specific object", EditorStyles.boldLabel);

		EditorGUILayout.BeginHorizontal();
		thisType = (ResearcheType)EditorGUILayout.EnumPopup("ResearchType:", thisType);

		switch (thisType) 
		{
		case ResearcheType.Tag:
			thisStringSearch = EditorGUILayout.TagField ( "Search This Tag :", thisStringSearch );
			break;
		case ResearcheType.Layer:
			thisNbr = EditorGUILayout.LayerField ( "Search This Number Layer :", thisNbr );
			thisStringSearch = thisNbr.ToString ( );
			break;
		case ResearcheType.Name:
			thisStringSearch = EditorGUILayout.TextField ( "Search This Name :", thisStringSearch );
			break;
		case ResearcheType.Component:
			objComp = EditorGUILayout.ObjectField ( "This component", objComp, typeof( Object ), true );
			break;
		}
		EditorGUILayout.EndHorizontal();

		var buttonStyle = new GUIStyle( EditorStyles.miniButton );

		if ( getChildren )
		{
			buttonStyle.normal.textColor = Color.green;
		}
		else
		{
			buttonStyle.normal.textColor = Color.red;
		}

		if ( GUILayout.Button ( "Search On Childrend", buttonStyle ) )
		{
			getChildren = !getChildren;
		}

		if ( GUILayout.Button ( "Object On Scene" ) )
		{
			aPageScene = 0;
			bPageScene = new List<int> ( );
			bScene = bPageScene;
			childScene = getChildren;

			AllObjectScene = SearchObject.LoadAssetOnScenes ( thisType, objComp, thisStringSearch, getChildren );
			getAllOnScene = AllObjectScene;

			for ( a = 0; a < getAllOnScene.Count; a++ )
			{
				bScene.Add ( 0 );
			}
		}

		EditorGUILayout.BeginHorizontal();
		if ( GUILayout.Button ( "Object On Project" ) )
		{
			bPageProj = new List<int> ( );
			bProj = bPageProj;
			aPageProj = 0;
			childProj = getChildren;
			AllObjectProject = SearchObject.LoadAssetsInProject ( thisType, objComp, thisStringSearch, getChildren, SpecificPath );
			getAllOnProj = AllObjectProject;

			for ( a = 0; a < getAllOnProj.Count; a++ )
			{
				bProj.Add ( 0 );
			}
		}

		EditorGUILayout.TextField ("On Specific folder :", SpecificPath);
		EditorGUILayout.EndHorizontal();

		EditorGUILayout.BeginHorizontal();
		if ( GUILayout.Button ( "Object On Prefabs" ) && thispref != null )
		{
			aPagePref = 0;

			InfoOnPrefab = SearchObject.LoadOnPrefab ( thisType, objComp, thispref, thisStringSearch, getChildren );
			getAllOnPrefab = InfoOnPrefab.ToArray ( );
		}

		thispref = (GameObject) EditorGUILayout.ObjectField ( "This component", thispref, typeof( GameObject ), true );
		EditorGUILayout.EndHorizontal();

		EditorGUILayout.Space ( );
		EditorGUILayout.Space ( );

		#region Scene Layout
		EditorGUILayout.BeginHorizontal();
		scrollPosScene = EditorGUILayout.BeginScrollView ( scrollPosScene );

		if ( getAllOnScene.Count > 10 )
		{
			EditorGUILayout.BeginHorizontal();
			EditorGUILayout.PrefixLabel( "Page : " + (getAllOnScene.Count / 10).ToString() );
			aPageScene = EditorGUILayout.IntSlider ( aPageScene, 0, getAllOnScene.Count / 10 );
			EditorGUILayout.EndHorizontal();
		}

		for ( a = aPageScene * 10; a < 10 * ( aPageScene + 1 ); a++ )
		{
			if ( a >= getAllOnScene.Count )
			{
				break;
			}

			EditorGUI.indentLevel = 0;
			EditorGUILayout.TextField ( getAllOnScene [ a ][0].name );

			if ( childScene )
			{
				EditorGUI.indentLevel = 2;
			}

			EditorGUILayout.BeginVertical ( );

			if ( getAllOnScene[a].Count > 10 )
			{
				EditorGUILayout.BeginHorizontal();
				EditorGUILayout.PrefixLabel( "Page : " + (getAllOnScene[a].Count / 10).ToString() );
				bScene[a] = EditorGUILayout.IntSlider ( bScene[a], 0, getAllOnScene[a].Count / 10 );
				EditorGUILayout.EndHorizontal();
			}
			else
			{
				bScene[a] = 0;
			}

			for ( b = bScene[a] * 10 + 1; b < 10 * ( bScene[a] + 1 ) + 1; b++ )
			{
				if ( b >= getAllOnScene[a].Count)
				{
					break;
				}

				EditorGUILayout.TextField ( getAllOnScene [ a ] [ b ].name );
			}
			EditorGUILayout.EndVertical ( );
		}
		EditorGUILayout.EndScrollView ( );
		#endregion

		#region Project layout
		scrollPosProj =	EditorGUILayout.BeginScrollView ( scrollPosProj );
		EditorGUILayout.BeginVertical ( );

		if ( getAllOnProj.Count > 10 )
		{
			EditorGUILayout.BeginHorizontal();
			EditorGUILayout.PrefixLabel( "Page : " + (getAllOnProj.Count / 10).ToString() );
			aPageProj = EditorGUILayout.IntSlider ( aPageProj, 0, getAllOnProj.Count / 10 );
			EditorGUILayout.EndHorizontal();
		}

		for ( a = aPageProj * 10; a < 10 * ( aPageProj + 1 ); a++ )
		{
			if ( a >= getAllOnProj.Count )
			{
				break;
			}

			EditorGUI.indentLevel = 0;
			EditorGUILayout.TextField ( getAllOnProj [ a ][0].name );

			if ( childProj )
			{
				EditorGUI.indentLevel = 2;
			}

			EditorGUILayout.BeginVertical ( );

			if ( getAllOnProj[a].Count > 10 )
			{
				EditorGUILayout.BeginHorizontal();
				EditorGUILayout.PrefixLabel( "Page : " + (getAllOnProj[a].Count / 10).ToString() );
				bProj[a] = EditorGUILayout.IntSlider ( bProj[a], 0, getAllOnProj[a].Count / 10 );
				EditorGUILayout.EndHorizontal();
			}
			else
			{
				bProj[a] = 0;
			}

			for ( b = bProj[a] * 10 + 1; b < 10 * ( bProj[a] + 1 ) + 1; b++ )
			{
				if ( b >= getAllOnProj[a].Count)
				{
					break;
				}

				EditorGUILayout.TextField ( getAllOnProj [ a ] [ b ].name );
			}
			EditorGUILayout.EndVertical ( );
		}
		EditorGUILayout.EndVertical ( );
		EditorGUILayout.EndScrollView ( );
		#endregion

		#region Scene Layout

		scrollPosPref =	EditorGUILayout.BeginScrollView(scrollPosPref);
		for ( a = 0; a < getAllOnPrefab.Length; a++ )
		{
			EditorGUI.indentLevel = 0;
			EditorGUILayout.TextField ( getAllOnPrefab [ a ].name );
		}
		EditorGUILayout.EndScrollView ( );
		#endregion

		EditorGUILayout.EndHorizontal();

		//  / foldout / essaie d'associer l'object au prefab / gameobject en scene
	}

	IEnumerator waitDisplay ( )
	{
		yield break;
	}
}


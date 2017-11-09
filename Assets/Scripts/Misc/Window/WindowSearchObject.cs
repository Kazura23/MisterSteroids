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
	List<bool> foldoutProj;
	List <int> bPageProj;
	bool childProj;

	int aPageScene;
	List <int> bPageScene;
	List<bool> foldoutScene;
	bool childScene;

	int aPagePref;
	List <int> bPagePref;
	List<bool> foldoutPref;
	bool childPref;

	bool getChildren;
	bool foldListPref;

	Object objComp;
	List<GameObject> thispref;
	Vector2 scrollPosProj;
	Vector2 scrollPosScene;
	Vector2 scrollPosPref;

	List<List<GameObject>> AllObjectProject;
	List<List<GameObject>> AllObjectScene;
	List<List<GameObject>> InfoOnPrefab;

	void OnEnable ()
	{
		thisType = ResearcheType.Tag;

		thisStringSearch = string.Empty;
		SpecificPath = string.Empty;

		objComp = null;
		thisNbr = 0;
		getChildren = true;

		childProj = true;
		childScene = true;
		childPref = true;
		foldListPref = true;

		aPageProj = 0;
		aPageScene = 0;
		aPagePref = 0;

		scrollPosProj = Vector2.zero;
		scrollPosScene = Vector2.zero;
		scrollPosPref = Vector2.zero;

		bPageProj = new List<int> ( );
		bPageScene = new List<int> ( );
		bPagePref = new List<int> ( );

		foldoutProj = new List<bool> ( );
		foldoutScene = new List<bool> ( );
		foldoutPref = new List<bool> ( );

		AllObjectProject = new List<List<GameObject>> ( );
		AllObjectScene = new List<List<GameObject>> ( );
		InfoOnPrefab = new List<List<GameObject>> ( );
		thispref = new List<GameObject> ( );
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
		List<List<GameObject>> getAllOnPrefab = InfoOnPrefab;
		List<int> bProj = bPageProj;
		List<int> bScene = bPageScene;
		List<int> bPref = bPagePref;

		List<bool> fPref = foldoutPref;
		List<bool> fScene = foldoutScene;
		List<bool> fProj = foldoutProj;


		int a; 
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
			fScene = foldoutScene;
			childScene = getChildren;

			AllObjectScene = SearchObject.LoadAssetOnScenes ( thisType, objComp, thisStringSearch, getChildren );
			getAllOnScene = AllObjectScene;

			for ( a = 0; a < getAllOnScene.Count; a++ )
			{
				bScene.Add ( 0 );
				fScene.Add ( false );
			}
		}

		EditorGUILayout.BeginHorizontal();
		if ( GUILayout.Button ( "Object On Project" ) )
		{
			bPageProj = new List<int> ( );
			foldoutProj = new List<bool> ( );
			fProj = foldoutProj;
			bProj = bPageProj;

			aPageProj = 0;
			childProj = getChildren;
			AllObjectProject = SearchObject.LoadAssetsInProject ( thisType, objComp, thisStringSearch, getChildren, SpecificPath );
			getAllOnProj = AllObjectProject;

			for ( a = 0; a < getAllOnProj.Count; a++ )
			{
				bProj.Add ( 0 );
				fProj.Add ( false );
			}
		}

		EditorGUILayout.TextField ("On Specific folder :", SpecificPath);
		EditorGUILayout.EndHorizontal();

		EditorGUILayout.BeginHorizontal();
		if ( GUILayout.Button ( "Object On Prefabs" ) && thispref != null )
		{
			aPagePref = 0;
			bPagePref = new List<int> ( );
			bPref = bPagePref;
			fPref = foldoutPref;
			childPref = getChildren;

			InfoOnPrefab = SearchObject.LoadOnPrefab ( thisType, objComp, thispref, thisStringSearch, getChildren );
			getAllOnPrefab = InfoOnPrefab;

			for ( a = 0; a < getAllOnPrefab.Count; a++ )
			{
				bPref.Add ( 0 );
				fPref.Add ( false );
			}
		}

		var list = thispref;
		int newCount = Mathf.Max(0, EditorGUILayout.IntField("Number Ref", list.Count));
		while ( newCount < list.Count )
		{
			list.RemoveAt( list.Count - 1 );
		}

		while ( newCount > list.Count )
		{
			list.Add ( null );
		}

		EditorGUILayout.BeginVertical ( );
		if ( thispref.Count > 0 )
		{
			foldListPref = EditorGUILayout.Foldout ( foldListPref, "Object List"  );
		}

		if ( foldListPref )
		{
			for( a = 0; a < thispref.Count; a++)
			{
				thispref [ a ] = ( GameObject ) EditorGUILayout.ObjectField ( "This component", thispref [ a ], typeof( GameObject ), true );
			}
		}

		EditorGUILayout.EndVertical ( );

		EditorGUILayout.EndHorizontal();

		EditorGUILayout.Space ( );
		EditorGUILayout.Space ( );

		EditorGUILayout.BeginHorizontal ( );
		#region Scene Layout
		scrollPosScene = EditorGUILayout.BeginScrollView ( scrollPosScene );
		LayoutSearch( getAllOnScene, bScene, fScene );
		EditorGUILayout.EndScrollView ( );
		#endregion

		#region Project layout
		scrollPosProj = EditorGUILayout.BeginScrollView ( scrollPosProj );
		LayoutSearch( getAllOnProj, bProj, fProj );
		EditorGUILayout.EndScrollView ( );
		#endregion

		#region Pref Layout
		scrollPosPref = EditorGUILayout.BeginScrollView ( scrollPosPref );
		LayoutSearch( getAllOnPrefab, bPref, fPref );
		EditorGUILayout.EndScrollView ( );
		#endregion
		EditorGUILayout.EndHorizontal();

		//  / foldout / essaie d'associer l'object au prefab / gameobject en scene
	}


	void LayoutSearch ( List<List<GameObject>> listSearch, List<int> bPage, List<bool> fDout )
	{
		int a; 
		int b;

		if ( listSearch.Count > 10 )
		{
			EditorGUILayout.BeginHorizontal();
			EditorGUILayout.PrefixLabel( "Page : " + (listSearch.Count / 10).ToString() );
			aPageScene = EditorGUILayout.IntSlider ( aPageScene, 0, listSearch.Count / 10 );
			EditorGUILayout.EndHorizontal();
		}

		for ( a = aPageScene * 10; a < 10 * ( aPageScene + 1 ); a++ )
		{
			if ( a >= listSearch.Count )
			{
				break;
			}

			EditorGUI.indentLevel = 0;
			EditorGUILayout.TextField ( listSearch [ a ][0].name );

			if ( childScene )
			{
				if ( listSearch [ a ].Count > 1 )
				{
					EditorGUI.indentLevel = 1;

					fDout [ a ] = EditorGUILayout.Foldout ( fDout [ a ], "Display Children : " + ( listSearch [ a ].Count - 1 ).ToString ( ) );
				}

				EditorGUI.indentLevel = 2;
			}

			if ( fDout [ a ] )
			{
				EditorGUILayout.BeginVertical ( );

				if ( listSearch[a].Count > 10 )
				{
					EditorGUILayout.BeginHorizontal();
					EditorGUILayout.PrefixLabel( "Page : " + (listSearch[a].Count / 10).ToString() );
					bPage[a] = EditorGUILayout.IntSlider ( bPage[a], 0, listSearch[a].Count / 10 );
					EditorGUILayout.EndHorizontal();
				}
				else
				{
					bPage[a] = 0;
				}

				for ( b = bPage[a] * 10 + 1; b < 10 * ( bPage[a] + 1 ) + 1; b++ )
				{
					if ( b >= listSearch[a].Count)
					{
						break;
					}

					EditorGUILayout.TextField ( listSearch [ a ] [ b ].name );
				}
				EditorGUILayout.EndVertical ( );
			}

			EditorGUILayout.Space ( );
			EditorGUILayout.Space ( );
		}
	}
}


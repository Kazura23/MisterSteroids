using UnityEditor;
using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class SearchTags : EditorWindow
{
	ResearcheType thisType;
	string thisStringSearch;
	string SpecificPath;
	bool allScene;
	bool getChildren;
	public Object objComp;
	void OnEnable ()
	{
		thisStringSearch = string.Empty;
		SpecificPath = string.Empty;
		allScene = false;
		getChildren = true;
		thisType = ResearcheType.Tag;
	}
	// Add menu item named "My Window" to the Window menu
	[MenuItem("Window/Custom/SearchTags")]
	public static void ShowWindow()
	{
		//Show existing window instance. If one doesn't exist, make one.
		EditorWindow.GetWindow(typeof(SearchTags));
	}

	void OnGUI()
	{
		List<List<GameObject>> AllObjectProject;
		List<List<GameObject>> AllObjectScene;

		GUILayout.Label ("Get Specific object", EditorStyles.boldLabel);

		EditorGUILayout.BeginHorizontal();
		thisType = (ResearcheType)EditorGUILayout.EnumPopup("ResearchType:", thisType);

		switch (thisType) 
		{
		case ResearcheType.Tag:
			EditorGUILayout.TextField ("Search This Tag :", thisStringSearch);
			break;
		case ResearcheType.Layer:
			EditorGUILayout.TextField ("Search This Number Layer :", thisStringSearch);
			break;
		case ResearcheType.Name:
			EditorGUILayout.TextField ("Search This Name :", thisStringSearch);
			break;
		case ResearcheType.Component:
			objComp = EditorGUILayout.ObjectField("This component",objComp, typeof(Object), true);
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

		EditorGUILayout.BeginHorizontal();
		if ( GUILayout.Button ( "Object On Project" ) )
		{
			AllObjectProject = LoadAllAssetsOfType (  );
		}

		EditorGUILayout.TextField ("On Specific folder :", SpecificPath);
		EditorGUILayout.EndHorizontal();

		EditorGUILayout.BeginHorizontal();
		if ( GUILayout.Button ( "Object On Scene" ) )
		{
			AllObjectScene = LoadGameObjectScene ( );
		}

		allScene = EditorGUILayout.Toggle ("Search On All Scene :", allScene);
		EditorGUILayout.EndHorizontal();
	}

	public List<List<GameObject>> LoadAllAssetsOfType(string optionalPath = "")
	{
		string currTag = thisStringSearch;
		string[] GUIDs;
		if(optionalPath != "")
		{
			if(optionalPath.EndsWith("/"))
			{
				optionalPath = optionalPath.TrimEnd('/');
			}
			GUIDs = AssetDatabase.FindAssets("t:GameObject",new string[] { optionalPath });
		}
		else
		{
			GUIDs = AssetDatabase.FindAssets("t:GameObject");
		}

		List<List<GameObject>> objectList = new List<List<GameObject>> ( );
		int getNbr = 0;
		string guid;
		string assetPath;
		GameObject asset;

		for (int a = 0; a < GUIDs.Length; a++)
		{
			guid = GUIDs[a];
			assetPath = AssetDatabase.GUIDToAssetPath(guid);
			asset = AssetDatabase.LoadAssetAtPath(assetPath, typeof(GameObject)) as GameObject;
			if ( getChildren )
			{
				objectList.Add ( returnCurrObj ( asset.GetComponentsInChildren<GameObject> ( ) ) );
			}
			else
			{
				objectList.Add ( new List<GameObject> ( ) );

				switch (thisType) 
				{
				case ResearcheType.Tag:
					if ( thisStringSearch == string.Empty || asset.tag == thisStringSearch )
					{
						objectList [ a ].Add ( asset );
					}
					break;
				case ResearcheType.Name:
					if ( asset.name == thisStringSearch )
					{
						objectList [ a ].Add ( asset );
					}
					break;
				case ResearcheType.Layer:
					if ( asset.layer == int.Parse ( thisStringSearch ) )
					{
						objectList [ a ].Add ( asset );
					}
					break;
				case ResearcheType.Component:
					if ( asset.GetComponent ( objComp.GetType ( ) ) != null )
					{
						objectList [ a ].Add ( asset );
					}
					break;
				}
			}
		}

		return objectList;
	}

	public List<List<GameObject>> LoadGameObjectScene ( )
	{
		GameObject[] objectList;
		List<List<GameObject>> objTagList = new List<List<GameObject>> ( );
		int a;
		int b;

		if ( allScene )
		{
			for ( a = 0; a < UnityEngine.SceneManagement.SceneManager.sceneCountInBuildSettings; a++ )
			{
				objectList = UnityEngine.SceneManagement.SceneManager.GetSceneByBuildIndex ( a ).GetRootGameObjects ( );
				objTagList.Add ( returnCurrObj ( objectList ) );
			}
		}
		else
		{
			objectList = UnityEngine.SceneManagement.SceneManager.GetActiveScene ( ).GetRootGameObjects ( );
			objTagList.Add ( returnCurrObj ( objectList ) );
		}

		return objTagList;
	}

	List<GameObject> returnCurrObj ( GameObject[] objectList )
	{
		List <GameObject> objTagList = new List<GameObject> ( );
		string getSearch = thisStringSearch;
		int a;
		int b;

		for ( a = 0; a < objectList.Length; a++ )
		{
			if ( getChildren )
			{
				foreach (GameObject thisChild in objectList[a].GetComponentsInChildren<GameObject>(true))
				{
					switch (thisType) 
					{
					case ResearcheType.Tag:
						if ( getSearch == string.Empty || thisChild.tag == getSearch )
						{
							objTagList.Add ( thisChild );
						}
						break;
					case ResearcheType.Name:
						if ( thisChild.name == getSearch )
						{
							objTagList.Add ( thisChild );
						}
						break;
					case ResearcheType.Layer:
						if ( thisChild.layer == int.Parse ( getSearch ) )
						{
							objTagList.Add ( thisChild );
						}
						break;
					case ResearcheType.Component:
						if ( thisChild.GetComponent ( objComp.GetType ( ) ) != null )
						{
							objTagList.Add ( thisChild );
						}
						break;
					}
				}
			}
			else
			{
				switch (thisType) 
				{
				case ResearcheType.Tag:
					if ( getSearch == string.Empty || objectList[a].tag == getSearch )
					{
						objTagList.Add ( objectList [ a ] );
					}
					break;
				case ResearcheType.Name:
					if ( objectList[a].name == getSearch )
					{
						objTagList.Add ( objectList [ a ] );
					}
					break;
				case ResearcheType.Layer:
					if ( objectList[a].layer == int.Parse ( getSearch ) )
					{
						objTagList.Add ( objectList [ a ] );
					}
					break;
				case ResearcheType.Component:
					if ( objectList[a].GetComponent ( objComp.GetType ( ) ) != null )
					{
						objTagList.Add ( objectList [ a ] );
					}
					break;
				}
			}
		}

		return objTagList;
	}
}
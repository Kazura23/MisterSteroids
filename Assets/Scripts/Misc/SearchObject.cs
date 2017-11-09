using UnityEditor;
using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class SearchObject : MonoBehaviour 
{
	#region Variables
	#endregion

	#region Mono
	#endregion

	#region Public Methods
	public static List<List<GameObject>> LoadAssetsInProject(ResearcheType thisType, Object objComp, string thisStringSearch, bool getChildren, string optionalPath = "")
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
		List<GameObject> getObj = new List<GameObject> ( );
		List<GameObject> asset = new List<GameObject> ( ); 

		string guid;
		string assetPath;
		int a;

		for ( a = 0; a < GUIDs.Length; a++ )
		{
			guid = GUIDs [ a ];
			assetPath = AssetDatabase.GUIDToAssetPath ( guid );
			asset.Add ( AssetDatabase.LoadAssetAtPath ( assetPath, typeof( GameObject ) ) as GameObject );
		}

		getObj = returnCurrObj ( asset.ToArray ( ), thisType, objComp, thisStringSearch, false );

		if ( getChildren )
		{
			for ( a = 0; a < getObj.Count; a++ )
			{
				objectList.Add ( new List<GameObject> ( ) );
				objectList [ a ] = returnCurrObj ( GetComponentsInChildrenOfAsset ( getObj [ a ] ), thisType, objComp, thisStringSearch, true );
			}
		}
		else if ( getObj.Count > 0 )
		{
			objectList.Add ( getObj );
		}

		return objectList;
	}

	public static List<List<GameObject>> LoadAssetOnScenes ( ResearcheType thisType, Object objComp, string thisStringSearch, bool getChildren )
	{
		GameObject[] objectList = UnityEngine.SceneManagement.SceneManager.GetActiveScene ( ).GetRootGameObjects ( );
		List<List<GameObject>> getAllObj = new List<List<GameObject>> ( );
		List<GameObject> getObj = returnCurrObj ( objectList, thisType, objComp, thisStringSearch, false );

		if ( getChildren )
		{
			for ( int a = 0; a < getObj.Count; a ++)
			{
				getAllObj.Add ( new List<GameObject> ( ) );
				getAllObj [ a ] = returnCurrObj ( GetComponentsInChildrenOfAsset ( getObj [ a ] ), thisType, objComp, thisStringSearch, true );
			}
		}
		else if ( getObj.Count > 0 )
		{
			getAllObj.Add ( getObj );
		}

		return getAllObj;
	}

	public static List<List<GameObject>> LoadOnPrefab ( ResearcheType thisType, Object objComp, List<GameObject> thisPref, string thisStringSearch, bool getChildren )
	{
		List<List<GameObject>> objectList = new List<List<GameObject>> ( );
		List<GameObject> getObj = new List<GameObject> ( );
		int a;

		getObj = returnCurrObj ( thisPref.ToArray ( ), thisType, objComp, thisStringSearch, false );

		if ( getChildren )
		{
			for ( a = 0; a < getObj.Count; a++ )
			{
				objectList.Add ( new List<GameObject> ( ) );
				objectList [ a ] = returnCurrObj ( GetComponentsInChildrenOfAsset ( getObj [ a ] ), thisType, objComp, thisStringSearch, true );
			}
		}
		else if ( getObj.Count > 0 )
		{
			objectList.Add ( getObj );
		}

		return objectList;
	
	}
	#endregion

	#region Private Methods
	static List<GameObject> returnCurrObj ( GameObject[] objectList, ResearcheType thisType, Object objComp, string thisStringSearch, bool getChildren )
	{
		List <GameObject> objTagList = new List<GameObject> ( );
		string getSearch = thisStringSearch;
		Component[] components;
		int a;
		int b;

		for ( a = 0; a < objectList.Length; a++ )
		{
			if ( objectList [ a ] == null )
			{
				continue;
			}

			if ( getChildren )
			{
				foreach ( GameObject thisChild in GetComponentsInChildrenOfAsset ( objectList[a] ) )
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
						components = thisChild.GetComponents<Component> ( );

						for ( b = 0; b < components.Length; b++ )
						{
							if ( components [ b ] != null && components [ b ].name == objComp.name )
							{
								objTagList.Add ( thisChild );
								break;
							}
						}
						break;
					case ResearcheType.MissingComp :
						components = thisChild.GetComponents<Component> ( );

						for ( b = 0; b < components.Length; b++ )
						{
							if ( !components [ b ] )
							{
								objTagList.Add ( thisChild );
								break;
							}
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
					components = objectList[a].GetComponents<Component> ( );

					for ( b = 0; b < components.Length; b++ )
					{
						if ( components [ b ] != null && components [ b ].name == objComp.name )
						{
							Debug.Log ( objComp );
							objTagList.Add ( objectList[a] );
							break;
						}
					}
					break;
				case ResearcheType.MissingComp :
					components = objectList[a].GetComponents<Component> ( );

					for ( b = 0; b < components.Length; b++ )
					{
						if ( !components [ b ] )
						{
							objTagList.Add ( objectList[a] );
							break;
						}
					}
					break;
				}
			}
		}

		return objTagList;
	}

	static GameObject[] GetComponentsInChildrenOfAsset( GameObject go )
	{
		List<GameObject> tfs = new List<GameObject>();
		CollectChildren( tfs, go.transform );

		return tfs.ToArray();
	}

	static void CollectChildren( List<GameObject> transforms, Transform tf)
	{
		transforms.Add(tf.gameObject);
		foreach(Transform child in tf)
		{
			CollectChildren(transforms, child);
		}
	}
	#endregion
}

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
		List<GameObject> asset = new List<GameObject> ( ); 
		List<GameObject> getObj;

		string guid;
		string assetPath;
		int a;

		for ( a = 0; a < GUIDs.Length; a++ )
		{
			guid = GUIDs [ a ];
			assetPath = AssetDatabase.GUIDToAssetPath ( guid );
			asset.Add ( AssetDatabase.LoadAssetAtPath ( assetPath, typeof( GameObject ) ) as GameObject );
		}

		if ( getChildren )
		{
			for ( a = 0; a < asset.Count; a++ )
			{
				getObj = returnCurrObj ( GetComponentsInChildrenOfAsset ( asset [ a ] ), thisType, objComp, thisStringSearch, true );

				if ( getObj.Count > 0 )
				{
					objectList.Add ( getObj );
				}
			}
		}
		else
		{
			getObj = returnCurrObj ( asset.ToArray ( ), thisType, objComp, thisStringSearch, false );

			if ( getObj.Count > 0 )
			{
				objectList.Add ( getObj );
			}
		}

		return objectList;
	}

	public static List<List<GameObject>> LoadAssetOnScenes ( ResearcheType thisType, Object objComp, string thisStringSearch, bool getChildren )
	{
		GameObject[] objectList = UnityEngine.SceneManagement.SceneManager.GetActiveScene ( ).GetRootGameObjects ( );
		List<List<GameObject>> getAllObj = new List<List<GameObject>> ( );
		List<GameObject> getObj;

		if ( getChildren )
		{
			for ( int a = 0; a < objectList.Length; a ++)
			{
				getObj = returnCurrObj ( GetComponentsInChildrenOfAsset ( objectList [ a ] ), thisType, objComp, thisStringSearch, true );

				if ( getObj.Count > 0 )
				{
					getAllObj.Add ( getObj );
				}
			}
		}
		else
		{
			getObj = returnCurrObj ( objectList, thisType, objComp, thisStringSearch, false );

			if ( getObj.Count > 0 )
			{
				getAllObj.Add ( getObj );
			}
		}

		return getAllObj;
	}

	public static List<List<GameObject>> LoadOnPrefab ( ResearcheType thisType, Object objComp, List<GameObject> thisPref, string thisStringSearch, bool getChildren  )
	{
		List<List<GameObject>> objectList = new List<List<GameObject>> ( );
		List<GameObject> getObj;
		int a;

		if ( getChildren )
		{
			for ( a = 0; a < thisPref.Count; a++ )
			{
				getObj = returnCurrObj ( GetComponentsInChildrenOfAsset ( thisPref [ a ] ), thisType, objComp, thisStringSearch, true );

				if ( getObj.Count > 0 )
				{
					objectList.Add ( getObj );
				}
			}
		}
		else 
		{
			getObj = returnCurrObj ( thisPref.ToArray ( ), thisType, objComp, thisStringSearch, false );

			if ( getObj.Count > 0 )
			{
				objectList.Add ( getObj );
			}
		}

		return objectList;
	
	}
	#endregion

	#region Private Methods
	static List<GameObject> returnCurrObj ( GameObject[] objectList, ResearcheType thisType, Object objComp, string thisStringSearch, bool getChildren  )
	{
		List <GameObject> objTagList = new List<GameObject> ( );
		Component [] components;
		Component [] componentsPref;

		GameObject getPref;

		if ( thisType == ResearcheType.SamePref )
		{
			if ( objComp == null )
			{
				return new List<GameObject> ( );
			}

			getPref = ( GameObject ) objComp;
			componentsPref = getPref.GetComponents<Component> ( );
		}
		else
		{
			getPref = objectList [ 0 ];
			componentsPref = getPref.GetComponents<Component> ( );
		}

		string getSearch = thisStringSearch;

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
				foreach (GameObject thisChild in GetComponentsInChildrenOfAsset ( objectList[a] ))
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

						if ( objComp == null )
						{
							return new List<GameObject> ( );
						}

						for ( b = 0; b < components.Length; b++ )
						{
							if ( components [ b ] != null && components [ b ].GetType ( ) == objComp.GetType ( ) )
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
					case ResearcheType.SamePref:
						components = thisChild.GetComponents<Component> ( );
						if ( (componentsPref.Length - components.Length  <= 2 ) && thisChild.name.Length >= getPref.name.Length && thisChild.name.Substring ( 0, getPref.name.Length ) == getPref.name  )
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
					components = objectList[a].GetComponents<Component> ( );

					if ( objComp == null )
					{
						return new List<GameObject> ( );
					}

					for ( b = 0; b < components.Length; b++ )
					{
						if ( components [ b ] != null && components [ b ].GetType ( ) == objComp.GetType ( )  )
						{
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
				case ResearcheType.SamePref:
					components = objectList [ a ].GetComponents<Component> ( );
					if ( (componentsPref.Length - components.Length  <= 2 ) && objectList [ a ].name.Length >= getPref.name.Length && objectList [ a ].name.Substring ( 0, getPref.name.Length ) == getPref.name )
					{
						objTagList.Add ( objectList [ a ] );
					}
					break;
				}
			}
		}

		return objTagList;
	}

	static GameObject[] GetComponentsInChildrenOfAsset( GameObject go  )
	{
		List<GameObject> tfs = new List<GameObject>();

		CollectChildren( tfs, go.transform );
		return tfs.ToArray();
	}

	static void CollectChildren( List<GameObject> transforms, Transform tf)
	{
		transforms.Add ( tf.gameObject );

		foreach(Transform child in tf)
		{
			CollectChildren ( transforms, child );
		}
	}
	#endregion
}

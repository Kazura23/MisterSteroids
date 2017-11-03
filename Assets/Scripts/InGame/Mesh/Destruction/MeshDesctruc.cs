using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeshDesctruc : MonoBehaviour 
{
	public GameObject TriangPrefb;
	public Material DebrisMaterial;
	public bool UseMatDeb = false;
//	public List<GameObject> stockElem;
	Transform garbage;

	void Start ( )
	{
		garbage = GlobalManager.GameCont.GarbageTransform;
		//stockElem = new List<GameObject> ( );
	}

	public IEnumerator SplitMesh ( GameObject objSource, float forcePro, float deleayDest, int lim = 10, bool little = false )    
	{
		WaitForEndOfFrame thisFrame = new WaitForEndOfFrame ( );

		if ( objSource.GetComponent<Collider> ( ) )
		{
			objSource.GetComponent<Collider>().enabled = false;
		}

		Mesh M = new Mesh ( );
		if ( objSource.GetComponent<MeshFilter> ( ) )
		{
			M = objSource.GetComponent<MeshFilter> ( ).mesh;
		}
		else if ( objSource.GetComponent<SkinnedMeshRenderer> ( ) )
		{
			M = objSource.GetComponent<SkinnedMeshRenderer> ( ).sharedMesh;
		}

		Material[] materials = new Material[0];
		if ( objSource.GetComponent<MeshRenderer> ( ) )
		{
			materials = objSource.GetComponent<MeshRenderer> ( ).materials;
		}
		else if ( objSource.GetComponent<SkinnedMeshRenderer> ( ) )
		{
			materials = objSource.GetComponent<SkinnedMeshRenderer> ( ).materials;
		}

		objSource.GetComponent<MeshRenderer> ( ).enabled = false;

		Vector3[] verts = M.vertices;
		Vector3[] normals = M.normals;
		Vector2[] uvs = M.uv;

		Vector3[] newVerts = new Vector3[9];
		Vector3[] newNormals = new Vector3[9];
		Vector2[] newUvs = new Vector2[9]; 

	//	List<GameObject> getAllSt;

		Transform getTrans = objSource.transform;
		Vector3 getSize = M.bounds.size;
		Vector3 calDir = getTrans.forward;
		GameObject GO;
		GameObject getTri = TriangPrefb;
		Mesh mesh;

		float matCD = calDir.magnitude / 4;

		int[] indices;

		int countTriangle;
		int index;
		int a;
		int b;
		int c;

		bool checkLim;
		getSize = new Vector2 ( getSize.x / 2, getSize.y / 2 );

		//getAllSt = stockElem;

		for ( a = 0; a < M.subMeshCount; a++ )
		{
			indices = M.GetTriangles ( a );
			countTriangle = 1;
			checkLim = false;

			while ( indices.Length / countTriangle > lim )
			{
				countTriangle += 3;
			}

			countTriangle--;

			for ( b = 0; b < indices.Length; b += 3 + countTriangle )
			{
				if ( b % 25 == 0 )
				{
					yield return thisFrame;
				}

				if ( objSource == null )
				{
					yield break;
				}

				for ( c = 0; c < 3; c++ )
				{
					if ( c + b > indices.Length )
					{
						checkLim = true;
						break;
					}

					index = indices[ b + c ];
					newVerts [ c ] = verts [ index ];
					newUvs [ c ] = uvs [ index ];
					newNormals [ c ] = normals [ index ];

					newUvs [ c + 3 ] = uvs [ index ];
					newNormals [ c + 3 ] = normals [ index ];

					newUvs [ c + 6 ] = uvs [ index ];
					newNormals [ c + 6 ] = normals [ index ];

					if ( !little )
					{
						newVerts [ c + 3 ] = new Vector3 ( -verts [ index ].y * Random.Range ( 0.5f, 1.2f ), -verts [ index ].x * Random.Range ( 0.5f, 1.2f ), -verts [ index ].z * Random.Range ( 0.5f, 1.2f ) );
						newVerts [ c + 6 ] = new Vector3 ( -verts [ index ].y * Random.Range ( 0.5f, 1.2f ), -verts [ index ].x * Random.Range ( 0.5f, 1.2f ), -verts [ index ].z * Random.Range ( 0.5f, 1.2f ) );
					}
					else
					{
						newVerts [ c + 3 ] = new Vector3 ( verts [ index ].x * Random.Range ( 0.5f, 1f ), verts [ index ].y * Random.Range ( 0.5f, 1f ), verts [ index ].z );
						newVerts [ c + 6 ] = new Vector3 ( -verts [ index ].x * Random.Range ( 0.1f, 0.5f ), -verts [ index ].y * Random.Range ( 0.1f, 0.5f ), verts [ index ].z );
					}
				}
					
				if ( checkLim )
				{
					break;
				}

				mesh = new Mesh ( );
				mesh.vertices = newVerts;
				mesh.normals = newNormals;
				mesh.uv = newUvs;
				mesh.triangles = new int[] 
				{
					0, 1, 2,

					1, 3, 2,
					0, 3, 1,
					0, 2, 3,

					0, 4, 1,
					1, 4, 2,
					0, 2, 4,

					2, 4, 1,
					1, 4, 3,
					2, 3, 4
				};

				/*if ( getAllSt.Count > 0 && !getAllSt [ 0 ].activeSelf )
				{
					GO = getAllSt [ 0 ];
					GO.SetActive ( true );

					getAllSt.RemoveAt ( 0 );
				}
				else
				{
					GO = ( GameObject ) Instantiate ( getTri );
					GO.transform.SetParent ( garbage );
				}*/
				GO = ( GameObject ) Instantiate ( getTri );
				GO.transform.SetParent ( garbage );
			
				if ( UseMatDeb )
				{
					GO.GetComponent<MeshRenderer> ( ).material = DebrisMaterial;
				}
				else
				{
					GO.GetComponent<MeshRenderer> ( ).material = materials [ a ];
				}

				GO.GetComponent<MeshFilter> ( ).mesh = mesh;

				GO.layer = LayerMask.NameToLayer ( "Particle" );
				GO.transform.position = getTrans.position;
				GO.transform.rotation = getTrans.rotation;

				GO.GetComponent<Rigidbody> ( ).AddForce ( ( new Vector3 ( Random.Range ( -matCD, matCD ), Random.Range ( 0, matCD ), Random.Range ( -matCD, matCD ) ) + calDir ) * Random.Range ( forcePro / 10, forcePro ), ForceMode.VelocityChange );

				GO.GetComponent<TimeToDisable> ( ).DisableThis ( deleayDest + Random.Range ( 0.0f, deleayDest ) );
			}
		}
			
		Destroy ( objSource );
	}

	/*public void ReAddObj ( GameObject thisObj )
	{
		if ( stockElem.Count > 100 )
		{
			Destroy ( thisObj );
		}
		else
		{
			stockElem.Add ( thisObj );
		}
	}*/
}

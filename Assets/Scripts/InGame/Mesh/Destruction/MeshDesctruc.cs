using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeshDesctruc : MonoBehaviour 
{
	public GameObject TriangPrefb;

	List<GameObject> stockElem;
	Transform garbage;

	void Start ( )
	{
		garbage = Manager.GameCont.GarbageTransform;
		stockElem = new List<GameObject> ( );
	}

	public IEnumerator SplitMesh ( Vector3 sourceCol, GameObject objSource )    
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

		Vector3[] newVerts = new Vector3[6];
		Vector3[] newNormals = new Vector3[6];
		Vector2[] newUvs = new Vector2[6]; 

		List<GameObject> getAllSt;

		Transform getTrans = objSource.transform;
		Vector3 explosionPos;
		Vector3 getSize = M.bounds.size;
		GameObject GO;
		GameObject getTri = TriangPrefb;
		Mesh mesh;

		int[] indices;

		int countTriangle;
		int index;
		int a;
		int b;
		int c;

		bool checkLim;
		getSize = new Vector2 ( getSize.x / 2, getSize.y / 2 );

		getAllSt = stockElem;

		for ( a = 0; a < M.subMeshCount; a++ )
		{
			indices = M.GetTriangles ( a );
			countTriangle = 1;
			checkLim = false;

			while ( indices.Length / countTriangle > 250 )
			{
				countTriangle += 3;
			}

			countTriangle--;

			for ( b = 0; b < indices.Length; b += 3 + countTriangle )
			{
				if ( b % 50 == 0 )
				{
					yield return thisFrame;
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
					newVerts [ c + 3 ] = new Vector3 ( -verts [ index ].x * Random.Range ( 0.8f, 1.5f ), -verts [ index ].y * Random.Range ( 0.8f, 1.5f ), -verts [ index ].z * Random.Range ( 0.8f, 1.5f ) );
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
					0, 2, 3
				};

				if ( getAllSt.Count > 0 && !getAllSt [ 0 ].activeSelf )
				{
					GO = getAllSt [ 0 ];
					GO.SetActive ( true );

					getAllSt.RemoveAt ( 0 );
				}
				else
				{
					GO = ( GameObject ) Instantiate ( getTri );
					GO.transform.SetParent ( garbage );
				}

				GO.GetComponent<MeshRenderer> ( ).material = materials [ a ];
				GO.GetComponent<MeshFilter> ( ).mesh = mesh;
				GO.AddComponent<BoxCollider> ( );

				GO.layer = LayerMask.NameToLayer ( "Particle" );
				GO.transform.position = getTrans.position;
				GO.transform.rotation = getTrans.rotation;

				explosionPos = new Vector3 ( getTrans.position.x + Random.Range ( -getSize.x, getSize.x ), getTrans.position.y + Random.Range ( -getSize.y, getSize.y ), getTrans.position.z + Random.Range ( -getSize.z, getSize.z ) );

				if ( Random.Range ( 0, 5 ) < 2 )
				{
					GO.GetComponent<Rigidbody> ( ).AddExplosionForce ( 10, explosionPos, 0, 0, ForceMode.Impulse );
				}
				else
				{
					GO.GetComponent<Rigidbody> ( ).AddForce ( Vector3.Normalize ( getTrans.position - objSource.transform.position ), ForceMode.Impulse );
				}

				GO.GetComponent<Rigidbody> ( ).mass = 0.0001f;
				GO.GetComponent<TimeToDisable> ( ).DisableThis ( 5 + Random.Range ( 0.0f, 5.0f ) );
			}
		}
			
		Destroy ( objSource );
	}

	public void ReAddObj ( GameObject thisObj )
	{
		stockElem.Add ( thisObj );
	}
}

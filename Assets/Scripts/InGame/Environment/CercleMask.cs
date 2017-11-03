using UnityEngine;

public class CercleMask : MonoBehaviour 
{
	#region Variable
	public float Radius, SoftNess, SmoothSpeed, ScaleFactor;

	Camera mainCam;
	RaycastHit thisHit;
	Ray thisRay;
	Vector3 mousePos, smoothPoint;
	#endregion
	
	#region Mono
	void Start ( ) 
	{
		mainCam = GameObject.FindGameObjectWithTag ( "MainCamera" ).GetComponent<Camera> ( );
	}
	
	void Update ( ) 
	{
		if ( Input.GetKey ( KeyCode.UpArrow ) )
		{
			Radius += ScaleFactor * Time.deltaTime;
		}
		if ( Input.GetKey ( KeyCode.DownArrow ) )
		{
			Radius -= ScaleFactor * Time.deltaTime;
		}
		if ( Input.GetKey ( KeyCode.LeftArrow ) )
		{
			SoftNess += ScaleFactor * Time.deltaTime;
		}
		if ( Input.GetKey ( KeyCode.RightArrow ) )
		{
			SoftNess -= ScaleFactor * Time.deltaTime;
		}

		Mathf.Clamp ( Radius, 0, 100 );
		Mathf.Clamp ( SoftNess, 0, 100 );

		mousePos = new Vector3 ( Input.mousePosition.x, Input.mousePosition.y, 0 );
		thisRay = mainCam.ScreenPointToRay ( mousePos );

		if ( Physics.Raycast ( thisRay, out thisHit ) )
		{
			smoothPoint = Vector3.MoveTowards ( smoothPoint, thisHit.point, SmoothSpeed * Time.deltaTime );
			Vector4 pos = new Vector4 ( smoothPoint.x, smoothPoint.y, smoothPoint.z, 0 );
			Shader.SetGlobalVector ( "GlobaleMask_Position", pos );
		}

		Shader.SetGlobalFloat ( "GlobaleMask_Radius", Radius );
		Shader.SetGlobalFloat ( "GlobaleMask_SoftNess", SoftNess );
	}
	#endregion
		
	#region Public
	#endregion
	
	#region Private
	#endregion
}
